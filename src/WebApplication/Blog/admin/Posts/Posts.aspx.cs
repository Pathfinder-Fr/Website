namespace BlogEngine.Admin.Posts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using BlogEngine.Core;
    using BlogEngine;

    public partial class Posts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebUtils.CheckRightsForAdminPostPages(false);
        }
    }
}