namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ProductPackage
    {
        public int MajorVersion { get; set; }

        public int PackageTypeID { get; set; }

        public int ProductID { get; set; }

        public int ProductPackageID { get; set; }

        public decimal Version { get; set; }
    }
}

