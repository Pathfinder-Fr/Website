﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ScrewTurn.Wiki.Plugins.SqlServer.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ScrewTurn.Wiki.Plugins.SqlServer.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///create table [Directory] (
        ///	[FullPath] nvarchar(250) not null,
        ///	[Parent] nvarchar(250),
        ///	constraint [PK_Directory] primary key clustered ([FullPath])
        ///)
        ///
        ///create table [File] (
        ///	[Name] nvarchar(200) not null,
        ///	[Directory] nvarchar(250) not null
        ///		constraint [FK_File_Directory] references [Directory]([FullPath])
        ///		on delete cascade on update cascade,
        ///	[Size] bigint not null,
        ///	[Downloads] int not null,
        ///	[LastModified] datetime not null,
        ///	[Data] varbinary(max) not null,
        ///	constraint [PK_File] pri [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string FilesDatabase {
            get {
                return ResourceManager.GetString("FilesDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///create table [Namespace] (
        ///	[Name] nvarchar(100) not null,
        ///	[DefaultPage] nvarchar(200),
        ///	constraint [PK_Namespace] primary key clustered ([Name])
        ///)
        ///
        ///create table [Category](
        ///	[Name] nvarchar(100) not null,
        ///	[Namespace] nvarchar(100) not null
        ///		constraint [FK_Category_Namespace] references [Namespace]([Name])
        ///		on delete cascade on update cascade,
        ///	constraint [PK_Category] primary key clustered ([Name], [Namespace])
        ///)
        ///
        ///create table [Page] (
        ///	[Name] nvarchar(200) not null,
        ///	[Namespace] nvar [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PagesDatabase {
            get {
                return ResourceManager.GetString("PagesDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///drop table [IndexWordMapping]
        ///drop table [IndexWord]
        ///drop table [IndexDocument]
        ///
        ///create table [IndexDocument] (
        ///	[Id] int not null,
        ///	[Name] nvarchar(200) not null
        ///		constraint [UQ_IndexDocument] unique,
        ///	[Title] nvarchar(200) not null,
        ///	[TypeTag] varchar(10) not null,
        ///	[DateTime] datetime not null,
        ///	constraint [PK_IndexDocument] primary key clustered ([Id])
        ///)
        ///
        ///create table [IndexWord] (
        ///	[Id] int not null,
        ///	[Text] nvarchar(200) not null
        ///		constraint [UQ_IndexWord] unique,
        ///	constraint [PK [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PagesDatabase_3000to3001 {
            get {
                return ResourceManager.GetString("PagesDatabase_3000to3001", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///create table [Setting] (
        ///	[Name] varchar(100) not null,
        ///	[Value] nvarchar(4000) not null,
        ///	constraint [PK_Setting] primary key clustered ([Name])
        ///)
        ///
        ///create table [Log] (
        ///	[Id] int not null identity,
        ///	[DateTime] datetime not null,
        ///	[EntryType] char not null,
        ///	[User] nvarchar(100) not null,
        ///	[Message] nvarchar(4000) not null,
        ///	constraint [PK_Log] primary key clustered ([Id])
        ///)
        ///
        ///create table [MetaDataItem] (
        ///	[Name] varchar(100) not null,
        ///	[Tag] nvarchar(100) not null,
        ///	[Data] nvarchar(4000 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string SettingsDatabase {
            get {
                return ResourceManager.GetString("SettingsDatabase", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///create table [User] (
        ///	[Username] nvarchar(100) not null,
        ///	[PasswordHash] varchar(100) not null,
        ///	[DisplayName] nvarchar(150),
        ///	[Email] varchar(100) not null,
        ///	[Active] bit not null,
        ///	[DateTime] datetime not null,
        ///	constraint [PK_User] primary key clustered ([Username])
        ///)
        ///
        ///create table [UserGroup] (
        ///	[Name] nvarchar(100) not null,
        ///	[Description] nvarchar(150),
        ///	constraint [PK_UserGroup] primary key clustered ([Name])
        ///)
        ///
        ///create table [UserGroupMembership] (
        ///	[User] nvarchar(100) not null
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string UsersDatabase {
            get {
                return ResourceManager.GetString("UsersDatabase", resourceCulture);
            }
        }
    }
}
