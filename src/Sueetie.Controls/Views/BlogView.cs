using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class BlogView : SueetieBaseControl
    {

        public virtual SueetieBlog Blog
        {
            get { return (SueetieBlog)(ViewState["Blog"] ?? default(SueetieBlog)); }
            set { ViewState["Blog"] = value; }
        }

    }
}