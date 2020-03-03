using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sueetie.Core;
using System.Web;

namespace Sueetie.Controls
{
    public class ContentPageView : SueetieBaseControl
    {

        public virtual SueetieContentPage ContentPage
        {
            get { return (SueetieContentPage)(ViewState["ContentPage"] ?? default(SueetieContentPage)); }
            set { ViewState["ContentPage"] = value; }
        }

        public bool IsCurrentPage()
        {
            bool isCurrentPage = false;
            if (HttpContext.Current.Request.RawUrl == this.ContentPage.Permalink)
                isCurrentPage = true;
            return isCurrentPage;
        }
    }
}
