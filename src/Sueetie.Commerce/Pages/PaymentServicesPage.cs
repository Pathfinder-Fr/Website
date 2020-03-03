namespace Sueetie.Commerce.Pages
{
    using Sueetie.Commerce;
    using System;
    using System.Web.UI.WebControls;

    public class PaymentServicesPage : MarketplaceAdminPage
    {
        protected Repeater rptPaymentServices;

        public PaymentServicesPage() : base("admin_marketplace_paymentservices")
        {
        }

        private void BindPaymentServices()
        {
            this.rptPaymentServices.DataSource = Payments.GetPaymentServiceList();
            this.rptPaymentServices.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.BindPaymentServices();
            }
        }

        protected void rptPaymentServices_OnCommand(object sender, RepeaterCommandEventArgs e)
        {
            Payments.SetPrimaryPaymentService(int.Parse(e.CommandArgument.ToString()));
            base.Response.Redirect("paymentservices.aspx");
        }

        protected void rptPaymentServices_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                PaymentService dataItem = (PaymentService) e.Item.DataItem;
                HyperLink link = e.Item.FindControl("hlPaymentService") as HyperLink;
                link.Text = dataItem.PaymentServiceName;
                link.NavigateUrl = string.Format("/admin/marketplace/payment/{0}.aspx?id={1}", dataItem.SharedPaymentServicePage, dataItem.PaymentServiceID);
                Button button = e.Item.FindControl("btnMakePrimary") as Button;
                button.CommandArgument = dataItem.PaymentServiceID.ToString();
            }
        }
    }
}

