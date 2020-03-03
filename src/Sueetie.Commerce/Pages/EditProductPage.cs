namespace Sueetie.Commerce.Pages
{
    using Sueetie.Commerce;
    using Sueetie.Commerce.Controls;
    using Sueetie.Core;
    using System;
    using System.Web.UI.WebControls;
    using System.IO;

    public class EditProductPage : MarketplaceAdminPage
    {
        protected FormView AdFormView;
        protected CommerceCategoryDropDown CategoryDropDown;
        protected PlaceHolder ChangeCategoryPanel;
        protected HyperLink ManagePhotosLink;
        protected HyperLink ShowProductLink;
        protected FileUpload UploadFileControl;
        protected Button UploadFileButton;

        public EditProductPage()
            : base("admin_marketplace_editproduct")
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int result = 0;
            string s = base.Request.QueryString["id"];
            if ((s == null) || !int.TryParse(s, out result))
            {
                base.Response.Redirect("default.aspx");
            }

            if (!this.Page.IsPostBack)
            {
                base.CurrentSueetieProduct = Products.GetSueetieProduct(result);
            }
        }

        protected void AdDataSource_OnUpdating(object sender, ObjectDataSourceMethodEventArgs e)
        {
            if (this.Page.IsValid)
            {
                var purchaseTypes = this.AdFormView.FindControl("ddPurchaseTypes") as DropDownList;
                var statusTypes = this.AdFormView.FindControl("ddStatusTypes") as DropDownList;
                var productTypes = this.AdFormView.FindControl("rblProductTypes") as RadioButtonList;
                var textBox = this.AdFormView.FindControl("PriceTextBox") as TextBox;
                var result = 0M;
                decimal.TryParse(textBox.Text, out result);

                var product = new SueetieProduct
                {
                    CategoryID = this.CategoryDropDown.CurrentCategoryId,
                    Title = e.InputParameters["title"] as string,
                    SubTitle = e.InputParameters["subtitle"] as string,
                    ProductDescription = e.InputParameters["productdescription"] as string,
                    Price = result,
                    ProductID = (int)e.InputParameters["productid"],
                    DownloadURL = e.InputParameters["downloadurl"] as string,
                    DocumentationURL = e.InputParameters["documentationurl"] as string,
                    ImageGalleryURL = e.InputParameters["imagegalleryurl"] as string,
                    PurchaseTypeID = int.Parse(purchaseTypes.SelectedValue),
                    StatusTypeID = int.Parse(statusTypes.SelectedValue),
                    ProductTypeID = int.Parse(productTypes.SelectedValue)
                };

                e.InputParameters.Remove("productid");
                e.InputParameters.Remove("title");
                e.InputParameters.Remove("subtitle");
                e.InputParameters.Remove("productdescription");
                e.InputParameters.Remove("price");
                e.InputParameters.Remove("purchasetypeid");
                e.InputParameters.Remove("downloadurl");
                e.InputParameters.Remove("ImageGalleryURL");
                e.InputParameters.Remove("DocumentationURL");

                e.InputParameters.Add("sueetieProduct", product);
            }
        }

        protected void AdDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            var returnValue = e.ReturnValue as SueetieProduct;

            if (returnValue != null && this.CurrentSueetieUser.IsMarketplaceAdministrator)
            {
                var productId = returnValue.ProductID.ToString();
                this.CategoryDropDown.CurrentCategoryId = returnValue.CategoryID;
                this.ManagePhotosLink.NavigateUrl = "ManagePhotos.aspx?id=" + productId;
                this.ShowProductLink.NavigateUrl = string.Format("/{0}/ShowProduct.aspx?id={1}", SueetieApplications.Get().Marketplace.ApplicationKey, productId);
                this.ShowProductLink.Target = "_blank";
            }
        }

        protected void AdDataSource_Updated(object sender, ObjectDataSourceStatusEventArgs e)
        {
            if (e.Exception == null)
            {
                base.Response.Redirect("manageproducts.aspx");
            }
        }

        protected void AdFormView_ItemUpdating(object sender, FormViewUpdateEventArgs e)
        {
            if (!this.Page.IsValid)
            {
                e.Cancel = true;
            }
        }

        protected void AdFormView_OnDataBound(object sender, EventArgs e)
        {
            var dataItem = (SueetieProduct)this.AdFormView.DataItem;

            // Status Type
            var statusTypes = this.AdFormView.FindControl("ddStatusTypes") as DropDownList;
            foreach (ProductStatusType type in Enum.GetValues(typeof(ProductStatusType)))
            {
                statusTypes.Items.Add(new ListItem(Enum.GetName(typeof(ProductStatusType), type), type.ToString("D")));
            }

            statusTypes.Items.FindByValue(dataItem.StatusTypeID.ToString()).Selected = true;

            // Product Types
            var productTypes = this.AdFormView.FindControl("rblProductTypes") as RadioButtonList;
            foreach (var item in CommerceCommon.GetProductTypeItemList())
            {
                productTypes.Items.Add(new ListItem(item.ProductTypeDescription, item.ProductTypeID.ToString()));
            }

            if (CommerceCommon.IsSueetiePackageDistributor())
            {
                productTypes.Items.Add(new ListItem("Sueetie Package", "5"));
            }

            productTypes.Items.FindByValue(dataItem.ProductTypeID.ToString()).Selected = true;

            // Purchase Types
            var purchaseTypes = this.AdFormView.FindControl("ddPurchaseTypes") as DropDownList;
            foreach (var item2 in CommerceCommon.GetPurchaseTypeItemList())
            {
                purchaseTypes.Items.Add(new ListItem(item2.PurchaseTypeDescription, item2.PurchaseTypeID.ToString()));
            }

            purchaseTypes.Items.FindByValue(dataItem.PurchaseTypeID.ToString()).Selected = true;

        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            base.Response.Redirect("manageproducts.aspx");
        }

        protected void UploadFileButton_Click(object sender, EventArgs e)
        {
            var localPath = this.CurrentSueetieProduct.ResolveFilePath(this.Server);

            var directory = Path.GetDirectoryName(localPath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            this.UploadFileControl.SaveAs(localPath);
        }

        protected void ChangeCategoryButton_Click(object sender, EventArgs e)
        {
            this.ChangeCategoryPanel.Visible = true;
            this.AdFormView.Enabled = false;
        }

        protected void ChangeCategoryCancelButton_Click(object sender, EventArgs e)
        {
            this.ChangeCategoryPanel.Visible = false;
            this.AdFormView.Enabled = true;
        }

        protected void ChangeCategoryOkButton_Click(object sender, EventArgs e)
        {
            if (this.CategoryDropDown.SelectedCategoryId != 0)
            {
                Products.UpdateProductCategory(Convert.ToInt32(this.AdFormView.DataKey.Value), this.CategoryDropDown.SelectedCategoryId);
                this.AdFormView.DataBind();
            }
            this.ChangeCategoryPanel.Visible = false;
            this.AdFormView.Enabled = true;
        }

        protected void PriceValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal result = -1M;
            TextBox box = this.AdFormView.FindControl("PriceTextBox") as TextBox;
            if (decimal.TryParse(box.Text, out result))
            {
                args.IsValid = result >= 0M;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void URLValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            TextBox box = this.AdFormView.FindControl("URLTextBox") as TextBox;
            if ((box != null) && !box.Text.Equals(string.Empty))
            {
                try
                {
                    Uri uri = new Uri(box.Text);
                    if (uri.IsWellFormedOriginalString() & ((uri.Scheme == "http") | (uri.Scheme == "https")))
                    {
                        args.IsValid = true;
                    }
                }
                catch
                {
                }
            }
            else
            {
                args.IsValid = true;
            }
        }
    }
}

