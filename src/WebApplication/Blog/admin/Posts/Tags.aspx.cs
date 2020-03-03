namespace BlogEngine.Admin.Posts
{
    using System;
    using BlogEngine.Core;
    using BlogEngine;

    public partial class Tags : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebUtils.CheckRightsForAdminPostPages(false);
        }
    }
}