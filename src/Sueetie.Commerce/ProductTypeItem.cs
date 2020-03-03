namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ProductTypeItem
    {
        public bool IsDisplayed { get; set; }

        public string ProductTypeCode { get; set; }

        public string ProductTypeDescription { get; set; }

        public int ProductTypeID { get; set; }
    }
}

