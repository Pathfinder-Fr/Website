using System;
using System.Collections.Generic;
using System.Web;
using YAF.Classes.Data;
using System.Text.RegularExpressions;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace YAF.Modules
{
    public class JetonBBCodeModule : YAF.Controls.YafBBCodeControl
    {
        public JetonBBCodeModule()
        {
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            string inner = this.Parameters["inner"];
            
            Match match = Regex.Match(inner, "(?<pos>[dgDG])(?<elems>.*?)\\](?<img>.*?)\\[.*");
            
            string pos = match.Groups["pos"].Value;
            string elems = match.Groups["elems"].Value.Trim();
            string img = match.Groups["img"].Value.Trim();
            
            // Position
            
            string margin = "0 12px 6px 0";
            
            if ((pos == "d") || (pos == "D"))
            {
              pos = "right";
              margin = "0 0 6px 12px";
            }
            else
              pos = "left";
              
            // Style
            
            string border = "default";
            string bcolor = "default";
            string color = "default";
            string mylink = ""; 
            
            // Analyse des paramètres
            
            string text = "";
            string ligne = "";
            string param = "";
            int type = 0;   // 1 = param, 2 = text;
            int incode = 0; // 1 = dans un code html < ... >
            int nelem = 0;  // début d'un nouvel élément
            
            int idx = 0;
            while (idx < elems.Length)
            {

              if ((incode == 0) && (elems[idx] == '!'))
              {
                nelem = 1;
                type = 1;
              }
              else if ((incode == 0) && (elems[idx] == '='))
              {
                nelem = 1;
                type = 2;
              }
              else if (type == 1)
                param += elems[idx];
              else if (type == 2) {
                ligne += elems[idx];
                if ((incode == 0) && (elems[idx] == '<')) incode = 1;
                if ((incode == 1) && (elems[idx] == '>')) incode = 0;
              }
                
              if ((nelem == 1) || (idx == elems.Length-1))
              {
                if (ligne.Length != 0)
                {
                  text += "<p style='fontsize: 80%; margin: 0px 6px;'>" + ligne + "</p>";
                  ligne = "";
                }
                if (param.Length != 0)
                {
                  match = Regex.Match(param, "^a:(?<contenu>.*)");
                  if (match.Success) mylink = match.Groups["contenu"].Value.Trim();
                  match = Regex.Match(param, "^b:(?<contenu>.*)");
                  if (match.Success) border = match.Groups["contenu"].Value.Trim();
                  match = Regex.Match(param, "^c:(?<contenu>.*)");
                  if (match.Success) color = match.Groups["contenu"].Value.Trim();
                  match = Regex.Match(param, "^f:(?<contenu>.*)");
                  if (match.Success) bcolor = match.Groups["contenu"].Value.Trim();
                  int parampv = 0;
                  match = Regex.Match(param, "^p:(?<pvp>[0-9]*)$");
                  if (match.Success)
                    parampv = 1;
                  if (parampv == 0)
                  {
                    match = Regex.Match(param, "^p:(?<pv>[^/]*)/(?<pvmax>[0-9]*)$");
                    if (match.Success)
                      parampv = 2;
                  }
                  if (parampv == 0)
                  {
                    match = Regex.Match(param, "^p:(?<pv>[^/]*)/(?<pvmax>[0-9]*)/(?<dgnl>[0-9]*)$");
                    if (match.Success)
                      parampv = 3;
                  }
                  if (parampv != 0)
                  {
                    double perc_pv = 100d;
                    double perc_dgnl = 0d;
                    int pvtmp = 1;
                    int pvmaxtmp = 1;
                    int dgnltmp = 0;
                    bool ok = false;
                    switch (parampv)
                    {
                      case 1:
                        ok = int.TryParse(match.Groups["pvp"].Value.Trim(), out pvtmp);
                        perc_pv = (double)pvtmp / 100d;
                        break;
                      case 2:
                        ok = int.TryParse(match.Groups["pv"].Value.Trim(), out pvtmp)
                             && int.TryParse(match.Groups["pvmax"].Value.Trim(), out pvmaxtmp);
                        perc_pv = (double)pvtmp / (double)pvmaxtmp;
                        break;
                      case 3:
                        ok = int.TryParse(match.Groups["pv"].Value.Trim(), out pvtmp)
                             && int.TryParse(match.Groups["pvmax"].Value.Trim(), out pvmaxtmp)
                             && int.TryParse(match.Groups["dgnl"].Value.Trim(), out dgnltmp);
                        perc_pv = (double)pvtmp / (double)pvmaxtmp;
                        perc_dgnl = (double)dgnltmp / (double)pvmaxtmp;
                        break;
                    }
                    if (ok)
                    {
                      if (perc_pv < 0) perc_pv = 0;
                      int lg = 80;
                      int lg_pv = (int) ((double)lg * perc_pv);
                      int lg_dgnl = (int) ((double)lg * perc_dgnl);
                      
                      text += "<div style='position: relative; width: " + (lg + 12) + "px; height: 15px; margin: 4px auto;'";
                      text += "title = '";
                      switch (parampv)
                      {
                        case 1:
                          text += "pv : " + match.Groups["pvp"].Value.Trim() + "%";
                          break;
                        case 2:
                          text += "pv : " + match.Groups["pv"].Value.Trim() + " / " + match.Groups["pvmax"].Value.Trim();
                          break;
                        case 3:
                          text += "pv : " + match.Groups["pv"].Value.Trim() + " / " + match.Groups["pvmax"].Value.Trim();
                          text += "; D&eacute;g&acirc;ts non l&eacute;taux : " + match.Groups["dgnl"].Value.Trim();
                          break;
                      }
                      text += "'>";

                        text += "<div style='width: " + lg + "px; height: 15px; position: absolute; left: 6px; top: 0;";
                        text += "background-color: #f3efe2; border: 1px solid #4b3124;";
                        text += "border-radius: 6px; -moz-border-radius: 6px; -webkit-border-radius: 6px'></div>";

                        if (lg_pv >= 1)
                        {
                          text += "<div style='width: " + lg_pv + "px; height: 15px; position: absolute; left: 6px; top: 0;";
                          text += "background-color: #4b3124; border: 1px solid #4b3124;";
//                        text += "background: -moz-linear-gradient(top, #e7dfc6 0%, #4b3124 35%, #4b3124 35%, #4b3124 100%);";
//                        text += "background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#e7dfc6), color-stop(35%,#4b3124), color-stop(35%,#4b3124), color-stop(100%,#4b3124));";
//                        text += "box-shadow: inset 0px 0px 2px #FFF; -webkit-box-shadow: inset 0px 0px 2px #FFF;";
                          text += "border-radius: 6px; -moz-border-radius: 6px; -webkit-border-radius: 6px'></div>";
                        }

                        if ((parampv == 3) && (lg_dgnl >= 1))
                        {
                          text += "<div style='width: " + lg_dgnl + "px; height: 15px; position: absolute; left: 6px; top: 0;";
                          text += "background-color: #998875; border: 1px solid #4b3124;";
                          text += "border-radius: 6px; -moz-border-radius: 6px; -webkit-border-radius: 6px'></div>";
                        }

                        text += "<div style='width: " + lg + "px; height: 15px; position: absolute; left: 6px; top: 1px;";
                        text += "text-shadow: 1px 1px 1px #4b3124; font-size: 12px;";
                        text += "font-weight: bold; color: #f3efe2; text-align: center; vertical-align: center;'>";
                        switch (parampv)
                        {
                          case 1:
                            text += match.Groups["pvp"].Value.Trim() + "%";
                            break;
                          case 2:
                            text += match.Groups["pv"].Value.Trim() + " / " + match.Groups["pvmax"].Value.Trim();
                            break;
                          case 3:
//                            text += "(" + match.Groups["dgnl"].Value.Trim() + ") ";
                            text += match.Groups["pv"].Value.Trim() + " / " + match.Groups["pvmax"].Value.Trim();
                            break;
                        }
                        text += "</div>";

                      text += "</div>";

                    }
                  }
                  param = "";
                }
                nelem = 0;
              }

              idx++;
            }
            
            if (border == "default")
              if (text == "")
                border = "0";
              else
                border = "1px solid #4b3124";
            if (bcolor == "default")
              if (text == "")
                bcolor = "none";
              else
                bcolor = "#e7dfc6";
            if (color == "default")
              color = "black";
                      
            // Affichage
           
            writer.Write("<div style='text-align: center;");
            writer.Write("float:" + pos + ";");
            writer.Write("margin:" + margin + ";");
            writer.Write("border:" + border + ";");
            writer.Write("background-color:" + bcolor + ";");
            writer.Write("color:" + color + "'>");

            if (mylink != "")
              writer.Write("<a href='" + mylink + "'>");
            writer.Write("<img src='" + img + "' style='width: 70px; margin: 4px;'/>");
            if (mylink != "")
              writer.Write("</a>");
            writer.Write(text);

            writer.Write("</div>");
            
        }
    }
}