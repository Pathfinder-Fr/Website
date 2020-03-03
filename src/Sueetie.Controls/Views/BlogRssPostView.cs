using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class BlogRssPostView : SueetieBaseControl
    {

        public virtual SueetieRssBlogPost Post
        {
            get { return (SueetieRssBlogPost)(ViewState["RssPost"] ?? default(SueetieRssBlogPost)); }
            set { ViewState["RssPost"] = value; }
        }

    }
}
