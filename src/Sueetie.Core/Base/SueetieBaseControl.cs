// -----------------------------------------------------------------------
// <copyright file="SueetieBaseControl.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class SueetieBaseControl : UserControl
    {
        public string CurrentTheme
        {
            get { return this.CurrentPageContext.Theme; }
        }

        public SueetieContext CurrentPageContext
        {
            get { return SueetieContext.Current; }
        }

        public SueetieUser CurrentSueetieUser
        {
            get { return SueetieContext.Current.User; }
        }

        public bool IsGroup
        {
            get
            {
                var _url = HttpContext.Current.Request.RawUrl.ToLower();
                if (_url.Contains("/groups"))
                    return true;
                return false;
            }
        }

        public int CurrentUserID
        {
            get { return SueetieContext.Current.User.UserID; }
        }

        public int CurrentSueetieUserID
        {
            get { return this.CurrentSueetieUser.UserID; }
        }

        public string HtmlEncode(object data)
        {
            // Suggested by Styx31 pre-v3.1 release
            return HttpUtility.HtmlEncode(data);
            //if (data == null || !(data is string)) return null;
            //return Server.HtmlEncode(data.ToString());
        }

        protected static HtmlLink MakeStyleSheetControl(string href)
        {
            var stylesheet = new HtmlLink();
            stylesheet.Href = href;
            stylesheet.Attributes.Add("rel", "stylesheet");
            stylesheet.Attributes.Add("type", "text/css");

            return stylesheet;
        }
    }
}