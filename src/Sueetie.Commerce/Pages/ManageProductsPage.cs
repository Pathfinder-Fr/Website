namespace Sueetie.Commerce.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Core;

    public class ManageProductsPage : MarketplaceAdminPage
    {
        protected Repeater rptProducts;

        public ManageProductsPage()
            : base("admin_marketplace_manageproducts")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                base.CurrentProductCategory = null;
                if (DataHelper.GetIntFromQueryString("c", -1) > 0)
                {
                    this.SetupForCategory();
                }
                else
                {
                    this.SetupForAllProducts();
                }
            }
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
                Image image2 = e.Item.FindControl("imgIsActive") as Image;
                image2.ImageUrl = "/images/shared/sueetie/true.png";
                if (dataItem.StatusTypeID != 100)
                {
                    image2.ImageUrl = "/images/shared/sueetie/false.png";
                }
                HyperLink link = e.Item.FindControl("hlProduct") as HyperLink;
                link.NavigateUrl = string.Format("EditProduct.aspx?id={0}", dataItem.ProductID);
                HyperLink link2 = e.Item.FindControl("hlCategory") as HyperLink;
                link2.NavigateUrl = string.Format("ManageProducts.aspx?c={0}", dataItem.CategoryID);
            }
        }

        private void SetupForAllProducts()
        {
            List<SueetieProduct> sueetieProductList = Products.GetSueetieProductList(false);
            this.rptProducts.DataSource = sueetieProductList;
            this.rptProducts.DataBind();
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

