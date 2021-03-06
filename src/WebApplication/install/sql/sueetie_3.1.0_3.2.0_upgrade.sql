/* ------------------------------------------------------------------------------------------------------------------- */
/* 
	
	Sueetie Upgrade from Version 3.1.0 to 3.2.0

	Processes: 
	
	ALTER procedure [dbo].[Sueetie_User_Create]
	UPDATE Sueetie_UserLogCategories
	UPDATE Sueetie_ContentTypes
	RECREATE TABLE [dbo].[Sueetie_Applications]
	ALTER PROCEDURE [dbo].[Sueetie_Application_Add]
	
	--Marketplace Object Creation (generated from SQL below)

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
	
	-- YAF 1.9.5.5

	ALTER procedure [dbo].[yaf_sueetie_post_list] (run after YAF upgrade)

	-- GSP 2.4.6
    
    UPDATE GSP 2.4.4 - 2.4.5
    UPDATE GSP 2.4.5 - 2.4.6

	-- AddonPack Slideshows

	CREATE TABLE [dbo].[SuAddons_Slideshows]
	CREATE TABLE [dbo].[SuAddons_SlideshowImages]
	CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Add]
	CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Update]
	CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Add]
	CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Update]
	INSERT INTO [dbo].[SuAddons_Slideshows]
	INSERT INTO [dbo].[SuAddons_SlideshowImages]
	DROP Sueetie_Slideshow_Objects
	CREATE VIEW [dbo].[SuAddons_vw_SlideShowImages]

	
	-- AddonPack Slideshows

	CREATE TABLE [dbo].[SuAddons_Slideshows]
	CREATE TABLE [dbo].[SuAddons_SlideshowImages]
	CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Add]
	CREATE PROCEDURE [dbo].[SuAddons_Slideshow_Update]
	CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Add]
	CREATE PROCEDURE [dbo].[SuAddons_SlideshowImage_Update]
	INSERT INTO [dbo].[SuAddons_Slideshows]
	INSERT INTO [dbo].[SuAddons_SlideshowImages]
	DROP Sueetie_Slideshow_Objects
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
	ALTER PROCEDURE [dbo].[Sueetie_Request_Add]

	-- Content Part sueetie_content insert fix

	ALTER PROCEDURE [dbo].[Sueetie_ContentPart_Update]

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

	-- Forum Message Post Fix

	ALTER PROCEDURE  [dbo].[Sueetie_ForumMessage_Add]
	ALTER PROCEDURE  [dbo].[Sueetie_ForumTopic_Add]

	-- Forum Answers

	INSERT INTO Sueetie_ContentTypes
	CREATE TABLE [dbo].[SuAddons_ForumAnswers]
	CREATE PROCEDURE [dbo].[SuAddons_ForumAnswer_Add]
	CREATE PROCEDURE [dbo].[SuAddons_ForumAnswer_Delete]
	INSERT INTO SuAddons_Settings

	-- Media Sets

	ALTER TABLE Sueetie_gs_album

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

	ALTER VIEW [dbo].[Sueetie_vw_MediaAlbums]
	CREATE VIEW [dbo].[Sueetie_vw_MediaDirectories]
	
	ALTER PROCEDURE [dbo].[Sueetie_UserLogActivities_Get]

*/
/* ------------------------------------------------------------------------------------------------------------------- */

SET NOCOUNT ON
GO

print 'ALTER procedure [dbo].[Sueetie_User_Create]'

/* -- ALTER procedure [dbo].[Sueetie_User_Create]  ----------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[sueetie_User_Create]    Script Date: 03/23/2011 10:44:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER procedure [dbo].[sueetie_User_Create]
(
	@UserName nvarchar(50),
	@Email nvarchar(255),
	@MembershipID uniqueidentifier,
	@DisplayName nvarchar(255),
	@IsActive bit,
	@IP nvarchar(15)
)
AS
BEGIN


insert into Sueetie_Users (MembershipID, UserName, Email, DisplayName, IsActive, IP) values (@MembershipID, @UserName, @Email, @DisplayName, @IsActive, @IP)

declare @id int
set @id = @@IDENTITY
 
insert into Sueetie_UserAvatar (UserID) values (@id)
insert into Sueetie_UserFollowCounts (UserID) values (@id)

-- Insert into GSP 2.4.4+ User Gallery Profile table
insert into gs_UserGalleryProfile select @UserName,1,'ShowMediaObjectMetadata','False'
insert into gs_UserGalleryProfile select @UserName,1,'EnableUserAlbum','True'
insert into gs_UserGalleryProfile select @UserName,1,'UserAlbumId','0'

select @id

END
GO





print 'UPDATE Sueetie_UserLogCategories'

/* -- UPDATE Sueetie_UserLogCategories  -------------------------------------------------------------------------------- */

UPDATE Sueetie_UserLogCategories set UserLogCategoryCode = 'Marketplace Product', 
	UserLogCategoryDescription = 'New Marketplace Product'
	where UserLogCategoryID = 600
	
GO

print 'UPDATE Sueetie_ContentTypes'

/* -- UPDATE Sueetie_ContentTypes ---------------------------------------- */

UPDATE Sueetie_ContentTypes set ContentTypeName = 'MarketplaceProduct', Description = 'Marketplace Products' 
	where ContentTypeID = 18
	

print 'RECREATE TABLE [dbo].[Sueetie_Applications]'

/* -- RECREATE TABLE [dbo].[Sueetie_Applications] ------------------------------------- */


CREATE TABLE #Sueetie_Applications(
	[ApplicationID] [int] NOT NULL,
	[ApplicationTypeID] [int] NOT NULL,
	[ApplicationKey] [nvarchar](25) NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[GroupID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL
	)
	
insert into #Sueetie_Applications select * from Sueetie_Applications
GO

DROP TABLE [dbo].[Sueetie_Applications]
GO
	
CREATE TABLE [dbo].[Sueetie_Applications](
	[ApplicationID] [int] NOT NULL,
	[ApplicationTypeID] [int] NOT NULL,
	[ApplicationKey] [nvarchar](25) NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[GroupID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_Applications] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Sueetie_Applications] ADD  CONSTRAINT [DF_Sueetie_Applications_ApplicationID1]  DEFAULT ((0)) FOR [ApplicationTypeID]
GO

ALTER TABLE [dbo].[Sueetie_Applications] ADD  CONSTRAINT [DF_Sueetie_Applications_GroupID1]  DEFAULT ((0)) FOR [GroupID]
GO

ALTER TABLE [dbo].[Sueetie_Applications] ADD  CONSTRAINT [DF_Sueetie_Applications_IsActive1]  DEFAULT ((1)) FOR [IsActive]
GO

ALTER TABLE [dbo].[Sueetie_Applications] ADD  CONSTRAINT [DF__Sueetie_A__IsLoc__174363E21]  DEFAULT ((1)) FOR [IsLocked]
GO

INSERT INTO [dbo].[Sueetie_Applications] (
	[ApplicationID],
	[ApplicationTypeID],
	[ApplicationKey],
	[Description],
	[GroupID],
	[IsActive],
	[IsLocked]
)
	SELECT 0, 0, 'na', 'Unknown or Other', 0, 1, 1 UNION
	SELECT 1, 1, 'blog', 'Blog', 0, 1, 1 UNION
	SELECT 2, 2, 'forum', 'Community Forums', 0, 1, 1 UNION
	SELECT 3, 3, 'wiki', 'Wiki', 0, 1, 1 UNION
	SELECT 4, 4, 'media', 'Media Library', 0, 1, 1 UNION
	SELECT 5, 5, 'marketplace', 'Marketplace', 0, 1, 1 UNION
	SELECT 6, 6, 'classifieds', 'Community Classifieds', 0, 1, 1 UNION
	SELECT 7, 7, 'cms', 'CMS', 0, 1, 1 
	GO		

	
DROP TABLE #sueetie_applications
go
	

print 'ALTER PROCEDURE [dbo].[Sueetie_Application_Add]'

/* -- ALTER PROCEDURE [dbo].[Sueetie_Application_Add] ------------------------------------- */

/****** Object:  StoredProcedure [dbo].[Sueetie_Application_Add]    Script Date: 03/18/2011 17:47:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Sueetie_Application_Add]
	@ApplicationID int,
	@ApplicationTypeID int,
	@ApplicationKey nvarchar(25),
	@Description nvarchar(1000),
	@GroupID int
 
AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_Applications] (
	[ApplicationID],
	[ApplicationTypeID],
	[ApplicationKey],
	[Description],
	[GroupID],
	[IsActive],
	[IsLocked]
) VALUES (
	@ApplicationID,
	@ApplicationTypeID,
	@ApplicationKey,
	@Description,
	@GroupID,
	1,0
)

select @ApplicationID

GO


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


print 'ALTER VIEW [dbo].[SuCommerce_vw_Purchases]'

/* -- ALTER VIEW [dbo].[SuCommerce_vw_Purchases] ------------------------------------------------------------ */

PRINT '/****** Object:  View [dbo].[SuCommerce_vw_Purchases]    Script Date: 03/29/2011 12:01:41 ******/'
SET ANSI_NULLS ON
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


print 'ALTER procedure [dbo].[yaf_sueetie_post_list]'

/* -- ALTER procedure [dbo].[yaf_sueetie_post_list] ------------------------------------- */

/****** Object:  StoredProcedure [dbo].[yaf_post_list]    Script Date: 04/05/2011 14:21:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER procedure [dbo].[yaf_sueetie_post_list](
                 @TopicID int,
				 @AuthorUserID int,
				 @UpdateViewCount smallint=1, 
				 @ShowDeleted bit = 1, 
				 @StyledNicks bit = 0, 
				 @SincePostedDate datetime, 
				 @ToPostedDate datetime, 
				 @SinceEditedDate datetime, 
				 @ToEditedDate datetime, 
				 @PageIndex int = 1, 
				 @PageSize int = 0, 
				 @SortPosted int = 2, 
				 @SortEdited int = 0,
				 @SortPosition int = 0,				
				 @ShowThanks bit = 0,
				 @MessagePosition int = 0) as
begin
   declare @post_totalrowsnumber int 
   declare @firstselectrownum int 
  
   declare @firstselectposted datetime
   declare @firstselectedited datetime

   declare @floor decimal
   declare @ceiling decimal
  
   declare @offset int 
   
   declare @pagecorrection int
   declare @pageshift int;

	set nocount on
	if @UpdateViewCount>0
		update [dbo].[yaf_Topic] set [Views] = [Views] + 1 where TopicID = @TopicID
	-- find total returned count
		select
		@post_totalrowsnumber = COUNT(m.MessageID)
	from
		[dbo].[yaf_Message] m
	where
		m.TopicID = @TopicID
		AND m.IsApproved = 1
		AND (m.IsDeleted = 0 OR ((@ShowDeleted = 1 AND m.IsDeleted = 1) OR (@AuthorUserID > 0 AND m.UserID = @AuthorUserID)))
		AND m.Posted BETWEEN
		 @SincePostedDate AND @ToPostedDate
		 /*
		AND 
		m.Edited >= SinceEditedDate
		*/
  
   -- number of messages on the last page @post_totalrowsnumber - @floor*@PageSize
   if (@MessagePosition > 0)
 begin

       -- round to ceiling - total number of pages  
       SELECT @ceiling = CEILING(CONVERT(decimal,@post_totalrowsnumber)/@PageSize) 
       -- round to floor - a number of full pages
       SELECT @floor = FLOOR(CONVERT(decimal,@post_totalrowsnumber)/@PageSize)

       SET @pageshift = @MessagePosition - (@post_totalrowsnumber - @floor*@PageSize)
            if  @pageshift < 0
			   begin
			      SET @pageshift = 0
				     end   
   -- here pageshift converts to full pages 
   if (@pageshift <= 0)
   begin    
   set @pageshift = 0
   end
   else 
   begin
   set @pageshift = CEILING(CONVERT(decimal,@pageshift)/@PageSize) 
   end   
   
   SET @PageIndex = @ceiling - @pageshift 
   if @ceiling != @floor
   SET @PageIndex = @PageIndex - 1	      

   select @firstselectrownum = (@PageIndex) * @PageSize + 1    
  
 end  
 else
 begin
   select @PageIndex = @PageIndex+1;
   select @firstselectrownum = (@PageIndex - 1) * @PageSize + 1 
 end 
  
   -- find first selectedrowid 
   if (@firstselectrownum > 0)   
   set rowcount @firstselectrownum
   else
   -- should not be 0
   set rowcount 1
   	
   select		
		@firstselectposted = m.Posted,
		@firstselectedited = m.Edited
	from
		[dbo].[yaf_Message] m
		join [dbo].[yaf_User] b on b.UserID=m.UserID
		join [dbo].[yaf_Topic] d on d.TopicID=m.TopicID
		join [dbo].[yaf_Forum] g on g.ForumID=d.ForumID
		join [dbo].[yaf_Category] h on h.CategoryID=g.CategoryID
		join [dbo].[yaf_Rank] c on c.RankID=b.RankID
	where
		m.TopicID = @TopicID
		AND m.IsApproved = 1
		AND (m.IsDeleted = 0 OR ((@ShowDeleted = 1 AND m.IsDeleted = 1) OR (@AuthorUserID > 0 AND m.UserID = @AuthorUserID)))
		AND m.Posted BETWEEN
		 @SincePostedDate AND @ToPostedDate
		 /*
		AND m.Edited > @SinceEditedDate
		*/
		
	order by
		(case 
        when @SortPosition = 1 then m.Position end) ASC,	
		(case 
        when @SortPosted = 2 then m.Posted end) DESC,
		(case 
        when @SortPosted = 1 then m.Posted end) ASC, 
		(case 
        when @SortEdited = 2 then m.Edited end) DESC,
		(case 
        when @SortEdited = 1 then m.Edited end) ASC  	 		
			
    
	set rowcount @PageSize	
		
	select
		d.TopicID,		
		TopicFlags	= d.Flags,
		ForumFlags	= g.Flags,
		m.MessageID,
		m.Posted,
		[Subject] = d.Topic,
		[Message] = m.Message, 
		m.UserID,
		m.Position,
		m.Indent,
		m.IP,
		m.Flags,
		m.EditReason,
		m.IsModeratorChanged,
		m.IsDeleted,
		m.DeleteReason,
		UserName	= IsNull(m.UserName,b.Name),
		b.Joined,
		b.Avatar,
		b.[Signature],
		Posts		= b.NumPosts,
		b.Points,
		d.[Views],
		d.ForumID,
		RankName = c.Name,		
		c.RankImage,
		Style = case(@StyledNicks)
			when 1 then  ISNULL(( SELECT TOP 1 f.Style FROM [dbo].[yaf_UserGroup] e 
		join [dbo].[yaf_Group] f on f.GroupID=e.GroupID WHERE e.UserID=b.UserID AND LEN(f.Style) > 2 ORDER BY f.SortOrder), c.Style)  
			else ''	 end, 
		Edited = IsNull(m.Edited,m.Posted),
		HasAttachments	= ISNULL((select top 1 1 from [dbo].[yaf_Attachment] x where x.MessageID=m.MessageID),0),
		HasAvatarImage = ISNULL((select top 1 1 from [dbo].[yaf_User] x where x.UserID=b.UserID and AvatarImage is not null),0),
		TotalRows = @post_totalrowsnumber,
		PageIndex = @PageIndex,
		SueetieUserID = s.UserID,
		SueetieDisplayName = isnull(s.DisplayName,s.UserName)
	from
		[dbo].[yaf_Message] m
		join [dbo].[yaf_User] b on b.UserID=m.UserID
		join [dbo].[yaf_Topic] d on d.TopicID=m.TopicID
		join [dbo].[yaf_Forum] g on g.ForumID=d.ForumID
		join [dbo].[yaf_Category] h on h.CategoryID=g.CategoryID
		join [dbo].[yaf_Rank] c on c.RankID=b.RankID
        join [dbo].[Sueetie_Users] s on s.MembershipID = b.ProviderUserKey
		join [dbo].[Sueetie_UserAvatar] v on v.UserID =  s.UserID 
	where
		m.TopicID = @TopicID
		AND m.IsApproved = 1
		AND (m.IsDeleted = 0 OR ((@ShowDeleted = 1 AND m.IsDeleted = 1) OR (@AuthorUserID > 0 AND m.UserID = @AuthorUserID)))
		AND (m.Posted is null OR (m.Posted is not null AND
		(m.Posted >= (case 
        when @SortPosted = 1 then
		 @firstselectposted end) OR m.Posted <= (case 
        when @SortPosted = 2 then @firstselectposted end) OR
		m.Posted >= (case 
        when @SortPosted = 0 then 0 end))))	AND
		(m.Posted <= @ToPostedDate)	
		/*
		AND (m.Edited is null OR (m.Edited is not null AND
		(m.Edited >= (case 
        when @SortEdited = 1 then @firstselectedited end) 
		OR m.Edited <= (case 
        when @SortEdited = 2 then @firstselectedited end) OR
		m.Edited >= (case 
        when @SortEdited = 0 then 0
		end)))) 
		*/
	order by		
		(case 
        when @SortPosition = 1 then m.Position end) ASC,	
		(case 
        when @SortPosted = 2 then m.Posted end) DESC,
		(case 
        when @SortPosted = 1 then m.Posted end) ASC, 
		(case 
        when @SortEdited = 2 then m.Edited end) DESC,
		(case 
        when @SortEdited = 1 then m.Edited end) ASC  

		SET ROWCOUNT 0
end

GO

print 'UPDATE GSP 2.4.4 - 2.4.5'

/* -- UPDATE GSP 2.4.4 - 2.4.5 -------------------------------------------------------------------------------- */

IF NOT EXISTS (SELECT * FROM [dbo].[gs_AppSetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='AllowGalleryAdminToManageUsersAndRoles')
BEGIN
	INSERT INTO [dbo].[gs_AppSetting] ([SettingName], [SettingValue])
	VALUES ('AllowGalleryAdminToManageUsersAndRoles','True');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_AppSetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='AllowGalleryAdminToViewAllUsersAndRoles')
BEGIN
	INSERT INTO [dbo].[gs_AppSetting] ([SettingName], [SettingValue])
	VALUES ('AllowGalleryAdminToViewAllUsersAndRoles','True');
END
GO

UPDATE [dbo].[gs_AppSetting]
SET [SettingValue] = '2.4.5'
WHERE [SettingName] = 'DataSchemaVersion';
GO

print 'UPDATE GSP 2.4.5 - 2.4.6'

/* -- UPDATE GSP 2.4.5- 2.4.6 -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[sueetie_User_Create]    Script Date: 03/23/2011 10:44:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

/* 1. Edit stored proc to remove unnecessary join (improves synchs). */
ALTER PROCEDURE [dbo].[gs_MediaObjectSelectHashKeys]
AS
SET NOCOUNT ON

SELECT HashKey
FROM [dbo].[gs_MediaObject]

RETURN
GO

/* 2. Add a non-clustered index to the gs_MediaObject table (improves searches). */
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObject]') AND name = N'IDX_gs_MediaObject_MediaObjectId_FKAlbumId')
CREATE NONCLUSTERED INDEX [IDX_gs_MediaObject_MediaObjectId_FKAlbumId] ON [dbo].[gs_MediaObject] 
( 
 [OriginalFilename] ASC
)
 INCLUDE ([MediaObjectId], [FKAlbumId])
 WITH (PAD_INDEX=OFF, STATISTICS_NORECOMPUTE=OFF, SORT_IN_TEMPDB=OFF, IGNORE_DUP_KEY=OFF, DROP_EXISTING=OFF, ONLINE=OFF, ALLOW_ROW_LOCKS=ON, ALLOW_PAGE_LOCKS=ON) ON [PRIMARY]
GO

/* 3. Adds a non-clustered index to the gs_Album table (improves synchs). */
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[gs_Album]') AND name = N'IDX_gs_Album_AlbumId')
CREATE NONCLUSTERED INDEX [IDX_gs_Album_AlbumId] ON [dbo].[gs_Album] 
(
 [AlbumId] ASC
)
 WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
GO

/* 4. Adds four statistics (improves synchs). */
IF NOT EXISTS (SELECT * FROM sys.stats WHERE object_id = OBJECT_ID(N'[dbo].[gs_Album]') AND name = N'STAT_gs_Album_FKGalleryId_AlbumId')
CREATE STATISTICS [STAT_gs_Album_FKGalleryId_AlbumId] ON [dbo].[gs_Album] ([FKGalleryId], [AlbumId])
GO

IF NOT EXISTS (SELECT * FROM sys.stats WHERE object_id = OBJECT_ID(N'[dbo].[gs_Album]') AND name = N'STAT_gs_Album_AlbumParentId_FKGalleryId_AlbumId')
CREATE STATISTICS [STAT_gs_Album_AlbumParentId_FKGalleryId_AlbumId] ON [dbo].[gs_Album] ([AlbumParentId], [FKGalleryId], [AlbumId])
GO

IF NOT EXISTS (SELECT * FROM sys.stats WHERE object_id = OBJECT_ID(N'[dbo].[gs_Album]') AND name = N'STAT_gs_Album_AlbumId_AlbumParentId')
CREATE STATISTICS [STAT_gs_Album_AlbumId_AlbumParentId] ON [dbo].[gs_Album] ([AlbumId], [AlbumParentId])
GO

IF NOT EXISTS (SELECT * FROM sys.stats WHERE object_id = OBJECT_ID(N'[dbo].[gs_MediaObjectMetadata]') AND name = N'STAT_gs_MediaObjectMetadata_MediaObjectMetadataId_FKMediaObjectId')
CREATE STATISTICS [STAT_gs_MediaObjectMetadata_MediaObjectMetadataId_FKMediaObjectId] ON [dbo].[gs_MediaObjectMetadata]([MediaObjectMetadataId], [FKMediaObjectId])
GO

/* 5. Update app settings. */
UPDATE [dbo].[gs_AppSetting]
SET [SettingValue] = '//ajax.googleapis.com/ajax/libs/jquery/1/jquery.min.js'
WHERE [SettingName] = 'JQueryScriptPath';
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_AppSetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='JQueryUiScriptPath')
BEGIN
 INSERT INTO [dbo].[gs_AppSetting] ([SettingName], [SettingValue])
 VALUES ('JQueryUiScriptPath','//ajax.googleapis.com/ajax/libs/jqueryui/1/jquery-ui.min.js');
END
GO

/* 6. Update gallery settings. */
IF NOT EXISTS (SELECT * FROM [dbo].[gs_GallerySetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='DiscardOriginalImageDuringImport')
BEGIN
 INSERT INTO [dbo].[gs_GallerySetting] ([FKGalleryId], [IsTemplate], [SettingName], [SettingValue])
 VALUES (-2147483648,1,'DiscardOriginalImageDuringImport','False');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_GallerySetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='EnableAlbumZipDownload')
BEGIN
 INSERT INTO [dbo].[gs_GallerySetting] ([FKGalleryId], [IsTemplate], [SettingName], [SettingValue])
 VALUES (-2147483648,1,'EnableAlbumZipDownload','True');
END
GO

UPDATE [dbo].[gs_GallerySetting]
SET [SettingName] = 'EnableGalleryObjectZipDownload'
WHERE [SettingName] = 'EnableMediaObjectZipDownload';
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_GallerySetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='EnableAutoSync')
BEGIN
 INSERT INTO [dbo].[gs_GallerySetting] ([FKGalleryId], [IsTemplate], [SettingName], [SettingValue])
 VALUES (-2147483648,1,'EnableAutoSync','False');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_GallerySetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='AutoSyncIntervalMinutes')
BEGIN
 INSERT INTO [dbo].[gs_GallerySetting] ([FKGalleryId], [IsTemplate], [SettingName], [SettingValue])
 VALUES (-2147483648,1,'AutoSyncIntervalMinutes','1440');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_GallerySetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='LastAutoSync')
BEGIN
 INSERT INTO [dbo].[gs_GallerySetting] ([FKGalleryId], [IsTemplate], [SettingName], [SettingValue])
 VALUES (-2147483648,1,'LastAutoSync','');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_GallerySetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='EnableRemoteSync')
BEGIN
 INSERT INTO [dbo].[gs_GallerySetting] ([FKGalleryId], [IsTemplate], [SettingName], [SettingValue])
 VALUES (-2147483648,1,'EnableRemoteSync','False');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_GallerySetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='RemoteAccessPassword')
BEGIN
 INSERT INTO [dbo].[gs_GallerySetting] ([FKGalleryId], [IsTemplate], [SettingName], [SettingValue])
 VALUES (-2147483648,1,'RemoteAccessPassword','');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_GallerySetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='MediaObjectCaptionTemplate')
BEGIN
 INSERT INTO [dbo].[gs_GallerySetting] ([FKGalleryId], [IsTemplate], [SettingName], [SettingValue])
 VALUES (-2147483648,1,'MediaObjectCaptionTemplate','{Title}');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_GallerySetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='MetadataDisplaySettings')
BEGIN
 INSERT INTO [dbo].[gs_GallerySetting] ([FKGalleryId], [IsTemplate], [SettingName], [SettingValue])
 VALUES (-2147483648,1,'MetadataDisplaySettings','29:T,34:T,35:F,8:T,102:T,106:T,22:T,14:T,9:T,5:T,28:T,2:T,26:T,4:T,6:T,7:T,12:T,13:T,15:T,16:T,17:T,18:T,21:T,23:T,24:T,10:T,25:T,27:T,11:T,1:T,32:T,3:T,0:T,31:T,20:T,30:T,33:T,19:T,36:T,37:F,38:T,39:F,40:T,101:F,103:F,104:F,105:F,108:F,107:F,110:T,109:T,1012:T,1013:T,1010:T,1011:T,1014:T,1017:T,1018:T,1015:T,1016:T,1003:T,1004:T,1001:T,1002:T,1005:T,1008:T,1009:T,1006:T,1007:T');
END
GO

IF NOT EXISTS (SELECT * FROM [dbo].[gs_GallerySetting] WITH (UPDLOCK, HOLDLOCK) WHERE [SettingName]='GpsMapUrlTemplate')
BEGIN
 INSERT INTO [dbo].[gs_GallerySetting] ([FKGalleryId], [IsTemplate], [SettingName], [SettingValue])
 VALUES (-2147483648,1,'GpsMapUrlTemplate','<a href=''http://bing.com/maps/default.aspx?sp=point.{GpsLatitude}_{GpsLongitude}_{TitleNoHtml}__{MediaObjectPageUrl}_{MediaObjectUrl}&style=a&lvl=13'' target=''_blank'' title=''View map''>{GpsLocation}</a>');
END
GO

UPDATE [dbo].[gs_AppSetting]
SET [SettingValue] = '2.4.6'
WHERE [SettingName] = 'DataSchemaVersion';
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

print 'DROP Sueetie_Slideshow_Objects'

/* -- DROP Sueetie_Slideshow_Objects -------------------------------------------------------------------------------- */


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_DateCreated]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_SlideshowImages] DROP CONSTRAINT [DF_Sueetie_DateCreated]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_IsMainPreview]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_SlideshowImages] DROP CONSTRAINT [DF_Sueetie_IsMainPreview]
END

GO

/****** Object:  Table [dbo].[Sueetie_SlideshowImages]    Script Date: 04/11/2011 09:34:49 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sueetie_SlideshowImages]') AND type in (N'U'))
DROP TABLE [dbo].[Sueetie_SlideshowImages]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_Slideshows_SourceID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_Slideshows] DROP CONSTRAINT [DF_Sueetie_Slideshows_SourceID]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_Slideshows_MainSlideshowImageID]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_Slideshows] DROP CONSTRAINT [DF_Sueetie_Slideshows_MainSlideshowImageID]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_Slideshows_Active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_Slideshows] DROP CONSTRAINT [DF_Sueetie_Slideshows_Active]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_Slideshows_FullFixedHeight]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_Slideshows] DROP CONSTRAINT [DF_Sueetie_Slideshows_FullFixedHeight]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_Slideshows_FullFixedWidth]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_Slideshows] DROP CONSTRAINT [DF_Sueetie_Slideshows_FullFixedWidth]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_Slideshows_MediumFixedHeight]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_Slideshows] DROP CONSTRAINT [DF_Sueetie_Slideshows_MediumFixedHeight]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_Slideshows_MediumFixedWidth]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_Slideshows] DROP CONSTRAINT [DF_Sueetie_Slideshows_MediumFixedWidth]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_Slideshows_SmallFixedHeight]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_Slideshows] DROP CONSTRAINT [DF_Sueetie_Slideshows_SmallFixedHeight]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Sueetie_Slideshows_SmallFixedWidth]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Sueetie_Slideshows] DROP CONSTRAINT [DF_Sueetie_Slideshows_SmallFixedWidth]
END

GO

/****** Object:  Table [dbo].[Sueetie_Slideshows]    Script Date: 04/11/2011 09:35:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sueetie_Slideshows]') AND type in (N'U'))
DROP TABLE [dbo].[Sueetie_Slideshows]
GO


/****** Object:  StoredProcedure [dbo].[Sueetie_Slideshow_Add]    Script Date: 04/11/2011 09:35:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sueetie_Slideshow_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Sueetie_Slideshow_Add]
GO


/****** Object:  StoredProcedure [dbo].[Sueetie_Slideshow_Update]    Script Date: 04/11/2011 09:35:23 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sueetie_Slideshow_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Sueetie_Slideshow_Update]
GO

/****** Object:  StoredProcedure [dbo].[Sueetie_SlideshowImage_Add]    Script Date: 04/11/2011 09:35:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sueetie_SlideshowImage_Add]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Sueetie_SlideshowImage_Add]
GO

/****** Object:  StoredProcedure [dbo].[Sueetie_SlideshowImage_Update]    Script Date: 04/11/2011 09:35:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sueetie_SlideshowImage_Update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Sueetie_SlideshowImage_Update]
GO

/****** Object:  StoredProcedure [dbo].[Sueetie_SlideshowMainImage_Set]    Script Date: 04/11/2011 09:35:43 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Sueetie_SlideshowMainImage_Set]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Sueetie_SlideshowMainImage_Set]
GO

/****** Object:  View [dbo].[Sueetie_vw_SlideshowImages]    Script Date: 04/11/2011 09:42:06 ******/
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[Sueetie_vw_SlideshowImages]'))
DROP VIEW [dbo].[Sueetie_vw_SlideshowImages]
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

print 'ALTER PROCEDURE [dbo].[Sueetie_Request_Add]'

/* -- ALTER PROCEDURE [dbo].[Sueetie_Request_Add]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[Sueetie_Request_Add]    Script Date: 04/29/2011 10:44:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[Sueetie_Request_Add]
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


print 'ALTER PROCEDURE [dbo].[Sueetie_ContentPart_Update]'

/* -- ALTER PROCEDURE [dbo].[Sueetie_ContentPart_Update]  -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[Sueetie_ContentPart_Update]    Script Date: 05/02/2011 13:22:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[Sueetie_ContentPart_Update]
	@ContentName nvarchar(256),
	@ContentPageID int,
	@ContentPageGroupID int,
	@Permalink nvarchar(255),
	@LastUpdateUserID int,
	@ContentText ntext,
	@ApplicationID int,
	@ContentID int OUTPUT
AS

SET NOCOUNT ON

DECLARE @ContentPartID int
Set @ContentID = -1

IF (SELECT COUNT(*) FROM [dbo].[Sueetie_ContentParts] WHERE ContentName = @ContentName) > 0
BEGIN

UPDATE [dbo].[Sueetie_ContentParts] SET
	[ContentName] = @ContentName,
	[ContentPageID] = @ContentPageID,
	[LastUpdateDateTime] = GETDATE(),
	[LastUpdateUserID] = @LastUpdateUserID,
	[ContentText] = @ContentText
WHERE
	[ContentName] = @ContentName

select @ContentPartID = ContentPartID from Sueetie_ContentParts where ContentName = @ContentName
	
END ELSE
BEGIN

INSERT INTO [dbo].[Sueetie_ContentParts] (
	[ContentName],
	[ContentPageID],	
	[LastUpdateDateTime],
	[LastUpdateUserID],
	[ContentText]
) VALUES (
	@ContentName,
	@ContentPageID,
	GETDATE(),
	@LastUpdateUserID,
	@ContentText
)
SET @ContentPartID = SCOPE_IDENTITY() 

END

if @ContentPageGroupID > 0
begin

	DECLARE @existingContentID int
	SELECT @existingContentID = contentID from Sueetie_Content where ContentTypeID = 21 and SourceID = @ContentPartID

	if @existingContentID is null
		BEGIN
			-- Note: '21' is SueetieContentTypeID for CMS Page Content Part
			insert into Sueetie_Content (SourceID, ContentTypeID, ApplicationID, UserID, Permalink, DateTimeCreated, IsRestricted) 
				values (@ContentPartID, 21, @ApplicationID, @LastUpdateUserID, @Permalink, GETDATE(), 0)

			SET @ContentID = SCOPE_IDENTITY() 
		END ELSE
		BEGIN
			update Sueetie_Content set Permalink = @Permalink where ContentTypeID = 21 and SourceID = @ContentPartID
			SET @ContentID = @existingContentID	
		END
END

select @ContentID
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


print 'ALTER VIEW [dbo].[Sueetie_vw_Blogs]'

/* -- ALTER VIEW [dbo].[Sueetie_vw_Blogs] -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[Sueetie_vw_Blogs]    Script Date: 05/11/2011 12:17:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[Sueetie_vw_Blogs]
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


print 'ALTER PROCEDURE  [dbo].[Sueetie_ForumMessage_Add]'

/* -- ALTER PROCEDURE  [dbo].[Sueetie_ForumMessage_Add] -------------------------------------------------------------------------------- */

/****** Object:  StoredProcedure [dbo].[Sueetie_ForumMessage_Add]    Script Date: 05/18/2011 15:37:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--region [dbo].[Sueetie_beComment]


ALTER PROCEDURE  [dbo].[Sueetie_ForumMessage_Add]
	@ContentTypeID int,
	@ApplicationID int,
	@BoardID int,
	@ForumID int,
	@MessageID int,
	@YAFUserID int
 
AS

SET NOCOUNT ON


declare @IsRestricted bit
declare @RegisteredGroupID int
declare @AccessMaskID int
declare @MemberAccessMaskID int
declare @ReadOnlyAccessMaskID int
declare @AnnouncementAccessMaskID int

select @IsRestricted = 0

select @MemberAccessMaskID = AccessMaskID from yaf_AccessMask 
	where BoardID = @BoardID and Name = 'Member Access'
	select @ReadOnlyAccessMaskID = AccessMaskID from yaf_AccessMask 
	where BoardID = @BoardID and Name = 'Read Only Access'
	select @AnnouncementAccessMaskID = AccessMaskID from yaf_AccessMask 
	where BoardID = @BoardID and Name = 'Announcement Access'
select @RegisteredGroupID = groupid from yaf_Group 
	where BoardID = @BoardID and Name = 'Registered'
select @AccessMaskID = AccessMaskID from yaf_ForumAccess 
	where ForumID = @ForumID and GroupID = @RegisteredGroupID

if (@AccessMaskID not in (@MemberAccessMaskID,@ReadOnlyAccessMaskID,@AnnouncementAccessMaskID))
begin
	select @IsRestricted = 1
end


declare @groupkey nvarchar(50)
declare @grouppath nvarchar(50)

select @groupkey = groupkey from sueetie_applications a inner join sueetie_groups g
on a.groupid = g.groupid  where a.applicationID = @ApplicationID;

if @groupkey is null
begin
	select @grouppath = ''
end
else begin
	select @grouppath = '/groups/' + @groupkey
end

declare @userid int
select @userid = s.UserID from Sueetie_Users s inner join yaf_User y on s.MembershipID = y.ProviderUserKey and y.UserID = @YAFUserID

declare @msgID nvarchar(10)
select @msgID = rtrim(convert(nvarchar(10),@MessageID))

declare @appkey nvarchar(25)
select @appkey = applicationkey from Sueetie_Applications where ApplicationID = @ApplicationID

INSERT INTO [dbo].[Sueetie_Content] (
	[SourceID],
	[ContentTypeID],
	[ApplicationID],
	[UserID],
	[Permalink],
	[DateTimeCreated],
	[IsRestricted]
) VALUES (
	@MessageID,
	@ContentTypeID,
	@ApplicationID,
	@UserID,
	@grouppath + '/' + @appkey + '/default.aspx?g=posts&m=' + @msgID + '#post' + @msgID,
	Getdate(),
	@IsRestricted
)

declare @ContentID int
select @ContentID = @@IDENTITY
select @ContentID
GO

print 'ALTER PROCEDURE  [dbo].[Sueetie_ForumTopic_Add]'

/* -- ALTER PROCEDURE  [dbo].[Sueetie_ForumTopic_Add] -------------------------------------------------------------------------------- */

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[Sueetie_beComment]


ALTER PROCEDURE  [dbo].[Sueetie_ForumTopic_Add]
	@ContentTypeID int,
	@ApplicationID int,
	@BoardID int,
	@ForumID int,
	@TopicID int,
	@YAFUserID int
 
AS

SET NOCOUNT ON


declare @IsRestricted bit
declare @RegisteredGroupID int
declare @AccessMaskID int
declare @MemberAccessMaskID int
declare @ReadOnlyAccessMaskID int
declare @AnnouncementAccessMaskID int

select @IsRestricted = 0
select @MemberAccessMaskID = AccessMaskID from yaf_AccessMask 
	where BoardID = @BoardID and Name = 'Member Access'
	select @ReadOnlyAccessMaskID = AccessMaskID from yaf_AccessMask 
	where BoardID = @BoardID and Name = 'Read Only Access'
	select @AnnouncementAccessMaskID = AccessMaskID from yaf_AccessMask 
	where BoardID = @BoardID and Name = 'Announcement Access'
select @RegisteredGroupID = groupid from yaf_Group 
	where BoardID = @BoardID and Name = 'Registered'
select @AccessMaskID = AccessMaskID from yaf_ForumAccess 
	where ForumID = @ForumID and GroupID = @RegisteredGroupID

if (@AccessMaskID not in (@MemberAccessMaskID,@ReadOnlyAccessMaskID,@AnnouncementAccessMaskID))
begin
	select @IsRestricted = 1
end

declare @groupkey nvarchar(50)
declare @grouppath nvarchar(50)

select @groupkey = groupkey from sueetie_applications a inner join sueetie_groups g
on a.groupid = g.groupid  where a.applicationID = @ApplicationID;

if @groupkey is null
begin
	select @grouppath = ''
end
else begin
	select @grouppath = '/groups/' + @groupkey
end

declare @userid int
select @userid = s.UserID from Sueetie_Users s inner join yaf_User y on s.MembershipID = y.ProviderUserKey and y.UserID = @YAFUserID

declare @appkey nvarchar(25)
select @appkey = applicationkey from Sueetie_Applications where ApplicationID = @ApplicationID

INSERT INTO [dbo].[Sueetie_Content] (
	[SourceID],
	[ContentTypeID],
	[ApplicationID],
	[UserID],
	[Permalink],
	[DateTimeCreated],
	[IsRestricted]
) VALUES (
	@TopicID,
	@ContentTypeID,
	@ApplicationID,
	@UserID,
	@grouppath + '/' + @appkey + '/default.aspx?g=posts&t=' + rtrim(convert(nvarchar(10),@TopicID)),
	Getdate(),
	@IsRestricted
)

declare @ContentID int
select @ContentID = @@IDENTITY
select @ContentID
GO

print 'INSERT INTO Sueetie_ContentTypes'

/* -- INSERT INTO Sueetie_ContentTypes -------------------------------------------------------------------------------- */

INSERT INTO Sueetie_ContentTypes select 24, 'ForumAnswer', 'Forum Answer', 0, 302, 0

declare @ctypeCount int
select @ctypeCount = count(*) from sueetie_ContentTypes where ContentTypeID = 16 
if @ctypeCount = 0 
BEGIN
	insert into Sueetie_ContentTypes SELECT 16, 'OtherAlbum', 'Other or Unknown Album Type', 1, 506, 556
END

INSERT INTO Sueetie_UserLogCategories select 302, 'Forum Answer', 'User marked a forum post as an Answer', 1, 1, 1
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

print 'INSERT INTO SuAddons_Settings'

/* -- INSERT INTO SuAddons_Settings  -------------------------------------------------------------------------------- */

INSERT INTO SuAddons_Settings select 'EnableForumAnswers','True'
GO


print 'ALTER TABLE Sueetie_gs_album'

/* -- ALTER TABLE Sueetie_gs_album -------------------------------------------------------------------------------- */

ALTER TABLE Sueetie_gs_album add SueetieAlbumPath nvarchar(1000) null
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



print 'ALTER VIEW [dbo].[Sueetie_vw_MediaAlbums]'

/* -- ALTER VIEW [dbo].[Sueetie_vw_MediaAlbums] -------------------------------------------------------------------------------- */

/****** Object:  View [dbo].[Sueetie_vw_MediaAlbums]    Script Date: 05/24/2011 20:28:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[Sueetie_vw_MediaAlbums]
AS
SELECT     dbo.gs_Album.AlbumId, dbo.gs_Album.AlbumParentId, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.SourceID, dbo.Sueetie_Content.ContentTypeID, 
                      dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_Content.UserID AS SueetieUserID, dbo.Sueetie_Content.Permalink, 
                      dbo.Sueetie_Content.DateTimeCreated, dbo.Sueetie_Content.IsRestricted, dbo.gs_Album.Title AS AlbumTitle, 
                      dbo.Sueetie_Applications.Description AS ApplicationDescription, dbo.Sueetie_Groups.GroupID, dbo.Sueetie_Groups.GroupKey, dbo.Sueetie_Groups.GroupName, 
                      dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, dbo.Sueetie_Users.DisplayName, '' AS MediaObjectUrl, dbo.gs_Gallery.GalleryId, 
                      dbo.gs_Gallery.Description AS GalleryName, dbo.Sueetie_ContentTypes.ContentTypeName, dbo.Sueetie_ContentTypes.IsAlbum, 
                      dbo.Sueetie_ContentTypes.Description AS ContentTypeDescription, dbo.Sueetie_gs_Album.SueetieAlbumID, dbo.Sueetie_ContentTypes.UserLogCategoryID, 
                      dbo.Sueetie_ContentTypes.AlbumMediaCategoryID, dbo.Sueetie_Applications.ApplicationTypeID, dbo.Sueetie_gs_Album.AlbumDescription, 
                      dbo.gs_Album.DateLastModified, dbo.ConcatMediaAlbumTags(dbo.gs_Album.AlbumId) AS Tags, dbo.Sueetie_gs_Gallery.GalleryKey, dbo.gs_Album.DirectoryName, 
                      dbo.Sueetie_gs_Album.SueetieAlbumPath
FROM         dbo.gs_Gallery INNER JOIN
                      dbo.gs_Album ON dbo.gs_Gallery.GalleryId = dbo.gs_Album.FKGalleryId INNER JOIN
                      dbo.Sueetie_gs_Album ON dbo.gs_Album.AlbumId = dbo.Sueetie_gs_Album.AlbumID INNER JOIN
                      dbo.Sueetie_Applications INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_Applications.ApplicationID = dbo.Sueetie_Content.ApplicationID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_Content.UserID = dbo.Sueetie_Users.UserID ON dbo.Sueetie_gs_Album.AlbumID = dbo.Sueetie_Content.SourceID INNER JOIN
                      dbo.Sueetie_ContentTypes ON dbo.Sueetie_gs_Album.ContentTypeID = dbo.Sueetie_ContentTypes.ContentTypeID INNER JOIN
                      dbo.Sueetie_gs_Gallery ON dbo.gs_Gallery.GalleryId = dbo.Sueetie_gs_Gallery.GalleryID

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

print 'ALTER PROCEDURE [dbo].[Sueetie_UserLogActivities_Get]'

/* -- ALTER PROCEDURE [dbo].[Sueetie_UserLogActivities_Get] -------------------------------------------------------------------------------- */


/****** Object:  StoredProcedure [dbo].[Sueetie_UserLogActivities_Get]    Script Date: 05/30/2011 15:08:45 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[Sueetie_UserLogActivities_Get]
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

