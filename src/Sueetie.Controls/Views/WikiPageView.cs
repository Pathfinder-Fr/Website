using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;

namespace Sueetie.Controls
{
    public class WikiPageView : SueetieBaseControl
    {

        public virtual SueetieWikiPage WikiPage
        {
            get { return (SueetieWikiPage)(ViewState["WikiPage"] ?? default(SueetieWikiPage)); }
            set { ViewState["WikiPage"] = value; }
        }

    }
}
