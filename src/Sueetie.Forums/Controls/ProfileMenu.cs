/* Yet Another Forum.NET
 * Copyright (C) 2006-2011 Jaben Cargman
 * http://www.yetanotherforum.net/
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */

namespace Sueetie.Forums.Controls
{
    #region Using

    using System.Text;
    using System.Web.UI;

    using YAF.Core;
    using YAF.Types.Interfaces;
    using YAF.Types.Constants;
    using YAF.Utils;
    using YAF.Types;
    using YAF.Controls;

    using Sueetie.Licensing;
    using Sueetie.Core;

    #endregion

    /// <summary>
    /// Summary description for ForumUsers.
    /// </summary>
    public class ProfileMenu : BaseControl
    {
        #region Methods

        /// <summary>
        /// The render.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        protected override void Render([NotNull] HtmlTextWriter writer)
        {
            var html = new StringBuilder(2000);

            html.Append(@"<table cellspacing=""0"" cellpadding=""0"" class=""content"" id=""yafprofilemenu"">");

            if (this.PageContext.BoardSettings.AllowPrivateMessages)
            {
                html.AppendFormat(@"<tr class=""header2""><td>{0}</td></tr>", this.GetText("MESSENGER"));
                html.AppendFormat(@"<tr><td class=""post""><ul id=""yafprofilemessenger"">");
                html.AppendFormat(
                  @"<li><a href=""{0}"">{1}</a></li>",
                  YafBuildLink.GetLink(ForumPages.cp_pm, "v=in"),
                  this.GetText("INBOX"));
                html.AppendFormat(
                  @"<li><a href=""{0}"">{1}</a></li>",
                  YafBuildLink.GetLink(ForumPages.cp_pm, "v=out"),
                  this.GetText("SENTITEMS"));
                html.AppendFormat(
                  @"<li><a href=""{0}"">{1}</a></li>",
                  YafBuildLink.GetLink(ForumPages.cp_pm, "v=arch"),
                  this.GetText("ARCHIVE"));
                html.AppendFormat(
                  @"<li><a href=""{0}"">{1}</a></li>",
                  YafBuildLink.GetLink(ForumPages.pmessage),
                  this.GetText("NEW_MESSAGE"));
                html.AppendFormat(@"</ul></td></tr>");
            }

            html.AppendFormat(
              @"<tr class=""header2""><td>{0}</td></tr>", this.GetText("PERSONAL_PROFILE"));
            html.AppendFormat(@"<tr><td class=""post""><ul id=""yafprofilepersonal"">");
            html.AppendFormat(
              @"<li><a href=""{0}"">{1}</a></li>",
              YafBuildLink.GetLink(ForumPages.profile, "u={0}", PageContext.PageUserID),
              this.GetText("VIEW_PROFILE"));
            html.AppendFormat(
              @"<li><a href=""{0}"">{1}</a></li>",
              YafBuildLink.GetLink(ForumPages.cp_editprofile),
              this.GetText("EDIT_PROFILE"));
            if (!this.PageContext.IsGuest && this.PageContext.BoardSettings.EnableThanksMod)
            {
                html.AppendFormat(
                  @"<li><a href=""{0}"">{1}</a></li>",
                  YafBuildLink.GetLink(ForumPages.viewthanks, "u={0}", PageContext.PageUserID),
                  this.GetText("ViewTHANKS", "TITLE"));
            }

            if (!this.PageContext.IsGuest && this.PageContext.BoardSettings.EnableBuddyList & this.PageContext.UserHasBuddies)
            {
                html.AppendFormat(
                  @"<li><a href=""{0}"">{1}</a></li>",
                  YafBuildLink.GetLink(ForumPages.cp_editbuddies),
                  this.GetText("EDIT_BUDDIES"));
            }

            if (!this.PageContext.IsGuest && (this.PageContext.BoardSettings.EnableAlbum || (this.PageContext.NumAlbums > 0)))
            {
                html.AppendFormat(
                  @"<li><a href=""{0}"">{1}</a></li>",
                  YafBuildLink.GetLink(ForumPages.albums, "u={0}", this.PageContext.PageUserID),
                  this.GetText("EDIT_ALBUMS"));
            }

            html.AppendFormat(
              @"<li><a href=""{0}"">{1}</a></li>",
              YafBuildLink.GetLink(ForumPages.cp_editavatar),
              this.GetText("EDIT_AVATAR"));
            if (this.PageContext.BoardSettings.AllowSignatures)
            {
                html.AppendFormat(
                  @"<li><a href=""{0}"">{1}</a></li>",
                  YafBuildLink.GetLink(ForumPages.cp_signature),
                  this.GetText("SIGNATURE"));
            }

            html.AppendFormat(
              @"<li><a href=""{0}"">{1}</a></li>",
              YafBuildLink.GetLink(ForumPages.cp_subscriptions),
              this.GetText("SUBSCRIPTIONS"));
            if (this.PageContext.BoardSettings.AllowPasswordChange)
            {
                html.AppendFormat(
                  @"<li><a href=""{0}"">{1}</a></li>",
                  YafBuildLink.GetLink(ForumPages.cp_changepassword),
                  this.GetText("CHANGE_PASSWORD"));
            }

            // Sueetie Modified - Add View Purchase Link

            //if (!this.PageContext.IsGuest &&
            //    (LicensingCommon.IsValidLicense(SueetiePackageType.Marketplace)))
            //{
            //    html.AppendFormat(@"<li><a href=""{0}"">{1}</a></li>", YafBuildLink.GetLink(ForumPages.su_purchases, "u={0}", this.PageContext.PageUserID),
            //        SueetieLocalizer.GetForumString("profilemenu_licenses"));
            //}

            html.AppendFormat(@"</ul></td></tr>");
            html.Append(@"</table>");

            writer.Write(html.ToString());
        }

        #endregion
    }
}