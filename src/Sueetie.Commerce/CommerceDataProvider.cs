namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration.Provider;
    using System.Data;
    using System.Web.Caching;

    public abstract class CommerceDataProvider : ProviderBase
    {
        private static object _lock = new object();
        private static CommerceDataProvider _provider;
        private static string providerKey = "MarketplaceSqlDataProvider";

        protected CommerceDataProvider()
        {
        }

        public abstract int AddPhoto(ProductPhoto productPhoto);
        public abstract int AddProductCategory(ProductCategory productCategory);
        public abstract void CreateProductLicense(ProductLicense productLicense);
        public abstract int CreateSueetieProduct(SueetieProduct sueetieProduct);
        public abstract void DeletePhoto(int photoID);
        public abstract List<ActionTypeItem> GetActionTypeItemList();
        public abstract List<CartLink> GetCartLinkList();
        public abstract List<ProductCategory> GetCategoriesByParentID(int parentcategoryID);
        public abstract StringDictionary GetCommerceSettingsDictionary();
        public abstract List<ProductLicense> GetLicensesByTransaction(string transactionXID);
        public abstract List<ParentCategory> GetParentCategoryList(int categoryID);
        public abstract PaymentService GetPaymentService(int _paymentServiceID, bool _getPrimaryService);
        public abstract List<PaymentService> GetPaymentServiceList();
        public abstract List<ProductCategory> GetProductCategoryList();
        public abstract List<ProductLicense> GetProductLicenseList();
        public abstract List<ProductPackage> GetProductPackageList();
        public abstract List<ProductPhoto> GetProductPhotoList();
        public abstract ProductPurchase GetProductPurchase(int purchaseID);
        public abstract List<ProductPurchase> GetProductPurchaseList();
        public abstract List<ProductTypeItem> GetProductTypeItemList();
        public abstract List<ProductPurchase> GetPurchasesByTransaction(string transactionXID);
        public abstract List<PurchaseTypeItem> GetPurchaseTypeItemList();
        public abstract SueetieProduct GetSueetieProduct(int productID);
        public abstract List<SueetieProduct> GetSueetieProductList();
        public abstract List<SueetieProduct> GetSueetieProductsByCategoryTree(int categoryID);
        public abstract List<ProductPurchase> GetUserPurchases(int userID);
        public abstract bool HasMarketplaceCategories();
        public static CommerceDataProvider LoadProvider()
        {
            _provider = SueetieCache.Current[providerKey] as CommerceDataProvider;
            if (_provider == null)
            {
                lock (_lock)
                {
                    if (_provider == null)
                    {
                        SueetieConfiguration configuration = SueetieConfiguration.Get();
                        SueetieProvider provider = configuration.SueetieProviders.Find(sp => sp.Name == "MarketplaceSqlDataProvider");
                        _provider = Activator.CreateInstance(Type.GetType(provider.ProviderType), new object[] { provider.ConnectionString }) as CommerceDataProvider;
                        SueetieCache.Current.InsertMax(providerKey, _provider, new CacheDependency(configuration.ConfigPath));
                    }
                }
            }
            return _provider;
        }

        public abstract void MoveProductCategory(ProductCategory productCategory);
        public abstract void MoveProductsToCategory(int currentCategoryID, int newCategoryID);
        public static void PopulateActionTypeItemList(IDataReader dr, ActionTypeItem _actionTypeItem)
        {
            _actionTypeItem.ActionID = (int) dr["actionid"];
            _actionTypeItem.ActionCode = dr["actioncode"] as string;
            _actionTypeItem.ActionDescription = dr["actiondescription"] as string;
            _actionTypeItem.IsDisplayed = (bool) dr["isdisplayed"];
        }

        public static void PopulateCartLinkList(IDataReader dr, CartLink _cartLink)
        {
            _cartLink.CartLinkID = (int) dr["cartlinkid"];
            _cartLink.ProductID = (int) dr["productid"];
            _cartLink.PackageTypeID = (int) dr["packagetypeid"];
            _cartLink.LicenseTypeID = (int) dr["licensetypeid"];
            _cartLink.Price = (decimal) dr["price"];
            _cartLink.PackageTypeCode = dr["packagetypecode"] as string;
            _cartLink.LicenseTypeDescription = dr["licensetypedescription"] as string;
            _cartLink.IsDisplayed = (bool) dr["isdisplayed"];
            _cartLink.PackageTypeDescription = dr["packagetypedescription"] as string;
            _cartLink.Title = dr["title"] as string;
            _cartLink.ProductTypeID = (int) dr["producttypeid"];
            _cartLink.ProductTypeCode = dr["producttypecode"] as string;
            _cartLink.Version = (decimal) dr["version"];
            _cartLink.IsActive = (bool) dr["isactive"];
        }

        public static void PopulateParentCategoryList(IDataReader dr, ParentCategory _parentCategory)
        {
            _parentCategory.CategoryID = (int) dr["categoryid"];
            _parentCategory.ParentCategoryID = (int) dr["parentcategoryid"];
            _parentCategory.ParentCategoryName = dr["categoryname"] as string;
            _parentCategory.NumActiveProducts = (int) dr["NumActiveProducts"];
        }

        public static void PopulatePaymentServiceList(IDataReader dr, PaymentService _paymentService, bool _addDetails)
        {
            _paymentService.PaymentServiceID = (int) dr["paymentserviceid"];
            _paymentService.PaymentServiceName = dr["paymentservicename"] as string;
            _paymentService.PaymentServiceDescription = dr["paymentservicedescription"] as string;
            _paymentService.SharedPaymentServicePage = dr["sharedpaymentservicepage"] as string;
            _paymentService.IsPrimary = (bool) dr["isprimary"];
            if (_addDetails)
            {
                _paymentService.AccountName = dr["accountname"] as string;
                _paymentService.CartUrl = dr["carturl"] as string;
                _paymentService.TransactionUrl = dr["transactionurl"] as string;
                _paymentService.PurchaseUrl = dr["purchaseurl"] as string;
                _paymentService.ReturnUrl = dr["returnurl"] as string;
                _paymentService.ShoppingUrl = dr["shoppingurl"] as string;
                _paymentService.IdentityToken = dr["identitytoken"] as string;
            }
        }

        public static void PopulateProductCategoryList(IDataReader dr, ProductCategory _productCategory)
        {
            _productCategory.CategoryID = (int) dr["categoryid"];
            _productCategory.ParentCategoryID = (int) dr["parentcategoryid"];
            _productCategory.Path = dr["path"] as string;
            _productCategory.CategoryHtmlName = dr["categoryname"] as string;
            _productCategory.NumActiveProducts = (int) dr["NumActiveProducts"];
            _productCategory.CategoryDescription = dr["categorydescription"] as string;
        }

        public static void PopulateProductLicenseList(IDataReader dr, ProductLicense _productLicense)
        {
            _productLicense.LicenseID = (int) dr["licenseid"];
            _productLicense.LicenseTypeID = (int) dr["licensetypeid"];
            _productLicense.LicenseTypeDescription = dr["licensetypedescription"] as string;
            _productLicense.UserID = (int) dr["userid"];
            _productLicense.DatePurchased = (DateTime) dr["datepurchased"];
            _productLicense.PurchasedVersion = (decimal) dr["purchasedversion"];
            _productLicense.UserName = dr["username"] as string;
            _productLicense.DisplayName = dr["displayname"] as string;
            _productLicense.Email = dr["email"] as string;
            _productLicense.MembershipID = DataHelper.GetGuid(dr, "membershipid");
            _productLicense.ForumUserID = (int) dr["forumuserid"];
            _productLicense.BoardID = (int) dr["boardid"];
            _productLicense.License = dr["license"] as string;
            _productLicense.PackageTypeCode = dr["packagetypecode"] as string;
            _productLicense.PackageTypeDescription = dr["packagetypedescription"] as string;
            _productLicense.CartLinkID = (int) dr["cartlinkid"];
            _productLicense.PackageTypeID = (int) dr["packagetypeid"];
            _productLicense.Version = (decimal) dr["version"];
            _productLicense.ProductID = (int) dr["productid"];
            _productLicense.PurchaseID = (int) dr["purchaseid"];
            _productLicense.PurchaseKey = dr["purchasekey"] as string;
            _productLicense.TransactionXID = dr["transactionxid"] as string;
            _productLicense.Price = (decimal) dr["price"];
            _productLicense.MajorVersion = dr.GetMajorVersion((decimal) dr["version"]);
            _productLicense.CustomLicenseTypeDescription = dr.GetCustomLicenseTypeDescription(_productLicense.LicenseTypeDescription);
        }

        public static void PopulateProductPackageList(IDataReader dr, ProductPackage _productPackage)
        {
            _productPackage.ProductPackageID = (int) dr["productpackageid"];
            _productPackage.ProductID = (int) dr["productid"];
            _productPackage.PackageTypeID = (int) dr["packagetypeid"];
            _productPackage.Version = (decimal) dr["version"];
            _productPackage.MajorVersion = dr.GetMajorVersion((decimal) dr["version"]);
        }

        public static void PopulateProductPhotoList(IDataReader dr, ProductPhoto _productPhoto)
        {
            _productPhoto.PhotoID = (int) dr["photoid"];
            _productPhoto.ProductID = (int) dr["productid"];
            _productPhoto.IsMainPreview = (bool) dr["ismainpreview"];
            _productPhoto.DateCreated = DataHelper.DateOrNull(dr["datecreated"].ToString());
        }

        public static void PopulateProductPurchaseList(IDataReader dr, ProductPurchase _productPurchase)
        {
            _productPurchase.PurchaseID = (int) dr["purchaseid"];
            _productPurchase.ProductID = (int) dr["productid"];
            _productPurchase.UserID = (int) dr["userid"];
            _productPurchase.PurchaseKey = dr["purchasekey"] as string;
            _productPurchase.PurchaseDateTime = DataHelper.DateOrNull(dr["purchasedatetime"].ToString());
            _productPurchase.CartLinkID = (int) dr["cartlinkid"];
            _productPurchase.ProductTypeID = (int) dr["producttypeid"];
            _productPurchase.ProductTypeCode = dr["producttypecode"] as string;
            _productPurchase.Price = (decimal) dr["price"];
            _productPurchase.Title = dr["title"] as string;
            _productPurchase.CategoryID = (int) dr["categoryid"];
            _productPurchase.LicenseTypeID = (int) dr["licensetypeid"];
            _productPurchase.PackageTypeID = (int) dr["packagetypeid"];
            _productPurchase.LicenseTypeDescription = dr["licensetypedescription"] as string;
            _productPurchase.PackageTypeDescription = dr["packagetypedescription"] as string;
            _productPurchase.CategoryName = dr["categoryname"] as string;
            _productPurchase.TransactionXID = dr["transactionxid"] as string;
            _productPurchase.UserName = dr["username"] as string;
            _productPurchase.Email = dr["email"] as string;
            _productPurchase.DisplayName = dr["displayname"] as string;
            _productPurchase.ActionID = (int) dr["actionid"];
            _productPurchase.ActionCode = dr["actioncode"] as string;
            _productPurchase.PurchaseTypeID = (int) dr["purchasetypeid"];
        }

        public static void PopulateProductTypeItemList(IDataReader dr, ProductTypeItem _productTypeItem)
        {
            _productTypeItem.ProductTypeID = (int) dr["producttypeid"];
            _productTypeItem.ProductTypeCode = dr["producttypecode"] as string;
            _productTypeItem.ProductTypeDescription = dr["producttypedescription"] as string;
            _productTypeItem.IsDisplayed = (bool) dr["isdisplayed"];
        }

        public static void PopulatePurchaseTypeItemList(IDataReader dr, PurchaseTypeItem _purchaseTypeItem)
        {
            _purchaseTypeItem.PurchaseTypeID = (int) dr["purchasetypeid"];
            _purchaseTypeItem.PurchaseTypeCode = dr["purchasetypecode"] as string;
            _purchaseTypeItem.PurchaseTypeDescription = dr["purchasetypedescription"] as string;
            _purchaseTypeItem.IsDisplayed = (bool) dr["isdisplayed"];
        }

        public static void PopulateSueetieProductList(IDataReader dr, SueetieProduct _sueetieProduct)
        {
            _sueetieProduct.ProductID = (int) dr["productid"];
            _sueetieProduct.UserID = (int) dr["userid"];
            _sueetieProduct.CategoryID = (int) dr["categoryid"];
            _sueetieProduct.Title = dr["title"] as string;
            _sueetieProduct.SubTitle = dr["subtitle"] as string;
            _sueetieProduct.ProductDescription = dr["productdescription"] as string;
            _sueetieProduct.DownloadURL = dr["downloadurl"] as string;
            _sueetieProduct.Price = (decimal) dr["price"];
            _sueetieProduct.ExpirationDate = DataHelper.DateOrNull(dr["expirationdate"].ToString());
            _sueetieProduct.DateCreated = DataHelper.DateOrNull(dr["datecreated"].ToString());
            _sueetieProduct.DateApproved = DataHelper.DateOrNull(dr["dateapproved"].ToString());
            _sueetieProduct.NumViews = (int) dr["numviews"];
            _sueetieProduct.NumDownloads = (int) dr["numdownloads"];
            _sueetieProduct.PurchaseTypeID = (int) dr["purchasetypeid"];
            _sueetieProduct.PreviewImageID = (int) dr["previewimageid"];
            _sueetieProduct.DocumentationURL = dr["documentationurl"] as string;
            _sueetieProduct.ImageGalleryURL = dr["imagegalleryurl"] as string;
            _sueetieProduct.StatusTypeID = (int) dr["statustypeid"];
            _sueetieProduct.UserName = dr["username"] as string;
            _sueetieProduct.Email = dr["email"] as string;
            _sueetieProduct.DisplayName = dr["displayname"] as string;
            _sueetieProduct.ContentID = (int) dr["contentid"];
            _sueetieProduct.ContentTypeID = (int) dr["contenttypeid"];
            _sueetieProduct.ApplicationID = (int) dr["applicationid"];
            _sueetieProduct.StatusTypeCode = dr["statustypecode"] as string;
            _sueetieProduct.CategoryName = dr["categoryname"] as string;
            _sueetieProduct.CategoryPath = dr["categorypath"] as string;
            _sueetieProduct.PurchaseTypeCode = dr["purchasetypecode"] as string;
            _sueetieProduct.ProductTypeID = (int) dr["producttypeid"];
            _sueetieProduct.ProductTypeCode = dr["producttypecode"] as string;
            _sueetieProduct.ProductTypeDescription = dr["producttypedescription"] as string;
        }

        public abstract int RecordPurchase(ProductPurchase productPurchase);
        public abstract int RemoveCategory(int categoryid);
        public abstract void SetPreviewPhoto(ProductPhoto productPhoto);
        public abstract void SetPrimaryPaymentService(int paymentServiceID);
        public abstract void UpdateCommerceSetting(CommerceSetting siteSetting);
        public abstract void UpdatePaymentServiceSetting(PaymentServiceSetting paymentServiceSetting);
        public abstract void UpdateProductCategory(ProductCategory productCategory);
        public abstract void UpdateProductCategory(int _productID, int _categoryID);
        public abstract void UpdateProductViewCount(int _productID);
        public abstract void UpdateSueetieProduct(SueetieProduct sueetieProduct);

        public static CommerceDataProvider Provider
        {
            get
            {
                LoadProvider();
                return _provider;
            }
        }
    }
}

