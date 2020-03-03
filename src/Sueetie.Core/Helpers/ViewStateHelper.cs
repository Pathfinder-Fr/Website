// -----------------------------------------------------------------------
// <copyright file="ViewStateHelper.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    public static class ViewStateHelper
    {
        // Written by Greg Reddick. http://www.xoc.net
        public static void SeeViewState(string strViewState, string strFilename)
        {
            if (strViewState != null)
            {
                Debug.Listeners.Clear();
                File.Delete(strFilename);
                Debug.Listeners.Add(new TextWriterTraceListener(strFilename));
                var strViewStateDecoded =
                    (new UTF8Encoding()).
                        GetString(Convert.FromBase64String(strViewState));
                var astrDecoded = strViewStateDecoded.Replace("<", "<\n").
                    Replace(">", "\n>").Replace(";", ";\n").Split('\n');
                Debug.IndentSize = 4;
                foreach (var str in astrDecoded)
                    if (str.Length > 0)
                        if (str.EndsWith(@"\<"))
                            Debug.Write(str);
                        else if (str.EndsWith(@"\"))
                            Debug.Write(str);
                        else if (str.EndsWith("<"))
                        {
                            Debug.WriteLine(str);
                            Debug.Indent();
                        }
                        else if (str.StartsWith(">;") || str.StartsWith(">"))
                        {
                            Debug.Unindent();
                            Debug.WriteLine(str);
                        }
                        else if (str.EndsWith(@"\;"))
                            Debug.Write(str);
                        else
                            Debug.WriteLine(str);
                Debug.Close();
                Debug.Listeners.Clear();

                //Get into the debugger after executing this line to see how .NET looks at
                //the ViewState info. Compare it to the text file produced above.
                //Triplet trp = (Triplet)((new LosFormatter()).Deserialize(strViewState));
            }
        }
    }
}