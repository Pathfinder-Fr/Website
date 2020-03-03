/* Yet Another Forum.NET
 * Copyright (C) 2006-2009 Jaben Cargman
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
using System;
using System.Web.UI;
using YAF.Classes;
using YAF.Classes.Core;
using YAF.Classes.Data;
using YAF.Classes.Utils;
using YAF.Controls;

namespace Sueetie.Forums.Controls
{

    public class ForumLink : BaseControl
    {
        public ForumLink()
        {

        }
        protected LocalizedLabel _localizedLabel = new LocalizedLabel();

        #region Properties

        public ForumPageLinkTo LinkTo
        {
            get { return (ForumPageLinkTo)(ViewState["LinkTo"] ?? ForumPageLinkTo.Nothing); }
            set { ViewState["LinkTo"] = value; }
        }

        /// <summary>
        /// Localized Page for the hyperlink text page
        /// </summary>
        public string TextLocalizedPage
        {
            get { return (_localizedLabel.LocalizedPage != null) ? _localizedLabel.LocalizedPage : "SUEETIE"; }
            set { _localizedLabel.LocalizedPage = value; }
        }

        /// <summary>
        /// Localized Tag for the hyperlink text
        /// </summary>
        public string TextLocalizedTag
        {
            get { return _localizedLabel.LocalizedTag; }
            set { _localizedLabel.LocalizedTag = value; }
        }

        /// <summary>
        /// Localized Page for the optional link description (title)
        /// </summary>
        public string TitleLocalizedPage
        {
            get { return (ViewState["TitleLocalizedPage"] != null) ? ViewState["TitleLocalizedPage"] as string : "SUEETIE"; }
            set { ViewState["TitleLocalizedPage"] = value; }
        }

        /// <summary>
        /// Localized Tag for the optional link description (title)
        /// </summary>
        public string TitleLocalizedTag
        {
            get { return (ViewState["TitleLocalizedTag"] != null) ? ViewState["TitleLocalizedTag"] as string : string.Empty; }
            set { ViewState["TitleLocalizedTag"] = value; }
        }

        /// <summary>
        /// Non-localized Title for optional link description
        /// </summary>
        public string TitleNonLocalized
        {
            get { return (ViewState["TitleNonLocalized"] != null) ? ViewState["TitleNonLocalized"] as string : string.Empty; }
            set { ViewState["TitleNonLocalized"] = value; }
        }

        /// <summary>
        /// Setting the link property will make this control non-postback.
        /// </summary>
        public string NavigateUrl
        {
            get { return (ViewState["NavigateUrl"] != null) ? ViewState["NavigateUrl"] as string : string.Empty; }
            set { ViewState["NavigateUrl"] = value; }
        }

        /// <summary>
        /// Make the link target "blank" to open in a new window.
        /// </summary>
        public bool BlankTarget
        {
            get
            {
                if (ViewState["BlankTarget"] != null)
                {
                    return Convert.ToBoolean(ViewState["BlankTarget"]);
                }
                return false;
            }
            set
            {
                ViewState["BlankTarget"] = value;
            }
        }

        public string Style
        {
            get
            {
                if (ViewState["Style"] != null)
                {
                    return ViewState["Style"].ToString();
                }
                return string.Empty;
            }
            set
            {
                ViewState["Style"] = value;
            }
        }

        #endregion

        #region Render

        protected override void Render(HtmlTextWriter output)
        {
            string title = GetLocalizedTitle();
            string text = GetLocalizedText();

            output.BeginRender();
            output.WriteBeginTag("a");
            if (!String.IsNullOrEmpty(this.ClientID)) output.WriteAttribute("id", this.ClientID);
            if (!String.IsNullOrEmpty(NavigateUrl))
            {
                output.WriteAttribute("href", NavigateUrl.Replace("&", "&amp;"));
            }
            else if (this.LinkTo != ForumPageLinkTo.Nothing)
            { 
                    output.WriteAttribute("href", YafBuildLink.GetLink(GetForumLinkTo()));
            }

            output.WriteAttribute("style", HtmlEncode(Style));
            if (BlankTarget) output.WriteAttribute("target", "_blank");
            if (!String.IsNullOrEmpty(OnClick)) output.WriteAttribute("onclick", OnClick);
            if (!String.IsNullOrEmpty(OnMouseOver)) output.WriteAttribute("onmouseover", OnMouseOver);
            if (!String.IsNullOrEmpty(CssClass)) output.WriteAttribute("class", CssClass);
            if (!String.IsNullOrEmpty(title))
            {
                output.WriteAttribute("title", title);
            }
            else if (!String.IsNullOrEmpty(TitleNonLocalized))
            {
                output.WriteAttribute("title", TitleNonLocalized);
            }
            output.Write(HtmlTextWriter.TagRightChar);
            output.WriteEncodedText(text);
            output.WriteEndTag("a");

            if (!String.IsNullOrEmpty(PostfixText))
            {
                output.Write(PostfixText);
            }

            output.EndRender();
        }

        #endregion

        #region Helpers

        protected ForumPages GetForumLinkTo()
        {
            switch (this.LinkTo)
            {
                case ForumPageLinkTo.Forums:
                    return ForumPages.forum;
                case ForumPageLinkTo.RecentActivity:
                    return ForumPages.su_recent;
                default:
                    break;
            }
            return ForumPages.nothing;
        }

        protected string GetLocalizedTitle()
        {
            if (this.Site != null && this.Site.DesignMode == true && !String.IsNullOrEmpty(TitleLocalizedTag))
            {
                return String.Format("[TITLE:{0}]", TitleLocalizedTag);
            }
            else if (!String.IsNullOrEmpty(TitleLocalizedPage) && !String.IsNullOrEmpty(TitleLocalizedTag))
            {
                return PageContext.Localization.GetText(TitleLocalizedPage, TitleLocalizedTag);
            }
            else if (!String.IsNullOrEmpty(TitleLocalizedTag))
            {
                return PageContext.Localization.GetText(TitleLocalizedTag);
            }

            return null;
        }


        protected string GetLocalizedText()
        {
            if (this.Site != null && this.Site.DesignMode == true && !String.IsNullOrEmpty(TextLocalizedTag))
            {
                return String.Format("[TITLE:{0}]", TextLocalizedTag);
            }
            else if (!String.IsNullOrEmpty(TextLocalizedPage) && !String.IsNullOrEmpty(TextLocalizedTag))
            {
                return PageContext.Localization.GetText(TextLocalizedPage, TextLocalizedTag);
            }
            else if (!String.IsNullOrEmpty(TextLocalizedTag))
            {
                return PageContext.Localization.GetText(TextLocalizedTag);
            }

            return null;
        }
        #endregion

        public string CssClass
        {
            get
            {
                if (ViewState["CssClass"] != null)
                {
                    return ViewState["CssClass"].ToString();
                }
                return string.Empty;
            }
            set
            {
                ViewState["CssClass"] = value;
            }
        }

        #region Client Events

        /// <summary>
        /// The onclick value for the profile link
        /// </summary>
        public string OnClick
        {
            get
            {
                if (ViewState["OnClick"] != null)
                {
                    return ViewState["OnClick"].ToString();
                }
                return string.Empty;
            }
            set
            {
                ViewState["OnClick"] = value;
            }
        }

        /// <summary>
        /// The onmouseover value for the profile link
        /// </summary>
        public string OnMouseOver
        {
            get
            {
                if (ViewState["OnMouseOver"] != null)
                {
                    return ViewState["OnMouseOver"].ToString();
                }
                return string.Empty;
            }
            set
            {
                ViewState["OnMouseOver"] = value;
            }
        }

        /// <summary>
        /// The name of the user for this profile link
        /// </summary>
        public string PostfixText
        {
            get
            {
                if (ViewState["PostfixText"] != null)
                {
                    return ViewState["PostfixText"].ToString();
                }
                return string.Empty;
            }
            set
            {
                ViewState["PostfixText"] = value;
            }
        }

        #endregion

                public enum ForumPageLinkTo
        {
            Forums,
            RecentActivity,
            Nothing
        }

    }
}
