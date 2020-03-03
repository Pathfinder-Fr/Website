
using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Web;
using System.Globalization;
using System.Reflection;


namespace Sueetie.Wiki {

    /// <summary>
    /// Contains useful Tools.
    /// </summary>
    public static class Tools {

        static int FileAccessTries = 10;
        static int FileAccessTryDelay = 50;
        /// <summary>
        /// Returns the content of a file.
        /// </summary>
        /// <param name="path">The full path of the file to read.</param>
        /// <returns>The content of a file.</returns>
        public static string LoadFile(string path) {
			if(!File.Exists(path)) return null;
            FileStream fs = null;
            for(int i = 0; i < FileAccessTries; i++) {
                try {
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                    break;
                }
                catch {
                    Thread.Sleep(FileAccessTryDelay);
                }
            }
            if(fs == null) throw new IOException("Unable to open the file: " + path);
            StreamReader sr = new StreamReader(fs, UTF8Encoding.UTF8);
            string res = sr.ReadToEnd();
            sr.Close();
            return res;
        }

        /// <summary>
        /// Writes the content of a File.
        /// </summary>
        /// <param name="path">The full path of the file to write.</param>
        /// <param name="content">The content of the file.</param>
        public static void WriteFile(string path, string content) {
            FileStream fs = null;
            for(int i = 0; i < FileAccessTries; i++) {
                try {
                    fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                    break;
                }
                catch {
                    Thread.Sleep(FileAccessTryDelay);
                }
            }
            if(fs == null) throw new IOException("Unable to open the file: " + path);
            StreamWriter sw = new StreamWriter(fs, UTF8Encoding.UTF8);
            sw.Write(content);
            sw.Close();
            Log.LogEntry("File " + Path.GetFileName(path) + " written", EntryType.General, "SUEETIE", null);
        }

		/// <summary>
		/// Appends some content to a File. If the file doesn't exist, it is created.
		/// </summary>
		/// <param name="path">The full path of the file to append the content to.</param>
		/// <param name="content">The content to append to the file.</param>
		public static void AppendFile(string path, string content) {
			FileStream fs = null;
			for(int i = 0; i < FileAccessTries; i++) {
				try {
					fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.None);
					break;
				}
				catch {
					Thread.Sleep(FileAccessTryDelay);
				}
			}
			if(fs == null) throw new IOException("Unable to open the file: " + path);
			StreamWriter sw = new StreamWriter(fs, UTF8Encoding.UTF8);
			sw.Write(content);
			sw.Close();
			Log.LogEntry("File " + Path.GetFileName(path) + " written", EntryType.General, "SUEETIE", null);
		}
	}

}
