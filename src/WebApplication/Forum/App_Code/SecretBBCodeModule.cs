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
    public class SecretBBCodeModule : YAF.Controls.YafBBCodeControl
    {
        public SecretBBCodeModule()
        {
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            //string targetText;
            //if (!this.Parameters.TryGetValue("target", out targetText))
            //{
                //targetText = string.Empty;
            //}
            
            //targetText = targetText.Trim();

            //string text = this.Parameters["inner"];
            
            string inner = this.Parameters["inner"];
            
            Match match = Regex.Match(inner, "^((?<param>[^:]+):)?(?<text>.*)$");
            
            string parameter = match.Groups["param"].Value.Trim();
            string text = match.Groups["text"].Value;
            
            this.RenderSecret(writer, parameter, text);
        }
        
        private void RenderSecret(System.Web.UI.HtmlTextWriter writer, string targetText, string text)
        {
            bool isVisible = false;
            
            if (CurrentMessageFlags != null && CurrentMessageFlags.IsLocked)
            {
                isVisible = true;
            }
            
            if (YAF.Core.YafContext.Current.CurrentUserData.UserID == this.DisplayUserID)
            {
                isVisible = true;
            }
            
            string label = "Message secret :";
            
            if (targetText == "*")
            {
                isVisible = true;
                label = "Message secret pour tous :";
            }
            else
            {
                string[] targets = targetText.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //writer.Write("IsGuest = " + YafContext.Current.CurrentUserData.IsGuest.ToString());
                //writer.Write("UserName = " + YafContext.Current.CurrentUserData.UserName.ToString());
                //writer.Write("targets.Length = " + targets.Length.ToString());

                if (!YAF.Core.YafContext.Current.CurrentUserData.IsGuest)
                {
                    string currentUserName = YAF.Core.YafContext.Current.CurrentUserData.UserName;
                    string currentUserDisplayName = YAF.Core.YafContext.Current.CurrentUserData.DisplayName;
                    
                    foreach (string target in targets)
                    {
                        //writer.Write("[" + currentUserName + "]??[" + target + "]");
                        //writer.Write(currentUserName.Equals(target, StringComparison.OrdinalIgnoreCase).ToString());
                        if (currentUserName.Equals(target.Trim(), StringComparison.OrdinalIgnoreCase) ||
                            currentUserDisplayName.Equals(target.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            isVisible = true;
                            break;
                        }
                    }
                }
                
                if (targets.Length > 0)
                {
                    label = "Message secret pour " + string.Join(",", targets) + " :";
                }
            }

            //writer.Write("isVisible = " + isVisible.ToString());

            writer.Write("<div class=\"secret ");
            writer.Write(isVisible ? "secret-visible" : "secret-invisible");
            writer.Write("\">");
            writer.Write("<span class=\"label\">");
            writer.Write(label);
            writer.Write("</span>");
            writer.Write("<div>");

            if (isVisible)
            {
                writer.Write(text);
            }
            else
            {
                writer.Write("...");
            }

            writer.Write("</div>");
            writer.Write("</div>");
        }
        
        private void RenderDebug(System.Web.UI.HtmlTextWriter writer)
        {
            writer.Write("<div class=\"secret\">");
            writer.Write("<ul>");
            foreach(string key in this.Parameters.Keys)
            {
                writer.Write("<li>");
                writer.Write(key);
                writer.Write(" = ");
                writer.Write(System.Web.HttpUtility.HtmlEncode(this.Parameters[key]));
                writer.Write("</li>");
            }
            writer.Write("</ul>");
            writer.Write("</div>");
        }
    }
}