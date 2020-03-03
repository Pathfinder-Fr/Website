namespace Sueetie.Commerce
{
    using System;
    using System.IO;
    using System.Web;

    [Serializable]
    public class SueetieProduct
    {
        public SueetieProduct()
        {
        }

        public int ApplicationID { get; set; }

        public int CategoryID { get; set; }

        public string CategoryName { get; set; }

        public string CategoryPath { get; set; }

        public int ContentID { get; set; }

        public int ContentTypeID { get; set; }

        public DateTime DateApproved { get; set; }

        public DateTime DateCreated { get; set; }

        public string DisplayName { get; set; }

        public string DocumentationURL { get; set; }

        public string DownloadURL { get; set; }

        public string Email { get; set; }

        public DateTime ExpirationDate { get; set; }

        public string ImageGalleryURL { get; set; }

        public int NumDownloads { get; set; }

        public int NumViews { get; set; }

        public int PreviewImageID { get; set; }

        public decimal Price { get; set; }

        public string ProductDescription { get; set; }

        public int ProductID { get; set; }

        public string ProductTypeCode { get; set; }

        public string ProductTypeDescription { get; set; }

        public int ProductTypeID { get; set; }

        public ProductType ProductType
        {
            get { return (ProductType)this.ProductTypeID; }
        }

        public string PurchaseTypeCode { get; set; }

        public int PurchaseTypeID { get; set; }

        public PurchaseType PurchaseType
        {
            get { return (PurchaseType)this.PurchaseTypeID; }
        }

        public string StatusTypeCode { get; set; }

        public int StatusTypeID { get; set; }

        public ProductStatusType StatusType
        {
            get { return (ProductStatusType)this.StatusTypeID; }
        }

        public string SubTitle { get; set; }

        public string Title { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string ResolveFilePath(HttpServerUtility server)
        {
            if (this.DateCreated == DateTime.MinValue)
            {
                this.DateCreated = DateTime.Now;
            }

            var yearPart = this.DateCreated.Year.ToString();
            var url = this.DownloadURL;

            string filePath;
            if (Path.IsPathRooted(url))
            {
                // Absolute location
                filePath = url;
            }
            else if (url.StartsWith("~"))
            {
                // Relative location with AppBase
                url = url.Substring(1);
                filePath = server.MapPath(url);
            }
            else
            {
                // Relative location to downloads dir
                filePath = string.Format(@"{0}Marketplace\Files\{1}\{2}", AppDomain.CurrentDomain.BaseDirectory, yearPart, url);
            }

            return filePath;
        }
    }
}