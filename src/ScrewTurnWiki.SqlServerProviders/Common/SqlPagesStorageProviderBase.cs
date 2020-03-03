using Dapper;
using ScrewTurn.Wiki.PluginFramework;
using ScrewTurn.Wiki.SearchEngine;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace ScrewTurn.Wiki.Plugins.SqlCommon
{
    /// <summary>
    ///     Implements a base class for a SQL pages storage provider.
    /// </summary>
    public abstract class SqlPagesStorageProviderBase : SqlStorageProviderBase, IPagesStorageProviderV30
    {
        private const int MaxStatementsInBatch = 20;
        private const int FirstRevision = 0;
        private const int CurrentRevision = -1;
        private const int DraftRevision = -100;

        private readonly ConcurrentDictionary<string, NamespaceInfo> nsCache = new ConcurrentDictionary<string, NamespaceInfo>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<string, PageInfo> pageCache = new ConcurrentDictionary<string, PageInfo>();
        private List<Snippet> snippetsCache;
        private readonly object snippetsCacheLock = new object();

        private bool alwaysGenerateDocument;

        /// <summary>
        ///     Initializes the Storage Provider.
        /// </summary>
        /// <param name="host">The Host of the Component.</param>
        /// <param name="config">The Configuration data, if any.</param>
        /// <remarks>
        ///     If the configuration string is not valid, the methoud should throw a
        ///     <see cref="InvalidConfigurationException" />.
        /// </remarks>
        public new void Init(IHostV30 host, string config)
        {
            base.Init(host, config);

            Index =
                new SqlIndex(new IndexConnector(GetWordFetcher, GetSize, GetCount, ClearIndex, DeleteDataForDocument,
                    SaveDataForDocument, TryFindWord));
        }

        #region IStorageProvider Members

        /// <summary>
        ///     Gets a value specifying whether the provider is read-only, i.e. it can only provide data and not store it.
        /// </summary>
        public bool ReadOnly
        {
            get { return false; }
        }

        #endregion

        #region Index and Search Engine

        /// <summary>
        ///     Sets test flags (to be used only for tests).
        /// </summary>
        /// <param name="alwaysGenerateDocument">
        ///     A value indicating whether to always generate a result when resolving a document,
        ///     even when the page does not exist.
        /// </param>
        public void SetFlags(bool alwaysGenerateDocument)
        {
            this.alwaysGenerateDocument = alwaysGenerateDocument;
        }

        /// <summary>
        ///     Gets a word fetcher.
        /// </summary>
        /// <returns>The word fetcher.</returns>
        private IWordFetcher GetWordFetcher()
        {
            return new SqlWordFetcher(CommandBuilder.GetConnection(connString), TryFindWord);
        }

        /// <summary>
        ///     Gets the search index (only used for testing purposes).
        /// </summary>
        public IIndex Index { get; private set; }

        /// <summary>
        ///     Performs a search in the index.
        /// </summary>
        /// <param name="parameters">The search parameters.</param>
        /// <returns>The results.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="parameters" /> is <c>null</c>.</exception>
        public SearchResultCollection PerformSearch(SearchParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            return Index.Search(parameters);
        }

        /// <summary>
        ///     Rebuilds the search index.
        /// </summary>
        public void RebuildIndex()
        {
            Index.Clear(null);

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var allNamespaces = new List<NamespaceInfo>(GetNamespaces());
            allNamespaces.Add(null);

            var indexedElements = 0;

            foreach (var nspace in allNamespaces)
            {
                foreach (var page in GetPages(transaction, nspace))
                {
                    IndexPage(GetContent(page), transaction);
                    indexedElements++;

                    foreach (var msg in GetMessages(transaction, page))
                    {
                        IndexMessageTree(page, msg, transaction);
                        indexedElements++;
                    }

                    // Every 10 indexed documents, commit the transaction to 
                    // reduce the number of database locks
                    if (indexedElements >= 10)
                    {
                        CommitTransaction(transaction);
                        indexedElements = 0;

                        connection = builder.GetConnection(connString);
                        transaction = BeginTransaction(connection);
                    }
                }
            }

            CommitTransaction(transaction);
        }

        /// <summary>
        ///     Gets a value indicating whether the search engine index is corrupted and needs to be rebuilt.
        /// </summary>
        public bool IsIndexCorrupted
        {
            get { return false; }
        }

        /// <summary>
        ///     Handles the construction of an <see cref="T:IDocument" /> for the search engine.
        /// </summary>
        /// <param name="dumpedDocument">The input dumped document.</param>
        /// <returns>The resulting <see cref="T:IDocument" />.</returns>
        private IDocument BuildDocument(DumpedDocument dumpedDocument)
        {
            if (alwaysGenerateDocument)
            {
                return new DummyDocument
                {
                    ID = dumpedDocument.ID,
                    Name = dumpedDocument.Name,
                    Title = dumpedDocument.Title,
                    TypeTag = dumpedDocument.TypeTag,
                    DateTime = dumpedDocument.DateTime
                };
            }

            if (dumpedDocument.TypeTag == PageDocument.StandardTypeTag)
            {
                var pageName = PageDocument.GetPageName(dumpedDocument.Name);

                var page = GetPage(pageName);

                if (page == null) return null;
                return new PageDocument(page, dumpedDocument, TokenizeContent);
            }
            if (dumpedDocument.TypeTag == MessageDocument.StandardTypeTag)
            {
                string pageFullName;
                int id;
                MessageDocument.GetMessageDetails(dumpedDocument.Name, out pageFullName, out id);

                var page = GetPage(pageFullName);
                if (page == null) return null;
                return new MessageDocument(page, id, dumpedDocument, TokenizeContent);
            }
            return null;
        }

        // Extremely dirty way for testing the search engine in combination with alwaysGenerateDocument
        private class DummyDocument : IDocument
        {
            public uint ID { get; set; }
            public string Name { get; set; }
            public string Title { get; set; }
            public string TypeTag { get; set; }
            public DateTime DateTime { get; set; }

            public WordInfo[] Tokenize(string content)
            {
                return SearchEngine.Tools.Tokenize(content);
            }
        }

        /// <summary>
        ///     Gets some statistics about the search engine index.
        /// </summary>
        /// <param name="documentCount">The total number of documents.</param>
        /// <param name="wordCount">The total number of unique words.</param>
        /// <param name="occurrenceCount">The total number of word-document occurrences.</param>
        /// <param name="size">The approximated size, in bytes, of the search engine index.</param>
        public void GetIndexStats(out int documentCount, out int wordCount, out int occurrenceCount, out long size)
        {
            documentCount = Index.TotalDocuments;
            wordCount = Index.TotalWords;
            occurrenceCount = Index.TotalOccurrences;
            size = GetSize();
        }

        /// <summary>
        ///     Gets the approximate size, in bytes, of the search engine index.
        /// </summary>
        private long GetSize()
        {
            // 1. Size of documents: 8 + 2*20 + 2*30 + 2*1 + 8 = 118 bytes
            // 2. Size of words: 8 + 2*8 = 24 bytes
            // 3. Size of mappings: 8 + 8 + 2 + 2 + 1 = 21 bytes
            // 4. Size = Size * 2

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var queryBuilder = new QueryBuilder(builder);

            long size = 0;

            var query = queryBuilder.SelectCountFrom("IndexDocument");
            var command = builder.GetCommand(connection, query, new List<Parameter>());
            var rows = ExecuteScalar(command, -1, false);

            if (rows == -1) return 0;
            size += rows * 118;

            query = queryBuilder.SelectCountFrom("IndexWord");
            command = builder.GetCommand(connection, query, new List<Parameter>());
            rows = ExecuteScalar(command, -1, false);

            if (rows == -1) return 0;
            size += rows * 24;

            query = queryBuilder.SelectCountFrom("IndexWordMapping");
            command = builder.GetCommand(connection, query, new List<Parameter>());
            rows = ExecuteScalar(command, -1, false);

            if (rows == -1) return 0;
            size += rows * 21;

            CloseConnection(connection);

            return size * 2;
        }

        /// <summary>
        ///     Gets the number of elements in the index.
        /// </summary>
        /// <param name="element">The type of elements.</param>
        /// <returns>The number of elements.</returns>
        private int GetCount(IndexElementType element)
        {
            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var queryBuilder = new QueryBuilder(builder);

            var count = 0;

            var elemName = "";
            if (element == IndexElementType.Documents) elemName = "IndexDocument";
            else if (element == IndexElementType.Words) elemName = "IndexWord";
            else if (element == IndexElementType.Occurrences) elemName = "IndexWordMapping";
            else throw new NotSupportedException("Unsupported element type");

            var query = queryBuilder.SelectCountFrom(elemName);

            var command = builder.GetCommand(connection, query, new List<Parameter>());
            count = ExecuteScalar(command, -1, true);

            return count;
        }

        /// <summary>
        ///     Deletes all data associated to a document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="state">A state object passed from the index (can be <c>null</c> or a <see cref="T:DbTransaction" />).</param>
        private void DeleteDataForDocument(IDocument document, object state)
        {
            // 1. Delete all data related to a document
            // 2. Delete all words that have no more mappings

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("IndexDocument");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "DocName");
            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "DocName", document.Name));

            var subQuery = queryBuilder.SelectFrom("IndexWordMapping", new[] { "Word" });
            subQuery = queryBuilder.GroupBy(subQuery, new[] { "Word" });
            var query2 = queryBuilder.DeleteFrom("IndexWord");
            query2 = queryBuilder.WhereNotInSubquery(query2, "IndexWord", "Id", subQuery);

            query = queryBuilder.AppendForBatch(query, query2);

            IDbCommand command = null;
            if (state != null) command = builder.GetCommand((IDbTransaction)state, query, parameters);
            else command = builder.GetCommand(connString, query, parameters);

            // Close only if state is null
            ExecuteNonQuery(command, state == null);
        }

        /// <summary>
        ///     Saves data for a new document.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="content">The content words.</param>
        /// <param name="title">The title words.</param>
        /// <param name="keywords">The keywords.</param>
        /// <param name="state">A state object passed from the index (can be <c>null</c> or a <see cref="T:DbTransaction" />).</param>
        /// <returns>The number of stored occurrences.</returns>
        private int SaveDataForDocument(IDocument document, WordInfo[] content, WordInfo[] title, WordInfo[] keywords,
            object state)
        {
            // 1. Insert document
            // 2. Insert all new words
            // 3. Load all word IDs
            // 4. Insert mappings

            // On error, return without rolling back if state != null, rollback otherwise
            // On completion, commit if state == null

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            IDbTransaction transaction = null;
            if (state != null) transaction = (IDbTransaction)state;
            else
            {
                var connection = builder.GetConnection(connString);
                transaction = BeginTransaction(connection);
            }

            var freeDocumentId = GetFreeElementId(IndexElementType.Documents, transaction);
            var freeWordId = GetFreeElementId(IndexElementType.Words, transaction);

            // Insert the document
            var query = queryBuilder.InsertInto("IndexDocument",
                new[] { "Id", "Name", "Title", "TypeTag", "DateTime" },
                new[] { "Id", "Name", "Title", "TypeTag", "DateTime" });

            var parameters = new List<Parameter>(5);
            parameters.Add(new Parameter(ParameterType.Int32, "Id", (int)freeDocumentId));
            parameters.Add(new Parameter(ParameterType.String, "Name", document.Name));
            parameters.Add(new Parameter(ParameterType.String, "Title", document.Title));
            parameters.Add(new Parameter(ParameterType.String, "TypeTag", document.TypeTag));
            parameters.Add(new Parameter(ParameterType.DateTime, "DateTime", document.DateTime));

            var command = builder.GetCommand(transaction, query, parameters);

            if (ExecuteNonQuery(command, false) != 1)
            {
                if (state == null) RollbackTransaction(transaction);
                return -1;
            }
            document.ID = freeDocumentId;

            var allWords = new List<WordInfo>(content.Length + title.Length + keywords.Length);
            allWords.AddRange(content);
            allWords.AddRange(title);
            allWords.AddRange(keywords);

            var existingWords = new List<WordInfo>(allWords.Count / 2);

            var wordIds = new Dictionary<string, uint>(1024);

            // Try to blindly insert all words (assumed to be lowercase and clean from diacritics)

            query = queryBuilder.InsertInto("IndexWord", new[] { "Id", "Text" }, new[] { "Id", "Text" });

            parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.Int32, "Id", 0));
            parameters.Add(new Parameter(ParameterType.String, "Text", ""));

            foreach (var word in allWords)
            {
                parameters[0].Value = (int)freeWordId;
                parameters[1].Value = word.Text;

                command = builder.GetCommand(transaction, query, parameters);

                if (ExecuteNonQuery(command, false, false) == 1)
                {
                    wordIds.Add(word.Text, freeWordId);
                    freeWordId++;
                }
                else
                {
                    existingWords.Add(word);
                }
            }

            // Load IDs of all existing words
            query = queryBuilder.SelectFrom("IndexWord", new[] { "Id" });
            query = queryBuilder.Where(query, "Text", WhereOperator.Equals, "Text");

            parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Text", ""));

            foreach (var word in existingWords)
            {
                parameters[0].Value = word.Text;

                command = builder.GetCommand(transaction, query, parameters);

                var id = ExecuteScalar(command, -1, false);
                if (id == -1)
                {
                    if (state == null) RollbackTransaction(transaction);
                    return -1;
                }

                if (!wordIds.ContainsKey(word.Text))
                {
                    wordIds.Add(word.Text, (uint)id);
                }
                else if (wordIds[word.Text] != (uint)id) throw new InvalidOperationException("Word ID mismatch");
            }

            // Insert all mappings
            query = queryBuilder.InsertInto("IndexWordMapping",
                new[] { "Word", "Document", "FirstCharIndex", "WordIndex", "Location" },
                new[] { "Word", "Document", "FirstCharIndex", "WordIndex", "Location" });

            parameters = new List<Parameter>(5);
            parameters.Add(new Parameter(ParameterType.Int32, "Word", 0));
            parameters.Add(new Parameter(ParameterType.Int32, "Document", (int)freeDocumentId));
            parameters.Add(new Parameter(ParameterType.Int16, "FirstCharIndex", 0));
            parameters.Add(new Parameter(ParameterType.Int16, "WordIndex", 0));
            parameters.Add(new Parameter(ParameterType.Byte, "Location", 0));

            foreach (var word in allWords)
            {
                parameters[0].Value = (int)wordIds[word.Text];
                parameters[1].Value = (int)freeDocumentId;
                parameters[2].Value = (short)word.FirstCharIndex;
                parameters[3].Value = (short)word.WordIndex;
                parameters[4].Value = word.Location.Location;

                command = builder.GetCommand(transaction, query, parameters);

                if (ExecuteNonQuery(command, false) != 1)
                {
                    if (state == null) RollbackTransaction(transaction);
                    return -1;
                }
            }

            if (state == null) CommitTransaction(transaction);

            return allWords.Count;
        }

        /// <summary>
        ///     Gets a free element ID from the database.
        /// </summary>
        /// <param name="element">The element type.</param>
        /// <param name="transaction">The current database transaction.</param>
        /// <returns>The free element ID.</returns>
        private uint GetFreeElementId(IndexElementType element, IDbTransaction transaction)
        {
            if (element == IndexElementType.Occurrences)
                throw new ArgumentException("Element cannot be Occurrences", nameof(element));

            var table = element == IndexElementType.Documents ? "IndexDocument" : "IndexWord";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom(table, new[] { "Id" });
            query = queryBuilder.OrderBy(query, new[] { "Id" }, new[] { Ordering.Desc });

            var command = builder.GetCommand(transaction, query, new List<Parameter>());

            var id = ExecuteScalar(command, -1, false);

            if (id == -1) return 0;
            return (uint)id + 1;
        }

        /// <summary>
        ///     Tries to load all data related to a word from the database.
        /// </summary>
        /// <param name="text">The word text.</param>
        /// <param name="word">The returned word.</param>
        /// <param name="connection">An open database connection.</param>
        /// <returns><c>true</c> if the word is found, <c>false</c> otherwise.</returns>
        private bool TryFindWord(string text, out Word word, IDbConnection connection)
        {
            // 1. Find word - if not found, return
            // 2. Read all raw word mappings
            // 3. Read all documents (unique)
            // 4. Build result data structure

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("IndexWord", new[] { "Id" });
            query = queryBuilder.Where(query, "Text", WhereOperator.Equals, "Text");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Text", text));

            var command = builder.GetCommand(connection, query, parameters);

            var wordId = ExecuteScalar(command, -1, false);

            if (wordId == -1)
            {
                word = null;
                return false;
            }

            // Read all raw mappings
            query = queryBuilder.SelectFrom("IndexWordMapping");
            query = queryBuilder.Where(query, "Word", WhereOperator.Equals, "WordId");

            parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.Int32, "WordId", wordId));

            command = builder.GetCommand(connection, query, parameters);

            var reader = ExecuteReader(command, false);

            var mappings = new List<DumpedWordMapping>(2048);
            while (reader != null && reader.Read())
            {
                mappings.Add(new DumpedWordMapping((uint)wordId,
                    (uint)(int)reader["Document"],
                    (ushort)(short)reader["FirstCharIndex"], (ushort)(short)reader["WordIndex"],
                    (byte)reader["Location"]));
            }
            CloseReader(reader);

            if (mappings.Count == 0)
            {
                word = null;
                return false;
            }

            // Find all documents
            query = queryBuilder.SelectFrom("IndexDocument");
            query = queryBuilder.Where(query, "Id", WhereOperator.Equals, "DocId");

            parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.Int32, "DocId", 0));

            var documents = new Dictionary<uint, IDocument>(64);
            foreach (var map in mappings)
            {
                var docId = map.DocumentID;
                if (documents.ContainsKey(docId)) continue;

                parameters[0].Value = (int)docId;
                command = builder.GetCommand(connection, query, parameters);

                reader = ExecuteReader(command, false);

                if (reader != null && reader.Read())
                {
                    var dumpedDoc = new DumpedDocument(docId,
                        reader["Name"] as string, reader["Title"] as string,
                        reader["TypeTag"] as string,
                        (DateTime)reader["DateTime"]);

                    var document = BuildDocument(dumpedDoc);

                    if (document != null) documents.Add(docId, document);
                }
                CloseReader(reader);
            }

            var occurrences = new OccurrenceDictionary(mappings.Count);
            foreach (var map in mappings)
            {
                if (!documents.ContainsKey(map.DocumentID))
                {
                    continue;
                }

                if (!occurrences.ContainsKey(documents[map.DocumentID]))
                {
                    occurrences.Add(documents[map.DocumentID], new SortedBasicWordInfoSet(2));
                }

                occurrences[documents[map.DocumentID]].Add(new BasicWordInfo(
                    map.FirstCharIndex, map.WordIndex, WordLocation.GetInstance(map.Location)));
            }

            word = new Word((uint)wordId, text, occurrences);
            return true;
        }

        /// <summary>
        ///     Clears the index.
        /// </summary>
        /// <param name="state">A state object passed from the index.</param>
        private void ClearIndex(object state)
        {
            // state can be null, depending on when the method is called
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("IndexWordMapping");
            query = queryBuilder.AppendForBatch(query, queryBuilder.DeleteFrom("IndexWord"));
            query = queryBuilder.AppendForBatch(query, queryBuilder.DeleteFrom("IndexDocument"));

            IDbCommand command = null;
            if (state == null) command = builder.GetCommand(connString, query, new List<Parameter>());
            else command = builder.GetCommand((IDbTransaction)state, query, new List<Parameter>());

            ExecuteNonQuery(command, state == null);
        }

        /// <summary>
        ///     Tokenizes page content.
        /// </summary>
        /// <param name="content">The content to tokenize.</param>
        /// <returns>The tokenized words.</returns>
        private static WordInfo[] TokenizeContent(string content)
        {
            var words = SearchEngine.Tools.Tokenize(content);
            return words;
        }

        /// <summary>
        ///     Indexes a page.
        /// </summary>
        /// <param name="content">The page content.</param>
        /// <param name="transaction">The current transaction.</param>
        /// <returns>The number of indexed words, including duplicates.</returns>
        private int IndexPage(PageContent content, IDbTransaction transaction)
        {
            try
            {
                if (string.IsNullOrEmpty(content.Title) || string.IsNullOrEmpty(content.Content)) return 0;

                var documentName = PageDocument.GetDocumentName(content.PageInfo);

                var ddoc = new DumpedDocument(0, documentName,
                    host.PrepareTitleForIndexing(content.PageInfo, content.Title),
                    PageDocument.StandardTypeTag, content.LastModified);

                // Store the document
                // The content should always be prepared using IHost.PrepareForSearchEngineIndexing()
                var count = Index.StoreDocument(new PageDocument(content.PageInfo, ddoc, TokenizeContent),
                    content.Keywords, host.PrepareContentForIndexing(content.PageInfo, content.Content), transaction);

                if (count == 0 && content.Content.Length > 0)
                {
                    host.LogEntry(
                        "Indexed 0 words for page " + content.PageInfo.FullName +
                        ": possible index corruption. Please report this error to the developers",
                        LogEntryType.Warning, null, this);
                }

                return count;
            }
            catch (Exception ex)
            {
                host.LogEntry("Page indexing error for " + content.PageInfo.FullName + " (skipping page): " + ex,
                    LogEntryType.Error, null, this);
                return 0;
            }
        }

        /// <summary>
        ///     Removes a page from the search engine index.
        /// </summary>
        /// <param name="content">The content of the page to remove.</param>
        /// <param name="transaction">The current transaction.</param>
        private void UnindexPage(PageContent content, IDbTransaction transaction)
        {
            var documentName = PageDocument.GetDocumentName(content.PageInfo);

            var ddoc = new DumpedDocument(0, documentName, host.PrepareTitleForIndexing(content.PageInfo, content.Title),
                PageDocument.StandardTypeTag, content.LastModified);
            Index.RemoveDocument(new PageDocument(content.PageInfo, ddoc, TokenizeContent), transaction);
        }

        /// <summary>
        ///     Indexes a message tree.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="root">The root message.</param>
        /// <param name="transaction">The current transaction.</param>
        private void IndexMessageTree(PageInfo page, Message root, IDbTransaction transaction)
        {
            IndexMessage(page, root.ID, root.Subject, root.DateTime, root.Body, transaction);
            foreach (var reply in root.Replies)
            {
                IndexMessageTree(page, reply, transaction);
            }
        }

        /// <summary>
        ///     Indexes a message.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="id">The message ID.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="dateTime">The date/time.</param>
        /// <param name="body">The body.</param>
        /// <param name="transaction">The current transaction.</param>
        /// <returns>The number of indexed words, including duplicates.</returns>
        private int IndexMessage(PageInfo page, int id, string subject, DateTime dateTime, string body,
            IDbTransaction transaction)
        {
            try
            {
                if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body)) return 0;

                // Trim "RE:" to avoid polluting the search engine index
                if (subject.ToLowerInvariant().StartsWith("re:") && subject.Length > 3)
                    subject = subject.Substring(3).Trim();

                var documentName = MessageDocument.GetDocumentName(page, id);

                var ddoc = new DumpedDocument(0, documentName, host.PrepareTitleForIndexing(null, subject),
                    MessageDocument.StandardTypeTag, dateTime);

                // Store the document
                // The content should always be prepared using IHost.PrepareForSearchEngineIndexing()
                var count = Index.StoreDocument(new MessageDocument(page, id, ddoc, TokenizeContent), null,
                    host.PrepareContentForIndexing(null, body), transaction);

                if (count == 0 && body.Length > 0)
                {
                    host.LogEntry(
                        "Indexed 0 words for message " + page.FullName + ":" + id +
                        ": possible index corruption. Please report this error to the developers",
                        LogEntryType.Warning, null, this);
                }

                return count;
            }
            catch (Exception ex)
            {
                host.LogEntry("Message indexing error for " + page.FullName + ":" + id + " (skipping message): " + ex,
                    LogEntryType.Error, null, this);
                return 0;
            }
        }

        /// <summary>
        ///     Removes a message tree from the search engine index.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="root">The tree root.</param>
        /// <param name="transaction">The current transaction.</param>
        private void UnindexMessageTree(PageInfo page, Message root, IDbTransaction transaction)
        {
            UnindexMessage(page, root.ID, root.Subject, root.DateTime, root.Body, transaction);
            foreach (var reply in root.Replies)
            {
                UnindexMessageTree(page, reply, transaction);
            }
        }

        /// <summary>
        ///     Removes a message from the search engine index.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="id">The message ID.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="dateTime">The date/time.</param>
        /// <param name="body">The body.</param>
        /// <param name="transaction">The current transaction.</param>
        /// <returns>The number of indexed words, including duplicates.</returns>
        private void UnindexMessage(PageInfo page, int id, string subject, DateTime dateTime, string body,
            IDbTransaction transaction)
        {
            // Trim "RE:" to avoid polluting the search engine index
            if (subject.ToLowerInvariant().StartsWith("re:") && subject.Length > 3)
                subject = subject.Substring(3).Trim();

            var documentName = MessageDocument.GetDocumentName(page, id);

            var ddoc = new DumpedDocument(0, documentName, host.PrepareTitleForIndexing(null, subject),
                MessageDocument.StandardTypeTag, DateTime.Now);
            Index.RemoveDocument(new MessageDocument(page, id, ddoc, TokenizeContent), transaction);
        }

        #endregion

        #region IPagesStorageProvider Members

        /// <summary>
        ///     Gets a namespace.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="name">The name of the namespace (cannot be <c>null</c> or empty).</param>
        /// <returns>The <see cref="T:NamespaceInfo" />, or <c>null</c> if no namespace is found.</returns>
        private NamespaceInfo GetNamespace(IDbTransaction transaction, string name)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            // select ... from Namespace left join Page on Namespace.DefaultPage = Page.Name where Namespace.Name = <name> and (Namespace.DefaultPage is null or Page.Namespace = <name>)
            var query = queryBuilder.SelectFrom("Namespace", "Page", "DefaultPage", "Name", Join.LeftJoin,
                new[] { "Name", "DefaultPage" }, new[] { "CreationDateTime" });
            query = queryBuilder.Where(query, "Namespace", "Name", WhereOperator.Equals, "Name1");
            query = queryBuilder.AndWhere(query, "Namespace", "DefaultPage", WhereOperator.IsNull, null, true, false);
            query = queryBuilder.OrWhere(query, "Page", "Namespace", WhereOperator.Equals, "Name2", false, true);

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name1", name));
            parameters.Add(new Parameter(ParameterType.String, "Name2", name));

            var command = builder.GetCommand(transaction, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                NamespaceInfo result = null;

                if (reader.Read())
                {
                    var realName = reader["Namespace_Name"] as string;
                    var page = GetNullableColumn<string>(reader, "Namespace_DefaultPage", null);
                    var defaultPage = string.IsNullOrEmpty(page)
                        ? null
                        : new PageInfo(NameTools.GetFullName(realName, page), this,
                            (DateTime)reader["Page_CreationDateTime"]);

                    result = new NamespaceInfo(realName, this, defaultPage);
                }

                CloseReader(reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets a namespace.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="name">The name of the namespace (cannot be <c>null</c> or empty).</param>
        /// <returns>The <see cref="T:NamespaceInfo" />, or <c>null</c> if no namespace is found.</returns>
        private NamespaceInfo GetNamespace(IDbConnection connection, string name)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            // select ... from Namespace left join Page on Namespace.DefaultPage = Page.Name where Namespace.Name = <name> and (Namespace.DefaultPage is null or Page.Namespace = <name>)
            var query = queryBuilder.SelectFrom("Namespace", "Page", "DefaultPage", "Name", Join.LeftJoin,
                new[] { "Name", "DefaultPage" }, new[] { "CreationDateTime" });
            query = queryBuilder.Where(query, "Namespace", "Name", WhereOperator.Equals, "Name1");
            query = queryBuilder.AndWhere(query, "Namespace", "DefaultPage", WhereOperator.IsNull, null, true, false);
            query = queryBuilder.OrWhere(query, "Page", "Namespace", WhereOperator.Equals, "Name2", false, true);

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name1", name));
            parameters.Add(new Parameter(ParameterType.String, "Name2", name));

            var command = builder.GetCommand(connection, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                NamespaceInfo result = null;

                if (reader.Read())
                {
                    var realName = reader["Namespace_Name"] as string;
                    var page = GetNullableColumn<string>(reader, "Namespace_DefaultPage", null);
                    var defaultPage = string.IsNullOrEmpty(page)
                        ? null
                        : new PageInfo(realName, page, this, (DateTime)reader["Page_CreationDateTime"]);

                    result = new NamespaceInfo(realName, this, defaultPage);
                }

                CloseReader(reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets a namespace.
        /// </summary>
        /// <param name="name">The name of the namespace (cannot be <c>null</c> or empty).</param>
        /// <returns>The <see cref="T:NamespaceInfo" />, or <c>null</c> if no namespace is found.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name" /> is empty.</exception>
        public NamespaceInfo GetNamespace(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty");

            return nsCache.GetOrAdd(name, n =>
            {
                var builder = CommandBuilder;
                var connection = builder.GetConnection(connString);

                var nspace = GetNamespace(connection, n);
                CloseConnection(connection);

                return nspace;
            });
        }

        /// <summary>
        ///     Gets all the sub-namespaces.
        /// </summary>
        /// <returns>The sub-namespaces, sorted by name.</returns>
        public IList<NamespaceInfo> GetNamespaces()
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            // select ... from Namespace left join Page on Namespace.DefaultPage = Page.Name where Namespace.Name <> '' and (Namespace.DefaultPage is null or Page.Namespace <> '')
            var query = queryBuilder.SelectFrom("Namespace", "Page", "DefaultPage", "Name", Join.LeftJoin,
                new[] { "Name", "DefaultPage" }, new[] { "CreationDateTime" });
            query = queryBuilder.Where(query, "Namespace", "Name", WhereOperator.NotEquals, "Empty1");
            query = queryBuilder.AndWhere(query, "Namespace", "DefaultPage", WhereOperator.IsNull, null, true, false);
            query = queryBuilder.OrWhere(query, "Page", "Namespace", WhereOperator.NotEquals, "Empty2", false, true);
            query = queryBuilder.OrderBy(query, new[] { "Namespace_Name" }, new[] { Ordering.Asc });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Empty1", ""));
            parameters.Add(new Parameter(ParameterType.String, "Empty2", ""));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<NamespaceInfo>(10);

                while (reader.Read())
                {
                    var realName = reader["Namespace_Name"] as string;
                    var page = GetNullableColumn<string>(reader, "Namespace_DefaultPage", null);
                    var defaultPage = string.IsNullOrEmpty(page)
                        ? null
                        : new PageInfo(NameTools.GetFullName(realName, page), this,
                            (DateTime)reader["Page_CreationDateTime"]);

                    // The query returns duplicate entries if the main page of two or more namespaces have the same name
                    if (result.Find(n => { return n.Name.Equals(realName); }) == null)
                    {
                        result.Add(new NamespaceInfo(realName, this, defaultPage));
                    }
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Adds a new namespace.
        /// </summary>
        /// <param name="name">The name of the namespace.</param>
        /// <returns>The correct <see cref="T:NamespaceInfo" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name" /> is empty.</exception>
        public NamespaceInfo AddNamespace(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", nameof(name));

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.InsertInto("Namespace", new[] { "Name" }, new[] { "Name" });

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));

            var command = builder.GetCommand(connString, query, parameters);

            var rows = ExecuteNonQuery(command);

            NamespaceInfo removed;
            nsCache.TryRemove(name, out removed);

            if (rows == 1) return new NamespaceInfo(name, this, null);
            return null;
        }

        /// <summary>
        ///     Renames a namespace.
        /// </summary>
        /// <param name="nspace">The namespace to rename.</param>
        /// <param name="newName">The new name of the namespace.</param>
        /// <returns>The correct <see cref="T:NamespaceInfo" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="nspace" /> or <paramref name="newName" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="newName" /> is empty.</exception>
        public NamespaceInfo RenameNamespace(NamespaceInfo nspace, string newName)
        {
            if (nspace == null) throw new ArgumentNullException(nameof(nspace));
            if (newName == null) throw new ArgumentNullException(nameof(newName));
            if (newName.Length == 0) throw new ArgumentException("New Name cannot be empty", nameof(newName));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (GetNamespace(transaction, nspace.Name) == null)
            {
                RollbackTransaction(transaction);
                return null;
            }

            foreach (var page in GetPages(transaction, nspace))
            {
                var content = GetContent(transaction, page, CurrentRevision);
                if (content != null)
                {
                    UnindexPage(content, transaction);
                }
                var messages = GetMessages(transaction, page);
                if (messages != null)
                {
                    foreach (var msg in messages)
                    {
                        UnindexMessageTree(page, msg, transaction);
                    }
                }
            }

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("Namespace", new[] { "Name" }, new[] { "NewName" });
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "OldName");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "NewName", newName));
            parameters.Add(new Parameter(ParameterType.String, "OldName", nspace.Name));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows > 0)
            {
                var result = GetNamespace(transaction, newName);

                foreach (var page in GetPages(transaction, result))
                {
                    var content = GetContent(transaction, page, CurrentRevision);
                    if (content != null)
                    {
                        IndexPage(content, transaction);
                    }
                    var messages = GetMessages(transaction, page);
                    if (messages != null)
                    {
                        foreach (var msg in messages)
                        {
                            IndexMessageTree(page, msg, transaction);
                        }
                    }
                }

                CommitTransaction(transaction);

                return result;
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Sets the default page of a namespace.
        /// </summary>
        /// <param name="nspace">The namespace of which to set the default page.</param>
        /// <param name="page">The page to use as default page, or <c>null</c>.</param>
        /// <returns>The correct <see cref="T:NamespaceInfo" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="nspace" /> is <c>null</c>.</exception>
        public NamespaceInfo SetNamespaceDefaultPage(NamespaceInfo nspace, PageInfo page)
        {
            if (nspace == null) throw new ArgumentNullException(nameof(nspace));

            // Namespace existence is verified by the affected rows (should be 1)

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (page != null && GetPage(transaction, page.FullName) == null)
            {
                RollbackTransaction(transaction);
                return null;
            }

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("Namespace", new[] { "DefaultPage" }, new[] { "DefaultPage" });
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(2);
            if (page == null) parameters.Add(new Parameter(ParameterType.String, "DefaultPage", DBNull.Value));
            else
                parameters.Add(new Parameter(ParameterType.String, "DefaultPage", NameTools.GetLocalName(page.FullName)));
            parameters.Add(new Parameter(ParameterType.String, "Name", nspace.Name));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1)
            {
                CommitTransaction(transaction);
                return new NamespaceInfo(nspace.Name, this, page);
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Removes a namespace.
        /// </summary>
        /// <param name="nspace">The namespace to remove.</param>
        /// <returns><c>true</c> if the namespace is removed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="nspace" /> is <c>null</c>.</exception>
        public bool RemoveNamespace(NamespaceInfo nspace)
        {
            if (nspace == null) throw new ArgumentNullException(nameof(nspace));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            foreach (var page in GetPages(transaction, nspace))
            {
                var content = GetContent(transaction, page, CurrentRevision);
                UnindexPage(content, transaction);
                foreach (var msg in GetMessages(transaction, page))
                {
                    UnindexMessageTree(page, msg, transaction);
                }
            }

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("Namespace");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", nspace.Name));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);
            if (rows > 0) CommitTransaction(transaction);
            else RollbackTransaction(transaction);

            NamespaceInfo removed;
            nsCache.TryRemove(nspace.Name, out removed);

            return rows > 0;
        }

        /// <summary>
        ///     Determines whether a page is the default page of its namespace.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="page">The page.</param>
        /// <returns><c>true</c> if the page is the default page, <c>false</c> otherwise.</returns>
        private bool IsDefaultPage(IDbTransaction transaction, PageInfo page)
        {
            var nspaceName = NameTools.GetNamespace(page.FullName);
            if (string.IsNullOrEmpty(nspaceName)) return false;

            var nspace = GetNamespace(transaction, nspaceName);
            if (nspace == null) return false;
            if (nspace.DefaultPage != null) return new PageNameComparer().Compare(nspace.DefaultPage, page) == 0;
            return false;
        }

        /// <summary>
        ///     Moves a page from its namespace into another.
        /// </summary>
        /// <param name="page">The page to move.</param>
        /// <param name="destination">The destination namespace (<c>null</c> for the root).</param>
        /// <param name="copyCategories">
        ///     A value indicating whether to copy the page categories in the destination
        ///     namespace, if not already available.
        /// </param>
        /// <returns>The correct instance of <see cref="T:PageInfo" />.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public PageInfo MovePage(PageInfo page, NamespaceInfo destination, bool copyCategories)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            // Check:
            // 1. Same namespace - ROOT, SUB (explicit check)
            // 2. Destination existence (update query affects 0 rows because it would break a FK)
            // 3. Page existence in target (update query affects 0 rows because it would break a FK)
            // 4. Page is default page of its namespace (explicit check)

            var destinationName = destination?.Name ?? string.Empty;
            string sourceNamespace = page.Namespace ?? string.Empty;

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (destinationName.ToLowerInvariant() == sourceNamespace.ToLowerInvariant()) return null;
            if (IsDefaultPage(transaction, page))
            {
                RollbackTransaction(transaction);
                return null;
            }

            var currentContent = GetContent(transaction, page, CurrentRevision);
            if (currentContent != null)
            {
                UnindexPage(currentContent, transaction);
                foreach (var msg in GetMessages(transaction, page))
                {
                    UnindexMessageTree(page, msg, transaction);
                }
            }

            var currCategories = GetCategories(transaction, sourceNamespace == "" ? null : GetNamespace(transaction, sourceNamespace));

            // Remove bindings
            RebindPage(transaction, page, new string[0]);

            var newCategories = new string[0];

            if (copyCategories)
            {
                // Retrieve categories for page
                // Copy missing ones in destination

                var lowerPageName = page.FullName.ToLowerInvariant();

                var pageCategories = new List<string>(10);
                foreach (var cat in currCategories)
                {
                    if (cat.Pages.Any(s => s.ToLowerInvariant() == lowerPageName))
                    {
                        pageCategories.Add(NameTools.GetLocalName(cat.FullName));
                    }
                }

                // Create categories into destination without checking existence (AddCategory will return null)
                var tempName = destinationName == "" ? null : destinationName;
                newCategories = new string[pageCategories.Count];

                for (var i = 0; i < pageCategories.Count; i++)
                {
                    var catName = NameTools.GetFullName(tempName, pageCategories[i]);
                    if (GetCategory(transaction, catName) == null)
                    {
                        var added = AddCategory(tempName, pageCategories[i]);
                    }
                    newCategories[i] = catName;
                }
            }

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("Page", new[] { "Namespace" }, new[] { "Destination" });
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Source");

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Destination", destinationName));
            parameters.Add(new Parameter(ParameterType.String, "Name", page.Name));
            parameters.Add(new Parameter(ParameterType.String, "Source", sourceNamespace));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows > 0)
            {
                var result = new PageInfo(destinationName, page.Name, this, page.CreationDateTime);

                // Re-bind categories
                if (copyCategories)
                {
                    var rebound = RebindPage(transaction, result, newCategories);
                    if (!rebound)
                    {
                        RollbackTransaction(transaction);
                        return null;
                    }
                }

                var newContent = GetContent(transaction, result, CurrentRevision);
                IndexPage(newContent, transaction);
                foreach (var msg in GetMessages(transaction, result))
                {
                    IndexMessageTree(result, msg, transaction);
                }

                CommitTransaction(transaction);

                return result;
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Gets a category.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="fullName">The full name of the category.</param>
        /// <returns>The <see cref="T:CategoryInfo" />, or <c>null</c> if no category is found.</returns>
        private CategoryInfo GetCategory(IDbTransaction transaction, string fullName)
        {
            string nspace = null;
            string name = null;
            NameTools.ExpandFullName(fullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("Category", "CategoryBinding", new[] { "Name", "Namespace" },
                new[] { "Category", "Namespace" }, Join.LeftJoin,
                new[] { "Name", "Namespace" }, new[] { "Page" });
            query = queryBuilder.Where(query, "Category", "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "Category", "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.String, "Name", name));

            var command = builder.GetCommand(transaction, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                CategoryInfo result = null;
                var pages = new List<string>(50);

                while (reader.Read())
                {
                    if (result == null)
                        result =
                            new CategoryInfo(
                                NameTools.GetFullName(reader["Category_Namespace"] as string,
                                    reader["Category_Name"] as string), this);

                    if (!IsDBNull(reader, "CategoryBinding_Page"))
                    {
                        pages.Add(NameTools.GetFullName(reader["Category_Namespace"] as string,
                            reader["CategoryBinding_Page"] as string));
                    }
                }

                CloseReader(reader);

                if (result != null) result.Pages = pages;

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets a category.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="fullName">The full name of the category.</param>
        /// <returns>The <see cref="T:CategoryInfo" />, or <c>null</c> if no category is found.</returns>
        private CategoryInfo GetCategory(DbConnection connection, string fullName)
        {
            string nspace = null;
            string name = null;
            NameTools.ExpandFullName(fullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("Category", "CategoryBinding", new[] { "Name", "Namespace" },
                new[] { "Category", "Namespace" }, Join.LeftJoin,
                new[] { "Name", "Namespace" }, new[] { "Page" });
            query = queryBuilder.Where(query, "Category", "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "Category", "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.String, "Name", name));

            var command = builder.GetCommand(connection, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                CategoryInfo result = null;
                var pages = new List<string>(50);

                while (reader.Read())
                {
                    if (result == null)
                        result =
                            new CategoryInfo(
                                NameTools.GetFullName(reader["Category_Namespace"] as string,
                                    reader["Category_Name"] as string), this);

                    if (!IsDBNull(reader, "CategoryBinding_Page"))
                    {
                        pages.Add(NameTools.GetFullName(reader["Category_Namespace"] as string,
                            reader["CategoryBinding_Page"] as string));
                    }
                }

                CloseReader(reader);

                if (result != null) result.Pages = pages;

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets a category.
        /// </summary>
        /// <param name="fullName">The full name of the category.</param>
        /// <returns>The <see cref="T:CategoryInfo" />, or <c>null</c> if no category is found.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="fullName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="fullName" /> is empty.</exception>
        public CategoryInfo GetCategory(string fullName)
        {
            if (fullName == null) throw new ArgumentNullException(nameof(fullName));
            if (fullName.Length == 0) throw new ArgumentException("Full Name cannot be empty", nameof(fullName));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var category = GetCategory(connection, fullName);
            CloseConnection(connection);

            return category;
        }

        /// <summary>
        ///     Gets all the Categories in a namespace.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="nspace">The namespace.</param>
        /// <returns>All the Categories in the namespace. The array is not sorted.</returns>
        private IList<CategoryInfo> GetCategories(IDbTransaction transaction, NamespaceInfo nspace)
        {
            var nspaceName = nspace != null ? nspace.Name : "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("Category", "CategoryBinding", new[] { "Name", "Namespace" },
                new[] { "Category", "Namespace" }, Join.LeftJoin,
                new[] { "Name", "Namespace" }, new[] { "Page" });
            query = queryBuilder.Where(query, "Category", "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.OrderBy(query, new[] { "Category_Name", "CategoryBinding_Page" },
                new[] { Ordering.Asc, Ordering.Asc });

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspaceName));

            var command = builder.GetCommand(transaction, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<CategoryInfo>(20);
                var pages = new List<string>(50);

                var prevName = "|||";
                string name = null;

                while (reader.Read())
                {
                    name = reader["Category_Name"] as string;

                    if (name != prevName)
                    {
                        if (prevName != "|||")
                        {
                            result[result.Count - 1].Pages = pages.ToList();
                            pages.Clear();
                        }

                        result.Add(new CategoryInfo(
                            NameTools.GetFullName(reader["Category_Namespace"] as string, name), this));
                    }

                    prevName = name;
                    if (!IsDBNull(reader, "CategoryBinding_Page"))
                    {
                        pages.Add(NameTools.GetFullName(reader["Category_Namespace"] as string,
                            reader["CategoryBinding_Page"] as string));
                    }
                }

                CloseReader(reader);

                if (result.Count > 0) result[result.Count - 1].Pages = pages.ToList();

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets all the Categories in a namespace.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="nspace">The namespace.</param>
        /// <returns>All the Categories in the namespace. The array is not sorted.</returns>
        private IList<CategoryInfo> GetCategories(IDbConnection connection, NamespaceInfo nspace)
        {
            var nspaceName = nspace != null ? nspace.Name : "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("Category", "CategoryBinding", new[] { "Name", "Namespace" },
                new[] { "Category", "Namespace" }, Join.LeftJoin,
                new[] { "Name", "Namespace" }, new[] { "Page" });
            query = queryBuilder.Where(query, "Category", "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.OrderBy(query, new[] { "Category_Name", "CategoryBinding_Page" },
                new[] { Ordering.Asc, Ordering.Asc });

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspaceName));

            var command = builder.GetCommand(connection, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<CategoryInfo>(20);
                var pages = new List<string>(50);

                var prevName = "|||";
                string name = null;

                while (reader.Read())
                {
                    name = reader["Category_Name"] as string;

                    if (name != prevName)
                    {
                        if (prevName != "|||")
                        {
                            result[result.Count - 1].Pages = pages.ToList();
                            pages.Clear();
                        }

                        result.Add(new CategoryInfo(
                            NameTools.GetFullName(reader["Category_Namespace"] as string, name), this));
                    }

                    prevName = name;
                    if (!IsDBNull(reader, "CategoryBinding_Page"))
                    {
                        pages.Add(NameTools.GetFullName(reader["Category_Namespace"] as string,
                            reader["CategoryBinding_Page"] as string));
                    }
                }

                CloseReader(reader);

                if (result.Count > 0) result[result.Count - 1].Pages = pages.ToList();

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets all the Categories in a namespace.
        /// </summary>
        /// <param name="nspace">The namespace.</param>
        /// <returns>All the Categories in the namespace, sorted by name.</returns>
        public IList<CategoryInfo> GetCategories(NamespaceInfo nspace)
        {
            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var categories = GetCategories(connection, nspace);
            CloseConnection(connection);

            return categories;
        }

        /// <summary>
        ///     Gets all the categories of a page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>The categories, sorted by name.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public IList<CategoryInfo> GetCategoriesForPage(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            string nspace, pageName;
            NameTools.ExpandFullName(page.FullName, out nspace, out pageName);
            if (nspace == null) nspace = "";

            var query = queryBuilder.SelectFrom("Category", "CategoryBinding", new[] { "Name", "Namespace" },
                new[] { "Category", "Namespace" }, Join.LeftJoin,
                new[] { "Name", "Namespace" }, new[] { "Page" });
            query = queryBuilder.Where(query, "CategoryBinding", "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "CategoryBinding", "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.OrderBy(query, new[] { "Category_Name" }, new[] { Ordering.Asc });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.String, "Page", pageName));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<CategoryInfo>(20);
                var pages = new List<string>(50);

                var prevName = "|||";
                string name = null;

                while (reader.Read())
                {
                    name = reader["Category_Name"] as string;

                    if (name != prevName)
                    {
                        if (prevName != "|||")
                        {
                            result[result.Count - 1].Pages = pages.ToArray();
                            pages.Clear();
                        }

                        result.Add(new CategoryInfo(
                            NameTools.GetFullName(reader["Category_Namespace"] as string, name), this));
                    }

                    prevName = name;
                    if (!IsDBNull(reader, "CategoryBinding_Page"))
                    {
                        pages.Add(NameTools.GetFullName(reader["Category_Namespace"] as string,
                            reader["CategoryBinding_Page"] as string));
                    }
                }

                CloseReader(command, reader);

                if (result.Count > 0) result[result.Count - 1].Pages = pages.ToArray();

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Adds a Category.
        /// </summary>
        /// <param name="nspace">The target namespace (<c>null</c> for the root).</param>
        /// <param name="name">The Category name.</param>
        /// <returns>The correct CategoryInfo object.</returns>
        /// <remarks>The method should set category's Pages to an empty array.</remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name" /> is empty.</exception>
        public CategoryInfo AddCategory(string nspace, string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", nameof(name));

            if (nspace == null) nspace = "";

            var builder = CommandBuilder;

            var query = QueryBuilder.NewQuery(builder)
                .InsertInto("Category", new[] { "Name", "Namespace" }, new[] { "Name", "Namespace" });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(connString, query, parameters);

            var rows = ExecuteNonQuery(command);

            if (rows == 1) return new CategoryInfo(NameTools.GetFullName(nspace, name), this);
            return null;
        }

        /// <summary>
        ///     Renames a Category.
        /// </summary>
        /// <param name="category">The Category to rename.</param>
        /// <param name="newName">The new Name.</param>
        /// <returns>The correct CategoryInfo object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="category" /> or <paramref name="newName" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="newName" /> is empty.</exception>
        public CategoryInfo RenameCategory(CategoryInfo category, string newName)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            if (newName == null) throw new ArgumentNullException(nameof(newName));
            if (newName.Length == 0) throw new ArgumentException("New Name cannot be empty", nameof(newName));

            string nspace = null;
            string name = null;
            NameTools.ExpandFullName(category.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("Category", new[] { "Name" }, new[] { "NewName" });
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "OldName");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "NewName", newName));
            parameters.Add(new Parameter(ParameterType.String, "OldName", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows > 0)
            {
                var result = GetCategory(transaction, NameTools.GetFullName(nspace, newName));
                CommitTransaction(transaction);
                return result;
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Removes a Category.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="category">The Category to remove.</param>
        /// <returns>True if the Category has been removed successfully.</returns>
        private bool RemoveCategory(IDbTransaction transaction, CategoryInfo category)
        {
            string nspace = null;
            string name = null;
            NameTools.ExpandFullName(category.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("Category");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows > 0;
        }

        /// <summary>
        ///     Removes a Category.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="category">The Category to remove.</param>
        /// <returns>True if the Category has been removed successfully.</returns>
        private bool RemoveCategory(IDbConnection connection, CategoryInfo category)
        {
            string nspace = null;
            string name = null;
            NameTools.ExpandFullName(category.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("Category");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(connection, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows > 0;
        }

        /// <summary>
        ///     Removes a Category.
        /// </summary>
        /// <param name="category">The Category to remove.</param>
        /// <returns>True if the Category has been removed successfully.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="category" /> is <c>null</c>.</exception>
        public bool RemoveCategory(CategoryInfo category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var removed = RemoveCategory(connection, category);
            CloseConnection(connection);

            return removed;
        }

        /// <summary>
        ///     Merges two Categories.
        /// </summary>
        /// <param name="source">The source Category.</param>
        /// <param name="destination">The destination Category.</param>
        /// <returns>The correct <see cref="T:CategoryInfo" /> object.</returns>
        /// <remarks>
        ///     The destination Category remains, while the source Category is deleted, and all its Pages re-bound
        ///     in the destination Category.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="source" /> or <paramref name="destination" /> are
        ///     <c>null</c>.
        /// </exception>
        public CategoryInfo MergeCategories(CategoryInfo source, CategoryInfo destination)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (destination == null) throw new ArgumentNullException(nameof(destination));

            // 1. Check for same namespace
            // 2. Load all pages in source
            // 3. Load all pages in destination
            // 4. Merge lists in memory
            // 5. Delete all destination bindings
            // 6. Delete source cat
            // 7. Insert new bindings stored in memory

            var sourceNs = NameTools.GetNamespace(source.FullName);
            var destinationNs = NameTools.GetNamespace(destination.FullName);

            // If one is null and the other not null, fail
            if (sourceNs == null && destinationNs != null || sourceNs != null && destinationNs == null) return null;
            // Both non-null or both null
            if (sourceNs != null)
            {
                // Both non-null, check names
                var tempSource = new NamespaceInfo(sourceNs, this, null);
                var tempDest = new NamespaceInfo(destinationNs, this, null);
                // Different names, fail
                if (new NamespaceComparer().Compare(tempSource, tempDest) != 0) return null;
            }
            // else both null, OK

            var nspace = sourceNs != null ? sourceNs : "";

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var actualSource = GetCategory(transaction, source.FullName);
            var actualDestination = GetCategory(transaction, destination.FullName);

            if (actualSource == null)
            {
                RollbackTransaction(transaction);
                return null;
            }
            if (actualDestination == null)
            {
                RollbackTransaction(transaction);
                return null;
            }

            var destinationName = NameTools.GetLocalName(actualDestination.FullName);

            var mergedPages = MergeArrays(actualSource.Pages, actualDestination.Pages);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("CategoryBinding");
            query = queryBuilder.Where(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "Category", WhereOperator.Equals, "Category");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.String, "Category", destinationName));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == -1)
            {
                RollbackTransaction(transaction);
                return null;
            }

            if (!RemoveCategory(transaction, source))
            {
                RollbackTransaction(transaction);
                return null;
            }

            var finalQuery = "";
            parameters = new List<Parameter>(MaxStatementsInBatch * 3);
            rows = 0;
            var count = 1;
            string countString;

            foreach (var page in mergedPages)
            {
                // This batch is executed in small chunks (MaxStatementsInBatch) to avoid exceeding DB's max batch length/size

                countString = count.ToString();

                query = queryBuilder.InsertInto("CategoryBinding", new[] { "Namespace", "Category", "Page" },
                    new[] { "Namespace" + countString, "Category" + countString, "Page" + countString });
                finalQuery = queryBuilder.AppendForBatch(finalQuery, query);

                parameters.Add(new Parameter(ParameterType.String, "Namespace" + countString, nspace));
                parameters.Add(new Parameter(ParameterType.String, "Category" + countString, destinationName));
                parameters.Add(new Parameter(ParameterType.String, "Page" + countString, NameTools.GetLocalName(page)));

                count++;

                if (count == MaxStatementsInBatch)
                {
                    // Batch is complete -> execute
                    command = builder.GetCommand(transaction, finalQuery, parameters);
                    rows += ExecuteNonQuery(command, false);

                    count = 1;
                    finalQuery = "";
                    parameters.Clear();
                }
            }

            if (finalQuery.Length > 0)
            {
                // Execute remaining queries, if any
                command = builder.GetCommand(transaction, finalQuery, parameters);
                rows += ExecuteNonQuery(command, false);
            }

            if (rows == mergedPages.Count)
            {
                CommitTransaction(transaction);
                var result = new CategoryInfo(actualDestination.FullName, this);
                result.Pages = mergedPages;
                return result;
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Merges two arrays of strings.
        /// </summary>
        /// <param name="array1">The first array.</param>
        /// <param name="array2">The second array.</param>
        /// <returns>The merged array.</returns>
        private static IList<string> MergeArrays(IList<string> array1, IList<string> array2)
        {
            var result = new List<string>(array1.Count + array2.Count);

            // A) BinarySearch is O(log n), but Insert is O(n) (+ QuickSort which is O(n*log n))
            // B) A linear search is O(n), and Add is O(1) (given that the list is already big enough)
            // --> B is faster, even when result approaches a size of array1.Length + array2.Length

            var comp = StringComparer.OrdinalIgnoreCase;

            result.AddRange(array1);

            foreach (var value in array2)
            {
                if (result.Find(x => { return comp.Compare(x, value) == 0; }) == null)
                {
                    result.Add(value);
                }
            }

            return result;
        }

        /// <summary>
        ///     Gets a page.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="fullName">The full name of the page.</param>
        /// <returns>The <see cref="T:PageInfo" />, or <c>null</c> if no page is found.</returns>
        private PageInfo GetPage(IDbTransaction transaction, string fullName) => GetPage(transaction.Connection, fullName, transaction);

        /// <summary>
        ///     Gets a page.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="fullName">The full name of the page.</param>
        /// <param name="transaction">Transaction associated with the connection.</param>
        /// <returns>The <see cref="T:PageInfo" />, or <c>null</c> if no page is found.</returns>
        private PageInfo GetPage(IDbConnection connection, string fullName, IDbTransaction transaction = null)
        {
            NameTools.ExpandFullName(fullName, out var nspace, out var name);
            if (name == null) return null;
            if (nspace == null) nspace = string.Empty;
            var result = connection.Query("SELECT Namespace, Name, CreationDateTime FROM Page WHERE Name = @Name AND Namespace = @Namespace", new { Name = name, Namespace = nspace }, transaction).FirstOrDefault();

            if (result == null)
                return null;

            return new PageInfo(result.Namespace, result.Name, this, result.CreationDateTime);
        }

        /// <summary>
        ///     Gets a page.
        /// </summary>
        /// <param name="fullName">The full name of the page.</param>
        /// <returns>The <see cref="T:PageInfo" />, or <c>null</c> if no page is found.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="fullName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="fullName" /> is empty.</exception>
        public PageInfo GetPage(string fullName)
        {
            if (fullName == null) throw new ArgumentNullException(nameof(fullName));
            if (fullName.Length == 0) throw new ArgumentException("Full Name cannot be empty", nameof(fullName));

            return pageCache.GetOrAdd(fullName, x =>
            {
                using (var connection = CommandBuilder.GetConnection(connString))
                {
                    return GetPage(connection, fullName);
                }
            });
        }

        /// <summary>
        ///     Gets all the Pages in a namespace.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="nspace">The namespace (<c>null</c> for the root).</param>
        /// <returns>All the Pages in the namespace. The array is not sorted.</returns>
        private IList<PageInfo> GetPages(IDbTransaction transaction, NamespaceInfo nspace)
        {
            var nspaceName = nspace != null ? nspace.Name : "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("Page");
            query = queryBuilder.Where(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.OrderBy(query, new[] { "Name" }, new[] { Ordering.Asc });

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspaceName));

            var command = builder.GetCommand(transaction, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<PageInfo>(100);

                while (reader.Read())
                {
                    result.Add(new PageInfo(reader["Namespace"] as string, reader["Name"] as string, this, (DateTime)reader["CreationDateTime"]));
                }

                CloseReader(reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets all the Pages in a namespace.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="nspace">The namespace (<c>null</c> for the root).</param>
        /// <returns>All the Pages in the namespace. The array is not sorted.</returns>
        private IList<PageInfo> GetPages(IDbConnection connection, NamespaceInfo nspace)
        {
            var nspaceName = nspace != null ? nspace.Name : "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("Page");
            query = queryBuilder.Where(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspaceName));
            query = queryBuilder.OrderBy(query, new[] { "Name" }, new[] { Ordering.Asc });

            var command = builder.GetCommand(connection, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<PageInfo>(100);

                while (reader.Read())
                {
                    result.Add(new PageInfo(reader["Namespace"] as string, reader["Name"] as string, this, (DateTime)reader["CreationDateTime"]));
                }

                CloseReader(reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets all the Pages in a namespace.
        /// </summary>
        /// <param name="nspace">The namespace (<c>null</c> for the root).</param>
        /// <returns>All the Pages in the namespace. The array is not sorted.</returns>
        public IList<PageInfo> GetPages(NamespaceInfo nspace)
        {
            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var pages = GetPages(connection, nspace);
            CloseConnection(connection);

            return pages;
        }

        /// <summary />
        public virtual IList<PageInfo> FindPages(FindPagesFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            var builder = CommandBuilder;
            if (builder == null)
                throw new InvalidOperationException("builder");

            DbConnection connection = null;
            try
            {
                connection = builder.GetConnection(connString);
                if (connection == null)
                    throw new InvalidOperationException("connection");

                var parameters = new List<Parameter>();
                parameters.Add(new Parameter(ParameterType.String, "namespace",
                    filter.Namespace != null ? filter.Namespace.Name : ""));
                parameters.Add(new Parameter(ParameterType.String, "name",
                    !string.IsNullOrEmpty(filter.Name) ? (object)filter.Name.Replace('*', '%') : DBNull.Value));

                var sql =
                    "select [Name], [Namespace], [CreationDateTime] from [Page] where (@namespace is null or [Namespace] = @namespace) and (@name is null or [Name] like @name) order by [Name]";
                if (filter.PageIndex > -1 && filter.PageSize > -1)
                {
                    sql =
                        "select Name, Namespace, CreationDateTime from (select ROW_NUMBER() over (order by Name) as RowNum, * from Page where Namespace = @namespace and (@name is null or name like @name)) as OrderedRows where RowNum >= @startIndex and RowNum < @endIndex";
                    var startIndex = (filter.PageIndex * filter.PageSize) + 1;
                    parameters.Add(new Parameter(ParameterType.Int32, "startIndex", startIndex));
                    parameters.Add(new Parameter(ParameterType.Int32, "endIndex", startIndex + filter.PageSize));
                }

                using (var command = builder.GetCommand(connection, sql, parameters))
                {
                    if (command.Parameters.Count != 0)
                    {
                        //throw new InvalidOperationException(string.Join(", ", command.Parameters.Cast<System.Data.SqlClient.SqlParameter>().Select(p => p.ParameterName)));
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        var result = new List<PageInfo>();
                        while (reader.Read())
                        {
                            result.Add(new PageInfo(reader.GetString(1), reader.GetString(0), this, reader.GetDateTime(2)));
                        }

                        return result;
                    }
                }
            }
            finally
            {
                if (connection != null)
                {
                    CloseConnection(connection);
                }
            }
        }

        /// <summary>
        ///     Gets all the pages in a namespace that are bound to zero categories.
        /// </summary>
        /// <param name="nspace">The namespace (<c>null</c> for the root).</param>
        /// <returns>The pages, sorted by name.</returns>
        public IList<PageInfo> GetUncategorizedPages(NamespaceInfo nspace)
        {
            var nspaceName = nspace != null ? nspace.Name : "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("Page", "CategoryBinding", "Name", "Page", Join.LeftJoin);
            query = queryBuilder.Where(query, "CategoryBinding", "Category", WhereOperator.IsNull, null);
            query = queryBuilder.AndWhere(query, "Page", "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.OrderBy(query, new[] { "Name" }, new[] { Ordering.Asc });

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspaceName));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<PageInfo>(100);

                while (reader.Read())
                {
                    result.Add(new PageInfo(reader["Namespace"] as string, reader["Name"] as string, this, (DateTime)reader["CreationDateTime"]));
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets the content of a specific revision of a page.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="page">The page.</param>
        /// <param name="revision">The revision.</param>
        /// <returns>The content.</returns>
        private PageContent GetContent(IDbConnection connection, PageInfo page, int revision)
        {
            // Internal version to work with GetContent, GetBackupContent, GetDraft

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            string name, nspace;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var query = queryBuilder.SelectFrom("PageContent", "PageKeyword", new[] { "Page", "Namespace", "Revision" },
                new[] { "Page", "Namespace", "Revision" }, Join.LeftJoin,
                new[] { "Title", "User", "LastModified", "Comment", "Content", "Description" }, new[] { "Keyword" });
            query = queryBuilder.Where(query, "PageContent", "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "PageContent", "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "PageContent", "Revision", WhereOperator.Equals, "Revision");

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.Int16, "Revision", (short)revision));

            var command = builder.GetCommand(connection, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                PageContent result = null;

                string title = null, user = null, comment = null, content = null, description = null;
                var dateTime = DateTime.MinValue;
                var keywords = new List<string>(10);

                while (reader.Read())
                {
                    if (title == null)
                    {
                        title = reader["PageContent_Title"] as string;
                        user = reader["PageContent_User"] as string;
                        dateTime = (DateTime)reader["PageContent_LastModified"];
                        comment = GetNullableColumn(reader, "PageContent_Comment", "");
                        content = reader["PageContent_Content"] as string;
                        description = GetNullableColumn<string>(reader, "PageContent_Description", null);
                    }

                    if (!IsDBNull(reader, "PageKeyword_Keyword"))
                    {
                        keywords.Add(reader["PageKeyword_Keyword"] as string);
                    }
                }

                if (title != null)
                {
                    result = new PageContent(page, title, user, dateTime, comment, content, keywords,
                        description);
                }

                CloseReader(reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets the content of a specific revision of a page.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="page">The page.</param>
        /// <param name="revision">The revision.</param>
        /// <returns>The content.</returns>
        private PageContent GetContent(IDbTransaction transaction, PageInfo page, int revision)
        {
            // Internal version to work with GetContent, GetBackupContent, GetDraft

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            string name, nspace;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var query = queryBuilder.SelectFrom("PageContent", "PageKeyword", new[] { "Page", "Namespace", "Revision" },
                new[] { "Page", "Namespace", "Revision" }, Join.LeftJoin,
                new[] { "Title", "User", "LastModified", "Comment", "Content", "Description" }, new[] { "Keyword" });
            query = queryBuilder.Where(query, "PageContent", "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "PageContent", "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "PageContent", "Revision", WhereOperator.Equals, "Revision");

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.Int16, "Revision", (short)revision));

            var command = builder.GetCommand(transaction, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                PageContent result = null;

                string title = null, user = null, comment = null, content = null, description = null;
                var dateTime = DateTime.MinValue;
                var keywords = new List<string>(10);

                while (reader.Read())
                {
                    if (title == null)
                    {
                        title = reader["PageContent_Title"] as string;
                        user = reader["PageContent_User"] as string;
                        dateTime = (DateTime)reader["PageContent_LastModified"];
                        comment = GetNullableColumn(reader, "PageContent_Comment", "");
                        content = reader["PageContent_Content"] as string;
                        description = GetNullableColumn<string>(reader, "PageContent_Description", null);
                    }

                    if (!IsDBNull(reader, "PageKeyword_Keyword"))
                    {
                        keywords.Add(reader["PageKeyword_Keyword"] as string);
                    }
                }

                if (title != null)
                {
                    result = new PageContent(page, title, user, dateTime, comment, content, keywords.ToArray(),
                        description);
                }

                CloseReader(reader);

                return result;
            }
            return null;
        }

        /// <summary>
        /// Gets the title of a Page.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <returns>
        /// The page title, <c>null</c> if the page does not exists or <paramref name="page"/> is <c>null</c>.
        /// </returns>
        public string GetTitle(PageInfo page)
        {
            if (page == null) return null;

            var builder = CommandBuilder;
            using (var conn = builder.GetConnection(connString))
            {
                NameTools.ExpandFullName(page.FullName, out var nspace, out var name);
                return conn.Query<string>("SELECT Title FROM PageContent WHERE Page = @Page AND Namespace = @Namespace AND Revision = @Revision", new { Page = name, Namespace = nspace, Revision = -1 }).FirstOrDefault();
            }
        }

        /// <summary>
        ///     Gets the Content of a Page.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <returns>
        ///     The Page Content object, <c>null</c> if the page does not exist or <paramref name="page" /> is <c>null</c>,
        ///     or an empty instance if the content could not be retrieved (<seealso cref="PageContent.GetEmpty" />).
        /// </returns>
        public PageContent GetContent(PageInfo page)
        {
            if (page == null) return null;

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var content = GetContent(connection, page, CurrentRevision);
            CloseConnection(connection);

            return content;
        }

        /// <summary>
        ///     Gets the content of a draft of a Page.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <returns>The draft, or <c>null</c> if no draft exists.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public PageContent GetDraft(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var content = GetContent(connection, page, DraftRevision);
            CloseConnection(connection);

            return content;
        }

        /// <summary>
        ///     Deletes a draft of a Page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns><c>true</c> if the draft is deleted, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public bool DeleteDraft(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var deleted = DeleteContent(connection, page, DraftRevision);
            CloseConnection(connection);

            return deleted;
        }

        /// <summary>
        ///     Gets the Backup/Revision numbers of a Page.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="page">The Page to get the Backups of.</param>
        /// <returns>The Backup/Revision numbers.</returns>
        private int[] GetBackups(IDbTransaction transaction, PageInfo page)
        {
            if (GetPage(transaction, page.FullName) == null)
            {
                return null;
            }

            string name, nspace;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("PageContent", new[] { "Revision" });
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "Revision", WhereOperator.GreaterThanOrEqualTo, "Revision");
            query = queryBuilder.OrderBy(query, new[] { "Revision" }, new[] { Ordering.Asc });

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.Int16, "Revision", FirstRevision));

            var command = builder.GetCommand(transaction, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<int>(100);

                while (reader.Read())
                {
                    result.Add((short)reader["Revision"]);
                }

                CloseReader(reader);

                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        ///     Gets the Backup/Revision numbers of a Page.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="page">The Page to get the Backups of.</param>
        /// <returns>The Backup/Revision numbers.</returns>
        private int[] GetBackups(IDbConnection connection, PageInfo page)
        {
            if (GetPage(connection, page.FullName) == null)
            {
                return null;
            }

            string name, nspace;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("PageContent", new[] { "Revision" });
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "Revision", WhereOperator.GreaterThanOrEqualTo, "Revision");
            query = queryBuilder.OrderBy(query, new[] { "Revision" }, new[] { Ordering.Asc });

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.Int16, "Revision", FirstRevision));

            var command = builder.GetCommand(connection, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<int>(100);

                while (reader.Read())
                {
                    result.Add((short)reader["Revision"]);
                }

                CloseReader(reader);

                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        ///     Gets the Backup/Revision numbers of a Page.
        /// </summary>
        /// <param name="page">The Page to get the Backups of.</param>
        /// <returns>The Backup/Revision numbers.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public int[] GetBackups(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var revisions = GetBackups(connection, page);
            CloseConnection(connection);

            return revisions;
        }

        /// <summary>
        ///     Gets the Content of a Backup of a Page.
        /// </summary>
        /// <param name="page">The Page to get the backup of.</param>
        /// <param name="revision">The Backup/Revision number.</param>
        /// <returns>The Page Backup.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="revision" /> is less than zero.</exception>
        public PageContent GetBackupContent(PageInfo page, int revision)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (revision < 0) throw new ArgumentOutOfRangeException(nameof(revision), "Invalid Revision");

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var content = GetContent(connection, page, revision);
            CloseConnection(connection);

            return content;
        }

        /// <summary>
        ///     Stores the content for a revision.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="content">The content.</param>
        /// <param name="revision">The revision.</param>
        /// <returns><c>true</c> if the content is stored, <c>false</c> otherwise.</returns>
        private bool SetContent(IDbTransaction transaction, PageContent content, int revision)
        {
            string name, nspace;
            NameTools.ExpandFullName(content.PageInfo.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.InsertInto("PageContent",
                new[]
                {"Page", "Namespace", "Revision", "Title", "User", "LastModified", "Comment", "Content", "Description"},
                new[]
                {"Page", "Namespace", "Revision", "Title", "User", "LastModified", "Comment", "Content", "Description"});

            var parameters = new List<Parameter>(9);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.Int16, "Revision", revision));
            parameters.Add(new Parameter(ParameterType.String, "Title", content.Title));
            parameters.Add(new Parameter(ParameterType.String, "User", content.User));
            parameters.Add(new Parameter(ParameterType.DateTime, "LastModified", content.LastModified));
            if (!string.IsNullOrEmpty(content.Comment))
                parameters.Add(new Parameter(ParameterType.String, "Comment", content.Comment));
            else parameters.Add(new Parameter(ParameterType.String, "Comment", DBNull.Value));
            parameters.Add(new Parameter(ParameterType.String, "Content", content.Content));
            if (!string.IsNullOrEmpty(content.Description))
                parameters.Add(new Parameter(ParameterType.String, "Description", content.Description));
            else parameters.Add(new Parameter(ParameterType.String, "Description", DBNull.Value));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows != 1) return false;

            if (content.Keywords.Count > 0)
            {
                parameters = new List<Parameter>(content.Keywords.Count * 4);
                var fullQuery = "";
                var count = 0;
                string countString;
                foreach (var kw in content.Keywords)
                {
                    countString = count.ToString();

                    query = queryBuilder.InsertInto("PageKeyword", new[] { "Page", "Namespace", "Revision", "Keyword" },
                        new[]
                        {
                            "Page" + countString, "Namespace" + countString, "Revision" + countString,
                            "Keyword" + countString
                        });
                    fullQuery = queryBuilder.AppendForBatch(fullQuery, query);

                    parameters.Add(new Parameter(ParameterType.String, "Page" + countString, name));
                    parameters.Add(new Parameter(ParameterType.String, "Namespace" + countString, nspace));
                    parameters.Add(new Parameter(ParameterType.Int16, "Revision" + countString, revision));
                    parameters.Add(new Parameter(ParameterType.String, "Keyword" + countString, kw));

                    count++;
                }

                command = builder.GetCommand(transaction, fullQuery, parameters);

                rows = ExecuteNonQuery(command, false);

                return rows == content.Keywords.Count;
            }
            return true;
        }

        /// <summary>
        ///     Deletes a revision of a page content.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="page">The page.</param>
        /// <param name="revision">The revision.</param>
        /// <returns><c>true</c> if the content ir deleted, <c>false</c> otherwise.</returns>
        private bool DeleteContent(IDbTransaction transaction, PageInfo page, int revision)
        {
            string name, nspace;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("PageContent");
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "Revision", WhereOperator.Equals, "Revision");

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.String, "Revision", revision));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows > 0;
        }

        /// <summary>
        ///     Deletes a revision of a page content.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="page">The page.</param>
        /// <param name="revision">The revision.</param>
        /// <returns><c>true</c> if the content ir deleted, <c>false</c> otherwise.</returns>
        private bool DeleteContent(IDbConnection connection, PageInfo page, int revision)
        {
            string name, nspace;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("PageContent");
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "Revision", WhereOperator.Equals, "Revision");

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.String, "Revision", revision));

            var command = builder.GetCommand(connection, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows > 0;
        }

        /// <summary>
        ///     Forces to overwrite or create a Backup.
        /// </summary>
        /// <param name="content">The Backup content.</param>
        /// <param name="revision">The revision.</param>
        /// <returns>True if the Backup has been created successfully.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="content" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="revision" /> is less than zero.</exception>
        public bool SetBackupContent(PageContent content, int revision)
        {
            if (content == null) throw new ArgumentNullException(nameof(content));
            if (revision < 0) throw new ArgumentOutOfRangeException(nameof(revision), "Invalid Revision");

            // 1. DeletebBackup, if any
            // 2. Set new content

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            DeleteContent(transaction, content.PageInfo, revision);

            var set = SetContent(transaction, content, revision);

            if (set) CommitTransaction(transaction);
            else RollbackTransaction(transaction);

            return set;
        }

        /// <summary>
        ///     Adds a Page.
        /// </summary>
        /// <param name="nspace">The target namespace (<c>null</c> for the root).</param>
        /// <param name="name">The Page Name.</param>
        /// <param name="creationDateTime">The creation Date/Time.</param>
        /// <returns>The correct PageInfo object or null.</returns>
        /// <remarks>This method should <b>not</b> create the content of the Page.</remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name" /> is empty.</exception>
        public PageInfo AddPage(string nspace, string name, DateTime creationDateTime)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", nameof(name));

            if (nspace == null) nspace = "";

            var builder = CommandBuilder;

            var query = QueryBuilder.NewQuery(builder)
                .InsertInto("Page", new[] { "Name", "Namespace", "CreationDateTime" },
                    new[] { "Name", "Namespace", "CreationDateTime" });

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.DateTime, "CreationDateTime", creationDateTime));

            var command = builder.GetCommand(connString, query, parameters);

            var rows = ExecuteNonQuery(command);

            if (rows == 1)
            {
                var pageInfo = new PageInfo(nspace, name, this, creationDateTime);
                pageCache.TryRemove(pageInfo.FullName, out var _);
                return pageInfo;
            }

            return null;
        }

        /// <summary>
        ///     Renames a Page.
        /// </summary>
        /// <param name="page">The Page to rename.</param>
        /// <param name="newName">The new Name.</param>
        /// <returns>The correct <see cref="T:PageInfo" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> or <paramref name="newName" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="newName" /> is empty.</exception>
        public PageInfo RenamePage(PageInfo page, string newName)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (newName == null) throw new ArgumentNullException(nameof(newName));
            if (newName.Length == 0) throw new ArgumentException("New Name cannot be empty", nameof(newName));

            // Check
            // 1. Page is default page of its namespace
            // 2. New name already exists

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (GetPage(transaction, page.FullName) == null)
            {
                RollbackTransaction(transaction);
                return null;
            }
            if (IsDefaultPage(transaction, page))
            {
                RollbackTransaction(transaction);
                return null;
            }
            if (
                GetPage(transaction,
                    NameTools.GetFullName(NameTools.GetNamespace(page.FullName), NameTools.GetLocalName(newName))) !=
                null)
            {
                RollbackTransaction(transaction);
                return null;
            }

            var currentContent = GetContent(transaction, page, CurrentRevision);
            UnindexPage(currentContent, transaction);
            foreach (var msg in GetMessages(transaction, page))
            {
                UnindexMessageTree(page, msg, transaction);
            }

            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var currCategories = GetCategories(transaction, nspace == "" ? null : GetNamespace(transaction, nspace));
            var lowerPageName = page.FullName.ToLowerInvariant();
            var pageCategories = new List<string>(10);
            foreach (var cat in currCategories)
            {
                if (cat.Pages.Any(s => s.ToLowerInvariant() == lowerPageName))
                {
                    pageCategories.Add(NameTools.GetLocalName(cat.FullName));
                }
            }

            RebindPage(transaction, page, new string[0]);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("Page", new[] { "Name" }, new[] { "NewName" });
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "OldName");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "NewName", newName));
            parameters.Add(new Parameter(ParameterType.String, "OldName", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows > 0)
            {
                var result = new PageInfo(nspace, newName, this, page.CreationDateTime);

                RebindPage(transaction, result, pageCategories.ToArray());

                var newContent = GetContent(transaction, result, CurrentRevision);

                IndexPage(newContent, transaction);
                foreach (var msg in GetMessages(transaction, result))
                {
                    IndexMessageTree(result, msg, transaction);
                }

                CommitTransaction(transaction);

                return result;
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Modifies the Content of a Page.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <param name="title">The Title of the Page.</param>
        /// <param name="username">The Username.</param>
        /// <param name="dateTime">The Date/Time.</param>
        /// <param name="comment">The Comment of the editor, about this revision.</param>
        /// <param name="content">The Page Content.</param>
        /// <param name="keywords">The keywords, usually used for SEO.</param>
        /// <param name="description">The description, usually used for SEO.</param>
        /// <param name="saveMode">The save mode for this modification.</param>
        /// <returns><c>true</c> if the Page has been modified successfully, <c>false</c> otherwise.</returns>
        /// <remarks>If <b>saveMode</b> equals <b>Draft</b> and a draft already exists, it is overwritten.</remarks>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="page" />, <paramref name="title" />,
        ///     <paramref name="username" /> or <paramref name="content" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="title" /> or <paramref name="username" /> are empty.</exception>
        public bool ModifyPage(PageInfo page, string title, string username, DateTime dateTime, string comment,
            string content, IList<string> keywords, string description, SaveMode saveMode)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (title == null) throw new ArgumentNullException(nameof(title));
            if (title.Length == 0) throw new ArgumentException("Title cannot be empty", nameof(title));
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (username.Length == 0) throw new ArgumentException("Username cannot be empty", nameof(username));
            if (content == null) throw new ArgumentNullException(nameof(content)); // content can be empty

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var currentContent = GetContent(transaction, page, CurrentRevision);

            var pageContent = new PageContent(page, title, username, dateTime, comment, content,
                keywords != null ? keywords : new string[0], description);

            switch (saveMode)
            {
                case SaveMode.Backup:
                    // Do backup (if there is something to backup), delete current version (if any), store new version
                    if (currentContent != null) UnindexPage(currentContent, transaction);
                    Backup(transaction, page);
                    DeleteContent(transaction, page, CurrentRevision);
                    var done1 = SetContent(transaction, pageContent, CurrentRevision);
                    if (done1) IndexPage(pageContent, transaction);

                    if (done1) CommitTransaction(transaction);
                    else RollbackTransaction(transaction);

                    return done1;
                case SaveMode.Normal:
                    // Delete current version (if any), store new version
                    if (currentContent != null) UnindexPage(currentContent, transaction);
                    DeleteContent(transaction, page, CurrentRevision);
                    var done2 = SetContent(transaction, pageContent, CurrentRevision);
                    if (done2) IndexPage(pageContent, transaction);

                    if (done2) CommitTransaction(transaction);
                    else RollbackTransaction(transaction);

                    return done2;
                case SaveMode.Draft:
                    // Delete current draft (if any), store new draft
                    DeleteContent(transaction, page, DraftRevision);
                    var done3 = SetContent(transaction, pageContent, DraftRevision);

                    if (done3) CommitTransaction(transaction);
                    else RollbackTransaction(transaction);

                    return done3;
                default:
                    RollbackTransaction(transaction);
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Backs up the content of a page.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="page">The page.</param>
        /// <returns><c>true</c> if the backup is performed, <c>false</c> otherwise.</returns>
        private bool Backup(IDbTransaction transaction, PageInfo page)
        {
            var currentContent = GetContent(transaction, page, CurrentRevision);

            if (currentContent != null)
            {
                // Insert a new revision
                var backups = GetBackups(transaction, page);
                if (backups == null) return false;

                var revision = backups.Length > 0 ? backups[backups.Length - 1] + 1 : FirstRevision;
                var set = SetContent(transaction, currentContent, revision);

                return set;
            }
            return false;
        }

        /// <summary>
        ///     Performs the rollback of a Page to a specified revision.
        /// </summary>
        /// <param name="page">The Page to rollback.</param>
        /// <param name="revision">The Revision to rollback the Page to.</param>
        /// <returns><c>true</c> if the rollback succeeded, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="revision" /> is less than zero.</exception>
        public bool RollbackPage(PageInfo page, int revision)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (revision < 0) throw new ArgumentOutOfRangeException(nameof(revision), "Invalid Revision");

            // 1. Load specific revision's content
            // 2. Modify page with loaded content, performing backup

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var targetContent = GetContent(transaction, page, revision);

            if (targetContent == null)
            {
                RollbackTransaction(transaction);
                return false;
            }

            UnindexPage(GetContent(transaction, page, CurrentRevision), transaction);

            var done = Backup(transaction, page);
            if (!done)
            {
                RollbackTransaction(transaction);
                return false;
            }

            done = DeleteContent(transaction, page, CurrentRevision);
            if (!done)
            {
                RollbackTransaction(transaction);
                return false;
            }

            done = SetContent(transaction, targetContent, CurrentRevision);
            if (!done)
            {
                RollbackTransaction(transaction);
                return false;
            }

            IndexPage(targetContent, transaction);

            CommitTransaction(transaction);

            return true;
        }

        /// <summary>
        ///     Deletes the Backups of a Page, up to a specified revision.
        /// </summary>
        /// <param name="page">The Page to delete the backups of.</param>
        /// <param name="revision">The newest revision to delete (newer revision are kept) o -1 to delete all the Backups.</param>
        /// <returns><c>true</c> if the deletion succeeded, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="revision" /> is less than -1.</exception>
        public bool DeleteBackups(PageInfo page, int revision)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (revision < -1) throw new ArgumentOutOfRangeException(nameof(revision), "Invalid Revision");

            // 1. Retrieve target content (revision-1 = first kept revision)
            // 2. Replace the current content (delete, store)
            // 3. Delete all older revisions up to the specified on (included) "N-m...N"
            // 4. Re-number remaining revisions starting from FirstRevision (zero) to revision-1 (don't re-number revs -1, -100)

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (GetPage(transaction, page.FullName) == null)
            {
                RollbackTransaction(transaction);
                return false;
            }

            var baks = GetBackups(transaction, page);
            if (baks.Length > 0 && revision > baks[baks.Length - 1])
            {
                RollbackTransaction(transaction);
                return true;
            }

            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("PageContent");
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
            if (revision != -1)
                query = queryBuilder.AndWhere(query, "Revision", WhereOperator.LessThanOrEqualTo, "Revision");
            query = queryBuilder.AndWhere(query, "Revision", WhereOperator.GreaterThanOrEqualTo, "FirstRevision");

            var parameters = new List<Parameter>(4);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            if (revision != -1) parameters.Add(new Parameter(ParameterType.Int16, "Revision", revision));
            parameters.Add(new Parameter(ParameterType.Int16, "FirstRevision", FirstRevision));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == -1)
            {
                RollbackTransaction(transaction);
                return false;
            }

            if (revision != -1)
            {
                var revisionDelta = revision + 1;

                query = queryBuilder.UpdateIncrement("PageContent", "Revision", -revisionDelta);
                query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
                query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
                query = queryBuilder.AndWhere(query, "Revision", WhereOperator.GreaterThanOrEqualTo, "FirstRevision");

                parameters = new List<Parameter>(3);
                parameters.Add(new Parameter(ParameterType.String, "Page", name));
                parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
                parameters.Add(new Parameter(ParameterType.Int16, "FirstRevision", FirstRevision));

                command = builder.GetCommand(transaction, query, parameters);

                rows = ExecuteNonQuery(command, false);

                if (rows > 0) CommitTransaction(transaction);
                else RollbackTransaction(transaction);

                return rows >= 0;
            }
            CommitTransaction(transaction);
            return true;
        }

        /// <summary>
        ///     Removes a Page.
        /// </summary>
        /// <param name="page">The Page to remove.</param>
        /// <returns>True if the Page is removed successfully.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public bool RemovePage(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (IsDefaultPage(transaction, page))
            {
                RollbackTransaction(transaction);
                return false;
            }

            var currentContent = GetContent(transaction, page, CurrentRevision);
            if (currentContent != null)
            {
                UnindexPage(currentContent, transaction);
                foreach (var msg in GetMessages(transaction, page))
                {
                    UnindexMessageTree(page, msg, transaction);
                }
            }

            RebindPage(transaction, page, new string[0]);

            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("Page");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows > 0) CommitTransaction(transaction);
            else RollbackTransaction(transaction);

            return rows > 0;
        }

        /// <summary>
        ///     Binds a Page with one or more Categories.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="page">The Page to bind.</param>
        /// <param name="categories">The Categories to bind the Page with.</param>
        /// <returns>True if the binding succeeded.</returns>
        /// <remarks>After a successful operation, the Page is bound with all and only the categories passed as argument.</remarks>
        private bool RebindPage(IDbTransaction transaction, PageInfo page, string[] categories)
        {
            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("CategoryBinding");
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows < 0) return false;

            if (categories.Length > 0)
            {
                var finalQuery = "";
                parameters = new List<Parameter>(categories.Length * 3);
                var count = 0;
                string countString;

                foreach (var cat in categories)
                {
                    countString = count.ToString();

                    query = queryBuilder.InsertInto("CategoryBinding", new[] { "Namespace", "Category", "Page" },
                        new[] { "Namespace" + countString, "Category" + countString, "Page" + countString });
                    finalQuery = queryBuilder.AppendForBatch(finalQuery, query);

                    parameters.Add(new Parameter(ParameterType.String, "Namespace" + countString, nspace));
                    parameters.Add(new Parameter(ParameterType.String, "Category" + countString,
                        NameTools.GetLocalName(cat)));
                    parameters.Add(new Parameter(ParameterType.String, "Page" + countString, name));

                    count++;
                }

                command = builder.GetCommand(transaction, finalQuery, parameters);

                rows = ExecuteNonQuery(command, false);

                return rows == categories.Length;
            }

            pageCache.TryRemove(page.FullName, out var _);

            return true;
        }

        /// <summary>
        ///     Binds a Page with one or more Categories.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="page">The Page to bind.</param>
        /// <param name="categories">The Categories to bind the Page with.</param>
        /// <returns>True if the binding succeeded.</returns>
        /// <remarks>After a successful operation, the Page is bound with all and only the categories passed as argument.</remarks>
        private bool RebindPage(IDbConnection connection, PageInfo page, string[] categories)
        {
            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("CategoryBinding");
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(connection, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows < 0) return false;

            if (categories.Length > 0)
            {
                var finalQuery = "";
                parameters = new List<Parameter>(categories.Length * 3);
                var count = 0;
                string countString;

                foreach (var cat in categories)
                {
                    countString = count.ToString();

                    query = queryBuilder.InsertInto("CategoryBinding", new[] { "Namespace", "Category", "Page" },
                        new[] { "Namespace" + countString, "Category" + countString, "Page" + countString });
                    finalQuery = queryBuilder.AppendForBatch(finalQuery, query);

                    parameters.Add(new Parameter(ParameterType.String, "Namespace" + countString, nspace));
                    parameters.Add(new Parameter(ParameterType.String, "Category" + countString,
                        NameTools.GetLocalName(cat)));
                    parameters.Add(new Parameter(ParameterType.String, "Page" + countString, name));

                    count++;
                }

                command = builder.GetCommand(connection, finalQuery, parameters);

                rows = ExecuteNonQuery(command, false);

                return rows == categories.Length;
            }

            pageCache.TryRemove(page.FullName, out var _);

            return true;
        }

        /// <summary>
        ///     Binds a Page with one or more Categories.
        /// </summary>
        /// <param name="page">The Page to bind.</param>
        /// <param name="categories">The Categories to bind the Page with.</param>
        /// <returns>True if the binding succeeded.</returns>
        /// <remarks>After a successful operation, the Page is bound with all and only the categories passed as argument.</remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> or <paramref name="categories" /> are <c>null</c>.</exception>
        public bool RebindPage(PageInfo page, string[] categories)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (categories == null) throw new ArgumentNullException(nameof(categories));

            foreach (var cat in categories)
            {
                if (cat == null) throw new ArgumentNullException(nameof(categories));
                if (cat.Length == 0) throw new ArgumentException("Category item cannot be empty", nameof(categories));
            }

            // 1. Delete old bindings
            // 2. Store new bindings

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var rebound = RebindPage(connection, page, categories);
            CloseConnection(connection);

            pageCache.TryRemove(page.FullName, out var _);

            return rebound;
        }

        /// <summary>
        ///     Gets the Page Messages.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="page">The Page.</param>
        /// <returns>The list of the <b>first-level</b> Messages, containing the replies properly nested, sorted by date/time.</returns>
        private IList<Message> GetMessages(IDbTransaction transaction, PageInfo page)
        {
            if (GetPage(transaction, page.FullName) == null) return null;

            // 1. Load all messages in memory in a dictionary id->message
            // 2. Build tree using ParentID

            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("Message",
                new[] { "Id", "Parent", "Username", "Subject", "DateTime", "Body" });
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.OrderBy(query, new[] { "DateTime", "Id" }, new[] { Ordering.Asc, Ordering.Asc });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(transaction, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var allMessages = new Dictionary<short, Message>(50);
                var ids = new List<short>(50);
                var parents = new List<short?>(50);

                while (reader.Read())
                {
                    var msg = new Message((short)reader["Id"], reader["Username"] as string,
                        reader["Subject"] as string,
                        (DateTime)reader["DateTime"], reader["Body"] as string);

                    ids.Add((short)msg.ID);

                    // Import from V2: parent = -1, otherwise null
                    if (!IsDBNull(reader, "Parent"))
                    {
                        var par = (short)reader["Parent"];
                        if (par >= 0) parents.Add(par);
                        else parents.Add(null);
                    }
                    else parents.Add(null);

                    allMessages.Add((short)msg.ID, msg);
                }

                CloseReader(reader);

                // Add messages to their parents and build the top-level messages list
                var result = new List<Message>(20);

                for (var i = 0; i < ids.Count; i++)
                {
                    var currentParent = parents[i];
                    var currentId = ids[i];

                    if (currentParent.HasValue)
                    {
                        var replies = new List<Message>(allMessages[currentParent.Value].Replies);
                        replies.Add(allMessages[currentId]);
                        allMessages[currentParent.Value].Replies = replies.ToArray();
                    }
                    else result.Add(allMessages[currentId]);
                }

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets the Page Messages.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="page">The Page.</param>
        /// <returns>The list of the <b>first-level</b> Messages, containing the replies properly nested, sorted by date/time.</returns>
        private IList<Message> GetMessages(IDbConnection connection, PageInfo page)
        {
            if (GetPage(connection, page.FullName) == null) return null;

            // 1. Load all messages in memory in a dictionary id->message
            // 2. Build tree using ParentID

            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("Message",
                new[] { "Id", "Parent", "Username", "Subject", "DateTime", "Body" });
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.OrderBy(query, new[] { "DateTime", "Id" }, new[] { Ordering.Asc, Ordering.Asc });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(connection, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var allMessages = new Dictionary<short, Message>(50);
                var ids = new List<short>(50);
                var parents = new List<short?>(50);

                while (reader.Read())
                {
                    var msg = new Message((short)reader["Id"], reader["Username"] as string,
                        reader["Subject"] as string,
                        (DateTime)reader["DateTime"], reader["Body"] as string);

                    ids.Add((short)msg.ID);

                    // Import from V2: parent = -1, otherwise null
                    if (!IsDBNull(reader, "Parent"))
                    {
                        var par = (short)reader["Parent"];
                        if (par >= 0) parents.Add(par);
                        else parents.Add(null);
                    }
                    else parents.Add(null);

                    allMessages.Add((short)msg.ID, msg);
                }

                CloseReader(reader);

                // Add messages to their parents and build the top-level messages list
                var result = new List<Message>(20);

                for (var i = 0; i < ids.Count; i++)
                {
                    var currentParent = parents[i];
                    var currentId = ids[i];

                    if (currentParent.HasValue)
                    {
                        var replies = new List<Message>(allMessages[currentParent.Value].Replies);
                        replies.Add(allMessages[currentId]);
                        allMessages[currentParent.Value].Replies = replies.ToArray();
                    }
                    else result.Add(allMessages[currentId]);
                }

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Gets the Page Messages.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <returns>The list of the <b>first-level</b> Messages, containing the replies properly nested, sorted by date/time.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public IList<Message> GetMessages(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var messages = GetMessages(connection, page);
            CloseConnection(connection);

            return messages;
        }

        /// <summary>
        ///     Gets the total number of Messages in a Page Discussion.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <returns>The number of messages.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public int GetMessageCount(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            if (GetPage(connection, page.FullName) == null)
            {
                CloseConnection(connection);
                return -1;
            }

            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectCountFrom("Message");
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(connection, query, parameters);

            var count = ExecuteScalar(command, 0);

            return count;
        }

        /// <summary>
        ///     Removes all messages for a page and stores the new messages.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="messages">The new messages to store.</param>
        /// <returns><c>true</c> if the messages are stored, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> or <paramref name="messages" /> are <c>null</c>.</exception>
        public bool BulkStoreMessages(PageInfo page, IList<Message> messages)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (messages == null) throw new ArgumentNullException(nameof(messages));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (GetPage(transaction, page.FullName) == null)
            {
                RollbackTransaction(transaction);
                return false;
            }

            foreach (var msg in GetMessages(transaction, page))
            {
                UnindexMessageTree(page, msg, transaction);
            }

            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("Message");
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(transaction, query, parameters);

            ExecuteNonQuery(command, false);

            List<Message> allMessages;
            List<int> parents;

            UnTreeMessages(messages, out allMessages, out parents, -1);

            var finalQuery = "";
            var count = 1;
            string countString;
            parameters = new List<Parameter>(MaxStatementsInBatch * 8);

            var rowsDone = 0;

            for (var i = 0; i < allMessages.Count; i++)
            {
                // Execute the batch in smaller chunks

                var msg = allMessages[i];
                var parent = parents[i];

                countString = count.ToString();

                query = queryBuilder.InsertInto("Message",
                    new[] { "Page", "Namespace", "Id", "Parent", "Username", "Subject", "DateTime", "Body" },
                    new[]
                    {
                        "Page" + countString, "Namespace" + countString, "Id" + countString, "Parent" + countString,
                        "Username" + countString, "Subject" + countString, "DateTime" + countString,
                        "Body" + countString
                    });

                parameters.Add(new Parameter(ParameterType.String, "Page" + countString, name));
                parameters.Add(new Parameter(ParameterType.String, "Namespace" + countString, nspace));
                parameters.Add(new Parameter(ParameterType.Int16, "Id" + countString, (short)msg.ID));
                if (parent != -1) parameters.Add(new Parameter(ParameterType.Int16, "Parent" + countString, parent));
                else parameters.Add(new Parameter(ParameterType.Int16, "Parent" + countString, DBNull.Value));
                parameters.Add(new Parameter(ParameterType.String, "Username" + countString, msg.Username));
                parameters.Add(new Parameter(ParameterType.String, "Subject" + countString, msg.Subject));
                parameters.Add(new Parameter(ParameterType.DateTime, "DateTime" + countString, msg.DateTime));
                parameters.Add(new Parameter(ParameterType.String, "Body" + countString, msg.Body));

                finalQuery = queryBuilder.AppendForBatch(finalQuery, query);

                count++;

                if (count == MaxStatementsInBatch)
                {
                    command = builder.GetCommand(transaction, finalQuery, parameters);

                    rowsDone += ExecuteNonQuery(command, false);

                    finalQuery = "";
                    count = 1;
                    parameters.Clear();
                }
            }

            if (finalQuery.Length > 0)
            {
                command = builder.GetCommand(transaction, finalQuery, parameters);

                rowsDone += ExecuteNonQuery(command, false);
            }

            if (rowsDone == allMessages.Count)
            {
                foreach (var msg in messages)
                {
                    IndexMessageTree(page, msg, transaction);
                }
                CommitTransaction(transaction);
                return true;
            }
            RollbackTransaction(transaction);
            return false;
        }

        /// <summary>
        ///     Deconstructs a tree of messages and converts it into a flat list.
        /// </summary>
        /// <param name="messages">The input tree.</param>
        /// <param name="flatList">The resulting flat message list.</param>
        /// <param name="parent">The list of parent IDs.</param>
        /// <param name="parents">The current parent ID.</param>
        private static void UnTreeMessages(IList<Message> messages, out List<Message> flatList, out List<int> parents,
            int parent)
        {
            flatList = new List<Message>(20);
            parents = new List<int>(20);

            flatList.AddRange(messages);
            for (var i = 0; i < messages.Count; i++)
            {
                parents.Add(parent);
            }

            foreach (var msg in messages)
            {
                List<Message> temp;
                List<int> tempParents;

                UnTreeMessages(msg.Replies, out temp, out tempParents, msg.ID);

                flatList.AddRange(temp);
                parents.AddRange(tempParents);
            }
        }

        /// <summary>
        ///     Adds a new Message to a Page.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <param name="username">The Username.</param>
        /// <param name="subject">The Subject.</param>
        /// <param name="dateTime">The Date/Time.</param>
        /// <param name="body">The Body.</param>
        /// <param name="parent">The Parent Message ID, or -1.</param>
        /// <returns>True if the Message is added successfully.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="page" />, <paramref name="username" />,
        ///     <paramref name="subject" /> or <paramref name="body" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="username" /> or <paramref name="subject" /> are empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="parent" /> is less than -1.</exception>
        public bool AddMessage(PageInfo page, string username, string subject, DateTime dateTime, string body,
            int parent)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (username.Length == 0) throw new ArgumentException("Username cannot be empty", nameof(username));
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (subject.Length == 0) throw new ArgumentException("Subject cannot be empty", nameof(subject));
            if (body == null) throw new ArgumentNullException(nameof(body)); // body can be empty
            if (parent < -1) throw new ArgumentOutOfRangeException(nameof(parent), "Invalid Parent Message ID");

            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (parent != -1 && FindMessage(GetMessages(transaction, page), parent) == null)
            {
                RollbackTransaction(transaction);
                return false;
            }

            var queryBuilder = new QueryBuilder(builder);

            short freeId = -1;

            var query = queryBuilder.SelectFrom("Message", new[] { "Id" });
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.OrderBy(query, new[] { "Id" }, new[] { Ordering.Desc });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(transaction, query, parameters);

            freeId = ExecuteScalar<short>(command, -1, false);

            if (freeId == -1) freeId = 0;
            else freeId++;

            query = queryBuilder.InsertInto("Message",
                new[] { "Page", "Namespace", "Id", "Parent", "Username", "Subject", "DateTime", "Body" },
                new[] { "Page", "Namespace", "Id", "Parent", "Username", "Subject", "DateTime", "Body" });

            parameters = new List<Parameter>(8);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.Int16, "Id", freeId));
            if (parent != -1) parameters.Add(new Parameter(ParameterType.Int16, "Parent", parent));
            else parameters.Add(new Parameter(ParameterType.Int16, "Parent", DBNull.Value));
            parameters.Add(new Parameter(ParameterType.String, "Username", username));
            parameters.Add(new Parameter(ParameterType.String, "Subject", subject));
            parameters.Add(new Parameter(ParameterType.DateTime, "DateTime", dateTime));
            parameters.Add(new Parameter(ParameterType.String, "Body", body));

            command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1)
            {
                IndexMessage(page, freeId, subject, dateTime, body, transaction);

                CommitTransaction(transaction);
                return true;
            }
            RollbackTransaction(transaction);
            return false;
        }

        /// <summary>
        ///     Finds a Message in a Message tree.
        /// </summary>
        /// <param name="messages">The Message tree.</param>
        /// <param name="id">The ID of the Message to find.</param>
        /// <returns>The Message or null.</returns>
        /// <remarks>The method is recursive.</remarks>
        private static Message FindMessage(IEnumerable<Message> messages, int id)
        {
            Message result = null;
            foreach (var msg in messages)
            {
                if (msg.ID == id)
                {
                    result = msg;
                }
                if (result == null)
                {
                    result = FindMessage(msg.Replies, id);
                }
                if (result != null) break;
            }
            return result;
        }

        /// <summary>
        ///     Finds the anchestor/parent of a Message.
        /// </summary>
        /// <param name="messages">The Messages.</param>
        /// <param name="id">The Message ID.</param>
        /// <returns>The anchestor Message or null.</returns>
        private static Message FindAnchestor(IEnumerable<Message> messages, int id)
        {
            Message result = null;
            foreach (var msg in messages)
            {
                for (var k = 0; k < msg.Replies.Length; k++)
                {
                    if (msg.Replies[k].ID == id)
                    {
                        result = msg;
                        break;
                    }
                    if (result == null)
                    {
                        result = FindAnchestor(msg.Replies, id);
                    }
                }
                if (result != null) break;
            }
            return result;
        }

        /// <summary>
        ///     Removes a Message.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="page">The Page.</param>
        /// <param name="id">The ID of the Message to remove.</param>
        /// <param name="removeReplies">A value specifying whether or not to remove the replies.</param>
        /// <returns>True if the Message is removed successfully.</returns>
        private bool RemoveMessage(IDbTransaction transaction, PageInfo page, int id, bool removeReplies)
        {
            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var messages = GetMessages(transaction, page);
            if (messages == null) return false;
            var message = FindMessage(messages, id);
            if (message == null) return false;
            var parent = FindAnchestor(messages, id);
            var parentId = parent != null ? parent.ID : -1;

            UnindexMessage(page, message.ID, message.Subject, message.DateTime, message.Body, transaction);

            if (removeReplies)
            {
                // Recursively remove all replies BEFORE removing parent (depth-first)
                foreach (var reply in message.Replies)
                {
                    if (!RemoveMessage(transaction, page, reply.ID, true)) return false;
                }
            }

            // Remove this message
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("Message");
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "Id", WhereOperator.Equals, "Id");

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.Int16, "Id", (short)id));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (!removeReplies && rows == 1)
            {
                // Update replies' parent id

                query = queryBuilder.Update("Message", new[] { "Parent" }, new[] { "NewParent" });
                query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
                query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
                query = queryBuilder.AndWhere(query, "Parent", WhereOperator.Equals, "OldParent");

                parameters = new List<Parameter>(4);
                if (parentId != -1) parameters.Add(new Parameter(ParameterType.Int16, "NewParent", parentId));
                else parameters.Add(new Parameter(ParameterType.Int16, "NewParent", DBNull.Value));
                parameters.Add(new Parameter(ParameterType.String, "Page", name));
                parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
                parameters.Add(new Parameter(ParameterType.Int16, "OldParent", (short)id));

                command = builder.GetCommand(transaction, query, parameters);

                rows = ExecuteNonQuery(command, false);
            }

            return rows > 0;
        }

        /// <summary>
        ///     Removes a Message.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <param name="id">The ID of the Message to remove.</param>
        /// <param name="removeReplies">A value specifying whether or not to remove the replies.</param>
        /// <returns>True if the Message is removed successfully.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="id" /> is less than zero.</exception>
        public bool RemoveMessage(PageInfo page, int id, bool removeReplies)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Invalid ID");

            // 1. If removeReplies, recursively delete all messages with parent == id
            //    Else remove current message, updating all replies' parent id (set to this message's parent or to NULL)
            // 2. If removeReplies, unindex the whole message tree
            //    Else unindex only this message

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var done = RemoveMessage(transaction, page, id, removeReplies);

            if (done) CommitTransaction(transaction);
            else RollbackTransaction(transaction);

            return done;
        }

        /// <summary>
        ///     Modifies a Message.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <param name="id">The ID of the Message to modify.</param>
        /// <param name="username">The Username.</param>
        /// <param name="subject">The Subject.</param>
        /// <param name="dateTime">The Date/Time.</param>
        /// <param name="body">The Body.</param>
        /// <returns>True if the Message is modified successfully.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="page" />, <paramref name="username" />,
        ///     <paramref name="subject" /> or <paramref name="body" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="id" /> is less than zero.</exception>
        /// <exception cref="ArgumentException">If <paramref name="username" /> or <paramref name="subject" /> are empty.</exception>
        public bool ModifyMessage(PageInfo page, int id, string username, string subject, DateTime dateTime, string body)
        {
            if (page == null) throw new ArgumentNullException(nameof(page));
            if (id < 0) throw new ArgumentOutOfRangeException(nameof(id), "Invalid Message ID");
            if (username == null) throw new ArgumentNullException(nameof(username));
            if (username.Length == 0) throw new ArgumentException("Username cannot be empty", nameof(username));
            if (subject == null) throw new ArgumentNullException(nameof(subject));
            if (subject.Length == 0) throw new ArgumentException("Subject cannot be empty", nameof(subject));
            if (body == null) throw new ArgumentNullException(nameof(body)); // body can be empty

            string nspace, name;
            NameTools.ExpandFullName(page.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var messages = GetMessages(transaction, page);
            if (messages == null)
            {
                RollbackTransaction(transaction);
                return false;
            }
            var oldMessage = FindMessage(messages, id);

            if (oldMessage == null)
            {
                RollbackTransaction(transaction);
                return false;
            }

            UnindexMessage(page, oldMessage.ID, oldMessage.Subject, oldMessage.DateTime, oldMessage.Body, transaction);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("Message", new[] { "Username", "Subject", "DateTime", "Body" },
                new[] { "Username", "Subject", "DateTime", "Body" });
            query = queryBuilder.Where(query, "Page", WhereOperator.Equals, "Page");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.AndWhere(query, "Id", WhereOperator.Equals, "Id");

            var parameters = new List<Parameter>(7);
            parameters.Add(new Parameter(ParameterType.String, "Username", username));
            parameters.Add(new Parameter(ParameterType.String, "Subject", subject));
            parameters.Add(new Parameter(ParameterType.DateTime, "DateTime", dateTime));
            parameters.Add(new Parameter(ParameterType.String, "Body", body));
            parameters.Add(new Parameter(ParameterType.String, "Page", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));
            parameters.Add(new Parameter(ParameterType.Int16, "Id", id));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1)
            {
                IndexMessage(page, id, subject, dateTime, body, transaction);
                CommitTransaction(transaction);
                return true;
            }
            RollbackTransaction(transaction);
            return false;
        }

        /// <summary>
        ///     Gets all the Navigation Paths in a Namespace.
        /// </summary>
        /// <param name="nspace">The Namespace.</param>
        /// <returns>All the Navigation Paths, sorted by name.</returns>
        public IList<NavigationPath> GetNavigationPaths(NamespaceInfo nspace)
        {
            var nspaceName = nspace != null ? nspace.Name : "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("NavigationPath", new[] { "Name", "Namespace", "Page" });
            query = queryBuilder.Where(query, "Namespace", WhereOperator.Equals, "Namespace");
            query = queryBuilder.OrderBy(query, new[] { "Namespace", "Name", "Number" },
                new[] { Ordering.Asc, Ordering.Asc, Ordering.Asc });

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspaceName));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<NavigationPath>(10);

                var prevName = "|||";
                string name;
                var actualNamespace = "";
                var pages = new List<string>(10);

                while (reader.Read())
                {
                    name = reader["Name"] as string;

                    if (name != prevName)
                    {
                        actualNamespace = reader["Namespace"] as string;

                        if (prevName != "|||")
                        {
                            result[result.Count - 1].Pages = pages.ToArray();
                            pages.Clear();
                        }

                        result.Add(new NavigationPath(NameTools.GetFullName(actualNamespace, name), this));
                    }

                    prevName = name;
                    pages.Add(NameTools.GetFullName(actualNamespace, reader["Page"] as string));
                }

                if (result.Count > 0)
                {
                    result[result.Count - 1].Pages = pages.ToArray();
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Adds a new Navigation Path.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="nspace">The target namespace (<c>null</c> for the root).</param>
        /// <param name="name">The Name of the Path.</param>
        /// <param name="pages">The Pages array.</param>
        /// <returns>The correct <see cref="T:NavigationPath" /> object.</returns>
        private NavigationPath AddNavigationPath(IDbTransaction transaction, string nspace, string name, PageInfo[] pages)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            string query, finalQuery = "";
            var parameters = new List<Parameter>(3 * pages.Length);
            var count = 0;
            string countString;

            foreach (var page in pages)
            {
                countString = count.ToString();

                query = queryBuilder.InsertInto("NavigationPath", new[] { "Name", "Namespace", "Page", "Number" },
                    new[]
                    {"Name" + countString, "Namespace" + countString, "Page" + countString, "Number" + countString});

                parameters.Add(new Parameter(ParameterType.String, "Name" + countString, name));
                parameters.Add(new Parameter(ParameterType.String, "Namespace" + countString, nspace));
                parameters.Add(new Parameter(ParameterType.String, "Page" + countString,
                    NameTools.GetLocalName(page.FullName)));
                parameters.Add(new Parameter(ParameterType.Int32, "Number" + countString, (short)count));

                finalQuery = queryBuilder.AppendForBatch(finalQuery, query);

                count++;
            }

            var command = builder.GetCommand(transaction, finalQuery, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == pages.Length)
            {
                var result = new NavigationPath(NameTools.GetFullName(nspace, name), this);
                result.Pages = Array.ConvertAll(pages, x => { return x.FullName; });
                return result;
            }
            return null;
        }

        /// <summary>
        ///     Adds a new Navigation Path.
        /// </summary>
        /// <param name="nspace">The target namespace (<c>null</c> for the root).</param>
        /// <param name="name">The Name of the Path.</param>
        /// <param name="pages">The Pages array.</param>
        /// <returns>The correct <see cref="T:NavigationPath" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> or <paramref name="pages" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name" /> or <paramref name="pages" /> are empty.</exception>
        public NavigationPath AddNavigationPath(string nspace, string name, PageInfo[] pages)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", nameof(name));
            if (pages == null) throw new ArgumentNullException(nameof(pages));
            if (pages.Length == 0) throw new ArgumentException("Pages cannot be empty");

            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            foreach (var page in pages)
            {
                if (page == null)
                {
                    RollbackTransaction(transaction);
                    throw new ArgumentNullException(nameof(pages));
                }
                if (GetPage(transaction, page.FullName) == null)
                {
                    RollbackTransaction(transaction);
                    throw new ArgumentException("Page not found", nameof(pages));
                }
            }

            var path = AddNavigationPath(transaction, nspace, name, pages);

            if (path != null) CommitTransaction(transaction);
            else RollbackTransaction(transaction);

            return path;
        }

        /// <summary>
        ///     Modifies an existing navigation path.
        /// </summary>
        /// <param name="path">The navigation path to modify.</param>
        /// <param name="pages">The new pages array.</param>
        /// <returns>The correct <see cref="T:NavigationPath" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path" /> or <paramref name="pages" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="pages" /> is empty.</exception>
        public NavigationPath ModifyNavigationPath(NavigationPath path, PageInfo[] pages)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (pages == null) throw new ArgumentNullException(nameof(pages));
            if (pages.Length == 0) throw new ArgumentException("Pages cannot be empty");

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            foreach (var page in pages)
            {
                if (page == null)
                {
                    RollbackTransaction(transaction);
                    throw new ArgumentNullException(nameof(pages));
                }
                if (GetPage(transaction, page.FullName) == null)
                {
                    RollbackTransaction(transaction);
                    throw new ArgumentException("Page not found", nameof(pages));
                }
            }

            if (RemoveNavigationPath(transaction, path))
            {
                string nspace, name;
                NameTools.ExpandFullName(path.FullName, out nspace, out name);
                if (nspace == null) nspace = "";

                var result = AddNavigationPath(transaction, nspace, name, pages);

                if (result != null)
                {
                    CommitTransaction(transaction);
                    return result;
                }
                RollbackTransaction(transaction);
                return null;
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Removes a Navigation Path.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="path">The navigation path to remove.</param>
        /// <returns><c>true</c> if the path is removed, <c>false</c> otherwise.</returns>
        private bool RemoveNavigationPath(IDbTransaction transaction, NavigationPath path)
        {
            string nspace, name;
            NameTools.ExpandFullName(path.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("NavigationPath");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows > 0;
        }

        /// <summary>
        ///     Removes a Navigation Path.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="path">The navigation path to remove.</param>
        /// <returns><c>true</c> if the path is removed, <c>false</c> otherwise.</returns>
        private bool RemoveNavigationPath(DbConnection connection, NavigationPath path)
        {
            string nspace, name;
            NameTools.ExpandFullName(path.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("NavigationPath");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");
            query = queryBuilder.AndWhere(query, "Namespace", WhereOperator.Equals, "Namespace");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Namespace", nspace));

            var command = builder.GetCommand(connection, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows > 0;
        }

        /// <summary>
        ///     Removes a Navigation Path.
        /// </summary>
        /// <param name="path">The navigation path to remove.</param>
        /// <returns><c>true</c> if the path is removed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path" /> is <c>null</c>.</exception>
        public bool RemoveNavigationPath(NavigationPath path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            string nspace, name;
            NameTools.ExpandFullName(path.FullName, out nspace, out name);
            if (nspace == null) nspace = "";

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var removed = RemoveNavigationPath(connection, path);
            CloseConnection(connection);

            return removed;
        }

        /// <summary>
        ///     Gets all the snippets.
        /// </summary>
        /// <returns>All the snippets, sorted by name.</returns>
        public IList<Snippet> GetSnippets()
        {
            if (snippetsCache == null)
            {
                lock (snippetsCacheLock)
                {
                    if (snippetsCache == null)
                    {

                        var builder = CommandBuilder;

                        var queryBuilder = QueryBuilder.NewQuery(builder);
                        var query = queryBuilder.SelectFrom("Snippet");
                        query = queryBuilder.OrderBy(query, new[] { "Name" }, new[] { Ordering.Asc });

                        var command = builder.GetCommand(connString, query, new List<Parameter>());

                        var reader = ExecuteReader(command);

                        if (reader != null)
                        {
                            var result = new List<Snippet>(10);

                            while (reader.Read())
                            {
                                result.Add(new Snippet(reader["Name"] as string, reader["Content"] as string, this));
                            }

                            CloseReader(command, reader);

                            snippetsCache = result;
                        }
                    }
                }
            }

            return snippetsCache;
        }

        /// <summary>
        ///     Adds a new snippet.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="name">The name of the snippet.</param>
        /// <param name="content">The content of the snippet.</param>
        /// <returns>The correct <see cref="T:Snippet" /> object.</returns>
        private Snippet AddSnippet(IDbTransaction transaction, string name, string content)
        {
            var builder = CommandBuilder;

            var query = QueryBuilder.NewQuery(builder)
                .InsertInto("Snippet", new[] { "Name", "Content" }, new[] { "Name", "Content" });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Content", content));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);


            if (rows == 1)
            {
                lock (snippetsCacheLock)
                {
                    snippetsCache = null;
                }

                return new Snippet(name, content, this);
            }
            return null;
        }

        /// <summary>
        ///     Adds a new snippet.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="name">The name of the snippet.</param>
        /// <param name="content">The content of the snippet.</param>
        /// <returns>The correct <see cref="T:Snippet" /> object.</returns>
        private Snippet AddSnippet(IDbConnection connection, string name, string content)
        {
            var builder = CommandBuilder;

            var query = QueryBuilder.NewQuery(builder)
                .InsertInto("Snippet", new[] { "Name", "Content" }, new[] { "Name", "Content" });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Content", content));

            var command = builder.GetCommand(connection, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1)
            {
                lock (snippetsCacheLock)
                {
                    snippetsCache = null;
                }

                return new Snippet(name, content, this);
            }
            return null;
        }

        /// <summary>
        ///     Adds a new snippet.
        /// </summary>
        /// <param name="name">The name of the snippet.</param>
        /// <param name="content">The content of the snippet.</param>
        /// <returns>The correct <see cref="T:Snippet" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> or <paramref name="content" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public Snippet AddSnippet(string name, string content)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", nameof(name));
            if (content == null) throw new ArgumentNullException(nameof(content)); // content can be empty

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var snippet = AddSnippet(connection, name, content);
            CloseConnection(connection);

            return snippet;
        }

        /// <summary>
        ///     Modifies an existing snippet.
        /// </summary>
        /// <param name="name">The name of the snippet to modify.</param>
        /// <param name="content">The content of the snippet.</param>
        /// <returns>The correct <see cref="T:Snippet" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> or <paramref name="content" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public Snippet ModifySnippet(string name, string content)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", nameof(name));
            if (content == null) throw new ArgumentNullException(nameof(content)); // content can be empty

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (RemoveSnippet(transaction, name))
            {
                var result = AddSnippet(transaction, name, content);

                if (result != null) CommitTransaction(transaction);
                else RollbackTransaction(transaction);

                return result;
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Removes a new Snippet.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="name">The Name of the Snippet to remove.</param>
        /// <returns><c>true</c> if the snippet is removed, <c>false</c> otherwise.</returns>
        private bool RemoveSnippet(IDbTransaction transaction, string name)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("Snippet");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            lock (snippetsCacheLock)
            {
                snippetsCache = null;
            }

            return rows == 1;
        }

        /// <summary>
        ///     Removes a new Snippet.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="name">The Name of the Snippet to remove.</param>
        /// <returns><c>true</c> if the snippet is removed, <c>false</c> otherwise.</returns>
        private bool RemoveSnippet(IDbConnection connection, string name)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("Snippet");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));

            var command = builder.GetCommand(connection, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            lock (snippetsCacheLock)
            {
                snippetsCache = null;
            }

            return rows == 1;
        }

        /// <summary>
        ///     Removes a new Snippet.
        /// </summary>
        /// <param name="name">The Name of the Snippet to remove.</param>
        /// <returns><c>true</c> if the snippet is removed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public bool RemoveSnippet(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", nameof(name));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var removed = RemoveSnippet(connection, name);
            CloseConnection(connection);

            lock (snippetsCacheLock)
            {
                snippetsCache = null;
            }

            return removed;
        }

        /// <summary>
        ///     Gets all the content templates.
        /// </summary>
        /// <returns>All the content templates, sorted by name.</returns>
        public ContentTemplate[] GetContentTemplates()
        {
            var builder = CommandBuilder;

            var queryBuilder = QueryBuilder.NewQuery(builder);
            var query = queryBuilder.SelectFrom("ContentTemplate");
            query = queryBuilder.OrderBy(query, new[] { "Name" }, new[] { Ordering.Asc });

            var command = builder.GetCommand(connString, query, new List<Parameter>());

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<ContentTemplate>(10);

                while (reader.Read())
                {
                    result.Add(new ContentTemplate(reader["Name"] as string, reader["Content"] as string, this));
                }

                CloseReader(command, reader);

                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        ///     Adds a new content template.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="name">The name of template.</param>
        /// <param name="content">The content of the template.</param>
        /// <returns>The correct <see cref="T:ContentTemplate" /> object.</returns>
        private ContentTemplate AddContentTemplate(IDbTransaction transaction, string name, string content)
        {
            var builder = CommandBuilder;

            var query = QueryBuilder.NewQuery(builder)
                .InsertInto("ContentTemplate", new[] { "Name", "Content" }, new[] { "Name", "Content" });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Content", content));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1)
            {
                return new ContentTemplate(name, content, this);
            }
            return null;
        }

        /// <summary>
        ///     Adds a new content template.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="name">The name of template.</param>
        /// <param name="content">The content of the template.</param>
        /// <returns>The correct <see cref="T:ContentTemplate" /> object.</returns>
        private ContentTemplate AddContentTemplate(IDbConnection connection, string name, string content)
        {
            var builder = CommandBuilder;

            var query = QueryBuilder.NewQuery(builder)
                .InsertInto("ContentTemplate", new[] { "Name", "Content" }, new[] { "Name", "Content" });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Content", content));

            var command = builder.GetCommand(connection, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1)
            {
                return new ContentTemplate(name, content, this);
            }
            return null;
        }

        /// <summary>
        ///     Adds a new content template.
        /// </summary>
        /// <param name="name">The name of template.</param>
        /// <param name="content">The content of the template.</param>
        /// <returns>The correct <see cref="T:ContentTemplate" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> or <paramref name="content" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public ContentTemplate AddContentTemplate(string name, string content)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", nameof(name));
            if (content == null) throw new ArgumentNullException(nameof(content)); // content can be empty

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var template = AddContentTemplate(connection, name, content);
            CloseConnection(connection);

            return template;
        }

        /// <summary>
        ///     Modifies an existing content template.
        /// </summary>
        /// <param name="name">The name of the template to modify.</param>
        /// <param name="content">The content of the template.</param>
        /// <returns>The correct <see cref="T:ContentTemplate" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> or <paramref name="content" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public ContentTemplate ModifyContentTemplate(string name, string content)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", nameof(name));
            if (content == null) throw new ArgumentNullException(nameof(content)); // content can be empty

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            if (RemoveContentTemplate(transaction, name))
            {
                var template = AddContentTemplate(transaction, name, content);

                if (template != null) CommitTransaction(transaction);
                else RollbackTransaction(transaction);

                return template;
            }
            RollbackTransaction(transaction);
            return null;
        }

        /// <summary>
        ///     Removes a content template.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="name">The name of the template to remove.</param>
        /// <returns><c>true</c> if the template is removed, <c>false</c> otherwise.</returns>
        private bool RemoveContentTemplate(IDbTransaction transaction, string name)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("ContentTemplate");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows == 1;
        }

        /// <summary>
        ///     Removes a content template.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="name">The name of the template to remove.</param>
        /// <returns><c>true</c> if the template is removed, <c>false</c> otherwise.</returns>
        private bool RemoveContentTemplate(IDbConnection connection, string name)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("ContentTemplate");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));

            var command = builder.GetCommand(connection, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows == 1;
        }

        /// <summary>
        ///     Removes a content template.
        /// </summary>
        /// <param name="name">The name of the template to remove.</param>
        /// <returns><c>true</c> if the template is removed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public bool RemoveContentTemplate(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", nameof(name));

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var removed = RemoveContentTemplate(connection, name);
            CloseConnection(connection);

            return removed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nspace"></param>
        /// <param name="category"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<PageInfoWithTitle> GetPagesInfoWithTitleFromCategories(string nspace, string category, int pageIndex, int pageSize)
        {
            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            List<PageInfoWithTitle> result;

            using (var command = connection.CreateCommand())
            {
                var start =
                    @"select p.Name, p.CreationDateTime, pc.Title from Page p join PageContent pc on pc.Namespace = p.Namespace and pc.Page = p.Name and Revision = -1";

                var countSql =
                    @"select count(*) from Page p join PageContent pc on pc.Namespace = p.Namespace and pc.Page = p.Name and Revision = -1";

                var orderBy = @" order by pc.Title ";

                if (pageSize != -1)
                {
                    orderBy += @"offset (@pageIndex * @pageSize) rows fetch next @pageSize rows only";
                }

                AddParameter(command, "@pageIndex", DbType.Int32, pageIndex);
                AddParameter(command, "@pageSize", DbType.Int32, pageSize);

                int count;
                string qry;

                if (!string.IsNullOrEmpty(category))
                {
                    // pages d'une catégorie spécifique
                    string catNspace;
                    string catName;
                    NameTools.ExpandFullName(category, out catNspace, out catName);

                    qry = " join CategoryBinding cb on cb.Namespace = p.Namespace and cb.Page = p.Name where cb.Category = @catName and cb.Namespace = @catNspace";
                    AddParameter(command, "@catName", DbType.String, catName);
                    AddParameter(command, "@catNspace", DbType.String, catNspace ?? string.Empty);

                    command.CommandText = countSql + qry;
                    count = Convert.ToInt32(command.ExecuteScalar());

                    command.CommandText = start + qry + orderBy;
                }
                else if (category == string.Empty)
                {
                    // pages sans catégories
                    qry = " left outer join CategoryBinding cb on cb.Namespace = p.Namespace and cb.Page = p.Name where cb.Category is null and p.Namespace = @nspace";
                    AddParameter(command, "@nspace", DbType.String, nspace);

                    command.CommandText = countSql + qry;
                    count = Convert.ToInt32(command.ExecuteScalar());

                    command.CommandText = start + qry + orderBy;
                }
                else
                {
                    // toutes les pages
                    qry = " where p.Namespace = @nspace";
                    AddParameter(command, "@nspace", DbType.String, nspace);

                    command.CommandText = countSql + qry;
                    count = Convert.ToInt32(command.ExecuteScalar());

                    command.CommandText = start + qry + orderBy;
                }

                result = Enumerable.Repeat((PageInfoWithTitle)null, count).ToList();

                using (var reader = command.ExecuteReader())
                {
                    var i = pageIndex * pageSize;
                    while (reader.Read())
                    {
                        result[i] = new PageInfoWithTitle(
                            reader.IsDBNull(2) ? null : reader.GetString(2),
                            NameTools.GetFullName(nspace, reader.GetString(0)),
                            this,
                            reader.GetDateTime(1)
                            );
                        i++;
                    }
                }
            }

            CloseConnection(connection);

            return result;
        }

        private static void AddParameter(DbCommand cmd, string name, DbType type, object value)
        {
            var param = cmd.CreateParameter();
            param.ParameterName = name;
            param.DbType = type;
            param.Value = value;
            cmd.Parameters.Add(param);
        }

        /// <summary />
        public PageContentCache GetContentCache(PageInfo pageInfo)
        {
            using (var connection = CommandBuilder.GetConnection(connString))
            {
                var row = connection.Query("SELECT FormattedContent, LinkedPages FROM PagesContentCache WHERE [Page] = @Name AND [Namespace] = @Namespace", new { pageInfo.Name, pageInfo.Namespace }).FirstOrDefault();
                if (row == null)
                    return null;

                var formattedContent = (string)row.FormattedContent;
                var links = ((string)row.LinkedPages)?.Split(',')?.ToList();

                return new PageContentCache(pageInfo, formattedContent, links);
            }
        }

        /// <summary />
        public void SetContentCache(PageInfo pageInfo, string formattedContent, IEnumerable<string> linkedPages)
        {
            using (var connection = CommandBuilder.GetConnection(connString))
            {
                var linkedPagesText = string.Join(",", linkedPages);

                connection.Execute(@"MERGE PagesContentCache WITH(HOLDLOCK) AS target USING (VALUES(@FormattedContent, @LinkedPages)) AS src(FormattedContent, LinkedPages) on target.Page = @Name AND target.Namespace = @Namespace
WHEN MATCHED THEN UPDATE SET FormattedContent = src.FormattedContent, LinkedPages = src.LinkedPages
WHEN NOT MATCHED THEN INSERT VALUES (@Name, @Namespace, src.FormattedContent, src.LinkedPages);", new { pageInfo.Name, pageInfo.Namespace, FormattedContent = formattedContent, LinkedPages = linkedPagesText });
            }
        }

        /// <summary />
        public void RemoveContentCache(PageInfo pageInfo)
        {
            using (var connection = CommandBuilder.GetConnection(connString))
            {
                connection.Execute("DELETE FROM PagesContentCache WHERE [Page] = @Name AND [Namespace] = @Namespace", new { pageInfo.Name, pageInfo.Namespace });
            }
        }

        /// <summary />
        public void PruneContentCache()
        {
            using (var connection = CommandBuilder.GetConnection(connString))
            {
                connection.Execute("DELETE FROM PagesContentCache");
            }
        }

        #endregion
    }
}