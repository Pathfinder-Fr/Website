namespace Sueetie.Commerce
{
    using Sueetie.Core;
    using Sueetie.Licensing;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public static class CommerceExtensions
    {
        public static int ActionType(this SueetieProduct product)
        {
            int num = -1;
            if ((product.ProductTypeID == 5) || (product.ProductTypeID == 2))
            {
                return 2;
            }
            if (product.ProductTypeID == 4)
            {
                return 4;
            }
            if ((product.ProductTypeID == 6) || (product.ProductTypeID == 1))
            {
                return 3;
            }
            if (product.ProductTypeID == 3)
            {
                return 5;
            }
            if (product.ProductTypeID == 7)
            {
                num = 6;
            }
            return num;
        }

        public static string AddItemNum(this string _constant, int _itemNumber)
        {
            return (_constant + _itemNumber.ToString());
        }

        private static string CleanLicenseType(string _licenseTypeString)
        {
            return _licenseTypeString.ToLower().Replace(" ", string.Empty);
        }

        public static string FreeIt(this decimal price)
        {
            if (price == 0M)
            {
                return SueetieLocalizer.GetString("list_free", "marketplace.xml");
            }
            return price.ToString("C");
        }

        public static string GetCustomLicenseTypeDescription(this IDataReader source, string _value)
        {
            return SueetieLocalizer.GetMarketplaceString("commerce_licensetype_" + CleanLicenseType(_value));
        }

        public static int GetMajorVersion(this IDataReader source, decimal _value)
        {
            return (int) Math.Floor(_value);
        }

        public static bool HasExistingLicense(this SueetieUser _user, ProductPackage _productPackage, ProductLicenseType _productLicenseType)
        {
            return Licenses.GetProductLicenseList().Any<ProductLicense>(l => ((((l.PackageTypeID == _productPackage.PackageTypeID) && (l.LicenseTypeID >= (int)_productLicenseType)) && (l.MajorVersion == _productPackage.MajorVersion)) && (l.UserID == _user.UserID)));
        }

        public static bool HasFreeLicenseOfCurrentVersion(this SueetieUser _user, ProductPackage _productPackage)
        {
            return Licenses.GetProductLicenseList().Any<ProductLicense>(l => ((((l.PackageTypeID == _productPackage.PackageTypeID) && (l.LicenseTypeID == 1)) && (l.MajorVersion == _productPackage.MajorVersion)) && (l.UserID == _user.UserID)));
        }

        public static bool HasPriorVersionLicense(this SueetieUser _user, ProductPackage _productPackage, ProductLicenseType _productLicenseType)
        {
            return Licenses.GetProductLicenseList().Any<ProductLicense>(l => ((((l.PackageTypeID == _productPackage.PackageTypeID) && (l.LicenseTypeID >= (int)_productLicenseType)) && (l.MajorVersion < _productPackage.MajorVersion)) && (l.UserID == _user.UserID)));
        }

        public static bool IsCommercialLicense(this int _licenseTypeID)
        {
            return (_licenseTypeID > 10);
        }

        public static bool IsEnterpriseLicense(this int _licenseTypeID)
        {
            return (_licenseTypeID == 15);
        }

        public static bool IsLicensePurchase(this ProductPurchase _productPurchase)
        {
            bool flag = false;
            if ((_productPurchase.ProductTypeID != 5) && (_productPurchase.ProductTypeID != 2))
            {
                return flag;
            }
            return true;
        }

        public static List<SueetieProduct> LicensedProductList(this List<SueetieProduct> _products)
        {
            List<SueetieProduct> list = _products;
            if (LicensingCommon.IsFreeLicense(SueetiePackageType.Marketplace) && !CommerceHelper.IsAdminPage())
            {
                list = _products.Take<SueetieProduct>(5).ToList<SueetieProduct>();
            }
            return list;
        }

        public static List<SueetieProduct> LicensedProductListByCategory(this List<SueetieProduct> _products)
        {
            List<SueetieProduct> list = _products;
            if (LicensingCommon.IsFreeLicense(SueetiePackageType.Marketplace) && !CommerceHelper.IsAdminPage())
            {
                list = _products.Take<SueetieProduct>(3).ToList<SueetieProduct>();
            }
            return list;
        }

        public static string ShowDotXVersion(this ProductLicense _productLicense)
        {
            return (Math.Floor(_productLicense.Version).ToString() + ".x");
        }
    }
}

