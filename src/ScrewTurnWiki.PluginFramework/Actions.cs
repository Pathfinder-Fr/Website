using System.Collections.Generic;
using ScrewTurn.Wiki.AclEngine;

namespace ScrewTurn.Wiki
{
    /// <summary>
    ///     Contains actions for resources.
    /// </summary>
    public static class Actions
    {
        /// <summary>
        ///     The full control action.
        /// </summary>
        public const string FullControl = AclEntry.FullControlAction;

        /// <summary>
        ///     Contains actions for global resources.
        /// </summary>
        public static class ForGlobals
        {
            /// <summary>
            ///     The master prefix for global resources ('G').
            /// </summary>
            public const string ResourceMasterPrefix = "G";

            /// <summary>
            ///     Manage user accounts.
            /// </summary>
            public const string ManageAccounts = "Man_Acc";

            /// <summary>
            ///     Manage user groups.
            /// </summary>
            public const string ManageGroups = "Man_Grp";

            /// <summary>
            ///     Manage pages and categories.
            /// </summary>
            public const string ManagePagesAndCategories = "Man_PgCat";

            /// <summary>
            ///     Manage page discussions.
            /// </summary>
            public const string ManageDiscussions = "Man_Disc";

            /// <summary>
            ///     Manage namespaces.
            /// </summary>
            public const string ManageNamespaces = "Man_Ns";

            /// <summary>
            ///     Manage configuration.
            /// </summary>
            public const string ManageConfiguration = "Man_Conf";

            /// <summary>
            ///     Manage providers.
            /// </summary>
            public const string ManageProviders = "Man_Prov";

            /// <summary>
            ///     Manage files.
            /// </summary>
            public const string ManageFiles = "Man_Files";

            /// <summary>
            ///     Manage snippets and templates.
            /// </summary>
            public const string ManageSnippetsAndTemplates = "Man_Snips_Temps";

            /// <summary>
            ///     Manage navigation paths.
            /// </summary>
            public const string ManageNavigationPaths = "Man_NavPath";

            /// <summary>
            ///     Manage meta-files.
            /// </summary>
            public const string ManageMetaFiles = "Man_MetaFiles";

            /// <summary>
            ///     Manage permissions.
            /// </summary>
            public const string ManagePermissions = "Man_Perms";

            /// <summary>
            ///     Gets an array containing all actions.
            /// </summary>
            public static readonly string[] All =
            {
                ManageAccounts,
                ManageGroups,
                ManagePagesAndCategories,
                ManageDiscussions,
                ManageNamespaces,
                ManageConfiguration,
                ManageProviders,
                ManageFiles,
                ManageSnippetsAndTemplates,
                ManageNavigationPaths,
                ManageMetaFiles,
                ManagePermissions
            };

            /// <summary>
            ///     Gets the full name of an action.
            /// </summary>
            /// <param name="name">The internal name.</param>
            /// <returns>The full name.</returns>
            public static string GetFullName(string name)
            {
                if (name == FullControl) return Exchanger.ResourceExchanger.GetResource("Action_FullControl");
                return Exchanger.ResourceExchanger.GetResource("Action_" + name);
            }
        }

        /// <summary>
        ///     Contains actions for namespaces.
        /// </summary>
        public static class ForNamespaces
        {
            /// <summary>
            ///     The master prefix for namespaces ('N.').
            /// </summary>
            public const string ResourceMasterPrefix = "N.";

            /// <summary>
            ///     Read pages.
            /// </summary>
            public const string ReadPages = "Rd_Pg";

            /// <summary>
            ///     Create pages.
            /// </summary>
            public const string CreatePages = "Crt_Pg";

            /// <summary>
            ///     Modify pages.
            /// </summary>
            public const string ModifyPages = "Mod_Pg";

            /// <summary>
            ///     Delete pages.
            /// </summary>
            public const string DeletePages = "Del_Pg";

            /// <summary>
            ///     Manage pages.
            /// </summary>
            public const string ManagePages = "Man_Pg";

            /// <summary>
            ///     Read page discussions.
            /// </summary>
            public const string ReadDiscussion = "Rd_Disc";

            /// <summary>
            ///     Post messages in page discussions.
            /// </summary>
            public const string PostDiscussion = "Pst_Disc";

            /// <summary>
            ///     Manage messages in page discussions.
            /// </summary>
            public const string ManageDiscussion = "Man_Disc";

            /// <summary>
            ///     Manage categories.
            /// </summary>
            public const string ManageCategories = "Man_Cat";

            /// <summary>
            ///     Download attachments.
            /// </summary>
            public const string DownloadAttachments = "Down_Attn";

            /// <summary>
            ///     Upload attachments.
            /// </summary>
            public const string UploadAttachments = "Up_Attn";

            /// <summary>
            ///     Delete attachments.
            /// </summary>
            public const string DeleteAttachments = "Del_Attn";

            /// <summary>
            ///     The local escalation policies.
            /// </summary>
            public static readonly Dictionary<string, string[]> LocalEscalators = new Dictionary<string, string[]>
            {
                {ReadPages, new[] {CreatePages, ModifyPages, DeletePages, ManagePages}},
                {ModifyPages, new[] {ManagePages}},
                {DeletePages, new[] {ManagePages}},
                {CreatePages, new[] {ManagePages}},
                {ReadDiscussion, new[] {PostDiscussion, ManageDiscussion}},
                {PostDiscussion, new[] {ManageDiscussion}},
                {ManageDiscussion, new[] {ManagePages}},
                {DownloadAttachments, new[] {UploadAttachments, DeleteAttachments}},
                {UploadAttachments, new[] {DeleteAttachments}}
            };

            /// <summary>
            ///     The global escalation policies.
            /// </summary>
            public static readonly Dictionary<string, string[]> GlobalEscalators = new Dictionary<string, string[]>
            {
                {ReadPages, new[] {ForGlobals.ManagePagesAndCategories, ForGlobals.ManageNamespaces}},
                {CreatePages, new[] {ForGlobals.ManagePagesAndCategories, ForGlobals.ManageNamespaces}},
                {ModifyPages, new[] {ForGlobals.ManagePagesAndCategories, ForGlobals.ManageNamespaces}},
                {ReadDiscussion, new[] {ForGlobals.ManageDiscussions}},
                {PostDiscussion, new[] {ForGlobals.ManageDiscussions}},
                {ManageDiscussion, new[] {ForGlobals.ManageDiscussions}},
                {DeletePages, new[] {ForGlobals.ManagePagesAndCategories, ForGlobals.ManageNamespaces}},
                {ManagePages, new[] {ForGlobals.ManagePagesAndCategories, ForGlobals.ManageNamespaces}},
                {ManageCategories, new[] {ForGlobals.ManagePagesAndCategories}},
                {DownloadAttachments, new[] {ForGlobals.ManageFiles}},
                {UploadAttachments, new[] {ForGlobals.ManageFiles}},
                {DeleteAttachments, new[] {ForGlobals.ManageFiles}}
            };

            /// <summary>
            ///     Gets an array containing all actions.
            /// </summary>
            public static readonly string[] All =
            {
                ReadPages,
                CreatePages,
                ModifyPages,
                DeletePages,
                ManagePages,
                ReadDiscussion,
                PostDiscussion,
                ManageDiscussion,
                ManageCategories,
                DownloadAttachments,
                UploadAttachments,
                DeleteAttachments
            };

            /// <summary>
            ///     Gets the full name of an action.
            /// </summary>
            /// <param name="name">The internal name.</param>
            /// <returns>The full name.</returns>
            public static string GetFullName(string name)
            {
                if (name == FullControl) return Exchanger.ResourceExchanger.GetResource("Action_FullControl");
                return Exchanger.ResourceExchanger.GetResource("Action_" + name);
            }
        }

        /// <summary>
        ///     Contains actions for pages.
        /// </summary>
        public static class ForPages
        {
            /// <summary>
            ///     The master prefix for pages ('P.').
            /// </summary>
            public const string ResourceMasterPrefix = "P.";

            /// <summary>
            ///     Read the page.
            /// </summary>
            public const string ReadPage = "Rd_1Pg";

            /// <summary>
            ///     Modify the page.
            /// </summary>
            public const string ModifyPage = "Mod_1Pg";

            /// <summary>
            ///     Manage the page.
            /// </summary>
            public const string ManagePage = "Man_1Pg";

            /// <summary>
            ///     Read page discussion.
            /// </summary>
            public const string ReadDiscussion = "Rd_1Disc";

            /// <summary>
            ///     Post messages in page discussion.
            /// </summary>
            public const string PostDiscussion = "Pst_1Disc";

            /// <summary>
            ///     Manage page discussion.
            /// </summary>
            public const string ManageDiscussion = "Man_1Disc";

            /// <summary>
            ///     Manage the categories of the page.
            /// </summary>
            public const string ManageCategories = "Man_1Cat";

            /// <summary>
            ///     Download attachments.
            /// </summary>
            public const string DownloadAttachments = "Down_1Attn";

            /// <summary>
            ///     Upload attachments.
            /// </summary>
            public const string UploadAttachments = "Up_1Attn";

            /// <summary>
            ///     Delete attachments.
            /// </summary>
            public const string DeleteAttachments = "Del_1Attn";

            /// <summary>
            ///     The local escalation policies.
            /// </summary>
            public static readonly Dictionary<string, string[]> LocalEscalators = new Dictionary<string, string[]>
            {
                {ReadPage, new[] {ModifyPage, ManagePage}},
                {ModifyPage, new[] {ManagePage}},
                {ReadDiscussion, new[] {PostDiscussion, ManageDiscussion}},
                {PostDiscussion, new[] {ManageDiscussion}},
                {ManageDiscussion, new[] {ManagePage}},
                {DownloadAttachments, new[] {UploadAttachments, DeleteAttachments}},
                {UploadAttachments, new[] {DeleteAttachments}}
            };

            /// <summary>
            ///     The namespace escalation policies.
            /// </summary>
            public static readonly Dictionary<string, string[]> NamespaceEscalators = new Dictionary<string, string[]>
            {
                {
                    ReadPage,
                    new[]
                    {
                        ForNamespaces.ReadPages, ForNamespaces.ModifyPages, ForNamespaces.ManagePages,
                        ForNamespaces.CreatePages, ForNamespaces.DeletePages
                    }
                },
                {
                    ModifyPage,
                    new[]
                    {
                        ForNamespaces.CreatePages, ForNamespaces.ModifyPages, ForNamespaces.ManagePages,
                        ForNamespaces.CreatePages, ForNamespaces.DeletePages
                    }
                },
                {ManagePage, new[] {ForNamespaces.ManagePages}},
                {
                    ReadDiscussion,
                    new[] {ForNamespaces.ReadDiscussion, ForNamespaces.PostDiscussion, ForNamespaces.ManageDiscussion}
                },
                {PostDiscussion, new[] {ForNamespaces.PostDiscussion, ForNamespaces.ManageDiscussion}},
                {ManageDiscussion, new[] {ForNamespaces.ManageDiscussion}},
                {
                    DownloadAttachments,
                    new[]
                    {
                        ForNamespaces.DownloadAttachments, ForNamespaces.UploadAttachments, ForNamespaces.DeleteAttachments
                    }
                },
                {ManageCategories, new[] {ForNamespaces.ManageCategories}},
                {UploadAttachments, new[] {ForNamespaces.UploadAttachments}},
                {DeleteAttachments, new[] {ForNamespaces.DeleteAttachments}}
            };

            /// <summary>
            ///     The global escalation policies.
            /// </summary>
            public static readonly Dictionary<string, string[]> GlobalEscalators = new Dictionary<string, string[]>
            {
                {ReadPage, new[] {ForGlobals.ManagePagesAndCategories, ForGlobals.ManageNamespaces}},
                {ModifyPage, new[] {ForGlobals.ManagePagesAndCategories, ForGlobals.ManageNamespaces}},
                {ManagePage, new[] {ForGlobals.ManagePagesAndCategories, ForGlobals.ManageNamespaces}},
                {ReadDiscussion, new[] {ForGlobals.ManageDiscussions}},
                {PostDiscussion, new[] {ForGlobals.ManageDiscussions}},
                {ManageDiscussion, new[] {ForGlobals.ManageDiscussions}},
                {ManageCategories, new[] {ForGlobals.ManagePagesAndCategories}},
                {DownloadAttachments, new[] {ForGlobals.ManageFiles}},
                {UploadAttachments, new[] {ForGlobals.ManageFiles}},
                {DeleteAttachments, new[] {ForGlobals.ManageFiles}}
            };

            /// <summary>
            ///     Gets an array containing all actions.
            /// </summary>
            public static readonly string[] All =
            {
                ReadPage,
                ModifyPage,
                ManagePage,
                ReadDiscussion,
                PostDiscussion,
                ManageDiscussion,
                ManageCategories,
                DownloadAttachments,
                UploadAttachments,
                DeleteAttachments
            };

            /// <summary>
            ///     Gets the full name of an action.
            /// </summary>
            /// <param name="name">The internal name.</param>
            /// <returns>The full name.</returns>
            public static string GetFullName(string name)
            {
                if (name == FullControl) return Exchanger.ResourceExchanger.GetResource("Action_FullControl");
                return Exchanger.ResourceExchanger.GetResource("Action_" + name);
            }
        }

        /// <summary>
        ///     Contains actions for file directories.
        /// </summary>
        public static class ForDirectories
        {
            /// <summary>
            ///     The master prefix for directories ('D.').
            /// </summary>
            public const string ResourceMasterPrefix = "D.";

            /// <summary>
            ///     List files and directories.
            /// </summary>
            public const string List = "List";

            /// <summary>
            ///     Download files.
            /// </summary>
            public const string DownloadFiles = "Down_Files";

            /// <summary>
            ///     Upload files.
            /// </summary>
            public const string UploadFiles = "Up_Files";

            /// <summary>
            ///     Delete files.
            /// </summary>
            public const string DeleteFiles = "Del_Files";

            /// <summary>
            ///     Create directories.
            /// </summary>
            public const string CreateDirectories = "Crt_Dirs";

            /// <summary>
            ///     Delete directories.
            /// </summary>
            public const string DeleteDirectories = "Del_Dirs";

            /// <summary>
            ///     The local escalation policies.
            /// </summary>
            public static readonly Dictionary<string, string[]> LocalEscalators = new Dictionary<string, string[]>
            {
                {List, new[] {DownloadFiles, UploadFiles, DeleteFiles, CreateDirectories, DeleteDirectories}},
                {DownloadFiles, new[] {UploadFiles, DeleteFiles, CreateDirectories, DeleteDirectories}},
                {UploadFiles, new[] {DeleteFiles}},
                {CreateDirectories, new[] {DeleteDirectories}}
            };

            /// <summary>
            ///     The global escalation policies.
            /// </summary>
            public static readonly Dictionary<string, string[]> GlobalEscalators = new Dictionary<string, string[]>
            {
                {List, new[] {ForGlobals.ManageFiles}},
                {DownloadFiles, new[] {ForGlobals.ManageFiles}},
                {UploadFiles, new[] {ForGlobals.ManageFiles}},
                {DeleteFiles, new[] {ForGlobals.ManageFiles}},
                {CreateDirectories, new[] {ForGlobals.ManageFiles}},
                {DeleteDirectories, new[] {ForGlobals.ManageFiles}}
            };

            /// <summary>
            ///     Gets an array containing all actions.
            /// </summary>
            public static readonly string[] All =
            {
                List,
                DownloadFiles,
                UploadFiles,
                DeleteFiles,
                CreateDirectories,
                DeleteDirectories
            };

            /// <summary>
            ///     Gets the full name of an action.
            /// </summary>
            /// <param name="name">The internal name.</param>
            /// <returns>The full name.</returns>
            public static string GetFullName(string name)
            {
                if (name == FullControl) return Exchanger.ResourceExchanger.GetResource("Action_FullControl");
                return Exchanger.ResourceExchanger.GetResource("Action_" + name);
            }
        }
    }
}