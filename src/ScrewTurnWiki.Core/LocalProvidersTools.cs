﻿using System.Text;

namespace ScrewTurn.Wiki
{

    /// <summary>
    /// Implements tools for local providers.
    /// </summary>
    public static class LocalProvidersTools
    {

        /// <summary>
        /// Checks a directory for write permissions.
        /// </summary>
        /// <param name="dir">The directory.</param>
        /// <returns><c>true</c> if the directory has write permissions, <c>false</c> otherwise.</returns>
        public static bool CheckWritePermissions(string dir)
        {
            var file = System.IO.Path.Combine(dir, "__StwTestFile.txt");

            var canWrite = true;

            System.IO.FileStream fs = null;
            try
            {
                fs = System.IO.File.Create(file);
                fs.Write(Encoding.ASCII.GetBytes("Hello"), 0, 5);
            }
            catch
            {
                canWrite = false;
            }
            finally
            {
                try
                {
                    if (fs != null) fs.Close();
                    System.IO.File.Delete(file);
                }
                catch
                {
                    canWrite = false;
                }
            }

            return canWrite;
        }

    }

}
