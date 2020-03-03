using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class BlogCommentView : SueetieBaseControl
    {

        public virtual SueetieBlogComment Comment
        {
            get { return (SueetieBlogComment)(ViewState["Comment"] ?? default(SueetieBlogComment)); }
            set { ViewState["Comment"] = value; }
        }

    }
}
