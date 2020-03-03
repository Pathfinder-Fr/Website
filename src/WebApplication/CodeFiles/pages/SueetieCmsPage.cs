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

namespace Sueetie.Web
{
    public partial class SueetieCmsPage : SueetieBaseThemedPage
    {

        public SueetieCmsPage()
            : base()
        {
            this.SueetieMasterPage = "cms.master";
            CurrentContentID = SueetieContext.Current.ContentPage.ContentID;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SueetieContentPage _currentPage = SueetieContext.Current.ContentPage;
            bool userAuthorized = SueetieUIHelper.IsUserAuthorized(_currentPage.ReaderRoles) && _currentPage.IsPublished;
            bool userIsEditor = SueetieUIHelper.IsUserAuthorized(_currentPage.EditorRoles);
            if (userAuthorized || userIsEditor)
            {
                // page displays normally
            }
            else
                Response.Redirect("/members/message.aspx?msgid=10");
        }
    }

}
