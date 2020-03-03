
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class ContentPageTitle : WebControl
    {
        #region Constructor

        public ContentPageTitle()
        { }

        #endregion

        public virtual AnchorHtml Anchor
        {
            get { return (AnchorHtml)(ViewState["Anchor"] ?? AnchorHtml.unknown); }
            set { ViewState["Anchor"] = value; }
        }

        public virtual string LanguageFile
        {
            get { return ((string)ViewState["LanguageFile"]) ?? "sueetie.xml"; }
            set { ViewState["LanguageFile"] = value; }
        }

        public virtual bool DisplayCssID
        {
            get
            {
                return (this.ViewState["DisplayCssID"] == null) ? true : (bool)this.ViewState["DisplayCssID"];
            }
            set { ViewState["DisplayCssID"] = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            SueetieContentPage _sueetieContentPage = SueetieContext.Current.ContentPage;
            if (_sueetieContentPage != null)
            {
                bool wrapText = this.Anchor != AnchorHtml.unknown;
                string anchor = this.Anchor.ToString();
                writer.BeginRender();
                if (wrapText)
                {
                    writer.Write("<{0}", anchor);

                    if (this.DisplayCssID)
                        writer.Write(" id=\"{0}\"", "cms_" + _sueetieContentPage.PageKey.ToLower() + "_title");

                    if (!string.IsNullOrEmpty(this.CssClass))
                        writer.Write(" class=\"{0}\"", this.CssClass);

                    if (this.Attributes != null && this.Attributes.Count > 0)
                    {
                        foreach (string key in this.Attributes.Keys)
                            writer.Write(" {0}=\"{1}\"", key, this.Attributes[key]);
                    }
                    writer.Write(">");
                }

                string _key = "cms_" + _sueetieContentPage.PageKey.ToLower() + "_title";
                string _resource = SueetieLocalizer.GetString(_key, this.LanguageFile);
                writer.Write(_resource);

                if (wrapText)
                    writer.Write("</{0}>", anchor);
                writer.EndRender();
            }
        }

        #region AnchorHtml Enum

        public enum AnchorHtml
        {
            acronym,
            b,
            blockquote,
            caption,
            center,
            dd,
            div,
            dl,
            dt,
            em,
            h1,
            h2,
            h3,
            h4,
            h5,
            h6,
            i,
            li,
            ol,
            ul,
            p,
            small,
            span,
            strike,
            strong,
            sub,
            sup,
            u,
            td,
            th,
            label,
            fieldset,
            legend,
            unknown
        }

        #endregion

    }
}
