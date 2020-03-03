namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class CommerceSetting
    {
        public CommerceSetting()
        {
        }

        public CommerceSetting(string _name, string _value)
        {
            this.SettingName = _name;
            this.SettingValue = _value;
        }

        public string SettingName { get; set; }

        public string SettingValue { get; set; }
    }
}

