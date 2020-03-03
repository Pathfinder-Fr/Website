// -----------------------------------------------------------------------
// <copyright file="site.master.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace BlogEngine.Themes.Chiclet
{
    using System;
    using System.Web.UI;
    using Sueetie.Blog;
    using Sueetie.Core;

    public partial class MobileSite : SueetieBaseMasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                this.Page.Header.Controls.Add(AddMetaTag("CommunityFramework", SiteStatistics.Instance.SueetieVersion));
                this.Page.Header.Controls.Add(SueetieBlogUtils.AddBlogEngineThemeCSS());
                this.Page.Header.Controls.Add(MakeStyleSheetControl("/themes/" + this.CurrentTheme + "/style/shared.css"));
                this.Page.Header.Controls.Add(MakeStyleSheetControl("/themes/" + this.CurrentTheme + "/style/blogs.css"));
                this.Page.Header.Controls.Add(new LiteralControl("<!--[if IE]><link rel=\"stylesheet\" href=\"/themes/" + this.CurrentTheme + "/style/ie.css\" type=\"text/css\" /><![endif]-->"));
            }
        }
    }
}