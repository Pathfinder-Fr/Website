
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using System.Web.UI.HtmlControls;

namespace Sueetie.Controls
{
    public class TagCloud : SueetieBaseControl
    {

        #region Properties

        private int _applicationTypeID = 0;
        public int ApplicationTypeID
        {
            get { return _applicationTypeID; }
            set { _applicationTypeID = value; }
        }


        private int _cloudTagNum = 100;
        public int CloudTagNum
        {
            get { return _cloudTagNum; }
            set { _cloudTagNum = value; }
        }

        private int _applicationID = 0;
        public int ApplicationID
        {
            get { return _applicationID; }
            set { _applicationID = value; }
        }

        private string _cssClass = "ulTags";
        public string CssClass
        {
            get { return _cssClass; }
            set { _cssClass = value; }
        }

        public virtual CloudApplicationType TagCloudContents
        {
            get { return (CloudApplicationType)(ViewState["TagCloudContents"] ?? CloudApplicationType.All); }
            set { ViewState["TagCloudContents"] = value; }
        }

        public enum CloudApplicationType
        {
            All = 0,
            Blogs = 1,
            Forums = 2,
            Wiki = 3,
            MediaGallery = 4,
            Marketplace = 5,
            Classifieds = 6,
            CMS = 7
        }

        #endregion

        #region Constants

        private const string LINK = "<a href=\"{0}\" class=\"{1}\">{2}</a> ";

        #endregion

        #region Protected Methods

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                SueetieTagQuery _query = new SueetieTagQuery
                {
                    ApplicationTypeID = this.ApplicationTypeID == 0 ? (int)TagCloudContents : this.ApplicationTypeID,
                    ApplicationID = this.ApplicationID,
                    CloudTagNum = this.CloudTagNum,
                    IsRestricted = true
                };

                HtmlGenericControl ulTags = new HtmlGenericControl("ul");
                ulTags.Attributes.Add("class",this.CssClass);
                this.Controls.Add(ulTags);

                List<SueetieTag> CloudTags = SueetieTags.GetSueetieTagCloudList(_query);
                int maxTagCount = 0;
                foreach (SueetieTag _tag in CloudTags)
                {
                    if (_tag.TagCount > maxTagCount)
                        maxTagCount = _tag.TagCount;
                }

                foreach (SueetieTag tag in CloudTags)
                {
                    HtmlGenericControl li = new HtmlGenericControl("li");
                    li.InnerHtml = string.Format(LINK, "/search/default.aspx?srch=Tags:" +
                        DataHelper.PrepareTag(tag.Tag), SueetieTags.TagWeightClass(tag.TagCount, maxTagCount), tag.Tag);
                    ulTags.Controls.Add(li);
                }
            }
        }

        #endregion

    }
}
