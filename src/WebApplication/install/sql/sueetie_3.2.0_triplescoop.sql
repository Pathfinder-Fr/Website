/* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- */
/* 
	
	INSTALLATION OF TRIPLE SCOOP PRODUCT AND LICENSING SUPPORT

	RELEASE 3.1 -----------------------------------------

	Processes: 
	
	CREATE TABLE [dbo].[SuLicensing_LicenseTypes]
	CREATE TABLE [dbo].[SuLicensing_Packages]
	INSERT INTO SuLicensing_licensetypes
	CREATE TABLE [dbo].[SuLicensing_PackageTypes]
	INSERT INTO SuLicensing_packagetypes
	INSERT INTO SuLicensing_Packages
	CREATE VIEW [dbo].[SuLicensing_vw_Packages]
	CREATE PROCEDURE [dbo].[SuLicensing_Package_Update]
	CREATE PROCEDURE [dbo].[SuLicensing_Package_Reset]


	CREATE TABLE [dbo].[SuAddons_Settings]
	CREATE TABLE [dbo].[SuAddons_BlockedIpCountries]
	CREATE TABLE [dbo].[SuAddons_BlockedIPImports]
	CREATE TABLE [dbo].[SuAddons_BlockedIpRanges]
	INSERT INTO SuAddons_BlockedIpCountries
	CREATE VIEW [dbo].[SuAddons_vw_BlockedIpRanges]
    CREATE VIEW [dbo].[SuAddons_vw_BlockedIpImports]
	CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRange_Add]
	CREATE PROCEDURE [dbo].[SuAddons_BlockedIpImport_Add]
	CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRanges_Prep]
	CREATE PROCEDURE [dbo].[SuAddons_BlockedIpCountry_Update]
	CREATE PROCEDURE [dbo].[SuAddons_BlockedIpCountry_Add]
	CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRange_Update]

	CREATE TABLE [dbo].[SuAddons_CrawlerAgents]
	CREATE PROCEDURE [dbo].[SuAddons_CrawlerAgent_Add]
	CREATE PROCEDURE [dbo].[SuAddons_CrawlerAgent_Update]

	CREATE TABLE [dbo].[SuAnalytics_ContactTypes]
	INSERT INTO SuAnalytics_ContactTypes
	    
    CREATE VIEW [dbo].[Sueetie_vw_Requests]
    CREATE PROCEDURE [dbo].[Sueetie_Request_Add]

	CREATE PROCEDURE [dbo].[SuAddons_RequestAgents_Get]
	CREATE PROCEDURE [dbo].[SuAddons_RequestAgents_Delete]
	CREATE PROCEDURE [dbo].[SuAddons_RequestIPs_Get] 
	CREATE PROCEDURE [dbo].[SuAddons_RequestIPs_Delete]
	CREATE PROCEDURE [dbo].[SuAddons_Setting_Update]

	CREATE PROCEDURE [dbo].[SuAnalytics_User_Filter]
	CREATE TABLE [dbo].[SuAnalytics_FilteredUrls]
	CREATE PROCEDURE [dbo].[SuAnalytics_AnonymousUrls_Get]
	CREATE PROCEDURE [dbo].[SuAnalytics_AnonymousUrlDetails_Get]

	CREATE PROCEDURE [dbo].[SuAnalytics_Logs_Clear]

	CREATE TABLE [dbo].[SuAnalytics_PageRules]
	CREATE PROCEDURE [dbo].[SuAnalytics_Pages_Delete]
	CREATE PROCEDURE [dbo].[SuAnalytics_PageRule_Create]
	CREATE PROCEDURE [dbo].[SuAnalytics_PageRule_Update]

	CREATE PROCEDURE [dbo].[SuAddons_AgentRequests_Get]
	CREATE VIEW [dbo].[SuAnalytics_vw_Pages]

	CREATE TABLE [dbo].[SuAnalytics_Settings]
	CREATE PROCEDURE [dbo].[SuAnalytics_Setting_Update]

	CREATE PROCEDURE [dbo].[SuAnalytics_Refreshes_Remove] 

	INSERT INTO [dbo].[SuAddons_CrawlerAgents]
	INSERT INTO [dbo].[SuAnalytics_FilteredUrls]
	INSERT INTO [dbo].[SuAnalytics_PageRules]

	RELEASE 3.2 --------------------------------------------

	CREATE MARKETPLACE OBJECTS (below)

	--Post Marketplace Object Creation

    INSERT INTO [dbo].[SuCommerce_ActionTypes]
    INSERT INTO [dbo].[SuCommerce_CartLinks]
    INSERT INTO [dbo].[SuCommerce_LicenseTypes]
    INSERT INTO [dbo].[SuCommerce_PackageTypes]
    INSERT INTO [dbo].[SuCommerce_PaymentServices]
    INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings]
    INSERT INTO [dbo].[SuCommerce_ProductTypes]
    INSERT INTO [dbo].[SuCommerce_PurchaseTypes]
    INSERT INTO [dbo].[SuCommerce_StatusTypes]
    INSERT INTO [dbo].[SuCommerce_Licenses] 
	INSERT INTO [dbo].[SuCommercie_Settings]
	
	-- AddonPack Slideshows

	CREATE TABLE [dbo].[SuAddons_Slideshows]
	CREATE TABLE [dbo].[SuAddons_SlideshowImages]
	CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Add]
	CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Update]
	CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Add]
	CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Update]
	INSERT INTO [dbo].[SuAddons_Slideshows]
	INSERT INTO [dbo].[SuAddons_SlideshowImages]
	CREATE VIEW [dbo].[SuAddons_vw_SlideShowImages]

	-- Analytics - Blog Rss

	CREATE TABLE [dbo].[SuAnalytics_BlogRss]
	CREATE TABLE [dbo].[SuAnalytics_BlogRssViews]
	CREATE PROCEDURE [dbo].[SuAnalytics_BlogRss_Add]
	CREATE PROCEDURE [dbo].[SuAnalytics_BlogRssView_Add]
	CREATE VIEW [dbo].[SuAnalytics_vw_BlogRss]
	CREATE VIEW [dbo].[SuAnalytics_vw_BlogRssViews]
	CREATE PROCEDURE [dbo].[SuAnalytics_BlogRss_Get]
	CREATE PROCEDURE [dbo].[SuAnalytics_BlogRssViews_Get]

	-- Request Log Updates

	CREATE VIEW [dbo].[SuAnalytics_vw_Requests]

	-- Tag Analytics

	CREATE TABLE [dbo].[SuAnalytics_TagClicks]
	CREATE PROCEDURE [dbo].[SuAnalytics_TagClick_Add]
	CREATE VIEW [dbo].[SuAnalytics_vw_TagClicks]
	CREATE PROCEDURE [dbo].[SuAnalytics_TagClicks_Get] 
	CREATE PROCEDURE [dbo].[SuAnalytics_TagClickViews_Get]

	-- Search Analytics

	CREATE TABLE [dbo].[SuAnalytics_SearchTypes]
	INSERT INTO [dbo].[SuAnalytics_SearchTypes]
	CREATE TABLE [dbo].[SuAnalytics_SearchTerms]
	CREATE VIEW [dbo].[SuAnalytics_vw_SearchTerms]
	CREATE PROCEDURE [dbo].[SuAnalytics_SearchTerm_Add]
	CREATE PROCEDURE [dbo].[SuAnalytics_SearchTerms_Get]
	CREATE PROCEDURE [dbo].[SuAnalytics_SearchTermsApp_Get]

	-- Blog Post Image Addon

	CREATE TABLE [dbo].[SuAddons_BlogPostImages]
	CREATE PROCEDURE [dbo].[Sueetie_Blog_Add]
	INSERT INTO SuAddons_BlogPostImages
	ALTER VIEW [dbo].[Sueetie_vw_Blogs]
	CREATE PROCEDURE [dbo].[SuAddons_BlogPostImage_Update]
	CREATE PROCEDURE [dbo].[SuAddons_BlogPostAlbums_Get]

	-- Forum Answers

	CREATE TABLE [dbo].[SuAddons_ForumAnswers]
	CREATE PROCEDURE [dbo].[SuAddons_ForumAnswer_Add]
	CREATE PROCEDURE [dbo].[SuAddons_ForumAnswer_Delete]
	INSERT INTO SuAddons_Settings

	-- Media Sets

	CREATE TABLE [dbo].[SuAddons_MediaSetGroups]
	CREATE TABLE [dbo].[SuAddons_MediaSets]
	CREATE TABLE [dbo].[SuAddons_MediaSetObjects]
	INSERT INTO SuAddons_MediaSetGroups
	CREATE PROCEDURE [dbo].[SuAddons_MediaSetGroup_Add]
	CREATE PROCEDURE [dbo].[SuAddons_MediaSetGroup_Update]
	CREATE PROCEDURE [dbo].[SuAddons_MediaSet_Add]
	CREATE PROCEDURE [dbo].[SuAddons_MediaSet_Update]

	CREATE PROCEDURE [dbo].[SuAddons_MediaSetAlbums_Get]
	CREATE VIEW [dbo].[SuAddons_vw_MediaSetObjects]
	CREATE PROCEDURE [dbo].[SuAddons_MediaSetObjectKeys_Get]

	CREATE VIEW [dbo].[Sueetie_vw_MediaDirectories]
	
	CREATE PROCEDURE [dbo].[Sueetie_UserLogActivities_Get] 

*/
/* ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- */

SET NOCOUNT ON
GO

print 'CREATE TABLE [dbo].[SuLicensing_LicenseTypes]'

/* -- CREATE TABLE [dbo].[SuLicensing_LicenseTypes]  -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuLicensing_LicenseTypes]    Script Date: 01/27/2011 18:11:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuLicensing_LicenseTypes](
	[LicenseTypeID] [int] NOT NULL,
	[LicenseTypeDescription] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_SuLicensing_LicenseTypes] PRIMARY KEY CLUSTERED 
(
	[LicenseTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


print 'CREATE TABLE [dbo].[SuLicensing_Packages]'

/* -- CREATE TABLE [dbo].[SuLicensing_Packages]  -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuLicensing_Packages]    Script Date: 01/27/2011 20:41:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuLicensing_Packages](
	[PackageID] [int] IDENTITY(1,1) NOT NULL,
	[PackageDescription] [nvarchar](255) NULL,
	[License] [nvarchar](60) NULL,
	[Version] [numeric](4, 1) NULL,
	[DateLicensed] [smalldatetime] NULL,
	[LicenseTypeID] [int] NULL,
	[PackageTypeID] [int] NULL,
 CONSTRAINT [PK_SuLicensing_Packages] PRIMARY KEY CLUSTERED 
(
	[PackageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


print 'INSERT INTO SuLicensing_licensetypes'

/* -- INSERT INTO SuLicensing_licensetypes  -------------------------------------------------------------------------------- */


INSERT INTO SuLicensing_licensetypes select -1, 'Not Valid';
INSERT INTO SuLicensing_licensetypes select 0, 'Trial Period';
INSERT INTO SuLicensing_licensetypes select 1, 'Free';
INSERT INTO SuLicensing_licensetypes select 2, 'Personal';
INSERT INTO SuLicensing_licensetypes select 11, 'Sueetie Insider';
INSERT INTO SuLicensing_licensetypes select 12, 'Entrepreneur';
INSERT INTO SuLicensing_licensetypes select 13, 'Small Business';
INSERT INTO SuLicensing_licensetypes select 14, 'Corporate';
INSERT INTO SuLicensing_licensetypes select 15, 'Enterprise';
INSERT INTO SuLicensing_licensetypes select 16, 'Evaluation';

GO


print 'CREATE TABLE [dbo].[SuLicensing_PackageTypes]'

/* -- CREATE TABLE [dbo].[SuLicensing_PackageTypes]  -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuLicensing_PackageTypes]    Script Date: 01/27/2011 20:27:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuLicensing_PackageTypes](
	[PackageTypeID] [int] NOT NULL,
	[PackageTypeCode] [nvarchar](25) NULL,
	[PackageTypeDescription] [nvarchar](255) NULL,
 CONSTRAINT [PK_SuLicensing_PackageTypes] PRIMARY KEY CLUSTERED 
(
	[PackageTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


print 'INSERT INTO SuLicensing_packagetypes'

/* -- INSERT INTO SuLicensing_packagetypes  -------------------------------------------------------------------------------- */

INSERT INTO SuLicensing_packagetypes select -1, 'NA', 'Unknown';
INSERT INTO SuLicensing_packagetypes select 1, 'AddonPack', 'Sueetie Addon Pack';
INSERT INTO SuLicensing_packagetypes select 2, 'Analytics', 'Sueetie Analytics';
INSERT INTO SuLicensing_packagetypes select 3, 'Marketplace', 'Sueetie Marketplace';
GO

print 'INSERT INTO SuLicensing_Packages'

/* -- INSERT INTO SuLicensing_Packages  -------------------------------------------------------------------------------- */

INSERT INTO SuLicensing_Packages (PackageDescription, Version, LicenseTypeID, PackageTypeID) values ('Sueetie Addon Pack',3.0,-1,1)
INSERT INTO SuLicensing_Packages (PackageDescription, Version, LicenseTypeID, PackageTypeID) values ('Sueetie Analytics',3.0,-1,2)
INSERT INTO SuLicensing_Packages (PackageDescription, Version, LicenseTypeID, PackageTypeID) values ('Marketplace',3.0,-1,3)
GO


print 'CREATE VIEW [dbo].[SuLicensing_vw_Packages]'

/* -- CREATE VIEW [dbo].[SuLicensing_vw_Packages]  -------------------------------------------------------------------------------- */


/****** Object:  View [dbo].[SuLicensing_vw_Packages]    Script Date: 01/28/2011 14:47:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SuLicensing_vw_Packages]
AS
SELECT     dbo.SuLicensing_Packages.PackageID, dbo.SuLicensing_Packages.PackageDescription, dbo.SuLicensing_Packages.License, dbo.SuLicensing_Packages.Version, 
                      dbo.SuLicensing_Packages.DateLicensed, dbo.SuLicensing_Packages.LicenseTypeID, dbo.SuLicensing_LicenseTypes.LicenseTypeDescription, 
                      dbo.SuLicensing_Packages.PackageTypeID, dbo.SuLicensing_PackageTypes.PackageTypeCode, dbo.SuLicensing_PackageTypes.PackageTypeDescription
FROM         dbo.SuLicensing_Packages INNER JOIN
                      dbo.SuLicensing_LicenseTypes ON dbo.SuLicensing_Packages.LicenseTypeID = dbo.SuLicensing_LicenseTypes.LicenseTypeID INNER JOIN
                      dbo.SuLicensing_PackageTypes ON dbo.SuLicensing_Packages.PackageTypeID = dbo.SuLicensing_PackageTypes.PackageTypeID

GO


print 'CREATE PROCEDURE [dbo].[SuLicensing_Package_Update]'

/* -- CREATE PROCEDURE [dbo].[SuLicensing_Package_Update]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuLicensing_Package_Update]    Script Date: 01/29/2011 11:44:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuLicensing_Package_Update]
	@PackageID int,
	@License nvarchar(60),
	@Version int,
	@LicenseTypeID int,
	@PackageTypeID int
AS

SET NOCOUNT ON

Declare @PackageDescription nvarchar(255)
select @PackageDescription = PackageTypeDescription from SuLicensing_PackageTypes where PackageTypeID = @PackageTypeID

UPDATE [dbo].[SuLicensing_Packages] SET
	[PackageDescription] = @PackageDescription,
	[License] = @License,
	[Version] = @Version,
	[DateLicensed] = GETDATE(),
	[LicenseTypeID] = @LicenseTypeID,
	[PackageTypeID] = @PackageTypeID
WHERE
	[PackageTypeID] = @PackageTypeID and
	floor([Version]) = @Version

--endregion


GO

print 'CREATE PROCEDURE [dbo].[SuLicensing_Package_Reset]'

/* -- CREATE PROCEDURE [dbo].[SuLicensing_Package_Reset]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuLicensing_Package_Reset]    Script Date: 01/30/2011 00:40:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuLicensing_Package_Reset]
	@PackageID int
AS

SET NOCOUNT ON

UPDATE [dbo].[SuLicensing_Packages] SET
	[License] = null,
	[DateLicensed] = null,
	[LicenseTypeID] = -1
WHERE
	[PackageID] = @PackageID


GO

print 'CREATE TABLE [dbo].[SuAnalytics_Log]'

/* -- CREATE TABLE [dbo].[SuAnalytics_Log] -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAnalytics_Log]    Script Date: 03/09/2011 14:44:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_Log](
	[LogID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[ContentID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[PageID] [int] NOT NULL,
	[RequestDateTime] [datetime] NOT NULL,
	[RecipientID] [int] NOT NULL,
	[ContactTypeID] [int] NOT NULL,
 CONSTRAINT [PK_SuAnalytics_Log] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAnalytics_Log] ADD  CONSTRAINT [DF_SuAnalytics_Log_ID]  DEFAULT (newid()) FOR [LogID]
GO

ALTER TABLE [dbo].[SuAnalytics_Log] ADD  CONSTRAINT [DF_SuAnalytics_Log_ContentID]  DEFAULT ((0)) FOR [ContentID]
GO

ALTER TABLE [dbo].[SuAnalytics_Log] ADD  CONSTRAINT [DF_SuAnalytics_Log_UserID]  DEFAULT ((-1)) FOR [UserID]
GO

ALTER TABLE [dbo].[SuAnalytics_Log] ADD  CONSTRAINT [DF_SuAnalytics_Log_PageID]  DEFAULT ((-1)) FOR [PageID]
GO

ALTER TABLE [dbo].[SuAnalytics_Log] ADD  CONSTRAINT [DF_SuAnalytics_Log_RequestDateTime]  DEFAULT (getdate()) FOR [RequestDateTime]
GO

ALTER TABLE [dbo].[SuAnalytics_Log] ADD  CONSTRAINT [DF_SuAnalytics_Log_RecipientID]  DEFAULT ((-1)) FOR [RecipientID]
GO

ALTER TABLE [dbo].[SuAnalytics_Log] ADD  CONSTRAINT [DF_SuAnalytics_Log_ContactTypeID]  DEFAULT ((0)) FOR [ContactTypeID]
GO


print 'CREATE TABLE [dbo].[SuAnalytics_Pages]'

/* -- CREATE TABLE [dbo].[SuAnalytics_Pages] -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAnalytics_Pages]    Script Date: 02/11/2011 16:11:04 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_Pages](
	[PageID] [int] IDENTITY(1,1) NOT NULL,
	[Url] [nvarchar](500) NULL,
	[PageTitle] [nvarchar](500) NULL,
	[ApplicationID] [int] NOT NULL,
	[IsDisplayed] [bit] NOT NULL,
 CONSTRAINT [PK_SuAnalytics_Pages] PRIMARY KEY CLUSTERED 
(
	[PageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAnalytics_Pages] ADD  CONSTRAINT [DF_SuAnalytics_Pages_ApplicationID]  DEFAULT ((0)) FOR [ApplicationID]
GO

ALTER TABLE [dbo].[SuAnalytics_Pages] ADD  CONSTRAINT [DF_SuAnalytics_Pages_IsDisplayed]  DEFAULT ((1)) FOR [IsDisplayed]
GO


print 'CREATE TABLE [dbo].[SuAnalytics_UserAgents]'

/* -- CREATE TABLE [dbo].[SuAnalytics_UserAgents] -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuAnalytics_UserAgents]    Script Date: 02/11/2011 16:11:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_UserAgents](
	[LogID] [uniqueidentifier] NOT NULL,
	[RemoteIP] [nvarchar](25) NULL,
	[UserAgent] [nvarchar](1000) NULL,
 CONSTRAINT [PK_SuAnalytics_UserAgents] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

print 'CREATE TABLE [dbo].[SuAddons_Settings]'

/* -- CREATE TABLE [dbo].[SuAddons_Settings]  -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAddons_Settings]    Script Date: 01/27/2011 11:57:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_Settings](
	[SettingName] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_SuAddons_SettingsName] PRIMARY KEY CLUSTERED 
(
	[SettingName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

print 'INSERT INTO suaddons_settings'

/* -- INSERT INTO suaddons_settings  -------------------------------------------------------------------------------- */

INSERT INTO suaddons_settings select 'IPGeoLookupUrl','http://www.ip2location.com/';
INSERT INTO suaddons_settings select 'RequestReportRecs','500';
INSERT INTO SuAddons_Settings select 'EnableForumAnswers','True';
GO

print 'CREATE TABLE [dbo].[SuAddons_BlockedIpCountries]'

/* -- CREATE TABLE [dbo].[SuAddons_BlockedIpCountries] -------------------------------------------------- */

/****** Object:  Table [dbo].[SuAddons_BlockedIpCountries]    Script Date: 01/25/2011 20:09:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_BlockedIpCountries](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
	[CountryCode] [nchar](2) NOT NULL,
	[LastUpdate] [smalldatetime] NOT NULL,
	[IsBlocked] [bit] NOT NULL,
 CONSTRAINT [PK_SuAddons_BlockedIpCountries] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAddons_BlockedIpCountries] ADD  CONSTRAINT [DF_SuAddons_BlockedIpCountries_LastUpdate]  DEFAULT ('6/9/1969') FOR [LastUpdate]
GO

ALTER TABLE [dbo].[SuAddons_BlockedIpCountries] ADD  CONSTRAINT [DF_SuAddons_BlockedIpCountries_IsBlocked]  DEFAULT ((0)) FOR [IsBlocked]
GO

print 'CREATE TABLE [dbo].[SuAddons_BlockedIPImports]'

/* -- CREATE TABLE [dbo].[SuAddons_BlockedIPImports] -------------------------------------------------- */


/****** Object:  Table [dbo].[SuAddons_BlockedIpImports]    Script Date: 01/24/2011 13:42:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_BlockedIpImports](
	[IpID] [uniqueidentifier] NOT NULL,
	[CIDR] [nvarchar](50) NULL,
	[CountryCode] [nchar](2) NULL,
 CONSTRAINT [PK_SuAddons_BlockedIpImports] PRIMARY KEY CLUSTERED 
(
	[IpID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


print 'CREATE TABLE [dbo].[SuAddons_BlockedIpRanges]'

/* -- CREATE TABLE [dbo].[SuAddons_BlockedIpRanges] -------------------------------------------------- */


/****** Object:  Table [dbo].[SuAddons_BlockedIpRanges]    Script Date: 01/25/2011 19:58:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_BlockedIpRanges](
	[IpID] [uniqueidentifier] NOT NULL,
	[CountryID] [int] NOT NULL,
	[IpStart] [nvarchar](15) NOT NULL,
	[IpEnd] [nvarchar](15) NOT NULL,
	[IsImport] [bit] NOT NULL,
	[DateEntered] [smalldatetime] NULL,
 CONSTRAINT [PK_SuAddons_BlockedIpRanges] PRIMARY KEY CLUSTERED 
(
	[IpID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAddons_BlockedIpRanges] ADD  CONSTRAINT [DF_SuAddons_BlockedIpRanges_IsImport]  DEFAULT ((1)) FOR [IsImport]
GO



print 'INSERT INTO SuAddons_BlockedIpCountries'

/* -- INSERT INTO SuAddons_BlockedIpCountries -------------------------------------------------- */

INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('China','CN')
INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('Vietnam','VN')
INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('Pakistan','PK')
INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('Indonesia','ID')
INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('Ukraine','UA')
INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('Taiwan','TW')
INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('Thailand','TH')
INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('Philippines','PH')
INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('India','IN')
INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('Russia','RU')
INSERT INTO SuAddons_BlockedIpCountries (CountryName, CountryCode) values ('North Korea','KP')
GO


print 'CREATE VIEW [dbo].[SuAddons_vw_BlockedIpRanges]'

/* -- CREATE VIEW [dbo].[SuAddons_vw_BlockedIpRanges] -------------------------------------------------- */

/****** Object:  View [dbo].[SuAddons_vw_BlockedIpRanges]    Script Date: 02/10/2011 10:25:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SuAddons_vw_BlockedIpRanges]
AS
SELECT     dbo.SuAddons_BlockedIpRanges.IpID, dbo.SuAddons_BlockedIpRanges.CountryID, dbo.SuAddons_BlockedIpRanges.IpStart, dbo.SuAddons_BlockedIpRanges.IpEnd, 
                      dbo.SuAddons_BlockedIpRanges.IsImport, dbo.SuAddons_BlockedIpCountries.CountryName, dbo.SuAddons_BlockedIpCountries.CountryCode, 
                      dbo.SuAddons_BlockedIpCountries.CountryName + N' (' + dbo.SuAddons_BlockedIpCountries.CountryCode + N')' AS CountryDescription, 
                      dbo.SuAddons_BlockedIpRanges.DateEntered
FROM         dbo.SuAddons_BlockedIpRanges INNER JOIN
                      dbo.SuAddons_BlockedIpCountries ON dbo.SuAddons_BlockedIpRanges.CountryID = dbo.SuAddons_BlockedIpCountries.CountryID

GO


print 'CREATE VIEW [dbo].[SuAddons_vw_BlockedIpImports]'

/* -- CREATE VIEW [dbo].[SuAddons_vw_BlockedIpImports] -------------------------------------------------- */

/****** Object:  View [dbo].[SuAddons_vw_BlockedIpImports]    Script Date: 01/24/2011 19:53:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SuAddons_vw_BlockedIpImports]
AS
SELECT     dbo.SuAddons_BlockedIpImports.IpID, dbo.SuAddons_BlockedIpImports.CIDR, dbo.SuAddons_BlockedIpImports.CountryCode, 
                      dbo.SuAddons_BlockedIpCountries.CountryID
FROM         dbo.SuAddons_BlockedIpImports INNER JOIN
                      dbo.SuAddons_BlockedIpCountries ON dbo.SuAddons_BlockedIpImports.CountryCode = dbo.SuAddons_BlockedIpCountries.CountryCode

GO


print 'CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRange_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRange_Add] -------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_BlockedIpRange_Add]    Script Date: 01/25/2011 20:00:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRange_Add]
	@ipID uniqueidentifier,
	@CountryID int,
	@IpStart nvarchar(15),
	@IpEnd nvarchar(15),
	@IsImport bit

AS

SET NOCOUNT ON


INSERT INTO [dbo].[SuAddons_BlockedIpRanges] (
	[IpID],
	[CountryID],
	[IpStart],
	[IpEnd],
	[IsImport],
	[DateEntered]
) VALUES (
	@IpID,
	@CountryID,
	@IpStart,
	@IpEnd,
	@IsImport,
	GETDATE()
)

GO



print 'CREATE PROCEDURE [dbo].[SuAddons_BlockedIpImport_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_BlockedIpImport_Add] -------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_BlockedIpImport_Add]    Script Date: 01/24/2011 20:53:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_BlockedIpImport_Add]
	@CIDR nvarchar(50),
	@CountryCode nchar(2)

AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuAddons_BlockedIpImports] (
	[IpID],
	[CIDR],
	[CountryCode]
) VALUES (
	NEWID(),
	@CIDR,
	@CountryCode
)

--endregion


GO

print 'CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRanges_Prep]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRanges_Prep] -------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_BlockedIpRanges_Prep]    Script Date: 01/25/2011 10:36:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRanges_Prep]

AS

SET NOCOUNT ON

delete from SuAddons_BlockedIpRanges where IsImport = 1
update SuAddons_BlockedIpCountries set IsBlocked = 0


GO


print 'CREATE PROCEDURE [dbo].[SuAddons_BlockedIpCountry_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_BlockedIpCountry_Update] -------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_BlockedIpCountry_Update]    Script Date: 01/25/2011 12:14:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_BlockedIpCountry_Update]
	@CountryID int,
	@CountryName nvarchar(50),
	@CountryCode nchar(2)
AS

SET NOCOUNT ON

UPDATE [dbo].[SuAddons_BlockedIpCountries] SET
	[CountryName] = @CountryName,
	[CountryCode] = @CountryCode,
	[IsBlocked] = 0
WHERE
	[CountryID] = @CountryID



GO

print 'CREATE PROCEDURE [dbo].[SuAddons_BlockedIpCountry_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_BlockedIpCountry_Add] -------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_BlockedIpCountry_Add]    Script Date: 01/25/2011 12:37:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_BlockedIpCountry_Add]
	@CountryName nvarchar(50),
	@CountryCode nchar(2)
AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuAddons_BlockedIpCountries] (
	[CountryName],
	[CountryCode],
	[IsBlocked]
) VALUES (
	@CountryName,
	@CountryCode,
	0
)


GO

print 'CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRange_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRange_Update] -------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_BlockedIpRange_Update]    Script Date: 01/25/2011 19:44:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_BlockedIpRange_Update]
	@IpID uniqueidentifier,
	@CountryID int,
	@IpStart nvarchar(15),
	@IpEnd nvarchar(15)
AS

SET NOCOUNT ON

UPDATE [dbo].[SuAddons_BlockedIpRanges] SET
	[CountryID] = @CountryID,
	[IpStart] = @IpStart,
	[IpEnd] = @IpEnd
WHERE
	[IpID] = @IpID



GO

print 'CREATE TABLE [dbo].[SuAddons_CrawlerAgents]'

/* -- CREATE TABLE [dbo].[SuAddons_CrawlerAgents] -------------------------------------------------- */


/****** Object:  Table [dbo].[SuAddons_CrawlerAgents]    Script Date: 01/31/2011 15:40:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_CrawlerAgents](
	[AgentID] [int] IDENTITY(1,1) NOT NULL,
	[AgentExcerpt] [nvarchar](25) NOT NULL,
	[IsBlocked] [bit] NOT NULL,
	[DateEntered] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_SuAddons_CrawlerAgents] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAddons_CrawlerAgents] ADD  CONSTRAINT [DF_SuAddons_CrawlerAgents_IsBlocked]  DEFAULT ((1)) FOR [IsBlocked]
GO


print 'CREATE PROCEDURE [dbo].[SuAddons_CrawlerAgent_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_CrawlerAgent_Add] -------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_CrawlerAgent_Add]    Script Date: 02/01/2011 10:24:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_CrawlerAgent_Add]
	@AgentExcerpt nvarchar(25),
	@IsBlocked bit

AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuAddons_CrawlerAgents] (
	[AgentExcerpt],
	[IsBlocked],
	[DateEntered]
) VALUES (
	@AgentExcerpt,
	@IsBlocked,
	GETDATE()
)


GO


print 'CREATE PROCEDURE [dbo].[SuAddons_CrawlerAgent_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_CrawlerAgent_Update] -------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_CrawlerAgent_Update]    Script Date: 02/01/2011 10:25:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAddons_CrawlerAgent_Update]
	@AgentID int,
	@AgentExcerpt nvarchar(25),
	@IsBlocked bit
AS

SET NOCOUNT ON

UPDATE [dbo].[SuAddons_CrawlerAgents] SET
	[AgentExcerpt] = @AgentExcerpt,
	[IsBlocked] = @IsBlocked
WHERE
	[AgentID] = @AgentID


GO





print 'CREATE TABLE [dbo].[SuAnalytics_ContactTypes]'

/* -- CREATE TABLE [dbo].[SuAnalytics_ContactTypes]  -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuAnalytics_ContactTypes]    Script Date: 02/11/2011 19:55:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_ContactTypes](
	[ContactTypeID] [int] NOT NULL,
	[ContactTypeCode] [nvarchar](50) NULL,
	[ContactTypeDescription] [nvarchar](255) NULL,
 CONSTRAINT [PK_SuAnalytics_ContactTypes] PRIMARY KEY CLUSTERED 
(
	[ContactTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

print 'INSERT INTO SuAnalytics_ContactTypes'

/* -- INSERT INTO SuAnalytics_ContactTypes  -------------------------------------------------------------------------------- */

INSERT INTO SuAnalytics_ContactTypes select 0, 'NoContact','NA'
INSERT INTO SuAnalytics_ContactTypes select 1, 'ViewProfile','Member views another member''s profile'
INSERT INTO SuAnalytics_ContactTypes select 2, 'PersonalMessage','Member sends another member a personal message'
INSERT INTO SuAnalytics_ContactTypes select 3, 'Email','Member sends another member an email'
GO


print 'CREATE VIEW [dbo].[Sueetie_vw_Requests]'

/* -- CREATE VIEW [dbo].[Sueetie_vw_Requests]  -------------------------------------------------------------------------------- */


/****** Object:  View [dbo].[Sueetie_vw_Requests]    Script Date: 02/11/2011 20:03:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Sueetie_vw_Requests]
AS
SELECT     dbo.SuAnalytics_UserAgents.LogID, dbo.SuAnalytics_Log.ContentID, dbo.SuAnalytics_Log.UserID, dbo.SuAnalytics_Log.PageID, 
                      dbo.SuAnalytics_Log.RequestDateTime, dbo.SuAnalytics_Pages.Url, dbo.SuAnalytics_Pages.PageTitle, dbo.SuAnalytics_Pages.ApplicationID, 
                      dbo.SuAnalytics_UserAgents.RemoteIP, dbo.SuAnalytics_UserAgents.UserAgent, CONVERT(int, 0) AS Count, dbo.SuAnalytics_Log.RecipientID, 
                      dbo.SuAnalytics_Log.ContactTypeID
FROM         dbo.SuAnalytics_Log INNER JOIN
                      dbo.SuAnalytics_Pages ON dbo.SuAnalytics_Log.PageID = dbo.SuAnalytics_Pages.PageID INNER JOIN
                      dbo.SuAnalytics_UserAgents ON dbo.SuAnalytics_Log.LogID = dbo.SuAnalytics_UserAgents.LogID

GO

print 'CREATE PROCEDURE [dbo].[Sueetie_Request_Add]'

/* -- CREATE PROCEDURE [dbo].[Sueetie_Request_Add]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[Sueetie_Request_Add]    Script Date: 04/29/2011 10:44:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_Request_Add]
	@ContentID int,
	@UserID int,
	@Url nvarchar(500),
	@PageTitle nvarchar(500),
	@ApplicationID int,
	@RemoteIP nvarchar(25),
	@UserAgent nvarchar(1000),
	@RecipientID int,
	@ContactTypeID int
	
AS

SET NOCOUNT ON

declare @PageID int

if @ContentID > 1
begin
    select @Url = permalink from Sueetie_Content where contentID = @contentID
end   

select @PageID = PageID from SuAnalytics_Pages where url = @url

if @PageID is null
begin
	INSERT INTO [dbo].[SuAnalytics_Pages] (
		[Url],
		[PageTitle],
		[ApplicationID]
	) VALUES (
		@Url,
		@PageTitle,
		@ApplicationID
	)
	select @PageID = @@IDENTITY
end 
else begin
	update SuAnalytics_Pages set pagetitle = @PageTitle where PageID = @PageID
end

-- User Profile Page ---------------------------------------------------

if @RecipientID > 0 and @ContactTypeID = 1
Begin
	DECLARE @username nvarchar(50)
	select @username = name from yaf_User where UserID = @RecipientID
	select @RecipientID = userid from Sueetie_Users where UserName = @username
End


DECLARE @guid uniqueidentifier
SELECT @guid = newid()

DECLARE @logID uniqueidentifier
SELECT @logID = l.LogID from SuAnalytics_Log l inner join SuAnalytics_UserAgents ua 
	ON l.LogID = ua.LogID
	WHERE RemoteIP = @RemoteIP 
	AND PageID = @PageID 
	AND DATEDIFF(mi,  l.RequestDateTime, getdate()) <= 30

if @logID is null
BEGIN

		INSERT INTO [dbo].[SuAnalytics_Log] (
			[LogID],
			[ContentID],
			[UserID],
			[PageID],
			[RequestDateTime],
			[RecipientID],
			[ContactTypeID]
		) VALUES (
			@guid,
			@ContentID,
			@UserID,
			@PageID,
			GETDATE(),
			@RecipientID,
			@ContactTypeID
		)


		INSERT INTO [dbo].[SuAnalytics_UserAgents] (
			[LogID],
			[RemoteIP],
			[UserAgent]
		) VALUES (
			@guid,
			@RemoteIP,
			@UserAgent
		)
		
END

if @UserID > 0
begin
	update Sueetie_Users set LastActivity = GETDATE() where UserID = @UserID
end

GO



print 'CREATE PROCEDURE [dbo].[SuAddons_RequestAgents_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_RequestAgents_Get]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_RequestAgents_Get]    Script Date: 02/07/2011 15:08:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAddons_RequestAgents_Get] 
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON


CREATE TABLE #z_sueetie_vw_requests(
	[LogID] [uniqueidentifier] NOT NULL,
	[ContentID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[PageID] [int] NOT NULL,
	[RequestDateTime] [datetime] NULL,
	[Url] [nvarchar](500) NULL,
	[PageTitle] [nvarchar](500) NULL,
	[ApplicationID] [int] NOT NULL,
	[RemoteIP] [nvarchar](25) NULL,
	[UserAgent] [nvarchar](1000) NULL,
	[Count] [int] NULL,
	[RecipientID] [int] NULL,
	[ContactTypeID] [int] NULL
)



CREATE TABLE #requestagents(
	[UserAgent] [nvarchar](1000) NULL,
 )

insert into #requestagents select distinct useragent from sueetie_vw_requests where userid > 0

select count(*) as 'cnt', MAX(requestDateTime) as 'RequestDateTime', REPLICATE('0',64) as 'LogID', 
	useragent into #tempAgents from Sueetie_vw_Requests where UserAgent not in (select UserAgent from #requestagents)
	group by useragent order by count(*) desc
	
	select top 100 * into #tempAgent100 from #tempAgents
	
	update #tempAgent100 set LogID = s.LogID from Sueetie_vw_Requests s
	inner join #tempAgent100 t on s.RequestDateTime = t.requestdatetime and s.UserAgent = t.useragent
	
	insert into #z_sueetie_vw_requests 
	select LogID, 1, -1, -1, RequestDateTime, null, null, 0, null, UserAgent, cnt, -1, 0
	from #tempAgent100

select * from #z_sueetie_vw_requests
 
	
drop table #requestAgents
drop table #tempAgents
drop table #tempAgent100
drop table #z_sueetie_vw_requests

END

GO

print 'CREATE PROCEDURE [dbo].[SuAddons_RequestAgents_Delete]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_RequestAgents_Delete]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_RequestAgents_Delete]    Script Date: 02/07/2011 15:10:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_RequestAgents_Delete]
	@LogID Uniqueidentifier
AS
BEGIN

SET NOCOUNT ON


DECLARE @agent nvarchar(1000)
DECLARE @startcount int
DECLARE @endcount int
DECLARE @recordsdeleted int


select @agent = useragent from SuAnalytics_UserAgents 
where LogID = @LogID

select @startcount = COUNT(*) from SuAnalytics_Log 

delete from SuAnalytics_Log where LogID in
(SELECT     p.LogID 
FROM         dbo.SuAnalytics_UserAgents q INNER JOIN
                      dbo.SuAnalytics_Log p ON p.LogID = q.LogID
                      where UserAgent = @agent)
                      
delete from SuAnalytics_UserAgents where UserAgent = @agent
             
select @endcount = COUNT(*) from SuAnalytics_Log

select @recordsdeleted = @startcount - @endcount
select @recordsdeleted

END

GO


print 'CREATE PROCEDURE [dbo].[SuAddons_RequestIPs_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_RequestIPs_Get]   -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_RequestIPs_Get]    Script Date: 02/08/2011 12:41:27 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_RequestIPs_Get] 
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON


CREATE TABLE #z_sueetie_vw_requests(
	[LogID] [uniqueidentifier] NOT NULL,
	[ContentID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[PageID] [int] NOT NULL,
	[RequestDateTime] [datetime] NULL,
	[Url] [nvarchar](500) NULL,
	[PageTitle] [nvarchar](500) NULL,
	[ApplicationID] [int] NOT NULL,
	[RemoteIP] [nvarchar](25) NULL,
	[UserAgent] [nvarchar](1000) NULL,
	[Count] [int] NULL,
	[RecipientID] [int] NULL,
	[ContactTypeID] [int] NULL
)

CREATE TABLE #requestIPs(
	[remoteip] [nvarchar](25) NULL,
 )

insert into #requestIPs select distinct remoteip from sueetie_vw_requests where userid > 0

select count(*) as 'cnt', MAX(requestDateTime) as 'RequestDateTime', REPLICATE('0',64) as 'LogID', 
	remoteip into #tempIPs from Sueetie_vw_Requests where RemoteIP not in (select RemoteIP from #requestIPs)
	group by RemoteIP order by count(*) desc
	
	select top 100 * into #tempIP100 from #tempIPs
	
	update #tempIP100 set LogID = s.LogID from Sueetie_vw_Requests s
	inner join #tempIP100 t on s.RequestDateTime = t.requestdatetime and s.RemoteIP = t.remoteIP
	
	insert into #z_sueetie_vw_requests 
	select LogID, 1, -1, -1, RequestDateTime, null, null, 0, remoteIP, null, cnt, -1, 0
	from #tempIP100

	update #z_sueetie_vw_requests set UserAgent = s.UserAgent from SuAnalytics_UserAgents s
	inner join #z_sueetie_vw_requests t on s.LogID = t.LogID
	
select * from #z_sueetie_vw_requests
	
drop table #requestIPs
drop table #tempIPs
drop table #tempIP100
drop table #z_sueetie_vw_requests

END

GO


print 'CREATE PROCEDURE [dbo].[SuAddons_RequestIPs_Delete]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_RequestIPs_Delete]   -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_RequestIPs_Delete]    Script Date: 02/08/2011 14:04:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_RequestIPs_Delete]
	@LogID Uniqueidentifier
AS
BEGIN

SET NOCOUNT ON


DECLARE @remoteIP nvarchar(25)
DECLARE @startcount int
DECLARE @endcount int
DECLARE @recordsdeleted int


select @remoteIP = RemoteIP from SuAnalytics_UserAgents 
where LogID = @LogID

select @startcount = COUNT(*) from SuAnalytics_Log 

delete from SuAnalytics_Log where LogID in
(SELECT     p.LogID 
FROM         dbo.SuAnalytics_UserAgents q INNER JOIN
                      dbo.SuAnalytics_Log p ON p.LogID = q.LogID
                      where RemoteIP = @remoteIP)
                      
delete from SuAnalytics_UserAgents where RemoteIP = @remoteIP
             
select @endcount = COUNT(*) from SuAnalytics_Log

select @recordsdeleted = @startcount - @endcount
select @recordsdeleted

END

GO


print 'CREATE PROCEDURE [dbo].[SuAddons_Setting_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_Setting_Update]   -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_Setting_Update]    Script Date: 02/08/2011 14:44:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[SuAddons_Setting_Update]
	@SettingName nvarchar(50),
	@SettingValue ntext
AS

SET NOCOUNT ON

delete from  [dbo].[SuAddons_Settings] where SettingName = @SettingName

insert into [dbo].[SuAddons_Settings] Values (@SettingName, @SettingValue)


GO


print 'CREATE PROCEDURE [dbo].[SuAnalytics_User_Filter]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_User_Filter]   -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_User_Filter]    Script Date: 02/10/2011 13:34:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAnalytics_User_Filter]
	@userID int
AS
BEGIN

SET NOCOUNT ON

DECLARE @startcount int
DECLARE @endcount int
DECLARE @recordsdeleted int

insert into SuAnalytics_FilteredUsers select @userID

select convert(UniqueIdentifier, LogID) as LogID into #tempLogIDs from SuAnalytics_Log 
where UserID = @userid

select @startcount = COUNT(*) from SuAnalytics_Log 

delete from SuAnalytics_Log where LogID in (select LogID from #tempLogIDs)
                      
delete from SuAnalytics_UserAgents where LogID in (select LogID from #tempLogIDs)
             
select @endcount = COUNT(*) from SuAnalytics_Log

select @recordsdeleted = @startcount - @endcount
select @recordsdeleted

drop table #tempLogIDs

END


GO



print 'CREATE TABLE [dbo].[SuAnalytics_FilteredUrls]'

/* -- CREATE TABLE [dbo].[SuAnalytics_FilteredUrls]   -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAnalytics_FilteredUrls]    Script Date: 02/10/2011 14:03:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_FilteredUrls](
	[FilteredUrlID] [int] IDENTITY(1,1) NOT NULL,
	[UrlExcerpt] [nvarchar] (50) NOT NULL,
	[DateTimeEntered] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_SuAnalytics_FilteredUrls] PRIMARY KEY CLUSTERED 
(
	[FilteredUrlID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

print 'CREATE PROCEDURE [dbo].[SuAnalytics_AnonymousUrls_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_AnonymousUrls_Get]   -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAnalytics_AnonymousUrls_Get]    Script Date: 02/10/2011 21:44:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAnalytics_AnonymousUrls_Get] 
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON


CREATE TABLE #z_sueetie_vw_requests(
	[LogID] [uniqueidentifier] NOT NULL,
	[ContentID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[PageID] [int] NOT NULL,
	[RequestDateTime] [datetime] NULL,
	[Url] [nvarchar](500) NULL,
	[PageTitle] [nvarchar](500) NULL,
	[ApplicationID] [int] NOT NULL,
	[RemoteIP] [nvarchar](25) NULL,
	[UserAgent] [nvarchar](1000) NULL,
	[Count] [int] NULL,
	[RecipientID] [int] NULL,
	[ContactTypeID] [int] NULL
)

CREATE TABLE #requesturls(
	[Url] [nvarchar](500) NULL,
 )

insert into #requesturls select distinct url from sueetie_vw_requests where userid > 0

select count(*) as 'cnt', MAX(requestDateTime) as 'RequestDateTime', REPLICATE('0',64) as 'LogID', 
	url into #tempUrls from Sueetie_vw_Requests where url not in (select url from #requesturls)
	group by url order by Url
	
	select top 1000 *, CONVERT(int,1) as 'contentID' into #tempAgent100 from #tempUrls
	
	update #tempAgent100 set LogID = s.LogID, contentID = s.ContentID from Sueetie_vw_Requests s
	inner join #tempAgent100 t on s.RequestDateTime = t.requestdatetime and s.Url = t.Url
	
	insert into #z_sueetie_vw_requests 
	select LogID, contentID, -1, -1, RequestDateTime, url, null, 0, null, null, cnt, -1, 0
	from #tempAgent100

select * from #z_sueetie_vw_requests order by url
 
	
drop table #requesturls
drop table #tempUrls
drop table #tempAgent100
drop table #z_sueetie_vw_requests

END

GO

print 'CREATE PROCEDURE [dbo].[SuAnalytics_AnonymousUrlDetails_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_AnonymousUrlDetails_Get]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_AnonymousUrlDetails_Get]    Script Date: 02/10/2011 18:51:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAnalytics_AnonymousUrlDetails_Get] 
(
	@urlRoot nvarchar(255)
)
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

select * from Sueetie_vw_Requests where Url like @urlRoot

END


GO

print 'CREATE PROCEDURE [dbo].[SuAnalytics_Logs_Clear]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_Logs_Clear]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAnalytics_Logs_Clear]    Script Date: 02/11/2011 19:23:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAnalytics_Logs_Clear] 
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

truncate table SuAnalytics_Log
truncate table SuAnalytics_Pages
truncate table SuAnalytics_UserAgents

END


GO


print 'CREATE TABLE [dbo].[SuAnalytics_PageRules]'

/* -- CREATE TABLE [dbo].[SuAnalytics_PageRules]  -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAnalytics_PageRules]    Script Date: 02/15/2011 17:33:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_PageRules](
	[PageRuleID] [int] IDENTITY(1,1) NOT NULL,
	[UrlExcerpt] [nvarchar](255) NULL,
	[UrlFinal] [nvarchar](255) NULL,
	[PageTitle] [nvarchar](255) NOT NULL,
	[IsEqual] [bit] NOT NULL,
 CONSTRAINT [PK_SuAnalytics_PageRules] PRIMARY KEY CLUSTERED 
(
	[PageRuleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAnalytics_PageRules] ADD  CONSTRAINT [DF_SuAnalytics_PageRules_IsEqual]  DEFAULT ((0)) FOR [IsEqual]
GO





print 'CREATE PROCEDURE [dbo].[SuAnalytics_Pages_Delete]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_Pages_Delete]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_Pages_Delete]    Script Date: 02/13/2011 18:29:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SuAnalytics_Pages_Delete] 
(
	@urlRoot nvarchar(50)
)
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

DECLARE @startcount int
DECLARE @endcount int
DECLARE @recordsdeleted int

CREATE TABLE #LogIDs(
	[LogID] [uniqueidentifier] NOT NULL,
	[PageID] int NOT NULL
)


select @startcount = COUNT(*) from SuAnalytics_Log 

insert into #LogIDs select  LogID, PageID  from sueetie_vw_requests where Url like @urlRoot + '%' order by url

delete from SuAnalytics_Pages where PageID in (select PageID from #LogIDs)
delete  from SuAnalytics_Log where LogID in (select LogID from #LogIDs)

select @endcount = COUNT(*) from SuAnalytics_Log

select @recordsdeleted = @startcount - @endcount
select @recordsdeleted

drop table #LogIDs

END



GO


print 'CREATE PROCEDURE [dbo].[SuAnalytics_PageRule_Create]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_PageRule_Create]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAnalytics_PageRule_Create]    Script Date: 02/15/2011 14:01:17 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAnalytics_PageRule_Create]
	@UrlExcerpt nvarchar(255),
	@UrlFinal nvarchar(255),
	@PageTitle nvarchar(255),
	@IsEqual bit
AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuAnalytics_PageRules] (
	[UrlExcerpt],
	[UrlFinal],
	[PageTitle],
	[IsEqual]
) VALUES (
	@UrlExcerpt,
	@UrlFinal,
	@PageTitle,
	@IsEqual
)


GO




print 'CREATE PROCEDURE [dbo].[SuAnalytics_PageRule_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_PageRule_Update]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_PageRule_Update]    Script Date: 02/21/2011 14:51:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAnalytics_PageRule_Update]
	@PageRuleID int,
	@UrlExcerpt nvarchar(255),
	@UrlFinal nvarchar(255),
	@PageTitle nvarchar(255),
	@IsEqual bit
AS

SET NOCOUNT ON

UPDATE [dbo].[SuAnalytics_PageRules] SET
	[UrlExcerpt] = @UrlExcerpt,
	[UrlFinal] = @UrlFinal,	
	[PageTitle] = @PageTitle,
	[IsEqual] = @IsEqual
WHERE
	[PageRuleID] = @PageRuleID



GO


print 'CREATE PROCEDURE [dbo].[SuAddons_AgentRequests_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_AgentRequests_Get]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_AgentRequests_Get]    Script Date: 02/15/2011 08:31:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_AgentRequests_Get] 
(
	@logID UniqueIdentifier
)
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

DECLARE @UserAgent nvarchar(1000)
select @UserAgent = UserAgent from SuAnalytics_UserAgents where logID = @logID

select * from Sueetie_vw_Requests where UserAgent = @UserAgent order by RequestDateTime desc

END


GO


print 'CREATE VIEW [dbo].[SuAnalytics_vw_Pages]'

/* -- CREATE VIEW [dbo].[SuAnalytics_vw_Pages]  -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[SuAnalytics_vw_Pages]    Script Date: 02/22/2011 09:11:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SuAnalytics_vw_Pages]
AS
SELECT     dbo.SuAnalytics_Log.LogID, dbo.SuAnalytics_Log.ContentID, dbo.Sueetie_Users.UserID, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.DisplayName, 
                      dbo.SuAnalytics_Pages.PageTitle, dbo.SuAnalytics_Pages.Url, dbo.SuAnalytics_Pages.ApplicationID, dbo.SuAnalytics_Log.RequestDateTime, 
                      dbo.SuAnalytics_Log.PageID, dbo.Sueetie_Content.ContentTypeID, dbo.SuAnalytics_Pages.IsDisplayed, dbo.Sueetie_Applications.ApplicationKey, 
                      dbo.Sueetie_Applications.Description AS ApplicationDescription, dbo.Sueetie_ContentTypes.Description AS ContentTypeDescription, CONVERT(int, 0) AS Count
FROM         dbo.SuAnalytics_Log INNER JOIN
                      dbo.SuAnalytics_Pages ON dbo.SuAnalytics_Log.PageID = dbo.SuAnalytics_Pages.PageID INNER JOIN
                      dbo.Sueetie_Content ON dbo.SuAnalytics_Log.ContentID = dbo.Sueetie_Content.ContentID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.SuAnalytics_Pages.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Users ON dbo.SuAnalytics_Log.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_ContentTypes ON dbo.Sueetie_Content.ContentTypeID = dbo.Sueetie_ContentTypes.ContentTypeID

GO

print 'CREATE TABLE [dbo].[SuAnalytics_Settings]'

/* -- CREATE TABLE [dbo].[SuAnalytics_Settings]  -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAnalytics_Settings]    Script Date: 02/22/2011 20:25:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_Settings](
	[SettingName] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_SuAnalytics_SettingsName] PRIMARY KEY CLUSTERED 
(
	[SettingName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


print 'CREATE PROCEDURE [dbo].[SuAnalytics_Setting_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_Setting_Update]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_Setting_Update]    Script Date: 02/22/2011 20:26:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAnalytics_Setting_Update]
	@SettingName nvarchar(50),
	@SettingValue ntext
AS

SET NOCOUNT ON

delete from  [dbo].[SuAnalytics_Settings] where SettingName = @SettingName

insert into [dbo].[SuAnalytics_Settings] Values (@SettingName, @SettingValue)


GO


print 'CREATE PROCEDURE [dbo].[SuAnalytics_Refreshes_Remove]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_Refreshes_Remove]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_Refreshes_Remove]    Script Date: 02/25/2011 21:03:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAnalytics_Refreshes_Remove] 
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

declare @prevremoteip nvarchar(25)
declare @currentremoteip nvarchar(25)

declare @prevrequestdatetime smalldatetime
declare @currentrequestdatetime smalldatetime

declare @prevurl nvarchar(500)
declare @currenturl nvarchar(500)

declare @prevlogid uniqueidentifier
declare @logid uniqueidentifier

select @prevremoteip = -1

declare @recsDeleted int
select @recsDeleted = 0

declare pageCursor cursor for select top 5000 logid, url, remoteip, requestDateTime from sueetie_vw_requests 
	where contentID > 0 order by remoteIP, requestdatetime desc

open pageCursor

fetch next from pageCursor into @logid, @currenturl, @currentremoteip, @currentrequestdatetime

while @@fetch_status = 0
begin

	if @currenturl = @prevurl
	begin
		if @currentremoteip = @prevremoteip and DATEDIFF(mi,@currentrequestdatetime, @prevrequestdatetime) < 10
		begin
			--print @currenturl
			delete from SuAnalytics_Log where LogID = @prevlogid
			delete from SuAnalytics_UserAgents where LogID = @prevlogid
			select @recsDeleted = @recsDeleted + 1
		end
	end
	
	select @prevremoteip = @currentremoteip
	select @prevrequestdatetime = @currentrequestdatetime
	select @prevurl = @currenturl
	select @prevlogid = @logid
		
	fetch next from pageCursor into @logid, @currenturl, @currentremoteip, @currentrequestdatetime
	
end

close pageCursor
deallocate pageCursor

select @recsDeleted
END


GO



print 'INSERT INTO [dbo].[SuAddons_CrawlerAgents]'

/* -- INSERT INTO [dbo].[SuAddons_CrawlerAgents]  -------------------------------------------------------------------------------- */


truncate table  [dbo].[SuAddons_CrawlerAgents]

SET IDENTITY_INSERT [dbo].[SuAddons_CrawlerAgents] ON

INSERT INTO [dbo].[SuAddons_CrawlerAgents] (
	[AgentID],
	[AgentExcerpt],
	[IsBlocked],
	[DateEntered]
)
	SELECT 1, 'msnbot', 0, getdate() UNION
	SELECT 2, 'googlebot', 0, getdate() UNION
	SELECT 3, 'sitebot', 0, getdate() UNION
	SELECT 4, 'bingbot', 0, getdate() UNION
	SELECT 5, 'dotbot', 0, getdate() UNION
	SELECT 6, 'sitemaps', 0, getdate() UNION
	SELECT 7, 'Mail.Ru', 1, getdate() UNION
	SELECT 8, 'MJ12bot', 1, getdate() UNION
	SELECT 9, 'Baiduspider', 1, getdate() UNION
	SELECT 10, 'Linquee', 1, getdate() UNION
	SELECT 11, 'Exabot', 1, getdate() UNION
	SELECT 12, 'R6_FeedFetcher', 1, getdate() UNION
	SELECT 13, 'YandexBot', 1, getdate() UNION
	SELECT 14, 'ScrapeBox', 1, getdate() UNION
	SELECT 15, 'Spider', 1, getdate() UNION
	SELECT 16, 'slurp', 0, getdate() UNION
	SELECT 17, 'findfiles', 1, getdate() UNION
	SELECT 18, 'archive', 0, getdate() UNION
	SELECT 19, 'crawler', 0, getdate() UNION
	SELECT 20, 'scoutjet', 0, getdate() UNION
	SELECT 21, 'linkedinbot', 0, getdate() UNION
	SELECT 22, 'purebot', 0, getdate() UNION
	SELECT 23, 'Huaweisymantecspider', 1, getdate() UNION
	SELECT 24, 'libwww-perl', 1, getdate() UNION
	SELECT 25, 'HolmesBot', 1, getdate() UNION
	SELECT 26, 'discobot', 1, getdate() UNION
	SELECT 27, 'paperlibot', 1, getdate() UNION
	SELECT 28, 'Ezooms', 1, getdate() UNION
	SELECT 29, 'Java/', 1, getdate() UNION
	SELECT 30, 'lexxebot', 0, getdate() UNION
	SELECT 31, 'Python', 1, getdate() UNION
	SELECT 32, 'steal', 1, getdate() UNION
	SELECT 33, 'reeder', 1, getdate() UNION
	SELECT 34, 'cmsworldmap', 1, getdate() UNION
	SELECT 35, 'violabot', 1, getdate()

SET IDENTITY_INSERT [dbo].[SuAddons_CrawlerAgents] OFF
GO


print 'INSERT INTO [dbo].[SuAnalytics_FilteredUrls]'

/* -- INSERT INTO [dbo].[SuAnalytics_FilteredUrls]  -------------------------------------------------------------------------------- */


truncate table [dbo].[SuAnalytics_FilteredUrls] 

SET IDENTITY_INSERT [dbo].[SuAnalytics_FilteredUrls] ON

INSERT INTO [dbo].[SuAnalytics_FilteredUrls] (
	[FilteredUrlID],
	[UrlExcerpt],
	[DateTimeEntered]
)
	SELECT 3, '/author/', getdate() UNION
	SELECT 4, '/category/', getdate() UNION
	SELECT 7, '/forum/default.aspx?g=-1%27&', getdate() UNION
	SELECT 8, '/forum/default.aspx?g%3d', getdate() UNION
	SELECT 9, '/forum/default.aspx?g=posts', getdate() UNION
	SELECT 10, '/forum/default.aspx?g=profile', getdate() UNION
	SELECT 11, '/forum/default.aspx?g=reportpost', getdate() UNION
	SELECT 13, '/forum/default.aspx?g=messagehistory', getdate() UNION
	SELECT 14, '/forum/yaf_messagehistory', getdate() UNION
	SELECT 15, '/wiki/allpages.aspx', getdate() UNION
	SELECT 16, '/wiki/default.aspx?ns=&page', getdate() UNION
	SELECT 17, '/members/profile', getdate() UNION
	SELECT 18, '/blog/default.aspx?page=', getdate() UNION
	SELECT 19, '&ns=&', getdate() UNION
	SELECT 20, '/wiki/default.aspx?page=', getdate() UNION
	SELECT 21, '/wiki/history.aspx', getdate() UNION
	SELECT 22, '/forum/default.aspx?f=', getdate() UNION
	SELECT 24, '/blog/default.aspx?id=', getdate() UNION
	SELECT 25, '/blog/default.aspx?name=', getdate() UNION
	SELECT 26, '/wiki/rss.aspx', getdate() UNION
	SELECT 27, 'ashx?code=1', getdate() UNION
	SELECT 28, 'ashx?discuss=1', getdate() UNION
	SELECT 29, '/wiki/pagenotfound', getdate() UNION
	SELECT 30, '/wiki/print.aspx?page=', getdate() UNION
	SELECT 31, '/wiki/sueetie insiders.', getdate() UNION
	SELECT 32, '/blog/default.aspx?tag=', getdate() UNION
	SELECT 34, '/forum/yaf_mytopics', getdate()

SET IDENTITY_INSERT [dbo].[SuAnalytics_FilteredUrls] OFF
GO


print 'INSERT INTO [dbo].[SuAnalytics_PageRules]'

/* -- INSERT INTO [dbo].[SuAnalytics_PageRules]   -------------------------------------------------------------------------------- */

truncate table [dbo].[SuAnalytics_PageRules]

SET IDENTITY_INSERT [dbo].[SuAnalytics_PageRules] ON

INSERT INTO [dbo].[SuAnalytics_PageRules] (
	[PageRuleID],
	[UrlExcerpt],
	[UrlFinal],
	[PageTitle],
	[IsEqual]
)
	SELECT 1, '/yaf_profile', '/util/misc/yaf_profile.aspx', 'Member Profile Page', 0 UNION
	SELECT 2, '/yaf_im_email', '/util/misc/yaf_im_email.aspx', 'Member Email', 0 UNION
	SELECT 3, '/yaf_pmessage', '/util/misc/yaf_pmessage.aspx', 'Member Personal Message', 0 UNION
	SELECT 4, '/yaf_cp_profile.aspx', '/forum/yaf_cp_profile.aspx', 'Member Dashboard', 0 UNION
	SELECT 5, '/yaf_members.aspx', '/forum/yaf_members.aspx', 'Members', 0 UNION
	SELECT 6, '/yaf_mytopics.aspx', '/forum/yaf_mytopics.aspx', 'Discussions - My Topics', 0 UNION
	SELECT 7, '/yaf_album.aspx', '/util/misc/yaf_album.aspx', 'Member My Albums', 0 UNION
	SELECT 8, '/yaf_cp_editalbumimages.aspx', '/util/misc/yaf_cp_editalbumimages.aspx', 'Member Album Management', 0 UNION
	SELECT 9, '/yaf_search.aspx', '/forum/yaf_search.aspx', 'Discussions - Search', 0 UNION
	SELECT 10, '/yaf_help_index.aspx', '/forum/yaf_help_index.aspx', 'Discussions - Help', 0 UNION
	SELECT 11, '/wiki/allpages.aspx', '/wiki/allpages.aspx', 'Wiki - All Pages', 0 UNION
	SELECT 12, '/wiki/search.aspx', '/wiki/search.aspx', 'Wiki - Search', 0 UNION
	SELECT 13, '?g=cp_pm', '/util/misc/yaf_cp_message.aspx', 'Member Conversations', 0 UNION
	SELECT 15, '/yaf_postmessage.aspx', '/util/misc/yaf_postmessage.aspx', 'Discussions - Post Message', 0 UNION
	SELECT 16, '/search/default.aspx', '/search/default.aspx', 'Site Search', 0 UNION
	SELECT 17, '/blog/default.aspx?page', '/blog/default.aspx', 'Sueetie - News and Insider Info', 0 UNION
	SELECT 18, '/blog/search.aspx?q', '/blog/search.aspx', 'Blog Search', 0 UNION
	SELECT 19, '/default.aspx', '/default.aspx', 'Home Page', 1 UNION
	SELECT 20, '/media/default.aspx?aid=1', '/media/default.aspx', 'Media Home ', 1 UNION
	SELECT 21, '/yaf_cp_message.aspx', '/util/misc/yaf_cp_message.aspx', 'Member Conversations', 0 UNION
	SELECT 22, '/yaf_cp_pm.aspx', '/util/misc/yaf_cp_message.aspx', 'Member Conversations', 0 UNION
	SELECT 23, '/yaf_rsstopic', '/util/misc/yaf_rsstopic.aspx', 'Discussions - Topic RSS', 0 UNION
	SELECT 24, '/yaf_login', '/util/misc/yaf_login.aspx', 'YAF Login Redirection', 0 UNION
	SELECT 25, '/yaf_viewthanks', '/util/misc/yaf_viewthanks.aspx', 'Discussions - View Thanks', 0

SET IDENTITY_INSERT [dbo].[SuAnalytics_PageRules] OFF
GO



print 'CREATE MARKETPLACE OBJECTS'

/* -- CREATE MARKETPLACE OBJECTS   -------------------------------------------------------------------------------- */

PRINT '/****** Object:  Table [dbo].[SuCommerce_ProductTypes]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_ProductTypes](
	[ProductTypeID] [int] NOT NULL,
	[ProductTypeCode] [nvarchar](50) NULL,
	[ProductTypeDescription] [nvarchar](50) NULL,
	[IsDisplayed] [bit] NOT NULL,
 CONSTRAINT [PK_SuCommerce_ProductTypes] PRIMARY KEY CLUSTERED 
(
	[ProductTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_ProductPackage]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_ProductPackage](
	[ProductPackageID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[PackageTypeID] [int] NOT NULL,
	[Version] [numeric](4, 1) NOT NULL,
 CONSTRAINT [PK_SuCommerce_ProductPackage] PRIMARY KEY CLUSTERED 
(
	[ProductPackageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_Photos]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_Photos](
	[PhotoID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[IsMainPreview] [bit] NOT NULL,
	[DateCreated] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_SuCommerce_Photos] PRIMARY KEY CLUSTERED 
(
	[PhotoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_PaymentServiceSettings]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_PaymentServiceSettings](
	[PaymentSettingID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentServiceID] [int] NOT NULL,
	[PaymentSettingName] [nvarchar](50) NULL,
	[PaymentSettingValue] [nvarchar](max) NULL,
	[PaymentSettingDescription] [nvarchar](255) NULL,
 CONSTRAINT [PK_SuCommerce_PaymentServiceSettings] PRIMARY KEY CLUSTERED 
(
	[PaymentSettingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_PaymentServices]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_PaymentServices](
	[PaymentServiceID] [int] NOT NULL,
	[PaymentServiceName] [nvarchar](50) NULL,
	[PaymentServiceDescription] [nvarchar](255) NULL,
	[SharedPaymentServicePage] [nvarchar](50) NULL,
	[IsPrimary] [bit] NOT NULL,
 CONSTRAINT [PK_SuCommerce_PaymentServices] PRIMARY KEY CLUSTERED 
(
	[PaymentServiceID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_Categories]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ParentCategoryID] [int] NOT NULL,
	[Path] [nvarchar](800) NULL,
	[CategoryName] [nvarchar](255) NULL,
	[NumActiveProducts] [int] NULL,
	[CategoryDescription] [nvarchar](1000) NULL,
 CONSTRAINT [PK_SuCommerce_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_CartLinks]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_CartLinks](
	[CartLinkID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[PackageTypeID] [int] NOT NULL,
	[LicenseTypeID] [int] NOT NULL,
	[Price] [money] NOT NULL,
	[Version] [numeric](4, 1) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SuCommerce_CartLinks] PRIMARY KEY CLUSTERED 
(
	[CartLinkID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_ActionTypes]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_ActionTypes](
	[ActionID] [int] NOT NULL,
	[ActionCode] [nvarchar](50) NULL,
	[ActionDescription] [nvarchar](255) NULL,
	[IsDisplayed] [bit] NOT NULL,
 CONSTRAINT [PK_SuCommerce_ActionTypes] PRIMARY KEY CLUSTERED 
(
	[ActionID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_ContentTypes]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_ContentTypes](
	[ContentTypeID] [int] NOT NULL,
	[ContentTypeCode] [nvarchar](50) NULL,
	[ContentTypeDescription] [nvarchar](50) NULL,
 CONSTRAINT [PK_SuCommerce_ContentTypes] PRIMARY KEY CLUSTERED 
(
	[ContentTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_PackageTypes]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_PackageTypes](
	[PackageTypeID] [int] NOT NULL,
	[PackageTypeCode] [nvarchar](25) NULL,
	[PackageTypeDescription] [nvarchar](255) NULL,
 CONSTRAINT [PK_SuCommerce_PackageTypes] PRIMARY KEY CLUSTERED 
(
	[PackageTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_LicenseTypes]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_LicenseTypes](
	[LicenseTypeID] [int] NOT NULL,
	[LicenseTypeDescription] [nvarchar](50) NOT NULL,
	[IsUsed] [bit] NULL,
	[IsDisplayed] [bit] NULL,
 CONSTRAINT [PK_SuCommerce_LicenseTypes] PRIMARY KEY CLUSTERED 
(
	[LicenseTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_Licenses]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_Licenses](
	[LicenseID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[PackageTypeID] [int] NOT NULL,
	[LicenseTypeID] [int] NOT NULL,
	[DatePurchased] [smalldatetime] NOT NULL,
	[Version] [numeric](4, 1) NOT NULL,
	[License] [nvarchar](60) NULL,
	[CartLinkID] [int] NOT NULL,
	[PurchaseID] [int] NOT NULL,
 CONSTRAINT [PK_SuCommerce_Licenses] PRIMARY KEY CLUSTERED 
(
	[LicenseID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_PurchaseTypes]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_PurchaseTypes](
	[PurchaseTypeID] [int] NOT NULL,
	[PurchaseTypeCode] [nvarchar](50) NULL,
	[PurchaseTypeDescription] [nvarchar](255) NULL,
	[IsDisplayed] [bit] NOT NULL,
 CONSTRAINT [PK_SuCommerce_PurchaseTypes] PRIMARY KEY CLUSTERED 
(
	[PurchaseTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_Purchases]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_Purchases](
	[PurchaseID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[PurchaseKey] [nvarchar](50) NOT NULL,
	[PurchaseDateTime] [datetime] NOT NULL,
	[CartLinkID] [int] NOT NULL,
	[TransactionXID] [nvarchar](50) NULL,
	[ActionID] [int] NOT NULL,
 CONSTRAINT [PK_SuCommerce_Purchases] PRIMARY KEY CLUSTERED 
(
	[PurchaseID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_StatusTypes]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_StatusTypes](
	[StatusTypeID] [int] NOT NULL,
	[StatusTypeCode] [nvarchar](50) NULL,
	[StatusTypeDescription] [nvarchar](255) NULL,
 CONSTRAINT [PK_SuCommerce_StatusTypes] PRIMARY KEY CLUSTERED 
(
	[StatusTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_Settings]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_Settings](
	[SettingName] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_SuCommerce_SettingsName] PRIMARY KEY CLUSTERED 
(
	[SettingName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_Setting_Update]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_Setting_Update]
	@SettingName nvarchar(50),
	@SettingValue ntext
AS

SET NOCOUNT ON

delete from  [dbo].[SuCommerce_Settings] where SettingName = @SettingName

insert into [dbo].[SuCommerce_Settings] Values (@SettingName, @SettingValue)
GO
PRINT '/****** Object:  Trigger [SuTrigger_OnCategoryInsert]    Script Date: 04/04/2011 12:56:01 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[SuTrigger_OnCategoryInsert]
ON [dbo].[SuCommerce_Categories]
FOR INSERT 
AS
DECLARE @numrows AS int
SET @numrows = @@rowcount
IF @numrows > 1
BEGIN
  RAISERROR('Only single row inserts are supported', 16, 1)
  ROLLBACK TRAN
END
ELSE
IF @numrows = 1
BEGIN
UPDATE SuCommerce_Categories
SET    
    Path = 
      CASE
          WHEN Inserted.ParentCategoryId = 0 THEN '.' 
          ELSE ParentCategory.Path
          END + CAST(Inserted.CategoryID AS varchar(10)) + '.'
FROM Inserted INNER JOIN
          SuCommerce_Categories ON Inserted.CategoryID = SuCommerce_Categories.CategoryID
          LEFT OUTER JOIN
          SuCommerce_Categories AS ParentCategory ON SuCommerce_Categories.ParentCategoryId = ParentCategory.CategoryID
            
END
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_License_Add]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_License_Add]
	@UserID int,
	@PackageTypeID int,
	@LicenseTypeID int,
	@Version numeric(4, 1),
	@License nvarchar(60),
	@CartLinkID int,
	@PurchaseID int

AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuCommerce_Licenses] (
	[UserID],
	[PackageTypeID],
	[LicenseTypeID],
	[DatePurchased],
	[Version],
	[License],
	[CartLinkID],
	[PurchaseID]
) VALUES (
	@UserID,
	@PackageTypeID,
	@LicenseTypeID,
	GETDATE(),
	@Version,
	@License,
	@CartLinkID,
	@PurchaseID
)
GO

PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_HasCategories]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_HasCategories]

AS

SET NOCOUNT ON

declare @hasCategories bit
declare @count int

select @count = count(*) from SuCommerce_Categories

if @count = 0
begin
select @hasCategories = 0
end
else begin
		SELECT 
			@hasCategories = 1 
end

select @hasCategories
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_CategoryCounts_Update]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SuCommerce_CategoryCounts_Update]
@LeafCategoryId int,
@NumberToAdd int
AS

SET NOCOUNT ON

UPDATE SuCommerce_Categories SET NumActiveProducts = NumActiveProducts + @NumberToAdd
WHERE CategoryID IN
	(SELECT CategoryID
	FROM SuCommerce_Categories
	WHERE (SELECT Path
	       FROM SuCommerce_Categories
	       WHERE CategoryID = @LeafCategoryId) LIKE Path + '%')
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_Category_Update]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_Category_Update]
(
	@CategoryID int,
	@CategoryName nvarchar(255),
	@CategoryDescription nvarchar(1000)
)
AS
SET NOCOUNT OFF;

update SuCommerce_Categories set
	CategoryName = @CategoryName,
	CategoryDescription = @CategoryDescription
where CategoryID = @CategoryID
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_PaymentService_SetPrimary]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_PaymentService_SetPrimary]
	@PaymentServiceID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

update SuCommerce_PaymentServices set IsPrimary = 0

update SuCommerce_PaymentServices set IsPrimary = 1 where paymentServiceID = @PaymentServiceID

END
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_PaymentService_Get]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_PaymentService_Get]
	@PaymentServiceID int,
	@GetPrimaryService bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


CREATE TABLE [dbo].[#SuCommerce_PaymentServices](
	[PaymentServiceID] [int]  NULL,
	[PaymentServiceName] [nvarchar](50) NULL,
	[PaymentServiceDescription] [nvarchar](255) NULL,
	[SharedPaymentServicePage] [nvarchar](50) NULL,
	[IsPrimary] [bit]  NULL,
	[AccountName] [nvarchar](50) NULL,
	[PurchaseUrl] [nvarchar](500) NULL,
	[CartUrl] [nvarchar](500) NULL,
	[TransactionUrl] [nvarchar](500) NULL,
	[ReturnUrl] [nvarchar](255) NULL,
	[ShoppingUrl] [nvarchar](255) NULL,
	[IdentityToken]  [nvarchar](255) NULL
	)

if @GetPrimaryService = 1
BEGIN
		insert into #SuCommerce_PaymentServices (
 			[PaymentServiceID],
			[PaymentServiceName],
			[PaymentServiceDescription],
			[SharedPaymentServicePage],
			[IsPrimary])
		select 
			paymentServiceID,
			paymentServicename,
			paymentServiceDescription,
			sharedpaymentServicePage,
			IsPrimary
		from SuCommerce_PaymentServices
		where [IsPrimary] = 1
		
		select @PaymentServiceID = PaymentServiceID from #SuCommerce_PaymentServices
		
END ELSE
BEGIN
		insert into #SuCommerce_PaymentServices (
 			[PaymentServiceID],
			[PaymentServiceName],
			[PaymentServiceDescription],
			[SharedPaymentServicePage],
			[IsPrimary])
		select 
			paymentServiceID,
			paymentServicename,
			paymentServiceDescription,
			sharedpaymentServicePage,
			IsPrimary
		from SuCommerce_PaymentServices
		where paymentServiceID = @paymentServiceID

END

declare @PaymentSettingServiceID int
declare @PaymentSettingName nvarchar(50)
declare @PaymentSettingValue nvarchar(500)
declare @column nvarchar(50)
declare @sql nvarchar(4000)

declare paymentCursor cursor for 
	select PaymentServiceID, PaymentSettingName, PaymentSettingValue from SuCommerce_PaymentServiceSettings
	where paymentServiceID = @PaymentServiceID

open paymentCursor

fetch next from paymentCursor into @PaymentSettingServiceID, @PaymentSettingName, @PaymentSettingValue 

while @@fetch_status = 0
begin
select @column = @PaymentSettingName
	set @sql = 'update #SuCommerce_PaymentServices set ' + @Column + ' = ''' + @PaymentSettingValue + ''''
		exec (@sql)
	--select @sql
	fetch next from paymentCursor into @PaymentSettingServiceID, @PaymentSettingName, @PaymentSettingValue 
	
end

close paymentCursor
deallocate paymentCursor

select * from #SuCommerce_PaymentServices

drop table #SuCommerce_PaymentServices
	
END
GO

PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_ParentCategoriesByID_Get]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_ParentCategoriesByID_Get] 
@CategoryID int
AS
SELECT CategoryID, ParentCategoryId, CategoryName, NumActiveProducts
FROM SuCommerce_Categories
WHERE (SELECT Path
       FROM SuCommerce_Categories
       WHERE CategoryID = @CategoryID) LIKE Path + '%'
ORDER BY Path
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_PaymentSetting_Update]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_PaymentSetting_Update]
	@PaymentServiceID int,
	@SettingName nvarchar(50),
	@SettingValue ntext
AS

SET NOCOUNT ON

UPDATE [dbo].[SuCommerce_PaymentServiceSettings] Set PaymentSettingValue = @SettingValue
	WHERE PaymentServiceID = @PaymentServiceID and PaymentSettingName = @SettingName
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_Category_Add]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_Category_Add]
(
	@ParentCategoryId int,
	@CategoryName nvarchar(255),
	@CategoryDescription nvarchar(1000),
 	@CategoryID int OUTPUT
)
AS
SET NOCOUNT OFF;

INSERT INTO SuCommerce_Categories
                      (ParentCategoryId, CategoryName, NumActiveProducts, CategoryDescription)
VALUES     (@ParentCategoryId,@CategoryName, 0, @CategoryDescription)

SET @CategoryID = @@IDENTITY;
SELECT @CategoryID AS [CategoryID]
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_CategoriesByParentID_Get]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SuCommerce_CategoriesByParentID_Get]
(
	@ParentCategoryID int = 0
)
AS
SET NOCOUNT ON;
SELECT    *
FROM         SuCommerce_Categories
WHERE    (ParentCategoryID = @ParentCategoryID)
ORDER BY CategoryName
GO
PRINT '/****** Object:  Table [dbo].[SuCommerce_Products]    Script Date: 04/04/2011 12:55:59 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SuCommerce_Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[Title] [nvarchar](255) NULL,
	[SubTitle] [nvarchar](500) NULL,
	[ProductDescription] [ntext] NULL,
	[DownloadURL] [nvarchar](255) NULL,
	[Price] [money] NULL,
	[ExpirationDate] [smalldatetime] NULL,
	[DateCreated] [smalldatetime] NULL,
	[DateApproved] [smalldatetime] NULL,
	[NumViews] [int] NOT NULL,
	[NumDownloads] [int] NOT NULL,
	[PurchaseTypeID] [int] NOT NULL,
	[PreviewImageID] [int] NOT NULL,
	[DocumentationURL] [nvarchar](255) NULL,
	[ImageGalleryURL] [nvarchar](255) NULL,
	[StatusTypeID] [int] NULL,
	[ProductTypeID] [int] NULL,
 CONSTRAINT [PK_SuCommerce_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_Category_Delete]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SuCommerce_Category_Delete]
(
	@CategoryID int
)
AS
SET NOCOUNT ON;

DECLARE @returnval int
SELECT @returnval = 1

-- cannot remove a category with products in it (they must be moved first)
---- return -1
IF EXISTS(SELECT CategoryID FROM SuCommerce_Products WHERE
			(SuCommerce_Products.CategoryId IN 
				(SELECT CategoryID FROM SuCommerce_Categories
				WHERE Path LIKE
					(SELECT Path
					FROM SuCommerce_Categories
					WHERE CategoryID = @CategoryID ) + '%')))
	SELECT @returnval = -1

-- cannot remove category if it is a parent of other categories
---- return -2
IF EXISTS(SELECT CategoryID FROM SuCommerce_Categories WHERE ParentCategoryId = @CategoryID)
    SELECT @returnval = -2

-- can remove category
IF (@returnval = 1)
  DELETE FROM [SuCommerce_Categories] WHERE [CategoryID] = @CategoryID

SELECT @returnval

RETURN 0
GO
PRINT '/****** Object:  Trigger [SuTrigger_ProductUpdated]    Script Date: 04/04/2011 12:56:01 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[SuTrigger_ProductUpdated]
ON [dbo].[SuCommerce_Products]
FOR	UPDATE
AS

DECLARE @CurrentProductID AS int

SET @CurrentProductID =(SELECT TOP 1 [ProductID]
	FROM Inserted
	ORDER BY [ProductID])


	-- loop through each row	
	WHILE @CurrentProductID IS NOT NULL
	BEGIN

		DECLARE	@StatusTypeID int;
		DECLARE	@CategoryID int;
		DECLARE	@OldStatusTypeID int;
		DECLARE	@OldCategoryID int;
	
		SELECT @StatusTypeID = Inserted.StatusTypeID,
			   @CategoryID = Inserted.CategoryID
		FROM Inserted
		WHERE ProductID = @CurrentProductID
		                    
		SELECT @OldStatusTypeID = Deleted.StatusTypeID,
		      @OldCategoryID = Deleted.CategoryID
		FROM Deleted
		WHERE ProductID = @CurrentProductID

		IF UPDATE(StatusTypeID) 
		BEGIN
		    IF (@StatusTypeID < 100 AND @OldStatusTypeID >= 100)
		        EXEC SuCommerce_CategoryCounts_Update @OldCategoryID, -1
		    ELSE IF (@StatusTypeID >= 100 AND @OldStatusTypeID < 100)
		        EXEC SuCommerce_CategoryCounts_Update @OldCategoryID, 1
		END
		
		IF UPDATE(CategoryID)
		BEGIN
		  IF (@OldCategoryID <> @CategoryID)
		     BEGIN
		         EXEC SuCommerce_CategoryCounts_Update @CategoryID, 1
		         EXEC SuCommerce_CategoryCounts_Update @OldCategoryID, -1
		     END
		END
			
		SET @CurrentProductID = (SELECT TOP 1 [ProductID]
					FROM Inserted
					WHERE [ProductID] > @CurrentProductID
					ORDER BY [ProductID])

	END
GO
PRINT '/****** Object:  Trigger [SuTrigger_ProductInserted]    Script Date: 04/04/2011 12:56:01 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[SuTrigger_ProductInserted]
ON [dbo].[SuCommerce_Products]
FOR INSERT
AS

DECLARE @CurrentProductID AS int

SET @CurrentProductID = (SELECT TOP 1 [ProductID]
			FROM Inserted
			ORDER BY [ProductID])


	-- loop through each row	
	WHILE @CurrentProductID IS NOT NULL
	BEGIN
		
		
		DECLARE	@StatusTypeID int;
		DECLARE	@CategoryID int;

		SELECT @StatusTypeID = StatusTypeID, @CategoryID = CategoryID
		FROM Inserted
		WHERE ProductID = @CurrentProductID;

		IF @StatusTypeID >= 100
		BEGIN
		    -- increment the affected category counts by adding +1
		    EXEC SuCommerce_CategoryCounts_Update @CategoryID, 1
		END

		SET @CurrentProductID = (SELECT TOP 1 [ProductID]
						FROM Inserted
						WHERE [ProductID] > @CurrentProductID
						ORDER BY [ProductID])

	END
GO
PRINT '/****** Object:  Trigger [SuTrigger_ProductDeleted]    Script Date: 04/04/2011 12:56:01 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[SuTrigger_ProductDeleted]
ON [dbo].[SuCommerce_Products]
FOR	DELETE
AS

DECLARE @CurrentProductID AS int
SET @CurrentProductID = 	(SELECT TOP 1 [ProductID]
			FROM Deleted
			ORDER BY [ProductID])


	-- loop through each row	
	WHILE @CurrentProductID IS NOT NULL
	BEGIN
	
		DECLARE	@StatusTypeID int;
		DECLARE	@CategoryID	int;
		
		SELECT @StatusTypeID = StatusTypeID, @CategoryID = CategoryId
		FROM Deleted
		WHERE ProductID = @CurrentProductID
		
		IF @StatusTypeID >= 100
		BEGIN
		    -- decrement the affected category counts by adding a -1
		    EXEC SuCommerce_CategoryCounts_Update @CategoryID, -1
		END

		SET @CurrentProductID = 	(SELECT TOP 1 [ProductID]
					FROM Deleted
					WHERE [ProductID] > @CurrentProductID
					ORDER BY [ProductID])

END
GO
PRINT '/****** Object:  View [dbo].[SuCommerce_vw_Purchases]    Script Date: 04/04/2011 12:56:01 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SuCommerce_vw_Purchases]
AS
SELECT     dbo.SuCommerce_Purchases.PurchaseID, dbo.SuCommerce_Purchases.ProductID, dbo.SuCommerce_Purchases.UserID, dbo.SuCommerce_Purchases.PurchaseKey, 
                      dbo.SuCommerce_Purchases.PurchaseDateTime, dbo.SuCommerce_Purchases.CartLinkID, dbo.SuCommerce_Products.ProductTypeID, 
                      dbo.SuCommerce_ProductTypes.ProductTypeCode, dbo.SuCommerce_CartLinks.Price, dbo.SuCommerce_Products.Title, dbo.SuCommerce_Products.CategoryID, 
                      dbo.SuCommerce_CartLinks.LicenseTypeID, dbo.SuCommerce_CartLinks.PackageTypeID, SuCommerce_LicenseTypes_1.LicenseTypeDescription, 
                      dbo.SuCommerce_PackageTypes.PackageTypeDescription, dbo.SuCommerce_Categories.CategoryName, dbo.SuCommerce_Purchases.TransactionXID, 
                      dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, dbo.Sueetie_Users.DisplayName, dbo.SuCommerce_Purchases.ActionID, 
                      dbo.SuCommerce_ActionTypes.ActionCode, dbo.SuCommerce_Products.PurchaseTypeID
FROM         dbo.SuCommerce_Purchases INNER JOIN
                      dbo.SuCommerce_Products ON dbo.SuCommerce_Purchases.ProductID = dbo.SuCommerce_Products.ProductID INNER JOIN
                      dbo.SuCommerce_CartLinks ON dbo.SuCommerce_Purchases.CartLinkID = dbo.SuCommerce_CartLinks.CartLinkID INNER JOIN
                      dbo.SuCommerce_ProductTypes ON dbo.SuCommerce_Products.ProductTypeID = dbo.SuCommerce_ProductTypes.ProductTypeID INNER JOIN
                      dbo.SuCommerce_Categories ON dbo.SuCommerce_Products.CategoryID = dbo.SuCommerce_Categories.CategoryID INNER JOIN
                      dbo.SuCommerce_PackageTypes ON dbo.SuCommerce_CartLinks.PackageTypeID = dbo.SuCommerce_PackageTypes.PackageTypeID INNER JOIN
                      dbo.SuCommerce_LicenseTypes AS SuCommerce_LicenseTypes_1 ON 
                      dbo.SuCommerce_CartLinks.LicenseTypeID = SuCommerce_LicenseTypes_1.LicenseTypeID INNER JOIN
                      dbo.Sueetie_Users ON dbo.SuCommerce_Purchases.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.SuCommerce_ActionTypes ON dbo.SuCommerce_Purchases.ActionID = dbo.SuCommerce_ActionTypes.ActionID
GO
PRINT '/****** Object:  View [dbo].[SuCommerce_vw_Products]    Script Date: 04/04/2011 12:56:01 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SuCommerce_vw_Products]
AS
SELECT     dbo.SuCommerce_Products.ProductID, dbo.SuCommerce_Products.UserID, dbo.SuCommerce_Products.CategoryID, dbo.SuCommerce_Products.Title, 
                      dbo.SuCommerce_Products.SubTitle, dbo.SuCommerce_Products.ProductDescription, dbo.SuCommerce_Products.DownloadURL, dbo.SuCommerce_Products.Price, 
                      dbo.SuCommerce_Products.ExpirationDate, dbo.SuCommerce_Products.DateCreated, dbo.SuCommerce_Products.DateApproved, 
                      dbo.SuCommerce_Products.NumViews, dbo.SuCommerce_Products.NumDownloads, dbo.SuCommerce_Products.PurchaseTypeID, 
                      dbo.SuCommerce_Products.PreviewImageID, dbo.SuCommerce_Products.DocumentationURL, dbo.SuCommerce_Products.ImageGalleryURL, 
                      dbo.SuCommerce_Products.StatusTypeID, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, dbo.Sueetie_Users.DisplayName, 
                      dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Content.ApplicationID, dbo.SuCommerce_StatusTypes.StatusTypeCode, 
                      dbo.SuCommerce_Categories.CategoryName, dbo.SuCommerce_Categories.Path AS CategoryPath, dbo.SuCommerce_PurchaseTypes.PurchaseTypeCode, 
                      dbo.SuCommerce_Products.ProductTypeID, dbo.SuCommerce_ProductTypes.ProductTypeCode, dbo.SuCommerce_ProductTypes.ProductTypeDescription
FROM         dbo.SuCommerce_Products INNER JOIN
                      dbo.Sueetie_Users ON dbo.SuCommerce_Products.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_Content ON dbo.SuCommerce_Products.ProductID = dbo.Sueetie_Content.SourceID INNER JOIN
                      dbo.SuCommerce_StatusTypes ON dbo.SuCommerce_Products.StatusTypeID = dbo.SuCommerce_StatusTypes.StatusTypeID INNER JOIN
                      dbo.SuCommerce_Categories ON dbo.SuCommerce_Products.CategoryID = dbo.SuCommerce_Categories.CategoryID INNER JOIN
                      dbo.SuCommerce_PurchaseTypes ON dbo.SuCommerce_Products.PurchaseTypeID = dbo.SuCommerce_PurchaseTypes.PurchaseTypeID INNER JOIN
                      dbo.SuCommerce_ProductTypes ON dbo.SuCommerce_Products.ProductTypeID = dbo.SuCommerce_ProductTypes.ProductTypeID
GO
PRINT '/****** Object:  View [dbo].[SuCommerce_vw_Licenses]    Script Date: 04/04/2011 12:56:01 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SuCommerce_vw_Licenses]
AS
SELECT     dbo.SuCommerce_Licenses.LicenseID, dbo.SuCommerce_Licenses.LicenseTypeID, dbo.SuCommerce_LicenseTypes.LicenseTypeDescription, 
                      dbo.SuCommerce_Licenses.UserID, dbo.SuCommerce_Licenses.DatePurchased, dbo.SuCommerce_Licenses.Version AS PurchasedVersion, 
                      dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.DisplayName, dbo.Sueetie_Users.Email, dbo.Sueetie_Users.MembershipID, 
                      dbo.yaf_User.UserID AS ForumUserID, dbo.yaf_User.BoardID, dbo.SuCommerce_Licenses.License, dbo.SuCommerce_PackageTypes.PackageTypeCode, 
                      dbo.SuCommerce_PackageTypes.PackageTypeDescription, dbo.SuCommerce_Licenses.CartLinkID, dbo.SuCommerce_Licenses.PackageTypeID, 
                      dbo.SuCommerce_CartLinks.Version, dbo.SuCommerce_CartLinks.ProductID, dbo.SuCommerce_Licenses.PurchaseID, dbo.SuCommerce_Purchases.PurchaseKey, 
                      dbo.SuCommerce_Purchases.TransactionXID, dbo.SuCommerce_CartLinks.Price
FROM         dbo.SuCommerce_Licenses INNER JOIN
                      dbo.SuCommerce_LicenseTypes ON dbo.SuCommerce_Licenses.LicenseTypeID = dbo.SuCommerce_LicenseTypes.LicenseTypeID INNER JOIN
                      dbo.Sueetie_Users ON dbo.SuCommerce_Licenses.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.yaf_User ON dbo.Sueetie_Users.MembershipID = dbo.yaf_User.ProviderUserKey INNER JOIN
                      dbo.SuCommerce_PackageTypes ON dbo.SuCommerce_Licenses.PackageTypeID = dbo.SuCommerce_PackageTypes.PackageTypeID INNER JOIN
                      dbo.SuCommerce_CartLinks ON dbo.SuCommerce_Licenses.CartLinkID = dbo.SuCommerce_CartLinks.CartLinkID INNER JOIN
                      dbo.SuCommerce_Purchases ON dbo.SuCommerce_Licenses.PurchaseID = dbo.SuCommerce_Purchases.PurchaseID
WHERE     (dbo.yaf_User.BoardID = 1)
GO
PRINT '/****** Object:  View [dbo].[SuCommerce_vw_CartLinks]    Script Date: 04/04/2011 12:56:01 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[SuCommerce_vw_CartLinks]
AS
SELECT     dbo.SuCommerce_CartLinks.CartLinkID, dbo.SuCommerce_CartLinks.ProductID, dbo.SuCommerce_CartLinks.LicenseTypeID, dbo.SuCommerce_CartLinks.Price, 
                      dbo.SuCommerce_CartLinks.PackageTypeID, dbo.SuCommerce_PackageTypes.PackageTypeCode, dbo.SuCommerce_LicenseTypes.LicenseTypeDescription, 
                      dbo.SuCommerce_LicenseTypes.IsDisplayed, dbo.SuCommerce_PackageTypes.PackageTypeDescription, dbo.SuCommerce_Products.Title, 
                      dbo.SuCommerce_Products.ProductTypeID, dbo.SuCommerce_ProductTypes.ProductTypeCode, dbo.SuCommerce_CartLinks.Version, 
                      dbo.SuCommerce_CartLinks.IsActive
FROM         dbo.SuCommerce_CartLinks INNER JOIN
                      dbo.SuCommerce_PackageTypes ON dbo.SuCommerce_CartLinks.PackageTypeID = dbo.SuCommerce_PackageTypes.PackageTypeID INNER JOIN
                      dbo.SuCommerce_LicenseTypes ON dbo.SuCommerce_CartLinks.LicenseTypeID = dbo.SuCommerce_LicenseTypes.LicenseTypeID INNER JOIN
                      dbo.SuCommerce_Products ON dbo.SuCommerce_CartLinks.ProductID = dbo.SuCommerce_Products.ProductID INNER JOIN
                      dbo.SuCommerce_ProductTypes ON dbo.SuCommerce_Products.ProductTypeID = dbo.SuCommerce_ProductTypes.ProductTypeID
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_Purchase_Record]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_Purchase_Record]
	@ProductID int,
	@UserID int,
	@PurchaseKey nvarchar(50),
	@CartLinkID int,
	@TransactionXID nvarchar(50),
	@ActionID int,
	@PurchaseID int OUTPUT
AS

SET NOCOUNT ON

select @purchaseID = 0
select @purchaseID = purchaseID from suCommerce_Purchases where PurchaseKey = @PurchaseKey
select @purchaseID

if @purchaseID = 0 
	begin
INSERT INTO [dbo].[SuCommerce_Purchases] (
	[ProductID],
	[UserID],
	[PurchaseKey],
	[PurchaseDateTime],
	[CartLinkID],
	[TransactionXID],
	[ActionID]
) VALUES (
	@ProductID,
	@UserID,
	@PurchaseKey,
	GETDATE(),
	@CartLinkID,
	@TransactionXID,
	@ActionID
)

SELECT @PurchaseID = SCOPE_IDENTITY() 
end

update SuCommerce_Products set NumDownloads = NumDownloads + 1 where ProductID = @ProductID
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_Photo_Add]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SuCommerce_Photo_Add]
@ProductID int,
@IsMainPreview bit = 0,
@PhotoID int OUTPUT
AS

INSERT SuCommerce_Photos
(ProductID, IsMainPreview, DateCreated)
VALUES
(@ProductID, @IsMainPreview, GETDATE())
SET @PhotoID = @@IDENTITY;

IF @IsMainPreview = 1
UPDATE SuCommerce_Products SET PreviewImageId = @PhotoID WHERE ProductID= @ProductID

RETURN @PhotoID
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_Product_Update]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_Product_Update]
	@ProductID int,
	@CategoryID int,
	@Title nvarchar(255),
	@SubTitle nvarchar(500),
	@ProductDescription ntext,
	@DownloadURL nvarchar(255),
	@Price money,
	@PurchaseTypeID int,
	@ProductTypeID int,
	@StatusTypeID int
AS


SET NOCOUNT ON

UPDATE [dbo].[SuCommerce_Products] SET
	[CategoryID] = @CategoryID,
	[Title] = @Title,
	[SubTitle] = @SubTitle,
	[ProductDescription] = @ProductDescription,
	[DownloadURL] = @DownloadURL,
	[Price] = @Price,
	[PurchaseTypeID] = @PurchaseTypeID,
	[ProductTypeID] = @ProductTypeID,
	[StatusTypeID] = @StatusTypeID
WHERE
	[ProductID] = @ProductID

DECLARE @cartnum int
SELECT @cartnum = COUNT(*) from SuCommerce_CartLinks where ProductID = @ProductID

if @cartnum = 1
BEGIN
	UPDATE SuCommerce_CartLinks set Price = @Price where ProductID = @ProductID
END
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_Product_Add]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SuCommerce_Product_Add]
	@UserID int,
	@CategoryID int,
	@Title nvarchar(255),
	@SubTitle nvarchar(500),
	@ProductDescription ntext,
	@DownloadURL nvarchar(255),
	@Price money,
	@PurchaseTypeID int,
	@PreviewImageID int,
	@DocumentationURL nvarchar(255),
	@ImageGalleryURL nvarchar(255),
	@StatusTypeID int,
	@ProductTypeID int,
 	@ProductID int OUTPUT
AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuCommerce_Products] (
	[UserID],
	[CategoryID],
	[Title],
	[SubTitle],
	[ProductDescription],
	[DownloadURL],
	[Price],
	[DateCreated],
	[PurchaseTypeID],
	[PreviewImageID],
	[DocumentationURL],
	[ImageGalleryURL],
	[StatusTypeID],
	[ProductTypeID]
) VALUES (
	@UserID,
	@CategoryID,
	@Title,
	@SubTitle,
	@ProductDescription,
	@DownloadURL,
	@Price,
	GETDATE(),
	@PurchaseTypeID,
	@PreviewImageID,
	@DocumentationURL,
	@ImageGalleryURL,
	@StatusTypeID,
	@ProductTypeID
)
	
SET @ProductID = @@IDENTITY


INSERT INTO SuCommerce_CartLinks select @ProductID,-1,-1,@Price, 1.0, 1

SELECT @ProductID
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_PreviewPhoto_Set]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SuCommerce_PreviewPhoto_Set]
@PhotoID int,
@ProductID int
AS
UPDATE SuCommerce_Photos SET
	IsMainPreview = 1 - ABS(SIGN(@PhotoID - PhotoID))
WHERE ProductID = @ProductID

UPDATE SuCommerce_Products
	SET PreviewImageId = @PhotoID
WHERE
	ProductID = @ProductID
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_ProductsByCategory_Get]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuCommerce_ProductsByCategory_Get]
@CategoryId int,
@ContentTypeID int
AS
	
	SELECT *
	FROM SuCommerce_vw_Products
	WHERE contenttypeid = @ContentTypeID  and
	(CategoryId = @CategoryId OR 
	CategoryId IN (
	SELECT CategoryID FROM SuCommerce_Categories
	WHERE Path LIKE
	  (SELECT Path
	   FROM SuCommerce_Categories
	   WHERE CategoryID = @CategoryId ) + '%'
	))
    ORDER BY DateCreated DESC
    
	SET ROWCOUNT 0
GO
PRINT '/****** Object:  StoredProcedure [dbo].[SuCommerce_Category_Move]    Script Date: 04/04/2011 12:55:56 ******/'
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SuCommerce_Category_Move]
@CategoryID int,
@NewParentCategoryID int
AS
declare @NumActiveProductsInCategory int;
declare @OldPath varchar(800);
declare @NewPath varchar(800);

-- Stored PROCEDURE mp_will not execute, if:

-- 1st case: Current Category = New Parent Category (it cannot become a parent of itself)
IF @CategoryID = @NewParentCategoryID
	RETURN -1

-- 2nd case: the input category is a parent of the new category
-- (a parent category cannot become a child category under one of its own children)
IF EXISTS 	(SELECT CategoryID FROM SuCommerce_Categories
		WHERE
			(SELECT Path
			FROM SuCommerce_Categories
			WHERE
			CategoryID = @CategoryID) LIKE Path + '%'
		AND
			CategoryID = @NewParentCategoryID)
	RETURN -1 -- exits

SELECT @OldPath = SuCommerce_Categories.Path

FROM         SuCommerce_Categories INNER JOIN
                      SuCommerce_Categories ParentCategories ON SuCommerce_Categories.ParentCategoryID = ParentCategories.CategoryID
WHERE     (SuCommerce_Categories.CategoryID = @CategoryID)

SELECT @NewPath =
	CASE WHEN
		@NewParentCategoryID = 0 THEN '.' + CAST(@CategoryID AS varchar(10)) + '.'
	ELSE 
		Path
	END 
	FROM SuCommerce_Categories
	WHERE @NewParentCategoryID = 0 OR CategoryID = @NewParentCategoryID

IF @OldPath IS NULL
BEGIN
	SELECT @OldPath = Path FROM SuCommerce_Categories WHERE CategoryID = @CategoryID
	SET @NewPath = @NewPath + CAST(@CategoryID AS VARCHAR(10)) + '.'
END


SELECT @NumActiveProductsInCategory  = Count(CategoryID) FROM SuCommerce_vw_Products WHERE 
StatusTypeID >= 100 AND ContentTypeID = 18 AND
(ExpirationDate > getdate() OR ExpirationDate is NULL) AND
SuCommerce_vw_Products.CategoryID IN (SELECT CategoryID FROM SuCommerce_Categories
WHERE Path LIKE
  (SELECT Path
   FROM SuCommerce_Categories
   WHERE CategoryID = @CategoryID ) + '%' )

DECLARE @NegativeCount int;
SET @NegativeCount = 0 - @NumActiveProductsInCategory 
EXEC SuCommerce_CategoryCounts_Update @CategoryID, @NegativeCount 

UPDATE SuCommerce_Categories
SET Path = 
	REPLACE(Path, @OldPath, @NewPath)

WHERE Path LIKE

  (SELECT Path
   FROM SuCommerce_Categories
   WHERE CategoryID = @CategoryID ) + '%'

EXEC SuCommerce_CategoryCounts_Update @CategoryID, @NumActiveProductsInCategory 

UPDATE SuCommerce_Categories SET ParentCategoryID = @NewParentCategoryID WHERE CategoryID = @CategoryID

RETURN 1
GO
PRINT '/****** Object:  Default [DF__SuCommerc__IsDis__006AEB82]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_ActionTypes] ADD  DEFAULT ((0)) FOR [IsDisplayed]
GO
PRINT '/****** Object:  Default [DF_SuCommerce_CartLinks_Price]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_CartLinks] ADD  CONSTRAINT [DF_SuCommerce_CartLinks_Price]  DEFAULT ((0)) FOR [Price]
GO
PRINT '/****** Object:  Default [DF__SuCommerc__Versi__6F405F80]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_CartLinks] ADD  DEFAULT ((0)) FOR [Version]
GO
PRINT '/****** Object:  Default [DF__SuCommerc__IsAct__703483B9]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_CartLinks] ADD  DEFAULT ((1)) FOR [IsActive]
GO
PRINT '/****** Object:  Default [DF_SuCommerce_Categories_ParentCategoryID]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Categories] ADD  CONSTRAINT [DF_SuCommerce_Categories_ParentCategoryID]  DEFAULT ((0)) FOR [ParentCategoryID]
GO
PRINT '/****** Object:  Default [DF__SuCommerc__CartL__7405149D]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Licenses] ADD  DEFAULT ((-1)) FOR [CartLinkID]
GO
PRINT '/****** Object:  Default [DF__SuCommerc__Purch__75ED5D0F]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Licenses] ADD  DEFAULT ((-1)) FOR [PurchaseID]
GO
PRINT '/****** Object:  Default [DF_SuCommerce_PaymentServices_IsPrimary]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_PaymentServices] ADD  CONSTRAINT [DF_SuCommerce_PaymentServices_IsPrimary]  DEFAULT ((0)) FOR [IsPrimary]
GO
PRINT '/****** Object:  Default [DF_CommercePhotos_IsMainPreview]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Photos] ADD  CONSTRAINT [DF_CommercePhotos_IsMainPreview]  DEFAULT ((0)) FOR [IsMainPreview]
GO
PRINT '/****** Object:  Default [DF_CommercePhotos_DateCreated]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Photos] ADD  CONSTRAINT [DF_CommercePhotos_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
PRINT '/****** Object:  Default [DF_SuCommerce_Products_NumViews]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Products] ADD  CONSTRAINT [DF_SuCommerce_Products_NumViews]  DEFAULT ((0)) FOR [NumViews]
GO
PRINT '/****** Object:  Default [DF_SuCommerce_Products_NumDownloads]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Products] ADD  CONSTRAINT [DF_SuCommerce_Products_NumDownloads]  DEFAULT ((0)) FOR [NumDownloads]
GO
PRINT '/****** Object:  Default [DF_SuCommerce_Products_PurchaseTypeID]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Products] ADD  CONSTRAINT [DF_SuCommerce_Products_PurchaseTypeID]  DEFAULT ((0)) FOR [PurchaseTypeID]
GO
PRINT '/****** Object:  Default [DF_SuCommerce_Products_PreviewImageID]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Products] ADD  CONSTRAINT [DF_SuCommerce_Products_PreviewImageID]  DEFAULT ((0)) FOR [PreviewImageID]
GO
PRINT '/****** Object:  Default [DF_Table_1_ProductStatusID]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Products] ADD  CONSTRAINT [DF_Table_1_ProductStatusID]  DEFAULT ((0)) FOR [StatusTypeID]
GO
PRINT '/****** Object:  Default [DF_Table_1_ProductContentTypeID]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Products] ADD  CONSTRAINT [DF_Table_1_ProductContentTypeID]  DEFAULT ((0)) FOR [ProductTypeID]
GO
PRINT '/****** Object:  Default [DF__SuCommerc__IsDis__7E82A310]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_ProductTypes] ADD  DEFAULT ((0)) FOR [IsDisplayed]
GO
PRINT '/****** Object:  Default [DF__SuCommerc__CartL__721CCC2B]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Purchases] ADD  DEFAULT ((-1)) FOR [CartLinkID]
GO
PRINT '/****** Object:  Default [DF__SuCommerc__Actio__7310F064]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_Purchases] ADD  DEFAULT ((-1)) FOR [ActionID]
GO
PRINT '/****** Object:  Default [DF__SuCommerc__IsDis__7F76C749]    Script Date: 04/04/2011 12:55:59 ******/'
ALTER TABLE [dbo].[SuCommerce_PurchaseTypes] ADD  DEFAULT ((0)) FOR [IsDisplayed]
GO



print 'INSERT INTO [dbo].[SuCommerce_ActionTypes]'

/* -- INSERT INTO [dbo].[SuCommerce_ActionTypes] ---------------------------------------------------- */

INSERT INTO [dbo].[SuCommerce_ActionTypes] (ActionID, ActionCode, ActionDescription, IsDisplayed) VALUES (-1, 'NA', 'Other or Not Known', 0);
INSERT INTO [dbo].[SuCommerce_ActionTypes] (ActionID, ActionCode, ActionDescription, IsDisplayed) VALUES (1, 'File Download', 'Electronic File Download', 1);
INSERT INTO [dbo].[SuCommerce_ActionTypes] (ActionID, ActionCode, ActionDescription, IsDisplayed) VALUES (2, 'Product License', 'License for Software Product', 0);
INSERT INTO [dbo].[SuCommerce_ActionTypes] (ActionID, ActionCode, ActionDescription, IsDisplayed) VALUES (3, 'Electronic Product Purchase', 'Paid Purchase of an Electronic Product not requiring a license', 1);
INSERT INTO [dbo].[SuCommerce_ActionTypes] (ActionID, ActionCode, ActionDescription, IsDisplayed) VALUES (4, 'Physical Product Purchase', 'Purchase of a Physical Item', 1);
INSERT INTO [dbo].[SuCommerce_ActionTypes] (ActionID, ActionCode, ActionDescription, IsDisplayed) VALUES (5, 'Subscription Payment', 'Payment of a Subscription Service', 0);
INSERT INTO [dbo].[SuCommerce_ActionTypes] (ActionID, ActionCode, ActionDescription, IsDisplayed) VALUES (6, 'Service', 'Payment for Services Rendered', 0);
GO


print 'INSERT INTO [dbo].[SuCommerce_CartLinks]'

/* -- INSERT INTO [dbo].[SuCommerce_CartLinks] ---------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[SuCommerce_CartLinks] ON

INSERT INTO [dbo].[SuCommerce_CartLinks] (CartLinkID, ProductID, PackageTypeID, LicenseTypeID, Price, Version, IsActive) VALUES (-1, -1, -1, -1, 0.0000, 3.0, 1);

SET IDENTITY_INSERT [dbo].[SuCommerce_CartLinks] OFF
GO

print 'INSERT INTO [dbo].[SuCommerce_LicenseTypes]'

/* -- INSERT INTO [dbo].[SuCommerce_LicenseTypes] ---------------------------------------------------- */


INSERT INTO [dbo].[SuCommerce_LicenseTypes] (LicenseTypeID, LicenseTypeDescription, IsUsed, IsDisplayed) VALUES (-1, 'Not Valid', 1, 0);
INSERT INTO [dbo].[SuCommerce_LicenseTypes] (LicenseTypeID, LicenseTypeDescription, IsUsed, IsDisplayed) VALUES (0, 'Trial Period', 1, 0);
INSERT INTO [dbo].[SuCommerce_LicenseTypes] (LicenseTypeID, LicenseTypeDescription, IsUsed, IsDisplayed) VALUES (1, 'Free', 1, 0);
INSERT INTO [dbo].[SuCommerce_LicenseTypes] (LicenseTypeID, LicenseTypeDescription, IsUsed, IsDisplayed) VALUES (2, 'Personal', 0, 0);
INSERT INTO [dbo].[SuCommerce_LicenseTypes] (LicenseTypeID, LicenseTypeDescription, IsUsed, IsDisplayed) VALUES (11, 'Sueetie Insider', 1, 0);
INSERT INTO [dbo].[SuCommerce_LicenseTypes] (LicenseTypeID, LicenseTypeDescription, IsUsed, IsDisplayed) VALUES (12, 'Entrepreneur', 1, 0);
INSERT INTO [dbo].[SuCommerce_LicenseTypes] (LicenseTypeID, LicenseTypeDescription, IsUsed, IsDisplayed) VALUES (13, 'Small Business', 1, 0);
INSERT INTO [dbo].[SuCommerce_LicenseTypes] (LicenseTypeID, LicenseTypeDescription, IsUsed, IsDisplayed) VALUES (14, 'Corporate', 1, 0);
INSERT INTO [dbo].[SuCommerce_LicenseTypes] (LicenseTypeID, LicenseTypeDescription, IsUsed, IsDisplayed) VALUES (15, 'Enterprise', 1, 0);
INSERT INTO [dbo].[SuCommerce_LicenseTypes] (LicenseTypeID, LicenseTypeDescription, IsUsed, IsDisplayed) VALUES (16, 'Evaluation', 1, 0);
GO

print 'INSERT INTO [dbo].[SuCommerce_PackageTypes]'

/* -- INSERT INTO [dbo].[SuCommerce_PackageTypes] ---------------------------------------------------- */

INSERT INTO [dbo].[SuCommerce_PackageTypes] (PackageTypeID, PackageTypeCode, PackageTypeDescription) VALUES (-1, 'NA', 'Unknown');
GO

print 'INSERT INTO [dbo].[SuCommerce_PaymentServices]'

/* -- INSERT INTO [dbo].[SuCommerce_PaymentServices] ---------------------------------------------------- */


INSERT INTO [dbo].[SuCommerce_PaymentServices] (PaymentServiceID, PaymentServiceName, PaymentServiceDescription, SharedPaymentServicePage, IsPrimary) VALUES (-1, 'na', 'Not Defined or Applicable', 'na', 0);
INSERT INTO [dbo].[SuCommerce_PaymentServices] (PaymentServiceID, PaymentServiceName, PaymentServiceDescription, SharedPaymentServicePage, IsPrimary) VALUES (1, 'PayPal Standard', 'Website Payments Standard. Processing on PayPal', 'PaypalStandard', 0);
INSERT INTO [dbo].[SuCommerce_PaymentServices] (PaymentServiceID, PaymentServiceName, PaymentServiceDescription, SharedPaymentServicePage, IsPrimary) VALUES (2, 'PayPal Sandbox', 'Sandbox for PayPal Website Payments Standard.', 'PaypalStandard', 1);
GO

print 'INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings]'

/* -- INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] ---------------------------------------------------- */


SET IDENTITY_INSERT [dbo].[SuCommerce_PaymentServiceSettings] ON

INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (1, 1, 'AccountName', 'your_paypal_account@email.com', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (2, 1, 'PurchaseUrl', 'https://www.paypal.com/cgi-bin/webscr?cmd=_cart&amp;add=1&amp;business={0}&amp;item_number={1}&amp;item_name={2}&amp;amount={3}&amp;no_shipping=0&amp;no_note=1&amp;currency_code=USD&amp;lc=US&amp;bn=PP-ShopCartBF&amp;return={4}&amp;shopping_url={5}', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (3, 1, 'CartUrl', 'https://www.paypal.com/cgi-bin/webscr?cmd=_cart&amp;display=1&amp;business={0}&amp;return={1}&amp;shopping_url={2}', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (4, 1, 'TransactionUrl', 'https://www.paypal.com/cgi-bin/webscr', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (5, 1, 'ReturnUrl', 'http://yourlivesite.com/marketplace/completepurchase.aspx', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (6, 1, 'ShoppingUrl', 'http://yourlivesite.com/marketplace/default.aspx', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (7, 1, 'IdentityToken', NULL, NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (8, 2, 'AccountName', 'your_sandbox_biz@email.com', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (9, 2, 'PurchaseUrl', 'https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_cart&amp;add=1&amp;business={0}&amp;item_number={1}&amp;item_name={2}&amp;amount={3}&amp;no_shipping=0&amp;no_note=1&amp;currency_code=USD&amp;lc=US&amp;bn=PP-ShopCartBF&amp;return={4}&amp;shopping_url={5}', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (10, 2, 'CartUrl', 'https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_cart&amp;display=1&amp;business={0}&amp;return={1}&amp;shopping_url={2}', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (11, 2, 'TransactionUrl', 'https://www.sandbox.paypal.com/cgi-bin/webscr', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (12, 2, 'ReturnUrl', 'http://yourdevsite/marketplace/completepurchase.aspx', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (13, 2, 'ShoppingUrl', 'http://yourdevsite/marketplace/default.aspx', NULL);
INSERT INTO [dbo].[SuCommerce_PaymentServiceSettings] (PaymentSettingID, PaymentServiceID, PaymentSettingName, PaymentSettingValue, PaymentSettingDescription) VALUES (14, 2, 'IdentityToken', NULL, NULL);

SET IDENTITY_INSERT [dbo].[SuCommerce_PaymentServiceSettings] OFF
GO

print 'INSERT INTO [dbo].[SuCommerce_ProductTypes]'

/* -- INSERT INTO [dbo].[SuCommerce_ProductTypes] ---------------------------------------------------- */

INSERT INTO [dbo].[SuCommerce_ProductTypes] (ProductTypeID, ProductTypeCode, ProductTypeDescription, IsDisplayed) VALUES (0, 'NA', 'Not Available', 0);
INSERT INTO [dbo].[SuCommerce_ProductTypes] (ProductTypeID, ProductTypeCode, ProductTypeDescription, IsDisplayed) VALUES (1, 'Electronic Download', 'Software or Other Downloaded File', 1);
INSERT INTO [dbo].[SuCommerce_ProductTypes] (ProductTypeID, ProductTypeCode, ProductTypeDescription, IsDisplayed) VALUES (2, 'Product License', 'Product License', 0);
INSERT INTO [dbo].[SuCommerce_ProductTypes] (ProductTypeID, ProductTypeCode, ProductTypeDescription, IsDisplayed) VALUES (3, 'Subscription', 'Subscription Service', 0);
INSERT INTO [dbo].[SuCommerce_ProductTypes] (ProductTypeID, ProductTypeCode, ProductTypeDescription, IsDisplayed) VALUES (4, 'Physical Product', 'Physical Product', 1);
INSERT INTO [dbo].[SuCommerce_ProductTypes] (ProductTypeID, ProductTypeCode, ProductTypeDescription, IsDisplayed) VALUES (5, 'Sueetie Package', 'Sueetie Application Requiring a Product Key', 0);
INSERT INTO [dbo].[SuCommerce_ProductTypes] (ProductTypeID, ProductTypeCode, ProductTypeDescription, IsDisplayed) VALUES (6, 'NonLicensed Software', 'Commercial Software not requiring license', 0);
INSERT INTO [dbo].[SuCommerce_ProductTypes] (ProductTypeID, ProductTypeCode, ProductTypeDescription, IsDisplayed) VALUES (7, 'Service', 'Services', 0);
GO

print 'INSERT INTO [dbo].[SuCommerce_PurchaseTypes]'

/* -- INSERT INTO [dbo].[SuCommerce_PurchaseTypes] ---------------------------------------------------- */

INSERT INTO [dbo].[SuCommerce_PurchaseTypes] (PurchaseTypeID, PurchaseTypeCode, PurchaseTypeDescription, IsDisplayed) VALUES (0, 'Unspecified', 'Not Specified or Recorded', 0);
INSERT INTO [dbo].[SuCommerce_PurchaseTypes] (PurchaseTypeID, PurchaseTypeCode, PurchaseTypeDescription, IsDisplayed) VALUES (1, 'Commercial', 'Purchased Item', 1);
INSERT INTO [dbo].[SuCommerce_PurchaseTypes] (PurchaseTypeID, PurchaseTypeCode, PurchaseTypeDescription, IsDisplayed) VALUES (2, 'Free Registered', 'Free to all Registered Members', 1);
INSERT INTO [dbo].[SuCommerce_PurchaseTypes] (PurchaseTypeID, PurchaseTypeCode, PurchaseTypeDescription, IsDisplayed) VALUES (3, 'Free Subscribers', 'Free to all Subscribers', 0);
INSERT INTO [dbo].[SuCommerce_PurchaseTypes] (PurchaseTypeID, PurchaseTypeCode, PurchaseTypeDescription, IsDisplayed) VALUES (4, 'Free All', 'Free to All', 1);
INSERT INTO [dbo].[SuCommerce_PurchaseTypes] (PurchaseTypeID, PurchaseTypeCode, PurchaseTypeDescription, IsDisplayed) VALUES (5, 'Donation', 'Donation or Contribution', 0);
GO

print 'INSERT INTO [dbo].[SuCommerce_StatusTypes]'

/* -- INSERT INTO [dbo].[SuCommerce_StatusTypes] ---------------------------------------------------- */

INSERT INTO [dbo].[SuCommerce_StatusTypes] (StatusTypeID, StatusTypeCode, StatusTypeDescription) VALUES (0, 'na', 'Not Recorded');
INSERT INTO [dbo].[SuCommerce_StatusTypes] (StatusTypeID, StatusTypeCode, StatusTypeDescription) VALUES (1, 'Pending Approval', 'Not Yet Active');
INSERT INTO [dbo].[SuCommerce_StatusTypes] (StatusTypeID, StatusTypeCode, StatusTypeDescription) VALUES (2, 'Expired', 'Past Expiration Date');
INSERT INTO [dbo].[SuCommerce_StatusTypes] (StatusTypeID, StatusTypeCode, StatusTypeDescription) VALUES (3, 'Sold Out', 'Product Sold Out');
INSERT INTO [dbo].[SuCommerce_StatusTypes] (StatusTypeID, StatusTypeCode, StatusTypeDescription) VALUES (4, 'No Longer Offered', 'Product No Longer For Sale');
INSERT INTO [dbo].[SuCommerce_StatusTypes] (StatusTypeID, StatusTypeCode, StatusTypeDescription) VALUES (100, 'Active', 'Active');
GO

print 'INSERT INTO [dbo].[SuCommerce_Licenses]'

/* -- INSERT INTO [dbo].[SuCommerce_Licenses] ---------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[SuCommerce_Licenses] ON

INSERT INTO [dbo].[SuCommerce_Licenses] (LicenseID, UserID, PackageTypeID, LicenseTypeID, DatePurchased, Version, License, CartLinkID, PurchaseID) VALUES (-1, -1, -1, -1, '03-26-2011 20:56:00.000', 1.0, '13A2A74E-240E-4FF4-BED8-95F1AF27ACFA', -1, -1);

SET IDENTITY_INSERT [dbo].[SuCommerce_Licenses] OFF
GO

print 'INSERT INTO [dbo].[SuCommerce_Settings]'

/* -- INSERT INTO [dbo].[SuCommerce_Settings] ---------------------------------------------------- */

insert into SuCommerce_Settings select 'ActivityReportNum','500'
insert into SuCommerce_Settings select 'FixedMediumImageHeight','102'
insert into SuCommerce_Settings select 'FixedMediumImageWidth','136'
insert into SuCommerce_Settings select 'FixedSmallImageHeight','42'
insert into SuCommerce_Settings select 'FixedSmallImageWidth','56'
insert into SuCommerce_Settings select 'MaxFullImageSize','450'
GO

print 'CREATE TABLE [dbo].[SuAddons_Slideshows]'

/* -- CREATE TABLE [dbo].[SuAddons_Slideshows] -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuAddons_Slideshows]    Script Date: 05/19/2011 12:41:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_Slideshows](
	[SlideshowID] [int] IDENTITY(1,1) NOT NULL,
	[ContentID] [int] NOT NULL,
	[SlideshowTitle] [nvarchar](255) NULL,
	[SlideshowDescription] [nvarchar](max) NULL,
	[FullFixedHeight] [int] NOT NULL,
	[FullFixedWidth] [int] NOT NULL,
	[MediumFixedHeight] [int] NOT NULL,
	[MediumFixedWidth] [int] NOT NULL,
	[SmallFixedHeight] [int] NOT NULL,
	[SmallFixedWidth] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SuAddons_Slideshows] PRIMARY KEY CLUSTERED 
(
	[SlideshowID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAddons_Slideshows] ADD  CONSTRAINT [DF_SuAddons_Slideshows_SourceID]  DEFAULT ((-1)) FOR [ContentID]
GO

ALTER TABLE [dbo].[SuAddons_Slideshows] ADD  CONSTRAINT [DF_SuAddons_Slideshows_FullFixedHeight]  DEFAULT ((-1)) FOR [FullFixedHeight]
GO

ALTER TABLE [dbo].[SuAddons_Slideshows] ADD  CONSTRAINT [DF_SuAddons_Slideshows_FullFixedWidth]  DEFAULT ((-1)) FOR [FullFixedWidth]
GO

ALTER TABLE [dbo].[SuAddons_Slideshows] ADD  CONSTRAINT [DF_SuAddons_Slideshows_MediumFixedHeight]  DEFAULT ((-1)) FOR [MediumFixedHeight]
GO

ALTER TABLE [dbo].[SuAddons_Slideshows] ADD  CONSTRAINT [DF_SuAddons_Slideshows_MediumFixedWidth]  DEFAULT ((-1)) FOR [MediumFixedWidth]
GO

ALTER TABLE [dbo].[SuAddons_Slideshows] ADD  CONSTRAINT [DF_SuAddons_Slideshows_SmallFixedHeight]  DEFAULT ((-1)) FOR [SmallFixedHeight]
GO

ALTER TABLE [dbo].[SuAddons_Slideshows] ADD  CONSTRAINT [DF_SuAddons_Slideshows_SmallFixedWidth]  DEFAULT ((-1)) FOR [SmallFixedWidth]
GO

ALTER TABLE [dbo].[SuAddons_Slideshows] ADD  CONSTRAINT [DF_SuAddons_Slideshows_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO



print 'CREATE TABLE [dbo].[SuAddons_SlideshowImages]'

/* -- CREATE TABLE [dbo].[SuAddons_SlideshowImages] -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuAddons_SlideshowImages]    Script Date: 04/11/2011 13:15:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_SlideshowImages](
	[SlideshowImageID] [int] IDENTITY(1,1) NOT NULL,
	[SlideshowID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[ImageDescription] [nvarchar](500) NULL,
	[DateTimeCreated] [smalldatetime] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SuAddons_SlideshowImages] PRIMARY KEY CLUSTERED 
(
	[SlideshowImageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAddons_SlideshowImages] ADD  CONSTRAINT [DF_SuAddons_DateCreated]  DEFAULT (getdate()) FOR [DateTimeCreated]
GO

ALTER TABLE [dbo].[SuAddons_SlideshowImages] ADD  CONSTRAINT [DF_SuAddons_DisplayOrder]  DEFAULT ((-1)) FOR [DisplayOrder]
GO

ALTER TABLE [dbo].[SuAddons_SlideshowImages] ADD  CONSTRAINT [DF_SuAddons_SlideshowImages_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO




print 'CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Add] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[Sueetie_Slideshow_Add]    Script Date: 04/11/2011 09:02:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Add]
	@SlideshowTitle nvarchar(255),
	@SlideshowDescription nvarchar(max),
	@FullFixedHeight int,
	@FullFixedWidth int,
	@MediumFixedHeight int,
	@MediumFixedWidth int,
	@SmallFixedHeight int,
	@SmallFixedWidth int

AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuAddons_Slideshows] (
	[SlideshowTitle],
	[SlideshowDescription],
	[FullFixedHeight],
	[FullFixedWidth],
	[MediumFixedHeight],
	[MediumFixedWidth],
	[SmallFixedHeight],
	[SmallFixedWidth],
	[IsActive]
) VALUES (
	@SlideshowTitle,
	@SlideshowDescription,
	@FullFixedHeight,
	@FullFixedWidth,
	@MediumFixedHeight,
	@MediumFixedWidth,
	@SmallFixedHeight,
	@SmallFixedWidth,
	1
)
GO

print 'CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Update] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_Slideshow_Update]    Script Date: 04/11/2011 09:08:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Update]
	@SlideshowID int,
	@SlideshowTitle nvarchar(255),
	@SlideshowDescription nvarchar(max),
	@IsActive bit,
	@FullFixedHeight int,
	@FullFixedWidth int,
	@MediumFixedHeight int,
	@MediumFixedWidth int,
	@SmallFixedHeight int,
	@SmallFixedWidth int
AS

SET NOCOUNT ON

UPDATE [dbo].[SuAddons_Slideshows] SET
	[SlideshowTitle] = @SlideshowTitle,
	[SlideshowDescription] = @SlideshowDescription,
	[IsActive] = @IsActive,
	[FullFixedHeight] = @FullFixedHeight,
	[FullFixedWidth] = @FullFixedWidth,
	[MediumFixedHeight] = @MediumFixedHeight,
	[MediumFixedWidth] = @MediumFixedWidth,
	[SmallFixedHeight] = @SmallFixedHeight,
	[SmallFixedWidth] = @SmallFixedWidth
WHERE
	[SlideshowID] = @SlideshowID
GO


print 'CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Add] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_SlideshowImage_Add]    Script Date: 04/11/2011 14:28:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Add]
	@SlideshowID int,
    @UserID int,
	@ImageDescription nvarchar(500),
	@DisplayOrder int,
	@SlideshowImageID int OUTPUT
AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuAddons_SlideshowImages] (
	[SlideshowID],
    [UserID],
	[ImageDescription],	
	[DateTimeCreated],	
	[DisplayOrder],
	[IsActive]
) VALUES (
	@SlideshowID,
    @UserID,
	@ImageDescription,
	GETDATE(),
	@DisplayOrder,
	1
)
SET @SlideshowImageID = SCOPE_IDENTITY() 
GO



print 'CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Update] -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_SlideshowImage_Update]    Script Date: 04/11/2011 09:14:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Update]
	@SlideshowImageID int,
	@ImageDescription nvarchar(500),
	@DisplayOrder int,
	@IsActive bit
AS

SET NOCOUNT ON

UPDATE [dbo].[SuAddons_SlideshowImages] SET
	[ImageDescription] = @ImageDescription,
	[DisplayOrder] = @DisplayOrder,
	[IsActive] = @IsActive
WHERE
	[SlideshowImageID] = @SlideshowImageID

GO

print 'INSERT INTO [dbo].[SuAddons_Slideshows]'

/* -- INSERT INTO [dbo].[SuAddons_Slideshows] -------------------------------------------------------------------------------- */


SET IDENTITY_INSERT [dbo].[SuAddons_Slideshows] ON

INSERT INTO [dbo].[SuAddons_Slideshows]
           ([SlideShowID]
           ,[ContentID]
           ,[SlideshowTitle]
           ,[SlideshowDescription]
           ,[FullFixedHeight]
           ,[FullFixedWidth]
           ,[MediumFixedHeight]
           ,[MediumFixedWidth]
           ,[SmallFixedHeight]
           ,[SmallFixedWidth]
           ,[IsActive])
     VALUES
           (-1, -1  ,'na'  ,'Placeholder Slideshow'  , 0  ,0  ,0  ,0  ,0  ,0  ,1)
GO

SET IDENTITY_INSERT [dbo].[SuAddons_Slideshows] OFF
GO

print 'INSERT INTO [dbo].[SuAddons_SlideshowImages]'

/* -- INSERT INTO [dbo].[SuAddons_SlideshowImages] -------------------------------------------------------------------------------- */


SET IDENTITY_INSERT [dbo].[SuAddons_SlideshowImages] ON


INSERT INTO [dbo].[SuAddons_SlideshowImages]
           ([SlideShowImageID],
           [SlideshowID]
           ,[UserID]
           ,[ImageDescription]
           ,[DateTimeCreated]
           ,[DisplayOrder]
		   ,[IsActive])
     VALUES
           (-1,-1 ,-1 ,'na' ,GETDATE(),-1 , 1)
GO

SET IDENTITY_INSERT [dbo].[SuAddons_SlideshowImages] OFF
GO


print 'CREATE VIEW [dbo].[SuAddons_vw_SlideShowImages]'

/* -- CREATE VIEW [dbo].[SuAddons_vw_SlideShowImages]  -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[SuAddons_vw_SlideShowImages]    Script Date: 04/11/2011 13:18:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SuAddons_vw_SlideShowImages]
AS
SELECT     dbo.SuAddons_SlideshowImages.SlideshowImageID, dbo.SuAddons_SlideshowImages.SlideshowID, dbo.SuAddons_SlideshowImages.UserID, 
                      dbo.SuAddons_SlideshowImages.ImageDescription, dbo.SuAddons_SlideshowImages.DateTimeCreated, dbo.SuAddons_SlideshowImages.DisplayOrder, 
                      dbo.SuAddons_SlideshowImages.IsActive, dbo.SuAddons_Slideshows.ContentID, dbo.SuAddons_Slideshows.SlideshowTitle
FROM         dbo.SuAddons_Slideshows INNER JOIN
                      dbo.SuAddons_SlideshowImages ON dbo.SuAddons_Slideshows.SlideshowID = dbo.SuAddons_SlideshowImages.SlideshowID

GO

print 'CREATE TABLE [dbo].[SuAnalytics_BlogRss]'

/* -- CREATE TABLE [dbo].[SuAnalytics_BlogRss]  -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAnalytics_BlogRss]    Script Date: 04/26/2011 16:00:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_BlogRss](
	[RssID] [uniqueidentifier] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[RemoteIP] [nvarchar](25) NULL,
	[UserAgent] [nvarchar](1000) NULL,
	[RssDateTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_SuAnalytics_BlogRss] PRIMARY KEY CLUSTERED 
(
	[RssID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

print 'CREATE TABLE [dbo].[SuAnalytics_BlogRssViews]'

/* -- CREATE TABLE [dbo].[SuAnalytics_BlogRssViews] -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAnalytics_BlogRssViews]    Script Date: 04/26/2011 16:00:29 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_BlogRssViews](
	[ViewID] [uniqueidentifier] NOT NULL,
	[PostID] [uniqueidentifier] NOT NULL,
	[RemoteIP] [nvarchar](25) NULL,
	[ViewDateTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_SuAnalytics_BlogRssViews] PRIMARY KEY CLUSTERED 
(
	[ViewID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


print 'CREATE PROCEDURE [dbo].[SuAnalytics_BlogRss_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_BlogRss_Add]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAnalytics_BlogRss_Add]    Script Date: 04/29/2011 14:48:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SuAnalytics_BlogRss_Add]
	@ApplicationID int,
	@RemoteIP nvarchar(25),
	@UserAgent nvarchar(1000)

AS

SET NOCOUNT ON

Declare @RssID UniqueIdentifier

select @RssID = RssID from SuAnalytics_BlogRss where 
	DATEDIFF(dd,  RssDateTime, getdate()) = 0 AND RemoteIP = @RemoteIP

IF @RssID IS NULL
BEGIN

	SET @RssID = NEWID()

	INSERT INTO [dbo].[SuAnalytics_BlogRss] (
		[RssID],
		[ApplicationID],
		[RemoteIP],
		[UserAgent],
		[RssDateTime]
	) VALUES (
		@RssID,
		@ApplicationID,
		@RemoteIP,
		@UserAgent,
		GETDATE()
	)
END
GO

print 'CREATE PROCEDURE [dbo].[SuAnalytics_BlogRssView_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_BlogRssView_Add]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_BlogRssView_Add]    Script Date: 05/01/2011 12:06:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAnalytics_BlogRssView_Add]
	@PostID uniqueidentifier,
	@RemoteIP nvarchar(25)

AS

SET NOCOUNT ON


Declare @ViewID UniqueIdentifier


select @ViewID = ViewID from SuAnalytics_BlogRssViews where 
		DATEDIFF(dd,  ViewDateTime, getdate()) = 0
		AND RemoteIP = @RemoteIP AND PostID = @PostID

IF @ViewID IS NULL
BEGIN


SET @ViewID = NEWID()

INSERT INTO [dbo].[SuAnalytics_BlogRssViews] (
	[ViewID],
	[PostID],
	[RemoteIP],
	[ViewDateTime]
) VALUES (
	@ViewID,
	@PostID,
	@RemoteIP,
	GETDATE()
)
END
GO

print 'CREATE VIEW [dbo].[SuAnalytics_vw_BlogRss]'

/* -- CREATE VIEW [dbo].[SuAnalytics_vw_BlogRss]  -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[SuAnalytics_vw_BlogRss]    Script Date: 04/26/2011 16:01:51 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SuAnalytics_vw_BlogRss]
AS
SELECT     dbo.SuAnalytics_BlogRss.RssID, dbo.SuAnalytics_BlogRss.ApplicationID, dbo.Sueetie_Blogs.BlogTitle, dbo.SuAnalytics_BlogRss.RemoteIP, 
                      dbo.SuAnalytics_BlogRss.UserAgent, dbo.SuAnalytics_BlogRss.RssDateTime
FROM         dbo.SuAnalytics_BlogRss INNER JOIN
                      dbo.Sueetie_Blogs ON dbo.SuAnalytics_BlogRss.ApplicationID = dbo.Sueetie_Blogs.ApplicationID

GO

print 'CREATE VIEW [dbo].[SuAnalytics_vw_BlogRssViews]'

/* -- CREATE VIEW [dbo].[SuAnalytics_vw_BlogRssViews]  -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[SuAnalytics_vw_BlogRssViews]    Script Date: 05/31/2011 13:21:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE  VIEW [dbo].[SuAnalytics_vw_BlogRssViews]
AS
SELECT     dbo.SuAnalytics_BlogRssViews.ViewID, dbo.SuAnalytics_BlogRssViews.PostID, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.ApplicationID, 
                      dbo.SuAnalytics_BlogRssViews.RemoteIP, dbo.SuAnalytics_BlogRssViews.ViewDateTime, dbo.Sueetie_bePosts.Title, dbo.Sueetie_bePosts.DateCreated, 
                      dbo.Sueetie_Content.Permalink, dbo.Sueetie_Content.ContentTypeID
FROM         dbo.SuAnalytics_BlogRssViews INNER JOIN
                      dbo.Sueetie_bePosts ON dbo.SuAnalytics_BlogRssViews.PostID = dbo.Sueetie_bePosts.PostID INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_bePosts.SueetiePostID = dbo.Sueetie_Content.SourceID


GO


print 'CREATE PROCEDURE [dbo].[SuAnalytics_BlogRss_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_BlogRss_Get]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_BlogRss_Get]    Script Date: 05/01/2011 12:07:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAnalytics_BlogRss_Get] 
(
	@ApplicationID int,
	@DaySpan int
)
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

Declare @views TABLE
(
	[Reach] [int] NULL,
	[ViewDateTime] [smalldatetime] NULL
)

Declare @rss TABLE
(
	[ApplicationID] [int] NULL,
	[RssDateTime] [smalldatetime] NULL,
	[RssCount] [int] NULL,
	[Reach] int NOT NULL Default(0)
)

insert into @views select COUNT(*) as 'Reach', DATEADD(dd, 0, DATEDIFF(dd, 0, ViewDateTime)) as 'ViewDateTime' 
	from SuAnalytics_vw_BlogRssViews
	where ApplicationID = @ApplicationID and ContentTypeID = 1
	AND DATEDIFF(day,ViewDateTime, GETDATE()) <= @DaySpan
	group by 
	DATEADD(dd, 0, DATEDIFF(dd, 0, ViewDateTime))

insert into @rss select 
	ApplicationID,
	DATEADD(dd, 0, DATEDIFF(dd, 0, RssDateTime)) as 'RssDateTime', 
	COUNT(*) as 'RssCount', 0 as 'Reach' 
	from SuAnalytics_vw_BlogRss 
	where DATEDIFF(day,RssDateTime, GETDATE()) <= @DaySpan
group by 
	DATEADD(dd, 0, DATEDIFF(dd, 0, RssDateTime)), BlogTitle, ApplicationID
order by 
	DATEADD(dd, 0, DATEDIFF(dd, 0, RssDateTime)) DESC

update @rss set reach = v.Reach from @views v, @rss r where  DATEADD(dd, 0, DATEDIFF(dd, 0, r.RssDateTime)) = DATEADD(dd, 0, DATEDIFF(dd, 0, v.ViewDateTime))

select * from @rss

END
GO

print 'CREATE PROCEDURE [dbo].[SuAnalytics_BlogRssViews_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_BlogRssViews_Get]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_BlogRssViews_Get]    Script Date: 04/28/2011 16:41:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAnalytics_BlogRssViews_Get] 
(
	@ApplicationID int,
	@DaySpan int
)
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

select ApplicationID, Title, Permalink, COUNT(*) as 'ViewCount'
	from SuAnalytics_vw_BlogRssViews
	where ApplicationID = @ApplicationID and ContentTypeID = 1
	AND DATEDIFF(day,ViewDateTime, GETDATE()) <= @DaySpan
	group by 
	Title, Permalink, ApplicationID
	order by COUNT(*) desc

END
GO

print 'CREATE VIEW [dbo].[SuAnalytics_vw_Requests]'

/* -- CREATE VIEW [dbo].[SuAnalytics_vw_Requests]  -------------------------------------------------------------------------------- */


/****** Object:  View [dbo].[SuAnalytics_vw_Requests]    Script Date: 04/29/2011 10:54:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SuAnalytics_vw_Requests]
AS
SELECT     dbo.SuAnalytics_Pages.PageID, dbo.SuAnalytics_Pages.Url, dbo.SuAnalytics_Pages.PageTitle, dbo.SuAnalytics_Log.UserID, dbo.SuAnalytics_UserAgents.RemoteIP, 
                      dbo.SuAnalytics_UserAgents.UserAgent, dbo.SuAnalytics_Log.RequestDateTime, dbo.Sueetie_Users.UserName
FROM         dbo.SuAnalytics_Log INNER JOIN
                      dbo.SuAnalytics_Pages ON dbo.SuAnalytics_Log.PageID = dbo.SuAnalytics_Pages.PageID INNER JOIN
                      dbo.SuAnalytics_UserAgents ON dbo.SuAnalytics_Log.LogID = dbo.SuAnalytics_UserAgents.LogID INNER JOIN
                      dbo.Sueetie_Users ON dbo.SuAnalytics_Log.UserID = dbo.Sueetie_Users.UserID

GO

print 'CREATE TABLE [dbo].[SuAnalytics_TagClicks]'

/* -- CREATE TABLE [dbo].[SuAnalytics_TagClicks]  -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAnalytics_TagClicks]    Script Date: 05/02/2011 18:30:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_TagClicks](
	[TagClickID] [int] IDENTITY(1,1) NOT NULL,
	[TagMasterID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[ContentID] [int] NOT NULL,
	[ClickDateTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_SuAnalytics_TagClicks] PRIMARY KEY CLUSTERED 
(
	[TagClickID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


print 'CREATE PROCEDURE [dbo].[SuAnalytics_TagClick_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_TagClick_Add]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAnalytics_TagClick_Add]    Script Date: 05/02/2011 18:31:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAnalytics_TagClick_Add]
	@TagMasterID int,
	@UserID int,
	@ContentID int
AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuAnalytics_TagClicks] (
	[TagMasterID],
	[UserID],
	[ContentID],
	[ClickDateTime]
) VALUES (
	@TagMasterID,
	@UserID,
	@ContentID,
	GETDATE()
)


GO


print 'CREATE VIEW [dbo].[SuAnalytics_vw_TagClick]'

/* -- CREATE VIEW [dbo].[SuAnalytics_vw_TagClick]  -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[SuAnalytics_vw_TagClick]    Script Date: 05/02/2011 18:33:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SuAnalytics_vw_TagClicks]
AS
SELECT     dbo.SuAnalytics_TagClicks.TagClickID, dbo.SuAnalytics_TagClicks.TagMasterID, dbo.SuAnalytics_TagClicks.UserID, dbo.SuAnalytics_TagClicks.ContentID, 
                      dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_TagMaster.Tag, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.DisplayName, 
                      dbo.Sueetie_Applications.Description, dbo.SuAnalytics_TagClicks.ClickDateTime
FROM         dbo.SuAnalytics_TagClicks INNER JOIN
                      dbo.Sueetie_TagMaster ON dbo.SuAnalytics_TagClicks.TagMasterID = dbo.Sueetie_TagMaster.TagMasterID INNER JOIN
                      dbo.Sueetie_Users ON dbo.SuAnalytics_TagClicks.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_Content ON dbo.SuAnalytics_TagClicks.ContentID = dbo.Sueetie_Content.ContentID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID

GO


print 'CREATE PROCEDURE [dbo].[SuAnalytics_TagClicks_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_TagClicks_Get]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAnalytics_TagClicks_Get]    Script Date: 05/02/2011 19:08:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAnalytics_TagClicks_Get] 
(
	@DaySpan int,
	@MembersOnly bit
)
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

if @MembersOnly = 1
BEGIN
		select TagMasterID, Tag,  COUNT(*) as 'ClickCount'
			from SuAnalytics_vw_TagClicks
			where DATEDIFF(day,ClickDateTime, GETDATE()) <= @DaySpan AND userID > 0
			group by 
			TagMasterID, Tag
			order by COUNT(*) desc
END ELSE
BEGIN
		select TagMasterID, Tag,  COUNT(*) as 'ClickCount'
			from SuAnalytics_vw_TagClicks
			where DATEDIFF(day,ClickDateTime, GETDATE()) <= @DaySpan
			group by 
			TagMasterID, Tag
			order by COUNT(*) desc		
END
END

GO


print 'CREATE PROCEDURE [dbo].[SuAnalytics_TagClickViews_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_TagClickViews_Get]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_TagClickViews_Get]    Script Date: 05/03/2011 12:13:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAnalytics_TagClickViews_Get] 
(
	@TagMasterID int,
	@DaySpan int,
	@MembersOnly bit
)
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

if @MembersOnly = 1
BEGIN
		select * from SuAnalytics_vw_TagClicks
			where TagMasterID = @TagMasterID and DATEDIFF(day,ClickDateTime, GETDATE()) <= @DaySpan AND userID > 0
			order by ClickDateTime desc
END ELSE
BEGIN
		select * from SuAnalytics_vw_TagClicks
			where TagMasterID = @TagMasterID and DATEDIFF(day,ClickDateTime, GETDATE()) <= @DaySpan
			order by ClickDateTime desc
END
END

GO


print 'CREATE TABLE [dbo].[SuAnalytics_SearchTypes]'

/* -- CREATE TABLE [dbo].[SuAnalytics_SearchTypes]  -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAnalytics_SearchTypes]    Script Date: 05/03/2011 18:07:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_SearchTypes](
	[SearchTypeID] [int] NOT NULL,
	[SearchTypeCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_SuAnalytics_SearchTypes] PRIMARY KEY CLUSTERED 
(
	[SearchTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

print 'INSERT INTO [dbo].[SuAnalytics_SearchTypes]'

/* -- INSERT INTO [dbo].[SuAnalytics_SearchTypes]  -------------------------------------------------------------------------------- */

insert into SuAnalytics_SearchTypes values (-1,'Unknown');
insert into SuAnalytics_SearchTypes values (1,'Global');
insert into SuAnalytics_SearchTypes values (2,'Blog');
insert into SuAnalytics_SearchTypes values (3,'Forum');
insert into SuAnalytics_SearchTypes values (4,'Wiki');
insert into SuAnalytics_SearchTypes values (5,'Media');
insert into SuAnalytics_SearchTypes values (6,'Marketplace');
GO


print 'CREATE TABLE [dbo].[SuAnalytics_SearchTerms]'

/* -- CREATE TABLE [dbo].[SuAnalytics_SearchTerms]  -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAnalytics_SearchTerms]    Script Date: 05/03/2011 18:30:49 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_SearchTerms](
	[SearchTermID] [int] IDENTITY(1,1) NOT NULL,
	[SearchTerms] [nvarchar](255) NULL,
	[SearchTypeID] [int] NULL,
	[UserID] [int] NULL,
	[RemoteIP] [nvarchar](15) NULL,
	[SearchDateTime] [smalldatetime] NULL,
 CONSTRAINT [PK_SuAnalytics_SearchTerms] PRIMARY KEY CLUSTERED 
(
	[SearchTermID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


print 'CREATE VIEW [dbo].[SuAnalytics_vw_SearchTerms]'

/* -- CREATE VIEW [dbo].[SuAnalytics_vw_SearchTerms]  -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[SuAnalytics_vw_SearchTerms]    Script Date: 05/03/2011 19:12:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SuAnalytics_vw_SearchTerms]
AS
SELECT     dbo.SuAnalytics_SearchTerms.SearchTermID, dbo.SuAnalytics_SearchTerms.SearchTerms, dbo.SuAnalytics_SearchTerms.SearchTypeID, 
                      dbo.SuAnalytics_SearchTerms.UserID, dbo.SuAnalytics_SearchTerms.RemoteIP, dbo.SuAnalytics_SearchTerms.SearchDateTime, 
                      dbo.SuAnalytics_SearchTypes.SearchTypeCode, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.DisplayName
FROM         dbo.SuAnalytics_SearchTerms INNER JOIN
                      dbo.SuAnalytics_SearchTypes ON dbo.SuAnalytics_SearchTerms.SearchTypeID = dbo.SuAnalytics_SearchTypes.SearchTypeID INNER JOIN
                      dbo.Sueetie_Users ON dbo.SuAnalytics_SearchTerms.UserID = dbo.Sueetie_Users.UserID

GO

print 'CREATE PROCEDURE [dbo].[SuAnalytics_SearchTerm_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_SearchTerm_Add] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_SearchTerm_Add]    Script Date: 05/16/2011 11:57:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAnalytics_SearchTerm_Add]
	@SearchTerms nvarchar(255),
	@SearchTypeID int,
	@UserID int,
	@RemoteIP nvarchar(15)
 
AS

SET NOCOUNT ON

if @SearchTerms is not null
BEGIN

DECLARE @SearchTermID int
SELECT @SearchTermID = SearchTermID from SuAnalytics_SearchTerms
	WHERE SearchTerms = @SearchTerms
	AND UserID = @UserID
	AND DATEDIFF(mi,  SearchDateTime, getdate()) <= 30
	
	if @SearchTermID is null
	BEGIN

		INSERT INTO [dbo].[SuAnalytics_SearchTerms] (
			[SearchTerms],
			[SearchTypeID],
			[UserID],
			[RemoteIP],
			[SearchDateTime]
		) VALUES (
			@SearchTerms,
			@SearchTypeID,
			@UserID,
			@RemoteIP,
			GETDATE()
		)
		
	END
END

GO



print 'CREATE PROCEDURE [dbo].[SuAnalytics_SearchTerms_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_SearchTerms_Get] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_SearchTerms_Get]    Script Date: 05/12/2011 12:25:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAnalytics_SearchTerms_Get] 
(
	@DaySpan int,
	@MembersOnly bit
)
AS
BEGIN
	SET NOCOUNT, XACT_ABORT ON

	if @MembersOnly = 1
		BEGIN
				select *
					from SuAnalytics_vw_SearchTerms
					where DATEDIFF(day,SearchDateTime, GETDATE()) <= @DaySpan AND userID > 0
					and SearchTerms is not null
		END ELSE
		BEGIN
				select *
					from SuAnalytics_vw_SearchTerms
					where DATEDIFF(day,SearchDateTime, GETDATE()) <= @DaySpan 
					and SearchTerms is not null
		END
		
END
GO


print 'CREATE PROCEDURE [dbo].[SuAnalytics_SearchTermsApp_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAnalytics_SearchTermsApp_Get] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAnalytics_SearchTermsApp_Get]    Script Date: 05/04/2011 16:14:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAnalytics_SearchTermsApp_Get] 
(
	@DaySpan int,
	@MembersOnly bit,
	@SearchTypeID int
)
AS
BEGIN
	SET NOCOUNT, XACT_ABORT ON

	if @MembersOnly = 1
		BEGIN
				select *
					from SuAnalytics_vw_SearchTerms
					where DATEDIFF(day,SearchDateTime, GETDATE()) <= @DaySpan AND userID > 0 AND SearchTypeID = @SearchTypeID
		END ELSE
		BEGIN
				select *
					from SuAnalytics_vw_SearchTerms
					where DATEDIFF(day,SearchDateTime, GETDATE()) <= @DaySpan AND SearchTypeID = @SearchTypeID
		END
		
END

GO


print 'CREATE TABLE [dbo].[SuAddons_BlogPostImages]'

/* -- CREATE TABLE [dbo].[SuAddons_BlogPostImages] -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuAddons_BlogPostImages]    Script Date: 05/11/2011 12:18:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_BlogPostImages](
	[BlogID] [int] NOT NULL,
	[PostImageTypeID] [int] NOT NULL,
	[PostImageHeight] [int] NULL,
	[PostImageWidth] [int] NULL,
	[AnchorPositionID] [int] NULL,
	[MediaAlbumID] [int] NULL,
	[DefaultPostImage] [nvarchar](255) NULL,
 CONSTRAINT [PK_SuAddons_BlogPostImages] PRIMARY KEY CLUSTERED 
(
	[BlogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAddons_BlogPostImages] ADD  CONSTRAINT [DF_SuAddons_BlogPostImages_PostImageTypeID]  DEFAULT ((0)) FOR [PostImageTypeID]
GO



print 'CREATE PROCEDURE [dbo].[Sueetie_Blog_Add]'

/* -- CREATE PROCEDURE [dbo].[Sueetie_Blog_Add] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[Sueetie_Blog_Update]    Script Date: 05/09/2011 20:13:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Blog_Add]
	@ApplicationID int,
	@BlogTitle nvarchar(255)
AS

SET NOCOUNT ON

DECLARE @BlogID int

INSERT INTO [dbo].[Sueetie_Blogs] (
	[ApplicationID],
	[BlogTitle],
	[DateBlogCreated]
) VALUES (
	@ApplicationID,
	@BlogTitle,
	GETDATE()
)

SET @BlogID = SCOPE_IDENTITY() 

INSERT INTO [dbo].[SuAddons_BlogPostImages] SELECT @BlogID, 0, 90, 90, 0, 0, null
GO

print 'INSERT INTO SuAddons_BlogPostImages'

/* -- INSERT INTO SuAddons_BlogPostImages -------------------------------------------------------------------------------- */

INSERT INTO SuAddons_BlogPostImages select  blogID, 0, 90, 90, 0, 0, null from Sueetie_blogs 


print 'CREATE VIEW [dbo].[Sueetie_vw_Blogs]'

/* -- CREATE VIEW [dbo].[Sueetie_vw_Blogs] -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[Sueetie_vw_Blogs]    Script Date: 05/11/2011 12:17:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Sueetie_vw_Blogs]
AS
SELECT     dbo.Sueetie_Blogs.BlogID, dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_Applications.Description AS AppDescription, dbo.Sueetie_Applications.GroupID, 
                      dbo.Sueetie_Blogs.ApplicationID, dbo.Sueetie_Blogs.CategoryID, dbo.Sueetie_Blogs.BlogOwnerRole, dbo.Sueetie_Blogs.BlogAccessRole, 
                      dbo.Sueetie_Blogs.BlogTitle, dbo.Sueetie_Blogs.BlogDescription, dbo.Sueetie_Blogs.PostCount, dbo.Sueetie_Blogs.CommentCount, 
                      dbo.Sueetie_Blogs.TrackbackCount, dbo.Sueetie_Blogs.IncludeInAggregateList, dbo.Sueetie_Blogs.IsActive, dbo.Sueetie_Content.ContentID, 
                      dbo.Sueetie_bePosts.UserID AS PostAuthorID, dbo.Sueetie_bePosts.Title AS PostTitle, dbo.Sueetie_bePosts.Description AS PostDescription, 
                      dbo.Sueetie_bePosts.PostContent, dbo.Sueetie_bePosts.DateCreated AS PostDateCreated, dbo.Sueetie_Users.UserName AS PostAuthorUserName, 
                      dbo.Sueetie_Users.Email AS PostAuthorEmail, dbo.Sueetie_Users.DisplayName AS PostAuthorDisplayName, dbo.Sueetie_bePosts.IsPublished, 
                      dbo.Sueetie_Content.Permalink, dbo.Sueetie_Blogs.DateBlogCreated, dbo.Sueetie_Blogs.RegisteredComments, dbo.SuAddons_BlogPostImages.PostImageTypeID, 
                      dbo.SuAddons_BlogPostImages.PostImageHeight, dbo.SuAddons_BlogPostImages.PostImageWidth, dbo.SuAddons_BlogPostImages.AnchorPositionID, 
                      dbo.SuAddons_BlogPostImages.MediaAlbumID, dbo.SuAddons_BlogPostImages.DefaultPostImage
FROM         dbo.Sueetie_Blogs INNER JOIN
                      dbo.Sueetie_Categories ON dbo.Sueetie_Blogs.CategoryID = dbo.Sueetie_Categories.CategoryID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Blogs.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_Blogs.MostRecentContentID = dbo.Sueetie_Content.ContentID INNER JOIN
                      dbo.Sueetie_Users INNER JOIN
                      dbo.Sueetie_bePosts ON dbo.Sueetie_Users.UserID = dbo.Sueetie_bePosts.UserID ON 
                      dbo.Sueetie_Content.SourceID = dbo.Sueetie_bePosts.SueetiePostID INNER JOIN
                      dbo.SuAddons_BlogPostImages ON dbo.Sueetie_Blogs.BlogID = dbo.SuAddons_BlogPostImages.BlogID

GO


print 'CREATE PROCEDURE [dbo].[SuAddons_BlogPostImage_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_BlogPostImage_Update] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_BlogPostImage_Update]    Script Date: 05/11/2011 14:55:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_BlogPostImage_Update]
	@BlogID int,
	@PostImageTypeID int,
	@PostImageHeight int,
	@PostImageWidth int,
	@AnchorPositionID int,
	@MediaAlbumID int,
	@DefaultPostImage nvarchar(255)
AS

SET NOCOUNT ON

UPDATE [dbo].[SuAddons_BlogPostImages] SET
	[PostImageTypeID] = @PostImageTypeID,
	[PostImageHeight] = @PostImageHeight,
	[PostImageWidth] = @PostImageWidth,
	[AnchorPositionID] = @AnchorPositionID,
	[MediaAlbumID] = @MediaAlbumID,
	[DefaultPostImage] = @DefaultPostImage
WHERE
	[BlogID] = @BlogID



GO


print 'CREATE PROCEDURE [dbo].[SuAddons_BlogPostAlbums_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_BlogPostAlbums_Get] -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_BlogPostImage_Update]    Script Date: 05/11/2011 19:29:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_BlogPostAlbums_Get]
AS

SET NOCOUNT ON


declare @albums table
(
	ParentAlbumID int,
	AlbumID int,
	IsParent bit,
	ParentTitle nvarchar(255),
	Title nvarchar(255)
)
insert into @albums select AlbumId, AlbumId, 1, title, REPLICATE(' ', 1) from gs_Album
	where AlbumId in (select AlbumParentId from gs_Album)
insert into @albums select AlbumParentId, AlbumID, 0, null, title from gs_Album 
	where AlbumParentId in (select AlbumId from @albums)

update @albums set parenttitle = a.title from gs_Album a, @albums b where a.AlbumId = b.ParentAlbumID
	and parenttitle is null

delete from @albums where ParentAlbumID = AlbumID	
	
select * from @albums where albumID > 1 order by parenttitle, title
GO


print 'CREATE TABLE [dbo].[SuAddons_ForumAnswers]'

/* -- CREATE TABLE [dbo].[SuAddons_ForumAnswers] -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuAddons_ForumAnswers]    Script Date: 05/18/2011 14:59:35 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_ForumAnswers](
	[AnswerID] [int] IDENTITY(1,1) NOT NULL,
	[TopicID] [int] NULL,
	[MessageID] [int] NULL,
	[UserID] [int] NULL,
	[IsFirst] [bit] NULL,
	[AnswerDateTime] [smalldatetime] NULL,
 CONSTRAINT [PK_SuAddons_Answers] PRIMARY KEY CLUSTERED 
(
	[AnswerID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

print 'CREATE PROCEDURE [dbo].[SuAddons_ForumAnswer_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_ForumAnswer_Add] -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_ForumAnswer_Add]    Script Date: 05/18/2011 17:08:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAddons_ForumAnswer_Add]
	@TopicID int,
	@MessageID int,
	@UserID int,
	@IsFirst bit,
	@AnswerID int OUTPUT
 
AS

SET NOCOUNT ON

DECLARE @ExistingAnswerID int
SELECT @ExistingAnswerID = AnswerID from SuAddons_ForumAnswers
	WHERE UserID = @UserID and MessageID = @MessageID
	
IF (@ExistingAnswerID > 0)
BEGIN
	SET @AnswerID = -1
END ELSE
BEGIN
INSERT INTO [dbo].[SuAddons_ForumAnswers] (
	[TopicID],
	[MessageID],
	[UserID],
	[IsFirst],
	[AnswerDateTime]
) VALUES (
	@TopicID,
	@MessageID,
	@UserID,
	@IsFirst,
	GETDATE()
)

SET @AnswerID = SCOPE_IDENTITY() 
END
GO

print 'CREATE PROCEDURE [dbo].[SuAddons_ForumAnswer_Delete]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_ForumAnswer_Delete] -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_ForumAnswer_Add]    Script Date: 05/18/2011 16:39:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAddons_ForumAnswer_Delete]
	@MessageID int,
	@UserID int
AS

SET NOCOUNT ON

DECLARE @AnswerID int
SELECT @AnswerID = AnswerID FROM [dbo].[SuAddons_ForumAnswers] WHERE MessageID = @MessageID and UserID = @UserID

DELETE FROM [dbo].[SuAddons_ForumAnswers]
	WHERE MessageID = @MessageID and UserID = @UserID
	
DELETE FROM Sueetie_UserLog where UserLogCategoryID = 302 
	and ItemID = @AnswerID and UserID = @UserID

DELETE FROM Sueetie_Content where SourceID	= @AnswerID and 
	ContentTypeID = 24 and UserID = @UserID
GO


print 'CREATE TABLE [dbo].[SuAddons_MediaSetGroups]'

/* -- CREATE TABLE [dbo].[SuAddons_MediaSetGroups]  -------------------------------------------------------------------------------- */


/****** Object:  Table [dbo].[SuAddons_MediaSetGroups]    Script Date: 05/23/2011 13:50:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_MediaSetGroups](
	[MediaSetGroupID] [int] IDENTITY(1,1) NOT NULL,
	[MediaSetGroupTitle] [nvarchar](255) NULL,
	[MediaSetGroupDescription] [nvarchar](max) NULL,
	[ContentID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDateTime] [smalldatetime] NULL,
 CONSTRAINT [PK_SuAddons_MediaSetGroups] PRIMARY KEY CLUSTERED 
(
	[MediaSetGroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAddons_MediaSetGroups] ADD  CONSTRAINT [DF_SuAddons_MediaSetGroups_ContentID]  DEFAULT ((-1)) FOR [ContentID]
GO

ALTER TABLE [dbo].[SuAddons_MediaSetGroups] ADD  CONSTRAINT [DF_SuAddons_MediaSetGroups_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO


print 'CREATE TABLE [dbo].[SuAddons_MediaSets]'

/* -- CREATE TABLE [dbo].[SuAddons_MediaSets]  -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuAddons_MediaSets]    Script Date: 05/23/2011 14:35:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_MediaSets](
	[MediaSetID] [int] IDENTITY(1,1) NOT NULL,
	[MediaSetGroupID] [int] NOT NULL,
	[MediaSetTitle] [nvarchar](255) NOT NULL,
	[MediaSetDescription] [nvarchar](max) NULL,
	[UserID] [int] NOT NULL,
	[ContentTypeID] [int] NOT NULL,
	[ContentID] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NULL,
	[LastUpdateDateTime] [smalldatetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_SuAddons_MediaSets] PRIMARY KEY CLUSTERED 
(
	[MediaSetID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAddons_MediaSets] ADD  CONSTRAINT [DF_SuAddons_MediaSets_MediaSetGroupID]  DEFAULT ((-1)) FOR [MediaSetGroupID]
GO

ALTER TABLE [dbo].[SuAddons_MediaSets] ADD  CONSTRAINT [DF_SuAddons_MediaSets_UserID]  DEFAULT ((-1)) FOR [UserID]
GO

ALTER TABLE [dbo].[SuAddons_MediaSets] ADD  CONSTRAINT [DF_SuAddons_MediaSets_ContentTypeID]  DEFAULT ((0)) FOR [ContentTypeID]
GO

ALTER TABLE [dbo].[SuAddons_MediaSets] ADD  CONSTRAINT [DF_SuAddons_MediaSets_ContentID]  DEFAULT ((0)) FOR [ContentID]
GO

ALTER TABLE [dbo].[SuAddons_MediaSets] ADD  CONSTRAINT [DF_SuAddons_MediaSets_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO


print 'CREATE TABLE [dbo].[SuAddons_MediaSetObjects]'

/* -- CREATE TABLE [dbo].[SuAddons_MediaSetObjects]  -------------------------------------------------------------------------------- */

/****** Object:  Table [dbo].[SuAddons_MediaSetObjects]    Script Date: 05/26/2011 11:12:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAddons_MediaSetObjects](
	[MediaSetObjectID] [int] IDENTITY(1,1) NOT NULL,
	[MediaSetID] [int] NOT NULL,
	[ContentID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[AddedDateTime] [smalldatetime] NULL,
 CONSTRAINT [PK_SuAddons_MediaSetObjects] PRIMARY KEY CLUSTERED 
(
	[MediaSetObjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[SuAddons_MediaSetObjects] ADD  CONSTRAINT [DF_SuAddons_MediaSetObjects_UserID]  DEFAULT ((-1)) FOR [UserID]
GO




print 'INSERT INTO SuAddons_MediaSetGroups'

/* -- INSERT INTO SuAddons_MediaSetGroups  -------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[SuAddons_MediaSetGroups] ON
GO

INSERT INTO SuAddons_MediaSetGroups (MediaSetGroupID, MediaSetGroupTitle, MediaSetGroupDescription, ContentID, IsActive, CreatedDateTime)
	values (-1,'No Group', 'Media Set not in a group',-1, 1, GETDATE())

SET IDENTITY_INSERT [dbo].[SuAddons_MediaSetGroups] OFF
GO


print 'CREATE PROCEDURE [dbo].[SuAddons_MediaSetGroup_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_MediaSetGroup_Add]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_MediaSetGroup_Add]    Script Date: 05/23/2011 15:23:28 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAddons_MediaSetGroup_Add]
	@MediaSetGroupTitle nvarchar(255),
	@MediaSetGroupDescription nvarchar(max),
	@ContentID int,
	@IsActive bit
 
AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuAddons_MediaSetGroups] (
	[MediaSetGroupTitle],
	[MediaSetGroupDescription],
	[ContentID],
	[IsActive],
	[CreatedDateTime]
) VALUES (
	@MediaSetGroupTitle,
	@MediaSetGroupDescription,
	0,
	@IsActive,
	GETDATE()
)


GO

print 'CREATE PROCEDURE [dbo].[SuAddons_MediaSetGroup_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_MediaSetGroup_Update]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[SuAddons_MediaSetGroup_Update]    Script Date: 05/23/2011 15:24:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_MediaSetGroup_Update]
	@MediaSetGroupID int,
	@MediaSetGroupTitle nvarchar(255),
	@MediaSetGroupDescription nvarchar(max),
	@ContentID int,
	@IsActive bit
AS

SET NOCOUNT ON

UPDATE [dbo].[SuAddons_MediaSetGroups] SET
	[MediaSetGroupTitle] = @MediaSetGroupTitle,
	[MediaSetGroupDescription] = @MediaSetGroupDescription,
	[ContentID] = @ContentID,
	[IsActive] = @IsActive
WHERE
	[MediaSetGroupID] = @MediaSetGroupID


GO


print 'CREATE PROCEDURE [dbo].[SuAddons_MediaSet_Add]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_MediaSet_Add]  -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_MediaSet_Add]    Script Date: 05/23/2011 15:24:34 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_MediaSet_Add]
	@MediaSetGroupID int,
	@MediaSetTitle nvarchar(255),
	@MediaSetDescription nvarchar(max),
	@UserID int,
	@ContentTypeID int,
	@ContentID int,
	@IsActive bit
 
AS

SET NOCOUNT ON

INSERT INTO [dbo].[SuAddons_MediaSets] (
	[MediaSetGroupID],
	[MediaSetTitle],
	[MediaSetDescription],
	[UserID],
	[ContentTypeID],
	[ContentID],
	[CreatedDateTime],
	[IsActive]
) VALUES (
	@MediaSetGroupID,
	@MediaSetTitle,
	@MediaSetDescription,
	@UserID,
	@ContentTypeID,
	@ContentID,
	GETDATE(),
	@IsActive
)



GO

print 'CREATE PROCEDURE [dbo].[SuAddons_MediaSet_Update]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_MediaSet_Update] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_MediaSet_Update]    Script Date: 05/23/2011 15:25:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_MediaSet_Update]
	@MediaSetID int,
	@MediaSetGroupID int,
	@MediaSetTitle nvarchar(255),
	@MediaSetDescription nvarchar(max),
	@ContentTypeID int,
	@ContentID int,
	@IsActive bit
AS

SET NOCOUNT ON

UPDATE [dbo].[SuAddons_MediaSets] SET
	[MediaSetGroupID] = @MediaSetGroupID,
	[MediaSetTitle] = @MediaSetTitle,
	[MediaSetDescription] = @MediaSetDescription,
	[ContentTypeID] = @ContentTypeID,
	[ContentID] = @ContentID,
	[LastUpdateDateTime] = GETDATE(),
	[IsActive] = @IsActive
WHERE
	[MediaSetID] = @MediaSetID


GO


print 'CREATE PROCEDURE [dbo].[SuAddons_MediaSetAlbums_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_MediaSetAlbums_Get] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_MediaSetAlbums_Get]    Script Date: 05/24/2011 21:02:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SuAddons_MediaSetAlbums_Get]
(
	@GalleryID int
)
AS

SET NOCOUNT ON


declare @albums table
(
	ParentAlbumID int,
	AlbumID int,
	IsParent bit,
	SueetieAlbumPath nvarchar(500),
	ParentTitle nvarchar(255),
	Title nvarchar(255)
)
insert into @albums select AlbumParentId, a.AlbumId, 1, s.SueetieAlbumPath, title, REPLICATE(' ', 1) from gs_Album a
	inner join Sueetie_gs_Album s on a.AlbumId = s.AlbumID
	where FKGalleryID = @GalleryID and a.AlbumId in (select AlbumParentId from gs_Album)
insert into @albums select AlbumParentId, a.AlbumID, 0, s.SueetieAlbumPath, null, title from gs_Album a
	inner join Sueetie_gs_Album s on a.AlbumId = s.AlbumID
	where FKGalleryID = @GalleryID and AlbumParentId in (select AlbumId from @albums)

update @albums set parenttitle = a.title from gs_Album a, @albums b where a.AlbumId = b.ParentAlbumID
	and parenttitle is null

delete from @albums where IsParent = 1
		
select * from @albums where albumID > 1 order by parenttitle, title


GO



print 'CREATE VIEW [dbo].[SuAddons_vw_MediaSetObjects]'

/* -- CREATE VIEW [dbo].[SuAddons_vw_MediaSetObjects] -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[SuAddons_vw_MediaSetObjects]    Script Date: 05/26/2011 11:47:05 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[SuAddons_vw_MediaSetObjects]
AS
SELECT     dbo.SuAddons_MediaSetObjects.MediaSetObjectID, dbo.SuAddons_MediaSetObjects.MediaSetID, dbo.SuAddons_MediaSetObjects.AddedDateTime, 
                      dbo.SuAddons_MediaSets.MediaSetGroupID, dbo.SuAddons_MediaSetGroups.MediaSetGroupTitle, dbo.Sueetie_Content.ContentID, 
                      dbo.gs_MediaObject.MediaObjectId, dbo.gs_Album.AlbumId, dbo.Sueetie_Content.SourceID, dbo.Sueetie_Content.ContentTypeID, 
                      dbo.Sueetie_ContentTypes.Description AS ContentTypeDescription, dbo.Sueetie_Content.UserID AS MediaObjectUserID, dbo.Sueetie_Content.Permalink, 
                      dbo.Sueetie_Content.DateTimeCreated, dbo.Sueetie_Content.IsRestricted, dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Applications.ApplicationKey, 
                      dbo.Sueetie_Applications.Description AS ApplicationDescription, dbo.gs_MediaObject.Title AS MediaObjectTitle, dbo.gs_Album.Title AS AlbumTitle, 
                      dbo.Sueetie_Groups.GroupID, dbo.Sueetie_Groups.GroupKey, dbo.Sueetie_Groups.GroupName, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, 
                      dbo.Sueetie_Users.DisplayName, '' AS MediaObjectUrl, dbo.gs_Gallery.GalleryId, dbo.gs_Gallery.Description AS GalleryName, 
                      dbo.Sueetie_gs_MediaObject.MediaObjectDescription, dbo.Sueetie_gs_MediaObject.InDownloadReport, dbo.Sueetie_Applications.ApplicationTypeID, 
                      dbo.Sueetie_gs_Bibliography.Abstract, dbo.Sueetie_gs_Bibliography.Authors, dbo.Sueetie_gs_Bibliography.Location, dbo.Sueetie_gs_Bibliography.Series, 
                      dbo.Sueetie_gs_Bibliography.DocumentType, dbo.Sueetie_gs_Bibliography.Keywords, dbo.Sueetie_gs_Bibliography.Misc, dbo.Sueetie_gs_Bibliography.Number, 
                      dbo.Sueetie_gs_Bibliography.Version, dbo.Sueetie_gs_Bibliography.Organization, dbo.Sueetie_gs_Bibliography.Conference, dbo.Sueetie_gs_Bibliography.ISxN, 
                      dbo.Sueetie_gs_Bibliography.PublicationDate, dbo.Sueetie_gs_Bibliography.Publisher, CONVERT(bit, 0) AS IsAlbum, dbo.gs_MediaObject.OriginalFilename, 
                      dbo.gs_MediaObject.CreatedBy, dbo.gs_MediaObject.DateAdded, dbo.gs_MediaObject.LastModifiedBy, dbo.gs_MediaObject.DateLastModified, 0 AS ThumbnailHeight,
                       0 AS ThumbnailWidth, CONVERT(bit, 0) AS IsImage, dbo.ConcatMediaObjectTags(dbo.gs_MediaObject.MediaObjectId) AS Tags, 
                      dbo.SuAddons_MediaSets.IsActive AS MediaSetIsActive, dbo.SuAddons_MediaSets.LastUpdateDateTime AS MediaSetLastUpdateDateTime, 
                      dbo.SuAddons_MediaSets.ContentID AS MediaSetContentID, dbo.SuAddons_MediaSetGroups.ContentID AS MediaSetGroupContentID, 
                      dbo.SuAddons_MediaSetObjects.UserID AS MediaSetUserID, dbo.Sueetie_gs_Album.SueetieAlbumPath, dbo.gs_MediaObject.ThumbnailFilename, 
                      dbo.gs_MediaObject.OptimizedFilename
FROM         dbo.Sueetie_Applications INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_Applications.ApplicationID = dbo.Sueetie_Content.ApplicationID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_Content.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.gs_MediaObject ON dbo.Sueetie_Content.SourceID = dbo.gs_MediaObject.MediaObjectId INNER JOIN
                      dbo.gs_Album ON dbo.gs_MediaObject.FKAlbumId = dbo.gs_Album.AlbumId INNER JOIN
                      dbo.gs_Gallery ON dbo.gs_Album.FKGalleryId = dbo.gs_Gallery.GalleryId INNER JOIN
                      dbo.Sueetie_gs_MediaObject ON dbo.gs_MediaObject.MediaObjectId = dbo.Sueetie_gs_MediaObject.MediaObjectID INNER JOIN
                      dbo.Sueetie_ContentTypes ON dbo.Sueetie_Content.ContentTypeID = dbo.Sueetie_ContentTypes.ContentTypeID INNER JOIN
                      dbo.Sueetie_gs_Bibliography ON dbo.gs_MediaObject.MediaObjectId = dbo.Sueetie_gs_Bibliography.MediaObjectID INNER JOIN
                      dbo.SuAddons_MediaSetObjects ON dbo.Sueetie_Content.ContentID = dbo.SuAddons_MediaSetObjects.ContentID INNER JOIN
                      dbo.SuAddons_MediaSets ON dbo.SuAddons_MediaSetObjects.MediaSetID = dbo.SuAddons_MediaSets.MediaSetID INNER JOIN
                      dbo.SuAddons_MediaSetGroups ON dbo.SuAddons_MediaSets.MediaSetGroupID = dbo.SuAddons_MediaSetGroups.MediaSetGroupID INNER JOIN
                      dbo.Sueetie_gs_Album ON dbo.gs_Album.AlbumId = dbo.Sueetie_gs_Album.AlbumID

GO




print 'CREATE PROCEDURE [dbo].[SuAddons_MediaSetObjectKeys_Get]'

/* -- CREATE PROCEDURE [dbo].[SuAddons_MediaSetObjectKeys_Get] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[SuAddons_MediaSetObjectKeys_Get]    Script Date: 05/25/2011 11:04:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SuAddons_MediaSetObjectKeys_Get]
(
	@SueetieAlbumPath nvarchar(255),
	@MediaSetID int,
	@GalleryID int
)
AS

SET NOCOUNT ON


declare @mediaSetKeys table
(
	ContentID int,
	MediaSetID int,
	MediaObjectID int,
	SueetieAlbumPath nvarchar(255), 
	AlbumID int
)

if @SueetieAlbumPath = 'all'
BEGIN
		insert into @mediaSetKeys SELECT TOP 50
			ContentID, @MediaSetID, m.MediaObjectID, SueetieAlbumPath, a.AlbumID FROM gs_MediaObject m
			INNER JOIN Sueetie_Content c ON 
			MediaObjectId = SourceID INNER JOIN gs_Album a ON
			m.FKAlbumId = a.AlbumId inner join Sueetie_gs_Album s ON
			a.AlbumId = s.AlbumID
		WHERE c.ContentTypeID = 6 AND a.FKGalleryId = @GalleryID
		order by c.DateTimeCreated desc
END ELSE
BEGIN
		insert into @mediaSetKeys SELECT 
			ContentID, @MediaSetID, m.MediaObjectID, SueetieAlbumPath, s.AlbumID FROM gs_MediaObject m
			INNER JOIN Sueetie_Content c ON 
			MediaObjectId = SourceID INNER JOIN Sueetie_gs_Album s ON
			m.FKAlbumId = s.AlbumID
		WHERE c.ContentTypeID = 6 AND s.SueetieAlbumPath like @SueetieAlbumPath + '%'
		order by c.DateTimeCreated desc
END

delete from @mediaSetKeys where
	ContentID in (select ContentID from SuAddons_MediaSetObjects m where
	m.MediaSetID = @MediaSetID)
	
select * from @mediaSetKeys


GO




print 'CREATE VIEW [dbo].[Sueetie_vw_MediaDirectories]'

/* -- CREATE VIEW [dbo].[Sueetie_vw_MediaDirectories] -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[Sueetie_vw_MediaDirectories]    Script Date: 05/24/2011 21:18:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Sueetie_vw_MediaDirectories]
AS
SELECT     dbo.gs_MediaObject.MediaObjectId, dbo.Sueetie_Content.ContentID, dbo.gs_Gallery.GalleryId, dbo.gs_Album.AlbumId, dbo.gs_Album.AlbumParentId, 
                      dbo.gs_Album.DirectoryName, dbo.gs_MediaObject.ThumbnailFilename, dbo.gs_MediaObject.OptimizedFilename, dbo.gs_MediaObject.OriginalFilename, 
                      dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Applications.ApplicationID, dbo.Sueetie_Applications.ApplicationKey, 
                      dbo.Sueetie_gs_Album.SueetieAlbumPath
FROM         dbo.gs_Gallery INNER JOIN
                      dbo.gs_Album ON dbo.gs_Gallery.GalleryId = dbo.gs_Album.FKGalleryId INNER JOIN
                      dbo.gs_MediaObject ON dbo.gs_Album.AlbumId = dbo.gs_MediaObject.FKAlbumId INNER JOIN
                      dbo.Sueetie_Content ON dbo.gs_MediaObject.MediaObjectId = dbo.Sueetie_Content.SourceID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_gs_Album ON dbo.gs_Album.AlbumId = dbo.Sueetie_gs_Album.AlbumID

GO

print 'CREATE PROCEDURE [dbo].[Sueetie_UserLogActivities_Get]'

/* -- CREATE PROCEDURE [dbo].[Sueetie_UserLogActivities_Get] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[Sueetie_UserLogActivities_Get]    Script Date: 05/30/2011 15:08:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[Sueetie_UserLogActivities_Get]
(
	@groupID int,
	@syndicatedList bit,
	@showAll bit
)
AS

SET NOCOUNT ON

CREATE TABLE [dbo].[#z_Sueetie_UserLogActivity]	(
	[UserLogID] [int] NULL,
	[UserLogCategoryID] [int] NULL,
	[UserLogCategoryCode] [nvarchar](25) NULL,
	[UserID] [int] NULL,
	[DisplayName] [nvarchar](50) NULL,
	[ItemID] [int] NULL,	
	[ContentID] [int] NULL,
	[ContentTypeID] [int] NULL,	
	[ApplicationID] [int] NULL,
	[GroupID] [int] NULL,
	[Permalink] [nvarchar](500) NULL,
	[SourceID] [int] NULL,
	[SourceDescription] [nvarchar](255) NULL,
	[SourceParentID] [int] NULL,
	[SourceParentDescription] [nvarchar](255) NULL,
	[SourceParentPermalink] [nvarchar](500) NULL,
	[ToUserID] [int] NULL,
	[ToUserDisplayName] [nvarchar](50) NULL,
	[GroupPath] [nvarchar](50) NULL,	
	[ApplicationPath] [nvarchar](50) NULL,
	[ApplicationName] [nvarchar](255) NULL,
	[DateTimeActivity] [smalldatetime] NULL,
	[Email] [nvarchar](255) NULL,
	[IsDisplayed] [bit] NULL,
	[IsSyndicated] [bit] NULL
)

-- Obtaining activity for top level (Group 0) or specific group only.  Will extend to retrieve all groups

select @groupID = 0

if @showAll = 1
begin
		insert into #z_Sueetie_UserLogActivity (UserLogID, UserLogCategoryID, UserLogCategoryCode, UserID, DisplayName, ItemID, DateTimeActivity, 
			ContentID, ContentTypeID, ApplicationID, GroupID, SourceID, SourceParentID, ToUserID, ApplicationPath, Email, IsDisplayed, IsSyndicated)  select top 100
		u.userlogid, u.UserLogCategoryID, c.UserLogCategoryCode, u.UserID, s.DisplayName, u.ItemID, u.LogDateTime,
			-1, -1, -1, -1, -1, -1, -1, 'members', s.Email, c.IsDisplayed, c.IsSyndicated
		   from Sueetie_UserLog u inner join Sueetie_Users s on u.UserID = s.UserID 
		inner join Sueetie_UserLogCategories c on u.UserLogCategoryID = c.UserLogCategoryID
		order by u.LogDateTime desc
end
else begin		
		if @syndicatedList = 1
		begin
				insert into #z_Sueetie_UserLogActivity (UserLogID, UserLogCategoryID, UserLogCategoryCode, UserID, DisplayName, ItemID, DateTimeActivity, 
					ContentID, ContentTypeID, ApplicationID, GroupID, SourceID, SourceParentID, ToUserID, ApplicationPath, Email, IsDisplayed, IsSyndicated)  select top 100
				u.userlogid, u.UserLogCategoryID, c.UserLogCategoryCode, u.UserID, s.DisplayName, u.ItemID, u.LogDateTime,
					-1, -1, -1, -1, -1, -1, -1, 'members', s.Email, c.IsDisplayed, c.IsSyndicated
				   from Sueetie_UserLog u inner join Sueetie_Users s on u.UserID = s.UserID 
				inner join Sueetie_UserLogCategories c on u.UserLogCategoryID = c.UserLogCategoryID
				where c.IsSyndicated = 1
				order by u.LogDateTime desc
		end
		else begin
				insert into #z_Sueetie_UserLogActivity (UserLogID, UserLogCategoryID, UserLogCategoryCode, UserID, DisplayName, ItemID, DateTimeActivity, 
					ContentID, ContentTypeID, ApplicationID, GroupID, SourceID, SourceParentID, ToUserID, ApplicationPath, Email, IsDisplayed, IsSyndicated)  select top 100
				u.userlogid, u.UserLogCategoryID, c.UserLogCategoryCode, u.UserID, s.DisplayName, u.ItemID, u.LogDateTime,
					-1, -1, -1, -1, -1, -1, -1, 'members', s.Email, c.IsDisplayed, c.IsSyndicated
				   from Sueetie_UserLog u inner join Sueetie_Users s on u.UserID = s.UserID 
				inner join Sueetie_UserLogCategories c on u.UserLogCategoryID = c.UserLogCategoryID
				where c.IsDisplayed = 1
				order by u.LogDateTime desc
		end
end

update #z_Sueetie_UserLogActivity set ToUserID = s.UserID, ToUserDisplayName = s.Displayname from Sueetie_Users s inner join #z_Sueetie_UserLogActivity u
on u.ItemID = s.UserID where u.UserLogCategoryID in (110, 112)

update #z_Sueetie_UserLogActivity set ApplicationID = c.ApplicationID,  GroupID = a.GroupID,
	ContentTypeID = c.ContentTypeID, ContentID = c.ContentID, SourceID = c.SourceID, 
	Permalink = c.Permalink
	from Sueetie_Content c inner join #z_Sueetie_UserLogActivity u
	on u.ItemID = c.ContentID inner join Sueetie_Applications a
	on a.ApplicationID = c.ApplicationID
	and u.UserLogCategoryID > 199 and a.GroupID = @groupID

declare @groupsFolderName nvarchar(50)
select @groupsFolderName = SettingValue from Sueetie_Settings where SettingName = 'GroupsFolderName'

update #z_Sueetie_UserLogActivity set GroupPath = '/' + @groupsFolderName + '/' + av.GroupKey + '/' + av.ApplicationKey
from Sueetie_vw_Applications av inner join #z_Sueetie_UserLogActivity u
on av.ApplicationID = u.ApplicationID and u.GroupID > 0

update #z_Sueetie_UserLogActivity set ApplicationPath = a.ApplicationKey,
	ApplicationName = a.Description from Sueetie_Applications a inner join #z_Sueetie_UserLogActivity u
	on a.ApplicationID = u.ApplicationID

update #z_Sueetie_UserLogActivity set 
SourceParentPermalink =  '/' + ApplicationPath + '/post?id=' + convert(nvarchar(100),p.PostID),
SourceParentDescription = p.Title,
SourceParentID = p.SueetiePostID
from Sueetie_bePosts p 
inner join Sueetie_beComments c on p.PostID = c.PostID
inner join #z_Sueetie_UserLogActivity u on u.SourceID = c.SueetieCommentID 
where u.ContentTypeID = 2 and u.UserLogCategoryID = 201

update #z_Sueetie_UserLogActivity set 
SourceDescription = p.Title
from Sueetie_bePosts p 
inner join #z_Sueetie_UserLogActivity u on u.SourceID = p.SueetiePostID 
where u.ContentTypeID = 1 and u.UserLogCategoryID = 200

update #z_Sueetie_UserLogActivity set 
SourceParentPermalink = 
'/' + ApplicationPath + '/default.aspx?g=posts&t=' + convert(nvarchar(6),t.TopicID),
SourceParentDescription = t.Topic,
SourceParentID = t.TopicID
from yaf_Topic t
inner join yaf_Message m on m.TopicID = t.TopicID
inner join #z_Sueetie_UserLogActivity u on u.SourceID = m.MessageID
where u.ContentTypeID = 4 and u.UserLogCategoryID = 301

update #z_Sueetie_UserLogActivity set 
SourceDescription = t.Topic 
from yaf_Topic t
inner join yaf_Message m on m.TopicID = t.TopicID
inner join #z_Sueetie_UserLogActivity u on u.SourceID = m.TopicID
where u.ContentTypeID = 3 and u.UserLogCategoryID = 300

update #z_Sueetie_UserLogActivity set 
SourceDescription = w.PageTitle
from Sueetie_WikiPages w inner join
#z_Sueetie_UserLogActivity u on u.SourceID = w.PageID
where u.ContentTypeID = 5 and u.UserLogCategoryID in (400,401)

update #z_Sueetie_UserLogActivity set 
SourceDescription = m.Subject,
SourceParentDescription = w.PageTitle,
SourceParentPermalink = c.Permalink
from Sueetie_WikiMessages m inner join
#z_Sueetie_UserLogActivity u on u.SourceID = m.MessageID
inner join Sueetie_WikiPages w on m.PageID = w.PageID
inner join Sueetie_Content c on c.SourceID = w.PageID
where u.ContentTypeID = 23 and c.ContentTypeID = 5 and u.UserLogCategoryID = 402


update #z_Sueetie_UserLogActivity set 
SourceParentDescription = g.Title
from gs_Album g inner join #z_Sueetie_UserLogActivity u
on g.AlbumId = u.SourceID
and u.UserLogCategoryID between 500 and 599

update #z_Sueetie_UserLogActivity set 
SourceDescription = ct.Description
from Sueetie_ContentTypes ct inner join #z_Sueetie_UserLogActivity u
on ct.UserLogCategoryID = u.UserLogCategoryID
and u.UserLogCategoryID between 500 and 599

update #z_Sueetie_UserLogActivity set 
ApplicationName = g.Description
from gs_Gallery g inner join
gs_Album a 
on a.FKGalleryId = g.GalleryId inner join 
#z_Sueetie_UserLogActivity u
on a.AlbumId = u.SourceID
and u.UserLogCategoryID between 500 and 599

update #z_Sueetie_UserLogActivity set 
SourceParentPermalink = '/' + ApplicationPath + '/' + s.GalleryKey + '.aspx'
from Sueetie_gs_Gallery s inner join
gs_Gallery g 
on s.GalleryID = g.GalleryId inner join
gs_Album a 
on a.FKGalleryId = g.GalleryId inner join 
#z_Sueetie_UserLogActivity u
on a.AlbumId = u.SourceID
and u.UserLogCategoryID between 500 and 599

update #z_Sueetie_UserLogActivity set 
SourceDescription = p.Title
from SuCommerce_Products p inner join #z_Sueetie_UserLogActivity u
on p.ProductID = u.SourceID
and u.UserLogCategoryID = 600


update #z_Sueetie_UserLogActivity set 
SourceDescription = cms.PageTitle
from Sueetie_ContentPages cms inner join #z_Sueetie_UserLogActivity u
on cms.ContentPageID = u.SourceID
where u.UserLogCategoryID = 800

update #z_Sueetie_UserLogActivity set 
SourceDescription = cms.PageTitle
from Sueetie_ContentPages cms inner join Sueetie_ContentParts cp 
on cms.ContentPageID = cp.ContentPageID inner join #z_Sueetie_UserLogActivity u
on cp.ContentPartID = u.SourceID
where u.UserLogCategoryID = 801

update #z_Sueetie_UserLogActivity set 
SourceDescription = cal.EventTitle
from Sueetie_CalendarEvents cal inner join #z_Sueetie_UserLogActivity u
on cal.EventID = u.SourceID
where u.UserLogCategoryID = 900

delete from #z_Sueetie_UserLogActivity where UserLogCategoryID = 900 and SourceDescription is null

update #z_Sueetie_UserLogActivity set 
SourceParentPermalink =  c.CalendarUrl,
SourceParentDescription = c.CalendarTitle,
SourceParentID = c.CalendarID
from Sueetie_Calendars c 
inner join Sueetie_CalendarEvents cal on c.CalendarID = cal.CalendarID
inner join #z_Sueetie_UserLogActivity u on u.SourceID = cal.EventID
where u.ContentTypeID = 22 and u.UserLogCategoryID = 900

update #z_Sueetie_UserLogActivity set 
SourceDescription = t.Topic
from yaf_topic t inner join SuAddons_ForumAnswers a 
on a.TopicID = t.TopicID
inner join #z_Sueetie_UserLogActivity u on u.SourceID = a.AnswerID
where u.ContentTypeID = 24 and u.UserLogCategoryID = 302

update #z_Sueetie_UserLogActivity set 
ToUserID = su.UserID, ToUserDisplayName = su.DisplayName
from SuAddons_ForumAnswers a inner join yaf_Message m 
on a.MessageID = m.MessageID inner join yaf_User yu 
on m.UserID = yu.UserID inner join Sueetie_Users su
on yu.ProviderUserKey = su.MembershipID inner join
#z_Sueetie_UserLogActivity u on u.SourceID = a.AnswerID
where u.ContentTypeID = 24 and u.UserLogCategoryID = 302

select * from #z_Sueetie_UserLogActivity order by DateTimeActivity desc

drop table #z_sueetie_Userlogactivity

GO

