namespace Sueetie.Commerce.Pages
{
    using System;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Commerce.Controls;
    using Sueetie.Core;
    using Sueetie.Licensing;

    public class ShowProductPage : MarketplaceBasePage
    {
        protected PlaceHolder AdActions;
        protected Panel AdDetailsPanel;
        protected Panel AdSavedPanel;
        protected CommerceBreadCrumbs CommerceCrumbs1;
        protected LinkButton DownloadButton2;
        protected HtmlGenericControl DownloadButtonLI2;
        protected Panel DownloadErrorPanel;
        protected TextBox EmailMessageTextBox;
        protected Panel EmailNotSentPanel;
        protected Panel EmailPanel;
        protected TextBox EmailRecipientAddressTextBox;
        protected RequiredFieldValidator EmailRecipientAddressTextBoxValidator1;
        protected TextBox EmailSenderAddressTextBox;
        protected RequiredFieldValidator EmailSenderAddressTextBoxValidator1;
        protected TextBox EmailSenderNameTextBox;
        protected Panel EmailSentPanel;
        protected TextBox EmailSubjectTextBox;
        protected ImageButton FullSizePhoto;
        protected HyperLink hyperlinkTitle;
        protected Label lblOptions;
        protected LinkButton lbtnProductKeyButton2;
        protected HyperLink LoginButton2;
        protected HtmlGenericControl LoginButtonLI2;
        protected Literal ltLicenseGeneration;
        protected Literal ltNewLicense;
        protected DataList PhotoList;
        protected Panel PhotoPanel;
        protected Panel pnlLicenseGeneration;
        protected HtmlGenericControl ProductKeyButtonLI2;
        protected TextBox ResponseCommentsTextBox;
        protected TextBox ResponseContactEmailTextBox;
        protected TextBox ResponseContactNameTextBox;
        protected RequiredFieldValidator ResponseEmailRequired1;
        protected Panel ResponsePanel;
        protected HtmlGenericControl SaveAddButtonLI1;
        protected HtmlGenericControl SaveAddButtonLI2;
        protected Label SubTitleLabel;

        public int CurrentPhotoIndex
        {
            get { return (int)(this.ViewState["CurrentPhotoIndex"] ?? -1); }
            set { this.ViewState["CurrentPhotoIndex"] = value; }
        }

        public bool IsFileAvailable
        {
            get { return (bool)(this.ViewState["IsFileAvailable"] ?? true); }
            set { this.ViewState["IsFileAvailable"] = value; }
        }

        public int NumPostBacks
        {
            get { return (int)(this.ViewState["NumPostBacks"] ?? 0); }
            set { this.ViewState["NumPostBacks"] = value; }
        }

        public int ProductID
        {
            get
            {
                if (this.ViewState["ProductID"] != null)
                {
                    return (int)this.ViewState["ProductID"];
                }
                return 0;
            }
            set
            {
                this.ViewState["ProductID"] = value;
            }
        }

        public string PurchaseKey
        {
            get
            {
                if (this.ViewState["PurchaseKey"] != null)
                {
                    return (string)this.ViewState["PurchaseKey"];
                }
                return "00000-00000-00000-00000-00000";
            }
            set
            {
                this.ViewState["PurchaseKey"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (DataHelper.GetIntFromQueryString("id", -1) == -1)
            {
                base.Response.Redirect("Browse.aspx");
                return;
            }

            if (this.Page.IsPostBack)
            {
                return;
            }

            var product = this.CurrentSueetieProduct;
            var user = this.CurrentSueetieUser;

            base.CurrentContentID = product.ContentID;
            this.PurchaseKey = CommerceCommon.GeneratePurchaseKey();
            this.LoginButtonLI2.Visible = false;
            this.ProductKeyButtonLI2.Visible = false;
            this.DownloadButtonLI2.Visible = false;

            this.DownloadButton2.Text = string.Format("Télécharger {0}", product.Title);
            this.LoginButton2.Text = SueetieLocalizer.GetMarketplaceString(string.Format("loginto_{0}", product.ProductType.ToString().ToLower()));

            if (!this.IsFileAvailable)
            {
                this.lblOptions.Text = "Nous sommes désolés, mais ce document n'est pas disponible actuellement.";
                return;
            }

            if (user.IsRegistered)
            {
                this.lblOptions.Text = "Actions possibles en tant que visiteur";

                this.ResponseContactEmailTextBox.Text = this.EmailSenderAddressTextBox.Text = user.Email;
                this.ResponseContactNameTextBox.Text = this.EmailSenderNameTextBox.Text = user.DisplayName;
            }
            else
            {
                this.lblOptions.Text = "Actions possibles en tant que membre";
            }

            if (product.PurchaseType == PurchaseType.FreeAll)
            {
                if (product.ProductType == ProductType.ElectronicDownload)
                {
                    this.DownloadButtonLI2.Visible = true;
                }
            }
            else if (product.PurchaseType == PurchaseType.FreeRegistered)
            {
                if (user.IsRegistered)
                {
                    this.DownloadButtonLI2.Visible = true;
                }
                else
                {
                    this.LoginButtonLI2.Visible = true;
                }
            }
        }

        protected void AdDetails_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var product = e.Item.DataItem as SueetieProduct;

            if (base.CurrentSueetieProduct.ProductTypeID == 1)
            {
                var breadcrumb = e.Item.FindControl("CommerceCrumbs1") as CommerceBreadCrumbs;
                if (breadcrumb != null)
                {
                    breadcrumb.CurrentCategoryID = base.CurrentSueetieProduct.CategoryID;
                }

                var sizeLiteral = e.Item.FindControl("ltSize") as Literal;
                if (sizeLiteral != null)
                {
                    var filePath = product.ResolveFilePath(this.Server);
                    var fileInfo = new FileInfo(filePath);
                    var fileSize = fileInfo.Length;

                    if (fileSize >= 1000 * 1000)
                    {
                        sizeLiteral.Text = string.Format("{0:#.##} Mo", (float)fileSize / (1000 * 1000));
                    }
                    else
                    {
                        sizeLiteral.Text = string.Format("{0:#.##} Ko", fileSize / 1000);
                    }
                }
            }
            else
            {
                HtmlTableRow row = e.Item.FindControl("rwSize") as HtmlTableRow;
                row.Visible = false;
            }

            if (base.CurrentSueetieProduct.ProductTypeID != 5)
            {
                Literal literal2 = e.Item.FindControl("ltPrice") as Literal;
                literal2.Text = CommerceHelper.FreeIt(base.CurrentSueetieProduct.Price);
            }
            else
            {
                HtmlTableRow row2 = e.Item.FindControl("rwPrice") as HtmlTableRow;
                row2.Visible = false;
            }

            if (product != null)
            {
                this.hyperlinkTitle.Text = product.Title;
                this.hyperlinkTitle.NavigateUrl = string.Format("ShowProduct.aspx?id={0}", product.ProductID);
                this.SubTitleLabel.Text = product.SubTitle;
                if (product.StatusTypeID == 100)
                {
                    if (base.CurrentSueetieUser.UserID != product.UserID)
                    {
                        Products.UpdateProductViewCount(this.ProductID);
                    }
                }
                else
                {
                    this.AdActions.Visible = false;
                }
            }
        }

        protected void AdsDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            SueetieProduct returnValue = e.ReturnValue as SueetieProduct;
            if (returnValue == null)
            {
                base.Response.Redirect("~/Browse.aspx", true);
            }
            else
            {
                this.ProductID = returnValue.ProductID;
                StringBuilder builder = new StringBuilder();
                builder.Append("Document : ");
                builder.AppendLine(returnValue.Title);
                builder.Append("Catégorie : ");
                builder.AppendLine(returnValue.CategoryName);
                builder.Append("Prix : ");
                builder.AppendFormat("{0:c}", returnValue.Price);
                builder.AppendLine();
                builder.AppendLine();
                builder.Append("Url : ");
                builder.AppendLine(string.Format("http://{0}/{1}/ShowProduct.aspx?id={2}", HttpContext.Current.Request.Url.Host, SueetieApplications.Current.ApplicationKey, returnValue.ProductID));
                builder.AppendLine();
                builder.AppendLine(SiteSettings.Instance.SiteName);
                builder.AppendLine(string.Format("http://{0}", HttpContext.Current.Request.Url.Host));
                this.EmailMessageTextBox.Text = builder.ToString();
                this.EmailSubjectTextBox.Text = "Take a look at: " + returnValue.Title;
            }
        }

        protected void CancelEmailButton_Click(object sender, EventArgs e)
        {
            this.HidePanel(this.EmailPanel);
        }

        protected void CancelResponseButton_Click(object sender, EventArgs e)
        {
            this.HidePanel(this.ResponsePanel);
        }

        protected void CategoryPath_CategorySelectionChanged(object sender, CategorySelectionChangedEventArgs e)
        {
            base.Response.Redirect("Browse.aspx?c=" + e.CategoryId.ToString());
        }

        protected void Download_Click(object sender, EventArgs e)
        {
            if (!this.IsFileAvailable)
            {
                base.Response.Redirect(base.Request.RawUrl + "&r=1");
                return;
            }

            ProductPurchase productPurchase = new ProductPurchase
            {
                ProductID = base.CurrentSueetieProduct.ProductID,
                UserID = base.CurrentSueetieUserID,
                PurchaseKey = this.PurchaseKey,
                CartLinkID = -1,
                ActionID = 1
            };

            Purchases.RecordPurchase(productPurchase);

            string sourceFileName = this.CurrentSueetieProduct.ResolveFilePath(this.Server);


            base.Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(sourceFileName));
            base.Response.Clear();
            base.Response.ContentType = "application/zip";
            this.Response.WriteFile(sourceFileName);
            base.Response.End();
        }

        protected void EmailAdButton_Click(object sender, EventArgs e)
        {
            this.SetActivePanel(this.EmailPanel);
        }

        protected void EmailSubmitButton_Click(object sender, EventArgs e)
        {
            this.EmailSenderAddressTextBoxValidator1.Validate();
            this.EmailRecipientAddressTextBoxValidator1.Validate();
            if (this.Page.IsValid)
            {
                if (CommerceHelper.SendTellFriendEmail(this.ProductID, base.Server.HtmlEncode(this.EmailSenderNameTextBox.Text), base.Server.HtmlEncode(this.EmailSenderAddressTextBox.Text), base.Server.HtmlEncode(this.EmailRecipientAddressTextBox.Text), base.Server.HtmlEncode(this.EmailSubjectTextBox.Text), base.Server.HtmlEncode(this.EmailMessageTextBox.Text)))
                {
                    this.SetActivePanel(this.EmailSentPanel);
                }
                else
                {
                    this.SetActivePanel(this.EmailNotSentPanel);
                }
            }
        }

        protected void FullSizePhoto_Click(object sender, ImageClickEventArgs e)
        {
            this.ShowNextPhoto();
        }

        protected void GenerateProductKey_Click(object sender, EventArgs e)
        {
            var productPackage = CommerceCommon.GetProductPackage(base.CurrentSueetieProduct.ProductID);
            var purchase2 = new ProductPurchase
            {
                UserID = base.CurrentSueetieUserID,
                CartLinkID = CommerceCommon.GetCartLinkList(base.CurrentSueetieProduct.ProductID).Find(p => p.LicenseTypeID == 1).CartLinkID,
                ProductID = base.CurrentSueetieProduct.ProductID,
                PurchaseKey = CommerceCommon.GeneratePurchaseKey(),
                ActionID = 2
            };

            var productPurchase = purchase2;
            var num = Purchases.RecordPurchase(productPurchase);
            var spt = (SueetiePackageType)Enum.ToObject(typeof(SueetiePackageType), productPackage.PackageTypeID);

            var productLicense = new ProductLicense
            {
                License = LicensingCommon.CreateLicenseKey(SueetieLicenseType.Free, spt),
                PackageTypeID = productPackage.PackageTypeID,
                LicenseTypeID = 1,
                Version = productPackage.Version,
                UserID = base.CurrentSueetieUserID,
                CartLinkID = productPurchase.CartLinkID,
                PurchaseID = num
            };

            Licenses.CreateProductLicense(productLicense);

            this.ltLicenseGeneration.Text = SueetieLocalizer.GetMarketplaceString("license_created_message");
            this.ltNewLicense.Text = productLicense.License;
            this.SetActivePanel(this.pnlLicenseGeneration);
        }

        protected void HidePanel(Panel panel)
        {
            panel.Visible = false;
            this.Page.SetFocus(this.AdDetailsPanel);
        }

        protected void HidePhotoPanel_Click(object sender, EventArgs e)
        {
            this.HidePanel(this.PhotoPanel);
        }

        protected void PhotoList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName.Equals("ShowFullSize"))
            {
                int photoId = Convert.ToInt32(e.CommandArgument);
                this.ShowFullSizePhoto(photoId);
                this.CurrentPhotoIndex = e.Item.ItemIndex;
            }
        }

        protected void RespondButton_Click(object sender, EventArgs e)
        {
            this.SetActivePanel(this.ResponsePanel);
        }

        protected void ResponseSubmitButton_Click(object sender, EventArgs e)
        {
            this.ResponseEmailRequired1.Validate();
            if (this.Page.IsValid)
            {
                if (CommerceHelper.SendAdResponse(this.ProductID, base.Server.HtmlEncode(this.ResponseContactNameTextBox.Text), base.Server.HtmlEncode(this.ResponseContactEmailTextBox.Text), base.Server.HtmlEncode(this.ResponseCommentsTextBox.Text)))
                {
                    this.SetActivePanel(this.EmailSentPanel);
                }
                else
                {
                    this.SetActivePanel(this.EmailNotSentPanel);
                }
            }
        }

        protected void SaveAdButton_Click(object sender, EventArgs e)
        {
        }

        protected void SetActivePanel(Panel panel)
        {
            if (panel != null)
            {
                this.ResponsePanel.Visible = false;
                this.EmailPanel.Visible = false;
                this.PhotoPanel.Visible = false;
                this.EmailSentPanel.Visible = false;
                this.EmailNotSentPanel.Visible = false;
                this.AdSavedPanel.Visible = false;
                this.DownloadErrorPanel.Visible = false;
                this.pnlLicenseGeneration.Visible = false;
                panel.Visible = true;
                this.Page.SetFocus(this.AdDetailsPanel);
            }
        }

        protected void ShowFullSizePhoto(int photoId)
        {
            this.PhotoPanel.Visible = true;
            this.FullSizePhoto.ImageUrl = string.Format("/util/handlers/PhotoDisplay.ashx?pid={0}&size=full&t=2", photoId.ToString());
            this.SetActivePanel(this.PhotoPanel);
        }

        protected void ShowNextPhoto()
        {
            int currentPhotoIndex = this.CurrentPhotoIndex;
            if (currentPhotoIndex != -1)
            {
                int num2 = (currentPhotoIndex + 1) % this.PhotoList.DataKeys.Count;
                int photoId = Convert.ToInt32(this.PhotoList.DataKeys[num2]);
                this.ShowFullSizePhoto(photoId);
                this.CurrentPhotoIndex = num2;
            }
        }
    }
}

