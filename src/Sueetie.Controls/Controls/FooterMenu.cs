using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web;
using System.IO;
using System.Xml;
using Sueetie.Controls;
using System.Xml.Linq;

namespace Sueetie.Controls
{


    public class FooterMenu : SueetieBaseControl
    {
        private bool _isAdminFooter = false;
        public bool IsAdminFooter
        {
            get
            {
                return this._isAdminFooter;
            }
            set
            {
                this._isAdminFooter = value;
            }
        }

        public string HighlightLink { get; set; }

        public void Page_Load()
        {
            Label _menu = new Label();
            string highlightedmenu = string.Empty;

            string htmlkey = FooterMenuHTMLCacheKey();
            string MenuHTML = SueetieCache.Current[htmlkey] as string;
            string _currentTheme = this.CurrentTheme;
            if (IsAdminFooter)
                _currentTheme = SueetieConfiguration.Get().Core.AdminTheme;

            if (MenuHTML == null)
            {
                string file = HttpContext.Current.Server.MapPath("/themes/") + _currentTheme + "\\config\\footer.config";
                StreamReader sr = new StreamReader(file);
                MenuHTML += sr.ReadToEnd();
                sr.Close();
                SueetieCache.Current.Insert(htmlkey, MenuHTML);
            }

            SueetieUser user = SueetieContext.Current.User;

            foreach (FooterMenuLink link in SueetieConfiguration.Get().FooterMenuLinks)
            {
                if (!IsUserAuthorizedForLink(user, link.role))
                    highlightedmenu = MenuHTML.Replace("id=\"" + link.id + "\"", "id=\"" + link.id + "\" style=\"display:none;\"");
                else
                    highlightedmenu = MenuHTML;

                MenuHTML = highlightedmenu;
            }

            _menu.Text = highlightedmenu;
            Controls.Add(_menu);

        }

        private bool IsUserAuthorizedForLink(SueetieUser user, string role)
        {
            bool isUserAuthorizedForLink = true;
            if (role != null)
            {
                if (user.IsInRole(role))
                    isUserAuthorizedForLink = true;
                else
                    isUserAuthorizedForLink = false;
            }
            return isUserAuthorizedForLink;
        }


        private string FooterMenuHTMLCacheKey()
        {
            return String.Format("FooterMenuHTML-{0}-{1}-{2}", SueetieConfiguration.Get().Core.SiteUniqueName, IsAdminFooter, this.CurrentTheme);
        }
    }
}
