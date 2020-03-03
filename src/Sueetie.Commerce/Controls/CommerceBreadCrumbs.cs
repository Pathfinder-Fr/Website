namespace Sueetie.Commerce.Controls
{
    using System;
    using System.ComponentModel;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Core;

    public class CommerceBreadCrumbs : SueetieBaseControl
    {
        public CommerceBreadCrumbs()
        {
            this.Prefix = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            this.Refresh();
        }

        [DefaultValue(true)]
        public bool Prefix { get; set; }

        public int CurrentCategoryID
        {
            get { return (int)(this.ViewState["CurrentCategoryID"] ?? -1); }
            set
            {
                if (this.CurrentCategoryID != value)
                {
                    this.ViewState["CurrentCategoryID"] = value;
                    this.Refresh();
                }
            }
        }

        private void Refresh()
        {
            this.Controls.Clear();

            Predicate<ProductCategory> match = null;
            var categoryId = -1;

            if (base.Request.QueryString["c"] != null)
            {
                categoryId = int.Parse(base.Request.QueryString["c"].ToString());
            }
            else if (this.CurrentCategoryID != -1)
            {
                categoryId = this.CurrentCategoryID;
            }

            var productCategoryList = ProductCategories.GetProductCategoryList();

            if (productCategoryList.Count != 0 && categoryId > 0)
            {
                if (match == null)
                {
                    match = u => u.CategoryID == categoryId;
                }

                var categories = productCategoryList.Find(match).Path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

                Predicate<ProductCategory> predicate = null;

                var first = true;
                foreach (string id in categories)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        if (!first || this.Prefix)
                        {
                            var child = new Literal { Text = SueetieLocalizer.GetString("breadcrumb_separator", "marketplace.xml") };
                            this.Controls.Add(child);
                        }

                        predicate = l => l.CategoryID == Convert.ToInt32(id);

                        var link = new HyperLink();
                        link.Text = productCategoryList.Find(predicate).CategoryName.Trim();
                        link.NavigateUrl = SueetieUrls.Instance.MarketplaceCategory(Convert.ToInt32(id)).Url;
                        this.Controls.Add(link);

                        first = false;
                    }
                }
            }
        }
    }
}