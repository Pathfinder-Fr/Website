namespace Sueetie.Commerce.Controls
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Core;

    public class PackageCartLinks : MarketplaceBaseControl
    {
        public void Page_Load()
        {
            Predicate<CartLink> match = null;
            Literal child = new Literal();
            StringBuilder builder = new StringBuilder();
            string str = string.Empty;
            string title = string.Empty;
            PaymentService primaryPaymentService = CommerceContext.Current.PrimaryPaymentService;

            if (base.CurrentSueetieProduct.ProductTypeID == 5)
            {
                foreach (CartLink link in (from p in CommerceCommon.GetCartLinkList(base.CurrentSueetieProduct.ProductID)
                                           where p.LicenseTypeID > 11
                                           select p).ToList<CartLink>())
                {
                    title = link.PackageTypeDescription + " " + link.LicenseTypeDescription + " Product Key";
                    str = string.Format(primaryPaymentService.PurchaseUrl, new object[] { primaryPaymentService.AccountName, link.CartLinkID, title, link.Price.ToString("##0.00"), primaryPaymentService.ReturnUrl, primaryPaymentService.ShoppingUrl });
                    if (this.IsSideBarLink)
                    {
                        builder.Append("<li><a href=\"" + str + "\">" + link.LicenseTypeDescription + " Product Key</a></li>");
                    }
                    else
                    {
                        builder.Append("<li><a href=\"" + str + "\">Add " + link.LicenseTypeDescription + " Product Key To Your Shopping Cart</a></li>");
                    }
                }
            }
            else if (base.CurrentSueetieProduct.PurchaseTypeID == 1)
            {
                if (match == null)
                {
                    match = p => p.ProductID == base.CurrentSueetieProduct.ProductID;
                }
                CartLink link2 = CommerceCommon.GetCartLinkList(base.CurrentSueetieProduct.ProductID).Find(match);
                title = base.CurrentSueetieProduct.Title;
                str = string.Format(primaryPaymentService.PurchaseUrl, new object[] { primaryPaymentService.AccountName, link2.CartLinkID, title, link2.Price.ToString("##0.00"), primaryPaymentService.ReturnUrl, primaryPaymentService.ShoppingUrl });
                if (this.IsSideBarLink)
                {
                    builder.Append("<li><a href=\"" + str + "\">" + SueetieLocalizer.GetMarketplaceString("cartlink_commercial_sidebar") + "</a></li>");
                }
                else
                {
                    builder.Append("<li><a href=\"" + str + "\">" + string.Format(SueetieLocalizer.GetMarketplaceString("cartlink_commercial_bottom"), base.CurrentSueetieProduct.Title) + "</a></li>");
                }
            }
            child.Text = builder.ToString();
            if (child.Text.Trim().Length > 0)
            {
                this.Controls.Add(child);
            }
        }

        public bool IsSideBarLink { get; set; }
    }
}

