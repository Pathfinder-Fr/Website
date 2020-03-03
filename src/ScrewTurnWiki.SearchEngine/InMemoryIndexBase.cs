using System;
using System.Collections.Generic;
using System.Globalization;

namespace ScrewTurn.Wiki.SearchEngine
{
    /// <summary>
    ///     Implements a base class for the search index.
    /// </summary>
    /// <remarks>All instance and static members are <b>thread-safe</b>.</remarks>
    public abstract class InMemoryIndexBase : IInMemoryIndex
    {
        /// <summary>
        ///     The <see cref="BuildDocument" /> delegate.
        /// </summary>
        protected BuildDocument buildDocument;

        /// <summary>
        ///     Contains the index catalog.
        /// </summary>
        protected Dictionary<string, Word> catalog;

        /// <summary>
        ///     The stop words to be used while indexing new content.
        /// </summary>
        protected string[] stopWords;

        /// <summary>
        ///     Initializes a new instance of the <see cref="InMemoryIndexBase" /> class.
        /// </summary>
        public InMemoryIndexBase()
        {
            stopWords = new string[0];
            catalog = new Dictionary<string, Word>(5000);
        }

        /// <summary>
        ///     An event fired when the index is changed.
        /// </summary>
        public event EventHandler<IndexChangedEventArgs> IndexChanged;

        /// <summary>
        ///     Sets the delegate used for converting a <see cref="DumpedDocument" /> to an instance of a class implementing
        ///     <see cref="IDocument" />,
        ///     while reading index data from a permanent storage.
        /// </summary>
        /// <param name="buildDocument">The delegate (cannot be <c>null</c>).</param>
        /// <remarks>This method must be called before invoking <see cref="InitializeData" />.</remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="buildDocument" /> is <c>null</c>.</exception>
        public void SetBuildDocumentDelegate(BuildDocument buildDocument)
        {
            if (buildDocument == null) throw new ArgumentNullException("buildDocument");
            lock (this)
            {
                this.buildDocument = buildDocument;
            }
        }

        /// <summary>
        ///     Gets or sets the stop words to be used while indexing new content.
        /// </summary>
        public string[] StopWords
        {
            get
            {
                lock (this)
                {
                    return stopWords;
                }
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value", "Stop words cannot be null");
                lock (this)
                {
                    stopWords = value;
                }
            }
        }

        /// <summary>
        ///     Gets the total count of unique words.
        /// </summary>
        /// <remarks>Computing the result is <n>O(1)</n>.</remarks>
        public int TotalWords
        {
            get
            {
                lock (this)
                {
                    return catalog.Count;
                }
            }
        }

        /// <summary>
        ///     Gets the total count of documents.
        /// </summary>
        /// <remarks>
        ///     Computing the result is <b>O(n*m)</b>, where <b>n</b> is the number of
        ///     words in the index and <b>m</b> is the number of documents.
        /// </remarks>
        public int TotalDocuments
        {
            get
            {
                var docs = new List<IDocument>(100);
                lock (this)
                {
                    foreach (var pair in catalog)
                    {
                        foreach (var pair2 in pair.Value.Occurrences)
                        {
                            if (!docs.Contains(pair2.Key)) docs.Add(pair2.Key);
                        }
                    }
                }
                return docs.Count;
            }
        }

        /// <summary>
        ///     Gets the total number of occurrences (count of words in each document).
        /// </summary>
        /// <remarks>
        ///     Computing the result is <b>O(n)</b>,
        ///     where <b>n</b> is the number of words in the index.
        /// </remarks>
        public int TotalOccurrences
        {
            get
            {
                var count = 0;
                lock (this)
                {
                    foreach (var pair in catalog)
                    {
                        count += pair.Value.TotalOccurrences;
                    }
                }
                return count;
            }
        }

        /// <summary>
        ///     Completely clears the index (stop words are not affected).
        /// </summary>
        /// <param name="state">A state object that is passed to the IndexStorer SaveDate/DeleteData function.</param>
        public void Clear(object state)
        {
            lock (this)
            {
                catalog.Clear();
                OnIndexChange(null, IndexChangeType.IndexCleared, null, state);
            }
        }

        /// <summary>
        ///     Initializes index data by completely emptying the index catalog and storing the specified data.
        /// </summary>
        /// <param name="documents">The documents.</param>
        /// <param name="words">The words.</param>
        /// <param name="mappings">The mappings.</param>
        /// <remarks>The method <b>does not</b> check the consistency of the data passed as arguments.</remarks>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="documents" />, <paramref name="words" /> or
        ///     <paramref name="mappings" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="InvalidOperationException">If <see cref="M:SetBuildDocumentDelegate" /> was not called.</exception>
        public void InitializeData(DumpedDocument[] documents, DumpedWord[] words, DumpedWordMapping[] mappings)
        {
            if (documents == null) throw new ArgumentNullException("documents");
            if (words == null) throw new ArgumentNullException("words");
            if (mappings == null) throw new ArgumentNullException("mappings");

            if (buildDocument == null)
                throw new InvalidOperationException(
                    "InitializeData can be invoked only when the BuildDocument delegate is set");

            lock (this)
            {
                catalog.Clear();
                catalog = new Dictionary<string, Word>(words.Length);

                // Contains the IDs of documents that are missing
                var missingDocuments = new List<uint>(50);

                // 1. Prepare a dictionary with all documents for use in the last step
                var tempDocuments = new Dictionary<uint, IDocument>(documents.Length);
                foreach (var doc in documents)
                {
                    var builtDoc = buildDocument(doc);
                    // Null means that the document no longer exists - silently skip it
                    if (builtDoc != null)
                    {
                        tempDocuments.Add(doc.ID, builtDoc);
                    }
                    else
                    {
                        missingDocuments.Add(doc.ID);
                    }
                }

                // 2. Load words into the catalog, keeping track of them by ID in a dictionary for the next step
                var tempWords = new Dictionary<ulong, Word>(words.Length);

                // Test for hashing algorithm -- no more used since sequential IDs
                //if(words.Length > 0 && words[0].ID != Tools.HashString(words[0].Text)) {
                //	throw new InvalidOperationException("The search engine index seems to use an outdated hashing algorithm");
                //}

                foreach (var w in words)
                {
                    var word = new Word(w.ID, w.Text);
                    /*if(tempWords.ContainsKey(w.ID)) {
						string t = string.Format("CURRENT: {0}, {1} --- EXISTING: {2}", w.ID, word, tempWords[w.ID]);
						Console.WriteLine(t);
					}*/
                    tempWords.Add(w.ID, word);
                    /*if(catalog.ContainsKey(w.Text)) {
						string t = string.Format("CURRENT: {0}, {1} --- EXISTING: {2}", w.ID, word, catalog[w.Text]);
						Console.WriteLine(t);
					}*/
                    catalog.Add(w.Text, word);
                }

                // 3. Add mappings and documents
                foreach (var map in mappings)
                {
                    // HACK: Skip mappings that refer to missing documents and gracefully skip unknown words
                    if (!missingDocuments.Contains(map.DocumentID))
                    {
                        try
                        {
                            tempWords[map.WordID].AddOccurrence(tempDocuments[map.DocumentID],
                                map.FirstCharIndex, map.WordIndex, WordLocation.GetInstance(map.Location));
                        }
                        catch (KeyNotFoundException)
                        {
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Stores a document in the index.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="keywords">The document keywords, if any, an empty array or <c>null</c> otherwise.</param>
        /// <param name="content">The content of the document.</param>
        /// <param name="state">A state object that is passed to the IndexStorer SaveDate/DeleteData function.</param>
        /// <returns>The number of indexed words (including duplicates) in the document title and content.</returns>
        /// <remarks>
        ///     Indexing the content of the document is <b>O(n)</b>,
        ///     where <b>n</b> is the total number of words in the document.
        ///     If the specified document was already in the index, all the old occurrences
        ///     are deleted from the index.
        /// </remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="document" /> or <paramref name="content" /> are <c>null</c>.</exception>
        public int StoreDocument(IDocument document, IList<string> keywords, string content, object state)
        {
            if (document == null) throw new ArgumentNullException("document");
            if (keywords == null) keywords = new string[0];
            if (content == null) throw new ArgumentNullException("content");

            lock (this)
            {
                var removeChange = RemoveDocumentInternal(document);

                if (removeChange != null)
                {
                    OnIndexChange(document, IndexChangeType.DocumentRemoved, removeChange, state);
                }
            }

            keywords = Tools.CleanupKeywords(keywords);

            // When the IndexStorer handles the IndexChanged event and a document is added, the storer generates a new ID and returns it
            // via the event handler, then the in-memory index is updated (the document instance is shared across all words) - the final ID
            // is generated by the actual IndexStorer implementation (SaveData properly populates the Result field in the args)

            var dw = new List<DumpedWord>(content.Length/5);
            var dm = new List<DumpedWordMapping>(content.Length/5);
            Word tempWord = null;
            var newWords = new List<Word>(50);
            DumpedWord tempDumpedWord = null;

            var count = 0;
            var sequentialWordId = uint.MaxValue;

            // Store content words
            var words = document.Tokenize(content);
            words = Tools.RemoveStopWords(words, stopWords);

            foreach (var info in words)
            {
                dm.Add(StoreWord(info.Text, document, info.FirstCharIndex, info.WordIndex, WordLocation.Content,
                    out tempWord, out tempDumpedWord));
                if (tempDumpedWord != null && tempWord != null)
                {
                    dm[dm.Count - 1].WordID = sequentialWordId;
                    tempDumpedWord.ID = sequentialWordId;
                    dw.Add(tempDumpedWord);
                    tempWord.ID = sequentialWordId;
                    newWords.Add(tempWord);
                    sequentialWordId--;
                }
            }
            count += words.Length;

            // Store title words
            words = document.Tokenize(document.Title);
            words = Tools.RemoveStopWords(words, stopWords);

            foreach (var info in words)
            {
                dm.Add(StoreWord(info.Text, document, info.FirstCharIndex, info.WordIndex, WordLocation.Title,
                    out tempWord, out tempDumpedWord));
                if (tempDumpedWord != null && tempWord != null)
                {
                    dm[dm.Count - 1].WordID = sequentialWordId;
                    tempDumpedWord.ID = sequentialWordId;
                    dw.Add(tempDumpedWord);
                    tempWord.ID = sequentialWordId;
                    newWords.Add(tempWord);
                    sequentialWordId--;
                }
            }
            count += words.Length;

            ushort tempCount = 0;

            // Store keywords
            for (ushort i = 0; i < (ushort) keywords.Count; i++)
            {
                dm.Add(StoreWord(keywords[i], document, tempCount, i, WordLocation.Keywords, out tempWord,
                    out tempDumpedWord));
                if (tempDumpedWord != null && tempWord != null)
                {
                    dm[dm.Count - 1].WordID = sequentialWordId;
                    tempDumpedWord.ID = sequentialWordId;
                    dw.Add(tempDumpedWord);
                    tempWord.ID = sequentialWordId;
                    newWords.Add(tempWord);
                    sequentialWordId--;
                }
                tempCount += (ushort) (1 + keywords[i].Length);
            }
            count += keywords.Count;

            var result = OnIndexChange(document, IndexChangeType.DocumentAdded,
                new DumpedChange(new DumpedDocument(document), dw, dm), state);

            // Update document ID
            if (result != null && result.DocumentID.HasValue)
            {
                document.ID = result.DocumentID.Value;
            }
            else
            {
                // HACK: result is null -> index is corrupted, silently return
                return 0;
            }

            // Update word IDs in newWords
            var wordIdUpdated = false;
            foreach (var word in newWords)
            {
                wordIdUpdated = false;
                foreach (var id in result.WordIDs)
                {
                    if (id.Text == word.Text)
                    {
                        word.ID = id.ID;
                        wordIdUpdated = true;
                        break;
                    }
                }
                if (!wordIdUpdated) throw new InvalidOperationException("No ID for new word");
            }

            return count;
        }

        /// <summary>
        ///     Removes a document from the index.
        /// </summary>
        /// <param name="document">The document to remove.</param>
        /// <param name="state">A state object that is passed to the IndexStorer SaveDate/DeleteData function.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="document" /> is <c>null</c>.</exception>
        public void RemoveDocument(IDocument document, object state)
        {
            if (document == null) throw new ArgumentNullException("document");

            var dc = RemoveDocumentInternal(document);

            if (dc != null)
            {
                OnIndexChange(document, IndexChangeType.DocumentRemoved, dc, state);
            }
            // else nothing to do
        }

        /// <summary>
        ///     Performs a search in the index.
        /// </summary>
        /// <param name="parameters">The search parameters.</param>
        /// <returns>The results.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="parameters" /> is <c>null</c>.</exception>
        public SearchResultCollection Search(SearchParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");

            using (IWordFetcher fetcher = new InMemoryIndexWordFetcher(catalog))
            {
                if (parameters.DocumentTypeTags == null)
                {
                    return Tools.SearchInternal(parameters.Query, null, false, parameters.Options, fetcher);
                }
                return Tools.SearchInternal(parameters.Query, parameters.DocumentTypeTags, true, parameters.Options,
                    fetcher);
            }
        }

        /// <summary>
        ///     Takes care of firing the <see cref="IndexChanged" /> event.
        /// </summary>
        /// <param name="document">The affected document.</param>
        /// <param name="change">The change performed.</param>
        /// <param name="changeData">The dumped change data.</param>
        /// <param name="state">A state object that is passed to the IndexStorer SaveDate/DeleteData function.</param>
        /// <returns>The storage result or <c>null</c>.</returns>
        protected IndexStorerResult OnIndexChange(IDocument document, IndexChangeType change, DumpedChange changeData,
            object state)
        {
            if (IndexChanged != null)
            {
                var args = new IndexChangedEventArgs(document, change, changeData, state);
                IndexChanged(this, args);
                return args.Result;
            }
            return null;
        }

        /// <summary>
        ///     Stores a word in the catalog.
        /// </summary>
        /// <param name="wordText">The word to store.</param>
        /// <param name="document">The document the word occurs in.</param>
        /// <param name="firstCharIndex">The index of the first character of the word in the document the word occurs at.</param>
        /// <param name="wordIndex">The index of the word in the document.</param>
        /// <param name="location">The location of the word.</param>
        /// <param name="newWord">The new word, or <c>null</c>.</param>
        /// <param name="dumpedWord">The dumped word data, or <c>null</c>.</param>
        /// <returns>The dumped word mapping data.</returns>
        /// <remarks>
        ///     Storing a word in the index is <b>O(n log n)</b>,
        ///     where <b>n</b> is the number of words already in the index.
        /// </remarks>
        protected DumpedWordMapping StoreWord(string wordText, IDocument document, ushort firstCharIndex,
            ushort wordIndex,
            WordLocation location, out Word newWord, out DumpedWord dumpedWord)
        {
            wordText = wordText.ToLower(CultureInfo.InvariantCulture);

            lock (this)
            {
                Word word = null;

                if (!catalog.TryGetValue(wordText, out word))
                {
                    // Use ZERO as initial ID, update when IndexStorer has stored the word
                    // A reference to this newly-created word must be passed outside this method
                    word = new Word(0, wordText);
                    catalog.Add(wordText, word);
                    newWord = word;
                    dumpedWord = new DumpedWord(word);
                }
                else
                {
                    newWord = null;
                    dumpedWord = null;
                }

                word.AddOccurrence(document, firstCharIndex, wordIndex, location);
                return new DumpedWordMapping(word.ID, document.ID, firstCharIndex, wordIndex, location.Location);
            }
        }

        /// <summary>
        ///     Finds a document with a specified name.
        /// </summary>
        /// <param name="name">The name of the document.</param>
        /// <returns>The document or <c>null</c>.</returns>
        private IDocument FindDocument(string name)
        {
            foreach (var pair in catalog)
            {
                foreach (var pair2 in pair.Value.Occurrences)
                {
                    if (StringComparer.OrdinalIgnoreCase.Compare(pair2.Key.Name, name) == 0)
                    {
                        return pair2.Key;
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     Removes a document from the index and generates the dumped change data.
        /// </summary>
        /// <param name="document">The document to remove.</param>
        /// <returns>The dumped change data, if any, <c>null</c> otherwise.</returns>
        protected DumpedChange RemoveDocumentInternal(IDocument document)
        {
            if (document == null) throw new ArgumentNullException("document");

            // Find real document to remove by name
            document = FindDocument(document.Name);
            if (document == null)
            {
                return null;
            }

            List<DumpedWord> dw = null;
            var dm = new List<DumpedWordMapping>(1500);

            foreach (var w in catalog.Keys)
            {
                dm.AddRange(catalog[w].RemoveOccurrences(document));
            }

            // Remove all words that have no occurrences left
            var toRemove = new List<string>(50);
            foreach (var w in catalog.Keys)
            {
                if (catalog[w].TotalOccurrences == 0) toRemove.Add(w);
            }
            dw = new List<DumpedWord>(toRemove.Count);
            foreach (var w in toRemove)
            {
                dw.Add(new DumpedWord(catalog[w]));
                catalog.Remove(w);
            }

            if (dm.Count > 0 || dw.Count > 0 || document != null)
            {
                return new DumpedChange(new DumpedDocument(document), dw, dm);
            }
            return null;
        }
    }

    /// <summary>
    ///     Implements a word fetcher for use with the in-memory index.
    /// </summary>
    public class InMemoryIndexWordFetcher : IWordFetcher
    {
        private readonly Dictionary<string, Word> catalog;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:InMemoryWordFetcher" /> class.
        /// </summary>
        /// <param name="catalog">The index catalog.</param>
        public InMemoryIndexWordFetcher(Dictionary<string, Word> catalog)
        {
            if (catalog == null) throw new ArgumentNullException("catalog");

            this.catalog = catalog;
        }

        /// <summary>
        ///     Tries to get a word.
        /// </summary>
        /// <param name="text">The text of the word.</param>
        /// <param name="word">The found word, if any, <c>null</c> otherwise.</param>
        /// <returns><c>true</c> if the word is found, <c>false</c> otherwise.</returns>
        public bool TryGetWord(string text, out Word word)
        {
            lock (catalog)
            {
                return catalog.TryGetValue(text, out word);
            }
        }

        #region IDisposable Members

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Nothing to do
        }

        #endregion
    }
}