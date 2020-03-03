using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Collections.Generic;

namespace Sueetie.Controls
{

    public class SueetieLink : SueetieBaseControl
    {

        #region Properties

        public string UrlName
        {
            get { return ((string)ViewState["UrlName"]) ?? string.Empty; }
            set { ViewState["UrlName"] = value; }
        }

        public virtual string TitleKey
        {
            get { return ((string)ViewState["TitleKey"]) ?? string.Empty; }
            set { ViewState["TitleKey"] = value; }
        }
        public virtual string TextKey
        {
            get { return ((string)ViewState["TextKey"]) ?? string.Empty; }
            set { ViewState["TextKey"] = value; }
        }
        public virtual string Text
        {
            get { return ((string)ViewState["Text"]) ?? string.Empty; }
            set { ViewState["Text"] = value; }
        }
        public virtual string LanguageFile
        {
            get { return ((string)ViewState["LanguageFile"]) ?? string.Empty; }
            set { ViewState["LanguageFile"] = value; }
        }
        public string ArgUrl1
        {
            get { return ((string)ViewState["ArgUrl1"]) ?? string.Empty; }
            set { ViewState["ArgUrl1"] = value; }
        }
        public string ArgUrl2
        {
            get { return ((string)ViewState["ArgUrl2"]) ?? string.Empty; }
            set { ViewState["ArgUrl2"] = value; }
        }
        public string ArgUrl3
        {
            get { return ((string)ViewState["ArgUrl3"]) ?? string.Empty; }
            set { ViewState["ArgUrl3"] = value; }
        }
        public string ArgUrl4
        {
            get { return ((string)ViewState["ArgUrl4"]) ?? string.Empty; }
            set { ViewState["ArgUrl4"] = value; }
        }
        public string ArgUrl5
        {
            get { return ((string)ViewState["ArgUrl5"]) ?? string.Empty; }
            set { ViewState["ArgUrl5"] = value; }
        }
        public string ArgText1
        {
            get { return ((string)ViewState["ArgText1"]) ?? string.Empty; }
            set { ViewState["ArgText1"] = value; }
        }
        public string ArgText2
        {
            get { return ((string)ViewState["ArgText2"]) ?? string.Empty; }
            set { ViewState["ArgText2"] = value; }
        }
        public string ArgText3
        {
            get { return ((string)ViewState["ArgText3"]) ?? string.Empty; }
            set { ViewState["ArgText3"] = value; }
        }
        public string ArgText4
        {
            get { return ((string)ViewState["ArgText4"]) ?? string.Empty; }
            set { ViewState["ArgText4"] = value; }
        }
        public string ArgText5
        {
            get { return ((string)ViewState["ArgText5"]) ?? string.Empty; }
            set { ViewState["ArgText5"] = value; }
        }
        public string LinkCssClass
        {
            get { return ((string)ViewState["LinkCssClass"]) ?? string.Empty; }
            set { ViewState["LinkCssClass"] = value; }
        }

        public virtual SueetieUrlLink SueetieUrlLinkTo
        {
            get { return (SueetieUrlLink)(ViewState["SueetieUrl"] ?? SueetieUrlLink.Unknown); }
            set { ViewState["SueetieUrl"] = value; }
        }

        #endregion

        #region Constructor

        public SueetieLink()
        { }

        #endregion

        #region GetSueetieUrl Selector

        public SueetieUrl GetSueetieUrl()
        {
            SueetieUrl sueetieUrl = null;
            if (this.SueetieUrlLinkTo != SueetieUrlLink.Unknown)
            {
                switch (this.SueetieUrlLinkTo)
                {
                    #region Common

                    case SueetieUrlLink.Home:
                        sueetieUrl = SueetieUrls.Instance.Home();
                        break;
                    case SueetieUrlLink.Contact:
                        sueetieUrl = SueetieUrls.Instance.Contact();
                        break;
                    case SueetieUrlLink.SearchHome:
                        sueetieUrl = SueetieUrls.Instance.SearchHome();
                        break;

                    #endregion

                    #region Members

                    case SueetieUrlLink.Login:
                        sueetieUrl = SueetieUrls.Instance.Login();
                        break;
                    case SueetieUrlLink.Logout:
                        sueetieUrl = SueetieUrls.Instance.Logout();
                        break;
                    case SueetieUrlLink.MyAccountInfo:
                        sueetieUrl = SueetieUrls.Instance.MyAccountInfo();
                        break;
                    case SueetieUrlLink.Register:
                        sueetieUrl = SueetieUrls.Instance.Register();
                        break;

                    #endregion

                    #region Blogs

                    case SueetieUrlLink.BlogsHome:
                        sueetieUrl = SueetieUrls.Instance.BlogsHome();
                        break;

                    #endregion

                    #region Media

                    case SueetieUrlLink.MediaHome:
                        sueetieUrl = SueetieUrls.Instance.MediaHome();
                        break;
      
                    #endregion

                    #region Wiki

                    case SueetieUrlLink.WikiHome:
                        sueetieUrl = SueetieUrls.Instance.WikiHome();
                        break;

                    #endregion

                    #region Forums

                    case SueetieUrlLink.ForumsHome:
                        sueetieUrl = SueetieUrls.Instance.ForumsHome();
                        break;

                    #endregion

                    #region Administration

                    case SueetieUrlLink.AdminHome:
                        sueetieUrl = SueetieUrls.Instance.AdminHome();
                        break;

                    #endregion

                    #region Groups

                    case SueetieUrlLink.GroupsHome:
                        sueetieUrl = SueetieUrls.Instance.GroupsHome();
                        break;

                    #endregion

                    #region Marketplace

                    case SueetieUrlLink.MarketplaceHome:
                        sueetieUrl = SueetieUrls.Instance.MarketplaceHome();
                        break;

                    #endregion

                    #region Calendars

                    case SueetieUrlLink.CalendarHome:
                        sueetieUrl = SueetieUrls.Instance.CalendarHome();
                        break;

                    #endregion
                }
            }
            return sueetieUrl;
        }

        #endregion

        #region OnLoad

        protected override void OnLoad(EventArgs e)
        {
            HyperLink _sueetieLink = new HyperLink();

            string _languageFile = "sueetie.xml";
            if (!string.IsNullOrEmpty(this.LanguageFile))
                _languageFile = this.LanguageFile;


            SueetieUrl _sueetieUrl = GetSueetieUrl();
            if (_sueetieUrl == null)
                _sueetieUrl = SueetieUrls.Instance.GetSueetieUrl(this.UrlName);

            if (_sueetieUrl.Url != null)
            {
                if (!string.IsNullOrEmpty(_sueetieUrl.Roles))
                {
                    if (!SueetieUIHelper.IsUserAuthorized(_sueetieUrl.Roles))
                        return;
                }

                if (!string.IsNullOrEmpty(this.ArgUrl1))
                {
                    string[] _urlArgs = new string[] { this.ArgUrl1, this.ArgUrl2, this.ArgUrl3, this.ArgUrl4, this.ArgUrl5 };
                    _sueetieLink.NavigateUrl = SueetieUrls.Instance.FormatUrl(_sueetieUrl.Url, _urlArgs);
                }
                else
                    _sueetieLink.NavigateUrl = _sueetieUrl.Url;

                if (!string.IsNullOrEmpty(this.ArgText1))
                {
                    string[] _textArgs = new string[] { this.ArgText1, this.ArgText2, this.ArgText3, this.ArgText4, this.ArgText5 };
                    _sueetieLink.Text = SueetieLocalizer.GetString(this.TextKey, _languageFile, _textArgs);
                }
                else if (!string.IsNullOrEmpty(this.Text))
                    _sueetieLink.Text = SueetieLocalizer.GetString(this.Text, _languageFile);
                else
                    _sueetieLink.Text = SueetieLocalizer.GetString(this.TextKey, _languageFile);


                if (!string.IsNullOrEmpty(this.TitleKey))
                    _sueetieLink.ToolTip = SueetieLocalizer.GetString(this.TitleKey, _languageFile);

                if (!string.IsNullOrEmpty(this.LinkCssClass))
                    _sueetieLink.CssClass = this.LinkCssClass;
                Controls.Add(_sueetieLink);
            }
            else
                SueetieLogs.LogException("SUEETIE URL NOT FOUND. UrlName: " + this.UrlName ?? this.SueetieUrlLinkTo.ToString());
        }

    }

        #endregion

}
