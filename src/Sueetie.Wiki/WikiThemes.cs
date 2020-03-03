using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Sueetie.Wiki
{
    public static class WikiThemes
    {
        public static void UpdateWikiTheme(string wikiPath, string themeName)
        {
            string _wikiPath = "/" + wikiPath + "/public/config.cs";
            string filename = System.Web.HttpContext.Current.Server.MapPath(_wikiPath);
            string[] lines = System.IO.File.ReadAllLines(filename);

            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                foreach (string line in lines)
                {
                    if (!line.StartsWith("Theme ="))
                    {
                        file.WriteLine(line);
                    }
                }
                file.WriteLine("Theme = " + themeName);
            }


        }
    }
}
