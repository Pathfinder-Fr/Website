namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ProductPurchase
    {
        public string ActionCode { get; set; }

        public int ActionID { get; set; }

        public int CartLinkID { get; set; }

        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string LicenseTypeDescription { get; set; }

        public int LicenseTypeID { get; set; }

        public string PackageTypeDescription { get; set; }

        public int PackageTypeID { get; set; }

        public decimal Price { get; set; }

        public int ProductID { get; set; }

        public string ProductTypeCode { get; set; }

        public int ProductTypeID { get; set; }

        public DateTime PurchaseDateTime { get; set; }

        public int PurchaseID { get; set; }

        public string PurchaseKey { get; set; }

        public int PurchaseTypeID { get; set; }

        public string Title { get; set; }

        public string TransactionXID { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }
    }
}

