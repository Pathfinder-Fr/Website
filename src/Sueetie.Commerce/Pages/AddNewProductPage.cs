namespace Sueetie.Commerce.Pages
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Commerce.Controls;
    using Sueetie.Controls;
    using Sueetie.Core;

    public class AddNewProductPage : MarketplaceAdminPage
    {
        private const string OtherLocationText = "Other...";

        protected WizardStep AdDetailsStep;
        protected CommerceCategoryPath CategoryPath;
        protected Label CategoryPathLabel;
        protected htmlEditor DescriptionTextBox;
        protected Label lblNeedCategories;
        protected PlaceHolder phHasCategories;
        protected PlaceHolder phNoCategories;
        protected Wizard PostAdWizard;
        protected TextBox PriceTextBox;
        protected RadioButtonList rblProductTypes;
        protected RadioButtonList rblPurchaseTypes;
        protected TextBox SubTitleTextBox;
        protected TextBox TitleTextBox;
        protected HyperLink UploadImagesLink;
        protected TextBox UrlTextBox;
        protected FileUpload FileUploadControl;

        public AddNewProductPage()
            : base("admin_marketplace_newproduct")
        {
        }

        protected void CategoryPath_CategorySelectionChanged(object sender, CategorySelectionChangedEventArgs e)
        {
            this.UpdateCategoryDisplay();
        }

        protected void ChangeCategoryButton_Click(object sender, EventArgs e)
        {
            this.SetCurrentCategory(0);
            this.PostAdWizard.MoveTo(this.PostAdWizard.WizardSteps[0]);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DescriptionTextBox.Attributes.Add("onkeydown", "textCounter(this,500);");
            this.DescriptionTextBox.Attributes.Add("onkeyup", "textCounter(this,500);");

            if (!this.Page.IsPostBack)
            {
                this.lblNeedCategories.Text = "You must add a category before placing an ad. If you are a Marketplace Administrator</br> you can do that <a href=\"/marketplace/admin/Categories.aspx\">here.</a>";

                bool flag = CommerceCommon.HasMarketplaceCategories();
                this.phHasCategories.Visible = flag;
                this.phNoCategories.Visible = !flag;
                foreach (ProductTypeItem item in CommerceCommon.GetProductTypeItemList())
                {
                    this.rblProductTypes.Items.Add(new ListItem(item.ProductTypeDescription, item.ProductTypeID.ToString()));
                }
                foreach (PurchaseTypeItem item2 in CommerceCommon.GetPurchaseTypeItemList())
                {
                    this.rblPurchaseTypes.Items.Add(new ListItem(item2.PurchaseTypeDescription, item2.PurchaseTypeID.ToString()));
                }
                this.rblPurchaseTypes.Items.FindByValue("1").Selected = true;
                if (CommerceCommon.IsSueetiePackageDistributor())
                {
                    this.rblProductTypes.Items.Add(new ListItem("Sueetie Package", "5"));
                }
                this.rblProductTypes.Items.FindByValue("1").Selected = true;
                this.PostAdWizard.MoveTo(this.PostAdWizard.WizardSteps[0]);
            }
            Button button = (Button)this.PostAdWizard.FindControl("StartNavigationTemplateContainerID$StartNextButton");
            button.Visible = false;
        }

        protected void PostAdWizard_FinishButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (this.Page.IsValid)
            {
                var purchaseType = PurchaseType.Commercial;
                if (Enum.IsDefined(typeof(PurchaseType), Convert.ToInt32(this.rblPurchaseTypes.SelectedValue)))
                {
                    purchaseType = (PurchaseType)Enum.Parse(typeof(PurchaseType), this.rblPurchaseTypes.SelectedValue);
                }

                var sueetieProduct = new SueetieProduct
                {
                    UserID = this.CurrentSueetieUserID,
                    CategoryID = this.CategoryPath.CurrentCategoryId,
                    Title = this.TitleTextBox.Text,
                    SubTitle = this.SubTitleTextBox.Text,
                    ProductDescription = this.DescriptionTextBox.Text,
                    DateCreated = DateTime.Now,
                    DownloadURL = this.UrlTextBox.Text,
                    Price = decimal.Parse(this.PriceTextBox.Text),
                    PurchaseTypeID = (int)purchaseType,
                    ProductTypeID = int.Parse(this.rblProductTypes.SelectedValue.ToString()),
                    StatusTypeID = 100
                };

                if (this.FileUploadControl != null && string.IsNullOrWhiteSpace(sueetieProduct.DownloadURL) && !string.IsNullOrWhiteSpace(this.FileUploadControl.FileName))
                {
                    sueetieProduct.DownloadURL = this.FileUploadControl.FileName;
                }

                int id = Products.CreateSueetieProduct(sueetieProduct);

                if (this.FileUploadControl != null && this.FileUploadControl.FileBytes != null && this.FileUploadControl.FileBytes.Length != 0)
                {
                    var localPath = sueetieProduct.ResolveFilePath(this.Server);
                    var directory = Path.GetDirectoryName(localPath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    this.FileUploadControl.SaveAs(localPath);
                }

                var sueetieContent = new SueetieContent
                {
                    ContentTypeID = 0x12,
                    ApplicationID = SueetieApplications.Get().Marketplace.ApplicationID,
                    IsRestricted = false,
                    SourceID = id,
                    UserID = base.CurrentSueetieUserID,
                    Permalink = string.Concat(new object[] { "/", SueetieApplications.Get().Marketplace.ApplicationKey, "/ShowProduct.aspx?id=", id })
                };

                var contentId = SueetieCommon.AddSueetieContent(sueetieContent);
                SueetieLogs.LogUserEntry(UserLogCategoryType.MarketplaceProduct, contentId, base.CurrentSueetieUserID);

                this.UploadImagesLink.Visible = true;
                this.UploadImagesLink.NavigateUrl = "ManagePhotos.aspx?id=" + id.ToString();

                CommerceCommon.ClearMarketplaceCache();
            }
        }

        protected void PostAdWizard_PreviousButtonClick(object sender, WizardNavigationEventArgs e)
        {
            if (e.NextStepIndex == 0)
            {
                this.SetCurrentCategory(0);
            }
        }

        protected void PriceValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            decimal result = -1M;
            if (decimal.TryParse(this.PriceTextBox.Text, out result))
            {
                args.IsValid = result >= 0M;
            }
            else
            {
                args.IsValid = false;
            }
        }

        protected void SetCurrentCategory(int categoryID)
        {
            base.CurrentProductCategory = ProductCategories.GetProductCategory(categoryID);
            this.CategoryPath.CurrentCategoryId = categoryID;
            this.UpdateCategoryDisplay();
        }

        protected void SubcategoriesDS_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            List<ProductCategory> returnValue = e.ReturnValue as List<ProductCategory>;
            if ((returnValue == null) || (returnValue.Count == 0))
            {
                this.PostAdWizard.MoveTo(this.PostAdWizard.WizardSteps[1]);
            }
        }

        protected void SubcategoriesList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int categoryID = Convert.ToInt32(e.CommandArgument);
            this.SetCurrentCategory(categoryID);
        }

        protected void UpdateCategoryDisplay()
        {
            this.CategoryPathLabel.Text = this.CategoryPath.FullCategoryPath;
        }

        protected void URLValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = false;
            TextBox box = this.AdDetailsStep.FindControl("UrlTextBox") as TextBox;
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

