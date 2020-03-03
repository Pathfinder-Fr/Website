namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Runtime.CompilerServices;

    public class MarketplaceBasePage : SueetieBasePage
    {
        protected override void OnLoad(EventArgs e)
        {
            if (string.IsNullOrEmpty(base.Title))
            {
                base.Title = "Pathfinder-fr.org";
                if (this.CurrentProductCategory != null)
                {
                    base.Title = this.CurrentProductCategory.CategoryName + " - " + this.Title;
                }

                if (this.CurrentSueetieProduct != null)
                {
                    base.Title = this.CurrentSueetieProduct.Title + " - " + this.Title;
                    base.CurrentContentID = this.CurrentSueetieProduct.ContentID;
                }
            }

            base.OnLoad(e);
        }

        protected override void OnPreInit(EventArgs e)
        {
            if (DataHelper.GetIntFromQueryString("c", -1) > 0)
            {
                this.CurrentProductCategory = ProductCategories.GetProductCategory(int.Parse(base.Request.QueryString["c"].ToString()));
            }
            if (DataHelper.GetIntFromQueryString("id", -1) > 0)
            {
                this.CurrentSueetieProduct = Products.GetSueetieProduct(int.Parse(base.Request.QueryString["id"].ToString()));
                this.CurrentProductCategory = ProductCategories.GetProductCategory(this.CurrentSueetieProduct.CategoryID);
            }
            string str = this.SueetieMasterPage ?? "marketplace.master";
            this.MasterPageFile = "~/themes/" + SueetieContext.Current.SiteSettings.Theme + "/masters/" + str;
            base.OnPreInit(e);
        }

        public CommerceContext CurrentCommerceContext
        {
            get { return CommerceContext.Current; }
        }

        public ProductCategory CurrentProductCategory
        {
            get { return (ProductCategory)this.ViewState["CurrentProductCategory"]; }
            set { this.ViewState["CurrentProductCategory"] = value; }
        }

        public SueetieProduct CurrentSueetieProduct
        {
            get { return (SueetieProduct)this.ViewState["CurrentSueetieProduct"]; }
            set { this.ViewState["CurrentSueetieProduct"] = value; }
        }

        public string SueetieMasterPage { get; set; }
    }
}

