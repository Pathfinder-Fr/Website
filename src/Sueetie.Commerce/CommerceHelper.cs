namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Net.Mail;
    using System.Text;
    using System.Web;

    public static class CommerceHelper
    {
        public static string FreeIt(decimal price)
        {
            if (price == 0M)
            {
                return SueetieLocalizer.GetMarketplaceString("list_free");
            }
            return price.ToString("C");
        }

        public static bool IsAdminPage()
        {
            return (HttpContext.Current.Request.RawUrl.ToLower().IndexOf("admin/marketplace/") > 0);
        }

        public static string ProductStatusToString(ProductStatusType productStatusType)
        {
            ProductStatusType type = productStatusType;
            switch (type)
            {
                case ProductStatusType.PendingApproval:
                    return SueetieLocalizer.GetMarketplaceString("statustype_pending");

                case ProductStatusType.Expired:
                    return SueetieLocalizer.GetMarketplaceString("statustype_expired");

                case ProductStatusType.SoldOut:
                    return SueetieLocalizer.GetMarketplaceString("statustype_soldout");
            }
            if (type != ProductStatusType.Active)
            {
                return SueetieLocalizer.GetMarketplaceString("statustype_other");
            }
            return SueetieLocalizer.GetMarketplaceString("statustype_active");
        }

        public static string ProductStatusToString(object productStatus)
        {
            return ProductStatusToString((ProductStatusType) ((int) productStatus));
        }

        public static string PurchasedProductTitle(ProductPurchase productPurchase)
        {
            string title = productPurchase.Title;
            if (productPurchase.IsLicensePurchase())
            {
                title = productPurchase.PackageTypeDescription + " " + productPurchase.LicenseTypeDescription + SueetieLocalizer.GetMarketplaceString("productpurchase_title_suffix");
            }
            return title;
        }

        public static string PurchaseTypeToString(PurchaseType productPurchaseType)
        {
            switch (productPurchaseType)
            {
                case PurchaseType.Commercial:
                    return SueetieLocalizer.GetMarketplaceString("purchasetype_commercial");

                case PurchaseType.FreeRegistered:
                    return SueetieLocalizer.GetMarketplaceString("purchasetype_freeregistered");

                case PurchaseType.FreeSubscribers:
                    return SueetieLocalizer.GetMarketplaceString("purchasetype_freesubscribers");

                case PurchaseType.FreeAll:
                    return SueetieLocalizer.GetMarketplaceString("purchasetype_freeall");

                case PurchaseType.Contribution:
                    return SueetieLocalizer.GetMarketplaceString("purchasetype_freecontribution");
            }
            return SueetieLocalizer.GetMarketplaceString("purchasetype_other");
        }

        public static string PurchaseTypeToString(object purchaseType)
        {
            return PurchaseTypeToString((PurchaseType) ((int) purchaseType));
        }

        public static bool SendAdResponse(int productID, string senderName, string senderAddress, string comments)
        {
            bool flag = false;
            SueetieProduct sueetieProduct = Products.GetSueetieProduct(productID);
            if (sueetieProduct == null)
            {
                return flag;
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("A user has sent a response to your Ad '{0}' at {1}", sueetieProduct.Title, SiteSettings.Instance.SiteName);
            builder.AppendLine();
            builder.AppendLine();
            builder.AppendLine("The contact information of the user is below:");
            builder.AppendLine();
            builder.AppendLine("Name: " + senderName);
            builder.AppendLine("Email: " + senderAddress);
            builder.AppendLine();
            builder.AppendLine("User comments/questions:");
            builder.AppendLine(comments);
            builder.AppendLine();
            builder.AppendLine("You can respond to the user by using the Reply feature of your email client.");
            builder.AppendLine();
            builder.AppendLine(SiteSettings.Instance.SiteName);
            builder.AppendLine(string.Format("http://{0}", HttpContext.Current.Request.Url.Host));
            string from = string.Format("{0} <{1}>", senderName, senderAddress);
            try
            {
                EmailHelper.AsyncSendEmail(new MailMessage(from, sueetieProduct.Email) { Subject = "Response for Ad: " + sueetieProduct.Title, Body = builder.ToString() });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool SendTellFriendEmail(int adId, string senderName, string senderAddress, string recipientEmail, string subject, string message)
        {
            string from = string.Format("{0} <{1}>", senderName, senderAddress);
            try
            {
                EmailHelper.AsyncSendEmail(new MailMessage(from, recipientEmail) { Subject = subject, Body = message });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

