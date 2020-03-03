namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Collections.Generic;
    using System.Web.Caching;

    public sealed class CategoryCache
    {
        internal const string AllCategoriesKey = "CategoryCache_AllCategoriesKey";
        private static readonly string[] AllCategoriesKeyArrayWrap = new string[] { "CategoryCache_AllCategoriesKey" };

        private CategoryCache()
        {
        }

        public static void ClearCategoryCache()
        {
            SueetieCache.Current.Remove("CategoryCache_AllCategoriesKey");
            ProductCategories.ClearProductCategoryListCache();
        }

        private static List<ProductCategory> FetchAllCategories()
        {
            List<ProductCategory> list = new List<ProductCategory>();
            FetchCategoriesRecursively(true, list, 0, string.Empty);
            return list;
        }

        private static List<ProductCategory> FetchCategoriesByParentId(int parentCategoryId)
        {
            List<ProductCategory> list = new List<ProductCategory>();
            FetchCategoriesRecursively(false, list, parentCategoryId, string.Empty);
            return list;
        }

        private static void FetchCategoriesRecursively(bool recursing, List<ProductCategory> list, int categoryId, string levelPrefix)
        {
            List<ProductCategory> categoriesByParentID = ProductCategories.GetCategoriesByParentID(categoryId);
            if (categoriesByParentID != null)
            {
                categoryId.ToString();
                foreach (ProductCategory category in categoriesByParentID)
                {
                    list.Add(new ProductCategory
                    {
                        CategoryHtmlName = category.CategoryName,
                        CategoryID = category.CategoryID,
                        NumActiveProducts = category.NumActiveProducts,
                        NameWithActiveCount = string.Format("{0} ({1})", category.CategoryName, category.NumActiveProducts),
                        LevelIndentedName = levelPrefix + " " + category.CategoryName
                    });

                    if (recursing)
                    {
                        FetchCategoriesRecursively(recursing, list, category.CategoryID, levelPrefix + "--");
                    }
                }
            }
        }

        private static List<ProductCategory> FetchParentCategoriesById(int categoryID)
        {
            List<ProductCategory> list = new List<ProductCategory>();
            foreach (ParentCategory category in ProductCategories.GetParentCategoriesByID(categoryID))
            {
                int num = 0;
                ProductCategory item = new ProductCategory
                {
                    CategoryID = category.CategoryID,
                    ParentCategoryID = num,
                    CategoryHtmlName = category.ParentCategoryName,
                    NumActiveProducts = category.NumActiveProducts,
                    NameWithActiveCount = string.Format("{0} ({1})", category.ParentCategoryName, category.NumActiveProducts),
                    LevelIndentedName = string.Empty
                };
                list.Add(item);
            }
            return list;
        }

        public static List<ProductCategory> GetAllCategories()
        {
            List<ProductCategory> list = SueetieCache.Current["CategoryCache_AllCategoriesKey"] as List<ProductCategory>;
            if (list == null)
            {
                list = FetchAllCategories();
                SueetieCache.Current.InsertMax("CategoryCache_AllCategoriesKey", list);
            }
            return list;
        }

        public static List<ProductCategory> GetBrowseCategoriesByParentId(int parentCategoryId)
        {
            return GetCategoriesByParentId(parentCategoryId, true);
        }

        public static List<ProductCategory> GetCategoriesByParentId(int parentCategoryId)
        {
            return GetCategoriesByParentId(parentCategoryId, false);
        }

        public static List<ProductCategory> GetCategoriesByParentId(int parentCategoryId, bool updateFrequently)
        {
            string key = (updateFrequently ? "BR" : "SC") + parentCategoryId.ToString();
            DateTime absoluteExpiration = updateFrequently ? DateTime.Now.AddSeconds(1.0) : DateTime.Now.AddDays(1.0);
            List<ProductCategory> list = SueetieCache.Current[key] as List<ProductCategory>;
            if (list == null)
            {
                list = FetchCategoriesByParentId(parentCategoryId);
                CacheDependency dependencies = new CacheDependency(null, AllCategoriesKeyArrayWrap);
                SueetieCache.Current.Add(key, list, dependencies, absoluteExpiration, TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            return list;
        }

        public static List<ProductCategory> GetParentCategoriesById(int categoryId)
        {
            string key = "PC" + categoryId.ToString();
            List<ProductCategory> list = SueetieCache.Current[key] as List<ProductCategory>;
            if (list == null)
            {
                list = FetchParentCategoriesById(categoryId);
                CacheDependency dependencies = new CacheDependency(null, AllCategoriesKeyArrayWrap);
                SueetieCache.Current.InsertMax(key, list, dependencies);
            }
            return list;
        }
    }
}

