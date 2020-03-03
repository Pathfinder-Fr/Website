namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class CartLink
    {
        public int CartLinkID { get; set; }

        public bool IsActive { get; set; }

        public bool IsDisplayed { get; set; }

        public string LicenseTypeDescription { get; set; }

        public int LicenseTypeID { get; set; }

        public string PackageTypeCode { get; set; }

        public string PackageTypeDescription { get; set; }

        public int PackageTypeID { get; set; }

        public decimal Price { get; set; }

        public int ProductID { get; set; }

        public string ProductTypeCode { get; set; }

        public int ProductTypeID { get; set; }

        public string Title { get; set; }

        public decimal Version { get; set; }
    }
}

