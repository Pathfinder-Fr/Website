namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ActionTypeItem
    {
        public string ActionCode { get; set; }

        public string ActionDescription { get; set; }

        public int ActionID { get; set; }

        public bool IsDisplayed { get; set; }
    }
}

