namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Licenses
    {
        public static void ClearProductLicenseListCache()
        {
            SueetieCache.Current.Remove(ProductLicenseListCacheKey());
        }

        public static void CreateProductLicense(ProductLicense productLicense)
        {
            CommerceDataProvider.LoadProvider().CreateProductLicense(productLicense);
            ClearProductLicenseListCache();
        }

        public static List<ProductLicense> GetLicensesByTransaction(string transactionXID)
        {
            return CommerceDataProvider.LoadProvider().GetLicensesByTransaction(transactionXID);
        }

        public static List<ProductLicense> GetNonCachedProductLicenseList()
        {
            return CommerceDataProvider.LoadProvider().GetProductLicenseList();
        }

        public static ProductLicense GetProductLicense(int _licenseID)
        {
            return GetNonCachedProductLicenseList().First<ProductLicense>(p => (p.LicenseID == _licenseID));
        }

        public static ProductLicense GetProductLicense(ProductPackage _productPackage, int _userID, ProductLicenseType _productLicenseType)
        {
            return GetProductLicenseList().Find(p => (((p.PackageTypeID == _productPackage.PackageTypeID) && (p.LicenseTypeID == (int)_productLicenseType)) && (p.Version == _productPackage.Version)) && (p.UserID == _userID));
        }

        public static List<ProductLicense> GetProductLicenseList()
        {
            string key = ProductLicenseListCacheKey();
            List<ProductLicense> productLicenseList = SueetieCache.Current[key] as List<ProductLicense>;
            if (productLicenseList == null)
            {
                productLicenseList = CommerceDataProvider.LoadProvider().GetProductLicenseList();
                SueetieCache.Current.Insert(key, productLicenseList);
            }
            return productLicenseList;
        }

        public static List<ProductLicense> GetUserProductLicenseList(int _forumUserID)
        {
            return (from p in GetNonCachedProductLicenseList()
                where p.ForumUserID == _forumUserID
                select p).ToList<ProductLicense>();
        }

        public static List<ProductLicense> GetUserProductLicenseList(int _forumUserID, int _packageTypeID)
        {
            return (from p in GetUserProductLicenseList(_forumUserID)
                where p.PackageTypeID == _packageTypeID
                orderby p.DatePurchased descending
                select p).ToList<ProductLicense>();
        }

        public static string ProductLicenseListCacheKey()
        {
            return string.Format("ProductLicenseList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }
    }
}

