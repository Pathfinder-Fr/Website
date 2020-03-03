namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Web;

    public static class CommerceCommon
    {
        private static string siteRootPath = HttpContext.Current.Server.MapPath("/");

        public static string ActionTypeItemListCacheKey()
        {
            return string.Format("ActionTypeItemList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static string CartLinkListCacheKey()
        {
            return string.Format("CartLinkList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearCartLinkListCache()
        {
            SueetieCache.Current.Remove(CartLinkListCacheKey());
        }

        public static void ClearCommerceSettingsCache()
        {
            SueetieCache.Current.Remove("CommerceSettings-" + SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void ClearMarketplaceCache()
        {
            ClearProductPackageListCache();
            CategoryCache.ClearCategoryCache();
            Products.ClearProductCache();
            ProductPhotos.ClearProductPhotoListCache();
            ClearCartLinkListCache();
        }

        public static void ClearProductPackageListCache()
        {
            SueetieCache.Current.Remove(ProductPackageListCacheKey());
        }

        public static void DownloadProductFile(int _productID, string _purchaseKey, int _cartLinkID)
        {
            int num2;
            HttpContext current = HttpContext.Current;
            Stream stream = null;
            SueetieProduct sueetieProduct = Products.GetSueetieProduct(_productID);
            ProductPurchase productPurchase = new ProductPurchase {
                ProductID = _productID,
                UserID = SueetieContext.Current.User.UserID,
                PurchaseKey = _purchaseKey,
                CartLinkID = _cartLinkID,
                ActionID = 1
            };
            Purchases.RecordPurchase(productPurchase);
            string marketplaceFolderName = SueetieConfiguration.Get().Core.MarketplaceFolderName;
            string str = sueetieProduct.DateCreated.Year.ToString();
            string str2 = Guid.NewGuid() + ".zip";
            string sourceFileName = siteRootPath + @"util\marketplace\downloads\" + str + @"\" + sueetieProduct.DownloadURL;
            string destFileName = siteRootPath + @"util\marketplace\downloads\tmp\" + str2;
            File.Copy(sourceFileName, destFileName);
            int num = 0x8000;
            byte[] buffer = new byte[num];
            stream = File.OpenRead(destFileName);
            current.Response.AddHeader("Content-Disposition", "attachment; filename=" + sueetieProduct.DownloadURL);
            current.Response.Clear();
            current.Response.ContentType = "application/zip";
            current.Response.Buffer = false;
            stream.Position = 0L;
            while ((num2 = stream.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (current.Response.IsClientConnected)
                {
                    current.Response.OutputStream.Write(buffer, 0, num2);
                    current.Response.Flush();
                }
                else
                {
                    return;
                }
            }
            if (stream != null)
            {
                stream.Close();
            }
            current.Response.End();
        }

        public static string GeneratePurchaseKey()
        {
            return GeneratePurchaseKey(30);
        }

        public static string GeneratePurchaseKey(int length)
        {
            RandomNumberGenerator generator = RandomNumberGenerator.Create();
            char[] chArray = new char[length];
            string str = "ABCEDFGHJKLMNPQRSTUVWXYZ23456789";
            for (int i = 0; i < length; i++)
            {
                byte[] data = new byte[1];
                generator.GetBytes(data);
                Random random = new Random(data[0]);
                if ((i % 6) == 0)
                {
                    chArray[i] = '-';
                }
                else
                {
                    chArray[i] = str[random.Next(0, 0x20)];
                }
            }
            return new string(chArray).Substring(1);
        }

        public static List<ActionTypeItem> GetActionTypeItemList()
        {
            string key = ActionTypeItemListCacheKey();
            List<ActionTypeItem> actionTypeItemList = SueetieCache.Current[key] as List<ActionTypeItem>;
            if (actionTypeItemList == null)
            {
                actionTypeItemList = CommerceDataProvider.LoadProvider().GetActionTypeItemList();
                SueetieCache.Current.Insert(key, actionTypeItemList);
            }
            return actionTypeItemList;
        }

        public static CartLink GetCartLink(int cartLinkID)
        {
            return GetCartLinkList().Find(c => c.CartLinkID == cartLinkID);
        }

        public static CartLink GetCartLink(int packageTypeID, int licenseTypeID)
        {
            return GetCartLinkList().Find(c => (c.PackageTypeID == packageTypeID) && (c.LicenseTypeID == licenseTypeID));
        }

        public static List<CartLink> GetCartLinkList()
        {
            string key = CartLinkListCacheKey();
            List<CartLink> cartLinkList = SueetieCache.Current[key] as List<CartLink>;
            if (cartLinkList == null)
            {
                cartLinkList = CommerceDataProvider.LoadProvider().GetCartLinkList();
                SueetieCache.Current.Insert(key, cartLinkList);
            }
            return cartLinkList;
        }

        public static List<CartLink> GetCartLinkList(int productID)
        {
            return (from c in GetCartLinkList()
                where c.ProductID == productID
                orderby c.Price
                select c).ToList<CartLink>();
        }

        public static StringDictionary GetCommerceSettingsDictionary()
        {
            return CommerceDataProvider.LoadProvider().GetCommerceSettingsDictionary();
        }

        public static ProductPackage GetProductPackage(int _productID)
        {
            return GetProductPackageList().Find(p => p.ProductID == _productID);
        }

        public static List<ProductPackage> GetProductPackageList()
        {
            string key = ProductPackageListCacheKey();
            List<ProductPackage> productPackageList = SueetieCache.Current[key] as List<ProductPackage>;
            if (productPackageList == null)
            {
                productPackageList = CommerceDataProvider.LoadProvider().GetProductPackageList();
                SueetieCache.Current.Insert(key, productPackageList);
            }
            return productPackageList;
        }

        public static List<ProductTypeItem> GetProductTypeItemList()
        {
            string key = ProductTypeItemListCacheKey();
            List<ProductTypeItem> productTypeItemList = SueetieCache.Current[key] as List<ProductTypeItem>;
            if (productTypeItemList == null)
            {
                productTypeItemList = CommerceDataProvider.LoadProvider().GetProductTypeItemList();
                SueetieCache.Current.Insert(key, productTypeItemList);
            }
            return productTypeItemList;
        }

        public static List<PurchaseTypeItem> GetPurchaseTypeItemList()
        {
            string key = PurchaseTypeItemListCacheKey();
            List<PurchaseTypeItem> purchaseTypeItemList = SueetieCache.Current[key] as List<PurchaseTypeItem>;
            if (purchaseTypeItemList == null)
            {
                purchaseTypeItemList = CommerceDataProvider.LoadProvider().GetPurchaseTypeItemList();
                SueetieCache.Current.Insert(key, purchaseTypeItemList);
            }
            return purchaseTypeItemList;
        }

        public static bool HasMarketplaceCategories()
        {
            return CommerceDataProvider.LoadProvider().HasMarketplaceCategories();
        }

        public static bool IsSueetiePackageDistributor()
        {
            bool flag = false;
            if (CommerceConfiguration.Get().Core.DistributionKey == "89462D24-BFF4-494C-BB19-4C942DFC24B4")
            {
                flag = true;
            }
            return flag;
        }

        public static void LogMarketplaceException(string desc, Exception ex)
        {
            SueetieLogs.LogSiteEntry(SiteLogType.Exception, SiteLogCategoryType.MarketplaceException, desc + ex.Message + "\n\nSTACK TRACE: " + ex.StackTrace, SueetieApplications.Current.ApplicationID);
        }

        public static string ProductPackageListCacheKey()
        {
            return string.Format("ProductPackageList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static string ProductTypeItemListCacheKey()
        {
            return string.Format("ProductTypeItemList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static string PurchaseTypeItemListCacheKey()
        {
            return string.Format("PurchaseTypeItemList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static void UpdateCommerceSetting(CommerceSetting commerceSetting)
        {
            CommerceDataProvider.LoadProvider().UpdateCommerceSetting(commerceSetting);
        }
    }
}

