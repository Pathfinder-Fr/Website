namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class PurchaseTypeItem
    {
        public bool IsDisplayed { get; set; }

        public string PurchaseTypeCode { get; set; }

        public string PurchaseTypeDescription { get; set; }

        public int PurchaseTypeID { get; set; }
    }
}

