namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ProductLicense
    {
        public int BoardID { get; set; }

        public int CartLinkID { get; set; }

        public string CustomLicenseTypeDescription { get; set; }

        public DateTime DatePurchased { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public int ForumUserID { get; set; }

        public string License { get; set; }

        public int LicenseID { get; set; }

        public string LicenseTypeDescription { get; set; }

        public int LicenseTypeID { get; set; }

        public int MajorVersion { get; set; }

        public Guid MembershipID { get; set; }

        public string PackageTypeCode { get; set; }

        public string PackageTypeDescription { get; set; }

        public int PackageTypeID { get; set; }

        public decimal Price { get; set; }

        public int ProductID { get; set; }

        public decimal PurchasedVersion { get; set; }

        public int PurchaseID { get; set; }

        public string PurchaseKey { get; set; }

        public string TransactionXID { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public decimal Version { get; set; }
    }
}

