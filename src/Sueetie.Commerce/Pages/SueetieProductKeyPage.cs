namespace Sueetie.Commerce.Pages
{
    using Sueetie.Commerce;
    using Sueetie.Core;
    using Sueetie.Licensing;
    using System;
    using System.Web.Security;
    using System.Web.UI.WebControls;

    public class SueetieProductKeyPage : MarketplaceAdminPage
    {
        private MembershipUserCollection allRegisteredUsers;
        protected Button btnCreate;
        protected Button btnSearch;
        protected Button btnValidate;
        protected Label lblKey;
        protected Label lblProductKeyMember;
        protected Panel pnlProductKey;
        protected Panel pnlSearch;
        protected RadioButtonList rblLicenseTypes;
        protected RadioButtonList rblPackageTypes;
        protected TextBox txtKey;
        protected TextBox txtSearchText;
        protected GridView UsersGridView;

        public SueetieProductKeyPage() : base("admin_marketplace_productkey")
        {
            this.allRegisteredUsers = Membership.GetAllUsers();
        }

        private void BindAllUsers(bool reloadAllUsers)
        {
            MembershipUserCollection allUsers = null;
            if (reloadAllUsers)
            {
                allUsers = Membership.GetAllUsers();
            }
            string usernameToMatch = "";
            if (!string.IsNullOrEmpty(this.UsersGridView.Attributes["SearchText"]))
            {
                usernameToMatch = this.UsersGridView.Attributes["SearchText"];
            }
            if (!string.IsNullOrEmpty(this.UsersGridView.Attributes["SearchByEmail"]))
            {
                bool.Parse(this.UsersGridView.Attributes["SearchByEmail"]);
            }
            if (usernameToMatch.Length > 0)
            {
                allUsers = Membership.FindUsersByName(usernameToMatch);
            }
            else
            {
                allUsers = this.allRegisteredUsers;
            }
            this.UsersGridView.DataSource = allUsers;
            this.UsersGridView.DataBind();
        }

        protected void btnProductKey_OnClick(object sender, EventArgs e)
        {
            SueetiePackageType spt = (SueetiePackageType) Enum.ToObject(typeof(SueetiePackageType), int.Parse(this.rblPackageTypes.SelectedValue));
            SueetieLicenseType type2 = (SueetieLicenseType) Enum.ToObject(typeof(SueetieLicenseType), int.Parse(this.rblLicenseTypes.SelectedValue));
            CartLink cartLink = CommerceCommon.GetCartLink((int) spt, (int) type2);
            base.CurrentSueetieProduct = Products.GetSueetieProduct(cartLink.ProductID);
            ProductPurchase productPurchase = new ProductPurchase {
                UserID = this.ProductKeySueetieUser.UserID,
                CartLinkID = cartLink.CartLinkID,
                ProductID = cartLink.ProductID,
                PurchaseKey = CommerceCommon.GeneratePurchaseKey(),
                ActionID = base.CurrentSueetieProduct.ActionType()
            };
            int num = Purchases.RecordPurchase(productPurchase);
            ProductPackage productPackage = CommerceCommon.GetProductPackage(cartLink.ProductID);
            Licenses.CreateProductLicense(new ProductLicense { PackageTypeID = productPackage.PackageTypeID, LicenseTypeID = cartLink.LicenseTypeID, Version = productPackage.Version, UserID = this.ProductKeySueetieUser.UserID, CartLinkID = cartLink.CartLinkID, PurchaseID = num, License = LicensingCommon.CreateLicenseKey(type2, spt) });
            this.txtKey.Text = LicensingCommon.CreateLicenseKey(type2, spt);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this.UsersGridView.Attributes.Add("SearchText", "%" + this.txtSearchText.Text + "%");
            this.BindAllUsers(false);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                if (base.Request.QueryString["username"] != null)
                {
                    this.pnlProductKey.Visible = true;
                    this.pnlSearch.Visible = false;
                    this.ProductKeySueetieUser = SueetieUsers.GetThinSueetieUser(base.Request.QueryString["username"].ToString().ToLower());
                    this.lblProductKeyMember.Text = this.ProductKeySueetieUser.DisplayName;
                }
                else
                {
                    this.pnlProductKey.Visible = false;
                    this.pnlSearch.Visible = true;
                }
            }
        }

        public SueetieUser ProductKeySueetieUser
        {
            get
            {
                return (SueetieUser) this.ViewState["ProductKeySueetieUser"];
            }
            set
            {
                this.ViewState["ProductKeySueetieUser"] = value;
            }
        }
    }
}

