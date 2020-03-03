using Dapper;
using ScrewTurn.Wiki.AclEngine;
using ScrewTurn.Wiki.PluginFramework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Transactions;
using Change = ScrewTurn.Wiki.PluginFramework.Change;

namespace ScrewTurn.Wiki.Plugins.SqlCommon
{
    /// <summary>
    ///     Implements a base class for a SQL settings storage provider.
    /// </summary>
    public abstract class SqlSettingsStorageProviderBase : SqlStorageProviderBase, ISettingsStorageProviderV30
    {
        private const int EstimatedLogEntrySize = 100; // bytes
        private const int MaxAssemblySize = 5242880; // 5 MB
        private const int MaxParametersInQuery = 50;

        private readonly ConcurrentDictionary<string, string> cache =
            new ConcurrentDictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     Holds a value indicating whether the application was started for the first time.
        /// </summary>
        protected bool isFirstStart = false;

        /// <summary>
        ///     Gets the default users storage provider, when no value is stored in the database.
        /// </summary>
        protected abstract string DefaultUsersStorageProvider { get; }

        /// <summary>
        ///     Gets the default pages storage provider, when no value is stored in the database.
        /// </summary>
        protected abstract string DefaultPagesStorageProvider { get; }

        /// <summary>
        ///     Gets the default files storage provider, when no value is stored in the database.
        /// </summary>
        protected abstract string DefaultFilesStorageProvider { get; }

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

            AclManager = new SqlAclManager(
                StoreEntry,
                DeleteEntries,
                RenameAclResource,
                RetrieveAllAclEntries,
                RetrieveAclEntriesForResource,
                RetrieveAclEntriesForSubject);
        }

        #region ISettingsStorageProvider Members

        /// <summary>
        ///     Retrieves the value of a Setting.
        /// </summary>
        /// <param name="name">The name of the Setting.</param>
        /// <returns>The value of the Setting, or null.</returns>
        /// <exception cref="ArgumentNullException">If <b>name</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>name</b> is empty.</exception>
        public string GetSetting(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");

            return cache.GetOrAdd(name, n =>
            {
                var builder = CommandBuilder;
                var queryBuilder = new QueryBuilder(builder);

                var query = queryBuilder.SelectFrom("Setting");
                query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

                var parameters = new List<Parameter>(1);
                parameters.Add(new Parameter(ParameterType.String, "Name", n));

                var command = builder.GetCommand(connString, query, parameters);

                var reader = ExecuteReader(command);

                if (reader != null)
                {
                    string result = null;

                    if (reader.Read())
                    {
                        result = reader["Value"] as string;
                    }

                    CloseReader(command, reader);

                    // HACK: this allows to correctly initialize a fully SQL-based wiki instance without any user intervention
                    if (string.IsNullOrEmpty(result))
                    {
                        if (n == "DefaultUsersProvider") result = DefaultUsersStorageProvider;
                        if (n == "DefaultPagesProvider") result = DefaultPagesStorageProvider;
                        if (n == "DefaultFilesProvider") result = DefaultFilesStorageProvider;
                    }

                    return result;
                }

                return null;
            });
        }

        /// <summary>
        ///     Stores the value of a Setting.
        /// </summary>
        /// <param name="name">The name of the Setting.</param>
        /// <param name="value">The value of the Setting. Value cannot contain CR and LF characters, which will be removed.</param>
        /// <returns>True if the Setting is stored, false otherwise.</returns>
        /// <remarks>This method stores the Value immediately.</remarks>
        public bool SetSetting(string name, string value)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");

            // 1. Delete old value, if any
            // 2. Store new value

            // Nulls are converted to empty strings
            if (value == null) value = "";

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("Setting");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == -1)
            {
                RollbackTransaction(transaction);
                return false; // Deletion command failed (0-1 are OK)
            }

            query = queryBuilder.InsertInto("Setting",
                new[] { "Name", "Value" }, new[] { "Name", "Value" });
            parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", name));
            parameters.Add(new Parameter(ParameterType.String, "Value", value));

            command = builder.GetCommand(transaction, query, parameters);

            rows = ExecuteNonQuery(command, false);

            if (rows == 1) CommitTransaction(transaction);
            else RollbackTransaction(transaction);

            cache.AddOrUpdate(name, value, (_, __) => value);

            return rows == 1;
        }

        /// <summary>
        ///     Gets the all the setting values.
        /// </summary>
        /// <returns>All the settings.</returns>
        public IDictionary<string, string> GetAllSettings()
        {
            var builder = CommandBuilder;

            // Sorting order is not relevant
            var query = QueryBuilder.NewQuery(builder).SelectFrom("Setting");

            var command = builder.GetCommand(connString, query, new List<Parameter>());

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new Dictionary<string, string>(50);

                while (reader.Read())
                {
                    result.Add(reader["Name"] as string, reader["Value"] as string);
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary />
        public string[] GetIncomingLinks(string page)
        {
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }
            if (page.Length == 0)
            {
                throw new ArgumentException("Page cannot be empty", "page");
            }
            var commandBuilder = CommandBuilder;
            var queryBuilder = new QueryBuilder(commandBuilder);
            string[] strArrays = { "Source" };
            var str = queryBuilder.SelectFrom("OutgoingLink", strArrays);
            str = queryBuilder.Where(str, "Destination", WhereOperator.Equals, "Destination");
            string[] strArrays1 = { "Source" };
            str = queryBuilder.OrderBy(str, strArrays1, new Ordering[1]);
            var parameters = new List<Parameter>(1)
            {
                new Parameter(ParameterType.String, "Destination", page)
            };
            var command = commandBuilder.GetCommand(connString, str, parameters);
            var dbDataReaders = ExecuteReader(command);
            if (dbDataReaders == null)
            {
                return null;
            }
            var strs = new List<string>(20);
            while (dbDataReaders.Read())
            {
                strs.Add(dbDataReaders.GetString(0));
            }
            CloseReader(command, dbDataReaders);
            return strs.ToArray();
        }

        /// <summary>
        ///     Starts a Bulk update of the Settings so that a bulk of settings can be set before storing them.
        /// </summary>
        public void BeginBulkUpdate()
        {
            // Do nothing - currently not supported
        }

        /// <summary>
        ///     Ends a Bulk update of the Settings and stores the settings.
        /// </summary>
        public void EndBulkUpdate()
        {
            // Do nothing - currently not supported
        }

        /// <summary>
        ///     Converts an <see cref="T:EntryType" /> to its character representation.
        /// </summary>
        /// <param name="type">The <see cref="T:EntryType" />.</param>
        /// <returns>Th haracter representation.</returns>
        private static char EntryTypeToChar(EntryType type)
        {
            switch (type)
            {
                case EntryType.Error:
                    return 'E';
                case EntryType.Warning:
                    return 'W';
                case EntryType.General:
                    return 'G';
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Converts the character representation of an <see cref="T:EntryType" /> back to the enumeration value.
        /// </summary>
        /// <param name="c">The character representation.</param>
        /// <returns>The<see cref="T:EntryType" />.</returns>
        private static EntryType EntryTypeFromChar(char c)
        {
            switch (char.ToUpperInvariant(c))
            {
                case 'E':
                    return EntryType.Error;
                case 'W':
                    return EntryType.Warning;
                case 'G':
                    return EntryType.General;
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Sanitizes a stiring from all unfriendly characters.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>The sanitized result.</returns>
        private static string Sanitize(string input)
        {
            var sb = new StringBuilder(input);
            sb.Replace("<", "&lt;");
            sb.Replace(">", "&gt;");
            return sb.ToString();
        }

        /// <summary>
        ///     Records a message to the System Log.
        /// </summary>
        /// <param name="message">The Log Message.</param>
        /// <param name="entryType">The Type of the Entry.</param>
        /// <param name="user">The User.</param>
        /// <remarks>
        ///     This method <b>should not</b> write messages to the Log using the method IHost.LogEntry.
        ///     This method should also never throw exceptions (except for parameter validation).
        /// </remarks>
        /// <exception cref="ArgumentNullException">If <b>message</b> or <b>user</b> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>message</b> or <b>user</b> are empty.</exception>
        public void LogEntry(string message, EntryType entryType, string user)
        {
            if (message == null) throw new ArgumentNullException("message");
            if (message.Length == 0) throw new ArgumentException("Message cannot be empty", "message");
            if (user == null) throw new ArgumentNullException("user");
            if (user.Length == 0) throw new ArgumentException("User cannot be empty", "user");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.InsertInto("Log",
                new[] { "DateTime", "EntryType", "User", "Message" }, new[] { "DateTime", "EntryType", "User", "Message" });

            var parameters = new List<Parameter>(4);
            parameters.Add(new Parameter(ParameterType.DateTime, "DateTime", DateTime.Now));
            parameters.Add(new Parameter(ParameterType.Char, "EntryType", EntryTypeToChar(entryType)));
            parameters.Add(new Parameter(ParameterType.String, "User", Sanitize(user)));
            parameters.Add(new Parameter(ParameterType.String, "Message", Sanitize(message)));

            try
            {
                var command = builder.GetCommand(connString, query, parameters);

                ExecuteNonQuery(command, true);

                // No transaction - accurate log sizing is not really a concern

                var logSize = LogSize;
                if (logSize > int.Parse(host.GetSettingValue(SettingName.MaxLogSize)))
                {
                    CutLog((int)(logSize * 0.75));
                }
            }
            catch
            {
            }
        }

        /// <summary>
        ///     Reduces the size of the Log to the specified size (or less).
        /// </summary>
        /// <param name="size">The size to shrink the log to (in bytes).</param>
        private void CutLog(int size)
        {
            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectCountFrom("Log");

            var command = builder.GetCommand(transaction, query, new List<Parameter>());

            var rows = ExecuteScalar(command, -1, false);

            if (rows == -1)
            {
                RollbackTransaction(transaction);
                return;
            }

            var estimatedSize = rows * EstimatedLogEntrySize;

            if (size < estimatedSize)
            {
                var difference = estimatedSize - size;
                var entriesToDelete = difference / EstimatedLogEntrySize;
                // Add 10% to avoid 1-by-1 deletion when adding new entries
                entriesToDelete += entriesToDelete / 10;

                if (entriesToDelete > 0)
                {
                    // This code is not optimized, but it surely works in most DBMS
                    query = queryBuilder.SelectFrom("Log", new[] { "Id" });
                    query = queryBuilder.OrderBy(query, new[] { "Id" }, new[] { Ordering.Asc });

                    command = builder.GetCommand(transaction, query, new List<Parameter>());

                    var reader = ExecuteReader(command);

                    var ids = new List<int>(entriesToDelete);

                    if (reader != null)
                    {
                        while (reader.Read() && ids.Count < entriesToDelete)
                        {
                            ids.Add((int)reader["Id"]);
                        }

                        CloseReader(reader);
                    }

                    if (ids.Count > 0)
                    {
                        // Given that the IDs to delete can be many, the query is split in many chunks, each one deleting 50 items
                        // This works-around the problem of too many parameters in a RPC call of SQL Server
                        // See also CutRecentChangesIfNecessary

                        for (var chunk = 0; chunk <= ids.Count / MaxParametersInQuery; chunk++)
                        {
                            query = queryBuilder.DeleteFrom("Log");
                            var parms = new List<string>(MaxParametersInQuery);
                            var parameters = new List<Parameter>(MaxParametersInQuery);

                            for (var i = chunk * MaxParametersInQuery;
                                i < Math.Min(ids.Count, (chunk + 1) * MaxParametersInQuery);
                                i++)
                            {
                                parms.Add("P" + i);
                                parameters.Add(new Parameter(ParameterType.Int32, parms[parms.Count - 1], ids[i]));
                            }

                            query = queryBuilder.WhereIn(query, "Id", parms.ToArray());

                            command = builder.GetCommand(transaction, query, parameters);

                            if (ExecuteNonQuery(command, false) < 0)
                            {
                                RollbackTransaction(transaction);
                                return;
                            }
                        }
                    }

                    CommitTransaction(transaction);
                }
            }
        }

        /// <summary>
        ///     Gets all the Log Entries, sorted by date/time (oldest to newest).
        /// </summary>
        /// <returns>The Log Entries.</returns>
        public LogEntry[] GetLogEntries()
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("Log", new[] { "DateTime", "EntryType", "User", "Message" });
            query = queryBuilder.OrderBy(query, new[] { "DateTime" }, new[] { Ordering.Asc });

            var command = builder.GetCommand(connString, query, new List<Parameter>());

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<LogEntry>(100);

                while (reader.Read())
                {
                    result.Add(new LogEntry(EntryTypeFromChar((reader["EntryType"] as string)[0]),
                        (DateTime)reader["DateTime"], reader["Message"] as string, reader["User"] as string));
                }

                CloseReader(command, reader);

                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        ///     Clear the Log.
        /// </summary>
        public void ClearLog()
        {
            var builder = CommandBuilder;

            var query = QueryBuilder.NewQuery(builder).DeleteFrom("Log");

            var command = builder.GetCommand(connString, query, new List<Parameter>());

            ExecuteNonQuery(command);
        }

        /// <summary>
        ///     Gets the current size of the Log, in KB.
        /// </summary>
        public int LogSize
        {
            get
            {
                var builder = CommandBuilder;
                var queryBuilder = new QueryBuilder(builder);

                var query = queryBuilder.SelectCountFrom("Log");

                var command = builder.GetCommand(connString, query, new List<Parameter>());

                var rows = ExecuteScalar(command, -1);

                if (rows == -1) return 0;

                var estimatedSize = rows * EstimatedLogEntrySize;

                return estimatedSize / 1024;
            }
        }

        /// <summary>
        /// Gets a meta-data item's content.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tag">The tag that specifies the context (usually the namespace).</param>
        /// <returns>The content.</returns>
        public string GetMetaDataItem(MetaDataItem item, string tag) => GetMetaDataItem(item, tag, false);

        /// <summary>
        ///     Gets a meta-data item's content.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tag">The tag that specifies the context (usually the namespace).</param>
        /// <param name="nullTagIfMissing">Indicates that the method should return the <c>null</c> tag content when <paramref name="tag"/> si set but no content is set.</param>
        /// <returns>The content.</returns>
        public string GetMetaDataItem(MetaDataItem item, string tag, bool nullTagIfMissing)
        {
            using (var conn = OpenDbConnection())
            {
                var result = conn.Query("SELECT Data FROM MetaDataItem WHERE Name = @Name AND Tag = @Tag", new { Name = item.ToString(), Tag = tag ?? string.Empty }).FirstOrDefault()?.Data;
                if (result == null && nullTagIfMissing && !string.IsNullOrEmpty(tag))
                    return conn.Query("SELECT Data FROM MetaDataItem WHERE Name = @Name AND Tag = @Tag", new { Name = item.ToString(), Tag = string.Empty }).FirstOrDefault()?.Data ?? string.Empty;
                return result ?? string.Empty;
            }
        }

        /// <summary>
        ///     Sets a meta-data items' content.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="tag">The tag that specifies the context (usually the namespace).</param>
        /// <param name="content">The content.</param>
        /// <returns><c>true</c> if the content is set, <c>false</c> otherwise.</returns>
        public bool SetMetaDataItem(MetaDataItem item, string tag, string content)
        {
            using (var transaction = new TransactionScope())
            using (var conn = OpenDbConnection())
            {
                conn.Execute("DELETE FROM MetaDataItem WHERE Name = @Name AND Tag = @Tag", new { Name = item.ToString(), Tag = tag ?? string.Empty });
                if (content != null)
                {
                    conn.Execute("INSERT INTO MetaDataItem (Name, Tag, Data) VALUES (@Name, @Tag, @Data)", new { Name = item.ToString(), Tag = tag ?? string.Empty, Data = content });
                }

                transaction.Complete();
                return true;
            }
        }

        /// <summary>
        ///     Converts a <see cref="T:ScrewTurn.Wiki.PluginFramework.Change" /> to its character representation.
        /// </summary>
        /// <param name="change">The <see cref="T:ScrewTurn.Wiki.PluginFramework.Change" />.</param>
        /// <returns>The character representation.</returns>
        private static char RecentChangeToChar(Change change)
        {
            switch (change)
            {
                case Change.MessageDeleted:
                    return 'M';
                case Change.MessageEdited:
                    return 'E';
                case Change.MessagePosted:
                    return 'P';
                case Change.PageDeleted:
                    return 'D';
                case Change.PageRenamed:
                    return 'N';
                case Change.PageRolledBack:
                    return 'R';
                case Change.PageUpdated:
                    return 'U';
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Converts a character representation of a <see cref="T:ScrewTurn.Wiki.PluginFramework.Change" /> back to the enum
        ///     value.
        /// </summary>
        /// <param name="c">The character representation.</param>
        /// <returns>The <see cref="T:ScrewTurn.Wiki.PluginFramework.Change" />.</returns>
        private static Change RecentChangeFromChar(char c)
        {
            switch (char.ToUpperInvariant(c))
            {
                case 'M':
                    return Change.MessageDeleted;
                case 'E':
                    return Change.MessageEdited;
                case 'P':
                    return Change.MessagePosted;
                case 'D':
                    return Change.PageDeleted;
                case 'N':
                    return Change.PageRenamed;
                case 'R':
                    return Change.PageRolledBack;
                case 'U':
                    return Change.PageUpdated;
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Gets the recent changes of the Wiki.
        /// </summary>
        /// <returns>The recent Changes, oldest to newest.</returns>
        public List<RecentChange> GetRecentChanges()
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("RecentChange", new[] { "Page", "Title", "MessageSubject", "DateTime", "User", "Change", "Description" });
            query = queryBuilder.OrderBy(query, new[] { "DateTime" }, new[] { Ordering.Asc });

            using (var command = builder.GetCommand(connString, query, new List<Parameter>()))
            {
                var reader = ExecuteReader(command);

                if (reader != null)
                {
                    var result = new List<RecentChange>(100);

                    while (reader.Read())
                    {
                        result.Add(new RecentChange(reader["Page"] as string, reader["Title"] as string,
                            GetNullableColumn(reader, "MessageSubject", ""),
                            (DateTime)reader["DateTime"], reader["User"] as string,
                            RecentChangeFromChar(((string)reader["Change"])[0]),
                            GetNullableColumn(reader, "Description", "")));
                    }

                    CloseReader(command, reader);

                    return result;
                }
            }

            return null;
        }

        /// <summary>
        ///     Adds a new change.
        /// </summary>
        /// <param name="page">The page name.</param>
        /// <param name="title">The page title.</param>
        /// <param name="messageSubject">The message subject (or <c>null</c>).</param>
        /// <param name="dateTime">The date/time.</param>
        /// <param name="user">The user.</param>
        /// <param name="change">The change.</param>
        /// <param name="descr">The description (optional).</param>
        /// <returns><c>true</c> if the change is saved, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>page</b>, <b>title</b> or <b>user</b> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>page</b>, <b>title</b> or <b>user</b> are empty.</exception>
        public bool AddRecentChange(string page, string title, string messageSubject, DateTime dateTime, string user,
            Change change, string descr)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (page.Length == 0) throw new ArgumentException("Page cannot be empty", "page");
            if (title == null) throw new ArgumentNullException("title");
            if (title.Length == 0) throw new ArgumentException("Title cannot be empty", "title");
            if (user == null) throw new ArgumentNullException("user");
            if (user.Length == 0) throw new ArgumentException("User cannot be empty", "user");

            var builder = CommandBuilder;

            var query = QueryBuilder.NewQuery(builder)
                .InsertInto("RecentChange",
                    new[] { "Page", "Title", "MessageSubject", "DateTime", "User", "Change", "Description" },
                    new[] { "Page", "Title", "MessageSubject", "DateTime", "User", "Change", "Description" });

            var parameters = new List<Parameter>(7);
            parameters.Add(new Parameter(ParameterType.String, "Page", page));
            parameters.Add(new Parameter(ParameterType.String, "Title", title));
            if (!string.IsNullOrEmpty(messageSubject))
                parameters.Add(new Parameter(ParameterType.String, "MessageSubject", messageSubject));
            else parameters.Add(new Parameter(ParameterType.String, "MessageSubject", DBNull.Value));
            parameters.Add(new Parameter(ParameterType.DateTime, "DateTime", dateTime));
            parameters.Add(new Parameter(ParameterType.String, "User", user));
            parameters.Add(new Parameter(ParameterType.Char, "Change", RecentChangeToChar(change)));
            if (!string.IsNullOrEmpty(descr)) parameters.Add(new Parameter(ParameterType.String, "Description", descr));
            else parameters.Add(new Parameter(ParameterType.String, "Description", DBNull.Value));

            var command = builder.GetCommand(connString, query, parameters);

            var rows = ExecuteNonQuery(command);

            CutRecentChangesIfNecessary();

            return rows == 1;
        }

        /// <summary>
        ///     Cuts the recent changes if necessary.
        /// </summary>
        private void CutRecentChangesIfNecessary()
        {
            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectCountFrom("RecentChange");

            var command = builder.GetCommand(transaction, query, new List<Parameter>());

            var rows = ExecuteScalar(command, -1, false);

            var maxChanges = int.Parse(host.GetSettingValue(SettingName.MaxRecentChanges));

            if (rows > maxChanges)
            {
                // Remove 10% of old changes to avoid 1-by-1 deletion every time a change is made
                var entriesToDelete = maxChanges / 10;
                if (entriesToDelete > rows) entriesToDelete = rows;
                //entriesToDelete += entriesToDelete / 10;

                // This code is not optimized, but it surely works in most DBMS
                query = queryBuilder.SelectFrom("RecentChange", new[] { "Id" });
                query = queryBuilder.OrderBy(query, new[] { "Id" }, new[] { Ordering.Asc });

                command = builder.GetCommand(transaction, query, new List<Parameter>());

                var reader = ExecuteReader(command);

                var ids = new List<int>(entriesToDelete);

                if (reader != null)
                {
                    while (reader.Read() && ids.Count < entriesToDelete)
                    {
                        ids.Add((int)reader["Id"]);
                    }

                    CloseReader(reader);
                }

                if (ids.Count > 0)
                {
                    // Given that the IDs to delete can be many, the query is split in many chunks, each one deleting 50 items
                    // This works-around the problem of too many parameters in a RPC call of SQL Server
                    // See also CutLog

                    for (var chunk = 0; chunk <= ids.Count / MaxParametersInQuery; chunk++)
                    {
                        query = queryBuilder.DeleteFrom("RecentChange");
                        var parms = new List<string>(MaxParametersInQuery);
                        var parameters = new List<Parameter>(MaxParametersInQuery);

                        for (var i = chunk * MaxParametersInQuery;
                            i < Math.Min(ids.Count, (chunk + 1) * MaxParametersInQuery);
                            i++)
                        {
                            parms.Add("P" + i);
                            parameters.Add(new Parameter(ParameterType.Int32, parms[parms.Count - 1], ids[i]));
                        }

                        query = queryBuilder.WhereIn(query, "Id", parms.ToArray());

                        command = builder.GetCommand(transaction, query, parameters);

                        if (ExecuteNonQuery(command, false) < 0)
                        {
                            RollbackTransaction(transaction);
                            return;
                        }
                    }
                }
            }

            CommitTransaction(transaction);
        }

        /// <summary>
        ///     Lists the stored plugin assemblies.
        /// </summary>
        public string[] ListPluginAssemblies()
        {
            var builder = CommandBuilder;

            // Sort order is not relevant
            var query = QueryBuilder.NewQuery(builder).SelectFrom("PluginAssembly", new[] { "Name" });

            var command = builder.GetCommand(connString, query, new List<Parameter>());

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<string>(10);

                while (reader.Read())
                {
                    result.Add(reader["Name"] as string);
                }

                CloseReader(command, reader);

                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        ///     Stores a plugin's assembly, overwriting existing ones if present.
        /// </summary>
        /// <param name="filename">The file name of the assembly, such as "Assembly.dll".</param>
        /// <param name="assembly">The assembly content.</param>
        /// <returns><c>true</c> if the assembly is stored, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>filename</b> or <b>assembly</b> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>filename</b> or <b>assembly</b> are empty.</exception>
        public bool StorePluginAssembly(string filename, byte[] assembly)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            if (filename.Length == 0) throw new ArgumentException("Filename cannot be empty", "filename");
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (assembly.Length == 0) throw new ArgumentException("Assembly cannot be empty", "assembly");
            if (assembly.Length > MaxAssemblySize) throw new ArgumentException("Assembly is too big", "assembly");

            // 1. Delete old plugin assembly, if any
            // 2. Store new assembly

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            DeletePluginAssembly(transaction, filename);

            var query = QueryBuilder.NewQuery(builder)
                .InsertInto("PluginAssembly", new[] { "Name", "Assembly" }, new[] { "Name", "Assembly" });

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Name", filename));
            parameters.Add(new Parameter(ParameterType.ByteArray, "Assembly", assembly));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1) CommitTransaction(transaction);
            else RollbackTransaction(transaction);

            return rows == 1;
        }

        /// <summary>
        ///     Retrieves a plugin's assembly.
        /// </summary>
        /// <param name="filename">The file name of the assembly.</param>
        /// <returns>The assembly content, or <c>null</c>.</returns>
        /// <exception cref="ArgumentNullException">If <b>filename</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>filename</b> is empty.</exception>
        public byte[] RetrievePluginAssembly(string filename)
        {
            if (filename == null) throw new ArgumentNullException("filename");
            if (filename.Length == 0) throw new ArgumentException("Filename cannot be empty", "filename");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("PluginAssembly", new[] { "Assembly" });
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", filename));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                byte[] result = null;

                if (reader.Read())
                {
                    result = GetBinaryColumn(reader, "Assembly", MaxAssemblySize);
                }

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Removes a plugin's assembly.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="filename">The file name of the assembly to remove, such as "Assembly.dll".</param>
        /// <returns><c>true</c> if the assembly is removed, <c>false</c> otherwise.</returns>
        private bool DeletePluginAssembly(IDbTransaction transaction, string filename)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("PluginAssembly");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", filename));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows == 1;
        }

        /// <summary>
        ///     Removes a plugin's assembly.
        /// </summary>
        /// <param name="connection">A database connection.</param>
        /// <param name="filename">The file name of the assembly to remove, such as "Assembly.dll".</param>
        /// <returns><c>true</c> if the assembly is removed, <c>false</c> otherwise.</returns>
        private bool DeletePluginAssembly(DbConnection connection, string filename)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("PluginAssembly");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", filename));

            var command = builder.GetCommand(connection, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            return rows == 1;
        }

        /// <summary>
        ///     Removes a plugin's assembly.
        /// </summary>
        /// <param name="filename">The file name of the assembly to remove, such as "Assembly.dll".</param>
        /// <returns><c>true</c> if the assembly is removed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>filename</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>filename</b> is empty.</exception>
        public bool DeletePluginAssembly(string filename)
        {
            if (filename == null) throw new ArgumentNullException(filename);
            if (filename.Length == 0) throw new ArgumentException("Filename cannot be empty", "filename");

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);

            var deleted = DeletePluginAssembly(connection, filename);
            CloseConnection(connection);

            return deleted;
        }

        /// <summary>
        ///     Prepares the plugin status row, if necessary.
        /// </summary>
        /// <param name="transaction">A database transaction.</param>
        /// <param name="typeName">The Type name of the plugin.</param>
        private void PreparePluginStatusRow(IDbTransaction transaction, string typeName)
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectCountFrom("PluginStatus");
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", typeName));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteScalar(command, -1, false);

            if (rows == -1) return;

            if (rows == 0)
            {
                // Insert a neutral row (enabled, empty config)

                query = queryBuilder.InsertInto("PluginStatus", new[] { "Name", "Enabled", "Configuration" },
                    new[] { "Name", "Enabled", "Configuration" });

                parameters = new List<Parameter>(3);
                parameters.Add(new Parameter(ParameterType.String, "Name", typeName));
                parameters.Add(new Parameter(ParameterType.Boolean, "Enabled", true));
                parameters.Add(new Parameter(ParameterType.String, "Configuration", ""));

                command = builder.GetCommand(transaction, query, parameters);

                ExecuteNonQuery(command, false);
            }
        }

        /// <summary>
        ///     Sets the status of a plugin.
        /// </summary>
        /// <param name="typeName">The Type name of the plugin.</param>
        /// <param name="enabled">The plugin status.</param>
        /// <returns><c>true</c> if the status is stored, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>typeName</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>typeName</b> is empty.</exception>
        public bool SetPluginStatus(string typeName, bool enabled)
        {
            if (typeName == null) throw new ArgumentNullException("typeName");
            if (typeName.Length == 0) throw new ArgumentException("Type Name cannot be empty", "typeName");

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            PreparePluginStatusRow(transaction, typeName);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("PluginStatus", new[] { "Enabled" }, new[] { "Enabled" });
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.Boolean, "Enabled", enabled));
            parameters.Add(new Parameter(ParameterType.String, "Name", typeName));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1) CommitTransaction(transaction);
            else RollbackTransaction(transaction);

            return rows == 1;
        }

        /// <summary>
        ///     Gets the status of a plugin.
        /// </summary>
        /// <param name="typeName">The Type name of the plugin.</param>
        /// <returns>The status (<c>false</c> for disabled, <c>true</c> for enabled), or <c>true</c> if no status is found.</returns>
        /// <exception cref="ArgumentNullException">If <b>typeName</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>typeName</b> is empty.</exception>
        public bool GetPluginStatus(string typeName)
        {
            if (typeName == null) throw new ArgumentNullException("typeName");
            if (typeName.Length == 0) throw new ArgumentException("Type Name cannot be empty", "typeName");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("PluginStatus", new[] { "Enabled" });
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", typeName));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            bool? enabled = null;

            if (reader != null && reader.Read())
            {
                if (!IsDBNull(reader, "Enabled")) enabled = (bool)reader["Enabled"];
            }
            CloseReader(command, reader);

            if (enabled.HasValue) return enabled.Value;
            if (typeName == "ScrewTurn.Wiki.UsersStorageProvider" ||
                typeName == "ScrewTurn.Wiki.PagesStorageProvider" ||
                typeName == "ScrewTurn.Wiki.FilesStorageProvider") return false;
            return true;
        }

        /// <summary>
        ///     Sets the configuration of a plugin.
        /// </summary>
        /// <param name="typeName">The Type name of the plugin.</param>
        /// <param name="config">The configuration.</param>
        /// <returns><c>true</c> if the configuration is stored, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>typeName</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>typeName</b> is empty.</exception>
        public bool SetPluginConfiguration(string typeName, string config)
        {
            if (typeName == null) throw new ArgumentNullException("typeName");
            if (typeName.Length == 0) throw new ArgumentException("Type Name cannot be empty", "typeName");

            if (config == null) config = "";

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            PreparePluginStatusRow(transaction, typeName);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("PluginStatus", new[] { "Configuration" }, new[] { "Configuration" });
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Configuration", config));
            parameters.Add(new Parameter(ParameterType.String, "Name", typeName));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == 1) CommitTransaction(transaction);
            else RollbackTransaction(transaction);

            return rows == 1;
        }

        /// <summary>
        ///     Gets the configuration of a plugin.
        /// </summary>
        /// <param name="typeName">The Type name of the plugin.</param>
        /// <returns>The plugin configuration, or <b>String.Empty</b>.</returns>
        /// <exception cref="ArgumentNullException">If <b>typeName</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>typeName</b> is empty.</exception>
        public string GetPluginConfiguration(string typeName)
        {
            if (typeName == null) throw new ArgumentNullException("typeName");
            if (typeName.Length == 0) throw new ArgumentException("Type Name cannot be empty", "typeName");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("PluginStatus", new[] { "Configuration" });
            query = queryBuilder.Where(query, "Name", WhereOperator.Equals, "Name");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Name", typeName));

            var command = builder.GetCommand(connString, query, parameters);

            var result = ExecuteScalar(command, "");

            return result;
        }

        /// <summary>
        ///     Gets the ACL Manager instance.
        /// </summary>
        public IAclManager AclManager { get; private set; }

        /// <summary>
        ///     Stores the outgoing links of a page, overwriting existing data.
        /// </summary>
        /// <param name="page">The full name of the page.</param>
        /// <param name="outgoingLinks">The full names of the pages that <b>page</b> links to.</param>
        /// <returns><c>true</c> if the outgoing links are stored, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>page</b> or <b>outgoingLinks</b> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>page</b> or <b>outgoingLinks</b> are empty.</exception>
        public bool StoreOutgoingLinks(string page, string[] outgoingLinks)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (page.Length == 0) throw new ArgumentException("Page cannot be empty", "page");
            if (outgoingLinks == null) throw new ArgumentNullException("outgoingLinks");

            foreach (var link in outgoingLinks)
            {
                if (link == null) throw new ArgumentNullException("outgoingLinks");
                if (link.Length == 0) throw new ArgumentException("Link cannot be empty", "outgoingLinks");
            }

            // 1. Delete old values, if any
            // 2. Store new values

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("OutgoingLink");
            query = queryBuilder.Where(query, "Source", WhereOperator.Equals, "Source");

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Source", page));

            var command = builder.GetCommand(transaction, query, parameters);

            if (ExecuteNonQuery(command, false) < 0)
            {
                RollbackTransaction(transaction);
                return false;
            }

            foreach (var link in outgoingLinks)
            {
                query = queryBuilder.InsertInto("OutgoingLink", new[] { "Source", "Destination" },
                    new[] { "Source", "Destination" });

                parameters = new List<Parameter>(2);
                parameters.Add(new Parameter(ParameterType.String, "Source", page));
                parameters.Add(new Parameter(ParameterType.String, "Destination", link));

                command = builder.GetCommand(transaction, query, parameters);

                var rows = ExecuteNonQuery(command, false);

                if (rows != 1)
                {
                    RollbackTransaction(transaction);
                    return false;
                }
            }

            CommitTransaction(transaction);
            return true;
        }

        /// <summary>
        ///     Gets the outgoing links of a page.
        /// </summary>
        /// <param name="page">The full name of the page.</param>
        /// <returns>The outgoing links.</returns>
        /// <exception cref="ArgumentNullException">If <b>page</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>page</b> is empty.</exception>
        public string[] GetOutgoingLinks(string page)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (page.Length == 0) throw new ArgumentException("Page cannot be empty", "page");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.SelectFrom("OutgoingLink", new[] { "Destination" });
            query = queryBuilder.Where(query, "Source", WhereOperator.Equals, "Source");
            query = queryBuilder.OrderBy(query, new[] { "Destination" }, new[] { Ordering.Asc });

            var parameters = new List<Parameter>(1);
            parameters.Add(new Parameter(ParameterType.String, "Source", page));

            var command = builder.GetCommand(connString, query, parameters);

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new List<string>(20);

                while (reader.Read())
                {
                    result.Add(reader["Destination"] as string);
                }

                CloseReader(command, reader);

                return result.ToArray();
            }
            return null;
        }

        /// <summary>
        ///     Gets all the outgoing links stored.
        /// </summary>
        /// <returns>The outgoing links, in a dictionary in the form page-&gt;outgoing_links.</returns>
        public IDictionary<string, string[]> GetAllOutgoingLinks()
        {
            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            // Explicit columns in order to allow usage of GROUP BY
            var query = queryBuilder.SelectFrom("OutgoingLink", new[] { "Source", "Destination" });
            query = queryBuilder.GroupBy(query, new[] { "Source", "Destination" });

            var command = builder.GetCommand(connString, query, new List<Parameter>());

            var reader = ExecuteReader(command);

            if (reader != null)
            {
                var result = new Dictionary<string, string[]>(100);

                var prevSource = "|||";
                string source = null;
                var destinations = new List<string>(20);

                while (reader.Read())
                {
                    source = reader["Source"] as string;

                    if (source != prevSource)
                    {
                        if (prevSource != "|||")
                        {
                            result.Add(prevSource, destinations.ToArray());
                            destinations.Clear();
                        }
                    }

                    prevSource = source;
                    destinations.Add(reader["Destination"] as string);
                }

                result.Add(prevSource, destinations.ToArray());

                CloseReader(command, reader);

                return result;
            }
            return null;
        }

        /// <summary>
        ///     Deletes the outgoing links of a page and all the target links that include the page.
        /// </summary>
        /// <param name="page">The full name of the page.</param>
        /// <returns><c>true</c> if the links are deleted, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>page</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>page</b> is empty.</exception>
        public bool DeleteOutgoingLinks(string page)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (page.Length == 0) throw new ArgumentException("Page cannot be empty", "page");

            var builder = CommandBuilder;
            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.DeleteFrom("OutgoingLink");
            query = queryBuilder.Where(query, "Source", WhereOperator.Equals, "Source");
            query = queryBuilder.OrWhere(query, "Destination", WhereOperator.Equals, "Destination");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "Source", page));
            parameters.Add(new Parameter(ParameterType.String, "Destination", page));

            var command = builder.GetCommand(connString, query, parameters);

            var rows = ExecuteNonQuery(command);

            return rows > 0;
        }

        /// <summary>
        ///     Updates all outgoing links data for a page rename.
        /// </summary>
        /// <param name="oldName">The old page name.</param>
        /// <param name="newName">The new page name.</param>
        /// <returns><c>true</c> if the data is updated, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>oldName</b> or <b>newName</b> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>oldName</b> or <b>newName</b> are empty.</exception>
        public bool UpdateOutgoingLinksForRename(string oldName, string newName)
        {
            if (oldName == null) throw new ArgumentNullException("oldName");
            if (oldName.Length == 0) throw new ArgumentException("Old Name cannot be empty", "oldName");
            if (newName == null) throw new ArgumentNullException("newName");
            if (newName.Length == 0) throw new ArgumentException("New Name cannot be empty", "newName");

            // 1. Rename sources
            // 2. Rename destinations

            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("OutgoingLink", new[] { "Source" }, new[] { "NewSource" });
            query = queryBuilder.Where(query, "Source", WhereOperator.Equals, "OldSource");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "NewSource", newName));
            parameters.Add(new Parameter(ParameterType.String, "OldSource", oldName));

            var command = builder.GetCommand(transaction, query, parameters);

            var rows = ExecuteNonQuery(command, false);

            if (rows == -1)
            {
                RollbackTransaction(transaction);
                return false;
            }

            var somethingUpdated = rows > 0;

            query = queryBuilder.Update("OutgoingLink", new[] { "Destination" }, new[] { "NewDestination" });
            query = queryBuilder.Where(query, "Destination", WhereOperator.Equals, "OldDestination");

            parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "NewDestination", newName));
            parameters.Add(new Parameter(ParameterType.String, "OldDestination", oldName));

            command = builder.GetCommand(transaction, query, parameters);

            rows = ExecuteNonQuery(command, false);

            if (rows >= 0) CommitTransaction(transaction);
            else RollbackTransaction(transaction);

            return somethingUpdated || rows > 0;
        }

        /// <summary>
        ///     Determines whether the application was started for the first time.
        /// </summary>
        /// <returns><c>true</c> if the application was started for the first time, <c>false</c> otherwise.</returns>
        public bool IsFirstApplicationStart()
        {
            return isFirstStart;
        }

        #endregion

        #region AclManager backend methods

        private List<AclEntry> _allEntries = new List<AclEntry>();
        private readonly object _aclEntriesLock = new object();
        private readonly ConcurrentDictionary<string, List<AclEntry>> _aclEntriesByResource = new ConcurrentDictionary<string, List<AclEntry>>();
        private readonly ConcurrentDictionary<string, List<AclEntry>> _aclEntriesBySubject = new ConcurrentDictionary<string, List<AclEntry>>();

        /// <summary>
        ///     Converts a <see cref="T:Value" /> to its corresponding character representation.
        /// </summary>
        /// <param name="value">The <see cref="T:Value" />.</param>
        /// <returns>The character representation.</returns>
        private static char AclEntryValueToChar(Value value)
        {
            switch (value)
            {
                case Value.Grant:
                    return 'G';
                case Value.Deny:
                    return 'D';
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Converts a character representation of a <see cref="T:Value" /> back to the enum value.
        /// </summary>
        /// <param name="c">The character representation.</param>
        /// <returns>The <see cref="T:Value" />.</returns>
        private static Value AclEntryValueFromChar(char c)
        {
            switch (char.ToUpperInvariant(c))
            {
                case 'G':
                    return Value.Grant;
                case 'D':
                    return Value.Deny;
                default:
                    throw new NotSupportedException();
            }
        }

        /// <summary>
        ///     Retrieves all ACL entries.
        /// </summary>
        /// <returns>The ACL entries.</returns>
        private IEnumerable<AclEntry> RetrieveAllAclEntries()
        {
            if (_allEntries == null)
            {
                lock (_aclEntriesLock)
                {
                    if (_allEntries == null)
                    {
                        var result = new List<AclEntry>(50);
                        using (var connection = CommandBuilder.GetConnection(connString))
                        {
                            foreach (var row in connection.Query("SELECT Resource, Action, Subject, Value FROM AclEntry"))
                            {
                                result.Add(new AclEntry(row.Resource, row.Action, row.Subject, AclEntryValueFromChar(row.Value[0])));
                            }
                        }

                        _allEntries = result;
                    }
                }
            }

            return _allEntries;
        }

        /// <summary>
        ///     Retrieves all ACL entries for a resource.
        /// </summary>
        /// <param name="resource">The resource.</param>
        /// <returns>The ACL entries for the resource.</returns>
        private IEnumerable<AclEntry> RetrieveAclEntriesForResource(string resource)
        {
            return _aclEntriesByResource.GetOrAdd(resource, x =>
            {
                var result = new List<AclEntry>(50);

                using (var connection = CommandBuilder.GetConnection(connString))
                {
                    foreach (var row in connection.Query("SELECT Resource, Action, Subject, Value FROM AclEntry WHERE Resource = @Resource", new { Resource = x }))
                    {
                        result.Add(new AclEntry(row.Resource, row.Action, row.Subject, AclEntryValueFromChar(row.Value[0])));
                    }
                }

                return result;
            });
        }

        /// <summary>
        ///     Retrieves all ACL entries for a subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <returns>The ACL entries for the subject.</returns>
        private IEnumerable<AclEntry> RetrieveAclEntriesForSubject(string subject)
        {
            return _aclEntriesBySubject.GetOrAdd(subject, x =>
            {
                var result = new List<AclEntry>(50);

                using (var connection = CommandBuilder.GetConnection(connString))
                {
                    foreach (var row in connection.Query("SELECT Resource, Action, Subject, Value FROM AclEntry WHERE Subject = @Subject", new { Subject = x }))
                    {
                        result.Add(new AclEntry(row.Resource, row.Action, row.Subject, AclEntryValueFromChar(row.Value[0])));
                    }
                }

                return result;
            });
        }

        /// <summary>
        ///     Deletes some ACL entries.
        /// </summary>
        /// <param name="entries">The entries to delete.</param>
        /// <returns><c>true</c> if one or more entries were deleted, <c>false</c> otherwise.</returns>
        private bool DeleteEntries(IEnumerable<AclEntry> entries)
        {
            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            lock (_aclEntriesLock)
            {
                _allEntries = null;
            }

            foreach (var entry in entries)
            {
                var query = queryBuilder.DeleteFrom("AclEntry");
                query = queryBuilder.Where(query, "Resource", WhereOperator.Equals, "Resource");
                query = queryBuilder.AndWhere(query, "Action", WhereOperator.Equals, "Action");
                query = queryBuilder.AndWhere(query, "Subject", WhereOperator.Equals, "Subject");

                var parameters = new List<Parameter>(3);
                parameters.Add(new Parameter(ParameterType.String, "Resource", entry.Resource));
                parameters.Add(new Parameter(ParameterType.String, "Action", entry.Action));
                parameters.Add(new Parameter(ParameterType.String, "Subject", entry.Subject));

                var command = builder.GetCommand(transaction, query, parameters);

                if (ExecuteNonQuery(command, false) <= 0)
                {
                    RollbackTransaction(transaction);
                    return false;
                }

                _aclEntriesBySubject.TryRemove(entry.Subject, out _);
                _aclEntriesByResource.TryRemove(entry.Resource, out _);
            }

            CommitTransaction(transaction);

            return true;
        }

        /// <summary>
        ///     Stores a ACL entry.
        /// </summary>
        /// <param name="entry">The entry to store.</param>
        /// <returns><c>true</c> if the entry was stored, <c>false</c> otherwise.</returns>
        private bool StoreEntry(AclEntry entry)
        {
            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.InsertInto("AclEntry", new[] { "Resource", "Action", "Subject", "Value" },
                new[] { "Resource", "Action", "Subject", "Value" });

            var parameters = new List<Parameter>(3);
            parameters.Add(new Parameter(ParameterType.String, "Resource", entry.Resource));
            parameters.Add(new Parameter(ParameterType.String, "Action", entry.Action));
            parameters.Add(new Parameter(ParameterType.String, "Subject", entry.Subject));
            parameters.Add(new Parameter(ParameterType.Char, "Value", AclEntryValueToChar(entry.Value)));

            var command = builder.GetCommand(transaction, query, parameters);

            if (ExecuteNonQuery(command, false) != 1)
            {
                RollbackTransaction(transaction);
                return false;
            }

            _aclEntriesBySubject.TryRemove(entry.Subject, out _);
            _aclEntriesByResource.TryRemove(entry.Resource, out _);

            lock (_aclEntriesLock)
            {
                _allEntries = null;
            }

            CommitTransaction(transaction);

            return true;
        }

        /// <summary>
        ///     Renames a ACL resource.
        /// </summary>
        /// <param name="resource">The resource to rename.</param>
        /// <param name="newName">The new name of the resource.</param>
        /// <returns><c>true</c> if one or more entries weere updated, <c>false</c> otherwise.</returns>
        private bool RenameAclResource(string resource, string newName)
        {
            var builder = CommandBuilder;
            var connection = builder.GetConnection(connString);
            var transaction = BeginTransaction(connection);

            var queryBuilder = new QueryBuilder(builder);

            var query = queryBuilder.Update("AclEntry", new[] { "Resource" }, new[] { "ResourceNew" });
            query = queryBuilder.Where(query, "Resource", WhereOperator.Equals, "ResourceOld");

            var parameters = new List<Parameter>(2);
            parameters.Add(new Parameter(ParameterType.String, "ResourceNew", newName));
            parameters.Add(new Parameter(ParameterType.String, "ResourceOld", resource));

            var command = builder.GetCommand(transaction, query, parameters);

            if (ExecuteNonQuery(command, false) <= 0)
            {
                RollbackTransaction(transaction);
                return false;
            }

            _aclEntriesByResource.TryRemove(resource, out _);
            _aclEntriesByResource.TryRemove(newName, out _);
            _aclEntriesBySubject.Clear();

            lock (_aclEntriesLock)
            {
                _allEntries = null;
            }

            CommitTransaction(transaction);

            return true;
        }

        #endregion
    }
}