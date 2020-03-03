namespace Sueetie.Commerce.Pages
{
    using Sueetie.Commerce;
    using System;
    using System.Web.UI.WebControls;

    public class ActivityReportPage : MarketplaceAdminPage
    {
        protected Repeater rptRecentActivity;

        public ActivityReportPage() : base("admin_marketplace_activityreport")
        {
        }

        private void DisplayByCategory(int _categoryID)
        {
            this.rptRecentActivity.DataSource = Purchases.GetPurchasesByCategoryID(_categoryID);
            this.rptRecentActivity.DataBind();
        }

        private void DisplayByProductID(int _productID)
        {
            this.rptRecentActivity.DataSource = Purchases.GetPurchasesByProductID(_productID);
            this.rptRecentActivity.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                if (base.Request.QueryString["c"] != null)
                {
                    int num = int.Parse(base.Request.QueryString["c"].ToString());
                    this.DisplayByCategory(num);
                }
                else if (base.Request.QueryString["p"] != null)
                {
                    int num2 = int.Parse(base.Request.QueryString["p"].ToString());
                    this.DisplayByProductID(num2);
                }
                else
                {
                    this.rptRecentActivity.DataSource = Purchases.GetProductPurchaseList();
                    this.rptRecentActivity.DataBind();
                }
            }
        }

        protected void rptRecentActivity_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                ProductPurchase dataItem = (ProductPurchase) e.Item.DataItem;
                Label label = e.Item.FindControl("lblTransactionXID") as Label;
                if (dataItem.TransactionXID != string.Empty)
                {
                    label.Text = dataItem.TransactionXID;
                }
                else
                {
                    label.Visible = false;
                }
                Label label2 = e.Item.FindControl("lblPrice") as Label;
                if (dataItem.Price > 0M)
                {
                    label2.Text = string.Format("${0:#,#.00}", dataItem.Price);
                }
                else
                {
                    label2.Text = "&nbsp;&nbsp;&nbsp;-";
                }
            }
        }
    }
}

