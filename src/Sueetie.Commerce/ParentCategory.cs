namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class ParentCategory
    {
        public int CategoryID { get; set; }

        public int NumActiveProducts { get; set; }

        public int ParentCategoryID { get; set; }

        public string ParentCategoryName { get; set; }
    }
}

