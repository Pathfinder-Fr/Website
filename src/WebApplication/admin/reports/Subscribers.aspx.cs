using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sueetie.Core;

namespace Sueetie.Web 
{
    public partial class Subscribers : SueetieAdminPage
    {
        public Subscribers()
            : base("admin_reports_subscribers")
        {
        }

  
        protected void Page_Load(object sender, EventArgs e)
        {
            List<SueetieSubscriber> sueetieSubscribers = SueetieUsers.GetSueetieSubscriberList();

            lblSubscriberCount.Text = sueetieSubscribers.Count.ToString();
            string emailAddresses = string.Empty;
            string displayNames = string.Empty;
            if (sueetieSubscribers.Count > 0)
            {
                foreach (SueetieSubscriber sueetieSubscriber in sueetieSubscribers)
                {
                    emailAddresses += sueetieSubscriber.Email + ", ";
                    displayNames += sueetieSubscriber.DisplayName + ", ";
                }
                lblEmailAddresses.Text = emailAddresses.Substring(0, emailAddresses.LastIndexOf(","));
                lblDisplayNames.Text = displayNames.Substring(0, displayNames.LastIndexOf(","));
            }
            else
            {
                lblEmailAddresses.Text = "No email addresses yet recorded";
                lblDisplayNames.Text = "No subscriber names have yet been recorded";
            }

        }
    }
}
