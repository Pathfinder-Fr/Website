namespace Sueetie.Commerce
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Web;

    [Serializable]
    public class ProductCategory
    {
        private string categoryHtmlName;

        public string CategoryDescription { get; set; }

        public int CategoryID { get; set; }

        public string CategoryName
        {
            get;
            private set;
        }

        public string CategoryHtmlName
        {
            get { return this.categoryHtmlName; }
            set
            {
                if (this.categoryHtmlName != value)
                {
                    this.categoryHtmlName = value;
                    this.CategoryName = HttpUtility.HtmlDecode(value);
                }
            }
        }

        public string LevelIndentedName { get; set; }

        public string NameWithActiveCount { get; set; }

        public int NumActiveProducts { get; set; }

        public int ParentCategoryID { get; set; }

        public string Path { get; set; }
    }
}

