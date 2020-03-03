namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class PaymentServiceSetting
    {
        public PaymentServiceSetting(int _paymentServiceID, string _name, string _value)
        {
            this.PaymentServiceID = _paymentServiceID;
            this.SettingName = _name;
            this.SettingValue = _value;
        }

        public int PaymentServiceID { get; set; }

        public string SettingName { get; set; }

        public string SettingValue { get; set; }
    }
}

