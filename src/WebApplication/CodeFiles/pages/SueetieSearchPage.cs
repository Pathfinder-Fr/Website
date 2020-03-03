using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using Sueetie.Controls;
using System.IO;
using System.Web.UI.HtmlControls;
using Sueetie.Search;
using Sueetie.Analytics;

namespace Sueetie.Web
{
    public partial class SueetieSearchPage : SueetieBaseThemedPage
    {

        protected Button btnSearch;
        protected CheckBoxList cblSearchApps;
        protected Literal terms;
        protected PlaceHolder noResults;
        protected Repeater rptResults;
        protected TextBox txtSearch;

        public int MaxResultsCount { get; set; }

        public SueetieSearchPage()
            : base("search_default")
        {
            this.MaxResultsCount = 100;
            this.SueetieMasterPage = "search.master";
        }

        public SueetieSearch search
        {
            get
            {
                return new SueetieSearch();
            }
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["srch"] != null)
                {
                    if (Request.QueryString["app"] != null)
                    {
                        string appKey = Request.QueryString.Get("app");
                        foreach (ListItem cb in cblSearchApps.Items)
                        {
                            if (cb.Value.ToLower() == appKey.ToLower())
                                cb.Selected = true;
                            else
                                cb.Selected = false;
                        }
                    }
                    else
                    {
                        foreach (ListItem cb in cblSearchApps.Items)
                        {
                            cb.Selected = true;
                        }
                    }
                    string _srchQuery = Request.QueryString["srch"].Replace(" ", "+");
                    txtSearch.Text = _srchQuery;
                    SearchCommon.EnterSearchTerm(_srchQuery, SearchType.Global);
                    DoSearch(_srchQuery);
                }

            }
            txtSearch.Focus();
            txtSearch.Attributes.Add("onKeyPress", "javascript:if (event.keyCode == 13) __doPostBack('" + btnSearch.UniqueID + "','')");

        }

        protected void btnSearchAllApps_OnClick(object sender, EventArgs e)
        {
            foreach (ListItem cb in cblSearchApps.Items)
            {
                cb.Selected = true;
            }
        }
        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            DoSearch(txtSearch.Text);
        }

        protected void DoSearch(string _srchQuery)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                SearchCommon.EnterSearchTerm(txtSearch.Text.Trim(), SearchType.Global);
                bool IsRestrictedUser = !CurrentSueetieUser.IsSueetieAdministrator;
                if (_srchQuery.ToLower().Contains(" or ") || _srchQuery.ToLower().Contains(" and "))
                    _srchQuery = "(" + _srchQuery.Replace(" or ", " OR ").Replace(" and ", " AND ").Replace(" not ", " NOT ") + ")";

                string _appFilter = string.Empty;
                if (!_srchQuery.Contains("App:"))
                {
                    foreach (ListItem cb in cblSearchApps.Items)
                    {
                        if (cb.Selected)
                            _appFilter += "App:" + cb.Value + "|";
                    }
                    if (!string.IsNullOrEmpty(_appFilter))
                        _appFilter = " AND (" + _appFilter.Substring(0, _appFilter.LastIndexOf("|")).Replace("|", " OR ") + ")";
                }
                else
                {
                    foreach (ListItem cb in cblSearchApps.Items)
                    {
                        cb.Selected = false;
                    }
                }
                string _groupFilter = " AND (GroupKey:na)";
                //string _groupFilter = string.Empty;

                List<SueetieSearchResult> results = search.Search(_srchQuery + _appFilter + _groupFilter, MaxResultsCount, IsRestrictedUser);
                rptResults.DataSource = results;
                rptResults.DataBind();
                if (results.Count() > 0)
                {
                    noResults.Visible = false;
                    rptResults.Visible = true;
                }
                else
                {
                    terms.Text = HttpUtility.HtmlEncode(txtSearch.Text);
                    noResults.Visible = true;
                }
            }
        }
        protected virtual void SearchResultsCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                SearchResult = (SueetieSearchResult)e.Item.DataItem;
                if (SearchResult != null)
                    BindLink(e, SearchResult);
            }
        }

        public SueetieSearchResult SearchResult
        {
            get;
            private set;
        }

        private void BindLink(RepeaterItemEventArgs e, SueetieSearchResult searchResult)
        {
            var relatedLink = (HyperLink)e.Item.FindControl("Link");
            var datePublished = (Literal)e.Item.FindControl("DatePublished");
            var score = (Literal)e.Item.FindControl("Score");
            var ltContainerName = (Literal)e.Item.FindControl("ltContainerName");
            var ltContainerLabel = (Literal)e.Item.FindControl("ltContainerLabel");
            var ltHighlightedContent = (Literal)e.Item.FindControl("ltHighlightedContent");
            var ltAuthor = (Literal)e.Item.FindControl("ltAuthor");
            var ltTags = (Literal)e.Item.FindControl("ltTags");
            var searchResultItem = (HtmlGenericControl)e.Item.FindControl("searchResultItem");
            if (relatedLink != null)
            {
                searchResultItem.Attributes.Add("class", "srch" + SearchResultItemClass(searchResult.ApplicationTypeID, searchResult.ContentTypeID));
                relatedLink.Text = searchResult.Title;
                relatedLink.NavigateUrl = searchResult.PermaLink;
                datePublished.Text = searchResult.PublishDate.ToShortDateString();
                score.Text = searchResult.Score.ToString();
                ltContainerLabel.Text = ContainerLabel(searchResult.ApplicationTypeID, searchResult.ContentTypeID);
                ltContainerName.Text = searchResult.ContainerName;
                ltHighlightedContent.Text = searchResult.HighlightedContent;
                ltAuthor.Text = searchResult.Author;
                if (string.IsNullOrEmpty(searchResult.DisplayTags))
                    ltTags.Text = SueetieLocalizer.GetString("no_tags");
                else
                    ltTags.Text = searchResult.DisplayTags;

            }
        }


        private string SearchResultItemClass(int applicationTypeID, int contentTypeID)
        {
            string _itemClass = "blog";
            switch (applicationTypeID)
            {
                case (int)SueetieApplicationType.Forum:
                    _itemClass = "forum";
                    break;
                case (int)SueetieApplicationType.MediaGallery:
                    switch (contentTypeID)
                    {
                        case (int)SueetieContentType.MediaAudioFile:
                        case (int)SueetieContentType.MediaDocument:
                        case (int)SueetieContentType.MediaImage:
                        case (int)SueetieContentType.MediaOther:
                        case (int)SueetieContentType.MediaVideo:
                            _itemClass = "media";
                            break;
                        default:
                            _itemClass = "album";
                            break;
                    }
                    break;
                case (int)SueetieApplicationType.Wiki:
                    _itemClass = "wiki";
                    break;
                case (int)SueetieApplicationType.CMS:
                    _itemClass = "cms";
                    break;
                default:
                    break;
            }
            return _itemClass;
        }

        private string ContainerLabel(int applicationTypeID, int contentTypeID)
        {
            string _containerLabel = "Blog:";
            switch (applicationTypeID)
            {
                case (int)SueetieApplicationType.Forum:
                    _containerLabel = "Forum:";
                    break;
                case (int)SueetieApplicationType.MediaGallery:
                    switch (contentTypeID)
                    {
                        case (int)SueetieContentType.MediaAudioFile:
                        case (int)SueetieContentType.MediaDocument:
                        case (int)SueetieContentType.MediaImage:
                        case (int)SueetieContentType.MediaOther:
                        case (int)SueetieContentType.MediaVideo:
                            _containerLabel = "Album:";
                            break;
                        default:
                            _containerLabel = "Gallery:";
                            break;
                    }
                    break;
                case (int)SueetieApplicationType.Wiki:
                    _containerLabel = "Wiki:";
                    break;
                case (int)SueetieApplicationType.CMS:
                    _containerLabel = "CMS:";
                    break;

                default:
                    break;
            }
            return _containerLabel;
        }

    }

}

