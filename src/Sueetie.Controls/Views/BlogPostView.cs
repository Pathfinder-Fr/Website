using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class BlogPostView : SueetieBaseControl
    {

        public virtual SueetieBlogPost Post
        {
            get { return (SueetieBlogPost)(ViewState["Post"] ?? default(SueetieBlogPost)); }
            set { ViewState["Post"] = value; }
        }

    }
}
