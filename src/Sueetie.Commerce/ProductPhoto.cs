namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ProductPhoto
    {
        public DateTime DateCreated { get; set; }

        public bool IsMainPreview { get; set; }

        public int PhotoID { get; set; }

        public int ProductID { get; set; }
    }
}

