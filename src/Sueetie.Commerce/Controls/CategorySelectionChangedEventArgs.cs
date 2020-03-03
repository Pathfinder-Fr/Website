namespace Sueetie.Commerce.Controls
{
    using System;

    public class CategorySelectionChangedEventArgs : EventArgs
    {
        private int _categoryId;

        public CategorySelectionChangedEventArgs(int categoryId)
        {
            this._categoryId = categoryId;
        }

        public int CategoryId
        {
            get
            {
                return this._categoryId;
            }
        }
    }
}

