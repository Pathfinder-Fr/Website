namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class Products
    {
        public static void ClearProductCache()
        {
            ClearSueetieProductListCache();
        }

        public static void ClearSueetieProductListCache()
        {
            SueetieCache.Current.Remove(SueetieProductListCacheKey());
            ClearSueetieProductListTreeCache();
        }

        private static void ClearSueetieProductListTreeCache()
        {
            foreach (ProductCategory category in ProductCategories.GetProductCategoryList())
            {
                SueetieCache.Current.Remove(string.Format("SueetieProductList-{0}", category.CategoryID));
            }
        }

        public static int CreateSueetieProduct(SueetieProduct sueetieProduct)
        {
            int num = CommerceDataProvider.LoadProvider().CreateSueetieProduct(sueetieProduct);
            CommerceCommon.ClearMarketplaceCache();
            return num;
        }

        public static List<SueetieProduct> GetActiveSueetieProducts()
        {
            return (from p in GetSueetieProductList()
                where p.StatusTypeID == 100
                select p).ToList<SueetieProduct>().LicensedProductList();
        }

        public static SueetieProduct GetSueetieProduct(int productID)
        {
            return CommerceDataProvider.LoadProvider().GetSueetieProduct(productID);
        }

        public static List<SueetieProduct> GetSueetieProductList()
        {
            string key = SueetieProductListCacheKey();
            List<SueetieProduct> sueetieProductList = SueetieCache.Current[key] as List<SueetieProduct>;
            if (sueetieProductList == null)
            {
                sueetieProductList = CommerceDataProvider.LoadProvider().GetSueetieProductList();
                SueetieCache.Current.Insert(key, sueetieProductList);
            }
            return sueetieProductList;
        }

        public static List<SueetieProduct> GetSueetieProductList(bool getCached)
        {
            if (getCached)
            {
                return GetSueetieProductList();
            }
            return CommerceDataProvider.LoadProvider().GetSueetieProductList();
        }

        public static List<SueetieProduct> GetSueetieProductsByCategory(int categoryID)
        {
            return GetSueetieProductsByCategory(categoryID, false).LicensedProductListByCategory();
        }

        public static List<SueetieProduct> GetSueetieProductsByCategory(int categoryID, bool getSubCategories)
        {
            Func<SueetieProduct, bool> predicate = null;
            List<SueetieProduct> sueetieProductList = GetSueetieProductList();
            if (!getSubCategories)
            {
                if (predicate == null)
                {
                    predicate = p => p.CategoryID == categoryID;
                }
                sueetieProductList = sueetieProductList.Where<SueetieProduct>(predicate).ToList<SueetieProduct>();
            }
            else
            {
                sueetieProductList = (from p in GetSueetieProductsByCategoryTree(categoryID)
                    where p.StatusTypeID == 100
                    select p).ToList<SueetieProduct>();
            }
            return sueetieProductList.LicensedProductListByCategory();
        }

        public static List<SueetieProduct> GetSueetieProductsByCategoryTree(int categoryID)
        {
            string key = SueetieProductListTreeCacheKey(categoryID);
            List<SueetieProduct> sueetieProductsByCategoryTree = SueetieCache.Current[key] as List<SueetieProduct>;
            if (sueetieProductsByCategoryTree == null)
            {
                sueetieProductsByCategoryTree = CommerceDataProvider.LoadProvider().GetSueetieProductsByCategoryTree(categoryID);
                SueetieCache.Current.Insert(key, sueetieProductsByCategoryTree);
            }
            return sueetieProductsByCategoryTree.LicensedProductListByCategory();
        }

        public static void MoveProductsToCategory(int currentCategoryID, int newCategoryID)
        {
            CommerceDataProvider.LoadProvider().MoveProductsToCategory(currentCategoryID, newCategoryID);
            CommerceCommon.ClearMarketplaceCache();
        }

        public static string SueetieProductListCacheKey()
        {
            return string.Format("SueetieAllProductList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static string SueetieProductListTreeCacheKey(int categoryID)
        {
            return string.Format("SueetieProductList-{0}", categoryID.ToString());
        }

        public static void UpdateProductCategory(int _productID, int _categoryID)
        {
            CommerceDataProvider.LoadProvider().UpdateProductCategory(_productID, _categoryID);
            CommerceCommon.ClearMarketplaceCache();
        }

        public static void UpdateProductViewCount(int _productID)
        {
            CommerceDataProvider.LoadProvider().UpdateProductViewCount(_productID);
        }

        public static void UpdateSueetieProduct(SueetieProduct sueetieProduct)
        {
            CommerceDataProvider.LoadProvider().UpdateSueetieProduct(sueetieProduct);
            CommerceCommon.ClearMarketplaceCache();
        }
    }
}

