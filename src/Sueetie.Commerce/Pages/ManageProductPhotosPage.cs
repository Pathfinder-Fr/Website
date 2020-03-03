namespace Sueetie.Commerce.Pages
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Core;

    public class ManageProductPhotosPage : MarketplaceAdminPage
    {
        protected Label AdTitleLabel;
        protected HyperLink BackToEditAdLink;
        protected Panel MainUploadsPanel;
        protected Panel NoUploadsPanel;
        protected GridView PhotoGridView;
        protected HyperLink ShowProductLink;
        protected Label UploadErrorMessage;
        protected DetailsView UploadPhotoDetailsView;

        public ManageProductPhotosPage()
            : base("admin_marketplace_photos")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                int result = 0;
                string s = base.Request.QueryString["id"];
                bool isMarketplaceAdministrator = base.CurrentSueetieUser.IsMarketplaceAdministrator;
                if ((s != null) && int.TryParse(s, out result))
                {
                    base.CurrentSueetieProduct = Products.GetSueetieProduct(result);
                }
                if ((base.CurrentSueetieProduct != null) && ((base.CurrentSueetieProduct.UserID == base.CurrentSueetieUser.UserID) || isMarketplaceAdministrator))
                {
                    this.AdTitleLabel.Text = base.CurrentSueetieProduct.Title;
                    string str2 = "id=" + base.Server.UrlEncode(s);
                    this.ShowProductLink.NavigateUrl = "/" + SueetieApplications.Get().Marketplace.ApplicationKey + "/ShowProduct.aspx?" + str2;
                    this.ShowProductLink.Target = "_blank";
                    this.BackToEditAdLink.NavigateUrl = "EditProduct.aspx?" + str2;
                    this.BackToEditAdLink.Visible = isMarketplaceAdministrator;
                }
                else
                {
                    base.Response.Redirect("/default.aspx", true);
                }
                this.MainUploadsPanel.Visible = true;
                this.NoUploadsPanel.Visible = false;
            }
        }

        protected void PhotoDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            List<ProductPhoto> returnValue = e.ReturnValue as List<ProductPhoto>;
            if (returnValue != null)
            {
                this.UploadPhotoDetailsView.Visible = returnValue.Count < 100;
            }
            else
            {
                this.UploadPhotoDetailsView.Visible = true;
            }
        }

        protected void PhotoGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("SelectAsPreview"))
            {
                int photoID = Convert.ToInt32(e.CommandArgument);
                ProductPhotos.SetAdPreviewPhoto(base.CurrentSueetieProduct.ProductID, photoID);
                this.PhotoGridView.DataBind();
            }
        }

        protected void UploadPhotoDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            byte[] imageFile = e.Values["UploadBytes"] as byte[];
            if ((imageFile == null) || (imageFile.Length == 0))
            {
                e.Cancel = true;
                this.UploadErrorMessage.Visible = false;
            }
            else
            {
                try
                {
                    byte[] fullImage = ProductPhotos.ResizeImageFile(imageFile, SueetiePhotoSize.Full);
                    byte[] mediumImage = ProductPhotos.ResizeImageFile(imageFile, SueetiePhotoSize.Medium);
                    byte[] smallImage = ProductPhotos.ResizeImageFile(imageFile, SueetiePhotoSize.Small);
                    e.Values.Add("bytesFull", fullImage);
                    e.Values.Add("bytesMedium", mediumImage);
                    e.Values.Add("bytesSmall", smallImage);
                    e.Values.Remove("UploadBytes");
                    bool flag = this.PhotoGridView.Rows.Count == 0;
                    e.Values.Add("useAsPreview", flag);
                    this.UploadErrorMessage.Visible = false;
                }
                catch
                {
                    e.Cancel = true;
                    this.UploadErrorMessage.Visible = true;
                    throw;
                }
            }
        }
    }
}

