// -----------------------------------------------------------------------
// <copyright file="SueetieBasePage.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;

    public class SueetieBasePage : Page
    {
        public SueetieBasePage()
        {
            this.CreateTitle(null);
        }

        public SueetieBasePage(string _pageKey)
        {
            this.CreateTitle(_pageKey);
        }

        public int CurrentContentID { get; set; }

        private void CreateTitle(string _pageKey)
        {
            var current = SueetieContext.Current;

            if (current != null && (current.IsNonApplicationPage || current.ContentPage != null))
            {
                var _title = string.Empty;
                if (_pageKey != null)
                {
                    _title = SueetieLocalizer.GetPageTitle(_pageKey);
                }
                else if (current.ContentPage != null)
                {
                    _title = HttpUtility.HtmlDecode(current.ContentPage.PageTitle);
                }

                var settings = SiteSettings.Instance;
                if (settings != null && !string.IsNullOrEmpty(settings.SitePageTitleLead))
                {
                    _title = settings.SitePageTitleLead + " " + _title;
                }

                this.Title = _title;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.IsCallback && !this.Page.IsPostBack)
            {
                try
                {
                    var metaStart = string.Format("{0}{0}<!-- Start Current Sueetie Framework Version -->{0}", Environment.NewLine);
                    var metaEnd = string.Format("{0}<!-- End Current Sueetie Framework Version -->{0}{0}", Environment.NewLine);
                    var ltMetaStart = new LiteralControl(metaStart);
                    var ltMetaEnd = new LiteralControl(metaEnd);
                    if (this.Page.Header != null)
                    {
                        this.Page.Header.Controls.Add(ltMetaStart);
                        this.AddMetaTag("CommunityFramework", SiteStatistics.Instance.SueetieVersion);
                        this.Page.Header.Controls.Add(ltMetaEnd);

                        if (!string.IsNullOrEmpty(this.CurrentPageContext.SiteSettings.HtmlHeader))
                        {
                            this.AddCustomCodeToHead();
                        }
                    }

                    this.AddTrackingScript();
                }
                catch
                {
                    // ScrewTurn Wiki SessionRefresh.aspx has no header, as it executes in an iFrame in Edit.aspx and elsewhere 
                }

                if (SueetieApplications.Current.ApplicationTypeID != (int)SueetieApplicationType.MediaGallery &&
                    SueetieApplications.Current.ApplicationTypeID != (int)SueetieApplicationType.Forum &&
                    SueetieApplications.Current.ApplicationTypeID != (int)SueetieApplicationType.Wiki)
                    SueetieLogs.LogRequest(this.Title, this.CurrentContentID, this.CurrentSueetieUserID);
            }

            base.OnLoad(e);
        }


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

        // Phasing out CurrentUserID for CurrentSueetieUserID

        public int CurrentUserID
        {
            get { return this.CurrentSueetieUser.UserID; }
        }

        public int CurrentSueetieUserID
        {
            get { return this.CurrentSueetieUser.UserID; }
        }

        protected virtual void AddTrackingScript()
        {
            this.ClientScript.RegisterStartupScript(this.GetType(), "tracking", "\n" + this.CurrentPageContext.SiteSettings.TrackingScript, false);
        }

        protected virtual void AddCustomCodeToHead()
        {
            var code = string.Format("{0}{0}{1}{0}{0}",
                Environment.NewLine, this.CurrentPageContext.SiteSettings.HtmlHeader);
            var control = new LiteralControl(code);
            this.Page.Header.Controls.Add(control);
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

        protected virtual void AddMetaTag(string name, string value)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(value))
                return;

            var meta = new HtmlMeta();
            meta.Name = name;
            meta.Content = value;
            this.Page.Header.Controls.Add(meta);
        }
    }
}