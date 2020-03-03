using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrewTurn.Wiki
{
    public static class LinqExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
        {
            if(@this == null)
            {
                return;
            }

            foreach(var item in @this)
            {
                action(item);
            }
        }
    }
}