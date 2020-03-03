// -----------------------------------------------------------------------
// <copyright file="SueetieBaseMasterPage.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class SueetieBaseMasterPage : MasterPage
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

        public int CurrentUserID
        {
            get { return this.CurrentSueetieUser.UserID; }
        }

        protected static HtmlLink MakeStyleSheetControl(string href)
        {
            var stylesheet = new HtmlLink();
            stylesheet.Href = href;
            stylesheet.Attributes.Add("rel", "stylesheet");
            stylesheet.Attributes.Add("type", "text/css");

            return stylesheet;
        }

        protected static LiteralControl AddStyleSheet(string _css)
        {
            var _literalControl = new LiteralControl();
            _literalControl.Text = "<link href=\"/themes/" + SueetieContext.Current.Theme + "/style/" + _css + "\" rel=\"stylesheet\" type=\"text/css\" />\n";
            return _literalControl;
        }

        protected static HtmlMeta AddMetaTag(string name, string value)
        {
            var meta = new HtmlMeta();
            meta.Name = name;
            meta.Content = value;
            return meta;
        }
    }
}