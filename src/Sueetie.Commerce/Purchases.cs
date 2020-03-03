namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Purchases
    {
        public static void ClearProductPurchaseListCache()
        {
            SueetieCache.Current.Remove(ProductPurchaseListCacheKey());
        }

        public static ProductPurchase GetProductPurchase(int purchaseID)
        {
            return CommerceDataProvider.LoadProvider().GetProductPurchase(purchaseID);
        }

        public static List<ProductPurchase> GetProductPurchaseList()
        {
            string key = ProductPurchaseListCacheKey();
            List<ProductPurchase> productPurchaseList = SueetieCache.Current[key] as List<ProductPurchase>;
            if (productPurchaseList == null)
            {
                productPurchaseList = CommerceDataProvider.LoadProvider().GetProductPurchaseList();
                SueetieCache.Current.Insert(key, productPurchaseList);
            }
            return productPurchaseList;
        }

        public static List<ProductPurchase> GetPurchasesByCategoryID(int categoryID)
        {
            return (from p in GetProductPurchaseList()
                where p.CategoryID == categoryID
                select p).ToList<ProductPurchase>();
        }

        public static List<ProductPurchase> GetPurchasesByProductID(int productID)
        {
            return (from p in GetProductPurchaseList()
                where p.ProductID == productID
                select p).ToList<ProductPurchase>();
        }

        public static List<ProductPurchase> GetPurchasesByTransaction(string transactionXID)
        {
            return CommerceDataProvider.LoadProvider().GetPurchasesByTransaction(transactionXID);
        }

        public static List<ProductPurchase> GetUserPurchases(int userID)
        {
            return CommerceDataProvider.LoadProvider().GetUserPurchases(userID);
        }

        public static string ProductPurchaseListCacheKey()
        {
            return string.Format("ProductPurchaseList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static int RecordPurchase(ProductPurchase productPurchase)
        {
            int num = -1;
            num = CommerceDataProvider.LoadProvider().RecordPurchase(productPurchase);
            ClearProductPurchaseListCache();
            return num;
        }
    }
}

