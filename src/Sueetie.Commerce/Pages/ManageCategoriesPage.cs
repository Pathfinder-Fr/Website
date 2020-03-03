namespace Sueetie.Commerce.Pages
{
    using System;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Commerce.Controls;

    public class ManageCategoriesPage : MarketplaceAdminPage
    {
        protected CommerceCategoryDropDown CategoryDropDown;
        protected CommerceCategoryPath CategoryPath;
        protected Label CurrentCategoryActionLabel;
        protected Label CurrentCategoryLabel;
        protected RadioButtonList MoveAction;
        protected Button MoveButton;
        protected Button RemoveCategoryButton;
        protected Button RenameCategoryButton;
        protected RequiredFieldValidator RequiredCategoryNameValidator;
        protected ListBox SubcategoriesList;
        protected TextBox txtCategoryDescription;
        protected TextBox txtCategoryName;
        protected TextBox txtEditCategoryDescription;
        protected TextBox txtEditCategoryName;

        public ManageCategoriesPage()
            : base("admin_marketplace_managecategories")
        {
        }

        protected void AddCategoryButton_Click(object sender, EventArgs e)
        {
            if (this.Page.IsValid)
            {
                ProductCategory productCategory = new ProductCategory
                {
                    ParentCategoryID = this.CategoryPath.CurrentCategoryId,
                    CategoryHtmlName = base.Server.HtmlEncode(this.txtCategoryName.Text),
                    CategoryDescription = base.Server.HtmlEncode(this.txtCategoryDescription.Text)
                };
                ProductCategories.AddProductCategory(productCategory);
                CategoryCache.ClearCategoryCache();
                this.CategoryDropDown.Refresh();
                this.txtCategoryName.Text = string.Empty;
                this.txtCategoryDescription.Text = string.Empty;
                this.SubcategoriesList.DataBind();
            }
        }

        protected void CategoryPath_CategorySelectionChanged(object sender, CategorySelectionChangedEventArgs e)
        {
            this.UpdateCategoryDisplay();
        }

        protected void MoveButton_Click(object sender, EventArgs e)
        {
            int currentCategoryId = this.CategoryPath.CurrentCategoryId;
            int selectedCategoryId = this.CategoryDropDown.SelectedCategoryId;
            if (this.MoveAction.SelectedValue.Equals("Category"))
            {
                ProductCategory productCategory = new ProductCategory
                {
                    CategoryID = currentCategoryId,
                    ParentCategoryID = selectedCategoryId
                };
                ProductCategories.MoveProductCategory(productCategory);
                CategoryCache.ClearCategoryCache();
                this.CategoryDropDown.Refresh();
                this.SetCurrentCategory(this.CategoryPath.CurrentParentCategoryId);
            }
            else if (this.MoveAction.SelectedValue.Equals("Ads"))
            {
                Products.MoveProductsToCategory(currentCategoryId, selectedCategoryId);
                this.CategoryDropDown.SelectedIndex = 0;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.Page.IsPostBack)
            {
                this.SetCurrentCategory(0);
                this.txtCategoryName.Text = string.Empty;
                this.txtCategoryDescription.Text = string.Empty;
                this.txtEditCategoryName.Text = string.Empty;
                this.txtEditCategoryDescription.Text = string.Empty;
            }
        }

        protected void RemoveCategoryButton_Click(object sender, EventArgs e)
        {
            if (ProductCategories.RemoveProductCategory(this.CategoryPath.CurrentCategoryId))
            {
                this.CategoryDropDown.Refresh();
                this.SetCurrentCategory(this.CategoryPath.CurrentParentCategoryId);
            }
            CategoryCache.ClearCategoryCache();
        }

        protected void RenameCategoryButton_Click(object sender, EventArgs e)
        {
            if (this.RequiredCategoryNameValidator.IsValid)
            {
                int currentCategoryId = this.CategoryPath.CurrentCategoryId;
                ProductCategory productCategory = new ProductCategory
                {
                    CategoryID = currentCategoryId,
                    CategoryHtmlName = base.Server.HtmlEncode(this.txtEditCategoryName.Text),
                    CategoryDescription = base.Server.HtmlEncode(this.txtEditCategoryDescription.Text)
                };
                ProductCategories.UpdateProductCategory(productCategory);
                CategoryCache.ClearCategoryCache();
                this.txtEditCategoryDescription.Text = string.Empty;
                this.txtEditCategoryName.Text = string.Empty;
                this.CategoryDropDown.Refresh();
                this.SetCurrentCategory(currentCategoryId);
            }
        }

        protected void SetCurrentCategory(int categoryId)
        {
            this.CategoryPath.CurrentCategoryId = categoryId;
            base.CurrentProductCategory = ProductCategories.GetProductCategory(categoryId);
            this.UpdateCategoryDisplay();
        }

        protected void SubcategoriesList_DataBound(object sender, EventArgs e)
        {
            if (this.SubcategoriesList.Items.Count > 0)
            {
                this.SubcategoriesList.Enabled = true;
            }
            else
            {
                this.SubcategoriesList.Items.Add(new ListItem("(no sub-categories)", "0"));
                this.SubcategoriesList.Enabled = false;
            }
        }

        protected void SubcategoriesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int categoryId = Convert.ToInt32(this.SubcategoriesList.SelectedValue);
            if (categoryId > 0)
            {
                this.SetCurrentCategory(categoryId);
            }
            else
            {
                base.CurrentProductCategory = null;
            }
        }

        protected void UpdateCategoryDisplay()
        {
            if (this.CategoryPath.CurrentCategoryId == 0)
            {
                this.CurrentCategoryLabel.Text = "[All Categories]";
                this.CurrentCategoryActionLabel.Text = "All Categories";
                this.txtEditCategoryName.Text = string.Empty;
                this.txtEditCategoryDescription.Text = string.Empty;
                this.txtEditCategoryName.Enabled = this.txtEditCategoryDescription.Enabled = this.RenameCategoryButton.Enabled = false;
                this.MoveButton.Enabled = false;
                this.MoveAction.Enabled = false;
                this.CategoryDropDown.Enabled = false;
                this.RemoveCategoryButton.Enabled = false;
            }
            else
            {
                base.CurrentProductCategory = ProductCategories.GetProductCategory(this.CategoryPath.CurrentCategoryId);
                this.CurrentCategoryLabel.Text = this.CurrentCategoryActionLabel.Text = this.txtEditCategoryName.Text = this.CategoryPath.CurrentCategoryName;
                this.txtEditCategoryDescription.Text = base.CurrentProductCategory.CategoryDescription;
                this.txtEditCategoryName.Enabled = this.txtEditCategoryDescription.Enabled = true;
                this.RenameCategoryButton.Enabled = true;
                this.MoveButton.Enabled = true;
                this.MoveAction.Enabled = true;
                this.CategoryDropDown.Enabled = true;
                this.RemoveCategoryButton.Enabled = true;
            }
        }
    }
}

