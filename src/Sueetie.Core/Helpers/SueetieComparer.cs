// -----------------------------------------------------------------------
// <copyright file="SueetieComparer.cs" company="Pathfinder-fr.org">
// Copyright (c) Pathfinder-fr.org. Tous droits reserves.
// </copyright>
// -----------------------------------------------------------------------

namespace Sueetie.Core
{
    using System;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public class SueetieComparer<T> : IComparer<T>
    {
        private SortDirection sortDirection;

        public SortDirection SortDirection
        {
            get { return this.sortDirection; }
            set { this.sortDirection = value; }
        }

        private string sortExpression;

        public SueetieComparer(string sortExpression, SortDirection sortDirection)
        {
            this.sortExpression = sortExpression;
            this.sortDirection = sortDirection;
        }


        public int Compare(T x, T y)
        {
            var propertyInfo = typeof (T).GetProperty(this.sortExpression);
            var obj1 = (IComparable)propertyInfo.GetValue(x, null);
            var obj2 = (IComparable)propertyInfo.GetValue(y, null);


            if (this.SortDirection == SortDirection.Ascending)
            {
                return obj1.CompareTo(obj2);
            }
            return obj2.CompareTo(obj1);
        }
    }
}