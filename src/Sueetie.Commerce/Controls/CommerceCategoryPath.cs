namespace Sueetie.Commerce.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Web.UI.WebControls;
    using Sueetie.Commerce;
    using Sueetie.Core;

    public class CommerceCategoryPath : SueetieBaseControl
    {
        private StringBuilder _categoryPath;
        private const string AllCategoriesText = "All Categories";
        protected ObjectDataSource CategoryDataSource;
        protected Repeater CategoryPath;
        protected ObjectDataSource ParentCategoryDataSource;
        private const string vskeyCurrentCategoryId = "CurrentCategoryId";
        private const string vskeyCurrentCategoryName = "CurrentCategoryName";
        private const string vskeyCurrentParentCategoryId = "CurrentParentCategoryId";
        private const string vskeyFullCategoryPath = "FullCategoryPath";

        public event CategorySelectionChangedEventHandler CategorySelectionChanged;

        protected void AllCategoriesButton_Click(object sender, EventArgs e)
        {
            int categoryId = 0;
            this.SetCurrentCategory(categoryId);
            this.OnCategorySelectionChanged(new CategorySelectionChangedEventArgs(categoryId));
        }

        private void AppendCategoryToPath(string categoryName)
        {
            if (this._categoryPath == null)
            {
                this.InitCategoryPath();
            }
            this._categoryPath.Append(" > ");
            this._categoryPath.Append(categoryName);
        }

        protected void CategoryPath_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName.Equals("CategoryClick"))
            {
                int categoryId = Convert.ToInt32(e.CommandArgument);
                this.SetCurrentCategory(categoryId);
                this.OnCategorySelectionChanged(new CategorySelectionChangedEventArgs(categoryId));
            }
        }

        protected void CategoryPath_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            ProductCategory dataItem = e.Item.DataItem as ProductCategory;
            if (dataItem != null)
            {
                this.AppendCategoryToPath(dataItem.CategoryName);
            }
        }

        private string GetCategoryPath()
        {
            if (this._categoryPath == null)
            {
                this.InitCategoryPath();
            }
            return this._categoryPath.ToString();
        }

        private void InitCategoryPath()
        {
            this._categoryPath = new StringBuilder();
            this._categoryPath.Append("All Categories");
        }

        protected void OnCategorySelectionChanged(CategorySelectionChangedEventArgs e)
        {
            if (this.CategorySelectionChanged != null)
            {
                this.CategorySelectionChanged(this, e);
            }
        }

        protected void ParentCategoryDataSource_Selected(object sender, ObjectDataSourceStatusEventArgs e)
        {
            string name = null;
            int id = 0;
            List<ProductCategory> returnValue = e.ReturnValue as List<ProductCategory>;
            if ((returnValue != null) && (returnValue.Count > 0))
            {
                ProductCategory category = returnValue[returnValue.Count - 1];
                name = category.CategoryName;
                if (category.ParentCategoryID != 0)
                {
                    id = category.ParentCategoryID;
                }
                else
                {
                    id = 0;
                }
            }
            else
            {
                name = "All Categories";
                id = 0;
            }
            this.SetCurrentCategoryName(name);
            this.SetCurrentParentCategoryId(id);
        }

        private void SetCurrentCategory(int categoryId)
        {
            this.ViewState["CurrentCategoryId"] = categoryId;
            this.ParentCategoryDataSource.SelectParameters[0].DefaultValue = categoryId.ToString();
            this.InitCategoryPath();
            this.CategoryPath.DataBind();
            this.SetFullCategoryPath(this.GetCategoryPath());
        }

        private void SetCurrentCategoryName(string name)
        {
            this.ViewState["CurrentCategoryName"] = name;
        }

        private void SetCurrentParentCategoryId(int id)
        {
            this.ViewState["CurrentParentCategoryId"] = id;
        }

        private void SetFullCategoryPath(string path)
        {
            this.ViewState["FullCategoryPath"] = path;
        }

        public int CurrentCategoryId
        {
            get
            {
                if (this.ViewState["CurrentCategoryId"] == null)
                {
                    this.ViewState["CurrentCategoryId"] = 0;
                    return 0;
                }
                return (int)this.ViewState["CurrentCategoryId"];
            }
            set
            {
                this.SetCurrentCategory(value);
            }
        }

        public string CurrentCategoryName
        {
            get
            {
                if (this.ViewState["CurrentCategoryName"] != null)
                {
                    return (string)this.ViewState["CurrentCategoryName"];
                }
                return string.Empty;
            }
        }

        public int CurrentParentCategoryId
        {
            get
            {
                if (this.ViewState["CurrentParentCategoryId"] == null)
                {
                    this.ViewState["CurrentParentCategoryId"] = 0;
                    return 0;
                }
                return (int)this.ViewState["CurrentParentCategoryId"];
            }
        }

        public string FullCategoryPath
        {
            get
            {
                if (this.ViewState["FullCategoryPath"] is string)
                {
                    return (this.ViewState["FullCategoryPath"] as string);
                }
                return string.Empty;
            }
        }
    }
}

