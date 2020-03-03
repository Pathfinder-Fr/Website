using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using YAF.Classes.Data;

namespace YAF.Modules
{
    public class WikiBBCodeModule : YAF.Controls.YafBBCodeControl
    {
        public WikiBBCodeModule()
        {
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //// Debug pour connaître la liste des variables détectées
            foreach (var key in this.Parameters.Keys)
            {
                writer.Write(string.Format("<!-- {0} = {1} -->", key, this.Parameters[key]));
            }

            Match match;
            string text;
            string link;
            string inner = this.Parameters["inner"];
            string suffix = string.Empty;
            string anchor = string.Empty;
            string @namespace = "Pathfinder-RPG";

            if (!string.IsNullOrWhiteSpace(inner))
            {
                match = Regex.Match(inner, "^((?<text>[^\\|]+)\\|)?(?<link>[^\\]]+)$", RegexOptions.CultureInvariant);
                text = match.Groups["text"].Value.Trim();
                link = match.Groups["link"].Value.Trim();
            }
            else
            {
                text = this.Parameters["text"];
                link = this.Parameters["link"];
                this.Parameters.TryGetValue("suffix", out suffix);
            }

            bool linkFromText = false;
            if (string.IsNullOrWhiteSpace(link))
            {
                link = text;
                linkFromText = true;
            }

            var i = link.IndexOf('.');
            if (i > 0 && i < link.Length - 1)
            {
                // Détection d'un point séparateur de namespaces
                @namespace = link.Substring(0, i);
                link = link.Substring(i + 1);

                if (linkFromText)
                {
                    // On modifie le texte pour qu'il corresponde au lien, mais sans l'information de namespace
                    text = link;
                }
            }
            
            i = -1;
            match = Regex.Match(link, "(?<!&)#");
            if (match.Success)
                i = match.Index;

            if (i > 0 && i < link.Length - 1)
            {
                // Détection d'une ancre de page
                anchor = link.Substring(i);
                link = link.Substring(0, i);

                if (linkFromText)
                {
                    // On modifie le texte pour qu'il corresponde au lien, mais uniquement avec l'information de l'ancre
                    text = anchor.Substring(1);
                }

                anchor = anchor.ToUpperInvariant();
            }

            link = link.Replace("'", string.Empty).Replace("&#39;", string.Empty);

            writer.Write(string.Format("<a href=\"/Wiki/{0}.{1}.ashx{4}\">{2}{3}</a>", @namespace, link, text, suffix, anchor));
        }
    }
}