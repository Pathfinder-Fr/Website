namespace Sueetie.Commerce.Controls
{
    using System;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Core;

    public class CommerceCategoryDropDown : SueetieBaseControl
    {
        private string _allCategoriesText = "--- All Categories ----------------------------------------";
        private bool _allCategoriesVisible = true;
        private int _currentCategoryId;
        private bool _isDataBound;
        private int _savedSelectedIndex = -1;
        private string _selectOptionText = "Select...";
        private bool _selectOptionVisible;
        protected DropDownList CategoryList;
        public event CategorySelectionChangedEventHandler CategorySelectionChanged;

        protected void CategoryList_DataBinding(object sender, EventArgs e)
        {
            if (this.CategoryList.SelectedItem != null)
            {
                this._savedSelectedIndex = this.CategoryList.SelectedIndex;
            }
        }

        protected void CategoryList_DataBound(object sender, EventArgs e)
        {
            this._isDataBound = true;
            if (this._allCategoriesVisible)
            {
                this.CategoryList.Items.Insert(0, new ListItem(this._allCategoriesText, "0"));
            }
            if (this._selectOptionVisible && (this.CategoryList.Items.Count > 0))
            {
                this.CategoryList.Items.Insert(0, new ListItem(this._selectOptionText, "-1"));
            }
            if (this._savedSelectedIndex != -1)
            {
                this.CategoryList.SelectedIndex = this._savedSelectedIndex;
            }
        }

        protected void CategoryList_OnSelecting(object sender, ObjectDataSourceSelectingEventArgs e)
        {
            e.InputParameters["parentCategoryID"] = 4;
        }

        protected void CategoryList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int categoryId = Convert.ToInt32(this.CategoryList.SelectedValue);
            if ((categoryId != this._currentCategoryId) && (categoryId >= 0))
            {
                this.OnCategorySelectionChanged(new CategorySelectionChangedEventArgs(categoryId));
            }
            this._currentCategoryId = categoryId;
        }

        protected void OnCategorySelectionChanged(CategorySelectionChangedEventArgs e)
        {
            if (this.CategorySelectionChanged != null)
            {
                this.CategorySelectionChanged(this, e);
            }
        }

        public void Refresh()
        {
            CategoryCache.ClearCategoryCache();
            this.CategoryList.DataBind();
        }

        private void SetCurrentCategory(int categoryId)
        {
            if (!this._isDataBound)
            {
                this.CategoryList.DataBind();
            }
            string str = categoryId.ToString();
            ListItem item = this.CategoryList.Items.FindByValue(str);
            if (item != null)
            {
                this.CategoryList.SelectedItem.Selected = false;
                item.Selected = true;
            }
        }

        public string AllCategoriesOptionText
        {
            get
            {
                return this._allCategoriesText;
            }
            set
            {
                this._allCategoriesText = value;
            }
        }

        public bool AllCategoriesOptionVisible
        {
            get
            {
                return this._allCategoriesVisible;
            }
            set
            {
                this._allCategoriesVisible = value;
            }
        }

        public int CurrentCategoryId
        {
            get
            {
                return this.SelectedCategoryId;
            }
            set
            {
                this.SetCurrentCategory(value);
            }
        }

        public bool Enabled
        {
            get
            {
                return this.CategoryList.Enabled;
            }
            set
            {
                this.CategoryList.Enabled = value;
            }
        }

        public int SelectedCategoryId
        {
            get
            {
                if (this.CategoryList.SelectedIndex > 0)
                {
                    return Convert.ToInt32(this.CategoryList.SelectedValue);
                }
                return 0;
            }
            set
            {
                string str = value.ToString();
                ListItem item = this.CategoryList.Items.FindByValue(str);
                if (item != null)
                {
                    item.Selected = true;
                }
            }
        }

        public int SelectedIndex
        {
            get
            {
                return this.CategoryList.SelectedIndex;
            }
            set
            {
                if (this.CategoryList.Items.Count > 0)
                {
                    this.CategoryList.SelectedIndex = value;
                }
            }
        }

        public string SelectOptionText
        {
            get
            {
                return this._selectOptionText;
            }
            set
            {
                this._selectOptionText = value;
            }
        }

        public bool SelectOptionVisible
        {
            get
            {
                return this._selectOptionVisible;
            }
            set
            {
                this._selectOptionVisible = value;
            }
        }
    }
}

