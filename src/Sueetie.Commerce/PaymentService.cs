namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class PaymentService
    {
        public string AccountName { get; set; }

        public string CartUrl { get; set; }

        public string IdentityToken { get; set; }

        public bool IsPrimary { get; set; }

        public string PaymentServiceDescription { get; set; }

        public int PaymentServiceID { get; set; }

        public string PaymentServiceName { get; set; }

        public string PurchaseUrl { get; set; }

        public string ReturnUrl { get; set; }

        public string SharedPaymentServicePage { get; set; }

        public string ShoppingUrl { get; set; }

        public string TransactionUrl { get; set; }
    }
}

