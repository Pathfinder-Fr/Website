using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Ionic.Zip;
using ScrewTurn.Wiki.AclEngine;
using ScrewTurn.Wiki.PluginFramework;

namespace ScrewTurn.Wiki.BackupRestore
{
    /// <summary>
    ///     Implements a Backup and Restore procedure for settings storage providers.
    /// </summary>
    public static class BackupRestore
    {
        private const string BACKUP_RESTORE_UTILITY_VERSION = "1.0";

        private static VersionFile generateVersionFile(string backupName)
        {
            return new VersionFile
            {
                BackupRestoreVersion = BACKUP_RESTORE_UTILITY_VERSION,
                WikiVersion = typeof (BackupRestore).Assembly.GetName().Version.ToString(),
                BackupName = backupName
            };
        }

        /// <summary>
        ///     Backups all the providers (excluded global settings storage provider).
        /// </summary>
        /// <param name="backupZipFileName">The name of the zip file where to store the backup file.</param>
        /// <param name="plugins">The available plugins.</param>
        /// <param name="settingsStorageProvider">The settings storage provider.</param>
        /// <param name="pagesStorageProviders">The pages storage providers.</param>
        /// <param name="usersStorageProviders">The users storage providers.</param>
        /// <param name="filesStorageProviders">The files storage providers.</param>
        /// <returns><c>true</c> if the backup has been succesfull.</returns>
        public static bool BackupAll(string backupZipFileName, string[] plugins,
            ISettingsStorageProviderV30 settingsStorageProvider, IPagesStorageProviderV30[] pagesStorageProviders,
            IUsersStorageProviderV30[] usersStorageProviders, IFilesStorageProviderV30[] filesStorageProviders)
        {
            var tempPath = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempPath);

            using (var backupZipFile = new ZipFile(backupZipFileName))
            {
                // Find all namespaces
                var namespaces = new List<string>();
                foreach (var pagesStorageProvider in pagesStorageProviders)
                {
                    foreach (var ns in pagesStorageProvider.GetNamespaces())
                    {
                        namespaces.Add(ns.Name);
                    }
                }

                // Backup settings storage provider
                var zipSettingsBackup = Path.Combine(tempPath,
                    "SettingsBackup-" + settingsStorageProvider.GetType().FullName + ".zip");
                BackupSettingsStorageProvider(zipSettingsBackup, settingsStorageProvider, namespaces.ToArray(), plugins);
                backupZipFile.AddFile(zipSettingsBackup, "");

                // Backup pages storage providers
                foreach (var pagesStorageProvider in pagesStorageProviders)
                {
                    var zipPagesBackup = Path.Combine(tempPath,
                        "PagesBackup-" + pagesStorageProvider.GetType().FullName + ".zip");
                    BackupPagesStorageProvider(zipPagesBackup, pagesStorageProvider);
                    backupZipFile.AddFile(zipPagesBackup, "");
                }

                // Backup users storage providers
                foreach (var usersStorageProvider in usersStorageProviders)
                {
                    var zipUsersProvidersBackup = Path.Combine(tempPath,
                        "UsersBackup-" + usersStorageProvider.GetType().FullName + ".zip");
                    BackupUsersStorageProvider(zipUsersProvidersBackup, usersStorageProvider);
                    backupZipFile.AddFile(zipUsersProvidersBackup, "");
                }

                // Backup files storage providers
                foreach (var filesStorageProvider in filesStorageProviders)
                {
                    var zipFilesProviderBackup = Path.Combine(tempPath,
                        "FilesBackup-" + filesStorageProvider.GetType().FullName + ".zip");
                    BackupFilesStorageProvider(zipFilesProviderBackup, filesStorageProvider, pagesStorageProviders);
                    backupZipFile.AddFile(zipFilesProviderBackup, "");
                }
                backupZipFile.Save();
            }

            Directory.Delete(tempPath, true);
            return true;
        }

        /// <summary>
        ///     Backups the specified settings provider.
        /// </summary>
        /// <param name="zipFileName">The zip file name where to store the backup.</param>
        /// <param name="settingsStorageProvider">The source settings provider.</param>
        /// <param name="knownNamespaces">The currently known page namespaces.</param>
        /// <param name="knownPlugins">The currently known plugins.</param>
        /// <returns><c>true</c> if the backup file has been succesfully created.</returns>
        public static bool BackupSettingsStorageProvider(string zipFileName,
            ISettingsStorageProviderV30 settingsStorageProvider, string[] knownNamespaces, string[] knownPlugins)
        {
            var settingsBackup = new SettingsBackup();

            // Settings
            settingsBackup.Settings = (Dictionary<string, string>) settingsStorageProvider.GetAllSettings();

            // Plugins Status and Configuration
            settingsBackup.PluginsFileNames = knownPlugins.ToList();
            var pluginsStatus = new Dictionary<string, bool>();
            var pluginsConfiguration = new Dictionary<string, string>();
            foreach (var plugin in knownPlugins)
            {
                pluginsStatus[plugin] = settingsStorageProvider.GetPluginStatus(plugin);
                pluginsConfiguration[plugin] = settingsStorageProvider.GetPluginConfiguration(plugin);
            }
            settingsBackup.PluginsStatus = pluginsStatus;
            settingsBackup.PluginsConfiguration = pluginsConfiguration;

            // Metadata
            var metadataList = new List<MetaData>();
            // Meta-data (global)
            metadataList.Add(new MetaData
            {
                Item = MetaDataItem.AccountActivationMessage,
                Tag = null,
                Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.AccountActivationMessage, null)
            });
            metadataList.Add(new MetaData
            {
                Item = MetaDataItem.PasswordResetProcedureMessage,
                Tag = null,
                Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.PasswordResetProcedureMessage, null)
            });
            metadataList.Add(new MetaData
            {
                Item = MetaDataItem.LoginNotice,
                Tag = null,
                Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.LoginNotice, null)
            });
            metadataList.Add(new MetaData
            {
                Item = MetaDataItem.PageChangeMessage,
                Tag = null,
                Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.PageChangeMessage, null)
            });
            metadataList.Add(new MetaData
            {
                Item = MetaDataItem.DiscussionChangeMessage,
                Tag = null,
                Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.DiscussionChangeMessage, null)
            });
            // Meta-data (ns-specific)
            var namespacesToProcess = new List<string>();
            namespacesToProcess.Add("");
            namespacesToProcess.AddRange(knownNamespaces);
            foreach (var nspace in namespacesToProcess)
            {
                metadataList.Add(new MetaData
                {
                    Item = MetaDataItem.EditNotice,
                    Tag = nspace,
                    Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.EditNotice, nspace)
                });
                metadataList.Add(new MetaData
                {
                    Item = MetaDataItem.Footer,
                    Tag = nspace,
                    Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.Footer, nspace)
                });
                metadataList.Add(new MetaData
                {
                    Item = MetaDataItem.Header,
                    Tag = nspace,
                    Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.Header, nspace)
                });
                metadataList.Add(new MetaData
                {
                    Item = MetaDataItem.HtmlHead,
                    Tag = nspace,
                    Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.HtmlHead, nspace)
                });
                metadataList.Add(new MetaData
                {
                    Item = MetaDataItem.PageFooter,
                    Tag = nspace,
                    Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.PageFooter, nspace)
                });
                metadataList.Add(new MetaData
                {
                    Item = MetaDataItem.PageHeader,
                    Tag = nspace,
                    Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.PageHeader, nspace)
                });
                metadataList.Add(new MetaData
                {
                    Item = MetaDataItem.Sidebar,
                    Tag = nspace,
                    Content = settingsStorageProvider.GetMetaDataItem(MetaDataItem.Sidebar, nspace)
                });
            }
            settingsBackup.Metadata = metadataList;

            // RecentChanges
            settingsBackup.RecentChanges = settingsStorageProvider.GetRecentChanges().ToList();

            // OutgoingLinks
            settingsBackup.OutgoingLinks = (Dictionary<string, string[]>) settingsStorageProvider.GetAllOutgoingLinks();

            // ACLEntries
            var aclEntries = settingsStorageProvider.AclManager.RetrieveAllEntries().ToList();
            settingsBackup.AclEntries = new List<AclEntryBackup>(aclEntries.Count);
            foreach (var aclEntry in aclEntries)
            {
                settingsBackup.AclEntries.Add(new AclEntryBackup
                {
                    Action = aclEntry.Action,
                    Resource = aclEntry.Resource,
                    Subject = aclEntry.Subject,
                    Value = aclEntry.Value
                });
            }

            var javascriptSerializer = new JavaScriptSerializer();
            javascriptSerializer.MaxJsonLength = javascriptSerializer.MaxJsonLength*10;

            var tempDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            var tempFile = File.Create(Path.Combine(tempDir, "Settings.json"));
            var buffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(settingsBackup));
            tempFile.Write(buffer, 0, buffer.Length);
            tempFile.Close();

            tempFile = File.Create(Path.Combine(tempDir, "Version.json"));
            buffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(generateVersionFile("Settings")));
            tempFile.Write(buffer, 0, buffer.Length);
            tempFile.Close();

            using (var zipFile = new ZipFile())
            {
                zipFile.AddDirectory(tempDir, "");
                zipFile.Save(zipFileName);
            }
            Directory.Delete(tempDir, true);

            return true;
        }

        /// <summary>
        ///     Backups the pages storage provider.
        /// </summary>
        /// <param name="zipFileName">The zip file name where to store the backup.</param>
        /// <param name="pagesStorageProvider">The pages storage provider.</param>
        /// <returns><c>true</c> if the backup file has been succesfully created.</returns>
        public static bool BackupPagesStorageProvider(string zipFileName, IPagesStorageProviderV30 pagesStorageProvider)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            javascriptSerializer.MaxJsonLength = javascriptSerializer.MaxJsonLength*10;

            var tempDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            var nspaces = new List<NamespaceInfo>(pagesStorageProvider.GetNamespaces());
            nspaces.Add(null);
            var namespaceBackupList = new List<NamespaceBackup>(nspaces.Count);
            foreach (var nspace in nspaces)
            {
                // Backup categories
                var categories = pagesStorageProvider.GetCategories(nspace);
                var categoriesBackup = new List<CategoryBackup>(categories.Count);
                foreach (var category in categories)
                {
                    // Add this category to the categoriesBackup list
                    categoriesBackup.Add(new CategoryBackup
                    {
                        FullName = category.FullName,
                        Pages = category.Pages
                    });
                }

                // Backup NavigationPaths
                var navigationPaths = pagesStorageProvider.GetNavigationPaths(nspace);
                var navigationPathsBackup = new List<NavigationPathBackup>(navigationPaths.Count);
                foreach (var navigationPath in navigationPaths)
                {
                    navigationPathsBackup.Add(new NavigationPathBackup
                    {
                        FullName = navigationPath.FullName,
                        Pages = navigationPath.Pages
                    });
                }

                // Add this namespace to the namespaceBackup list
                namespaceBackupList.Add(new NamespaceBackup
                {
                    Name = nspace == null ? "" : nspace.Name,
                    DefaultPageFullName = nspace == null ? "" : nspace.DefaultPage.FullName,
                    Categories = categoriesBackup,
                    NavigationPaths = navigationPathsBackup
                });

                // Backup pages (one json file for each page containing a maximum of 100 revisions)
                var pages = pagesStorageProvider.GetPages(nspace);
                foreach (var page in pages)
                {
                    var pageContent = pagesStorageProvider.GetContent(page);
                    var pageBackup = new PageBackup();
                    pageBackup.FullName = page.FullName;
                    pageBackup.CreationDateTime = page.CreationDateTime;
                    pageBackup.LastModified = pageContent.LastModified;
                    pageBackup.Content = pageContent.Content;
                    pageBackup.Comment = pageContent.Comment;
                    pageBackup.Description = pageContent.Description;
                    pageBackup.Keywords = pageContent.Keywords;
                    pageBackup.Title = pageContent.Title;
                    pageBackup.User = pageContent.User;
                    pageBackup.LinkedPages = pageContent.LinkedPages;
                    pageBackup.Categories = (from c in pagesStorageProvider.GetCategoriesForPage(page)
                        select c.FullName).ToArray();

                    // Backup the 100 most recent versions of the page
                    var pageContentBackupList = new List<PageRevisionBackup>();
                    var revisions = pagesStorageProvider.GetBackups(page);
                    for (var i = revisions.Length - 1; i > revisions.Length - 100 && i >= 0; i--)
                    {
                        var pageRevision = pagesStorageProvider.GetBackupContent(page, revisions[i]);
                        var pageContentBackup = new PageRevisionBackup
                        {
                            Revision = revisions[i],
                            Content = pageRevision.Content,
                            Comment = pageRevision.Comment,
                            Description = pageRevision.Description,
                            Keywords = pageRevision.Keywords,
                            Title = pageRevision.Title,
                            User = pageRevision.User,
                            LastModified = pageRevision.LastModified
                        };
                        pageContentBackupList.Add(pageContentBackup);
                    }
                    pageBackup.Revisions = pageContentBackupList;

                    // Backup draft of the page
                    var draft = pagesStorageProvider.GetDraft(page);
                    if (draft != null)
                    {
                        pageBackup.Draft = new PageRevisionBackup
                        {
                            Content = draft.Content,
                            Comment = draft.Comment,
                            Description = draft.Description,
                            Keywords = draft.Keywords,
                            Title = draft.Title,
                            User = draft.User,
                            LastModified = draft.LastModified
                        };
                    }

                    // Backup all messages of the page
                    var messageBackupList = new List<MessageBackup>();
                    foreach (var message in pagesStorageProvider.GetMessages(page))
                    {
                        messageBackupList.Add(BackupMessage(message));
                    }
                    pageBackup.Messages = messageBackupList;

                    var tempFile = File.Create(Path.Combine(tempDir, page.FullName + ".json"));
                    var buffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(pageBackup));
                    tempFile.Write(buffer, 0, buffer.Length);
                    tempFile.Close();
                }
            }
            var tempNamespacesFile = File.Create(Path.Combine(tempDir, "Namespaces.json"));
            var namespacesBuffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(namespaceBackupList));
            tempNamespacesFile.Write(namespacesBuffer, 0, namespacesBuffer.Length);
            tempNamespacesFile.Close();

            // Backup content templates
            var contentTemplates = pagesStorageProvider.GetContentTemplates();
            var contentTemplatesBackup = new List<ContentTemplateBackup>(contentTemplates.Length);
            foreach (var contentTemplate in contentTemplates)
            {
                contentTemplatesBackup.Add(new ContentTemplateBackup
                {
                    Name = contentTemplate.Name,
                    Content = contentTemplate.Content
                });
            }
            var tempContentTemplatesFile = File.Create(Path.Combine(tempDir, "ContentTemplates.json"));
            var contentTemplateBuffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(contentTemplatesBackup));
            tempContentTemplatesFile.Write(contentTemplateBuffer, 0, contentTemplateBuffer.Length);
            tempContentTemplatesFile.Close();

            // Backup Snippets
            var snippets = pagesStorageProvider.GetSnippets();
            var snippetsBackup = new List<SnippetBackup>(snippets.Count);
            foreach (var snippet in snippets)
            {
                snippetsBackup.Add(new SnippetBackup
                {
                    Name = snippet.Name,
                    Content = snippet.Content
                });
            }
            var tempSnippetsFile = File.Create(Path.Combine(tempDir, "Snippets.json"));
            var snippetBuffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(snippetsBackup));
            tempSnippetsFile.Write(snippetBuffer, 0, snippetBuffer.Length);
            tempSnippetsFile.Close();

            var tempVersionFile = File.Create(Path.Combine(tempDir, "Version.json"));
            var versionBuffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(generateVersionFile("Pages")));
            tempVersionFile.Write(versionBuffer, 0, versionBuffer.Length);
            tempVersionFile.Close();

            using (var zipFile = new ZipFile())
            {
                zipFile.AddDirectory(tempDir, "");
                zipFile.Save(zipFileName);
            }
            Directory.Delete(tempDir, true);

            return true;
        }

        // Backup a message with a recursive function to backup all its replies.
        private static MessageBackup BackupMessage(Message message)
        {
            var messageBackup = new MessageBackup
            {
                Id = message.ID,
                Subject = message.Subject,
                Body = message.Body,
                DateTime = message.DateTime,
                Username = message.Username
            };
            var repliesBackup = new List<MessageBackup>(message.Replies.Length);
            foreach (var reply in message.Replies)
            {
                repliesBackup.Add(BackupMessage(reply));
            }
            messageBackup.Replies = repliesBackup;
            return messageBackup;
        }

        /// <summary>
        ///     Backups the users storage provider.
        /// </summary>
        /// <param name="zipFileName">The zip file name where to store the backup.</param>
        /// <param name="usersStorageProvider">The users storage provider.</param>
        /// <returns><c>true</c> if the backup file has been succesfully created.</returns>
        public static bool BackupUsersStorageProvider(string zipFileName, IUsersStorageProviderV30 usersStorageProvider)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            javascriptSerializer.MaxJsonLength = javascriptSerializer.MaxJsonLength*10;

            var tempDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            // Backup users
            var users = usersStorageProvider.GetUsers().ToList();
            var usersBackup = new List<UserBackup>(users.Count);
            foreach (var user in users)
            {
                usersBackup.Add(new UserBackup
                {
                    Username = user.Username,
                    Active = user.Active,
                    DateTime = user.DateTime,
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Groups = user.Groups.ToArray(),
                    UserData = usersStorageProvider.RetrieveAllUserData(user)
                });
            }
            var tempFile = File.Create(Path.Combine(tempDir, "Users.json"));
            var buffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(usersBackup));
            tempFile.Write(buffer, 0, buffer.Length);
            tempFile.Close();

            // Backup UserGroups
            var userGroups = usersStorageProvider.GetUserGroups().ToList();
            var userGroupsBackup = new List<UserGroupBackup>(userGroups.Count);
            foreach (var userGroup in userGroups)
            {
                userGroupsBackup.Add(new UserGroupBackup
                {
                    Name = userGroup.Name,
                    Description = userGroup.Description
                });
            }

            tempFile = File.Create(Path.Combine(tempDir, "Groups.json"));
            buffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(userGroupsBackup));
            tempFile.Write(buffer, 0, buffer.Length);
            tempFile.Close();

            tempFile = File.Create(Path.Combine(tempDir, "Version.json"));
            buffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(generateVersionFile("Users")));
            tempFile.Write(buffer, 0, buffer.Length);
            tempFile.Close();


            using (var zipFile = new ZipFile())
            {
                zipFile.AddDirectory(tempDir, "");
                zipFile.Save(zipFileName);
            }
            Directory.Delete(tempDir, true);

            return true;
        }

        /// <summary>
        ///     Backups the files storage provider.
        /// </summary>
        /// <param name="zipFileName">The zip file name where to store the backup.</param>
        /// <param name="filesStorageProvider">The files storage provider.</param>
        /// <param name="pagesStorageProviders">The pages storage providers.</param>
        /// <returns><c>true</c> if the backup file has been succesfully created.</returns>
        public static bool BackupFilesStorageProvider(string zipFileName, IFilesStorageProviderV30 filesStorageProvider,
            IPagesStorageProviderV30[] pagesStorageProviders)
        {
            var javascriptSerializer = new JavaScriptSerializer();
            javascriptSerializer.MaxJsonLength = javascriptSerializer.MaxJsonLength*10;

            var tempDir = Path.Combine(Environment.GetEnvironmentVariable("TEMP"), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);

            var directoriesBackup = BackupDirectory(filesStorageProvider, tempDir, null);
            var tempFile = File.Create(Path.Combine(tempDir, "Files.json"));
            var buffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(directoriesBackup));
            tempFile.Write(buffer, 0, buffer.Length);
            tempFile.Close();


            // Backup Pages Attachments
            var pagesWithAttachment = filesStorageProvider.GetPagesWithAttachments();
            foreach (var pageWithAttachment in pagesWithAttachment)
            {
                var pageInfo = FindPageInfo(pageWithAttachment, pagesStorageProviders);
                if (pageInfo != null)
                {
                    var attachments = filesStorageProvider.ListPageAttachments(pageInfo);
                    var attachmentsBackup = new List<AttachmentBackup>(attachments.Count);
                    foreach (var attachment in attachments)
                    {
                        var attachmentDetails = filesStorageProvider.GetPageAttachmentDetails(pageInfo, attachment);
                        attachmentsBackup.Add(new AttachmentBackup
                        {
                            Name = attachment,
                            PageFullName = pageWithAttachment,
                            LastModified = attachmentDetails.LastModified,
                            Size = attachmentDetails.Size
                        });
                        using (var stream = new MemoryStream())
                        {
                            filesStorageProvider.RetrievePageAttachment(pageInfo, attachment, stream, false);
                            stream.Seek(0, SeekOrigin.Begin);
                            var tempBuffer = new byte[stream.Length];
                            stream.Read(tempBuffer, 0, (int) stream.Length);

                            var dir =
                                Directory.CreateDirectory(Path.Combine(tempDir,
                                    Path.Combine("__attachments", pageInfo.FullName)));
                            tempFile = File.Create(Path.Combine(dir.FullName, attachment));
                            tempFile.Write(tempBuffer, 0, tempBuffer.Length);
                            tempFile.Close();
                        }
                    }
                    tempFile =
                        File.Create(Path.Combine(tempDir,
                            Path.Combine("__attachments", Path.Combine(pageInfo.FullName, "Attachments.json"))));
                    buffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(attachmentsBackup));
                    tempFile.Write(buffer, 0, buffer.Length);
                    tempFile.Close();
                }
            }

            tempFile = File.Create(Path.Combine(tempDir, "Version.json"));
            buffer = Encoding.Unicode.GetBytes(javascriptSerializer.Serialize(generateVersionFile("Files")));
            tempFile.Write(buffer, 0, buffer.Length);
            tempFile.Close();

            using (var zipFile = new ZipFile())
            {
                zipFile.AddDirectory(tempDir, "");
                zipFile.Save(zipFileName);
            }
            Directory.Delete(tempDir, true);

            return true;
        }

        private static PageInfo FindPageInfo(string pageWithAttachment, IPagesStorageProviderV30[] pagesStorageProviders)
        {
            foreach (var pagesStorageProvider in pagesStorageProviders)
            {
                var pageInfo = pagesStorageProvider.GetPage(pageWithAttachment);
                if (pageInfo != null) return pageInfo;
            }
            return null;
        }

        private static DirectoryBackup BackupDirectory(IFilesStorageProviderV30 filesStorageProvider, string zipFileName,
            string directory)
        {
            var directoryBackup = new DirectoryBackup();

            var files = filesStorageProvider.ListFiles(directory);
            var filesBackup = new List<FileBackup>(files.Count());
            foreach (var file in files)
            {
                var fileDetails = filesStorageProvider.GetFileDetails(file);
                filesBackup.Add(new FileBackup
                {
                    Name = file,
                    Size = fileDetails.Size,
                    LastModified = fileDetails.LastModified
                });

                var tempFile = File.Create(Path.Combine(zipFileName.Trim('/').Trim('\\'), file.Trim('/').Trim('\\')));
                using (var stream = new MemoryStream())
                {
                    filesStorageProvider.RetrieveFile(file, stream, false);
                    stream.Seek(0, SeekOrigin.Begin);
                    var buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    tempFile.Write(buffer, 0, buffer.Length);
                    tempFile.Close();
                }
            }
            directoryBackup.Name = directory;
            directoryBackup.Files = filesBackup;

            var directories = filesStorageProvider.ListDirectories(directory).ToList();
            var subdirectoriesBackup = new List<DirectoryBackup>(directories.Count);
            foreach (var d in directories)
            {
                subdirectoriesBackup.Add(BackupDirectory(filesStorageProvider, zipFileName, d));
            }
            directoryBackup.SubDirectories = subdirectoriesBackup;

            return directoryBackup;
        }
    }

    internal class SettingsBackup
    {
        public Dictionary<string, string> Settings { get; set; }
        public List<string> PluginsFileNames { get; set; }
        public Dictionary<string, bool> PluginsStatus { get; set; }
        public Dictionary<string, string> PluginsConfiguration { get; set; }
        public List<MetaData> Metadata { get; set; }
        public List<RecentChange> RecentChanges { get; set; }
        public Dictionary<string, string[]> OutgoingLinks { get; set; }
        public List<AclEntryBackup> AclEntries { get; set; }
    }

    internal class AclEntryBackup
    {
        public Value Value { get; set; }
        public string Subject { get; set; }
        public string Resource { get; set; }
        public string Action { get; set; }
    }

    internal class MetaData
    {
        public MetaDataItem Item { get; set; }
        public string Tag { get; set; }
        public string Content { get; set; }
    }

    internal class GlobalSettingsBackup
    {
        public Dictionary<string, string> Settings { get; set; }
        public List<string> pluginsFileNames { get; set; }
    }

    internal class PageBackup
    {
        public string FullName { get; set; }
        public DateTime CreationDateTime { get; set; }
        public DateTime LastModified { get; set; }
        public string Content { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public IList<string> Keywords { get; set; }
        public string Title { get; set; }
        public string User { get; set; }
        public IList<string> LinkedPages { get; set; }
        public List<PageRevisionBackup> Revisions { get; set; }
        public PageRevisionBackup Draft { get; set; }
        public List<MessageBackup> Messages { get; set; }
        public string[] Categories { get; set; }
    }

    internal class PageRevisionBackup
    {
        public string Content { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public IList<string> Keywords { get; set; }
        public string Title { get; set; }
        public string User { get; set; }
        public DateTime LastModified { get; set; }
        public int Revision { get; set; }
    }

    internal class NamespaceBackup
    {
        public string Name { get; set; }
        public string DefaultPageFullName { get; set; }
        public List<CategoryBackup> Categories { get; set; }
        public List<NavigationPathBackup> NavigationPaths { get; set; }
    }

    internal class CategoryBackup
    {
        public string FullName { get; set; }
        public IList<string> Pages { get; set; }
    }

    internal class ContentTemplateBackup
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    internal class MessageBackup
    {
        public List<MessageBackup> Replies { get; set; }
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public DateTime DateTime { get; set; }
        public string Username { get; set; }
    }

    internal class NavigationPathBackup
    {
        public string FullName { get; set; }
        public string[] Pages { get; set; }
    }

    internal class SnippetBackup
    {
        public string Name { get; set; }
        public string Content { get; set; }
    }

    internal class UserBackup
    {
        public string Username { get; set; }
        public bool Active { get; set; }
        public DateTime DateTime { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string[] Groups { get; set; }
        public IDictionary<string, string> UserData { get; set; }
    }

    internal class UserGroupBackup
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    internal class DirectoryBackup
    {
        public List<FileBackup> Files { get; set; }
        public List<DirectoryBackup> SubDirectories { get; set; }
        public string Name { get; set; }
    }

    internal class FileBackup
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public DateTime LastModified { get; set; }
        public string DirectoryName { get; set; }
    }

    internal class VersionFile
    {
        public string BackupRestoreVersion { get; set; }
        public string WikiVersion { get; set; }
        public string BackupName { get; set; }
    }

    internal class AttachmentBackup
    {
        public string Name { get; set; }
        public string PageFullName { get; set; }
        public DateTime LastModified { get; set; }
        public long Size { get; set; }
    }
}