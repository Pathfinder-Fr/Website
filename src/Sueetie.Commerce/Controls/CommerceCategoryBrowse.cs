namespace Sueetie.Commerce.Controls
{
    using System;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Core;

    public class CommerceCategoryBrowse : SueetieBaseControl
    {
        private bool _autoNavigate;

        public event CategorySelectionChangedEventHandler CategorySelectionChanged;

        protected void NestedSubCategoryRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int categoryId = Convert.ToInt32(e.CommandArgument);
            this.OnCategorySelectionChanged(new CategorySelectionChangedEventArgs(categoryId));
        }

        protected void OnCategorySelectionChanged(CategorySelectionChangedEventArgs e)
        {
            if (this._autoNavigate)
            {
                base.Response.Redirect(SueetieUrls.Instance.MarketplaceCategory(e.CategoryId).Url);
            }
            else if (this.CategorySelectionChanged != null)
            {
                this.CategorySelectionChanged(this, e);
            }
        }

        protected void TopCategoryList_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int categoryId = Convert.ToInt32(e.CommandArgument);
            this.OnCategorySelectionChanged(new CategorySelectionChangedEventArgs(categoryId));
        }

        protected void TopCategoryList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            ObjectDataSource source = e.Item.FindControl("NestedCategoryDS") as ObjectDataSource;
            ProductCategory dataItem = e.Item.DataItem as ProductCategory;
            source.SelectParameters[0].DefaultValue = dataItem.CategoryID.ToString();
        }

        public bool AutoNavigate
        {
            get
            {
                return this._autoNavigate;
            }
            set
            {
                this._autoNavigate = value;
            }
        }
    }
}

