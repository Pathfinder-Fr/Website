namespace Sueetie.Commerce.Pages
{
    using Sueetie.Commerce;
    using Sueetie.Core;
    using Sueetie.Licensing;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.UI.WebControls;

    public class CompletePurchasePage : MarketplaceBasePage
    {
        public const string ITEM_NUMBER = "item_number";
        protected Label lblPurchaseTitle;
        protected Label lblTransactionXID;
        public const string NUM_CART_ITEMS = "num_cart_items";
        protected Panel pnlFailure;
        protected Panel pnlLicenses;
        protected Panel pnlPurchases;
        protected Panel pnlSuccess;
        protected Panel pnlTransactionCode;
        public const string QUANTITY = "quantity";
        protected Repeater rptLicenses;
        protected Repeater rptPurchases;
        public string transactionXID;
        public const string TXN_ID = "txn_id";

        private Dictionary<string, string> CreateDictionary(List<string> _tx)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            for (int i = 0; i < _tx.Count; i++)
            {
                int index = _tx[i].IndexOf("=");
                if ((index + 1) < _tx[i].Trim().Length)
                {
                    dictionary.Add(_tx[i].Substring(0, index), _tx[i].Substring(index + 1));
                }
            }
            return dictionary;
        }

        private void DisplayValues()
        {
            foreach (KeyValuePair<string, string> pair in this.PaymentProperties)
            {
                base.Response.Write(pair.Key + " : " + pair.Value + "<br />");
            }
        }

        private int GetNum(string key)
        {
            int num = 0;
            foreach (KeyValuePair<string, string> pair in this.PaymentProperties)
            {
                if (pair.Key == key)
                {
                    num = int.Parse(pair.Value);
                }
            }
            return num;
        }

        private decimal GetPrice(string key)
        {
            decimal num = 0M;
            foreach (KeyValuePair<string, string> pair in this.PaymentProperties)
            {
                if (pair.Key == key)
                {
                    num = Convert.ToDecimal(pair.Value);
                }
            }
            return num;
        }

        private string GetString(string key)
        {
            string str = string.Empty;
            foreach (KeyValuePair<string, string> pair in this.PaymentProperties)
            {
                if (pair.Key == key)
                {
                    str = pair.Value;
                }
            }
            return str;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.lblPurchaseTitle.Text = SueetieLocalizer.GetMarketplaceString("productpurchase_title_success");
                this.pnlLicenses.Visible = false;
                this.transactionXID = base.Request.QueryString["tx"];
                PaymentService primaryPaymentService = CommerceContext.Current.PrimaryPaymentService;
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(primaryPaymentService.TransactionUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] bytes = base.Request.BinaryRead(HttpContext.Current.Request.ContentLength);
                string str = Encoding.ASCII.GetString(bytes) + string.Format("&cmd=_notify-synch&tx={0}&at={1}", this.transactionXID, primaryPaymentService.IdentityToken);
                request.ContentLength = str.Length;
                StreamWriter writer = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
                writer.Write(str);
                writer.Close();
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                string input = reader.ReadToEnd();
                reader.Close();
                this.lblTransactionXID.Text = this.transactionXID;
                List<string> list = (from s in Regex.Split(input, "\n", RegexOptions.ExplicitCapture)
                    where !string.IsNullOrEmpty(s)
                    select s).ToList<string>();
                if (list[0] == "SUCCESS")
                {
                    List<ProductPurchase> list2 = new List<ProductPurchase>();
                    List<ProductLicense> list3 = new List<ProductLicense>();
                    this.PaymentProperties = this.CreateDictionary((from t in list
                        where t.Contains("=")
                        select t).ToList<string>());
                    int num = this.GetNum("num_cart_items");
                    int num2 = 0;
                    int cartLinkID = -1;
                    int num4 = -1;
                    this.transactionXID = this.GetString("txn_id");
                    for (int i = 1; i <= num; i++)
                    {
                        num2 = this.GetNum("quantity".AddItemNum(i));
                        cartLinkID = this.GetNum("item_number".AddItemNum(i));
                        CartLink cartLink = CommerceCommon.GetCartLink(cartLinkID);
                        for (int j = 0; j < num2; j++)
                        {
                            base.CurrentSueetieProduct = Products.GetSueetieProduct(cartLink.ProductID);
                            ProductPurchase item = new ProductPurchase {
                                TransactionXID = this.transactionXID,
                                UserID = base.CurrentSueetieUserID,
                                CartLinkID = cartLinkID,
                                ProductID = cartLink.ProductID,
                                PurchaseKey = CommerceCommon.GeneratePurchaseKey(),
                                ActionID = base.CurrentSueetieProduct.ActionType()
                            };
                            list2.Add(item);
                            num4 = Purchases.RecordPurchase(item);
                            if (base.CurrentSueetieProduct.ProductTypeID == 5)
                            {
                                ProductPackage productPackage = CommerceCommon.GetProductPackage(cartLink.ProductID);
                                SueetiePackageType spt = (SueetiePackageType) System.Enum.ToObject(typeof(SueetiePackageType), productPackage.PackageTypeID);
                                ProductLicense license = new ProductLicense {
                                    PackageTypeID = productPackage.PackageTypeID,
                                    LicenseTypeID = cartLink.LicenseTypeID,
                                    Version = productPackage.Version,
                                    UserID = base.CurrentSueetieUserID,
                                    CartLinkID = cartLinkID,
                                    PurchaseID = num4,
                                    License = Guid.NewGuid().ToString().ToUpper()
                                };
                                SueetieLicenseType type2 = (SueetieLicenseType) System.Enum.ToObject(typeof(SueetieLicenseType), cartLink.LicenseTypeID);
                                license.License = LicensingCommon.CreateLicenseKey(type2, spt);
                                list3.Add(license);
                                Licenses.CreateProductLicense(license);
                            }
                        }
                    }
                    this.rptPurchases.DataSource = Purchases.GetPurchasesByTransaction(this.transactionXID);
                    this.rptPurchases.DataBind();
                    if (list3.Count > 0)
                    {
                        this.rptLicenses.DataSource = Licenses.GetLicensesByTransaction(this.transactionXID);
                        this.rptLicenses.DataBind();
                        this.pnlLicenses.Visible = true;
                    }
                }
                else
                {
                    this.lblPurchaseTitle.Text = SueetieLocalizer.GetMarketplaceString("productpurchase_title_failure");
                    this.pnlLicenses.Visible = false;
                    this.pnlPurchases.Visible = false;
                    this.pnlTransactionCode.Visible = false;
                    this.pnlSuccess.Visible = false;
                    this.pnlFailure.Visible = true;
                    MailMessage message = new MailMessage {
                        From = new MailAddress(SiteSettings.Instance.FromEmail, SiteSettings.Instance.FromName)
                    };
                    MailAddress address = new MailAddress(SiteSettings.Instance.ContactEmail, SiteSettings.Instance.SiteName + SueetieLocalizer.GetMarketplaceString("purchase_failure_email_admin"));
                    message.To.Add(address);
                    message.Subject = SueetieLocalizer.GetMarketplaceString("purchase_failure_email_subject");
                    string str4 = SueetieLocalizer.GetMarketplaceString("purchase_failure_email_firstline") + Environment.NewLine + Environment.NewLine;
                    object obj2 = str4 + base.CurrentSueetieUser.UserName.ToString() + " (" + base.CurrentSueetieUser.DisplayName + ")";
                    string str3 = string.Concat(new object[] { obj2, Environment.NewLine, DateTime.Now.ToLongDateString(), ' ', DateTime.Now.ToLongTimeString() }) + Environment.NewLine + Environment.NewLine;
                    message.Body = str3;
                    if (SueetieConfiguration.Get().Core.SendEmails)
                    {
                        EmailHelper.AsyncSendEmail(message);
                    }
                }
            }
        }

        protected void rptPurchases_OnItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if ((e.Item.ItemType == ListItemType.Item) || (e.Item.ItemType == ListItemType.AlternatingItem))
            {
                ProductPurchase dataItem = (ProductPurchase) e.Item.DataItem;
                Label label = e.Item.FindControl("lblTitle") as Label;
                label.Text = CommerceHelper.PurchasedProductTitle(dataItem);
            }
        }

        public Dictionary<string, string> PaymentProperties { get; set; }
    }
}

