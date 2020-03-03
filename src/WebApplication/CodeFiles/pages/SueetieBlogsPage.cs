using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;
using Sueetie.Controls;

namespace Sueetie.Web
{
    public partial class SueetieBlogsPage : SueetieBaseThemedPage
    {

        protected SueetieLink SueetieLink1;
        protected SueetieLink SueetieLink2;
        protected SueetieLink SueetieLink3;
        protected AggregateBlogListWebControl AggregateBlogList1;
        protected AggregateBlogPostListWebControl AggregateBlogPostList2;

        public SueetieBlogsPage()
            : base("blogs_default")
        {
            this.SueetieMasterPage = "blogs.master";
        }

    }

}
