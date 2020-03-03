namespace Sueetie.Commerce.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Core;

    public class BrowsePage : MarketplaceBasePage
    {
        protected Button CommonWhatsNewButton;
        protected Label lblTitle;
        protected Panel pnlWhatsNew;
        protected Repeater rptProducts;

        public virtual bool BlnDirection
        {
            get
            {
                return ((this.ViewState["blnDirection"] != null) && ((bool)this.ViewState["blnDirection"]));
            }
            set
            {
                this.ViewState["blnDirection"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.pnlWhatsNew.Visible = CommerceConfiguration.Get().Core.ShowWhatsNew;

            if (!this.Page.IsPostBack)
            {
                if (DataHelper.GetIntFromQueryString("c", -1) > 0)
                {
                    this.SetupForCategory();
                }
                else
                {
                    this.NoCategory();
                }
            }

            if (base.CurrentProductCategory != null)
            {
                this.lblTitle.Text = base.CurrentProductCategory.CategoryName;
            }
            else
            {
                this.lblTitle.Text = Properties.Resources.Products_All;
            }

            this.Page.Title = SueetieContext.Current.SiteSettings.SiteName + " : " + this.lblTitle.Text;
        }

        protected void AdsGrid_OnSorting(object sender, GridViewSortEventArgs e)
        {
            this.BlnDirection = !this.BlnDirection;
            var blnDirection = this.BlnDirection;

            var activeSueetieProducts = Products.GetActiveSueetieProducts();
            activeSueetieProducts.Sort(new SueetieComparer<SueetieProduct>(e.SortExpression, e.SortDirection));

            this.rptProducts.DataSource = activeSueetieProducts;
            this.rptProducts.DataBind();
        }

        protected void CommonWhatsNewButton_Click(object sender, EventArgs e)
        {
            this.CommonWhatsNewButton.CommandArgument = "CrossPagePost";
        }

        private void NoCategory()
        {
            base.CurrentProductCategory = null;
            List<SueetieProduct> activeSueetieProducts = Products.GetActiveSueetieProducts();
            this.rptProducts.DataSource = activeSueetieProducts;
            this.rptProducts.DataBind();
        }

        protected void rptProducts_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                SueetieProduct dataItem = (SueetieProduct)e.Item.DataItem;

                Image image = e.Item.FindControl("imgProductPhoto") as Image;

                if (dataItem.PreviewImageID == -1)
                {
                    image.Visible = false;
                }
                else
                {
                    image.ImageUrl = string.Format("/images/products/{0}.Md.jpg", dataItem.ProductID);
                }

                HyperLink link = e.Item.FindControl("hlProduct") as HyperLink;
                link.NavigateUrl = string.Format("ShowProduct.aspx?id={0}", dataItem.ProductID);
                HyperLink link2 = e.Item.FindControl("hlCategory") as HyperLink;
                link2.NavigateUrl = string.Format("Browse.aspx?c={0}", dataItem.CategoryID);
            }
        }

        private void SetupForCategory()
        {
            base.CurrentProductCategory = ProductCategories.GetProductCategory(int.Parse(base.Request.QueryString["c"].ToString()));
            List<SueetieProduct> sueetieProductsByCategory = Products.GetSueetieProductsByCategory(base.CurrentProductCategory.CategoryID, true);
            this.rptProducts.DataSource = sueetieProductsByCategory;
            this.rptProducts.DataBind();
        }
    }
}

