namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ProductCategories
    {
        public static int AddProductCategory(ProductCategory productCategory)
        {
            return CommerceDataProvider.LoadProvider().AddProductCategory(productCategory);
        }

        public static void ClearProductCategoryListCache()
        {
            SueetieCache.Current.Remove(ProductCategoryListCacheKey());
        }

        public static List<ProductCategory> GetCategoriesByParentID(int parentcategoryID)
        {
            return CommerceDataProvider.LoadProvider().GetCategoriesByParentID(parentcategoryID);
        }

        public static List<ParentCategory> GetParentCategoriesByID(int categoryID)
        {
            return CommerceDataProvider.LoadProvider().GetParentCategoryList(categoryID);
        }

        public static ProductCategory GetProductCategory(int categoryID)
        {
            return GetProductCategoryList().Find(c => c.CategoryID == categoryID);
        }

        public static List<ProductCategory> GetProductCategoryByParentID(int categoryID)
        {
            return (from c in GetProductCategoryList()
                where c.ParentCategoryID == categoryID
                select c).ToList<ProductCategory>();
        }

        public static List<ProductCategory> GetProductCategoryList()
        {
            string key = ProductCategoryListCacheKey();
            List<ProductCategory> productCategoryList = SueetieCache.Current[key] as List<ProductCategory>;
            if (productCategoryList == null)
            {
                productCategoryList = CommerceDataProvider.LoadProvider().GetProductCategoryList();
                SueetieCache.Current.Insert(key, productCategoryList);
            }
            return productCategoryList;
        }

        public static void MoveProductCategory(ProductCategory productCategory)
        {
            CommerceDataProvider.LoadProvider().MoveProductCategory(productCategory);
        }

        public static string ProductCategoryListCacheKey()
        {
            return string.Format("ProductCategoryList-{0}", SueetieConfiguration.Get().Core.SiteUniqueName);
        }

        public static bool RemoveProductCategory(int categoryID)
        {
            int num = 0;
            if (categoryID != 0)
            {
                num = CommerceDataProvider.LoadProvider().RemoveCategory(categoryID);
            }
            return (num > 0);
        }

        public static void UpdateProductCategory(ProductCategory productCategory)
        {
            CommerceDataProvider.LoadProvider().UpdateProductCategory(productCategory);
        }
    }
}

