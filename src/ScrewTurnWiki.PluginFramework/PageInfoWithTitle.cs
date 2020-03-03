using System;
using System.Collections.Generic;

namespace ScrewTurn.Wiki.PluginFramework
{
    /// <summary>
    /// 
    /// </summary>
    public class PageInfoWithTitle : PageInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="T:PageInfo" /> class.
        /// </summary>
        /// <param name="title">Title of the page.</param>
        /// <param name="fullName">The Full Name of the Page.</param>
        /// <param name="provider">The Pages Storage Provider that manages this Page.</param>
        /// <param name="creationDateTime">The Page creation Date/Time.</param>
        public PageInfoWithTitle(string title, string fullName, IPagesStorageProviderV30 provider, DateTime creationDateTime)
            : base(fullName, provider, creationDateTime)
        {
            Title = title;
        }

        /// <summary>
        /// Gets or sets the title of the page.
        /// </summary>
        public string Title { get; }

    }

    /// <summary>
    ///     Compares two <see cref="T:PageInfoWithTitle" /> objects, using the Title as parameter.
    /// </summary>
    /// <remarks>The comparison is <b>case insensitive</b>.</remarks>
    public class PageTitleComparer : IComparer<PageInfoWithTitle>
    {
        /// <summary>
        ///     Compares two <see cref="T:PageInfoWithTitle" /> objects, using the Title as parameter.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        /// <returns>The comparison result (-1, 0 or 1).</returns>
        public int Compare(PageInfoWithTitle x, PageInfoWithTitle y)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(x.Title, y.Title);
        }
    }
}