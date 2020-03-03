namespace Sueetie.Commerce.Pages
{
    using Sueetie.Commerce;
    using Sueetie.Core;
    using System;
    using System.Collections.Generic;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    public class MarketplaceDefaultPage : MarketplaceBasePage
    {
        protected HtmlGenericControl borderDiv;
        protected Panel pnlCategoryBrowser;
        protected Panel pnlProductsDisplay;
        protected Repeater rptProducts;

        private void NoCategory()
        {
            base.CurrentProductCategory = null;
            List<SueetieProduct> activeSueetieProducts = Products.GetActiveSueetieProducts();
            this.rptProducts.DataSource = activeSueetieProducts;
            this.rptProducts.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (DataHelper.GetIntFromQueryString("c", -1) > 0)
                {
                    this.SetupForCategory();
                }
                else
                {
                    this.pnlProductsDisplay.Visible = false;
                    this.NoCategory();
                }
            }
        }

        protected void rptProducts_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                SueetieProduct dataItem = (SueetieProduct) e.Item.DataItem;
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

        public virtual bool blnDirection
        {
            get
            {
                return ((this.ViewState["blnDirection"] != null) && ((bool) this.ViewState["blnDirection"]));
            }
            set
            {
                this.ViewState["blnDirection"] = value;
            }
        }

        public virtual int CurrentProductCategoryID
        {
            get
            {
                if (this.ViewState["CurrentProductCategoryID"] != null)
                {
                    return (int) this.ViewState["CurrentProductCategoryID"];
                }
                return -1;
            }
            set
            {
                this.ViewState["CurrentProductCategoryID"] = value;
            }
        }
    }
}

