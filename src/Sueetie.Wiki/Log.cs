
using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Web;
using System.Text;

namespace Sueetie.Wiki {

    /// <summary>
    /// Records and retrieves Log Entries.
    /// </summary>
    public static class Log {


        static int FileAccessTries = 10;
        static int FileAccessTryDelay = 50;
        static int LoggingLevel = 3;
        static int MaxLogSize = 64;

        /// <summary>
        /// Writes an Entry in the Log.
		/// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="type">The Type of the Entry.</param>
        /// <param name="user">The User that generated the Entry.</param>
        public static void LogEntry(string message, EntryType type, string user, string groupName) {
			message = Sanitize(message);
			user = Sanitize(user);
        FileStream fs = null;
            // Since the Log is the most accessed file, the # of access tries has been doubled
            for(int i = 0; i < FileAccessTries * 2; i++) {
                try {
                    fs = new FileStream(SueetieSettings.LogFile(groupName), FileMode.Append, FileAccess.Write, FileShare.None);
                    break;
                }
                catch {
                    Thread.Sleep(FileAccessTryDelay);
                }
            }
            if(fs == null) throw new IOException("Unable to open the file: " + SueetieSettings.LogFile(groupName));
            StreamWriter sw = new StreamWriter(fs, System.Text.UTF8Encoding.UTF8);
            // Type | DateTime | Message | User
			try {
				sw.Write(EntryTypeToString(type) + "|" + string.Format("{0:yyyy'/'MM'/'dd' 'HH':'mm':'ss}", DateTime.Now) + "|" + message + "|" + user + "\r\n");
			}
			catch { }
            sw.Close();
			FileInfo fi = new FileInfo(SueetieSettings.LogFile(groupName));

			if(fi.Length > (long)(MaxLogSize * 1024)) {
				CutLog(groupName);
			}
        }

        private static void CutLog(string groupName)
        {
            // Contains the log messages from the newest to the oldest
            List<LogEntry> entries = ReadEntries(groupName);

            // Estimate the size of each entry at 80 chars
            FileInfo fi = new FileInfo(SueetieSettings.LogFile(groupName));
            long size = fi.Length;
            int difference = (int)(size - (long)(MaxLogSize * 1024));
            int removeEntries = difference / 80 * 2; // Double the number of remove entries in order to reduce the # of times Cut is needed
            int preserve = entries.Count - removeEntries; // The number of entries to be preserved

            // Copy the entries to preserve in a temp list
            List<LogEntry> toStore = new List<LogEntry>();
            for (int i = 0; i < preserve; i++)
            {
                toStore.Add(entries[i]);
            }

            // Reverse the temp list because entries contains the log messages
            // starting from the newest to the oldest
            toStore.Reverse();

            StringBuilder sb = new StringBuilder();
            // Type | DateTime | Message | User
            foreach (LogEntry e in toStore)
            {
                sb.Append(EntryTypeToString(e.EntryType));
                sb.Append("|");
                sb.Append(e.DateTime.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"));
                sb.Append("|");
                sb.Append(e.Message);
                sb.Append("|");
                sb.Append(e.User);
                sb.Append("\r\n");
            }
            FileStream fs = null;
            // Since the Log is the most accessed file, the # of access tries has been doubled
            for (int i = 0; i < FileAccessTries * 2; i++)
            {
                try
                {
                    fs = new FileStream(SueetieSettings.LogFile(groupName), FileMode.Create, FileAccess.Write, FileShare.None);
                    break;
                }
                catch
                {
                    Thread.Sleep(FileAccessTryDelay);
                }
            }
            if (fs == null) throw new IOException("Unable to open the file: " + SueetieSettings.LogFile(groupName));
            StreamWriter sw = new StreamWriter(fs, System.Text.UTF8Encoding.UTF8);
            // Type | DateTime | Message | User
            try
            {
                sw.Write(sb.ToString());
            }
            catch { }
            sw.Close();
        }

        /// <summary>
        /// Reads all the Log Entries.
        /// </summary>
        /// <returns>The Entries.</returns>
        public static List<LogEntry> ReadEntries(string groupName)
        {
            string content = Tools.LoadFile(SueetieSettings.LogFile(groupName).Replace("\r", ""));
            List<LogEntry> result = new List<LogEntry>(50);
            string[] lines = content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] fields;
            for (int i = 0; i < lines.Length; i++)
            {
                fields = lines[i].Split('|');
                try
                {
                    // Try/catch to avoid problems with corrupted file (raw method)
                    result.Add(new LogEntry(EntryTypeParse(fields[0]), DateTime.Parse(fields[1]), Resanitize(fields[2]), Resanitize(fields[3])));
                }
                catch { }
            }
            result.Reverse();
            return result;
        }


		private static string Sanitize(string input) {
			StringBuilder sb = new StringBuilder(input);
			sb.Replace("|", "{PIPE}");
			sb.Replace("\r", "");
			sb.Replace("\n", "{BR}");
			sb.Replace("<", "&lt;");
			sb.Replace(">", "&gt;");
			return sb.ToString();
		}

		private static string Resanitize(string input) {
			StringBuilder sb = new StringBuilder(input);
			sb.Replace("<", "&lt;");
			sb.Replace(">", "&gt;");
			sb.Replace("{BR}", "<br />");
			sb.Replace("{PIPE}", "|");
			return sb.ToString();
		}

		/// <summary>
		/// Converts an EntryType into a string.
		/// </summary>
		/// <param name="type">The EntryType.</param>
		/// <returns>The string.</returns>
		public static string EntryTypeToString(EntryType type) {
            switch(type) {
                case EntryType.General:
                    return "G";
                case EntryType.Warning:
                    return "W";
                case EntryType.Error:
                    return "E";
                default:
                    return "G";
            }
        }

        /// <summary>
        /// Parses a string and converts it to an EntryType.
        /// </summary>
        /// <param name="value">The string.</param>
        /// <returns>The EntryType.</returns>
        public static EntryType EntryTypeParse(string value) {
            switch(value) {
                case "G":
                    return EntryType.General;
                case "W":
                    return EntryType.Warning;
                case "E":
                    return EntryType.Error;
                default:
                    return EntryType.General;
            }
        }

    }

	/// <summary>
	/// Represents a Log Entry.
	/// </summary>
    public class LogEntry {

        private EntryType type;
        private DateTime dateTime;
        private string message;
        private string user;

        /// <summary>
        /// Initializes a new instance of the <b>LogEntry</b> class.
        /// </summary>
        /// <param name="type">The type of the Entry</param>
        /// <param name="dateTime">The DateTime.</param>
        /// <param name="message">The Message.</param>
        /// <param name="user">The User.</param>
        public LogEntry(EntryType type, DateTime dateTime, string message, string user) {
            this.type = type;
            this.dateTime = dateTime;
            this.message = message;
            this.user = user;
        }

        /// <summary>
        /// Gets the EntryType.
        /// </summary>
        public EntryType EntryType {
			get { return type; }
        }

        /// <summary>
        /// Gets the DateTime.
        /// </summary>
        public DateTime DateTime {
            get { return dateTime; }
        }

        /// <summary>
        /// Gets the Message.
        /// </summary>
        public string Message {
            get { return message; }
        }

        /// <summary>
        /// Gets the User.
        /// </summary>
        public string User {
            get { return user; }
        }

    }

	/// <summary>
	/// The Log Entry Types.
	/// </summary>
    public enum EntryType {
		/// <summary>
		/// General.
		/// </summary>
        General,
		/// <summary>
		/// Warning.
		/// </summary>
        Warning,
		/// <summary>
		/// Error.
		/// </summary>
        Error
    }

}
