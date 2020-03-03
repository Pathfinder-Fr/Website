// -----------------------------------------------------------------------
// <copyright file="Pending.aspx.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace BlogEngine.Admin.Comments
{
    using System;
    using System.Collections;
    using System.Web.Services;
    using System.Web.UI;
    using Core.Json;

    public partial class Pending : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WebUtils.CheckRightsForAdminCommentsPages(false);
        }

        /// <summary>
        /// Number of comments in the list
        /// </summary>
        protected static int CommentCounter { get; set; }

        [WebMethod]
        public static IEnumerable LoadComments(int page)
        {
            WebUtils.CheckRightsForAdminCommentsPages(false);

            var commentList = JsonComments.GetComments(CommentType.Pending, page);
            CommentCounter = commentList.Count;
            return commentList;
        }

        [WebMethod]
        public static string LoadPager(int page)
        {
            WebUtils.CheckRightsForAdminCommentsPages(false);

            return JsonComments.GetPager(page);
        }
    }
}