namespace Sueetie.Commerce.Pages
{
    using Sueetie.Commerce;
    using System;
    using System.Runtime.CompilerServices;
    using System.Web.UI.WebControls;

    public class PaypalStandardPage : MarketplaceAdminPage
    {
        protected Label lblPaymentServiceName;
        protected Label lblResults;
        protected TextBox txtAccountName;
        protected TextBox txtCartUrl;
        protected TextBox txtIdentityToken;
        protected TextBox txtPurchaseUrl;
        protected TextBox txtReturnUrl;
        protected TextBox txtShoppingUrl;
        protected TextBox txtTransactionUrl;

        public PaypalStandardPage() : base("admin_marketplace_paypalstandard")
        {
        }

        protected void btnUpdate_OnCommand(object sender, CommandEventArgs e)
        {
            Payments.UpdatePaymentServiceSetting(new PaymentServiceSetting(this.paymentServiceID, "AccountName", this.txtAccountName.Text));
            Payments.UpdatePaymentServiceSetting(new PaymentServiceSetting(this.paymentServiceID, "PurchaseUrl", this.txtPurchaseUrl.Text));
            Payments.UpdatePaymentServiceSetting(new PaymentServiceSetting(this.paymentServiceID, "CartUrl", this.txtCartUrl.Text));
            Payments.UpdatePaymentServiceSetting(new PaymentServiceSetting(this.paymentServiceID, "ShoppingUrl", this.txtShoppingUrl.Text));
            Payments.UpdatePaymentServiceSetting(new PaymentServiceSetting(this.paymentServiceID, "ReturnUrl", this.txtReturnUrl.Text));
            Payments.UpdatePaymentServiceSetting(new PaymentServiceSetting(this.paymentServiceID, "IdentityToken", this.txtIdentityToken.Text));
            Payments.UpdatePaymentServiceSetting(new PaymentServiceSetting(this.paymentServiceID, "TransactionUrl", this.txtTransactionUrl.Text));
            this.lblResults.Text = "Payment Service Updated!";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.paymentServiceID = int.Parse(base.Request.QueryString["id"].ToString());
            if (!base.IsPostBack)
            {
                this.PopulateForm(this.paymentServiceID);
            }
        }

        private void PopulateForm(int paymentServiceID)
        {
            PaymentService paymentService = Payments.GetPaymentService(paymentServiceID);
            this.txtAccountName.Text = paymentService.AccountName;
            this.txtReturnUrl.Text = paymentService.ReturnUrl;
            this.txtShoppingUrl.Text = paymentService.ShoppingUrl;
            this.txtIdentityToken.Text = paymentService.IdentityToken;
            this.txtPurchaseUrl.Text = paymentService.PurchaseUrl;
            this.txtCartUrl.Text = paymentService.CartUrl;
            this.txtTransactionUrl.Text = paymentService.TransactionUrl;
        }

        public int paymentServiceID { get; set; }
    }
}

