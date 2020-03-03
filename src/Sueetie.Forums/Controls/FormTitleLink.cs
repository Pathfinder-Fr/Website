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
using YAF.Classes.Data;
using YAF.Classes.Utils;
using YAF.Controls;


namespace Sueetie.Forums.Controls
{

    public class ForumTitleLink : BaseControl
    {
        public ForumTitleLink()
        {

        }

        protected override void Render(HtmlTextWriter output)
        {

            string _text = PageContext.Localization.GetText("SUEETIE", "SITETITLE");
            string _url = YafBuildLink.GetLink(ForumPages.forum);

            if (SueetieForumContext.Current.IsConversationPage)
            {
                _text = "Conversations";
                _url = YafBuildLink.GetLink(ForumPages.cp_pm);
            }
            if (SueetieForumContext.Current.IsDashboardPage)
            {
                _text = CurrentSueetieUser.DisplayName;
                _url = YafBuildLink.GetLink(ForumPages.profile, "u={0}", CurrentSueetieUser.ForumUserID);
            }
            if (SueetieForumContext.Current.IsMemberPage)
            {
                _text = PageContext.Localization.GetText("SUEETIE", "MEMBERS");
                _url = YafBuildLink.GetLink(ForumPages.members);
            }
            output.BeginRender();
            output.WriteBeginTag("a");
            if (!String.IsNullOrEmpty(this.ClientID)) output.WriteAttribute("id", this.ClientID);
            output.WriteAttribute("href", _url);
            output.WriteAttribute("title", _text);
            output.WriteAttribute("style", HtmlEncode(Style));
            if (BlankTarget) output.WriteAttribute("target", "_blank");
            if (!String.IsNullOrEmpty(OnClick)) output.WriteAttribute("onclick", OnClick);
            if (!String.IsNullOrEmpty(OnMouseOver)) output.WriteAttribute("onmouseover", OnMouseOver);
            if (!String.IsNullOrEmpty(CssClass)) output.WriteAttribute("class", CssClass);
 
            output.Write(HtmlTextWriter.TagRightChar);

            output.WriteEncodedText(_text);
            output.WriteEndTag("a");


            if (!String.IsNullOrEmpty(PostfixText))
            {
                output.Write(PostfixText);
            }

            output.EndRender();

        }

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
    }
}
