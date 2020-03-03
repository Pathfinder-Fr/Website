using System;
using System.Collections.Generic;

namespace ScrewTurn.Wiki.PluginFramework
{
    /// <summary>
    ///     Contains basic information about a Page.
    /// </summary>
    public class PageInfo
    {
        /// <summary>
        ///     The Name of the Page.
        /// </summary>
        private string _name;

        /// <summary>
        ///     The namespace of the Page.
        /// </summary>
        private string _nspace;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:PageInfo" /> class.
        /// </summary>
        /// <param name="fullName">The Full Name of the Page.</param>
        /// <param name="provider">The Pages Storage Provider that manages this Page.</param>
        /// <param name="creationDateTime">The Page creation Date/Time.</param>
        public PageInfo(string fullName, IPagesStorageProviderV30 provider, DateTime creationDateTime)
        {
            NameTools.ExpandFullName(fullName, out _nspace, out _name);
            FullName = fullName;
            Provider = provider;
            CreationDateTime = creationDateTime;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:PageInfo" /> class.
        /// </summary>
        /// <param name="nspace">Namespace of the Page.</param>
        /// <param name="name">Name of the Page.</param>
        /// <param name="provider">The Pages Storage Provider that manages this Page.</param>
        /// <param name="creationDateTime">The Page creation Date/Time.</param>
        public PageInfo(string nspace,  string name, IPagesStorageProviderV30 provider, DateTime creationDateTime)
        {
            _nspace = nspace;
            _name = name;
            FullName = NameTools.GetFullName(nspace, name);
            Provider = provider;
            CreationDateTime = creationDateTime;
        }

        /// <summary>
        ///     Gets or sets the full name of the Page, such as 'Namespace.Page' or 'Page'.
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets the name of the page.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the namespace of the page.
        /// </summary>
        public string Namespace => _nspace;

        /// <summary>
        ///     Gets or sets the Pages Storage Provider.
        /// </summary>
        public IPagesStorageProviderV30 Provider { get; }

        /// <summary>
        ///     Gets or sets a value specifying whether the Page should NOT be cached by the engine.
        /// </summary>
        public bool NonCached { get; set; }

        /// <summary>
        ///     Gets or sets the creation Date/Time.
        /// </summary>
        public DateTime CreationDateTime { get; }

        /// <summary>
        ///     Gets the hashcode to use with this PageInfo.
        /// </summary>
        public override int GetHashCode() => FullName.GetHashCode();

        /// <summary>
        /// Rename the page with the provided <paramref name="newFullName"/>.
        /// </summary>
        /// <param name="newFullName">New full name for the page.</param>
        public void Rename(string newFullName)
        {
            FullName = newFullName;
            NameTools.ExpandFullName(FullName, out _nspace, out _name);
        }

        /// <summary>
        ///     Converts the current PageInfo to a string.
        /// </summary>
        /// <returns>The string.</returns>
        public override string ToString()
        {
            return string.Format("{0} [{1}]", FullName, Provider.Information.Name);
        }

        /// <summary>
        ///     Compare two objets together.
        /// </summary>
        public override bool Equals(object obj)
        {
            var other = obj as PageInfo;
            if (other != null)
            {
                return ToString().Equals(other.ToString(), StringComparison.OrdinalIgnoreCase);
            }

            return base.Equals(obj);
        }
    }

    /// <summary>
    ///     Compares two <see cref="T:PageInfo" /> objects, using the FullName as parameter.
    /// </summary>
    /// <remarks>The comparison is <b>case insensitive</b>.</remarks>
    public class PageNameComparer : IComparer<PageInfo>
    {
        /// <summary>
        ///     Compares two <see cref="T:PageInfo" /> objects, using the FullName as parameter.
        /// </summary>
        /// <param name="x">The first object.</param>
        /// <param name="y">The second object.</param>
        /// <returns>The comparison result (-1, 0 or 1).</returns>
        public int Compare(PageInfo x, PageInfo y)
        {
            return StringComparer.OrdinalIgnoreCase.Compare(x.FullName, y.FullName);
        }
    }
}