namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Collections.Generic;
    using System.Web;

    public static class Payments
    {
        public static void ClearPaymentServiceCache()
        {
            SueetieCache.Current.Remove(PaymentServiceCacheKey());
        }

        public static PaymentService GetPaymentService(int paymentServiceID)
        {
            return CommerceDataProvider.LoadProvider().GetPaymentService(paymentServiceID, false);
        }

        public static List<PaymentService> GetPaymentServiceList()
        {
            return CommerceDataProvider.LoadProvider().GetPaymentServiceList();
        }

        public static PaymentService GetPrimaryPaymentService()
        {
            string key = PaymentServiceCacheKey();
            PaymentService paymentService = SueetieCache.Current[key] as PaymentService;
            if (paymentService == null)
            {
                paymentService = CommerceDataProvider.LoadProvider().GetPaymentService(-1, true);
                SueetieCache.Current.Insert(key, paymentService);
            }
            return paymentService;
        }

        public static string PaymentServiceCacheKey()
        {
            return string.Format("PrimaryPaymentService-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void SetPrimaryPaymentService(int paymentServiceID)
        {
            CommerceDataProvider.LoadProvider().SetPrimaryPaymentService(paymentServiceID);
            ClearPaymentServiceCache();
        }

        public static string ShoppingCartLink()
        {
            string str = string.Empty;
            if (!SueetieContext.Current.IsAnonymousUser)
            {
                PaymentService primaryPaymentService = CommerceContext.Current.PrimaryPaymentService;
                return string.Format(primaryPaymentService.CartUrl, primaryPaymentService.AccountName, primaryPaymentService.ReturnUrl, primaryPaymentService.ShoppingUrl);
            }
            str = "/members/login.aspx";
            if (CommerceContext.Current.CurrentSueetieProduct != null)
            {
                string str2 = str;
                return (str2 + "?ReturnUrl=/" + SueetieApplications.Get().Marketplace.ApplicationKey + "/ShowProduct.aspx" + HttpContext.Current.Request.Url.Query);
            }
            return (str + "?ReturnUrl=/" + SueetieApplications.Get().Marketplace.ApplicationKey + "/default.aspx");
        }

        public static void UpdatePaymentServiceSetting(PaymentServiceSetting paymentServiceSetting)
        {
            CommerceDataProvider.LoadProvider().UpdatePaymentServiceSetting(paymentServiceSetting);
        }
    }
}

