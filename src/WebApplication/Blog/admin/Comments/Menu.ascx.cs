// -----------------------------------------------------------------------
// <copyright file="Menu.ascx.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace BlogEngine.Admin.Comments
{
    using System;
    using System.Web.UI;
    using Core;

    /// <summary>
    /// The admin comments menu.
    /// </summary>
    public partial class Menu : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetCounters();
        }

        /// <summary>
        /// Comment counter
        /// </summary>
        protected int CommentCount { get; set; }

        /// <summary>
        /// Pingback/trackback counter
        /// </summary>
        protected int PingbackCount { get; set; }

        /// <summary>
        /// Spam counter
        /// </summary>
        protected int SpamCount { get; set; }

        /// <summary>
        /// Pending approval
        /// </summary>
        protected int PendingCount { get; set; }

        /// <summary>
        /// Indicate that menu item selected
        /// </summary>
        /// <param name="pg">Page address</param>
        /// <returns>CSS class to append for current menu item</returns>
        protected string Current(string pg)
        {
            if (this.Request.Path.ToLower().Contains(pg.ToLower()))
            {
                return "class=\"content-box-selected\"";
            }
            return "";
        }

        /// <summary>
        /// Gets the cookie with visitor information if any is set.
        ///     Then fills the contact information fields in the form.
        /// </summary>
        private void SetCounters()
        {
            foreach (Post p in Post.Posts)
            {
                this.PendingCount += p.NotApprovedComments.Count;
                this.PingbackCount += p.Pingbacks.Count;
                this.CommentCount += p.ApprovedComments.Count;
                this.SpamCount += p.SpamComments.Count;
            }
        }
    }
}