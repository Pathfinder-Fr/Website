namespace Sueetie.Commerce
{
    using Sueetie.Core;

    public class MarketplaceBaseControl : SueetieBaseControl
    {
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
    }
}

