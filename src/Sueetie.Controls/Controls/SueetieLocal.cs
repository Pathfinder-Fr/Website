using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Controls
{

    public class SueetieLocal : WebControl
    {

        #region Properties

        public virtual string LocalCssID
        {
            get { return ((string)ViewState["LocalCssID"]) ?? string.Empty; }
            set { ViewState["LocalCssID"] = value; }
        }
        public virtual string Key
        {
            get { return ((string)ViewState["Key"]) ?? string.Empty; }
            set { ViewState["Key"] = value; }
        }
        public virtual string LanguageFile
        {
            get { return ((string)ViewState["LanguageFile"]) ?? string.Empty; }
            set { ViewState["LanguageFile"] = value; }
        }
        public virtual string Parameter1
        {
            get { return ((string)ViewState["Parameter1"]) ?? string.Empty; }
            set { ViewState["Parameter1"] = value; }
        }
        public virtual string Parameter2
        {
            get { return ((string)ViewState["Parameter2"]) ?? string.Empty; }
            set { ViewState["Parameter2"] = value; }
        }
        public virtual string Parameter3
        {
            get { return ((string)ViewState["Parameter3"]) ?? string.Empty; }
            set { ViewState["Parameter3"] = value; }
        }

        public virtual bool UseCurrentApplicationFile
        {
            get
            {
                return (this.ViewState["UseCurrentApplicationFile"] == null) ? false : (bool)this.ViewState["UseCurrentApplicationFile"];
            }
            set { ViewState["UseCurrentApplicationFile"] = value; }
        }
        public virtual AnchorHtml Anchor
        {
            get { return (AnchorHtml)(ViewState["Anchor"] ?? AnchorHtml.unknown); }
            set { ViewState["Anchor"] = value; }
        }

        #endregion

        #region Constructor

        public SueetieLocal()
        { }

        #endregion

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

        #region Render

        protected override void Render(HtmlTextWriter writer)
        {
            bool wrapText = this.Anchor != AnchorHtml.unknown;
            string anchor = this.Anchor.ToString();
            writer.BeginRender();
            if (wrapText)
            {
                writer.Write("<{0}", anchor);

                if (!string.IsNullOrEmpty(this.LocalCssID))
                    writer.Write(" id=\"{0}\"", this.LocalCssID);

                if (!string.IsNullOrEmpty(this.CssClass))
                    writer.Write(" class=\"{0}\"", this.CssClass);

                if (this.Attributes != null && this.Attributes.Count > 0)
                {
                    foreach (string key in this.Attributes.Keys)
                        writer.Write(" {0}=\"{1}\"", key, this.Attributes[key]);
                }
                writer.Write(">");
            }


            string _resource = SueetieLocalizer.GetString(this.Key, this.LanguageFile);
            if (this.UseCurrentApplicationFile)
                _resource = SueetieLocalizer.GetString(this.Key, true);
            if (!string.IsNullOrEmpty(this.Parameter1))
            {
                string[] _params = new string[] { this.Parameter1, this.Parameter2, this.Parameter3 };
                _resource = string.Format(_resource, _params);
            }
            writer.Write(_resource);

            if (wrapText)
                writer.Write("</{0}>", anchor);
            writer.EndRender();
        }

        #endregion

    }
}
