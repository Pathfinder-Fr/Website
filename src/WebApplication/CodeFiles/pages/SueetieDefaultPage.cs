using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sueetie.Core;

namespace Sueetie.Web
{
    public partial class SueetieDefaultPage : SueetieBaseThemedPage
    {
        public SueetieDefaultPage()
            : base("home_page")
        {
            SueetieMasterPage = "home.master";
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Title = SueetieLocalizer.GetPageTitle("home_page");
        }
    }
}
