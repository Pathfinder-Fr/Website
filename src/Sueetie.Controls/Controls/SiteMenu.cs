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


    public class SiteMenu : SueetieBaseControl
    {

        public string HighlightTab { get; set; }


        public void Page_Load()
        {
            Label _menu = new Label();

            string highlightedmenu = string.Empty;

            string htmlkey = MenuHTMLCacheKey();
            string MenuHTML = SueetieCache.Current[htmlkey] as string;
            if (MenuHTML == null)
            {
                string file = HttpContext.Current.Server.MapPath("/themes/") + this.CurrentTheme + "\\config\\menu.config";
                StreamReader sr = new StreamReader(file);
                MenuHTML += sr.ReadToEnd();
                sr.Close();
                SueetieCache.Current.Insert(htmlkey, MenuHTML);
            }

            SueetieUser user = SueetieContext.Current.User;
            bool _noLight = true;

            foreach (SiteMenuTab tab in SueetieConfiguration.Get().SiteMenuTabs)
            {
                if (IsUserAuthorizedForTab(user, tab.roles))
                {
                    if (IsGroup && _noLight)
                    {
                        highlightedmenu = MenuHTML.Replace("id=\"GroupsTab\"", "id=\"GroupsTab\" class=\"current\"");
                        _noLight = false;
                    }
                    else if (HttpContext.Current.Request.RawUrl.ToLower().Contains("/search/") && _noLight)
                    {
                        highlightedmenu = MenuHTML.Replace("id=\"SearchTab\"", "id=\"SearchTab\" class=\"current\"");
                        _noLight = false;
                    }
                    else if (HttpContext.Current.Request.RawUrl.ToLower() == tab.url && _noLight)
                    {
                        highlightedmenu = MenuHTML.Replace("id=\"" + tab.id + "\"", "id=\"" + tab.id + "\" class=\"current\"");
                        _noLight = false;
                    }
                    else if (String.Compare(SueetieApplications.Current.ApplicationKey, tab.appkey, true) == 0 && ShowAppTab(tab) && _noLight)
                    {
                        highlightedmenu = MenuHTML.Replace("id=\"" + tab.id + "\"", "id=\"" + tab.id + "\" class=\"current\"");
                        _noLight = false;
                    }
                    else if (String.Compare(SueetieApplications.Current.ApplicationName, tab.app, true) == 0 && ShowAppTab(tab) && _noLight)
                    {
                        highlightedmenu = MenuHTML.Replace("id=\"" + tab.id + "\"", "id=\"" + tab.id + "\" class=\"current\"");
                        _noLight = false;
                    }
                    else
                        highlightedmenu = MenuHTML;
                }
                else if (!IsAnonymousTab(user, tab.roles))
                    highlightedmenu = MenuHTML.Replace("id=\"" + tab.id + "\"", "id=\"" + tab.id + "\" style=\"display:none;\"");

                MenuHTML = highlightedmenu;
            }

            highlightedmenu = MenuHTML.Replace("id=\"" + HighlightTab + "\"", "id=\"" + HighlightTab + "\" class=\"current\"");
            _menu.Text = highlightedmenu;
            Controls.Add(_menu);

        }

        private static bool ShowAppTab(SiteMenuTab tab)
        {
            bool showAppTab = true;
            if (tab.maskurl != null)
            {
                if (HttpContext.Current.Request.RawUrl.ToLower() == tab.maskurl.ToLower())
                    showAppTab = false;
            }
            return showAppTab;
        }

        private static bool IsUserAuthorizedForTab(SueetieUser user, string roles)
        {
            bool isUserAuthorizedForTab = true;
            if (roles != null)
            {
                isUserAuthorizedForTab = false;
                string[] rolesList = roles.Split(',');
                foreach (string role in rolesList)
                {
                    if (user.IsInRole(role))
                        isUserAuthorizedForTab = true;
                }
            }
            return isUserAuthorizedForTab;
        }

        private static bool IsAnonymousTab(SueetieUser user, string roles)
        {
            bool isAnonymousTab = true;
            if (roles != null)
            {
                isAnonymousTab = false;
                string[] rolesList = roles.Split(',');
                foreach (string role in rolesList)
                {
                    if (user.IsAnonymous && role.ToLower() == "anonymous")
                        isAnonymousTab = true;
                }
            }
            return isAnonymousTab;
        }

        private static string MenuHTMLCacheKey()
        {
            return String.Format("SiteMenuHTML-{0}-{1}", SueetieConfiguration.Get().Core.SiteUniqueName, SueetieContext.Current.Theme);
        }
    }
}
