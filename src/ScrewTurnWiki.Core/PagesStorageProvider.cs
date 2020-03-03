using ScrewTurn.Wiki.PluginFramework;
using ScrewTurn.Wiki.SearchEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ScrewTurn.Wiki
{
    /// <summary>
    ///     Implements a Pages Storage Provider.
    /// </summary>
    public class PagesStorageProvider : ProviderBase, IPagesStorageProviderV30
    {
        private const string NamespacesFile = "Namespaces.cs";
        private const string PagesFile = "Pages.cs";
        private const string CategoriesFile = "Categories.cs";
        private const string NavigationPathsFile = "NavigationPaths.cs";
        private const string DraftsDirectory = "Drafts";
        private const string PagesDirectory = "Pages";
        private const string MessagesDirectory = "Messages";
        private const string SnippetsDirectory = "Snippets";
        private const string ContentTemplatesDirectory = "ContentTemplates";
        private const string IndexDocumentsFile = "IndexDocuments.cs";
        private const string IndexWordsFile = "IndexWords.cs";
        private const string IndexMappingsFile = "IndexMappings.cs";
        private CategoryInfo[] categoriesCache;
        private IHostV30 host;
        private IInMemoryIndex index;
        private IndexStorerBase indexStorer;
        // This cache is needed due to performance problems
        private NamespaceInfo[] namespacesCache;
        private PageInfo[] pagesCache;

        /// <summary>
        ///     Initializes the Provider.
        /// </summary>
        /// <param name="host">The Host of the Provider.</param>
        /// <param name="config">The Configuration data, if any.</param>
        /// <exception cref="ArgumentNullException">If <b>host</b> or <b>config</b> are <c>null</c>.</exception>
        /// <exception cref="InvalidConfigurationException">If <b>config</b> is not valid or is incorrect.</exception>
        public void Init(IHostV30 host, string config)
        {
            if (host == null) throw new ArgumentNullException("host");
            if (config == null) throw new ArgumentNullException("config");

            this.host = host;

            if (!LocalProvidersTools.CheckWritePermissions(GetDataDirectory(host)))
            {
                throw new InvalidConfigurationException("Cannot write into the public directory - check permissions");
            }

            if (!Directory.Exists(Path.Combine(GetDataDirectory(host), PagesDirectory)))
            {
                Directory.CreateDirectory(Path.Combine(GetDataDirectory(host), PagesDirectory));
            }
            if (!Directory.Exists(Path.Combine(GetDataDirectory(host), MessagesDirectory)))
            {
                Directory.CreateDirectory(Path.Combine(GetDataDirectory(host), MessagesDirectory));
            }
            if (!Directory.Exists(Path.Combine(GetDataDirectory(host), SnippetsDirectory)))
            {
                Directory.CreateDirectory(Path.Combine(GetDataDirectory(host), SnippetsDirectory));
            }
            if (!Directory.Exists(Path.Combine(GetDataDirectory(host), ContentTemplatesDirectory)))
            {
                Directory.CreateDirectory(Path.Combine(GetDataDirectory(host), ContentTemplatesDirectory));
            }
            if (!Directory.Exists(Path.Combine(GetDataDirectory(host), DraftsDirectory)))
            {
                Directory.CreateDirectory(Path.Combine(GetDataDirectory(host), DraftsDirectory));
            }

            var upgradeNeeded = false;

            if (!File.Exists(GetFullPath(NamespacesFile)))
            {
                File.Create(GetFullPath(NamespacesFile)).Close();
            }

            upgradeNeeded = VerifyIfPagesFileNeedsAnUpgrade();

            if (!File.Exists(GetFullPath(PagesFile)))
            {
                File.Create(GetFullPath(PagesFile)).Close();
            }
            else if (upgradeNeeded)
            {
                VerifyAndPerformUpgradeForPages();
            }

            if (!File.Exists(GetFullPath(CategoriesFile)))
            {
                File.Create(GetFullPath(CategoriesFile)).Close();
            }
            else if (upgradeNeeded)
            {
                VerifyAndPerformUpgradeForCategories();
            }

            if (!File.Exists(GetFullPath(NavigationPathsFile)))
            {
                File.Create(GetFullPath(NavigationPathsFile)).Close();
            }
            else if (upgradeNeeded)
            {
                VerifyAndPerformUpgradeForNavigationPaths();
            }

            // Prepare search index
            index = new StandardIndex();
            index.SetBuildDocumentDelegate(BuildDocumentHandler);
            indexStorer = new IndexStorer(GetFullPath(IndexDocumentsFile),
                GetFullPath(IndexWordsFile),
                GetFullPath(IndexMappingsFile),
                index);
            indexStorer.LoadIndex();

            if (indexStorer.DataCorrupted)
            {
                host.LogEntry("Search Engine Index is corrupted and needs to be rebuilt\r\n" +
                              indexStorer.ReasonForDataCorruption, LogEntryType.Warning, null, this);
            }
        }

        /// <summary>
        ///     Method invoked on shutdown.
        /// </summary>
        /// <remarks>This method might not be invoked in some cases.</remarks>
        public void Shutdown()
        {
            lock (this)
            {
                indexStorer.Dispose();
            }
        }

        /// <summary>
        ///     Gets the Information about the Provider.
        /// </summary>
        public ComponentInformation Information { get; } = new ComponentInformation("Local Pages Provider",
            "Threeplicate Srl", Settings.WikiVersion, "http://www.screwturn.eu", null);

        /// <summary>
        ///     Gets a brief summary of the configuration string format, in HTML. Returns <c>null</c> if no configuration is
        ///     needed.
        /// </summary>
        public string ConfigHelpHtml
        {
            get { return null; }
        }

        /// <summary>
        ///     Gets a value specifying whether the provider is read-only, i.e. it can only provide data and not store it.
        /// </summary>
        public bool ReadOnly
        {
            get { return false; }
        }

        /// <summary>
        ///     Gets a namespace.
        /// </summary>
        /// <param name="name">The name of the namespace.</param>
        /// <returns>The <see cref="T:NamespaceInfo" />, or <c>null</c> if no namespace is found.</returns>
        /// <exception cref="ArgumentNullException">If <b>name</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>name</b> is empty.</exception>
        public NamespaceInfo GetNamespace(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");

            lock (this)
            {
                return FindNamespace(name, GetNamespaces());
            }
        }

        /// <summary>
        ///     Gets all the sub-namespaces.
        /// </summary>
        /// <returns>The sub-namespaces, sorted by name.</returns>
        public IList<NamespaceInfo> GetNamespaces()
        {
            lock (this)
            {
                // Namespaces must be loaded from disk
                if (namespacesCache == null)
                {
                    var lines = File.ReadAllText(GetFullPath(NamespacesFile))
                        .Replace("\r", "")
                        .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    var result = new List<NamespaceInfo>(lines.Length);

                    var allPages = GetAllPages();
                    Array.Sort(allPages, new PageNameComparer());

                    // Line format
                    // Name[|Name.DefaultPage]

                    string[] fields;
                    char[] delimiters = { '|' };
                    string name = null, defaultPage = null;

                    foreach (var line in lines)
                    {
                        fields = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                        if (fields.Length >= 1)
                        {
                            name = fields[0];
                        }
                        else continue; // Skip entry
                        if (fields.Length == 2)
                        {
                            defaultPage = fields[1];
                        }

                        result.Add(new NamespaceInfo(name, this,
                            FindPage(name, NameTools.GetLocalName(defaultPage), allPages)));
                    }

                    result.Sort(new NamespaceComparer());

                    namespacesCache = result.ToArray();
                }

                return namespacesCache;
            }
        }

        /// <summary>
        ///     Adds a new namespace.
        /// </summary>
        /// <param name="name">The name of the namespace.</param>
        /// <returns>The correct <see cref="T:NamespaceInfo" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <b>name</b> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>name</b> is empty.</exception>
        public NamespaceInfo AddNamespace(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");

            lock (this)
            {
                if (NamespaceExists(name)) return null;

                // Append a line to the namespaces file
                File.AppendAllText(GetFullPath(NamespacesFile), name + "\r\n");

                // Create folder for page content files
                Directory.CreateDirectory(GetFullPathForPageContent(GetNamespacePartialPathForPageContent(name)));

                // Create folder for messages files
                Directory.CreateDirectory(GetFullPathForMessages(GetNamespacePartialPathForPageContent(name)));

                namespacesCache = null;
                return new NamespaceInfo(name, this, null);
            }
        }

        /// <summary>
        ///     Renames a namespace.
        /// </summary>
        /// <param name="nspace">The namespace to rename.</param>
        /// <param name="newName">The new name of the namespace.</param>
        /// <returns>The correct <see cref="T:NamespaceInfo" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <b>nspace</b> or <b>newName</b> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <b>newName</b> is empty.</exception>
        public NamespaceInfo RenameNamespace(NamespaceInfo nspace, string newName)
        {
            if (nspace == null) throw new ArgumentNullException("nspace");
            if (newName == null) throw new ArgumentNullException("newName");
            if (newName.Length == 0) throw new ArgumentException("New Name cannot be empty", "newName");

            lock (this)
            {
                if (NamespaceExists(newName)) return null;

                var oldName = nspace.Name;

                // Remove all pages and their messages from search engine index
                foreach (var page in GetPages(nspace))
                {
                    var content = GetContent(page);
                    UnindexPage(content);

                    var messages = GetMessages(page);
                    foreach (var msg in messages)
                    {
                        UnindexMessageTree(page, msg);
                    }
                }

                // Load namespace list and change name
                var allNamespaces = GetNamespaces();
                var comp = new NamespaceComparer();
                var result = FindNamespace(nspace.Name, allNamespaces);
                if (result == null) return null;
                result.Name = newName;
                // Change default page full name
                if (result.DefaultPage != null)
                {
                    result.DefaultPage =
                        new LocalPageInfo(
                            NameTools.GetFullName(newName, NameTools.GetLocalName(result.DefaultPage.FullName)),
                            this, result.DefaultPage.CreationDateTime,
                            GetNamespacePartialPathForPageContent(newName) +
                            Path.GetFileName(((LocalPageInfo)result.DefaultPage).File));
                }

                DumpNamespaces(allNamespaces);

                // Update Category list with new namespace name
                var allCategories = GetAllCategories();
                for (var k = 0; k < allCategories.Length; k++)
                {
                    var category = allCategories[k];
                    var catNamespace = NameTools.GetNamespace(category.FullName);
                    if (catNamespace != null && StringComparer.OrdinalIgnoreCase.Compare(catNamespace, oldName) == 0)
                    {
                        category.FullName = NameTools.GetFullName(newName, NameTools.GetLocalName(category.FullName));
                        for (var i = 0; i < category.Pages.Count; i++)
                        {
                            category.Pages[i] = NameTools.GetFullName(newName, NameTools.GetLocalName(category.Pages[i]));
                        }
                    }
                }
                DumpCategories(allCategories);

                // Rename namespace folder
                Directory.Move(GetFullPathForPageContent(GetNamespacePartialPathForPageContent(oldName)),
                    GetFullPathForPageContent(GetNamespacePartialPathForPageContent(newName)));

                // Rename drafts folder
                var oldDraftsFullPath = GetFullPathForPageDrafts(nspace.Name);
                if (Directory.Exists(oldDraftsFullPath))
                {
                    var newDraftsFullPath = GetFullPathForPageDrafts(newName);

                    Directory.Move(oldDraftsFullPath, newDraftsFullPath);
                }

                // Rename messages folder
                Directory.Move(GetFullPathForMessages(GetNamespacePartialPathForPageContent(oldName)),
                    GetFullPathForMessages(GetNamespacePartialPathForPageContent(newName)));

                // Update Page list with new namespace name and file
                var allPages = GetAllPages();
                foreach (var page in allPages)
                {
                    var pageNamespace = NameTools.GetNamespace(page.FullName);
                    if (pageNamespace != null && StringComparer.OrdinalIgnoreCase.Compare(pageNamespace, oldName) == 0)
                    {
                        var local = (LocalPageInfo)page;
                        local.Rename(NameTools.GetFullName(newName, NameTools.GetLocalName(local.FullName)));
                        local.File = GetNamespacePartialPathForPageContent(newName) + Path.GetFileName(local.File);
                    }
                }

                DumpPages(allPages);

                namespacesCache = null;
                pagesCache = null;
                categoriesCache = null;

                // Re-add all pages and their messages to the search engine index
                foreach (var page in GetPages(result))
                {
                    // result contains the new name
                    var content = GetContent(page);
                    IndexPage(content);

                    var messages = GetMessages(page);
                    foreach (var msg in messages)
                    {
                        IndexMessageTree(page, msg);
                    }
                }

                return result;
            }
        }

        /// <summary>
        ///     Sets the default page of a namespace.
        /// </summary>
        /// <param name="nspace">The namespace of which to set the default page.</param>
        /// <param name="page">The page to use as default page, or <c>null</c>.</param>
        /// <returns>The correct <see cref="T:NamespaceInfo" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <b>nspace</b> is <c>null</c>.</exception>
        public NamespaceInfo SetNamespaceDefaultPage(NamespaceInfo nspace, PageInfo page)
        {
            if (nspace == null) throw new ArgumentNullException("nspace");

            lock (this)
            {
                // Find requested namespace and page: if they don't exist, return null
                var allNamespaces = GetNamespaces();
                var targetNamespace = FindNamespace(nspace.Name, allNamespaces);
                if (targetNamespace == null) return null;

                LocalPageInfo localPage = null;

                if (page != null)
                {
                    localPage = LoadLocalPageInfo(page);
                    if (localPage == null) return null;
                }

                targetNamespace.DefaultPage = localPage;
                DumpNamespaces(allNamespaces);
                namespacesCache = null;

                return new NamespaceInfo(targetNamespace.Name, this, targetNamespace.DefaultPage);
            }
        }

        /// <summary>
        ///     Removes a namespace.
        /// </summary>
        /// <param name="nspace">The namespace to remove.</param>
        /// <returns><c>true</c> if the namespace is removed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <b>nspace</b> is <c>null</c>.</exception>
        public bool RemoveNamespace(NamespaceInfo nspace)
        {
            if (nspace == null) throw new ArgumentNullException("nspace");

            lock (this)
            {
                // Load all namespaces and remove the one to remove
                var allNamespaces = new List<NamespaceInfo>(GetNamespaces());
                var comp = new NamespaceComparer();
                var index = allNamespaces.FindIndex(delegate (NamespaceInfo x) { return comp.Compare(x, nspace) == 0; });

                if (index >= 0)
                {
                    // Delete all categories
                    foreach (var cat in GetCategories(nspace))
                    {
                        RemoveCategory(cat);
                    }

                    // Delete all pages in the namespace (RemovePage removes the page from the search engine index)
                    nspace.DefaultPage = null; // TODO: Remove this trick (needed in order to delete the default page)
                    foreach (var page in GetPages(nspace))
                    {
                        RemovePage(page);
                    }

                    // Update namespaces file
                    allNamespaces.RemoveAt(index);
                    DumpNamespaces(allNamespaces.ToArray());

                    // Remove namespace folder
                    Directory.Delete(GetFullPathForPageContent(GetNamespacePartialPathForPageContent(nspace.Name)), true);

                    // Remove drafts folder
                    var oldDraftsFullPath = GetFullPathForPageDrafts(nspace.Name);
                    if (Directory.Exists(oldDraftsFullPath))
                    {
                        Directory.Delete(oldDraftsFullPath, true);
                    }

                    // Remove messages folder
                    Directory.Delete(GetFullPathForMessages(GetNamespacePartialPathForPageContent(nspace.Name)), true);

                    namespacesCache = null;
                    pagesCache = null;
                    categoriesCache = null;

                    return true;
                }
                return false;
            }
        }

        /// <summary>
        ///     Moves a page from its namespace into another.
        /// </summary>
        /// <param name="page">The page to move.</param>
        /// <param name="destination">The destination namespace (null for the root).</param>
        /// <param name="copyCategories">
        ///     A value indicating whether to copy the page categories in the destination
        ///     namespace, if not already available.
        /// </param>
        /// <returns>The correct instance of <see cref="T:PageInfo" />.</returns>
        /// <exception cref="ArgumentNullException">If <b>page</b> is <c>null</c>.</exception>
        public PageInfo MovePage(PageInfo page, NamespaceInfo destination, bool copyCategories)
        {
            if (page == null) throw new ArgumentNullException("page");

            var destinationName = destination != null ? destination.Name : null;

            var currentNs = FindNamespace(NameTools.GetNamespace(page.FullName), GetNamespaces());
            var nsComp = new NamespaceComparer();
            if ((currentNs == null && destination == null) || nsComp.Compare(currentNs, destination) == 0) return null;

            if (
                PageExists(new PageInfo(NameTools.GetFullName(destinationName, NameTools.GetLocalName(page.FullName)),
                    this, page.CreationDateTime))) return null;
            if (!NamespaceExists(destinationName)) return null;

            if (currentNs != null && currentNs.DefaultPage != null)
            {
                // Cannot move the default page
                if (new PageNameComparer().Compare(currentNs.DefaultPage, page) == 0) return null;
            }

            // Store categories for copying them, if needed
            var pageCategories = GetCategoriesForPage(page);
            // Update categories names with new namespace (don't modify the same instance because it's actually the cache!)
            for (var i = 0; i < pageCategories.Count; i++)
            {
                var pages = pageCategories[i].Pages;
                pageCategories[i] =
                    new CategoryInfo(
                        NameTools.GetFullName(destinationName, NameTools.GetLocalName(pageCategories[i].FullName)), this);
                pageCategories[i].Pages = new string[pages.Count];
                for (var k = 0; k < pages.Count; k++)
                {
                    pageCategories[i].Pages[k] = NameTools.GetFullName(destinationName, NameTools.GetLocalName(pages[k]));
                }
            }

            // Delete category bindings
            RebindPage(page, new string[0]);

            // Change namespace and file
            var allPages = GetAllPages();
            var comp = new PageNameComparer();
            PageInfo newPage = null;
            foreach (var current in allPages)
            {
                if (comp.Compare(current, page) == 0)
                {
                    // Page found, update data

                    // Change namespace and file
                    var local = (LocalPageInfo)current;

                    // Update search engine index
                    var oldPageContent = GetContent(local);
                    UnindexPage(oldPageContent);
                    foreach (var msg in GetMessages(local))
                    {
                        UnindexMessageTree(local, msg);
                    }

                    // Move backups in new folder
                    MoveBackups(page, destination);

                    var newFile = GetNamespacePartialPathForPageContent(destinationName) + Path.GetFileName(local.File);

                    // Move data file
                    File.Move(GetFullPathForPageContent(local.File), GetFullPathForPageContent(newFile));

                    // Move messages file
                    var messagesFullPath = GetFullPathForMessages(local.File);
                    if (File.Exists(messagesFullPath))
                    {
                        File.Move(messagesFullPath, GetFullPathForMessages(newFile));
                    }

                    // Move draft file
                    var draftFullPath = GetFullPathForPageDrafts(local.File);
                    if (File.Exists(draftFullPath))
                    {
                        var newDraftFullPath = GetFullPathForPageDrafts(newFile);
                        if (!Directory.Exists(Path.GetDirectoryName(newDraftFullPath)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(newDraftFullPath));
                        }
                        File.Move(draftFullPath, newDraftFullPath);
                    }

                    //local.Namespace = destinationName;
                    local.Rename(NameTools.GetFullName(destinationName, NameTools.GetLocalName(local.FullName)));
                    local.File = newFile;
                    newPage = local;

                    DumpPages(allPages);

                    // Update search engine index
                    IndexPage(new PageContent(newPage, oldPageContent.Title, oldPageContent.User,
                        oldPageContent.LastModified,
                        oldPageContent.Comment, oldPageContent.Content, oldPageContent.Keywords,
                        oldPageContent.Description));
                    foreach (var msg in GetMessages(local))
                    {
                        IndexMessageTree(newPage, msg);
                    }

                    break;
                }
            }

            // Rebind page, if needed
            if (copyCategories)
            {
                // Foreach previously bound category, verify that is present in the destination namespace, if not then create it
                var newCategories = new List<string>(pageCategories.Count);
                foreach (var oldCategory in pageCategories)
                {
                    if (!CategoryExists(new CategoryInfo(oldCategory.FullName, this)))
                    {
                        AddCategory(destination != null ? destination.Name : null,
                            NameTools.GetLocalName(oldCategory.FullName));
                    }
                    newCategories.Add(oldCategory.FullName);
                }
                RebindPage(newPage, newCategories.ToArray());
            }

            namespacesCache = null;
            pagesCache = null;
            categoriesCache = null;
            return newPage;
        }

        /// <summary>
        ///     Gets a category.
        /// </summary>
        /// <param name="fullName">The full name of the category.</param>
        /// <returns>The <see cref="T:CategoryInfo" />, or <c>null</c> if no category is found.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="fullName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="fullName" /> is empty.</exception>
        public CategoryInfo GetCategory(string fullName)
        {
            if (fullName == null) throw new ArgumentNullException("fullName");
            if (fullName.Length == 0) throw new ArgumentException("Full Name cannot be empty", "fullName");

            lock (this)
            {
                var categories = GetAllCategories();

                var comp = StringComparer.OrdinalIgnoreCase;

                foreach (var cat in categories)
                {
                    if (comp.Compare(cat.FullName, fullName) == 0) return cat;
                }

                return null;
            }
        }

        /// <summary>
        ///     Gets all the Categories in a namespace.
        /// </summary>
        /// <param name="nspace">The namespace.</param>
        /// <returns>All the Categories in the namespace, sorted by name.</returns>
        public IList<CategoryInfo> GetCategories(NamespaceInfo nspace)
        {
            lock (this)
            {
                var allCategories = GetAllCategories(); // Sorted

                // Preallocate assuming that there are 4 namespaces and that they are distributed evenly among them:
                // categories might be a few dozens at most, so preallocating a smaller number of items is not a problem
                var selectedCategories = new List<CategoryInfo>(allCategories.Length / 4);

                // Select categories that have the same namespace as the requested one,
                // either null-null or same name
                foreach (var cat in allCategories)
                {
                    var catNamespace = NameTools.GetNamespace(cat.FullName);
                    if (nspace == null && catNamespace == null) selectedCategories.Add(cat);
                    else if (nspace != null && catNamespace != null &&
                             StringComparer.OrdinalIgnoreCase.Compare(nspace.Name, catNamespace) == 0)
                        selectedCategories.Add(cat);
                }

                return selectedCategories;
            }
        }

        /// <summary>
        ///     Gets all the categories of a page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>The categories, sorted by name.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public IList<CategoryInfo> GetCategoriesForPage(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException("page");

            var pageNamespace = NameTools.GetNamespace(page.FullName);
            var categories = GetCategories(FindNamespace(pageNamespace, GetNamespaces())); // Sorted

            var result = new List<CategoryInfo>(10);

            var comp = new PageNameComparer();
            foreach (var cat in categories)
            {
                foreach (var p in cat.Pages)
                {
                    if (comp.Compare(page, new PageInfo(p, this, DateTime.Now)) == 0)
                    {
                        result.Add(cat);
                        break;
                    }
                }
            }

            return result.ToArray();
        }

        /// <summary>
        ///     Adds a new Category.
        /// </summary>
        /// <param name="nspace">The target namespace (<c>null</c> for the root).</param>
        /// <param name="name">The Category name.</param>
        /// <returns>The correct CategoryInfo object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name" /> is empty.</exception>
        public CategoryInfo AddCategory(string nspace, string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");

            lock (this)
            {
                var result = new CategoryInfo(NameTools.GetFullName(nspace, name), this);

                if (CategoryExists(result)) return null;

                // Structure
                // Namespace.Category|Page1|Page2|...
                File.AppendAllText(GetFullPath(CategoriesFile), "\r\n" + result.FullName);
                result.Pages = new string[0];
                categoriesCache = null;
                return result;
            }
        }

        /// <summary>
        ///     Renames a Category.
        /// </summary>
        /// <param name="category">The Category to rename.</param>
        /// <param name="newName">The new Name.</param>
        /// <returns>The correct CategoryInfo object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="category" /> or <paramref name="newName" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="newName" /> is empty.</exception>
        public CategoryInfo RenameCategory(CategoryInfo category, string newName)
        {
            if (category == null) throw new ArgumentNullException("category");
            if (newName == null) throw new ArgumentNullException("newName");
            if (newName.Length == 0) throw new ArgumentException("New Name cannot be empty", "newName");

            lock (this)
            {
                var result = new CategoryInfo(
                    NameTools.GetFullName(NameTools.GetNamespace(category.FullName), newName), this);
                if (CategoryExists(result)) return null;

                var cats = GetAllCategories();

                var comp = new CategoryNameComparer();
                for (var i = 0; i < cats.Length; i++)
                {
                    if (comp.Compare(cats[i], category) == 0)
                    {
                        result.Pages = cats[i].Pages;
                        cats[i] = result;
                        DumpCategories(cats);
                        categoriesCache = null;
                        return result;
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     Removes a Category.
        /// </summary>
        /// <param name="category">The Category to remove.</param>
        /// <returns>True if the Category has been removed successfully.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="category" /> is <c>null</c>.</exception>
        public bool RemoveCategory(CategoryInfo category)
        {
            if (category == null) throw new ArgumentNullException("category");

            lock (this)
            {
                var cats = GetAllCategories();
                var comp = new CategoryNameComparer();
                for (var i = 0; i < cats.Length; i++)
                {
                    if (comp.Compare(cats[i], category) == 0)
                    {
                        var tmp = new List<CategoryInfo>(cats);
                        tmp.Remove(tmp[i]);
                        DumpCategories(tmp.ToArray());
                        categoriesCache = null;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///     Merges two Categories.
        /// </summary>
        /// <param name="source">The source Category.</param>
        /// <param name="destination">The destination Category.</param>
        /// <returns>True if the Categories have been merged successfully.</returns>
        /// <remarks>
        ///     The destination Category remains, while the source Category is deleted, and all its Pages re-bound in the
        ///     destination Category.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="source" /> or <paramref name="destination" /> are
        ///     <c>null</c>.
        /// </exception>
        public CategoryInfo MergeCategories(CategoryInfo source, CategoryInfo destination)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (destination == null) throw new ArgumentNullException("destination");

            lock (this)
            {
                var allNamespaces = GetNamespaces();
                var sourceNs = FindNamespace(NameTools.GetNamespace(source.FullName), allNamespaces);
                var destinationNs = FindNamespace(NameTools.GetNamespace(destination.FullName), allNamespaces);
                var nsComp = new NamespaceComparer();
                if (!(sourceNs == null && destinationNs == null) && nsComp.Compare(sourceNs, destinationNs) != 0)
                {
                    // Different namespaces
                    return null;
                }

                var cats = GetAllCategories();
                int idxSource = -1, idxDest = -1;
                var comp = new CategoryNameComparer();
                for (var i = 0; i < cats.Length; i++)
                {
                    if (comp.Compare(cats[i], source) == 0) idxSource = i;
                    if (comp.Compare(cats[i], destination) == 0) idxDest = i;
                    if (idxSource != -1 && idxDest != -1) break;
                }
                if (idxSource == -1 || idxDest == -1) return null;

                var tmp = new List<CategoryInfo>(cats);
                var newPages = new List<string>(cats[idxDest].Pages);
                for (var i = 0; i < cats[idxSource].Pages.Count; i++)
                {
                    var found = false;
                    for (var k = 0; k < newPages.Count; k++)
                    {
                        if (StringComparer.OrdinalIgnoreCase.Compare(newPages[k], cats[idxSource].Pages[i]) == 0)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        newPages.Add(cats[idxSource].Pages[i]);
                    }
                }
                tmp[idxDest].Pages = newPages.ToArray();
                tmp.Remove(tmp[idxSource]);
                DumpCategories(tmp.ToArray());
                var newCat = new CategoryInfo(destination.FullName, this);
                newCat.Pages = newPages.ToArray();
                categoriesCache = null;
                return newCat;
            }
        }

        /// <summary>
        ///     Performs a search in the index.
        /// </summary>
        /// <param name="parameters">The search parameters.</param>
        /// <returns>The results.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="parameters" /> is <c>null</c>.</exception>
        public SearchResultCollection PerformSearch(SearchParameters parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");

            lock (this)
            {
                return index.Search(parameters);
            }
        }

        /// <summary>
        ///     Rebuilds the search index.
        /// </summary>
        public void RebuildIndex()
        {
            lock (this)
            {
                index.Clear(null);

                foreach (var page in GetAllPages())
                {
                    IndexPage(GetContent(page));

                    foreach (var msg in GetMessages(page))
                    {
                        IndexMessageTree(page, msg);
                    }
                }
            }
        }

        /// <summary>
        ///     Gets some statistics about the search engine index.
        /// </summary>
        /// <param name="documentCount">The total number of documents.</param>
        /// <param name="wordCount">The total number of unique words.</param>
        /// <param name="occurrenceCount">The total number of word-document occurrences.</param>
        /// <param name="size">The approximated size, in bytes, of the search engine index.</param>
        public void GetIndexStats(out int documentCount, out int wordCount, out int occurrenceCount, out long size)
        {
            lock (this)
            {
                documentCount = index.TotalDocuments;
                wordCount = index.TotalWords;
                occurrenceCount = index.TotalOccurrences;
                size = indexStorer.Size;
            }
        }

        /// <summary>
        ///     Gets a value indicating whether the search engine index is corrupted and needs to be rebuilt.
        /// </summary>
        public bool IsIndexCorrupted
        {
            get
            {
                lock (this)
                {
                    return indexStorer.DataCorrupted;
                }
            }
        }

        /// <summary>
        ///     Gets a page.
        /// </summary>
        /// <param name="fullName">The full name of the page.</param>
        /// <returns>The <see cref="T:PageInfo" />, or <c>null</c> if no page is found.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="fullName" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="fullName" /> is empty.</exception>
        public PageInfo GetPage(string fullName)
        {
            if (fullName == null) throw new ArgumentNullException("fullName");
            if (fullName.Length == 0) throw new ArgumentException("Full Name cannot be empty", "fullName");

            lock (this)
            {
                string nspace, name;
                NameTools.ExpandFullName(fullName, out nspace, out name);
                return FindPage(nspace, name, GetAllPages());
            }
        }

        /// <summary>
        ///     Gets all the Pages in a namespace.
        /// </summary>
        /// <param name="nspace">The namespace (<c>null</c> for the root).</param>
        /// <returns>All the Pages in the namespace. The array is not sorted.</returns>
        public IList<PageInfo> GetPages(NamespaceInfo nspace)
        {
            lock (this)
            {
                var allPages = GetAllPages();

                // Preallocate assuming that there are 2 namespaces and that they are evenly distributed across them:
                // pages can be as much as many thousands, so preallocating a smaller number can cause a performance loss
                var selectedPages = new List<PageInfo>(allPages.Length / 2);

                // Select pages that have the same namespace as the requested one,
                // either null-null or same name
                foreach (var page in allPages)
                {
                    var pageNamespace = NameTools.GetNamespace(page.FullName);
                    if (nspace == null && pageNamespace == null) selectedPages.Add(page);
                    if (nspace != null && pageNamespace != null &&
                        StringComparer.OrdinalIgnoreCase.Compare(nspace.Name, pageNamespace) == 0)
                        selectedPages.Add(page);
                }

                return selectedPages;
            }
        }

        /// <summary />
        public IList<PageInfo> FindPages(FindPagesFilter filter)
        {
            return filter.ApplyOn(GetAllPages()).ToArray();
        }

        /// <summary>
        ///     Gets all the pages in a namespace that are bound to zero categories.
        /// </summary>
        /// <param name="nspace">The namespace (<c>null</c> for the root).</param>
        /// <returns>The pages, sorted by name.</returns>
        public IList<PageInfo> GetUncategorizedPages(NamespaceInfo nspace)
        {
            lock (this)
            {
                var pages = GetPages(nspace);
                var categories = GetCategories(nspace);

                var result = new List<PageInfo>(pages.Count);

                foreach (var p in pages)
                {
                    var found = false;
                    foreach (var c in categories)
                    {
                        foreach (var name in c.Pages)
                        {
                            if (StringComparer.OrdinalIgnoreCase.Compare(name, p.FullName) == 0)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                    if (!found) result.Add(p);
                }

                return result.ToArray();
            }
        }

        /// <summary>
        ///     Adds a Page.
        /// </summary>
        /// <param name="nspace">The target namespace (<c>null</c> for the root).</param>
        /// <param name="name">The Page Name.</param>
        /// <param name="creationDateTime">The creation Date/Time.</param>
        /// <returns>The correct PageInfo object or null.</returns>
        /// <remarks>This method should <b>not</b> create the content of the Page.</remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name" /> is empty.</exception>
        public PageInfo AddPage(string nspace, string name, DateTime creationDateTime)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");

            lock (this)
            {
                if (!NamespaceExists(nspace)) return null;
                if (PageExists(new PageInfo(NameTools.GetFullName(nspace, name), this, DateTime.Now))) return null;

                var result = new LocalPageInfo(NameTools.GetFullName(nspace, name), this, creationDateTime,
                    GetNamespacePartialPathForPageContent(nspace) + name + ".cs");

                BackupPagesFile();

                // Structure
                // Namespace.Page|File|CreationDateTime
                File.AppendAllText(GetFullPath(PagesFile),
                    result.FullName + "|" + result.File + "|" +
                    creationDateTime.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss") + "\r\n");
                //File.Create(GetFullPathForPageContent(result.File)).Close(); // Empty content file might cause problems with backups
                File.WriteAllText(GetFullPathForPageContent(result.File),
                    "--\r\n--|1900/01/01 0:00:00|\r\n##PAGE##\r\n--");
                pagesCache = null;

                return result;
            }
        }

        public string GetTitle(PageInfo page) => GetContent(page)?.Title;

        /// <summary>
        ///     Gets the Content of a Page.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <returns>
        ///     The Page Content object, <c>null</c> if the page does not exist or <paramref name="page" /> is <c>null</c>,
        ///     or an empty instance if the content could not be retrieved (<seealso cref="PageContent.GetEmpty" />).
        /// </returns>
        public PageContent GetContent(PageInfo page)
        {
            if (page == null) return null;

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return null;

                string text = null;
                try
                {
                    text = File.ReadAllText(GetFullPathForPageContent(local.File));
                }
                catch (Exception ex)
                {
                    host.LogEntry(
                        "Could not load content file (" + local.File + ") for page " + local.FullName +
                        " - returning empty (" + ex.Message + ")",
                        LogEntryType.Error, null, this);
                    return PageContent.GetEmpty(page);
                }

                return ExtractContent(text, page);
            }
        }

        /// <summary>
        ///     Gets the Backup/Revision numbers of a Page.
        /// </summary>
        /// <param name="page">The Page to get the Backups of.</param>
        /// <returns>The list of Backup/Revision numbers.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public int[] GetBackups(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException("page");

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return null;

                // Files in <Public>\Pages\[Namespace\]FileName.NNNNN.cs
                var dir = GetFullPath(PagesDirectory);
                var nsDir = GetNamespacePartialPathForPageContent(NameTools.GetNamespace(page.FullName));
                if (nsDir.Length > 0) dir = Path.Combine(dir, nsDir);

                var files = Directory.GetFiles(dir,
                    Path.GetFileNameWithoutExtension(local.File) + ".*" + Path.GetExtension(local.File));

                var result = new List<int>(30);
                for (var i = 0; i < files.Length; i++)
                {
                    var num =
                        Path.GetFileNameWithoutExtension(files[i])
                            .Substring(NameTools.GetLocalName(page.FullName).Length + 1);
                    var bak = -1;
                    if (int.TryParse(num, out bak)) result.Add(bak);
                }
                return result.ToArray();
            }
        }

        /// <summary>
        ///     Gets the Content of a Backup.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <param name="revision">The revision.</param>
        /// <returns>The content.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="revision" /> is less than zero.</exception>
        public PageContent GetBackupContent(PageInfo page, int revision)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (revision < 0) throw new ArgumentOutOfRangeException("revision", "Invalid Revision");

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return null;

                var filename = Path.GetFileNameWithoutExtension(local.File) + "." + Tools.GetVersionString(revision) +
                               Path.GetExtension(local.File);
                var path =
                    GetFullPathForPageContent(
                        GetNamespacePartialPathForPageContent(NameTools.GetNamespace(page.FullName)) + filename);

                if (!File.Exists(path)) return null;
                return ExtractContent(File.ReadAllText(path), page);
            }
        }

        /// <summary>
        ///     Forces to overwrite or create a Backup.
        /// </summary>
        /// <param name="content">The Backup content.</param>
        /// <param name="revision">The revision.</param>
        /// <returns>True if the Backup has been created successfully.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="content" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="revision" /> is less than zero.</exception>
        public bool SetBackupContent(PageContent content, int revision)
        {
            if (content == null) throw new ArgumentNullException("content");
            if (revision < 0) throw new ArgumentOutOfRangeException("Invalid Revision", "revision");

            lock (this)
            {
                var local = LoadLocalPageInfo(content.PageInfo);
                if (local == null) return false;

                var sb = new StringBuilder();
                sb.Append(content.Title);
                sb.Append("\r\n");
                sb.Append(content.User);
                sb.Append("|");
                sb.Append(content.LastModified.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"));
                if (!string.IsNullOrEmpty(content.Comment))
                {
                    sb.Append("|");
                    sb.Append(Tools.EscapeString(content.Comment));
                }
                sb.Append("\r\n##PAGE##\r\n");
                sb.Append(content.Content);

                var filename = Path.GetFileNameWithoutExtension(local.File) + "." + Tools.GetVersionString(revision) +
                               Path.GetExtension(local.File);
                File.WriteAllText(
                    GetFullPathForPageContent(
                        GetNamespacePartialPathForPageContent(NameTools.GetNamespace(content.PageInfo.FullName)) +
                        filename), sb.ToString());
            }
            return true;
        }

        /// <summary>
        ///     Renames a Page.
        /// </summary>
        /// <param name="page">The Page to rename.</param>
        /// <param name="newName">The new Name.</param>
        /// <returns>True if the Page has been renamed successfully.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> or <paramref name="newName" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="newName" /> is empty.</exception>
        public PageInfo RenamePage(PageInfo page, string newName)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (newName == null) throw new ArgumentNullException("newName");
            if (newName.Length == 0) throw new ArgumentException("New Name cannot be empty", "newName");

            lock (this)
            {
                if (
                    PageExists(new PageInfo(NameTools.GetFullName(NameTools.GetNamespace(page.FullName), newName), this,
                        DateTime.Now))) return null;

                var currentNs = FindNamespace(NameTools.GetNamespace(page.FullName), GetNamespaces());
                if (currentNs != null && currentNs.DefaultPage != null)
                {
                    // Cannot rename the default page
                    if (new PageNameComparer().Compare(currentNs.DefaultPage, page) == 0) return null;
                }

                var pgs = GetAllPages();
                var comp = new PageNameComparer();

                // Store page's categories for rebinding with new page
                var tmp = GetCategoriesForPage(page);
                var cats = new string[tmp.Count];
                for (var i = 0; i < tmp.Count; i++)
                {
                    cats[i] = tmp[i].FullName;
                }
                // Remove all bindings for old page
                RebindPage(page, new string[0]);

                // Find page and rename files
                for (var i = 0; i < pgs.Length; i++)
                {
                    if (comp.Compare(pgs[i], page) == 0)
                    {
                        var local = pgs[i] as LocalPageInfo;

                        var oldContent = GetContent(page);

                        var messages = GetMessages(local);

                        // Update search engine index
                        UnindexPage(oldContent);
                        foreach (var msg in messages)
                        {
                            UnindexMessageTree(local, msg);
                        }

                        var oldFullName = local.FullName;
                        local.Rename(NameTools.GetFullName(local.Namespace, newName));

                        var newFile = GetNamespacePartialPathForPageContent(NameTools.GetNamespace(local.FullName)) +
                                      newName +
                                      Path.GetExtension(local.File);

                        // Rename content file
                        var oldLocalName = local.File;
                        var oldFullPath = GetFullPathForPageContent(local.File);
                        var newFullPath = GetFullPathForPageContent(newFile);
                        File.Move(oldFullPath, newFullPath);

                        // Rename messages file
                        if (File.Exists(GetFullPathForMessages(oldLocalName)))
                        {
                            File.Move(GetFullPathForMessages(oldLocalName), GetFullPathForMessages(newFile));
                        }

                        // Rename draft file, if any
                        var oldDraftFullPath = GetDraftFullPath(local);
                        if (File.Exists(oldDraftFullPath))
                        {
                            var newDraftFullPath =
                                GetDraftFullPath(new LocalPageInfo(local.FullName, this, local.CreationDateTime, newFile));

                            File.Move(oldDraftFullPath, newDraftFullPath);
                        }

                        // Rename all backups, store new page list on disk
                        // and rebind new page with old categories
                        RenameBackups(new LocalPageInfo(oldFullName, this, local.CreationDateTime, oldLocalName),
                            newName);

                        // Set new filename (local references an element in the pgs array)
                        local.File = newFile;

                        DumpPages(pgs);
                        // Clear internal cache
                        categoriesCache = null;
                        pagesCache = null;
                        // Re-bind page with previously saved categories
                        RebindPage(local, cats);

                        // Update search engine index
                        IndexPage(new PageContent(local, oldContent.Title, oldContent.User, oldContent.LastModified,
                            oldContent.Comment,
                            oldContent.Content, oldContent.Keywords, oldContent.Description));
                        foreach (var msg in messages)
                        {
                            IndexMessageTree(local, msg);
                        }

                        return local;
                    }
                }

                // Page not found, return null
                return null;
            }
        }

        /// <summary>
        ///     Modifies the Content of a Page.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <param name="title">The Title of the Page.</param>
        /// <param name="username">The Username.</param>
        /// <param name="dateTime">The Date/Time.</param>
        /// <param name="comment">The Comment of the editor, about this revision.</param>
        /// <param name="content">The Page Content.</param>
        /// <param name="keywords">The keywords, usually used for SEO.</param>
        /// <param name="description">The description, usually used for SEO.</param>
        /// <param name="saveMode">The save mode for this modification.</param>
        /// <returns><c>true</c> if the Page has been modified successfully, <c>false</c> otherwise.</returns>
        /// <remarks>If <b>saveMode</b> equals <b>Draft</b> and a draft already exists, it is overwritten.</remarks>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="page" />, <paramref name="title" />
        ///     <paramref name="username" /> or <paramref name="content" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="title" /> or <paramref name="username" /> are empty.</exception>
        public bool ModifyPage(PageInfo page, string title, string username, DateTime dateTime, string comment,
            string content,
            IList<string> keywords, string description, SaveMode saveMode)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (title == null) throw new ArgumentNullException("title");
            if (title.Length == 0) throw new ArgumentException("Title cannot be empty", "title");
            if (username == null) throw new ArgumentNullException("username");
            if (username.Length == 0) throw new ArgumentException("Username cannot be empty", "username");
            if (content == null) throw new ArgumentNullException("content"); // content can be empty

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return false;

                if (saveMode == SaveMode.Backup)
                {
                    Backup(local);
                }

                var sb = new StringBuilder();
                sb.Append(title);
                sb.Append("\r\n");
                sb.Append(username);
                sb.Append("|");
                sb.Append(dateTime.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"));
                if (!string.IsNullOrEmpty(comment))
                {
                    sb.Append("|");
                    sb.Append(Tools.EscapeString(comment));
                }
                if ((keywords != null && keywords.Count > 0) || !string.IsNullOrEmpty(description))
                {
                    sb.Append("|(((");
                    if (keywords != null)
                    {
                        for (var i = 0; i < keywords.Count; i++)
                        {
                            sb.Append(Tools.EscapeString(keywords[i]));
                            if (i != keywords.Count - 1) sb.Append(",");
                        }
                    }
                    sb.Append(")))(((");
                    sb.Append(Tools.EscapeString(description));
                    sb.Append(")))");
                }
                sb.Append("\r\n##PAGE##\r\n");
                sb.Append(content);

                if (saveMode == SaveMode.Draft)
                {
                    // Create the namespace directory for the draft, if needed
                    // Drafts\NS\Page.cs
                    var targetFileFullPath = GetDraftFullPath(local);
                    if (!Directory.Exists(Path.GetDirectoryName(targetFileFullPath)))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(targetFileFullPath));
                    }
                    File.WriteAllText(targetFileFullPath, sb.ToString());
                }
                else
                {
                    File.WriteAllText(GetFullPathForPageContent(local.File), sb.ToString());

                    // Update search engine index
                    var pageContent = new PageContent(page, title, username, dateTime, comment, content, keywords,
                        description);
                    IndexPage(pageContent);
                }
            }
            return true;
        }

        /// <summary>
        ///     Gets the content of a draft of a Page.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <returns>The draft, or <c>null</c> if no draft exists.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public PageContent GetDraft(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException("page");

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return null;

                var targetFileFullPath = GetDraftFullPath(local);

                if (!File.Exists(targetFileFullPath)) return null;
                return ExtractContent(File.ReadAllText(targetFileFullPath), local);
            }
        }

        /// <summary>
        ///     Deletes a draft of a Page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns><c>true</c> if the draft is deleted, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public bool DeleteDraft(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException("page");

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return false;

                var targetFileFullPath = GetDraftFullPath(local);

                if (!File.Exists(targetFileFullPath)) return false;
                File.Delete(targetFileFullPath);
                // Delete directory if empty
                if (Directory.GetFiles(Path.GetDirectoryName(targetFileFullPath)).Length == 0)
                {
                    Directory.Delete(Path.GetDirectoryName(targetFileFullPath));
                }
                return true;
            }
        }

        /// <summary>
        ///     Performs the rollback of a Page to a specified revision.
        /// </summary>
        /// <param name="page">The Page to rollback.</param>
        /// <param name="revision">The Revision to rollback the Page to.</param>
        /// <returns><c>true</c> if the rollback succeeded, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="revision" /> is less than zero.</exception>
        public bool RollbackPage(PageInfo page, int revision)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (revision < 0) throw new ArgumentOutOfRangeException("Invalid Revision", "revision");

            lock (this)
            {
                if (!PageExists(page)) return false;

                // Operations:
                // - Load specific revision's content
                // - Modify page with loaded content, performing backup

                var revisionContent = GetBackupContent(page, revision);
                if (revisionContent == null) return false;

                var done = ModifyPage(page, revisionContent.Title, revisionContent.User, revisionContent.LastModified,
                    revisionContent.Comment, revisionContent.Content, revisionContent.Keywords,
                    revisionContent.Description,
                    SaveMode.Backup);

                return done;
            }
        }

        /// <summary>
        ///     Deletes the Backups of a Page, up to a specified revision.
        /// </summary>
        /// <param name="page">The Page to delete the backups of.</param>
        /// <param name="revision">The newest revision to delete (newer revision are kept) or -1 to delete all the Backups.</param>
        /// <returns><c>true</c> if the deletion succeeded, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="revision" /> is less than -1.</exception>
        public bool DeleteBackups(PageInfo page, int revision)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (revision < -1) throw new ArgumentOutOfRangeException("Invalid Revision", "revision");

            lock (this)
            {
                var temp = GetBackups(page);
                if (temp == null) return false;
                if (temp.Length == 0) return true;

                var backups = new List<int>(temp);

                var idx = (revision != -1 ? backups.IndexOf(revision) : backups[backups.Count - 1]);

                // Operations
                // - Delete old beckups, from 0 to revision
                // - Rename newer backups starting from 0

                var local = (LocalPageInfo)page;
                var extension = Path.GetExtension(local.File);
                var filenameNoExt = Path.GetFileNameWithoutExtension(local.File);
                var nsDir = GetNamespacePartialPathForPageContent(NameTools.GetNamespace(page.FullName));

                for (var i = 0; i <= idx; i++)
                {
                    File.Delete(
                        GetFullPathForPageContent(nsDir + filenameNoExt + "." + Tools.GetVersionString(backups[i]) +
                                                  extension));
                }

                if (revision != -1)
                {
                    for (var i = revision + 1; i < backups.Count; i++)
                    {
                        File.Move(
                            GetFullPathForPageContent(nsDir + filenameNoExt + "." + Tools.GetVersionString(backups[i]) +
                                                      extension),
                            GetFullPathForPageContent(nsDir + filenameNoExt + "." +
                                                      Tools.GetVersionString(backups[i] - revision - 1) + extension));
                    }
                }
            }
            return true;
        }

        /// <summary>
        ///     Removes a Page.
        /// </summary>
        /// <param name="page">The Page to remove.</param>
        /// <returns><c>true</c> if the Page has been removed successfully, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public bool RemovePage(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException("page");

            lock (this)
            {
                var currentNs = FindNamespace(NameTools.GetNamespace(page.FullName), GetNamespaces());
                if (currentNs != null && currentNs.DefaultPage != null)
                {
                    // Cannot remove the default page
                    if (new PageNameComparer().Compare(currentNs.DefaultPage, page) == 0) return false;
                }

                var allPages = new List<PageInfo>(GetAllPages());
                var comp = new PageNameComparer();
                for (var i = 0; i < allPages.Count; i++)
                {
                    if (comp.Compare(allPages[i], page) == 0)
                    {
                        var content = GetContent(page);

                        var local = page as LocalPageInfo;

                        // Update search engine index
                        UnindexPage(content);
                        var messages = GetMessages(local);
                        foreach (var msg in messages)
                        {
                            UnindexMessageTree(local, msg);
                        }

                        allPages.Remove(allPages[i]);
                        DeleteBackups(page, -1);
                        DumpPages(allPages.ToArray());
                        try
                        {
                            File.Delete(
                                GetFullPathForPageContent(
                                    GetNamespacePartialPathForPageContent(NameTools.GetNamespace(page.FullName)) +
                                    ((LocalPageInfo)page).File));
                        }
                        catch
                        {
                        }
                        try
                        {
                            File.Delete(GetDraftFullPath(local));
                        }
                        catch
                        {
                        }
                        try
                        {
                            File.Delete(GetFullPathForMessages(local.File));
                        }
                        catch
                        {
                        }
                        pagesCache = null;
                        categoriesCache = null;

                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///     Binds a Page with one or more Categories.
        /// </summary>
        /// <param name="page">The Page to bind.</param>
        /// <param name="categories">The Categories to bind the Page with (full name).</param>
        /// <returns>True if the binding succeeded.</returns>
        /// <remarks>After a successful operation, the Page is bound with all and only the categories passed as argument.</remarks>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> or <paramref name="categories" /> are <c>null</c>.</exception>
        public bool RebindPage(PageInfo page, string[] categories)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (categories == null) throw new ArgumentNullException("categories");

            lock (this)
            {
                if (!PageExists(page)) return false;

                var cats = GetAllCategories();

                // Check all categories (they all must exist and be valid)
                foreach (var cat in categories)
                {
                    if (cat == null) throw new ArgumentNullException("categories", "A category name cannot be null");
                    if (cat.Length == 0) throw new ArgumentException("A category name cannot be empty", "categories");

                    var comp = new CategoryNameComparer();
                    if (
                        Array.Find(cats,
                            delegate (CategoryInfo x) { return comp.Compare(x, new CategoryInfo(cat, this)) == 0; }) ==
                        null) return false;
                }

                // Operations:
                // - Remove the Page from every Category
                // - For each specified category, add (if needed) the Page
                List<string> pages;
                var catComp = new CategoryNameComparer();
                for (var i = 0; i < cats.Length; i++)
                {
                    pages = new List<string>(cats[i].Pages);

                    var idx = GetIndex(pages, page.FullName);

                    if (idx != -1) pages.Remove(pages[idx]);
                    cats[i].Pages = pages.ToArray();
                }

                for (var i = 0; i < cats.Length; i++)
                {
                    for (var k = 0; k < categories.Length; k++)
                    {
                        if (catComp.Compare(cats[i], new CategoryInfo(categories[k], this)) == 0)
                        {
                            pages = new List<string>(cats[i].Pages);
                            pages.Add(page.FullName);
                            cats[i].Pages = pages.ToArray();
                        }
                    }
                }
                DumpCategories(cats);
                pagesCache = null;
                categoriesCache = null;
            }
            return true;
        }

        /// <summary>
        ///     Gets the Page Messages.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <returns>The list of the <b>first-level</b> Messages, containing the replies properly nested, sorted by date/time.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public IList<Message> GetMessages(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException("page");

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return null;

                // Shortcut
                if (!File.Exists(GetFullPathForMessages(local.File))) return new Message[0];

                var data = File.ReadAllText(GetFullPathForMessages(local.File)).Replace("\r", "");

                var lines = data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var result = new List<Message>();

                // Structure
                // ID|Username|Subject|DateTime|ParentID|Body

                // This algorithm DOES not handle replies that are stored BEFORE their parent,
                // so every reply MUST be stored anywhere but AFTER its parent
                // (the message tree should be stored depht-first; new messages can be appended at the end of the file)

                string[] fields;
                int id, parent;
                string username, subject, body;
                DateTime dateTime;
                for (var i = 0; i < lines.Length; i++)
                {
                    fields = lines[i].Split('|');
                    id = int.Parse(fields[0]);
                    username = fields[1];
                    subject = Tools.UnescapeString(fields[2]);
                    dateTime = DateTime.Parse(fields[3]);
                    parent = int.Parse(fields[4]);
                    body = Tools.UnescapeString(fields[5]);
                    if (parent != -1)
                    {
                        // Find parent
                        var p = FindMessage(result, parent);
                        if (p == null)
                        {
                            // Add as top-level message
                            result.Add(new Message(id, username, subject, dateTime, body));
                        }
                        else
                        {
                            // Add to parent's replies
                            var newMessages = new Message[p.Replies.Length + 1];
                            Array.Copy(p.Replies, newMessages, p.Replies.Length);
                            newMessages[newMessages.Length - 1] = new Message(id, username, subject, dateTime, body);
                            p.Replies = newMessages;
                        }
                    }
                    else
                    {
                        // Add as top-level message
                        result.Add(new Message(id, username, subject, dateTime, body));
                    }
                }

                result.Sort((a, b) => { return a.DateTime.CompareTo(b.DateTime); });

                return result;
            }
        }

        /// <summary>
        ///     Gets the total number of Messages in a Page Discussion.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <returns>The number of messages.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        public int GetMessageCount(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException("page");

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return -1;

                if (!File.Exists(GetFullPathForMessages(local.File))) return 0;
                var data = File.ReadAllText(GetFullPathForMessages(local.File)).Replace("\r", "");
                return data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries).Length;
            }
        }

        /// <summary>
        ///     Removes all messages for a page and stores the new messages.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="messages">The new messages to store.</param>
        /// <returns><c>true</c> if the messages are stored, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> or <paramref name="messages" /> are <c>null</c>.</exception>
        public bool BulkStoreMessages(PageInfo page, IList<Message> messages)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (messages == null) throw new ArgumentNullException("messages");

            var local = LoadLocalPageInfo(page);
            if (local == null) return false;

            // Validate IDs by using a dictionary as a way of validation
            try
            {
                var ids = new Dictionary<int, byte>(50);
                foreach (var msg in messages)
                {
                    AddAllIds(ids, msg);
                }
            }
            catch (ArgumentException)
            {
                return false;
            }

            // Be sure to remove all old messages from the search engine index
            foreach (var msg in GetMessages(local))
            {
                UnindexMessageTree(local, msg);
            }

            // Simply overwrite all messages on disk
            DumpMessages(local, messages);

            // Add the new messages to the search engine index 
            foreach (var msg in messages)
            {
                IndexMessageTree(local, msg);
            }

            return true;
        }

        /// <summary>
        ///     Adds a new Message to a Page.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <param name="username">The Username.</param>
        /// <param name="subject">The Subject.</param>
        /// <param name="dateTime">The Date/Time.</param>
        /// <param name="body">The Body.</param>
        /// <param name="parent">The Parent Message ID, or -1.</param>
        /// <returns>True if the Message has been added successfully.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="page" />, <paramref name="username" />,
        ///     <paramref name="subject" /> or <paramref name="body" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">If <paramref name="username" /> or <paramref name="subject" /> are empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="parent" /> is less than -1.</exception>
        public bool AddMessage(PageInfo page, string username, string subject, DateTime dateTime, string body,
            int parent)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (username == null) throw new ArgumentNullException("username");
            if (username.Length == 0) throw new ArgumentException("Username cannot be empty", "username");
            if (subject == null) throw new ArgumentNullException("subject");
            if (subject.Length == 0) throw new ArgumentException("Subject cannot be empty", "subject");
            if (body == null) throw new ArgumentNullException("body"); // body can be empty
            if (parent < -1) throw new ArgumentOutOfRangeException("parent", "Invalid Parent Message ID");

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return false;

                if (parent != -1)
                {
                    // Check for existence of parent message
                    var allMessages = GetMessages(page);
                    if (FindMessage(new List<Message>(allMessages), parent) == null) return false;
                }

                subject = Tools.EscapeString(subject);
                body = Tools.EscapeString(body);
                var sb = new StringBuilder();

                // Structure
                // ID|Username|Subject|DateTime|ParentID|Body

                var messageID = GetFreeMessageID(local);

                sb.Append(messageID);
                sb.Append("|");
                sb.Append(username);
                sb.Append("|");
                sb.Append(subject);
                sb.Append("|");
                sb.Append(dateTime.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"));
                sb.Append("|");
                sb.Append(parent.ToString());
                sb.Append("|");
                sb.Append(body);
                sb.Append("\r\n");

                File.AppendAllText(GetFullPathForMessages(local.File), sb.ToString());

                // Update search engine index
                IndexMessage(local, messageID, subject, dateTime, body);
            }
            return true;
        }

        /// <summary>
        ///     Removes a Message.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <param name="id">The ID of the Message to remove.</param>
        /// <param name="removeReplies">A value specifying whether or not to remove the replies.</param>
        /// <returns>True if the Message has been removed successfully.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="page" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="id" /> is less than zero.</exception>
        public bool RemoveMessage(PageInfo page, int id, bool removeReplies)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (id < 0) throw new ArgumentOutOfRangeException("Invalid ID", "id");

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return false;

                var messages = GetMessages(page);
                var msg = FindMessage(messages, id);
                if (msg == null) return false;

                var replies = new Message[0];
                if (!removeReplies)
                {
                    replies = msg.Replies;
                }

                if (!removeReplies && replies.Length > 0)
                {
                    // Find Message's anchestor
                    var anchestor = FindAnchestor(messages, msg.ID);
                    if (anchestor != null)
                    {
                        var newReplies = new Message[anchestor.Replies.Length + replies.Length];
                        Array.Copy(anchestor.Replies, newReplies, anchestor.Replies.Length);
                        Array.Copy(replies, 0, newReplies, anchestor.Replies.Length, replies.Length);
                        anchestor.Replies = newReplies;
                    }
                    else
                    {
                        messages = messages.Concat(replies).ToList();
                    }
                }

                // Recursively update search engine index
                if (removeReplies)
                {
                    UnindexMessageTree(page, msg);
                }
                else UnindexMessage(page, msg.ID, msg.Subject, msg.DateTime, msg.Body);

                var tempList = new List<Message>(messages);
                RemoveMessage(tempList, msg);
                messages = tempList.ToArray();
                tempList = null;

                DumpMessages(page, messages);
            }
            return true;
        }

        /// <summary>
        ///     Modifies a Message.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <param name="id">The ID of the Message to modify.</param>
        /// <param name="username">The Username.</param>
        /// <param name="subject">The Subject.</param>
        /// <param name="dateTime">The Date/Time.</param>
        /// <param name="body">The Body.</param>
        /// <returns>True if the Message has been modified successfully.</returns>
        /// <exception cref="ArgumentNullException">
        ///     If <paramref name="page" />, <paramref name="username" />,
        ///     <paramref name="subject" /> or <paramref name="body" /> are <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">If <paramref name="id" /> is less than zero.</exception>
        /// <exception cref="ArgumentException">If <paramref name="username" /> or <paramref name="subject" /> are empty.</exception>
        public bool ModifyMessage(PageInfo page, int id, string username, string subject, DateTime dateTime, string body)
        {
            if (page == null) throw new ArgumentNullException("page");
            if (id < 0) throw new ArgumentOutOfRangeException("Invalid Message ID", "id");
            if (username == null) throw new ArgumentNullException("username");
            if (username.Length == 0) throw new ArgumentException("Username cannot be empty", "username");
            if (subject == null) throw new ArgumentNullException("subject");
            if (subject.Length == 0) throw new ArgumentException("Subject cannot be empty", "subject");
            if (body == null) throw new ArgumentNullException("body"); // body can be empty

            lock (this)
            {
                if (LoadLocalPageInfo(page) == null) return false;

                var messages = new List<Message>(GetMessages(page));

                var msg = FindMessage(messages, id);

                if (msg == null) return false;

                // Update search engine index
                UnindexMessage(page, id, msg.Subject, msg.DateTime, msg.Body);

                msg.Username = username;
                msg.Subject = subject;
                msg.DateTime = dateTime;
                msg.Body = body;

                DumpMessages(page, messages);

                // Update search engine index
                IndexMessage(page, id, subject, dateTime, body);
            }
            return true;
        }

        /// <summary>
        ///     Gets all the Navigation Paths in a Namespace.
        /// </summary>
        /// <param name="nspace">The Namespace.</param>
        /// <returns>All the Navigation Paths, sorted by name.</returns>
        public IList<NavigationPath> GetNavigationPaths(NamespaceInfo nspace)
        {
            lock (this)
            {
                var allNavigationPaths = GetAllNavigationPaths();

                var selectedNavigationPaths = new List<NavigationPath>(allNavigationPaths.Length / 4);

                foreach (var path in allNavigationPaths)
                {
                    var pathNamespace = NameTools.GetNamespace(path.FullName);
                    if (nspace == null && pathNamespace == null) selectedNavigationPaths.Add(path);
                    if (nspace != null && pathNamespace != null &&
                        StringComparer.OrdinalIgnoreCase.Compare(nspace.Name, pathNamespace) == 0)
                        selectedNavigationPaths.Add(path);
                }

                selectedNavigationPaths.Sort(new NavigationPathComparer());

                return selectedNavigationPaths;
            }
        }

        /// <summary>
        ///     Adds a new Navigation Path.
        /// </summary>
        /// <param name="nspace">The target namespace (<c>null</c> for the root).</param>
        /// <param name="name">The Name of the Path.</param>
        /// <param name="pages">The Pages array.</param>
        /// <returns>The correct <see cref="T:NavigationPath" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> or <paramref name="pages" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="name" /> or <paramref name="pages" /> are empty.</exception>
        public NavigationPath AddNavigationPath(string nspace, string name, PageInfo[] pages)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");
            if (pages == null) throw new ArgumentNullException("pages");
            if (pages.Length == 0) throw new ArgumentException("Pages cannot be empty");

            lock (this)
            {
                var comp = new NavigationPathComparer();
                var temp = new NavigationPath(NameTools.GetFullName(nspace, name), this);
                if (
                    Array.Find(GetAllNavigationPaths(),
                        delegate (NavigationPath p) { return comp.Compare(p, temp) == 0; }) != null) return null;
                temp = null;

                foreach (var page in pages)
                {
                    if (page == null) throw new ArgumentNullException("pages", "A page element cannot be null");
                    if (LoadLocalPageInfo(page) == null) throw new ArgumentException("Page not found", "pages");
                }

                var result = new NavigationPath(NameTools.GetFullName(nspace, name), this);
                var tempPages = new List<string>(pages.Length);

                var sb = new StringBuilder(500);

                sb.Append("\r\n");
                sb.Append(result.FullName);
                for (var i = 0; i < pages.Length; i++)
                {
                    if (pages[i].Provider == this)
                    {
                        sb.Append("|");
                        sb.Append(pages[i].FullName);
                        tempPages.Add(pages[i].FullName);
                    }
                }
                result.Pages = tempPages.ToArray();

                File.AppendAllText(GetFullPath(NavigationPathsFile), sb.ToString());
                return result;
            }
        }

        /// <summary>
        ///     Modifies an existing Navigation Path.
        /// </summary>
        /// <param name="path">The Navigation Path to modify.</param>
        /// <param name="pages">The new Pages array.</param>
        /// <returns>The correct <see cref="T:NavigationPath" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path" /> or <paramref name="pages" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="pages" /> is empty.</exception>
        public NavigationPath ModifyNavigationPath(NavigationPath path, PageInfo[] pages)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (pages == null) throw new ArgumentNullException("pages");
            if (pages.Length == 0) throw new ArgumentException("Pages cannot be empty");

            lock (this)
            {
                foreach (var page in pages)
                {
                    if (page == null) throw new ArgumentNullException("pages", "A page element cannot be null");
                    if (LoadLocalPageInfo(page) == null) throw new ArgumentException("Page not found", "pages");
                }

                var paths = GetAllNavigationPaths();
                var comp = new NavigationPathComparer();
                for (var i = 0; i < paths.Length; i++)
                {
                    if (comp.Compare(path, paths[i]) == 0)
                    {
                        paths[i].Pages = new string[0];

                        var np = new NavigationPath(path.FullName, this);
                        var tempPages = new List<string>(pages.Length);

                        for (var k = 0; k < pages.Length; k++)
                        {
                            if (pages[k].Provider == this)
                            {
                                tempPages.Add(pages[k].FullName);
                            }
                        }
                        np.Pages = tempPages.ToArray();
                        paths[i] = np;

                        DumpNavigationPaths(paths);
                        return np;
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     Removes a Navigation Path.
        /// </summary>
        /// <param name="path">The Navigation Path to remove.</param>
        /// <returns>True if the Path is removed successfully.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="path" /> is <c>null</c>.</exception>
        public bool RemoveNavigationPath(NavigationPath path)
        {
            if (path == null) throw new ArgumentNullException("path");

            lock (this)
            {
                var paths = new List<NavigationPath>(GetAllNavigationPaths());
                var comp = new NavigationPathComparer();
                for (var i = 0; i < paths.Count; i++)
                {
                    if (comp.Compare(path, paths[i]) == 0)
                    {
                        paths.Remove(paths[i]);
                        DumpNavigationPaths(paths.ToArray());
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        ///     Gets all the Snippets.
        /// </summary>
        /// <returns>All the Snippets, sorted by name.</returns>
        public IList<Snippet> GetSnippets()
        {
            lock (this)
            {
                var files = Directory.GetFiles(GetFullPath(SnippetsDirectory), "*.cs");

                var snippets = new Snippet[files.Length];
                for (var i = 0; i < files.Length; i++)
                {
                    snippets[i] = new Snippet(Path.GetFileNameWithoutExtension(files[i]), File.ReadAllText(files[i]),
                        this);
                }

                Array.Sort(snippets, new SnippetNameComparer());

                return snippets;
            }
        }

        /// <summary>
        ///     Adds a new Snippet.
        /// </summary>
        /// <param name="name">The Name of the Snippet.</param>
        /// <param name="content">The Content of the Snippet.</param>
        /// <returns>The correct Snippet object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> or <paramref name="content" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public Snippet AddSnippet(string name, string content)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");
            if (content == null) throw new ArgumentNullException("content"); // content can be empty

            lock (this)
            {
                var comp = new SnippetNameComparer();
                var temp = new Snippet(name, content, this);
                if (GetSnippets().Any(s => comp.Compare(s, temp) == 0))
                    return null;
                temp = null;

                File.WriteAllText(GetFullPathForSnippets(name + ".cs"), content);
                return new Snippet(name, content, this);
            }
        }

        /// <summary>
        ///     Modifies a new Snippet.
        /// </summary>
        /// <param name="name">The Name of the Snippet to modify.</param>
        /// <param name="content">The Content of the Snippet.</param>
        /// <returns>The correct Snippet object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> or <paramref name="content" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public Snippet ModifySnippet(string name, string content)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");
            if (content == null) throw new ArgumentNullException("content"); // content can be empty

            lock (this)
            {
                var comp = new SnippetNameComparer();
                var temp = new Snippet(name, content, this);
                if (GetSnippets().Any(s => comp.Compare(s, temp) != 0))
                    return null;
                temp = null;

                File.WriteAllText(GetFullPathForSnippets(name + ".cs"), content);
                return new Snippet(name, content, this);
            }
        }

        /// <summary>
        ///     Removes a new Snippet.
        /// </summary>
        /// <param name="name">The Name of the Snippet to remove.</param>
        /// <returns>True if the Snippet is removed successfully.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public bool RemoveSnippet(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");

            lock (this)
            {
                var comp = new SnippetNameComparer();
                var temp = new Snippet(name, "", this);
                if (GetSnippets().Any(s => comp.Compare(s, temp) == 0))
                    return false;
                temp = null;

                File.Delete(GetFullPathForSnippets(name + ".cs"));
            }
            return true;
        }

        /// <summary>
        ///     Gets all the content templates.
        /// </summary>
        /// <returns>All the content templates, sorted by name.</returns>
        public ContentTemplate[] GetContentTemplates()
        {
            lock (this)
            {
                var files = Directory.GetFiles(GetFullPath(ContentTemplatesDirectory), "*.cs");

                var templates = new ContentTemplate[files.Length];
                for (var i = 0; i < files.Length; i++)
                {
                    templates[i] = new ContentTemplate(Path.GetFileNameWithoutExtension(files[i]),
                        File.ReadAllText(files[i]), this);
                }

                Array.Sort(templates, new ContentTemplateNameComparer());

                return templates;
            }
        }

        /// <summary>
        ///     Adds a new content template.
        /// </summary>
        /// <param name="name">The name of template.</param>
        /// <param name="content">The content of the template.</param>
        /// <returns>The correct <see cref="T:ContentTemplate" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> or <paramref name="content" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public ContentTemplate AddContentTemplate(string name, string content)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");
            if (content == null) throw new ArgumentNullException("content");

            lock (this)
            {
                var file = GetFullPathForContentTemplate(name + ".cs");

                if (File.Exists(file)) return null;

                File.WriteAllText(file, content);

                return new ContentTemplate(name, content, this);
            }
        }

        /// <summary>
        ///     Modifies an existing content template.
        /// </summary>
        /// <param name="name">The name of the template to modify.</param>
        /// <param name="content">The content of the template.</param>
        /// <returns>The correct <see cref="T:ContentTemplate" /> object.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> or <paramref name="content" /> are <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public ContentTemplate ModifyContentTemplate(string name, string content)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");
            if (content == null) throw new ArgumentNullException("content");

            lock (this)
            {
                var file = GetFullPathForContentTemplate(name + ".cs");

                if (!File.Exists(file)) return null;

                File.WriteAllText(file, content);

                return new ContentTemplate(name, content, this);
            }
        }

        /// <summary>
        ///     Removes a content template.
        /// </summary>
        /// <param name="name">The name of the template to remove.</param>
        /// <returns><c>true</c> if the template is removed, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="name" /> is empty.</exception>
        public bool RemoveContentTemplate(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Length == 0) throw new ArgumentException("Name cannot be empty", "name");

            lock (this)
            {
                var file = GetFullPathForContentTemplate(name + ".cs");

                if (!File.Exists(file)) return false;

                File.Delete(file);

                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nspace"></param>
        /// <param name="category"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IList<PageInfoWithTitle> GetPagesInfoWithTitleFromCategories(string nspace, string category, int pageIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        private string GetFullPath(string filename)
        {
            return Path.Combine(GetDataDirectory(host), filename);
        }

        private string GetFullPathForPageContent(string filename)
        {
            return Path.Combine(Path.Combine(GetDataDirectory(host), PagesDirectory), filename);
        }

        private string GetFullPathForPageDrafts(string filename)
        {
            return Path.Combine(Path.Combine(GetDataDirectory(host), DraftsDirectory), filename);
        }

        private string GetDraftFullPath(LocalPageInfo page)
        {
            /*return GetFullPathForPageDrafts(GetNamespacePartialPathForPageContent(NameTools.GetNamespace(page.FullName))
                + page.File);*/
            return GetFullPathForPageDrafts(page.File);
        }

        private string GetFullPathForMessages(string filename)
        {
            return Path.Combine(Path.Combine(GetDataDirectory(host), MessagesDirectory), filename);
        }

        private string GetFullPathForSnippets(string filename)
        {
            return Path.Combine(Path.Combine(GetDataDirectory(host), SnippetsDirectory), filename);
        }

        private string GetFullPathForContentTemplate(string filename)
        {
            return Path.Combine(Path.Combine(GetDataDirectory(host), ContentTemplatesDirectory), filename);
        }

        /// <summary>
        ///     Gets the partial path of the folder that contains page content files for the specified namespace, followed by a
        ///     directory separator char if appropriate.
        /// </summary>
        /// <param name="nspace">The namespace, or <c>null</c>.</param>
        /// <returns>The correct partial path, such as 'Namespace\' or ''.</returns>
        private string GetNamespacePartialPathForPageContent(string nspace)
        {
            if (nspace == null || nspace.Length == 0) return "";
            return nspace + Path.DirectorySeparatorChar;
        }

        /// <summary>
        ///     Verifies the need for a data upgrade, and performs it when needed.
        /// </summary>
        private void VerifyAndPerformUpgradeForCategories()
        {
            // Load file lines, replacing all dots with underscores in category names

            host.LogEntry("Upgrading categories format from 2.0 to 3.0", LogEntryType.General, null, this);

            var lines = File.ReadAllText(GetFullPath(CategoriesFile))
                .Replace("\r", "")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length > 0)
            {
                string[] fields;
                for (var i = 0; i < lines.Length; i++)
                {
                    fields = lines[i].Split('|');
                    // Rename category
                    if (fields[0].Contains("."))
                    {
                        fields[0] = fields[0].Replace(".", "_");
                    }
                    // Rename all pages
                    for (var f = 1; f < fields.Length; f++)
                    {
                        if (fields[f].Contains("."))
                        {
                            fields[f] = fields[f].Replace(".", "_");
                        }
                    }
                    lines[i] = string.Join("|", fields);
                }

                var backupFile =
                    GetFullPath(Path.GetFileNameWithoutExtension(CategoriesFile) + "_v2" +
                                Path.GetExtension(CategoriesFile));
                File.Copy(GetFullPath(CategoriesFile), backupFile);

                File.WriteAllLines(GetFullPath(CategoriesFile), lines);
            }
        }

        /// <summary>
        ///     Verifies whether the pages files needs to be upgraded.
        /// </summary>
        /// <returns></returns>
        private bool VerifyIfPagesFileNeedsAnUpgrade()
        {
            var file = GetFullPath(PagesFile);
            if (!File.Exists(file)) return false;

            var lines = File.ReadAllText(file)
                .Replace("\r", "")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            string[] fields;
            foreach (var line in lines)
            {
                fields = line.Split('|');

                // Field count has never been 3 except in version 3.0
                if (fields.Length == 3) return false;

                // Version 1.0
                if (fields.Length == 2) return true;
                // Version 2.0
                if (fields.Length == 4) return true;
            }

            return false;
        }

        /// <summary>
        ///     Verifies the need for a data upgrade, and performs it when needed.
        /// </summary>
        private void VerifyAndPerformUpgradeForPages()
        {
            // Load file lines
            // Parse first line (if any) with old (v2) algorithm
            // If parsing is successful, then the file must be converted
            // Conversion consists in removing the 'Status' field and properly modifying permissions of pages

            host.LogEntry("Upgrading pages format from 2.0 to 3.0", LogEntryType.General, null, this);

            //string[] lines = File.ReadAllLines(GetFullPath(PagesFile));
            var lines = File.ReadAllText(GetFullPath(PagesFile))
                .Replace("\r", "")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length > 0)
            {
                var pages = new LocalPageInfo[lines.Length];
                var oldStylePermissions = new char[lines.Length];

                char[] splitter = { '|' };

                for (var i = 0; i < lines.Length; i++)
                {
                    var fields = lines[i].Split(splitter, StringSplitOptions.RemoveEmptyEntries);

                    // Structure in version 1.0
                    // PageName|PageFile

                    // Structure in version 2.0
                    // PageName|PageFile|Status|DateTime

                    // Use default values (status and date/time were not available in earlier versions)
                    var creationDateTime = new DateTime(2000, 1, 1);

                    // Default to Normal
                    oldStylePermissions[i] = 'N';

                    if (fields.Length == 2)
                    {
                        // Version 1.0
                        // Use the Date/Time of the file
                        var fi = new FileInfo(GetFullPathForPageContent(fields[1]));
                        creationDateTime = fi.CreationTime;
                    }
                    if (fields.Length >= 3)
                    {
                        // Might be version 2.0
                        switch (fields[2].ToLowerInvariant())
                        {
                            case "locked":
                                oldStylePermissions[i] = 'L';
                                break;
                            case "public":
                                oldStylePermissions[i] = 'P';
                                break;
                            case "normal":
                                oldStylePermissions[i] = 'N';
                                break;
                            default:
                                try
                                {
                                    // If this succeeded, then it's Version 3.0, not 2.0 (at least for this line)
                                    creationDateTime = DateTime.Parse(fields[2]);
                                }
                                catch
                                {
                                    // Use the Date/Time of the file
                                    var fi = new FileInfo(GetFullPathForPageContent(fields[1]));
                                    creationDateTime = fi.CreationTime;
                                }
                                break;
                        }
                        if (fields.Length == 4)
                        {
                            // Version 2.0
                            creationDateTime = DateTime.Parse(fields[3]);
                        }
                    }

                    pages[i] = new LocalPageInfo(fields[0], this, creationDateTime, fields[1]);
                    pages[i].Rename(pages[i].FullName.Replace(".", "_"));
                    // TODO: host.UpdateContentForPageRename(oldName, newName);
                }

                // Setup permissions for single pages
                for (var i = 0; i < oldStylePermissions.Length; i++)
                {
                    if (oldStylePermissions[i] != 'N')
                    {
                        // Need to set permissions emulating old-style behavior
                        host.UpgradePageStatusToAcl(pages[i], oldStylePermissions[i]);
                    }
                }

                var backupFile =
                    GetFullPath(Path.GetFileNameWithoutExtension(PagesFile) + "_v2" + Path.GetExtension(PagesFile));
                File.Copy(GetFullPath(PagesFile), backupFile);

                // Re-dump pages so that old format data is discarded
                DumpPages(pages);
            }
        }

        /// <summary>
        ///     Verifies the need for a data upgrade, and performs it when needed.
        /// </summary>
        private void VerifyAndPerformUpgradeForNavigationPaths()
        {
            // Load file lines, replacing all dots with underscores in category names

            host.LogEntry("Upgrading navigation paths format from 2.0 to 3.0", LogEntryType.General, null, this);

            var lines = File.ReadAllText(GetFullPath(NavigationPathsFile))
                .Replace("\r", "")
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (lines.Length > 0)
            {
                string[] fields;
                for (var i = 0; i < lines.Length; i++)
                {
                    fields = lines[i].Split('|');
                    // Rename navigation path
                    if (fields[0].Contains("."))
                    {
                        fields[0] = fields[0].Replace(".", "_");
                    }
                    // Rename all pages
                    for (var f = 1; f < fields.Length; f++)
                    {
                        if (fields[f].Contains("."))
                        {
                            fields[f] = fields[f].Replace(".", "_");
                        }
                    }
                    lines[i] = string.Join("|", fields);
                }

                var backupFile =
                    GetFullPath(Path.GetFileNameWithoutExtension(NavigationPathsFile) + "_v2" +
                                Path.GetExtension(NavigationPathsFile));
                File.Copy(GetFullPath(NavigationPathsFile), backupFile);

                File.WriteAllLines(GetFullPath(NavigationPathsFile), lines);
            }
        }

        /// <summary>
        ///     Finds a page.
        /// </summary>
        /// <param name="nspace">The namespace that contains the page.</param>
        /// <param name="name">The name of the page to find.</param>
        /// <param name="pages">The pages array.</param>
        /// <returns>The found page, or <c>null</c>.</returns>
        private PageInfo FindPage(string nspace, string name, PageInfo[] pages)
        {
            if (name == null) return null;

            var comp = new PageNameComparer();
            var target = new PageInfo(NameTools.GetFullName(nspace, name), this, DateTime.Now);

            var result = Array.Find(pages, delegate (PageInfo p) { return comp.Compare(p, target) == 0; });

            return result;
        }

        /// <summary>
        ///     Finds a namespace.
        /// </summary>
        /// <param name="name">The name of the namespace to find.</param>
        /// <param name="namespaces">The namespaces array.</param>
        /// <returns>The found namespace, or <c>null</c>.</returns>
        private NamespaceInfo FindNamespace(string name, IEnumerable<NamespaceInfo> namespaces)
        {
            if (name == null) return null;

            var target = new NamespaceInfo(name, this, null);
            var comp = new NamespaceComparer();

            return namespaces.FirstOrDefault(n => comp.Compare(n, target) == 0);
        }

        /// <summary>
        ///     Determines whether a namespace exists.
        /// </summary>
        /// <param name="name">The name to check.</param>
        /// <returns>
        ///     <c>true</c> if the namespace exists or <b>name</b> is <c>null</c> (indicating the root), <c>false</c>
        ///     otherwise.
        /// </returns>
        private bool NamespaceExists(string name)
        {
            if (name == null) return true;
            if (FindNamespace(name, GetNamespaces()) != null) return true;
            return false;
        }

        /// <summary>
        ///     Dumps namespaces on disk.
        /// </summary>
        /// <param name="namespaces">The namespaces to dump.</param>
        private void DumpNamespaces(IEnumerable<NamespaceInfo> namespaces)
        {
            var sb = new StringBuilder();
            foreach (var ns in namespaces)
            {
                sb.Append(ns.Name);
                sb.Append("|");
                sb.Append(ns.DefaultPage != null ? ns.DefaultPage.FullName : "");
                sb.Append("\r\n");
            }
            File.WriteAllText(GetFullPath(NamespacesFile), sb.ToString());
        }

        /// <summary>
        ///     Moves the backups of a page into a new namespace.
        /// </summary>
        /// <param name="page">The page that is being moved.</param>
        /// <param name="destination">The destination namespace (<c>null</c> for the root).</param>
        /// <remarks>This method should be invoked <b>before</b> moving the corresponding page.</remarks>
        private void MoveBackups(PageInfo page, NamespaceInfo destination)
        {
            lock (this)
            {
                var backups = GetBackups(page);
                if (backups == null) return; // Page does not exist

                var local = (LocalPageInfo)page;
                var extension = Path.GetExtension(local.File);
                var currDir = GetNamespacePartialPathForPageContent(NameTools.GetNamespace(page.FullName));
                var newDir = GetNamespacePartialPathForPageContent(destination != null ? destination.Name : null);
                var currPartialName = currDir + Path.GetFileNameWithoutExtension(local.File) + ".";
                var newPartialName = newDir + NameTools.GetLocalName(page.FullName) + ".";

                for (var i = 0; i < backups.Length; i++)
                {
                    File.Move(
                        GetFullPathForPageContent(currPartialName + Tools.GetVersionString(backups[i]) + extension),
                        GetFullPathForPageContent(newPartialName + Tools.GetVersionString(backups[i]) + extension));
                }
            }
        }

        /// <summary>
        ///     Extracts an instance of <see cref="T:CategoryInfo" /> from a line contained in the categories file.
        /// </summary>
        /// <param name="fileLine">The line to process.</param>
        /// <returns>The instance of <see cref="T:CategoryInfo" />.</returns>
        private CategoryInfo BuildCategoryInfo(string fileLine)
        {
            var fields = fileLine.Split('|');

            // Structure
            // Namespace.Cat|Namespace.Page1|Namespace.Page2|...
            // First field can be 'Cat' or 'Namespace.Cat'

            string nspace, name;
            NameTools.ExpandFullName(fields[0], out nspace, out name);

            var result = new CategoryInfo(fields[0], this);

            var pages = new List<string>(fields.Length);
            for (var k = 0; k < fields.Length - 1; k++)
            {
                if (PageExists(new PageInfo(fields[k + 1], this, DateTime.Now)))
                {
                    pages.Add(fields[k + 1]);
                }
            }
            result.Pages = pages.ToArray();

            return result;
        }

        /// <summary>
        ///     Gets all the Categories.
        /// </summary>
        /// <returns>The Categories.</returns>
        private CategoryInfo[] GetAllCategories()
        {
            lock (this)
            {
                if (categoriesCache == null)
                {
                    var tmp = File.ReadAllText(GetFullPath(CategoriesFile)).Replace("\r", "");

                    var lines = tmp.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var result = new CategoryInfo[lines.Length];

                    for (var i = 0; i < lines.Length; i++)
                    {
                        result[i] = BuildCategoryInfo(lines[i]);
                    }

                    Array.Sort(result, new CategoryNameComparer());

                    categoriesCache = result;
                }

                return categoriesCache;
            }
        }

        /// <summary>
        ///     Determines whether a category exists.
        /// </summary>
        /// <param name="category">The category to check.</param>
        /// <returns><c>true</c> if the category exists, <c>false</c> otherwise.</returns>
        private bool CategoryExists(CategoryInfo category)
        {
            lock (this)
            {
                var cats = GetCategories(FindNamespace(NameTools.GetNamespace(category.FullName), GetNamespaces()));
                var comp = new CategoryNameComparer();
                for (var i = 0; i < cats.Count; i++)
                {
                    if (comp.Compare(cats[i], category) == 0) return true;
                }
            }
            return false;
        }

        /// <summary>
        ///     Handles the construction of an <see cref="T:IDocument" /> for the search engine.
        /// </summary>
        /// <param name="dumpedDocument">The input dumped document.</param>
        /// <returns>The resulting <see cref="T:IDocument" />.</returns>
        private IDocument BuildDocumentHandler(DumpedDocument dumpedDocument)
        {
            if (dumpedDocument.TypeTag == PageDocument.StandardTypeTag)
            {
                var pageName = PageDocument.GetPageName(dumpedDocument.Name);

                var page = FindPage(NameTools.GetNamespace(pageName), NameTools.GetLocalName(pageName),
                    GetAllPages());

                if (page == null) return null;
                return new PageDocument(page, dumpedDocument, TokenizeContent);
            }
            if (dumpedDocument.TypeTag == MessageDocument.StandardTypeTag)
            {
                string pageFullName;
                int id;
                MessageDocument.GetMessageDetails(dumpedDocument.Name, out pageFullName, out id);

                var page = FindPage(NameTools.GetNamespace(pageFullName), NameTools.GetLocalName(pageFullName),
                    GetAllPages());
                if (page == null) return null;
                return new MessageDocument(page, id, dumpedDocument, TokenizeContent);
            }
            return null;
        }

        /// <summary>
        ///     Tokenizes page content.
        /// </summary>
        /// <param name="content">The content to tokenize.</param>
        /// <returns>The tokenized words.</returns>
        private static WordInfo[] TokenizeContent(string content)
        {
            var words = SearchEngine.Tools.Tokenize(content);
            return words;
        }

        /// <summary>
        ///     Indexes a page.
        /// </summary>
        /// <param name="content">The content of the page.</param>
        /// <returns>The number of indexed words, including duplicates.</returns>
        private int IndexPage(PageContent content)
        {
            lock (this)
            {
                try
                {
                    if (string.IsNullOrEmpty(content.Title) || string.IsNullOrEmpty(content.Content)) return 0;

                    var documentName = PageDocument.GetDocumentName(content.PageInfo);

                    var ddoc = new DumpedDocument(0, documentName,
                        host.PrepareTitleForIndexing(content.PageInfo, content.Title),
                        PageDocument.StandardTypeTag, content.LastModified);

                    // Store the document
                    // The content should always be prepared using IHost.PrepareForSearchEngineIndexing()
                    var count = index.StoreDocument(new PageDocument(content.PageInfo, ddoc, TokenizeContent),
                        content.Keywords, host.PrepareContentForIndexing(content.PageInfo, content.Content), null);

                    if (count == 0 && content.Content.Length > 0)
                    {
                        host.LogEntry(
                            "Indexed 0 words for page " + content.PageInfo.FullName +
                            ": possible index corruption. Please report this error to the developers",
                            LogEntryType.Warning, null, this);
                    }

                    return count;
                }
                catch (Exception ex)
                {
                    host.LogEntry("Page indexing error for " + content.PageInfo.FullName + " (skipping page): " + ex,
                        LogEntryType.Error, null, this);
                    return 0;
                }
            }
        }

        /// <summary>
        ///     Removes a page from the search engine index.
        /// </summary>
        /// <param name="content">The content of the page to remove.</param>
        private void UnindexPage(PageContent content)
        {
            lock (this)
            {
                var documentName = PageDocument.GetDocumentName(content.PageInfo);

                var ddoc = new DumpedDocument(0, documentName,
                    host.PrepareTitleForIndexing(content.PageInfo, content.Title),
                    PageDocument.StandardTypeTag, content.LastModified);
                index.RemoveDocument(new PageDocument(content.PageInfo, ddoc, TokenizeContent), null);
            }
        }

        /// <summary>
        ///     Extracts an instance of <see cref="T:LocalPageInfo" /> from a line of the pages file.
        /// </summary>
        /// <param name="fileLine">The line to process.</param>
        /// <returns>The instance of <see cref="T:LocalPageInfo" />.</returns>
        private LocalPageInfo BuildLocalPageInfo(string fileLine)
        {
            var fields = fileLine.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            // Structure (file format already converted from earlier versions)
            // Namespace.PageName|PageFile|DateTime

            if (fields.Length == 3)
            {
                return new LocalPageInfo(fields[0], this, DateTime.Parse(fields[2]), fields[1]);
            }
            throw new ArgumentException("Unsupported data format", "fileLine");
        }

        /// <summary>
        ///     Gets all the Pages.
        /// </summary>
        /// <returns>All the Pages.</returns>
        private PageInfo[] GetAllPages()
        {
            lock (this)
            {
                if (pagesCache == null)
                {
                    var tmp = File.ReadAllText(GetFullPath(PagesFile)).Replace("\r", "");

                    var lines = tmp.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var result = new PageInfo[lines.Length];

                    for (var i = 0; i < lines.Length; i++)
                    {
                        result[i] = BuildLocalPageInfo(lines[i]);
                    }

                    pagesCache = result;
                }

                return pagesCache;
            }
        }

        /// <summary>
        ///     Finds a corresponding instance of <see cref="T:LocalPageInfo" /> in the available pages.
        /// </summary>
        /// <param name="page">The instance of <see cref="T:PageInfo" /> to "convert" to <see cref="T:LocalPageInfo" />.</param>
        /// <returns>The instance of <see cref="T:LocalPageInfo" />, or <c>null</c>.</returns>
        private LocalPageInfo LoadLocalPageInfo(PageInfo page)
        {
            if (page == null) return null;
            lock (this)
            {
                var pages = GetAllPages();
                var comp = new PageNameComparer();
                for (var i = 0; i < pages.Length; i++)
                {
                    if (comp.Compare(pages[i], page) == 0) return pages[i] as LocalPageInfo;
                }
            }
            return null;
        }

        /// <summary>
        ///     Determines whether a page exists.
        /// </summary>
        /// <param name="page">The instance of <see cref="T:PageInfo" /> to look for.</param>
        /// <returns><c>true</c> if the page exists, <c>false</c> otherwise.</returns>
        private bool PageExists(PageInfo page)
        {
            lock (this)
            {
                var pages = GetAllPages();
                var comp = new PageNameComparer();
                for (var i = 0; i < pages.Length; i++)
                {
                    if (comp.Compare(pages[i], page) == 0) return true;
                }
            }
            return false;
        }

        private PageContent ExtractContent(string data, PageInfo pageInfo)
        {
            if (data == null) return null;
            // Structure (Keywords and Description are new in v3)
            // Page Title
            // Username|DateTime[|Comment][|(((Keyword,Keyword,Keyword)))(((Description)))] --- Comment is optional
            // ##PAGE##
            // Content...
            data = data.Replace("\r", "");
            var lines = data.Split('\n');
            if (lines.Length < 4)
            {
                host.LogEntry("Corrupted or malformed page data for page " + pageInfo.FullName + " - returning empty",
                    LogEntryType.Error, null, this);
                return PageContent.GetEmpty(pageInfo);
            }
            var fields = lines[1].Split('|');

            string comment = null;
            string[] keywords = null;
            string description = null;

            if (fields.Length >= 3 && !fields[2].StartsWith("(((")) comment = Tools.UnescapeString(fields[2]);
            else comment = "";

            var lastField = fields[fields.Length - 1];
            if (lastField.StartsWith("(((") && lastField.EndsWith(")))"))
            {
                // Keywords and/or description are specified
                var closedBracketsIndex = lastField.IndexOf(")))"); // This identifies the end of keywords block
                var keywordsBlock = lastField.Substring(0, closedBracketsIndex).Trim('(', ')');
                keywords = keywordsBlock.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < keywords.Length; i++)
                {
                    keywords[i] = Tools.UnescapeString(keywords[i]);
                }

                description = Tools.UnescapeString(lastField.Substring(closedBracketsIndex + 3).Trim('(', ')'));
                if (string.IsNullOrEmpty(description)) description = null;
            }

            var nlIndex = data.IndexOf("\n"); // Index of first new-line char
            // Don't consider page title, since it might contain "##PAGE##"
            var idx = data.Substring(nlIndex + 1).IndexOf("##PAGE##") + 8 + 1 + nlIndex + 1;

            return new PageContent(pageInfo, lines[0], fields[0], DateTime.Parse(fields[1]), comment,
                data.Substring(idx), keywords, description);
        }

        /// <summary>
        ///     Backups a Page.
        /// </summary>
        /// <param name="page">The Page to backup.</param>
        /// <returns>True if the Page has been backupped successfully.</returns>
        private bool Backup(PageInfo page)
        {
            if (page == null) throw new ArgumentNullException("page");

            lock (this)
            {
                var local = LoadLocalPageInfo(page);
                if (local == null) return false;

                var backups = GetBackups(page);
                var rev = (backups.Length > 0 ? backups[backups.Length - 1] + 1 : 0);
                File.Copy(
                    GetFullPathForPageContent(local.File),
                    GetFullPathForPageContent(
                        GetNamespacePartialPathForPageContent(NameTools.GetNamespace(page.FullName)) +
                        Path.GetFileNameWithoutExtension(local.File) + "." + Tools.GetVersionString(rev) +
                        Path.GetExtension(local.File)));
            }
            return true;
        }

        /// <summary>
        ///     Renames the backups of a page.
        /// </summary>
        /// <param name="page">The page that is being renamed.</param>
        /// <param name="newName">The new name of the page.</param>
        /// <remarks>This method should be invoked <b>before</b> renaming the corresponding page.</remarks>
        private void RenameBackups(PageInfo page, string newName)
        {
            lock (this)
            {
                var backups = GetBackups(page);
                if (backups == null) return; // Page does not exist

                var local = (LocalPageInfo)page;
                var extension = Path.GetExtension(local.File);
                var nsDir = GetNamespacePartialPathForPageContent(NameTools.GetNamespace(page.FullName));
                var partialName = nsDir + Path.GetFileNameWithoutExtension(local.File) + ".";

                for (var i = 0; i < backups.Length; i++)
                {
                    File.Move(GetFullPathForPageContent(partialName + Tools.GetVersionString(backups[i]) + extension),
                        GetFullPathForPageContent(nsDir + newName + "." + Tools.GetVersionString(backups[i]) + extension));
                }
            }
        }

        private static int GetIndex(List<string> pages, string page)
        {
            for (var i = 0; i < pages.Count; i++)
            {
                if (StringComparer.OrdinalIgnoreCase.Compare(pages[i], page) == 0) return i;
            }
            return -1;
        }

        /// <summary>
        ///     Makes a backup copy of the pages file.
        /// </summary>
        private void BackupPagesFile()
        {
            lock (this)
            {
                File.Copy(GetFullPath(PagesFile),
                    GetFullPath(Path.GetFileNameWithoutExtension(PagesFile) +
                                ".bak" + Path.GetExtension(PagesFile)), true);
            }
        }

        /// <summary>
        ///     Writes all pages in the storage file.
        /// </summary>
        /// <param name="pages">The pages to write.</param>
        private void DumpPages(PageInfo[] pages)
        {
            lock (this)
            {
                BackupPagesFile();

                var sb = new StringBuilder();
                for (var i = 0; i < pages.Length; i++)
                {
                    sb.Append(pages[i].FullName);
                    sb.Append("|");
                    sb.Append(((LocalPageInfo)pages[i]).File);
                    sb.Append("|");
                    sb.Append(pages[i].CreationDateTime.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"));
                    sb.Append("\r\n");
                }
                File.WriteAllText(GetFullPath(PagesFile), sb.ToString());
            }
        }

        /// <summary>
        ///     Makes a backup copy of the categories file.
        /// </summary>
        private void BackupCategoriesFile()
        {
            lock (this)
            {
                File.Copy(GetFullPath(CategoriesFile),
                    GetFullPath(Path.GetFileNameWithoutExtension(CategoriesFile) +
                                ".bak" + Path.GetExtension(CategoriesFile)), true);
            }
        }

        /// <summary>
        ///     Writes all categories in the storage file.
        /// </summary>
        /// <param name="categories">The categories.</param>
        private void DumpCategories(IEnumerable<CategoryInfo> categories)
        {
            lock (this)
            {
                BackupCategoriesFile();

                // Format
                // NS.Category|NS.Page1|NS.Page2
                var sb = new StringBuilder(10480);
                foreach (var category in categories)
                {
                    sb.Append(category.FullName);
                    if (category.Pages.Count > 0)
                    {
                        for (var k = 0; k < category.Pages.Count; k++)
                        {
                            sb.Append("|");
                            sb.Append(category.Pages[k]);
                        }
                    }
                    sb.Append("\r\n");
                }
                File.WriteAllText(GetFullPath(CategoriesFile), sb.ToString());
            }
        }

        /// <summary>
        ///     Finds a Message in a Message tree.
        /// </summary>
        /// <param name="messages">The Message tree.</param>
        /// <param name="id">The ID of the Message to find.</param>
        /// <returns>The Message or null.</returns>
        /// <remarks>The method is recursive.</remarks>
        private static Message FindMessage(IEnumerable<Message> messages, int id)
        {
            Message result = null;
            foreach (var msg in messages)
            {
                if (msg.ID == id)
                {
                    result = msg;
                }
                if (result == null)
                {
                    result = FindMessage(msg.Replies, id);
                }
                if (result != null) break;
            }
            return result;
        }

        private static void AddAllIds(Dictionary<int, byte> dictionary, Message msg)
        {
            dictionary.Add(msg.ID, 0);
            foreach (var m in msg.Replies)
            {
                AddAllIds(dictionary, m);
            }
        }

        /// <summary>
        ///     Indexes a message.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="id">The message ID.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="dateTime">The date/time.</param>
        /// <param name="body">The body.</param>
        /// <returns>The number of indexed words, including duplicates.</returns>
        private int IndexMessage(PageInfo page, int id, string subject, DateTime dateTime, string body)
        {
            lock (this)
            {
                try
                {
                    if (string.IsNullOrEmpty(subject) || string.IsNullOrEmpty(body)) return 0;

                    // Trim "RE:" to avoid polluting the search engine index
                    if (subject.ToLowerInvariant().StartsWith("re:") && subject.Length > 3)
                        subject = subject.Substring(3).Trim();

                    var documentName = MessageDocument.GetDocumentName(page, id);

                    var ddoc = new DumpedDocument(0, documentName, host.PrepareTitleForIndexing(null, subject),
                        MessageDocument.StandardTypeTag, dateTime);

                    // Store the document
                    // The content should always be prepared using IHost.PrepareForSearchEngineIndexing()
                    var count = index.StoreDocument(new MessageDocument(page, id, ddoc, TokenizeContent), null,
                        host.PrepareContentForIndexing(null, body), null);

                    if (count == 0 && body.Length > 0)
                    {
                        host.LogEntry(
                            "Indexed 0 words for message " + page.FullName + ":" + id +
                            ": possible index corruption. Please report this error to the developers",
                            LogEntryType.Warning, null, this);
                    }

                    return count;
                }
                catch (Exception ex)
                {
                    host.LogEntry(
                        "Message indexing error for " + page.FullName + ":" + id + " (skipping message): " + ex,
                        LogEntryType.Error, null, this);
                    return 0;
                }
            }
        }

        /// <summary>
        ///     Indexes a message tree.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="root">The tree root.</param>
        private void IndexMessageTree(PageInfo page, Message root)
        {
            IndexMessage(page, root.ID, root.Subject, root.DateTime, root.Body);
            foreach (var reply in root.Replies)
            {
                IndexMessageTree(page, reply);
            }
        }

        /// <summary>
        ///     Removes a message from the search engine index.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="id">The message ID.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="dateTime">The date/time.</param>
        /// <param name="body">The body.</param>
        /// <returns>The number of indexed words, including duplicates.</returns>
        private void UnindexMessage(PageInfo page, int id, string subject, DateTime dateTime, string body)
        {
            lock (this)
            {
                // Trim "RE:" to avoid polluting the search engine index
                if (subject.ToLowerInvariant().StartsWith("re:") && subject.Length > 3)
                    subject = subject.Substring(3).Trim();

                var documentName = MessageDocument.GetDocumentName(page, id);

                var ddoc = new DumpedDocument(0, documentName, host.PrepareTitleForIndexing(null, subject),
                    MessageDocument.StandardTypeTag, DateTime.Now);
                index.RemoveDocument(new MessageDocument(page, id, ddoc, TokenizeContent), null);
            }
        }

        /// <summary>
        ///     Removes a message tree from the search engine index.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="root">The tree root.</param>
        private void UnindexMessageTree(PageInfo page, Message root)
        {
            UnindexMessage(page, root.ID, root.Subject, root.DateTime, root.Body);
            foreach (var reply in root.Replies)
            {
                UnindexMessageTree(page, reply);
            }
        }

        /// <summary>
        ///     Find a free Message ID for a Page.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <returns>The Message ID.</returns>
        private int GetFreeMessageID(LocalPageInfo page)
        {
            lock (this)
            {
                if (!File.Exists(GetFullPathForMessages(page.File))) return 0;

                var result = 0;

                var data = File.ReadAllText(GetFullPathForMessages(page.File)).Replace("\r", "");

                var lines = data.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                int idx, tmp;
                for (var i = 0; i < lines.Length; i++)
                {
                    idx = lines[i].IndexOf('|');
                    tmp = int.Parse(lines[i].Substring(0, idx));
                    if (tmp > result) result = tmp;
                }

                result++;

                return result;
            }
        }

        /// <summary>
        ///     Finds the anchestor/parent of a Message.
        /// </summary>
        /// <param name="messages">The Messages.</param>
        /// <param name="id">The Message ID.</param>
        /// <returns>The anchestor Message or null.</returns>
        private static Message FindAnchestor(IEnumerable<Message> messages, int id)
        {
            Message result = null;
            foreach (var msg in messages)
            {
                for (var k = 0; k < msg.Replies.Length; k++)
                {
                    if (msg.Replies[k].ID == id)
                    {
                        result = msg;
                        break;
                    }
                    if (result == null)
                    {
                        result = FindAnchestor(msg.Replies, id);
                    }
                }
                if (result != null) break;
            }
            return result;
        }

        /// <summary>
        ///     Removes a Message from a Message Tree.
        /// </summary>
        /// <param name="messages">The Message Tree.</param>
        /// <param name="msg">The Message to Remove.</param>
        /// <returns>True if the Message has been removed.</returns>
        private static bool RemoveMessage(List<Message> messages, Message msg)
        {
            for (var i = 0; i < messages.Count; i++)
            {
                if (messages.Contains(msg))
                {
                    messages.Remove(msg);
                    return true;
                }
                var tempList = new List<Message>(messages[i].Replies);
                var done = RemoveMessage(tempList, msg);
                if (done)
                {
                    messages[i].Replies = tempList.ToArray();
                    // Message found and removed
                    return true;
                }
            }

            // Message not found
            return false;
        }

        /// <summary>
        ///     Dumps the Message tree of a Page to disk.
        /// </summary>
        /// <param name="page">The Page.</param>
        /// <param name="messages">The Message tree.</param>
        private void DumpMessages(PageInfo page, IEnumerable<Message> messages)
        {
            lock (this)
            {
                var sb = new StringBuilder(5000);
                AppendMessages(messages, -1, sb);
                File.WriteAllText(GetFullPathForMessages(((LocalPageInfo)page).File), sb.ToString());
            }
        }

        /// <summary>
        ///     Appends to a StringBuilder object the branches and leaves of a Message tree.
        /// </summary>
        /// <param name="messages">The Message tree branch to append.</param>
        /// <param name="parent">The ID of the parent of the Message tree or -1.</param>
        /// <param name="sb">The StringBuilder.</param>
        /// <remarks>The methods appends the Messages traversing the tree depht-first, and it is recursive.</remarks>
        private void AppendMessages(IEnumerable<Message> messages, int parent, StringBuilder sb)
        {
            // Depht-first

            // Structure
            // ID|Username|Subject|DateTime|ParentID|Body
            lock (this)
            {
                foreach (var msg in messages)
                {
                    sb.Append(msg.ID.ToString());
                    sb.Append("|");
                    sb.Append(msg.Username);
                    sb.Append("|");
                    sb.Append(Tools.EscapeString(msg.Subject));
                    sb.Append("|");
                    sb.Append(msg.DateTime.ToString("yyyy'/'MM'/'dd' 'HH':'mm':'ss"));
                    sb.Append("|");
                    sb.Append(parent.ToString());
                    sb.Append("|");
                    sb.Append(Tools.EscapeString(msg.Body));
                    sb.Append("\r\n");
                    AppendMessages(msg.Replies, msg.ID, sb);
                }
            }
        }

        /// <summary>
        ///     Extracts an instance of <see cref="T:NavigationPath" /> from a line in the navigation paths file.
        /// </summary>
        /// <param name="fileLine">The line to process.</param>
        /// <returns>The instance of <see cref="T:NavigationPath" /></returns>
        private NavigationPath BuildNavigationPath(string fileLine)
        {
            // Structure
            // Namespace.PathName|Page1|Page2|...
            // First field can be 'Namespace.PathName' or 'PathName'

            var fields = fileLine.Split('|');
            var fullName = fields[0].Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            string nspace, name;

            if (fullName.Length == 1)
            {
                nspace = null;
                name = fullName[0];
            }
            else
            {
                nspace = fullName[0];
                name = fullName[1];
            }

            var result = new NavigationPath(NameTools.GetFullName(nspace, name), this);
            var tempPages = new List<string>(10);
            for (var k = 1; k < fields.Length; k++)
            {
                tempPages.Add(fields[k]);
            }
            result.Pages = tempPages.ToArray();

            return result;
        }

        /// <summary>
        ///     Gets all the Navigation Paths.
        /// </summary>
        /// <returns>The Navigation Paths.</returns>
        private NavigationPath[] GetAllNavigationPaths()
        {
            lock (this)
            {
                var paths = new List<NavigationPath>(10);

                var lines = File.ReadAllText(GetFullPath(NavigationPathsFile))
                    .Replace("\r", "")
                    .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

                // Structure
                // Namespace/PathName|Page1|Page2|...
                // First field can be 'Namespace/PathName', '/PathName' or 'PathName'

                for (var i = 0; i < lines.Length; i++)
                {
                    paths.Add(BuildNavigationPath(lines[i]));
                }

                return paths.ToArray();
            }
        }

        /// <summary>
        ///     Writes an array of Navigation Paths to disk.
        /// </summary>
        /// <param name="paths">The array.</param>
        private void DumpNavigationPaths(NavigationPath[] paths)
        {
            lock (this)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < paths.Length; i++)
                {
                    sb.Append(paths[i].FullName);
                    for (var k = 0; k < paths[i].Pages.Length; k++)
                    {
                        sb.Append("|");
                        sb.Append(paths[i].Pages[k]);
                    }
                    if (i != paths.Length - 1) sb.Append("\r\n");
                }
                File.WriteAllText(GetFullPath(NavigationPathsFile), sb.ToString());
            }
        }

        public PageContentCache GetContentCache(PageInfo pageInfo) => null;

        public void SetContentCache(PageInfo pageInfo, string formattedContent, IEnumerable<string> linkedPages) { }

        public void RemoveContentCache(PageInfo pageInfo) { }

        public void PruneContentCache() { }
    }
}