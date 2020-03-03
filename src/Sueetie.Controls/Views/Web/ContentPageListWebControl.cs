using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class ContentPageListWebControl : SueetieBaseControl
    {

        protected PlaceHolder phContentPages;

        public ContentPageListWebControl()
        {
            ContentGroupID = -1;
            ViewName = "ContentPageView";
            CacheMinutes = 5;
            SortBy = SueetieSortBy.ItemTitle;
        }

        public int NumRecords { get; set; }
        public int UserID { get; set; }
        public int ContentGroupID { get; set; }
        public bool IsRestricted { get; set; }
        public string ViewName { get; set; }
        public int CacheMinutes { get; set; }
        public SueetieSortBy SortBy { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {

            string path = this.ViewName.ControlPath();

            List<SueetieContentPage> sueetieContentPages = (from _page in SueetieContentParts.GetSueetieContentPageList(ContentGroupID)
                                                            orderby _page.DisplayOrder ascending
                                                            select _page).ToList();
            if (SortBy == SueetieSortBy.ItemTitle)
                sueetieContentPages = sueetieContentPages.OrderBy(p => p.PageTitle).ToList();

            if (SortBy == SueetieSortBy.ChronologicalOrder)
                sueetieContentPages = sueetieContentPages.OrderBy(p => p.LastUpdateDateTime).ToList();

            foreach (SueetieContentPage sueetieContentPage in sueetieContentPages)
            {
                Sueetie.Controls.ContentPageView control = (Sueetie.Controls.ContentPageView)LoadControl(path);
                control.ContentPage = sueetieContentPage;
                bool userAuthorized = SueetieUIHelper.IsUserAuthorized(sueetieContentPage.ReaderRoles) && sueetieContentPage.IsPublished;
                bool userIsEditor = SueetieUIHelper.IsUserAuthorized(sueetieContentPage.EditorRoles);
                if (userAuthorized || userIsEditor)
                    phContentPages.Controls.Add(control);
            }

        }

    }
}