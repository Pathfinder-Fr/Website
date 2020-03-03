// -----------------------------------------------------------------------
// <copyright file="SueetieBasePanel.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Web;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class SueetieBasePanel : Panel
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

        public string HtmlEncode(object data)
        {
            return HttpContext.Current.Server.HtmlEncode(data.ToString());
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