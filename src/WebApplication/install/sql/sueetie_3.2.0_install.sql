
Print '/****** Object:  Table [dbo].[Sueetie_Groups]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Groups](
	[GroupID] [int] IDENTITY(0,1) NOT NULL,
	[GroupKey] [nvarchar](25) NULL,
	[GroupName] [nvarchar](255) NOT NULL,
	[GroupAdminRole] [nvarchar](50) NULL,
	[GroupUserRole] [nvarchar](50) NULL,
	[GroupDescription] [nvarchar](1000) NULL,
	[GroupTypeID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[HasAvatar] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_Groups] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_Applications]    Script Date: 10/15/2010 13:32:33 ******/'

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

Print '/****** Object:  Table [dbo].[Sueetie_Content]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Content](
	[ContentID] [int] IDENTITY(1,1) NOT NULL,
	[SourceID] [int] NOT NULL,
	[ContentTypeID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[Permalink] [nvarchar](500) NULL,
	[DateTimeCreated] [datetime] NOT NULL,
	[IsRestricted] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_Content] PRIMARY KEY CLUSTERED 
(
	[ContentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_Users]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[MembershipID] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[DisplayName] [nvarchar](150) NOT NULL,
	[Bio] [ntext] NULL,
	[IsActive] [bit] NOT NULL,
	[IP] [nvarchar](15) NULL,
	[LastActivity] [smalldatetime] NULL,
	[InAnalytics] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_ForumTopicTags]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_ForumTopicTags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[TopicID] [int] NOT NULL,
	[Tag] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Sueetie_ForumTopicTags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ForumTopic_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[Sueetie_beComment]


CREATE PROCEDURE  [dbo].[Sueetie_ForumTopic_Get]
	@ContentTypeID int,
	@ApplicationID int,
	@TopicID int
 
AS

SET NOCOUNT ON

select * from Sueetie_vw_ForumTopics where SourceID = @TopicID
	and ContentTypeID = @ContentTypeID
	and ApplicationID = @ApplicationID
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Search_ForumMessages_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[Sueetie_Search_ForumMessages_Get] 4, '9/6/1969' 

CREATE PROCEDURE [dbo].[Sueetie_Search_ForumMessages_Get] 
(
	@contentTypeID int,
	@pubdate datetime
)
AS
SET NOCOUNT, XACT_ABORT ON


BEGIN
WITH OrderedEntries AS
(
SELECT  *, 
	--dbo.ConcatCategories(pageid) as 'categories',
	row_number() over(order by DateTimeCreated DESC) RowNumber
	from Sueetie_vw_ForumMessages where ContentTypeID = @contentTypeID 
	and DateTimeCreated >= @pubdate
)

	select * from orderedEntries 

	--	select * from OrderedEntries
	--	where RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize 

END
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ForumMessages_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_ForumMessages_Get] 
(
	@contentTypeID int,
	@userID int,
	@groupID int,
	@applicationID int,
	@numRecords int,
	@IsRestricted bit
)
AS
SET NOCOUNT, XACT_ABORT ON

	set rowcount  @numRecords
		
SELECT  * from Sueetie_vw_ForumMessages where ContentTypeID = @contentTypeID 
	and GroupID = CASE WHEN @groupID > -1 then @groupID ELSE GroupID END
	and ApplicationID = CASE WHEN @applicationID > 0 then @applicationID ELSE ApplicationID END
	and SueetieUserID = CASE WHEN @UserID > 0 then @UserID ELSE SueetieUserID END
	and IsRestricted = CASE WHEN @IsRestricted = 1 then 0 ELSE IsRestricted END
	order by DateTimeCreated desc

	
	set RowCount 0
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ForumMessage_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[Sueetie_beComment]


CREATE PROCEDURE  [dbo].[Sueetie_ForumMessage_Get]
	@ContentTypeID int,
	@ApplicationID int,
	@MessageID int
 
AS

SET NOCOUNT ON

select * from Sueetie_vw_ForumMessages where SourceID = @MessageID
	and ContentTypeID = @ContentTypeID
	and ApplicationID = @ApplicationID
GO
Print '/****** Object:  Table [dbo].[Sueetie_UserLog]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_UserLog](
	[UserLogID] [int] IDENTITY(1,1) NOT NULL,
	[UserLogCategoryID] [int] NOT NULL,
	[ItemID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[LogDateTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Sueetie_UserLog] PRIMARY KEY CLUSTERED 
(
	[UserLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_UserFollowCounts]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_UserFollowCounts](
	[UserID] [int] NOT NULL,
	[Following] [int] NOT NULL,
	[Followers] [int] NOT NULL,
	[Friends] [int] NOT NULL,
 CONSTRAINT [PK_Sueetie_UserFriendsFavorites] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_UserAvatar]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_UserAvatar](
	[UserID] [int] NOT NULL,
	[AvatarImage] [image] NULL,
	[AvatarImageType] [nvarchar](50) NULL,
 CONSTRAINT [PK_Sueetie_UserAvatar] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_UserLogCategories]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_UserLogCategories](
	[UserLogCategoryID] [int] NOT NULL,
	[UserLogCategoryCode] [nvarchar](25) NULL,
	[UserLogCategoryDescription] [nvarchar](255) NULL,
	[IsDisplayed] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[IsSyndicated] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_UserLogCategories] PRIMARY KEY CLUSTERED 
(
	[UserLogCategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_Ban]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_User_Ban]
	@UserID int,
	@Mask nvarchar(15)
AS

SET NOCOUNT ON

declare @NonLocalMask nvarchar(15)
select @NonLocalMask = '127.0.0.1'

if @NonLocalMask <> @Mask
begin

INSERT INTO [dbo].[Sueetie_UserBanned] (
	[UserID],
	[Mask],
	[BannedDateTime]
) VALUES (
	@UserID,
	@Mask,
	GETDATE()
)

end



GO
Print '/****** Object:  Table [dbo].[Sueetie_SiteLogTypes]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_SiteLogTypes](
	[SiteLogTypeID] [int] NOT NULL,
	[SiteLogTypeCode] [nvarchar](25) NOT NULL,
 CONSTRAINT [PK_Sueetie_SiteLogTypes] PRIMARY KEY CLUSTERED 
(
	[SiteLogTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_SiteLogCategories]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_SiteLogCategories](
	[SiteLogCategoryID] [int] NOT NULL,
	[SiteLogCategoryCode] [nvarchar](25) NOT NULL,
	[SiteLogCategoryDescription] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Sueetie_SiteLogCategories] PRIMARY KEY CLUSTERED 
(
	[SiteLogCategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_TaskQueue]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_TaskQueue](
	[TaskQueueID] [int] IDENTITY(1,1) NOT NULL,
	[TaskTypeID] [int] NOT NULL,
	[TaskStartDateTime] [datetime] NULL,
	[TaskEndDateTime] [datetime] NULL,
	[SuccessBit] [bit] NULL,
	[Active] [bit] NULL,
 CONSTRAINT [PK_Sueetie_TaskQueue] PRIMARY KEY CLUSTERED 
(
	[TaskQueueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_Tags]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Tags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[TagMasterID] [int] NOT NULL,
	[ContentID] [int] NOT NULL,
	[Tag] [nvarchar](100) NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[CreatedDateTime] [smalldatetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_Tags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_TagMaster]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_TagMaster](
	[TagMasterID] [int] IDENTITY(1,1) NOT NULL,
	[Tag] [nvarchar](255) NOT NULL,
	[CreatedDateTime] [smalldatetime] NULL,
	[CreatedBy] [int] NOT NULL,
	[LastUsedDateTime] [smalldatetime] NULL,
	[LastUsedBy] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_TagMaster] PRIMARY KEY CLUSTERED 
(
	[TagMasterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_WikiPageTags]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_WikiPageTags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[PageID] [int] NOT NULL,
	[Tag] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Sueetie_WikiPageTags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_WikiPages]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_WikiPages](
	[PageID] [int] IDENTITY(1,1) NOT NULL,
	[PageFileName] [nvarchar](200) NOT NULL,
	[PageTitle] [nvarchar](500) NOT NULL,
	[Keywords] [nvarchar](1000) NOT NULL,
	[Abstract] [nvarchar](max) NULL,
	[Namespace] [nvarchar](200) NULL,
	[DateTimeCreated] [smalldatetime] NULL,
	[DateTimeModified] [smalldatetime] NULL,
	[UserID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[PageContent] [ntext] NULL,
 CONSTRAINT [PK_Sueetie_WikiPages] PRIMARY KEY CLUSTERED 
(
	[PageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_WikiPageCategories]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_WikiPageCategories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[PageID] [int] NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Sueetie_WikiPageCategories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Print '/****** Object:  UserDefinedFunction [dbo].[fnSplit]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnSplit](
    @sInputList VARCHAR(8000) -- List of delimited items
  , @sDelimiter VARCHAR(8000) = ',' -- delimiter that separates items
) RETURNS @List TABLE (item VARCHAR(8000))

BEGIN
DECLARE @sItem VARCHAR(8000)
WHILE CHARINDEX(@sDelimiter,@sInputList,0) <> 0
 BEGIN
 SELECT
  @sItem=RTRIM(LTRIM(SUBSTRING(@sInputList,1,CHARINDEX(@sDelimiter,@sInputList,0)-1))),
  @sInputList=RTRIM(LTRIM(SUBSTRING(@sInputList,CHARINDEX(@sDelimiter,@sInputList,0)+LEN(@sDelimiter),LEN(@sInputList))))
 
 IF LEN(@sItem) > 0
  INSERT INTO @List SELECT @sItem
 END

IF LEN(@sInputList) > 0
 INSERT INTO @List SELECT @sInputList -- Put the last item in
RETURN
END
GO
Print '/****** Object:  Table [dbo].[be_Users]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[be_Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](255) NOT NULL,
	[LastLoginTime] [datetime] NULL,
	[EmailAddress] [nvarchar](100) NULL,
 CONSTRAINT [PK_be_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[be_UserRoles]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[be_UserRoles](
	[UserRoleID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_be_UserRoles] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[be_Roles]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[be_Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[Role] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_be_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[be_Profiles]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[be_Profiles](
	[ProfileID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NULL,
	[SettingName] [nvarchar](200) NULL,
	[SettingValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_be_Profiles] PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_Categories]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[Category] [nvarchar](255) NOT NULL,
	[IsContentCategory] [bit] NOT NULL,
	[IsGlobalCategory] [bit] NOT NULL,
	[ApplicationTypeID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_Calendars]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Calendars](
	[CalendarID] [int] IDENTITY(1,1) NOT NULL,
	[CalendarTitle] [nvarchar](255) NOT NULL,
	[CalendarDescription] [nvarchar](max) NOT NULL,
	[CalendarUrl] [nvarchar](255) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_Calendars] PRIMARY KEY CLUSTERED 
(
	[CalendarID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_CalendarEvents]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_CalendarEvents](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[EventGuid] [uniqueidentifier] NOT NULL,
	[CalendarID] [int] NOT NULL,
	[EventTitle] [nvarchar](255) NOT NULL,
	[EventDescription] [nvarchar](1000) NULL,
	[StartDateTime] [smalldatetime] NOT NULL,
	[EndDateTime] [smalldatetime] NOT NULL,
	[AllDayEvent] [bit] NOT NULL,
	[Url] [nvarchar](500) NULL,
	[RepeatEndDate] [smalldatetime] NOT NULL,
	[SourceContentID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedDateTIme] [smalldatetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
 CONSTRAINT [PK_Sueetie_CalendarEvents] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_ContentPageTags]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_ContentPageTags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[ContentPageID] [int] NOT NULL,
	[Tag] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Sueetie_ContentPageTags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_ContentPages]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_ContentPages](
	[ContentPageID] [int] IDENTITY(1,1) NOT NULL,
	[ContentPageGroupID] [int] NOT NULL,
	[PageKey] [nvarchar](50) NULL,
	[PageSlug] [nvarchar](255) NULL,
	[PageTitle] [nvarchar](255) NULL,
	[PageDescription] [nvarchar](255) NULL,
	[ReaderRoles] [nvarchar](255) NULL,
	[LastUpdateDateTime] [smalldatetime] NULL,
	[LastUpdateUserID] [int] NOT NULL,
	[IsPublished] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
 CONSTRAINT [PK_Sueetie_ContentPages] PRIMARY KEY CLUSTERED 
(
	[ContentPageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_ContentPageGroups]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_ContentPageGroups](
	[ContentPageGroupID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ContentPageGroupTitle] [nvarchar](255) NULL,
	[EditorRoles] [nvarchar](255) NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_ContentPageGroups] PRIMARY KEY CLUSTERED 
(
	[ContentPageGroupID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_BannedIPs]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_BannedIPs](
	[BannedID] [int] IDENTITY(1,1) NOT NULL,
	[Mask] [nvarchar](15) NULL,
	[BannedDateTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Sueetie_BannedIPs] PRIMARY KEY CLUSTERED 
(
	[BannedID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_ApplicationTypes]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_ApplicationTypes](
	[ApplicationTypeID] [int] NOT NULL,
	[ApplicationName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Sueetie_Apps] PRIMARY KEY CLUSTERED 
(
	[ApplicationTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_beComments]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_beComments](
	[SueetieCommentID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[PostCommentID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[PostID] [uniqueidentifier] NOT NULL,
	[CommentDate] [datetime] NOT NULL,
	[Author] [nvarchar](255) NULL,
	[Email] [nvarchar](255) NULL,
	[Website] [nvarchar](255) NULL,
	[Comment] [nvarchar](max) NULL,
	[Country] [nvarchar](255) NULL,
	[Ip] [nvarchar](50) NULL,
	[IsApproved] [bit] NULL,
	[ParentCommentID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Sueetie_beComments] PRIMARY KEY CLUSTERED 
(
	[SueetieCommentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_bePostTags]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_bePostTags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[SueetiePostID] [int] NOT NULL,
	[Tag] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Sueetie_bePostTags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_bePosts]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_bePosts](
	[SueetiePostID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[PostID] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
	[Title] [nvarchar](255) NULL,
	[Description] [nvarchar](max) NULL,
	[PostContent] [ntext] NULL,
	[DateCreated] [datetime] NULL,
	[DateModified] [datetime] NULL,
	[Author] [nvarchar](50) NULL,
	[IsPublished] [bit] NULL,
	[IsCommentEnabled] [bit] NULL,
	[Raters] [int] NULL,
	[Rating] [real] NULL,
	[Slug] [nvarchar](255) NULL,
 CONSTRAINT [PK_Sueetie_bePosts] PRIMARY KEY CLUSTERED 
(
	[SueetiePostID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_bePostCategories]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_bePostCategories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[SueetiePostID] [int] NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Sueetie_bePostCategories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_Downloads]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Downloads](
	[DownloadID] [int] IDENTITY(1,1) NOT NULL,
	[ContentID] [int] NOT NULL,
	[UserID] [int] NOT NULL,
	[DownloadDateTime] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Sueetie_Downloads] PRIMARY KEY CLUSTERED 
(
	[DownloadID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_ContentTypes]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_ContentTypes](
	[ContentTypeID] [int] NOT NULL,
	[ContentTypeName] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[IsAlbum] [bit] NOT NULL,
	[UserLogCategoryID] [int] NOT NULL,
	[AlbumMediaCategoryID] [int] NOT NULL,
 CONSTRAINT [PK_Sueetie_AppContentTypes] PRIMARY KEY CLUSTERED 
(
	[ContentTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_ContentParts]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_ContentParts](
	[ContentPartID] [int] IDENTITY(1,1) NOT NULL,
	[ContentName] [nvarchar](256) NOT NULL,
	[ContentPageID] [int] NOT NULL,
	[LastUpdateDateTime] [smalldatetime] NOT NULL,
	[LastUpdateUserID] [int] NULL,
	[ContentText] [ntext] NULL,
 CONSTRAINT [PK_Sueetie_ContentParts] PRIMARY KEY CLUSTERED 
(
	[ContentPartID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_Blogs]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Blogs](
	[BlogID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
	[BlogOwnerRole] [nvarchar](50) NULL,
	[BlogAccessRole] [nvarchar](50) NULL,
	[BlogTitle] [nvarchar](255) NOT NULL,
	[BlogDescription] [nvarchar](max) NULL,
	[MostRecentContentID] [int] NOT NULL,
	[PostCount] [int] NOT NULL,
	[CommentCount] [int] NOT NULL,
	[TrackbackCount] [int] NOT NULL,
	[IncludeInAggregateList] [bit] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[DateBlogCreated] [smalldatetime] NULL,
	[RegisteredComments] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_Blogs] PRIMARY KEY CLUSTERED 
(
	[BlogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_Favorites]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Favorites](
	[FavoriteID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[ContentID] [int] NOT NULL,
	[DateTimeCreated] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Sueetie_Favorites] PRIMARY KEY CLUSTERED 
(
	[FavoriteID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_gs_MediaObjectTags]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_gs_MediaObjectTags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[MediaObjectID] [int] NOT NULL,
	[Tag] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Sueetie_gs_MediaObjectTags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_gs_MediaObject]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_gs_MediaObject](
	[MediaObjectID] [int] NOT NULL,
	[MediaObjectDescription] [nvarchar](max) NULL,
	[InDownloadReport] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_gs_MediaObject] PRIMARY KEY CLUSTERED 
(
	[MediaObjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_gs_Categories]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_gs_Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[MediaObjectID] [int] NOT NULL,
	[Category] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Sueetie_gs_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_gs_Bibliography]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_gs_Bibliography](
	[MediaObjectID] [int] NOT NULL,
	[Abstract] [nvarchar](max) NULL,
	[Authors] [nvarchar](255) NULL,
	[Location] [nvarchar](255) NULL,
	[Series] [nvarchar](50) NULL,
	[DocumentType] [nvarchar](50) NULL,
	[Keywords] [nvarchar](500) NULL,
	[Misc] [nvarchar](255) NULL,
	[Number] [nvarchar](50) NULL,
	[Version] [nvarchar](25) NULL,
	[Organization] [nvarchar](255) NULL,
	[Conference] [nvarchar](100) NULL,
	[ISxN] [nvarchar](25) NULL,
	[PublicationDate] [nvarchar](50) NULL,
	[Publisher] [nvarchar](100) NULL,
 CONSTRAINT [PK_Sueetie_gs_Bibliography] PRIMARY KEY CLUSTERED 
(
	[MediaObjectID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_gs_AlbumTags]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_gs_AlbumTags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[AlbumID] [int] NOT NULL,
	[Tag] [nvarchar](100) NULL,
 CONSTRAINT [PK_Sueetie_gs_AlbumTags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_gs_Album]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sueetie_gs_Album](
	[SueetieAlbumID] [int] IDENTITY(1,1) NOT NULL,
	[AlbumID] [int] NOT NULL,
	[ContentTypeID] [int] NOT NULL,
	[AlbumDescription] [nvarchar](max) NULL,
	[SueetieAlbumPath] [nvarchar](1000) NULL,
 CONSTRAINT [PK_Sueetie_gs_Album] PRIMARY KEY CLUSTERED 
(
	[SueetieAlbumID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


Print '/****** Object:  Table [dbo].[Sueetie_Followers]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Followers](
	[FollowID] [int] IDENTITY(1,1) NOT NULL,
	[FollowerUserID] [int] NOT NULL,
	[FollowingUserID] [int] NOT NULL,
	[ContentIDFollowed] [int] NOT NULL,
	[DateTimeFollowed] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Sueetie_Followers] PRIMARY KEY CLUSTERED 
(
	[FollowID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  UserDefinedFunction [dbo].[Sueetie_fn_GetElement]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Sueetie_fn_GetElement]
(
@ord AS INT,
@str AS VARCHAR(8000),
@delim AS VARCHAR(1) )
RETURNS INT
AS
BEGIN
  -- If input is invalid, return null.
  IF  @str IS NULL
      OR LEN(@str) = 0
      OR @ord IS NULL
      OR @ord < 1
      -- @ord > [is the] expression that calculates the number of elements.
      OR @ord > LEN(@str) - LEN(REPLACE(@str, @delim, '')) + 1
    RETURN NULL
  DECLARE @pos AS INT, @curord AS INT
  SELECT @pos = 1, @curord = 1
  -- Find next element's start position and increment index.
  WHILE @curord < @ord
    SELECT
      @pos    = CHARINDEX(@delim, @str, @pos) + 1,
      @curord = @curord + 1
  RETURN    CAST(SUBSTRING(@str, @pos, CHARINDEX(@delim, @str + @delim, @pos) - @pos) AS INT)
END
GO
Print '/****** Object:  Table [dbo].[Sueetie_Roles]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Roles](
	[SueetieRoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleID] [uniqueidentifier] NOT NULL,
	[RoleName] [nvarchar](256) NOT NULL,
	[IsGroupAdminRole] [bit] NOT NULL,
	[IsGroupUserRole] [bit] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[IsBlogOwnerRole] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_Roles] PRIMARY KEY CLUSTERED 
(
	[SueetieRoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Print '/****** Object:  Table [dbo].[Sueetie_SiteLog]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_SiteLog](
	[SiteLogID] [int] IDENTITY(1,1) NOT NULL,
	[SiteLogTypeID] [int] NOT NULL,
	[SiteLogCategoryID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[Message] [ntext] NULL,
	[LogDateTime] [datetime] NULL,
 CONSTRAINT [PK_Sueetie_SiteLog] PRIMARY KEY CLUSTERED 
(
	[SiteLogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_Settings]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_Settings](
	[SettingName] [nvarchar](50) NOT NULL,
	[SettingValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_Sueetie_SettingsName] PRIMARY KEY CLUSTERED 
(
	[SettingName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  Table [dbo].[Sueetie_SearchIndexCounts]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_SearchIndexCounts](
	[TaskQueueID] [int] NOT NULL,
	[BlogPosts] [int] NOT NULL,
	[ForumMessages] [int] NOT NULL,
	[WikiPages] [int] NOT NULL,
	[MediaAlbums] [int] NOT NULL,
	[MediaObjects] [int] NOT NULL,
	[DocumentsIndexed] [int] NOT NULL,
	[ContentPages] [int] NOT NULL,
 CONSTRAINT [PK_Sueetie_SearchIndexCounts] PRIMARY KEY CLUSTERED 
(
	[TaskQueueID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_SiteSetting_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_SiteSetting_Update]
	@SettingName nvarchar(50),
	@SettingValue ntext
AS

SET NOCOUNT ON

delete from  [dbo].[Sueetie_Settings] where SettingName = @SettingName

insert into [dbo].[Sueetie_Settings] Values (@SettingName, @SettingValue)
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Role_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Role_Update]
	@RoleName nvarchar(256),
	@IsGroupAdminRole bit,
	@IsGroupUserRole bit,
	@IsBlogOwnerRole bit
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_Roles] SET

	[IsGroupAdminRole] = @IsGroupAdminRole,
	[IsGroupUserRole] = @IsGroupUserRole,
	[IsBlogOwnerRole] = @IsBlogOwnerRole
WHERE
	[RoleName] = @RoleName
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Role_Delete]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Role_Delete]
	@RoleName nvarchar(256)
AS

SET NOCOUNT ON

declare @wasDeleted bit
select @wasDeleted = 0

declare @islocked bit
select @islocked = islocked from Sueetie_Roles where RoleName = @RoleName

if @islocked = 0
begin
	delete from Sueetie_Roles where RoleName = @RoleName
	select @wasDeleted = 1
end

select @wasDeleted
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Search_IndexTask_End]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[Sueetie_Tasks_QueueInfo_Get] 1

CREATE PROCEDURE [dbo].[Sueetie_Search_IndexTask_End]
	@TaskQueueID int,
	@BlogPosts int,
	@ForumMessages int,
	@WikiPages int,
	@MediaAlbums int,
	@MediaObjects int,
	@ContentPages int,
	@DocumentsIndexed int
AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_SearchIndexCounts] (
	[TaskQueueID],
	[BlogPosts],
	[ForumMessages],
	[WikiPages],
	[MediaAlbums],
	[MediaObjects],
	[ContentPages],
	[DocumentsIndexed]
) VALUES (
	@TaskQueueID,
	@BlogPosts,
	@ForumMessages,
	@WikiPages,
	@MediaAlbums,
	@MediaObjects,
	@ContentPages,
	@DocumentsIndexed
)

update Sueetie_TaskQueue set TaskEndDateTime = GETDATE(), SuccessBit = 1, Active = 0 where TaskQueueID = @TaskQueueID
	
return
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaAlbum_AdminUpdate]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[Sueetie_MediaAlbums_Get] -1, -1, -1, -1, -1, 100, 0

CREATE PROCEDURE [dbo].[Sueetie_MediaAlbum_AdminUpdate] 
(
	@albumID int,
	@contentID int,
	@contentTypeID int
)
AS
SET NOCOUNT, XACT_ABORT ON

update Sueetie_gs_Album set ContentTypeID = @contentTypeID where AlbumID = @albumID
update Sueetie_Content set ContentTypeID = @contentTypeID where ContentID = @contentID
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Followers_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Followers_Get]
	@UserID int,
	@FollowType int

AS

SET NOCOUNT ON
	

SELECT     dbo.Sueetie_Followers.FollowID, dbo.Sueetie_Followers.FollowerUserID, dbo.Sueetie_Followers.FollowingUserID, dbo.Sueetie_Users.DisplayName, 0 AS 'IsAFriend', 0 as 'ThumbnailFilename'
	into #tempFollowers
FROM         dbo.Sueetie_Followers INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_Followers.FollowerUserID = dbo.Sueetie_Users.UserID
                      where FollowingUserID = @UserID 

update #tempFollowers set thumbnailFilename = UserID from 
	Sueetie_UserAvatar u inner join #tempFollowers t on u.UserID = t.FollowerUserID
	where u.AvatarImage is not null

SELECT     dbo.Sueetie_Followers.FollowID, dbo.Sueetie_Followers.FollowerUserID, dbo.Sueetie_Followers.FollowingUserID, dbo.Sueetie_Users.DisplayName, 0 AS 'IsAFriend', 0 as 'ThumbnailFilename'
	into #tempFollowing
FROM         dbo.Sueetie_Followers INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_Followers.FollowingUserID = dbo.Sueetie_Users.UserID
                      where FollowerUserID = @UserID

update #tempFollowing set thumbnailFilename = UserID from 
	Sueetie_UserAvatar u inner join #tempFollowing t on u.UserID = t.FollowingUserID
	where u.AvatarImage is not null
	
update #tempFollowing set isafriend = 1 where followinguserID in (select followeruserID from #tempfollowers)

if @FollowType = 0  -- Who the user is following 
begin
	select followingUserID as 'UserID', displayname, thumbnailFilename from #tempFollowing order by displayname
end else
if @FollowType = 1 -- Followers of user
begin
	select followerUserID as 'UserID', displayname, thumbnailFilename  from #tempFollowers order by displayname
end 
if @FollowType = 2 -- Friends
begin
	select followerUserID as 'UserID', displayname, thumbnailFilename from #tempfollowing where Isafriend = 1 order by displayname
end	

drop table #tempFollowing
drop table #tempFollowers
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ContentPageUrls_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_ContentPageUrls_Update]
	@ContentPageID int,
	@Permalink nvarchar(500)
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_ContentParts] SET
LastUpdateDateTime = GETDATE()
where ContentPageID = @ContentPageID

update Sueetie_Content set Permalink = @Permalink where SourceID in 
	(select ContentPartID from Sueetie_ContentParts cp where cp.ContentPageID = @ContentPageID)
	and ContentTypeID = 21

update Sueetie_Content set Permalink = @Permalink where SourceID = @ContentPageID
	and ContentTypeID = 20


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ContentPageTags_CreateUpdate]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_ContentPageTags_CreateUpdate]
	@ContentID int,
	@ItemID int,
	@ContentTypeID int,
	@UserID int,
	@Tags nvarchar(500)
AS


SET NOCOUNT ON

delete from Sueetie_ContentPageTags where ContentPageID = @ItemID
delete from Sueetie_Tags where ContentID = @ContentID

declare @tagMasterID int
declare @tagID int
declare @tag nvarchar(50)

declare tagCursor cursor for select item from  fnSplit(@tags,'|')

open tagCursor

fetch next from tagCursor into @tag
while @@fetch_status = 0
begin

	insert into Sueetie_ContentPageTags (ContentPageID, Tag) values (@ItemID, @tag)
	insert into Sueetie_Tags (ContentID, Tag, CreatedBy, CreatedDateTime) values (@ContentID, @tag, @UserID, GETDATE())
	select @tagID = @@IDENTITY
	
	select @tagMasterID = 0
	select @tagMasterID = tagMasterID from Sueetie_TagMaster where Lower(Tag) = Lower(@tag)
	if @tagMasterID = 0
	begin
		insert into Sueetie_TagMaster (Tag, CreatedDateTime, CreatedBy, LastUsedBy) 
			values (@tag, GETDATE(), @UserID,  -1)
			select @tagMasterID = @@IDENTITY
	end else
		begin
			update Sueetie_TagMaster set LastUsedDateTime = GETDATE(), LastUsedBy = @UserID
				where Lower(Tag) = Lower(@tag)
		end
	
	update Sueetie_Tags set TagMasterID = @tagMasterID where TagID = @tagID
	fetch next from tagCursor into @tag
	
end

close tagCursor
deallocate tagCursor

update Sueetie_ContentPages set LastUpdateDateTime = GETDATE(), LastUpdateUserID = @UserID where ContentPageID = @ItemID
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Follower_Remove]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Follower_Remove]
	@FollowerUserID int,
	@FollowingUserID int

AS

SET NOCOUNT ON

declare @testID int
select @testID = followID from Sueetie_Followers where FollowerUserID = @FollowerUserID and FollowingUserID = @FollowingUserID

if @testID is not null
begin

update Sueetie_UserFollowCounts set Friends = Friends - 1 
from Sueetie_Followers where FollowerUserID = @FollowingUserID and FollowingUserID = @FollowerUserID 
and Sueetie_UserFollowCounts.UserID in (@FollowingUserID, @FollowerUserID)

delete from Sueetie_Followers where FollowerUserID = @FollowerUserID and FollowingUserID = @FollowingUserID

update Sueetie_UserFollowCounts 
set Following = Following - 1 where UserID = @FollowerUserID 

update Sueetie_UserFollowCounts 
set Followers = Followers - 1 where UserID = @FollowingUserID


end
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Follower_GetID]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Follower_GetID]
	@FollowerUserID int,
	@FollowingUserID int,
	@FollowID int OUTPUT

AS

SET NOCOUNT ON

select @FollowID = followID from Sueetie_Followers where FollowerUserID = @FollowerUserID and FollowingUserID = @FollowingUserID

if @FollowID is null
begin

select @FollowID = -1
end

select @FollowID
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Follower_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Follower_Add]
	@FollowerUserID int,
	@FollowingUserID int,
	@ContentIDFollowed int,
	@FollowID int OUTPUT

AS

SET NOCOUNT ON

declare @testID int
select @testID = followID from Sueetie_Followers where FollowerUserID = @FollowerUserID and FollowingUserID = @FollowingUserID

SET @FollowID = SCOPE_IDENTITY() 

if @testID is null
begin


INSERT INTO [dbo].[Sueetie_Followers] (
	[FollowerUserID],
	[FollowingUserID],
	[ContentIDFollowed],
	[DateTimeFollowed]
) VALUES (
	@FollowerUserID,
	@FollowingUserID,
	@ContentIDFollowed,
	GETDATE()
)

SET @FollowID = SCOPE_IDENTITY() 

update Sueetie_UserFollowCounts
set Following = Following + 1 where UserID = @FollowerUserID 


update Sueetie_UserFollowCounts 
set Followers = Followers + 1 where UserID = @FollowingUserID

update Sueetie_UserFollowCounts set Friends = Friends + 1 
from Sueetie_Followers where FollowerUserID = @FollowingUserID and FollowingUserID = @FollowerUserID 
and Sueetie_UserFollowCounts.UserID in (@FollowingUserID, @FollowerUserID)


end
else begin

SET @FollowID = -1
end
GO
Print '/****** Object:  UserDefinedFunction [dbo].[Sueetie_fn_GetProfileElement]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[Sueetie_fn_GetProfileElement]
(
@fieldName AS NVARCHAR(100),
@fields AS NVARCHAR(4000),
@values AS NVARCHAR(4000))
RETURNS NVARCHAR(4000)
AS
BEGIN

  -- If input is invalid, return null.
  IF  @fieldName IS NULL
      OR LEN(@fieldName) = 0
      OR @fields IS NULL
      OR LEN(@fields) = 0
      OR @values IS NULL
      OR LEN(@values) = 0
    RETURN NULL

-- locate FieldName in Fields
DECLARE @fieldNameToken AS NVARCHAR(20)
DECLARE @fieldNameStart AS INTEGER, @valueStart AS INTEGER, @valueLength AS INTEGER

-- Only handle string type fields (:S:)
SET @fieldNameStart = CHARINDEX(@fieldName + ':S',@Fields,0)

-- If field is not found, return null
IF @fieldNameStart = 0 RETURN NULL
SET @fieldNameStart = @fieldNameStart + LEN(@fieldName) + 3

-- Get the field token which I've defined as the start of the field offset to the end of the length
SET @fieldNameToken = SUBSTRING(@Fields,@fieldNameStart,LEN(@Fields)-@fieldNameStart)

-- Get the values for the offset and length
SET @valueStart = dbo.sueetie_fn_getelement(1,@fieldNameToken,':')
SET @valueLength = dbo.sueetie_fn_getelement(2,@fieldNameToken,':')

-- Check for sane values, 0 length means the profile item was stored, just no data
IF @valueLength = 0 RETURN ''

-- Return the string
RETURN SUBSTRING(@values, @valueStart+1, @valueLength)
END
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaObject_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_MediaObject_Update]
	@MediaObjectID int,
	@MediaObjectDescription nvarchar(max),
	@Abstract nvarchar(max),
	@Authors nvarchar(255),
	@Location nvarchar(255),
	@Series nvarchar(50),
	@DocumentType nvarchar(50),
	@Keywords nvarchar(500),
	@Misc nvarchar(255),
	@Number nvarchar(50),
	@Version nvarchar(25),
	@Organization nvarchar(255),
	@Conference nvarchar(100),
	@ISxN nvarchar(25),
	@PublicationDate nvarchar(50),
	@Publisher nvarchar(100)
AS

SET NOCOUNT ON

UPDATE [dbo].[sueetie_gs_bibliography] SET
	[Abstract] = @Abstract,
	[Authors] = @Authors,
	[Location] = @Location,
	[Series] = @Series,
	[DocumentType] = @DocumentType,
	[Keywords] = @Keywords,
	[Misc] = @Misc,
	[Number] = @Number,
	[Version] = @Version,
	[Organization] = @Organization,
	[Conference] = @Conference,
	[ISxN] = @ISxN,
	[PublicationDate] = @PublicationDate,
	[Publisher] = @Publisher
WHERE
	[MediaObjectID] = @MediaObjectID


UPDATE [dbo].[sueetie_gs_mediaobject] SET
	[MediaObjectDescription] = @MediaObjectDescription
WHERE
	[MediaObjectID] = @MediaObjectID


GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaObject_Add]    Script Date: 01/11/2011 10:40:39 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_MediaObject_Add]
	@MediaObjectID int,
	@MediaObjectDescription nvarchar(max),
	@InDownloadReport bit
AS

SET NOCOUNT ON

declare @MediaObjectPresent int
select @MediaObjectPresent = COUNT(*) from Sueetie_gs_MediaObject where MediaObjectID = @MediaObjectID
if @MediaObjectPresent = 0
begin

	INSERT INTO [dbo].[Sueetie_gs_MediaObject] (
		[MediaObjectID],
		[MediaObjectDescription],
		[InDownloadReport]
	) VALUES (
		@MediaObjectID,
		@MediaObjectDescription,
		@InDownloadReport
	)

	INSERT INTO [dbo].[Sueetie_gs_Bibliography] ([MediaObjectID]) VALUES (@MediaObjectID)

end
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Group_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Group_Update]
	@GroupID int,
	@GroupKey nvarchar(25),
	@GroupName nvarchar(255),
	@GroupAdminRole nvarchar(50),
	@GroupUserRole nvarchar(50),
	@GroupDescription nvarchar(1000),
	@GroupTypeID int,
	@IsActive bit,
	@HasAvatar bit
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_Groups] SET
	[GroupKey] = @GroupKey,
	[GroupName] = @GroupName,
	[GroupAdminRole] = @GroupAdminRole,
	[GroupUserRole] = @GroupUserRole,
	[GroupDescription] = @GroupDescription,
	[GroupTypeID] = @GroupTypeID,
	[IsActive] = @IsActive,
	[HasAvatar] = @HasAvatar
WHERE
	[GroupID] = @GroupID


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Group_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Group_Add]
	@GroupKey nvarchar(25),
	@GroupName nvarchar(255),
	@GroupAdminRole nvarchar(50),
	@GroupUserRole nvarchar(50),
	@GroupDescription nvarchar(1000),
	@GroupTypeID int,
	@IsActive bit,
	@HasAvatar bit,
	@GroupID int OUTPUT
 
AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_Groups] (
	[GroupKey],
	[GroupName],
	[GroupAdminRole],
	[GroupUserRole],
	[GroupDescription],
	[GroupTypeID],
	[IsActive],
	[HasAvatar]
) VALUES (
	@GroupKey,
	@GroupName,
	@GroupAdminRole,
	@GroupUserRole,
	@GroupDescription,
	@GroupTypeID,
	@IsActive,
	@HasAvatar
)

SET @GroupID = SCOPE_IDENTITY() 


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Favorite_GetID]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Favorite_GetID]
	@ContentID int,
	@UserID int,
	@FavoriteID int OUTPUT
AS

SET NOCOUNT ON


declare @existingfaveID int
select @existingfaveID= favoriteID from Sueetie_Favorites where
contentid = @contentID and userid = @userid


if @existingfaveID is not null

begin

select favoriteID from Sueetie_Favorites where
	userid = @UserID and ContentID = @ContentID
	
SET @FavoriteID = SCOPE_IDENTITY() 

end
else begin

SET @FavoriteID = -1
end

select @FavoriteID

GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Favorite_Delete]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Favorite_Delete]
	@FavoriteID int
AS

SET NOCOUNT ON


CREATE TABLE [dbo].[#UserContent](
	[FavoriteID] [int] NOT NULL,
	[UserID] [int] NULL,
	[ContentTypeID] [int] NULL,
	[GroupID] [int] NULL,
	[IsRestricted] [bit] NULL
 )

insert into #UserContent select dbo.Sueetie_Favorites.FavoriteID, dbo.Sueetie_Favorites.UserID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Applications.GroupID, 
                      dbo.Sueetie_Content.IsRestricted
FROM         dbo.Sueetie_Content INNER JOIN
                      dbo.Sueetie_Favorites ON dbo.Sueetie_Content.ContentID = dbo.Sueetie_Favorites.ContentID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID
                      where favoriteID = @favoriteID
                      
delete from Sueetie_Favorites where FavoriteID = @FavoriteID

select * from #UserContent
drop table #UserContent
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Favorite_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Favorite_Add]
	@ContentID int,
	@UserID int,
	@FavoriteID int OUTPUT
AS

SET NOCOUNT ON


declare @existingfaveID int
select @existingfaveID= favoriteID from Sueetie_Favorites where
contentid = @contentID and userid = @userid


if @existingfaveID is null

begin

INSERT INTO [dbo].[Sueetie_Favorites] (
	[UserID],
	[ContentID],
	[DateTimeCreated]
) VALUES (
	@UserID,
	@ContentID,
	GETDATE()
)
SET @FavoriteID = SCOPE_IDENTITY() 

end
else begin

SET @FavoriteID = -1
end


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Downloads_Record]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Downloads_Record]
	@MediaObjectID int,
	@ApplicationID int,
	@UserID int
AS

SET NOCOUNT ON

declare @contentID int
select @contentID = contentID from Sueetie_Content where
	SourceID = @MediaObjectID and ApplicationID = @ApplicationID
	and ContentTypeID between 6 and 10
	
if @contentID > 0
begin
	
INSERT INTO [dbo].[Sueetie_Downloads] (
	[ContentID],
	[UserID],
	[DownloadDateTime]
) VALUES (
	@ContentID,
	@UserID,
	GETDATE()
)
end


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_BlogAdmin_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_BlogAdmin_Add]
	@Username nvarchar(50)
 
AS

SET NOCOUNT ON

declare @beUserID int
select @beUserID = userid from be_users where username = @Username

declare @userRoleID int
select @userRoleID = userroleID from be_userroles where UserID = @beUserID and RoleID = 1

if @userRoleID is null
begin

INSERT INTO [dbo].[be_UserRoles] (
	[UserID],
	[RoleID]
) VALUES (
	@beUserID,
	1
)

end
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Blog_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Blog_Update]
	@BlogID int,
	@CategoryID int,
	@BlogOwnerRole nvarchar(50),
	@BlogAccessRole nvarchar(50),
	@BlogTitle nvarchar(255),
	@BlogDescription nvarchar(max),
	@IncludeInAggregateList bit,
	@RegisteredComments bit,
	@IsActive bit
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_Blogs] SET
	[CategoryID] = @CategoryID,
	[BlogOwnerRole] = @BlogOwnerRole,
	[BlogAccessRole] = @BlogAccessRole,
	[BlogTitle] = @BlogTitle,
	[BlogDescription] = @BlogDescription,
	[IncludeInAggregateList] = @IncludeInAggregateList,
	[RegisteredComments] = @RegisteredComments,
	[IsActive] = @IsActive
WHERE
	[BlogID] = @BlogID
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Blog_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Blog_Get]
(
	@blogID int,
	@applicationkey nvarchar(25) = null
)
AS
SET NOCOUNT, XACT_ABORT ON

CREATE TABLE [dbo].[#z_Sueetie_Blog](
	[BlogID] [int] NULL,
	[ApplicationKey] [nvarchar](25) NULL,
	[AppDescription] [nvarchar](1000) NULL,
	[GroupID] [int] NULL,
	[ApplicationID] [int] NULL,
	[CategoryID] [int] NULL,
	[BlogOwnerRole] [nvarchar](50) NULL,
	[BlogAccessRole] [nvarchar](50) NULL,
	[BlogTitle] [nvarchar](255) NULL,
	[BlogDescription] [nvarchar](max) NULL,
	[PostCount] [int] NULL,
	[CommentCount] [int] NULL,
	[TrackbackCount] [int] NULL,
	[IncludeInAggregateList] [bit] NULL,
	[IsActive] [bit] NULL,
	[ContentID] [int] NULL,
	[PostAuthorID] [int] NULL,
	[PostTitle] [nvarchar](255) NULL,
	[PostDescription] [nvarchar](max) NULL,
	[PostContent] [ntext] NULL,
	[PostDateCreated] [datetime] NULL,
	[PostAuthorUserName] [nvarchar](50) NULL,
	[PostAuthorEmail] [nvarchar](255) NULL,
	[PostAuthorDisplayName] [nvarchar](150) NULL,
	[IsPublished] [bit] NULL,
	[Permalink] [nvarchar](500) NULL,
	[DateBlogCreated] [datetime] NULL,
	[RegisteredComments] [bit] NULL
)

if @blogID > 0
begin

insert into #z_Sueetie_Blog (BlogID, ApplicationKey, AppDescription, GroupID, ApplicationID, CategoryID, BlogOwnerRole, BlogAccessRole,
	BlogTitle, BlogDescription, PostCount, CommentCount, TrackbackCount, IncludeInAggregateList, IsActive, ContentID, PostAuthorID, IsPublished, DateBlogCreated, RegisteredComments) 
	select @blogID, a.ApplicationKey, a.[Description], a.GroupID, 
		b.applicationid, b.categoryid, b.blogownerrole, b.blogaccessrole, b.blogtitle, b.blogdescription,
		0,0,0, b.IncludeInAggregateList, b.IsActive, 0,0, 1, b.DateBlogCreated, b.RegisteredComments
	from sueetie_Blogs b inner join Sueetie_Applications a
	on b.applicationID = a.ApplicationID
	where b.blogid = @blogID
end else
if @applicationkey is not null
begin
insert into #z_Sueetie_Blog (BlogID, ApplicationKey, AppDescription, GroupID, ApplicationID, CategoryID, BlogOwnerRole, BlogAccessRole,
	BlogTitle, BlogDescription, PostCount, CommentCount, TrackbackCount, IncludeInAggregateList, IsActive, ContentID, PostAuthorID, IsPublished, DateBlogCreated, RegisteredComments) 
	select @blogID, a.ApplicationKey, a.[Description], a.GroupID, 
		b.applicationid, b.categoryid, b.blogownerrole, b.blogaccessrole, b.blogtitle, b.blogdescription,
		0,0,0, b.IncludeInAggregateList, b.IsActive, 0,0, 1, b.DateBlogCreated, b.RegisteredComments
	from sueetie_Blogs b inner join Sueetie_Applications a
	on b.applicationID = a.ApplicationID
	where a.ApplicationKey = @applicationkey
end

	select * from #z_sueetie_blog
	
	drop table #z_sueetie_blog
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ContentPart_Update]    Script Date: 10/15/2010 13:32:29 ******/'

/****** Object:  StoredProcedure [dbo].[Sueetie_ContentPart_Update]    Script Date: 05/02/2011 13:22:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_ContentPart_Update]
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


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Application_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Application_Update]
	@ApplicationID int,
	@ApplicationTypeID int,
	@ApplicationKey nvarchar(25),
	@Description nvarchar(1000),
	@GroupID int,
	@IsActive bit
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_Applications] SET
	[ApplicationTypeID] = @ApplicationTypeID,
	[ApplicationKey] = @ApplicationKey,
	[Description] = @Description,
	[GroupID] = @GroupID,
	[IsActive] = @IsActive
WHERE
	[ApplicationID] = @ApplicationID


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Application_Add]    Script Date: 10/15/2010 13:32:29 ******/'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Application_Add]
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
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_bePost_CreateUpdate]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_bePost_CreateUpdate]
	@UserID int,
	@ApplicationID int,
	@ContentTypeID int,
	@PostID uniqueidentifier,
	@Title nvarchar(255),
	@Description nvarchar(max),
	@PostContent ntext,
	@DateCreated datetime,
	@DateModified datetime,
	@Author nvarchar(50),
	@IsPublished bit,
	@IsCommentEnabled bit,
	@Raters int,
	@Rating real,
	@Slug nvarchar(255),
	@Permalink nvarchar(1000),
	@Categories nvarchar(2000),
	@Tags nvarchar(2000)
	
 
AS

SET NOCOUNT ON

declare @ExistingPostID uniqueidentifier
select @ExistingPostID = null
select @ExistingPostID = PostID from Sueetie_bePosts where PostID = @PostID


if @ExistingPostID is null
begin

INSERT INTO [dbo].[Sueetie_bePosts] (
	[UserID],
	[ApplicationID],
	[PostID],
	[Title],
	[Description],
	[PostContent],
	[DateCreated],
	[DateModified],
	[Author],
	[IsPublished],
	[IsCommentEnabled],
	[Raters],
	[Rating],
	[Slug]
) VALUES (
	@UserID,
	@ApplicationID,
	@PostID,
	@Title,
	@Description,
	@PostContent,
	@DateCreated,
	@DateModified,
	@Author,
	@IsPublished,
	@IsCommentEnabled,
	@Raters,
	@Rating,
	@Slug
)

declare @sourceID int
select @sourceID = @@IDENTITY
	delete from Sueetie_Content where SourceID = @sourceID and ContentTypeID = @ContentTypeID and ApplicationID = @ApplicationID
	
	insert into Sueetie_Content (sourceID, contenttypeid, applicationID, userID, permalink, datetimecreated) 
	values( @sourceID, @ContentTypeID, @ApplicationID, @UserID,  @permalink, GETDATE())

end else 
begin

UPDATE [dbo].[Sueetie_bePosts] SET
	[UserID] = @UserID,
	[PostID] = @PostID,
	[Title] = @Title,
	[Description] = @Description,
	[PostContent] = @PostContent,
	[DateCreated] = @DateCreated,
	[DateModified] = @DateModified,
	[Author] = @Author,
	[IsPublished] = @IsPublished,
	[IsCommentEnabled] = @IsCommentEnabled,
	[Raters] = @Raters,
	[Rating] = @Rating,
	[Slug] = @Slug
WHERE
	[PostID] = @PostID
	
		update Sueetie_Content set permalink = @permalink 
		from Sueetie_bePosts p, Sueetie_Content c
		where c.sourceid = p.SueetiePostID 
		and c.ApplicationID = @ApplicationID and c.ContentTypeID = @ContentTypeID
		and p.PostID = @PostID


end

declare @contentID int
select @ContentID = contentID from Sueetie_bePosts p, Sueetie_Content c
		where c.sourceid = p.SueetiePostID 
		and c.ApplicationID = @ApplicationID and c.ContentTypeID = @ContentTypeID
		and p.PostID = @PostID
select @ContentID	


declare @SueetiePostID int
select @SueetiePostID = SueetiePostID from Sueetie_bePosts where PostID = @PostID

delete from Sueetie_bePostCategories where SueetiePostID = @SueetiePostID
delete from Sueetie_bePostTags where SueetiePostID = @SueetiePostID

declare @categoryID int
declare @category nvarchar(50)

declare categoryCursor cursor for select item from  fnSplit(@categories,'|')

open categoryCursor

fetch next from categoryCursor into @category
while @@fetch_status = 0
begin

	insert into Sueetie_bePostCategories (SueetiePostID, Category) values (@SueetiePostID, @category)
	fetch next from categoryCursor into @category
	
end

close categoryCursor
deallocate categoryCursor


delete from Sueetie_Tags where ContentID = @ContentID

declare @tagMasterID int
declare @tagID int
declare @tag nvarchar(50)


declare tagCursor cursor for select item from  fnSplit(@tags,'|')

open tagCursor

fetch next from tagCursor into @tag
while @@fetch_status = 0
begin

	insert into Sueetie_bePostTags (SueetiePostID, Tag) values (@SueetiePostID, @tag)
	insert into Sueetie_Tags (ContentID, Tag, CreatedBy, CreatedDateTime) values (@ContentID, @tag, @UserID, GETDATE())
	select @tagID = @@IDENTITY
	
	select @tagMasterID = 0
	select @tagMasterID = tagMasterID from Sueetie_TagMaster where Lower(Tag) = Lower(@tag)
	if @tagMasterID = 0
	begin
		insert into Sueetie_TagMaster (Tag, CreatedDateTime, CreatedBy, LastUsedBy) 
			values (@tag, GETDATE(), @UserID,  -1)
			select @tagMasterID = @@IDENTITY
	end else
		begin
			update Sueetie_TagMaster set LastUsedDateTime = GETDATE(), LastUsedBy = @UserID
				where Lower(Tag) = Lower(@tag)
		end
	
	update Sueetie_Tags set TagMasterID = @tagMasterID where TagID = @tagID
	fetch next from tagCursor into @tag
	
end

close tagCursor
deallocate tagCursor
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_beComment_CreateUpdate]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_beComment_CreateUpdate]
	@UserID int,
	@ApplicationID int,
	@ContentTypeID int,
	@PostCommentID uniqueidentifier,
	@PostID uniqueidentifier,
	@CommentDate datetime,
	@Author nvarchar(255),
	@Email nvarchar(255),
	@Website nvarchar(255),
	@Comment nvarchar(max),
	@Country nvarchar(255),
	@Ip nvarchar(50),
	@IsApproved bit,
	@ParentCommentID uniqueidentifier,
	@Permalink nvarchar(1000)
	
AS

SET NOCOUNT ON

declare @ExistingCommentID uniqueidentifier
select @ExistingCommentID = null
select @ExistingCommentID = PostCommentID from Sueetie_beComments where PostCommentID = @PostCommentID

if @ExistingCommentID is null
begin

declare @slug nvarchar(255)
select @slug = slug from Sueetie_bePosts where PostID = @PostID

INSERT INTO [dbo].[Sueetie_beComments] (
	[UserID],
	[PostCommentID],
	[PostID],
	[CommentDate],
	[Author],
	[Email],
	[Website],
	[Comment],
	[Country],
	[Ip],
	[IsApproved],
	[ParentCommentID]
) VALUES (
	@UserID,
	@PostCommentID,
	@PostID,
	@CommentDate,
	@Author,
	@Email,
	@Website,
	@Comment,
	@Country,
	@Ip,
	@IsApproved,
	@ParentCommentID
)


	insert into Sueetie_Content (sourceID, contenttypeid, applicationID, userID, permalink, datetimecreated) 
	values(@@IDENTITY, @ContentTypeID, @ApplicationID, @UserID,  @Permalink, GETDATE())

end else 
begin


UPDATE [dbo].[Sueetie_beComments] SET
	[UserID] = @UserID,
	[PostCommentID] = @PostCommentID,
	[PostID] = @PostID,
	[CommentDate] = @CommentDate,
	[Author] = @Author,
	[Email] = @Email,
	[Website] = @Website,
	[Comment] = @Comment,
	[Country] = @Country,
	[Ip] = @Ip,
	[IsApproved] = @IsApproved
WHERE
	[PostCommentID] = @PostCommentID

		
end

declare @ContentID int	
select @ContentID = contentID from Sueetie_beComments p, Sueetie_Content c
		where c.sourceid = p.SueetieCommentID
		and c.ApplicationID = @ApplicationID and c.ContentTypeID = @ContentTypeID
		and p.PostCommentID = @PostCommentID	
select @ContentID
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_CloudTags_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_CloudTags_Get]
@IsRestricted bit,
@ApplicationTypeID int,
@ApplicationID int,
@CloudTagNum int
AS

SET ROWCOUNT @CloudTagNum


		SELECT    dbo.Sueetie_Tags.TagMasterID, dbo.Sueetie_Tags.Tag, COUNT(*) AS 'TagCount'
		into #temp_tags
		FROM         dbo.Sueetie_Tags INNER JOIN
							  dbo.Sueetie_Content ON dbo.Sueetie_Tags.ContentID = dbo.Sueetie_Content.ContentID INNER JOIN
							  dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID
		WHERE     dbo.Sueetie_Applications.ApplicationID = CASE WHEN @ApplicationID > 0 then @ApplicationID ELSE dbo.Sueetie_Applications.ApplicationID END
		 AND dbo.Sueetie_Applications.ApplicationTypeID = CASE WHEN @ApplicationTypeID > 0 then @ApplicationTypeID ELSE dbo.Sueetie_Applications.ApplicationTypeID END
		AND 	IsRestricted = CASE WHEN @IsRestricted = 1 then 0 ELSE IsRestricted END
		GROUP BY dbo.Sueetie_Tags.TagMasterID, dbo.Sueetie_Tags.Tag
		ORDER BY COUNT(*) DESC


select * from #temp_tags order by tag
drop table #temp_tags

SET ROWCOUNT 0
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ContentPageGroup_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_ContentPageGroup_Update]
	@ContentPageGroupID int,
	@ContentPageGroupTitle nvarchar(255),
	@EditorRoles nvarchar(255),
	@IsActive bit
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_ContentPageGroups] SET
	[ContentPageGroupTitle] = @ContentPageGroupTitle,
	[EditorRoles] = @EditorRoles,
	[IsActive] = @IsActive
WHERE
	[ContentPageGroupID] = @ContentPageGroupID


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ContentPageGroup_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_ContentPageGroup_Add]
	@ApplicationID int,
	@ContentPageGroupTitle nvarchar(255),
	@EditorRoles nvarchar(255),
	@IsActive bit
AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_ContentPageGroups] (
	[ApplicationID],
	[ContentPageGroupTitle],
	[EditorRoles],
	[IsActive]
) VALUES (
	@ApplicationID,
	@ContentPageGroupTitle,
	@EditorRoles,
	@IsActive
)


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ContentPage_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_ContentPage_Update]
	@ContentPageID int,
	@PageKey nvarchar(50),
	@PageSlug nvarchar(255),
	@PageTitle nvarchar(255),
	@PageDescription nvarchar(255),
	@ReaderRoles nvarchar(255),
	@LastUpdateUserID int,
	@IsPublished bit,
	@DisplayOrder int
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_ContentPages] SET
	[PageKey] = @PageKey,
	[PageSlug] = @PageSlug,
	[PageTitle] = @PageTitle,
	[PageDescription] = @PageDescription,
	[ReaderRoles] = @ReaderRoles,
	[LastUpdateDateTime] = GETDATE(),
	[LastUpdateUserID] = @LastUpdateUserID,
	[IsPublished] = @IsPublished,
	[DisplayOrder] = @DisplayOrder
WHERE
	[ContentPageID] = @ContentPageID


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ContentPage_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_ContentPage_Add]
	@ContentPageGroupID int,
	@PageKey nvarchar(50),
	@PageSlug nvarchar(255),
	@PageTitle nvarchar(255),
	@PageDescription nvarchar(255),
	@ReaderRoles nvarchar(255),
	@LastUpdateUserID int,
	@IsPublished bit,
	@DisplayOrder int,
	@ContentPageID int OUTPUT

AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_ContentPages] (
	[ContentPageGroupID],
	[PageKey],
	[PageSlug],
	[PageTitle],
	[PageDescription],
	[ReaderRoles],
	[LastUpdateDateTime],
	[LastUpdateUserID],
	[IsPublished],
	[DisplayOrder]
) VALUES (
	@ContentPageGroupID,
	@PageKey,
	@PageSlug,
	@PageTitle,
	@PageDescription,
	@ReaderRoles,
	getdate(),
	@LastUpdateUserID,
	@IsPublished,
	@DisplayOrder
)


SET @ContentPageID = SCOPE_IDENTITY() 

select @ContentPageID


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Content_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Content_Add]
	@SourceID int,
	@ContentTypeID int,
	@ApplicationID int,
	@UserID int,
	@Permalink nvarchar(500),
	@IsRestricted bit,
 	@ContentID int OUTPUT
AS

SET NOCOUNT ON

delete from Sueetie_Content where SourceID = @SourceID and ContentTypeID = @ContentTypeID and ApplicationID = @ApplicationID

INSERT INTO [dbo].[Sueetie_Content] (
	[SourceID],
	[ContentTypeID],
	[ApplicationID],
	[UserID],
	[Permalink],
	[DateTimeCreated],
	[IsRestricted]
) VALUES (
	@SourceID,
	@ContentTypeID,
	@ApplicationID,
	@UserID,
	@Permalink,
	GETDATE(),
	@IsRestricted
)

SET @ContentID =@@IDENTITY
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_CalendarEvent_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_CalendarEvent_Update]
	@EventGuid uniqueidentifier,
	@CalendarID int,
	@EventTitle nvarchar(255) = null,
	@EventDescription nvarchar(1000) = null,
	@StartDateTime smalldatetime,
	@EndDateTime smalldatetime,
	@AllDayEvent bit,
	@RepeatEndDate smalldatetime,
	@IsActive bit
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_CalendarEvents] SET
	[CalendarID] = @CalendarID,
	[EventTitle] = @EventTitle,
	[EventDescription] = @EventDescription	,
	[StartDateTime] = @StartDateTime,
	[EndDateTime] = @EndDateTime,
	[AllDayEvent] = @AllDayEvent,
	[RepeatEndDate] = @RepeatEndDate,
	[IsActive] = @IsActive
WHERE
	[EventGuid] = @EventGuid



GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_CalendarEvent_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_CalendarEvent_Add]
	@EventGuid uniqueidentifier,
	@CalendarID int,
	@EventTitle nvarchar(255),
	@EventDescription nvarchar(1000),
	@StartDateTime smalldatetime,
	@EndDateTime smalldatetime,
	@AllDayEvent bit,
	@Url nvarchar(500),
	@RepeatEndDate smalldatetime,
	@SourceContentID int,
	@CreatedBy int,
	@EventID int OUTPUT
 
AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_CalendarEvents] (
	[EventGuid],
	[CalendarID],
	[EventTitle],
	[EventDescription],
	[StartDateTime],
	[EndDateTime],
	[AllDayEvent],
	[Url],
	[RepeatEndDate],
	[SourceContentID],
	[IsActive],
	[CreatedDateTIme],
	[CreatedBy]
) VALUES (
	@EventGuid,
	@CalendarID,
	@EventTitle,
	@EventDescription,
	@StartDateTime,
	@EndDateTime,
	@AllDayEvent,
	@Url,
	@RepeatEndDate,
	@SourceContentID,
	1,
	Getdate(),
	@CreatedBy
)

SET @EventID = SCOPE_IDENTITY() 


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Calendar_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_Calendar_Update]
	@CalendarID int,
	@CalendarTitle nvarchar(255),
	@CalendarDescription nvarchar(max),
	@CalendarUrl nvarchar(255),
	@IsActive bit
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_Calendars] SET
	[CalendarTitle] = @CalendarTitle,
	[CalendarDescription] = @CalendarDescription,
	[CalendarUrl] = @CalendarUrl,
	[IsActive] = @IsActive
WHERE
	[CalendarID] = @CalendarID


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Calendar_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Calendar_Add]
	@CalendarTitle nvarchar(255),
	@CalendarDescription nvarchar(max),
	@CalendarUrl nvarchar(255)

AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_Calendars] (
	[CalendarTitle],
	[CalendarDescription],
	[CalendarUrl],
	[IsActive]
) VALUES (
	@CalendarTitle,
	@CalendarDescription,
	@CalendarUrl,
	1
)




GO
Print '/****** Object:  UserDefinedFunction [dbo].[ConcatContentPageTags]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConcatContentPageTags](@contentpageid int)
RETURNS VARCHAR(2000)
AS
BEGIN
	DECLARE @Output VARCHAR(2000)
	SELECT @Output = COALESCE(@Output+'|', '') + CONVERT(varchar(200), t.tag)
	FROM	dbo.[sueetie_ContentPageTags] t
		JOIN dbo.sueetie_ContentPages P
			ON t.ContentPageID = @contentpageid
			and p.ContentPageID = @contentpageid
	ORDER BY t.tag

	RETURN @Output
END
GO
Print '/****** Object:  UserDefinedFunction [dbo].[ConcatCategories]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConcatCategories](@pageid int)
RETURNS VARCHAR(8000)
AS
BEGIN
	DECLARE @Output VARCHAR(8000)
	SELECT @Output = COALESCE(@Output+'|', '') + CONVERT(varchar(200), c.category)
	FROM	dbo.[sueetie_wikipagecategories] c
		JOIN dbo.sueetie_wikipages P
			ON c.pageid = @pageid
			and p.pageid = @pageid
	ORDER BY c.category

	RETURN @Output
END
GO
Print '/****** Object:  UserDefinedFunction [dbo].[ConcatWikiPageTags]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConcatWikiPageTags](@pageid int)
RETURNS VARCHAR(2000)
AS
BEGIN
	DECLARE @Output VARCHAR(2000)
	SELECT @Output = COALESCE(@Output+'|', '') + CONVERT(varchar(200), t.tag)
	FROM	dbo.[sueetie_WikiPageTags] t
		JOIN dbo.sueetie_wikipages w
			ON t.pageID = @pageID
			and w.pageID = @pageID
	ORDER BY t.tag

	RETURN @Output
END
GO
Print '/****** Object:  UserDefinedFunction [dbo].[ConcatWikiPageCategories]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConcatWikiPageCategories](@pageid int)
RETURNS VARCHAR(2000)
AS
BEGIN
	DECLARE @Output VARCHAR(2000)
	SELECT @Output = COALESCE(@Output+'|', '') + CONVERT(varchar(200),c.category)
	FROM	dbo.[sueetie_WikiPageCategories] c
		JOIN dbo.sueetie_wikipages w
			ON c.pageID = @pageID
			and w.pageID = @pageID
	ORDER BY c.category

	RETURN @Output
END
GO
Print '/****** Object:  UserDefinedFunction [dbo].[ConcatPostTags]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConcatPostTags](@sueetiepostid int)
RETURNS VARCHAR(2000)
AS
BEGIN
	DECLARE @Output VARCHAR(2000)
	SELECT @Output = COALESCE(@Output+'|', '') + CONVERT(varchar(200), t.tag)
	FROM	dbo.[sueetie_bePostTags] t
		JOIN dbo.sueetie_bePosts P
			ON t.sueetiepostID = @sueetiepostid
			and p.sueetiePostID = @sueetiepostid
	ORDER BY t.tag

	RETURN @Output
END
GO
Print '/****** Object:  UserDefinedFunction [dbo].[ConcatPostCategories]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConcatPostCategories](@sueetiepostid int)
RETURNS VARCHAR(2000)
AS
BEGIN
	DECLARE @Output VARCHAR(2000)
	SELECT @Output = COALESCE(@Output+'|', '') + CONVERT(varchar(200), c.category)
	FROM	dbo.[sueetie_bePostCategories] c
		JOIN dbo.sueetie_bePosts P
			ON c.sueetiepostID = @sueetiepostid
			and p.sueetiePostID = @sueetiepostid
	ORDER BY c.category

	RETURN @Output
END
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_WikiPageTags_CreateUpdate]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_WikiPageTags_CreateUpdate]
	@ContentID int,
	@ItemID int,
	@ContentTypeID int,
	@UserID int,
	@Tags nvarchar(500)
AS


SET NOCOUNT ON

delete from Sueetie_WikiPageTags where PageID = @ItemID
delete from Sueetie_Tags where ContentID = @ContentID

declare @tagMasterID int
declare @tagID int
declare @tag nvarchar(50)

declare tagCursor cursor for select item from  fnSplit(@tags,'|')

open tagCursor

fetch next from tagCursor into @tag
while @@fetch_status = 0
begin

	insert into Sueetie_WikiPageTags (PageID, Tag) values (@ItemID, @tag)
	insert into Sueetie_Tags (ContentID, Tag, CreatedBy, CreatedDateTime) values (@ContentID, @tag, @UserID, GETDATE())
	select @tagID = @@IDENTITY
	
	select @tagMasterID = 0
	select @tagMasterID = tagMasterID from Sueetie_TagMaster where Lower(Tag) = Lower(@tag)
	if @tagMasterID = 0
	begin
		insert into Sueetie_TagMaster (Tag, CreatedDateTime, CreatedBy, LastUsedBy) 
			values (@tag, GETDATE(), @UserID,  -1)
			select @tagMasterID = @@IDENTITY
	end else
		begin
			update Sueetie_TagMaster set LastUsedDateTime = GETDATE(), LastUsedBy = @UserID
				where Lower(Tag) = Lower(@tag)
		end
	
	update Sueetie_Tags set TagMasterID = @tagMasterID where TagID = @tagID
	fetch next from tagCursor into @tag
	
end

close tagCursor
deallocate tagCursor

update Sueetie_WikiPages set DateTimeModified = GETDATE(), UserID = @UserID where PageID = @ItemID
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_WikiPage_Update]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_WikiPage_Update]
	@PageID int,
	@PageTitle nvarchar(500),
	@Keywords nvarchar(1000),
	@Abstract nvarchar(max),
	@UserID int,
	@Categories nvarchar(2000),
	@PageContent ntext
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_WikiPages] SET
	[PageTitle] = @PageTitle,
	[Keywords] = @Keywords,
	[Abstract] = @Abstract,
	[DateTimeModified] = GETDATE(),
	[UserID] = @UserID,
	[PageContent] = @PageContent
WHERE
	[PageID] = @PageID


declare @categoryID int
declare @category nvarchar(255)

declare categoryCursor cursor for select item from  fnSplit(@categories,'|')

open categoryCursor

fetch next from categoryCursor into @category
while @@fetch_status = 0
begin
	select @categoryID = 0
	select @categoryID = categoryID from Sueetie_WikiPageCategories where PageID = @pageID and Category = @category
	if @categoryID = 0
	begin
		insert into Sueetie_WikiPageCategories (PageID, Category) values (@pageID, @category)
	end

	fetch next from categoryCursor into @category
	
end

close categoryCursor
deallocate categoryCursor

delete from Sueetie_WikiPageCategories where PageID = @pageID and Category not in (select item from fnSplit(@categories,'|'))
--select * from Sueetie_WikiPageCategories



select @PageID
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_WikiPage_Add]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_WikiPage_Add]
	@PageFileName nvarchar(200),
	@PageTitle nvarchar(500),
	@Keywords nvarchar(1000),
	@Abstract nvarchar(max),
	@Namespace nvarchar(200),
	@UserID int,
	@ApplicationID int,
	@Categories nvarchar(2000),
	@PageContent ntext,
	@PageID int OUTPUT
 
AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_WikiPages] (
	[PageFileName],
	[PageTitle],
	[Keywords],
	[Abstract],
	[Namespace],
	[DateTimeCreated],	
	[DateTimeModified],
	[UserID],
	[ApplicationID],
	[PageContent]
) VALUES (
	@PageFileName,
	@PageTitle,
	@Keywords,
	@Abstract,
	@Namespace,
	GETDATE(),	
	GETDATE(),
	@UserID,
	@ApplicationID,
	@PageContent
)

SET @PageID = SCOPE_IDENTITY() 

declare @categoryID int
declare @category nvarchar(255)

declare categoryCursor cursor for select item from  fnSplit(@categories,'|')

open categoryCursor

fetch next from categoryCursor into @category
while @@fetch_status = 0
begin
	select @categoryID = 0
	select @categoryID = categoryID from Sueetie_WikiPageCategories where PageID = @pageID and Category = @category
	if @categoryID = 0
	begin
		insert into Sueetie_WikiPageCategories (PageID, Category) values (@pageID, @category)
	end

	fetch next from categoryCursor into @category
	
end

close categoryCursor
deallocate categoryCursor

delete from Sueetie_WikiPageCategories where PageID = @pageID and Category not in (select item from fnSplit(@categories,'|'))
--select * from Sueetie_WikiPageCategories



select @PageID
GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_Create]    Script Date: 12/27/2010 11:01:54 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE procedure [dbo].[Sueetie_User_Create]
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
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[mp_members]'))
begin
	insert into mp_Members (Id,AspNetUsername, AspNetApplicationName, DateCreated) values (@id, @UserName, 'Sueetie', GETDATE())
end

select @id
END

-- Insert into GSP 2.4.4+ User Gallery Profile table
insert into gs_UserGalleryProfile select @UserName,1,'ShowMediaObjectMetadata','False'
insert into gs_UserGalleryProfile select @UserName,1,'EnableUserAlbum','True'
insert into gs_UserGalleryProfile select @UserName,1,'UserAlbumId','0'

GO


CREATE PROCEDURE [dbo].[Sueetie_UserLogCategory_Update]
	@UserLogCategoryID int,
	@IsDisplayed bit,
	@IsSyndicated bit
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_UserLogCategories] SET
	[IsDisplayed] = @IsDisplayed,
	[IsSyndicated] = @IsSyndicated
WHERE
	[UserLogCategoryID] = @UserLogCategoryID


GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_UserLogCategory_Add]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_UserLogCategory_Add]
	@UserLogCategoryID int,
	@UserLogCategoryCode nvarchar(25),
	@UserLogCategoryDescription nvarchar(255),
	@IsDisplayed bit,
	@IsSyndicated bit
AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_UserLogCategories] (
	[UserLogCategoryID],
	[UserLogCategoryCode],
	[UserLogCategoryDescription],
	[IsDisplayed],
	[IsSyndicated]
) VALUES (
	@UserLogCategoryID,
	@UserLogCategoryCode,
	@UserLogCategoryDescription,
	@IsDisplayed,
	@IsSyndicated
)

GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_SiteLog_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_SiteLog_Get]
	@SiteLogTypeID int,
	@SiteLogCategoryID int,
	@ApplicationID int

AS

SET NOCOUNT ON

SELECT     l.SiteLogID, l.SiteLogTypeID, t.SiteLogTypeCode, l.SiteLogCategoryID, l.ApplicationID, 
                      l.[Message], l.LogDateTime, a.ApplicationKey, 
                     c.SiteLogCategoryCode
FROM         dbo.Sueetie_SiteLog  l INNER JOIN
                      dbo.Sueetie_SiteLogCategories c ON l.SiteLogCategoryID = c.SiteLogCategoryID INNER JOIN
                      dbo.Sueetie_SiteLogTypes t ON l.SiteLogTypeID =t.SiteLogTypeID  INNER JOIN
                      dbo.Sueetie_Applications a ON l.ApplicationID = a.ApplicationID
		where 
	l.SiteLogTypeID = CASE WHEN @SiteLogTypeID > -1 then @SiteLogTypeID ELSE l.SiteLogTypeID END
	and l.SiteLogCategoryID = CASE WHEN @SiteLogCategoryID > -1 then @SiteLogCategoryID ELSE l.SiteLogCategoryID END
	and l.ApplicationID = CASE WHEN @ApplicationID > 0 then @ApplicationID ELSE l.ApplicationID END
	order by LogDateTime desc
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_SiteLog_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_SiteLog_Add]
	@SiteLogTypeID int,
	@SiteLogCategoryID int,
	@ApplicationID int,
	@Message ntext

AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_SiteLog] (
	[SiteLogTypeID],
	[SiteLogCategoryID],
	[ApplicationID],
	[Message],
	[LogDateTime]
) VALUES (
	@SiteLogTypeID,
	@SiteLogCategoryID,
	@ApplicationID,
	@Message,
	GETDATE()
)




GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Tasks_QueueInfo_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_Tasks_QueueInfo_Get]
(
	@TaskTypeID int
)

AS
	SET NOCOUNT ON

CREATE TABLE [dbo].[#z_Sueetie_TaskQueueInfo](
	[TaskQueueID] [int] NULL,
	[TaskStartDateTime] [datetime] NULL
)

	update Sueetie_TaskQueue set active = 0 where TaskTypeID = @TaskTypeID
	
	declare @taskQueueID int
	declare @taskStartDateTime datetime
	
	select @taskStartDateTime = max(TaskStartDateTime) from Sueetie_TaskQueue where TaskTypeID = @TaskTypeID and SuccessBit = 1

	insert into Sueetie_TaskQueue (TaskTypeID, TaskStartDateTime, SuccessBit, Active) values (@TaskTypeID, GETDATE(), 0, 1)
	
	insert into #z_Sueetie_TaskQueueInfo values (@@IDENTITY, @taskStartDateTime)
	select * from #z_Sueetie_TaskQueueInfo
	
	drop table #z_sueetie_TaskQueueInfo
	
return
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_SaveAvatar]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[Sueetie_User_SaveAvatar]
(
	@UserID int,
	@AvatarImage image = NULL,
	@AvatarImageType nvarchar(50) = NULL
)
AS
BEGIN

		UPDATE
			[dbo].[Sueetie_UserAvatar]
		SET
			AvatarImage = @AvatarImage,
			AvatarImageType = @AvatarImageType
		WHERE
			UserID = @UserID
	END
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_user_IsNewUsername]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_user_IsNewUsername]
	@username nvarchar(50)
 
AS

SET NOCOUNT ON

declare @isNewUsername bit
select @isNewUsername = 1

declare @userid int

select @userid = userid from Sueetie_Users where username =@username

if @userid > 0
begin
	select @isNewUsername = 0
end

select @isNewUsername
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_user_IsNewEmailAddress]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_user_IsNewEmailAddress]
	@Email nvarchar(50)
 
AS

SET NOCOUNT ON

declare @isNewEmailAddress bit
select @isNewEmailAddress = 1

declare @userid int

select @userid = userid from Sueetie_Users where Email  =@Email

if @userid > 0
begin
	select @isNewEmailAddress = 0
end

select @isNewEmailAddress
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_UserLog_Add]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_UserLog_Add]
	@UserLogCategoryID int,
	@ItemID int,
	@UserID int
AS

SET NOCOUNT ON

declare @userlogID int
declare @duplicateBlogPost bit

select @duplicateBlogPost = 0

-- Testing for republishing a blog post by ContentID and UserLogCategoryID to prevent duplication

if @UserLogCategoryID = 200
begin
	select @userlogID = userlogID from Sueetie_UserLog where ItemID = @ItemID and UserLogCategoryID = 200
	if @userlogID > 0  
	begin
		select @duplicateBlogPost = 1
	end
end	

if @duplicateBlogPost = 0
begin
	INSERT INTO [dbo].[Sueetie_UserLog] (
		[UserLogCategoryID],
		[ItemID],
		[UserID],
		[LogDateTime]
	) VALUES (
		@UserLogCategoryID,
		@ItemID,
		@UserID,
		GETDATE()
	)
end
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_UpdateBio]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_User_UpdateBio]
(
	@UserID int,
	@Bio ntext
)
AS
BEGIN

	update sueetie_users set Bio = @Bio where UserID = @UserID

END
GO
Print '/****** Object:  View [dbo].[Sueetie_vw_Applications]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_Applications]
AS
SELECT     dbo.Sueetie_Applications.ApplicationID, dbo.Sueetie_Applications.ApplicationTypeID, dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_Applications.Description, 
                      dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Applications.IsActive, dbo.Sueetie_Groups.GroupKey, dbo.Sueetie_ApplicationTypes.ApplicationName, 
                      dbo.Sueetie_Applications.IsLocked
FROM         dbo.Sueetie_Applications INNER JOIN
                      dbo.Sueetie_ApplicationTypes ON dbo.Sueetie_Applications.ApplicationTypeID = dbo.Sueetie_ApplicationTypes.ApplicationTypeID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID
GO

Print '/****** Object:  View [dbo].[Sueetie_vw_BlogComments]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_BlogComments]
AS
SELECT     dbo.Sueetie_beComments.SueetieCommentID, dbo.Sueetie_beComments.UserID, dbo.Sueetie_beComments.PostCommentID, dbo.Sueetie_beComments.PostID, 
                      dbo.Sueetie_beComments.CommentDate, dbo.Sueetie_beComments.Author, dbo.Sueetie_Users.Email, dbo.Sueetie_beComments.Website, 
                      dbo.Sueetie_beComments.Comment, dbo.Sueetie_beComments.Country, dbo.Sueetie_beComments.Ip, dbo.Sueetie_beComments.IsApproved, 
                      dbo.Sueetie_beComments.ParentCommentID, dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Groups.GroupKey, 
                      dbo.Sueetie_Users.DisplayName, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Content.Permalink, 
                      dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Content.DateTimeCreated, dbo.Sueetie_Content.IsRestricted, dbo.Sueetie_bePosts.Title, 
                      Sueetie_Users_1.UserID AS PostUserID, Sueetie_Users_1.DisplayName AS PostDisplayName
FROM         dbo.Sueetie_Users INNER JOIN
                      dbo.Sueetie_beComments ON dbo.Sueetie_Users.UserID = dbo.Sueetie_beComments.UserID INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_beComments.SueetieCommentID = dbo.Sueetie_Content.SourceID INNER JOIN
                      dbo.Sueetie_Applications INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID ON 
                      dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_bePosts ON dbo.Sueetie_beComments.PostID = dbo.Sueetie_bePosts.PostID INNER JOIN
                      dbo.Sueetie_Users AS Sueetie_Users_1 ON dbo.Sueetie_bePosts.UserID = Sueetie_Users_1.UserID
GO

Print '/****** Object:  View [dbo].[Sueetie_vw_FaveBlogPosts]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_FaveBlogPosts]
AS
SELECT     dbo.Sueetie_Favorites.FavoriteID, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Favorites.UserID, dbo.Sueetie_Users.UserID AS AuthorUserID, 
                      dbo.Sueetie_Users.DisplayName, dbo.Sueetie_bePosts.Title, dbo.Sueetie_bePosts.Description, dbo.Sueetie_Content.Permalink, dbo.Sueetie_Content.ApplicationID, 
                      dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Groups.GroupName, dbo.Sueetie_Content.IsRestricted, 
                      dbo.Sueetie_Content.DateTimeCreated, dbo.Sueetie_Applications.Description AS ApplicationName, dbo.Sueetie_ContentTypes.Description AS ContentType
FROM         dbo.Sueetie_Favorites INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_Favorites.ContentID = dbo.Sueetie_Content.ContentID INNER JOIN
                      dbo.Sueetie_bePosts ON dbo.Sueetie_Content.SourceID = dbo.Sueetie_bePosts.SueetiePostID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_Content.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_ContentTypes ON dbo.Sueetie_Content.ContentTypeID = dbo.Sueetie_ContentTypes.ContentTypeID
GO

Print '/****** Object:  View [dbo].[Sueetie_vw_FaveBlogComments]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_FaveBlogComments]
AS
SELECT     dbo.Sueetie_Favorites.FavoriteID, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Favorites.UserID, dbo.Sueetie_Users.UserID AS AuthorUserID, 
                      dbo.Sueetie_Users.DisplayName, dbo.Sueetie_bePosts.Title, dbo.Sueetie_beComments.Comment AS Description, dbo.Sueetie_Content.Permalink, 
                      dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Groups.GroupName, 
                      dbo.Sueetie_Content.IsRestricted, dbo.Sueetie_Content.DateTimeCreated, dbo.Sueetie_Applications.Description AS ApplicationName, 
                      dbo.Sueetie_ContentTypes.Description AS ContentType
FROM         dbo.Sueetie_Favorites INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_Favorites.ContentID = dbo.Sueetie_Content.ContentID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_Content.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_ContentTypes ON dbo.Sueetie_Content.ContentTypeID = dbo.Sueetie_ContentTypes.ContentTypeID INNER JOIN
                      dbo.Sueetie_beComments ON dbo.Sueetie_Content.SourceID = dbo.Sueetie_beComments.SueetieCommentID INNER JOIN
                      dbo.Sueetie_bePosts ON dbo.Sueetie_beComments.PostID = dbo.Sueetie_bePosts.PostID
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Favorites_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Favorites_Get]
	@UserID int
AS

SET NOCOUNT ON


--declare @contentType nvarchar(50)
--select @contentType = ContentTypeName from Sueetie_ContentTypes where ContentTypeID = @ContentTypeID


	select * from Sueetie_vw_FaveBlogPosts where userid = @UserID and ContentTypeID = 1 union
	select * from Sueetie_vw_FaveBlogComments where userid = @UserID and ContentTypeID = 2 union
	select * from Sueetie_vw_FaveForumTopics where userid = @UserID and ContentTypeID = 3  union
	select * from Sueetie_vw_FaveForumMessages where userid = @UserID and ContentTypeID = 4
GO
Print '/****** Object:  View [dbo].[Sueetie_vw_ContentParts]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_ContentParts]
AS
SELECT     dbo.Sueetie_ContentParts.ContentPartID, dbo.Sueetie_ContentParts.ContentName, dbo.Sueetie_ContentPages.PageKey, dbo.Sueetie_ContentParts.ContentPageID, 
                      dbo.Sueetie_ContentPages.ContentPageGroupID, dbo.Sueetie_ContentPages.PageTitle, dbo.Sueetie_ContentParts.LastUpdateDateTime, 
                      dbo.Sueetie_ContentParts.LastUpdateUserID, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, dbo.Sueetie_Users.DisplayName, 
                      dbo.Sueetie_ContentParts.ContentText, dbo.Sueetie_ContentPages.PageSlug, REPLICATE(' ', 10) AS Permalink, 
                      dbo.Sueetie_ContentPageGroups.ContentPageGroupTitle, dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_ContentPageGroups.ApplicationID
FROM         dbo.Sueetie_ContentParts INNER JOIN
                      dbo.Sueetie_ContentPages ON dbo.Sueetie_ContentParts.ContentPageID = dbo.Sueetie_ContentPages.ContentPageID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_ContentParts.LastUpdateUserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_ContentPageGroups ON dbo.Sueetie_ContentPages.ContentPageGroupID = dbo.Sueetie_ContentPageGroups.ContentPageGroupID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_ContentPageGroups.ApplicationID = dbo.Sueetie_Applications.ApplicationID
GO
Print '/****** Object:  View [dbo].[Sueetie_vw_ContentPageParts]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_ContentPageParts]
AS
SELECT     dbo.Sueetie_ContentParts.ContentPartID, dbo.Sueetie_ContentParts.ContentName, dbo.Sueetie_ContentPages.PageKey, dbo.Sueetie_ContentParts.ContentPageID, 
                      dbo.Sueetie_ContentPages.ContentPageGroupID, dbo.Sueetie_ContentPages.PageTitle, dbo.Sueetie_ContentParts.LastUpdateDateTime, 
                      dbo.Sueetie_ContentParts.LastUpdateUserID, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, dbo.Sueetie_Users.DisplayName, 
                      dbo.Sueetie_ContentParts.ContentText, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Content.Permalink, 
                      dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_ContentPages.PageSlug, dbo.Sueetie_ContentPageGroups.ContentPageGroupTitle
FROM         dbo.Sueetie_ContentParts INNER JOIN
                      dbo.Sueetie_ContentPages ON dbo.Sueetie_ContentParts.ContentPageID = dbo.Sueetie_ContentPages.ContentPageID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_ContentParts.LastUpdateUserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_ContentPageGroups ON dbo.Sueetie_ContentPages.ContentPageGroupID = dbo.Sueetie_ContentPageGroups.ContentPageGroupID INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_ContentParts.ContentPartID = dbo.Sueetie_Content.SourceID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_ContentPageGroups.ApplicationID = dbo.Sueetie_Applications.ApplicationID
GO
Print '/****** Object:  View [dbo].[Sueetie_vw_ContentPageGroups]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_ContentPageGroups]
AS
SELECT     dbo.Sueetie_ContentPageGroups.ContentPageGroupID, dbo.Sueetie_ContentPageGroups.ApplicationID, dbo.Sueetie_Applications.ApplicationKey, 
                      dbo.Sueetie_ContentPageGroups.ContentPageGroupTitle, dbo.Sueetie_ContentPageGroups.EditorRoles, dbo.Sueetie_ContentPageGroups.IsActive
FROM         dbo.Sueetie_Applications INNER JOIN
                      dbo.Sueetie_ContentPageGroups ON dbo.Sueetie_Applications.ApplicationID = dbo.Sueetie_ContentPageGroups.ApplicationID
GO
Print '/****** Object:  View [dbo].[Sueetie_vw_CalendarEvents]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_CalendarEvents]
AS
SELECT     dbo.Sueetie_CalendarEvents.EventID, dbo.Sueetie_CalendarEvents.EventGuid, dbo.Sueetie_CalendarEvents.CalendarID, dbo.Sueetie_CalendarEvents.EventTitle, 
                      dbo.Sueetie_CalendarEvents.EventDescription, dbo.Sueetie_CalendarEvents.StartDateTime, dbo.Sueetie_CalendarEvents.EndDateTime, 
                      dbo.Sueetie_CalendarEvents.AllDayEvent, dbo.Sueetie_CalendarEvents.RepeatEndDate, dbo.Sueetie_CalendarEvents.IsActive, 
                      dbo.Sueetie_CalendarEvents.SourceContentID, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Content.UserID, 
                      dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.DisplayName, dbo.Sueetie_CalendarEvents.CreatedDateTIme, dbo.Sueetie_CalendarEvents.CreatedBy, 
                      dbo.Sueetie_CalendarEvents.Url, dbo.Sueetie_Calendars.CalendarTitle, dbo.Sueetie_Calendars.CalendarDescription, dbo.Sueetie_Calendars.CalendarUrl
FROM         dbo.Sueetie_CalendarEvents INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_CalendarEvents.EventID = dbo.Sueetie_Content.SourceID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_Content.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_Calendars ON dbo.Sueetie_CalendarEvents.CalendarID = dbo.Sueetie_Calendars.CalendarID
GO


Print '/****** Object:  View [dbo].[Sueetie_vw_BlogPosts]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_BlogPosts]
AS
SELECT     dbo.Sueetie_bePosts.SueetiePostID, dbo.Sueetie_bePosts.UserID, dbo.Sueetie_bePosts.PostID, dbo.Sueetie_bePosts.Title, dbo.Sueetie_bePosts.Description, 
                      dbo.Sueetie_bePosts.PostContent, dbo.Sueetie_bePosts.DateCreated, dbo.Sueetie_bePosts.DateModified, dbo.Sueetie_bePosts.Author, 
                      dbo.Sueetie_bePosts.IsPublished, dbo.Sueetie_bePosts.IsCommentEnabled, dbo.Sueetie_bePosts.Raters, dbo.Sueetie_bePosts.Rating, dbo.Sueetie_bePosts.Slug, 
                      dbo.Sueetie_Users.Email, dbo.Sueetie_Users.DisplayName, dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Groups.GroupKey, dbo.Sueetie_bePosts.ApplicationID, 
                      dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_Applications.ApplicationTypeID, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.ContentTypeID, 
                      dbo.Sueetie_Content.Permalink, dbo.Sueetie_Content.IsRestricted, dbo.Sueetie_Groups.GroupName, dbo.Sueetie_Applications.Description AS ApplicationName, 
                      dbo.Sueetie_Blogs.BlogAccessRole, dbo.Sueetie_Blogs.BlogTitle, dbo.Sueetie_Blogs.IncludeInAggregateList, dbo.Sueetie_Blogs.IsActive, 
                      dbo.ConcatPostCategories(dbo.Sueetie_bePosts.SueetiePostID) AS Categories, dbo.ConcatPostTags(dbo.Sueetie_bePosts.SueetiePostID) AS Tags
FROM         dbo.Sueetie_bePosts INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_bePosts.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_bePosts.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_bePosts.SueetiePostID = dbo.Sueetie_Content.SourceID INNER JOIN
                      dbo.Sueetie_Blogs ON dbo.Sueetie_bePosts.ApplicationID = dbo.Sueetie_Blogs.ApplicationID
GO

Print '/****** Object:  View [dbo].[Sueetie_vw_ContentPages]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_ContentPages]
AS
SELECT     dbo.Sueetie_ContentPages.ContentPageID, dbo.Sueetie_ContentPageGroups.ContentPageGroupID, dbo.Sueetie_ContentPages.PageSlug, 
                      dbo.Sueetie_ContentPages.PageTitle, dbo.Sueetie_ContentPages.PageDescription, dbo.Sueetie_ContentPages.ReaderRoles, 
                      dbo.Sueetie_ContentPages.LastUpdateDateTime, dbo.Sueetie_ContentPages.LastUpdateUserID, dbo.Sueetie_ContentPages.PageKey, 
                      dbo.Sueetie_ContentPageGroups.EditorRoles, dbo.Sueetie_ContentPages.IsPublished, dbo.Sueetie_ContentPages.DisplayOrder, dbo.Sueetie_Content.ContentTypeID, 
                      dbo.Sueetie_Content.Permalink, dbo.Sueetie_Content.DateTimeCreated, dbo.Sueetie_Content.IsRestricted, dbo.Sueetie_Content.ContentID, 
                      dbo.Sueetie_ContentPageGroups.ApplicationID, dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_ContentPageGroups.ContentPageGroupTitle, 
                      dbo.ConcatContentPageTags(dbo.Sueetie_ContentPages.ContentPageID) AS Tags, REPLICATE(' ', 10) AS SearchBody, 
                      dbo.Sueetie_Applications.Description AS ApplicationName, dbo.Sueetie_Applications.ApplicationTypeID, dbo.Sueetie_Users.UserName, 
                      dbo.Sueetie_Users.DisplayName
FROM         dbo.Sueetie_ContentPages INNER JOIN
                      dbo.Sueetie_ContentPageGroups ON dbo.Sueetie_ContentPages.ContentPageGroupID = dbo.Sueetie_ContentPageGroups.ContentPageGroupID INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_ContentPages.ContentPageID = dbo.Sueetie_Content.SourceID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_ContentPageGroups.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_ContentPages.LastUpdateUserID = dbo.Sueetie_Users.UserID
GO

Print '/****** Object:  View [dbo].[Sueetie_vw_user]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_user]
AS
SELECT     dbo.Sueetie_Users.UserID, dbo.Sueetie_Users.MembershipID, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, dbo.Sueetie_UserAvatar.AvatarImage, 
                      dbo.Sueetie_UserAvatar.AvatarImageType, dbo.Sueetie_Users.DisplayName
FROM         dbo.Sueetie_Users INNER JOIN
                      dbo.Sueetie_UserAvatar ON dbo.Sueetie_Users.UserID = dbo.Sueetie_UserAvatar.UserID INNER JOIN
                      dbo.aspnet_Users ON dbo.Sueetie_Users.MembershipID = dbo.aspnet_Users.UserId
GO
Print '/****** Object:  View [dbo].[Sueetie_vw_WikiPages]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_WikiPages]
AS
SELECT     dbo.Sueetie_WikiPages.PageID, dbo.Sueetie_WikiPages.PageFileName, dbo.Sueetie_WikiPages.PageTitle, dbo.Sueetie_WikiPages.Keywords, 
                      dbo.Sueetie_WikiPages.Abstract, dbo.Sueetie_WikiPages.Namespace, dbo.Sueetie_WikiPages.DateTimeCreated, dbo.Sueetie_WikiPages.DateTimeModified, 
                      dbo.Sueetie_WikiPages.UserID, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.Permalink, dbo.Sueetie_Content.IsRestricted, dbo.Sueetie_Users.Email, 
                      dbo.Sueetie_Users.DisplayName, dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_Applications.Description AS ApplicationName, 
                      dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Groups.GroupKey, dbo.Sueetie_Groups.GroupName, dbo.Sueetie_Content.ContentTypeID, 
                      dbo.Sueetie_WikiPages.Active, dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_WikiPages.PageContent, dbo.Sueetie_Applications.ApplicationTypeID, 
                      dbo.Sueetie_Users.UserName, dbo.ConcatWikiPageTags(dbo.Sueetie_WikiPages.PageID) AS Tags, dbo.ConcatWikiPageCategories(dbo.Sueetie_WikiPages.PageID) 
                      AS Categories
FROM         dbo.Sueetie_WikiPages INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_WikiPages.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_WikiPages.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_WikiPages.PageID = dbo.Sueetie_Content.SourceID
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Blogs_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Blogs_Get] 
(
	@numRecords int,
	@categoryID int,
	@groupID int,
	@IsRestricted bit
)
AS
SET NOCOUNT, XACT_ABORT ON

	set rowcount  @numRecords
		
SELECT * from Sueetie_vw_Blogs where 
	isactive = 1 and includeinaggregateList = 1
	and CategoryID = CASE WHEN @categoryID > -1 then @categoryID ELSE CategoryID END
	and GroupID = CASE WHEN @groupID > -1 then @groupID ELSE GroupID END
	order by DateBlogCreated desc


	set RowCount 0
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Applications_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Applications_Get] 
AS
SET NOCOUNT, XACT_ABORT ON


CREATE TABLE #z_sueetie_vw_applications(
	[ApplicationID] [int] NOT NULL,
	[ApplicationTypeID] [int] NOT NULL,
	[ApplicationKey] [nvarchar](25) NOT NULL,
	[Description] [nvarchar](1000) NOT NULL,
	[GroupID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[GroupKey] [nvarchar](25) NULL,
	[ApplicationName] [nvarchar](50) NOT NULL,
	[IsLocked] [bit] NULL,
	[IsGroup] [bit] NULL	
)

	

insert into #z_sueetie_vw_applications select *, 0 from Sueetie_vw_Applications
update #z_sueetie_vw_applications set IsGroup = 1 where GroupKey is not null

select * from #z_sueetie_vw_applications
drop table #z_sueetie_vw_applications
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ContentPart_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_ContentPart_Get] 
(
	@ContentName nvarchar(255)
)
AS
SET NOCOUNT, XACT_ABORT ON
	
SELECT   * from [dbo].[Sueetie_vw_ContentParts] where ContentName = @ContentName
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_BlogComments_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_BlogComments_Get] 
(
	@contentTypeID int,
	@userID int,
	@groupID int,
	@applicationID int,
	@numRecords int,
	@IsRestricted bit
)
AS
SET NOCOUNT, XACT_ABORT ON

	set rowcount  @numRecords
		
SELECT  * from Sueetie_vw_BlogComments where ContentTypeID = @contentTypeID 
	and GroupID = CASE WHEN @groupID > -1 then @groupID ELSE GroupID END
	and ApplicationID = CASE WHEN @applicationID > 0 then @applicationID ELSE ApplicationID END
	and UserID = CASE WHEN @UserID > 0 then @UserID ELSE UserID END
	and IsRestricted = CASE WHEN @IsRestricted = 1 then 0 ELSE IsRestricted END
	order by DateTimeCreated desc

	
	set RowCount 0
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_BlogComment_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_BlogComment_Get] 
(
	@postGuid nvarchar(60),
	@contentTypeID int
)
AS
SET NOCOUNT, XACT_ABORT ON

declare @applicationID int
select @applicationID = applicationID from Sueetie_beComments c inner join Sueetie_bePosts p
	on c.PostID = p.PostID and c.PostCommentID = @postGuid

	
SELECT   * from Sueetie_vw_BlogComments where PostCommentID = @postGuid 
	and ApplicationID = @applicationID and ContentTypeID = @contentTypeID
GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Search_ContentPages_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Search_ContentPages_Get] 
(
	@contentTypeID int,
	@pubdate datetime
)
AS
SET NOCOUNT, XACT_ABORT ON


BEGIN

CREATE TABLE #z_sueetie_vw_ContentPages(
	[ContentPageID] [int] NOT NULL,
	[ContentPageGroupID] [int] NOT NULL,
	[PageSlug] [nvarchar](255) NULL,
	[PageTitle] [nvarchar](255) NULL,
	[PageDescription] [nvarchar](255) NULL,
	[ReaderRoles] [nvarchar](255) NULL,
	[LastUpdateDateTime] [smalldatetime] NULL,
	[LastUpdateUserID] [int] NOT NULL,
	[PageKey] [nvarchar](50) NULL,
	[EditorRoles] [nvarchar](255) NULL,
	[IsPublished] [bit] NOT NULL,
	[DisplayOrder] [int] NOT NULL,
	[ContentTypeID] [int] NOT NULL,
	[Permalink] [nvarchar](500) NULL,
	[DateTimeCreated] [datetime] NOT NULL,
	[IsRestricted] [bit] NOT NULL,
	[ContentID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationKey] [nvarchar](25) NOT NULL,
	[ContentPageGroupTitle] [nvarchar](255) NULL,
	[Tags] [varchar](2000) NULL,
	[SearchBody] nvarchar(max) NULL,
	[ApplicationName] [nvarchar](1000) NOT NULL,
	[ApplicationTypeID] [int] NULL,
	[UserName] [nvarchar](50) NULL,
	[DisplayName] [nvarchar](150) NULL
)


insert into #z_sueetie_vw_contentpages SELECT  *
	from Sueetie_vw_ContentPages where ContentTypeID = @contentTypeID 
	and LastUpdateDateTime > @pubdate



declare @contentPartID int
declare @contentPageID int
declare @contentText nvarchar(max)

declare contentPageCursor cursor for select contentpartID, contentpageID, convert(nvarchar(max), contentText) from sueetie_contentparts where contentpageid > 0

open contentPageCursor

fetch next from contentPageCursor into @contentPartID, @contentPageID, @contentText
while @@fetch_status = 0
begin

	update #z_sueetie_vw_ContentPages set SearchBody = SearchBody + ' ' + convert(nvarchar(max),@contentText) where ContentPageID = @contentPageID
	fetch next from contentPageCursor into @contentPartID, @contentPageID, @contentText
	
end

close contentPageCursor
deallocate contentPageCursor

select * from #z_sueetie_vw_ContentPages order by LastUpdateDateTime

drop table #z_sueetie_vw_ContentPages
END
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Search_BlogPosts_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[Sueetie_Search_BlogPosts_Get] 1, '9/6/1969' 

CREATE PROCEDURE [dbo].[Sueetie_Search_BlogPosts_Get] 
(
	@contentTypeID int,
	@pubdate datetime
)
AS
SET NOCOUNT, XACT_ABORT ON


BEGIN
WITH OrderedEntries AS
(
SELECT
	[SueetiePostID],
	[UserID],
	[PostID],
	[Title],
	[Description],
	[PostContent],
	[DateCreated],
	[DateModified],
	[Author],
	[IsPublished],
	[IsCommentEnabled],
	[Raters],
	[Rating],
	[Slug],
	[Email],
	[DisplayName],
	[GroupID],
	[GroupKey],
	[ApplicationID],
	[ApplicationKey],
	[ApplicationTypeID],
	[ContentID],
	[ContentTypeID],
	[Permalink],
	[IsRestricted],
	[GroupName],
	[ApplicationName],
	[BlogAccessRole],
	[BlogTitle],
	[IncludeInAggregateList],
	[IsActive],
	[Categories],
	[Tags],
	row_number() over(order by DateModified DESC) RowNumber
FROM Sueetie_vw_BlogPosts
where ContentTypeID = @contentTypeID 
and DateModified > @pubdate
--and Author in ('testguy','admin')
)

	select * from orderedEntries 

	--	select * from OrderedEntries
	--	where RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize 

END
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Search_WikiPages_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[Sueetie_Search_WikiPages_Get] 5, '9/6/1969' 

CREATE PROCEDURE [dbo].[Sueetie_Search_WikiPages_Get] 
(
	@contentTypeID int,
	@pubdate datetime
)
AS
SET NOCOUNT, XACT_ABORT ON


BEGIN
WITH OrderedEntries AS
(
SELECT  *,
	row_number() over(order by DateTimeModified DESC) RowNumber
	from Sueetie_vw_WikiPages where ContentTypeID = @contentTypeID 
	and DateTimeModified > @pubdate
)

	select * from orderedEntries 

	--	select * from OrderedEntries
	--	where RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize 

END
GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_BlogPosts_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_BlogPosts_Get] 
(
	@contentTypeID int,
	@userID int,
	@groupID int,
	@applicationID int,
	@numRecords int,
	@IsRestricted bit
)
AS
SET NOCOUNT, XACT_ABORT ON

	set rowcount  @numRecords
		
SELECT  * into #sueetie_vw_blogposts from Sueetie_vw_BlogPosts where ContentTypeID = @contentTypeID 
	and GroupID = CASE WHEN @groupID > -1 then @groupID ELSE GroupID END
	and ApplicationID = CASE WHEN @applicationID > 0 then @applicationID ELSE ApplicationID END
	and UserID = CASE WHEN @UserID > 0 then @UserID ELSE UserID END
	and IsRestricted = CASE WHEN @IsRestricted = 1 then 0 ELSE IsRestricted END
	order by DateCreated desc

	-- @contentTypeID = AggregateBlogPost Display. Retrieve only blog IsActive and IncludeInAggregate
	-- In Sueetie SueetieContentViewType.AggregateBlogPostList = 8
	
	if @contentTypeID = 8
	begin
		select * from #sueetie_vw_blogposts where isactive = 1 and includeinaggregateList = 1
	end
	else
	begin
		select * from #sueetie_vw_blogposts 
	end
	drop table #sueetie_vw_blogposts 
	
	set RowCount 0
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_BlogPost_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Sueetie_BlogPost_Get] 
(
	@postGuid nvarchar(60),
	@contentTypeID int
)
AS
SET NOCOUNT, XACT_ABORT ON

declare @applicationID int
select @applicationID = applicationID from Sueetie_bePosts where postid = @postGuid

	
SELECT   * from Sueetie_vw_BlogPosts where PostID = @postGuid 
	and ApplicationID = @applicationID and ContentTypeID = @contentTypeID
GO


Print '/****** Object:  View [dbo].[sueetie_vw_UserProfileData]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[sueetie_vw_UserProfileData]
AS
SELECT     dbo.Sueetie_Users.UserID, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, dbo.Sueetie_fn_GetProfileElement(N'DisplayName', 
                      dbo.aspnet_Profile.PropertyNames, dbo.aspnet_Profile.PropertyValuesString) AS DisplayName, dbo.Sueetie_fn_GetProfileElement(N'Gender', 
                      dbo.aspnet_Profile.PropertyNames, dbo.aspnet_Profile.PropertyValuesString) AS Gender, dbo.Sueetie_fn_GetProfileElement(N'Country', 
                      dbo.aspnet_Profile.PropertyNames, dbo.aspnet_Profile.PropertyValuesString) AS Country, dbo.Sueetie_fn_GetProfileElement(N'Occupation', 
                      dbo.aspnet_Profile.PropertyNames, dbo.aspnet_Profile.PropertyValuesString) AS Occupation, dbo.Sueetie_fn_GetProfileElement(N'Website', 
                      dbo.aspnet_Profile.PropertyNames, dbo.aspnet_Profile.PropertyValuesString) AS Website, dbo.Sueetie_fn_GetProfileElement(N'TwitterName', 
                      dbo.aspnet_Profile.PropertyNames, dbo.aspnet_Profile.PropertyValuesString) AS TwitterName, dbo.Sueetie_fn_GetProfileElement(N'Newsletter', 
                      dbo.aspnet_Profile.PropertyNames, dbo.aspnet_Profile.PropertyValuesString) AS Newsletter
FROM         dbo.aspnet_Profile INNER JOIN
                      dbo.Sueetie_Users ON dbo.aspnet_Profile.UserId = dbo.Sueetie_Users.MembershipID
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_WikiPages_Get]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_WikiPages_Get] 
(
	@contentTypeID int,
	@userID int,
	@groupID int,
	@applicationID int,
	@numRecords int,
	@IsRestricted bit
)
AS
SET NOCOUNT, XACT_ABORT ON

	set rowcount  @numRecords


CREATE TABLE [dbo].[#z_sueetie_vw_WikiPages](
	[PageID] [int] NOT NULL,
	[PageFileName] [nvarchar](200) NOT NULL,
	[PageTitle] [nvarchar](500) NOT NULL,
	[Keywords] [nvarchar](1000) NOT NULL,
	[Abstract] [nvarchar](max) NULL,
	[Namespace] [nvarchar](200) NULL,
	[DateTimeCreated] [smalldatetime] NULL,
	[DateTimeModified] [smalldatetime] NULL,
	[UserID] [int] NOT NULL,
	[ContentID] [int] NOT NULL,
	[Permalink] [nvarchar](500) NULL,
	[IsRestricted] [bit] NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[DisplayName] [nvarchar](150) NOT NULL,
	[ApplicationKey] [nvarchar](25) NOT NULL,
	[ApplicationName] [nvarchar](1000) NOT NULL,
	[GroupID] [int] NOT NULL,
	[GroupKey] [nvarchar](25) NULL,
	[GroupName] [nvarchar](255) NOT NULL,
	[ContentTypeID] [int] NOT NULL,
	[Active] [bit] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[PageContent][ntext] NULL,
	[ApplicationTypeID] [int] NOT NULL,
	[UserName] [nvarchar](100) NOT NULL,
	[Tags][nvarchar](2000) NULL,
	[Categories][nvarchar](2000) NULL		
)
	

insert into #z_sueetie_vw_WikiPages
SELECT  * from Sueetie_vw_WikiPages where ContentTypeID = @contentTypeID 
	and GroupID = CASE WHEN @groupID > -1 then @groupID ELSE GroupID END
	and ApplicationID = CASE WHEN @applicationID > 0 then @applicationID ELSE ApplicationID END
	and UserID = CASE WHEN @UserID > 0 then @UserID ELSE UserID END
	and IsRestricted = CASE WHEN @IsRestricted = 1 then 0 ELSE IsRestricted END
	order by DateTimeCreated desc
	
	

select * from #z_sueetie_vw_WikiPages
drop table #z_sueetie_vw_wikipages

	set RowCount 0
GO


Print '/****** Object:  View [dbo].[Sueetie_vw_AspnetUsers]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_AspnetUsers]
AS
SELECT     dbo.Sueetie_Users.UserID, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, dbo.Sueetie_Users.DisplayName, dbo.aspnet_Membership.CreateDate, 
                      dbo.aspnet_Membership.LastLoginDate, dbo.aspnet_Membership.FailedPasswordAttemptCount, dbo.aspnet_Users.LastActivityDate
FROM         dbo.aspnet_Membership INNER JOIN
                      dbo.aspnet_Users ON dbo.aspnet_Membership.UserId = dbo.aspnet_Users.UserId INNER JOIN
                      dbo.Sueetie_Users ON dbo.aspnet_Users.UserId = dbo.Sueetie_Users.MembershipID
GO

Print '/****** Object:  View [dbo].[Sueetie_vw_AspnetUserlist]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_AspnetUserlist]
AS
SELECT     dbo.Sueetie_Users.UserID, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, dbo.aspnet_Membership.CreateDate, dbo.aspnet_Membership.LastLoginDate, 
                      dbo.aspnet_Users.LastActivityDate, dbo.Sueetie_Users.IsActive, dbo.aspnet_Membership.IsApproved
FROM         dbo.aspnet_Membership INNER JOIN
                      dbo.aspnet_Users ON dbo.aspnet_Membership.UserId = dbo.aspnet_Users.UserId INNER JOIN
                      dbo.Sueetie_Users ON dbo.aspnet_Users.UserId = dbo.Sueetie_Users.MembershipID
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_UpdateProfile]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_User_UpdateProfile]
(
	@UserID int,
	@PropertyNames ntext,
	@PropertyValues ntext
)
AS
BEGIN

	declare @membershipID uniqueidentifier;
	select @membershipID = membershipID from Sueetie_Users where UserID = @UserID;
	
	update aspnet_Profile set PropertyNames = @PropertyNames, PropertyValuesString = @PropertyValues where UserId = @membershipID
	

END
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_GetProfileDetails]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_User_GetProfileDetails]

@userid int

AS
SELECT   
                     Prof.UserName,
                     Prof.DisplayName,
                     Prof.Gender,
                     Prof.Country,
                     Prof.Occupation, 
                     Prof.Website,
                     Prof.Twittername,
                     Prof.Newsletter 
                     

FROM sueetie_vw_UserProfileData Prof
WHERE Prof.userid = @userid
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_GetProfileByUsername]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_User_GetProfileByUsername]
(
@username nvarchar(50)
)
AS
SELECT   
                     Prof.UserName,
                     Prof.DisplayName,
                     Prof.Gender,
                     Prof.Country,
                     Prof.Occupation, 
                     Prof.Website,
                     Prof.Twittername,
                     Prof.Newsletter 
                     

FROM sueetie_vw_UserProfileData Prof
WHERE Prof.UserName = @username
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Search_MediaAlbums_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Search_MediaAlbums_Get] 
(
	@pubdate smalldatetime
)
AS
SET NOCOUNT, XACT_ABORT ON


BEGIN
WITH OrderedEntries AS
(
SELECT  *, 
	--dbo.ConcatCategories(pageid) as 'categories',
	row_number() over(order by DateLastModified DESC) RowNumber
	from Sueetie_vw_MediaAlbums where ContentTypeID between 11 and 17
	and albumID > 1	
	and DateLastModified > @pubdate
)

	select * from orderedEntries 

	--	select * from OrderedEntries
	--	where RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize 

END
GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaAlbums_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[Sueetie_MediaAlbums_Get] 1, -1, -1, -1, -1, 100, 0

CREATE PROCEDURE [dbo].[Sueetie_MediaAlbums_Get] 
(
	@contentViewTypeID int,
	@contentTypeID int,
	@userID int,
	@groupID int,
	@applicationID int,
	@numRecords int,
	@IsRestricted bit
)
AS
SET NOCOUNT, XACT_ABORT ON

	set rowcount  @numRecords
		

SELECT  * from Sueetie_vw_MediaAlbums where ContentTypeID between 11 and 17
	and albumID > 1
	and ContentTypeID = CASE WHEN @contentTypeID > -1 then @contentTypeID ELSE ContentTypeID END 
	and GroupID = CASE WHEN @groupID > -1 then @groupID ELSE GroupID END
	and ApplicationID = CASE WHEN @applicationID > 0 then @applicationID ELSE ApplicationID END
	and SueetieUserID = CASE WHEN @UserID > 0 then @UserID ELSE SueetieUserID END
	and IsRestricted = CASE WHEN @IsRestricted = 1 then 0 ELSE IsRestricted END
	order by GroupID, GalleryName, AlbumTitle

	
	set RowCount 0
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaAlbum_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_MediaAlbum_Get]
(
	@albumID int
)
AS
SET NOCOUNT, XACT_ABORT ON

select * from sueetie_vw_MediaAlbums where sourceid = @albumID and contenttypeID between 11 and 17
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Search_MediaObjects_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[Sueetie_Search_MediaObjects_Get] '9/6/1969' 

CREATE PROCEDURE [dbo].[Sueetie_Search_MediaObjects_Get] 
(
	@pubdate smalldatetime
)
AS
SET NOCOUNT, XACT_ABORT ON


BEGIN
WITH OrderedEntries AS
(
SELECT  *, 
	--dbo.ConcatCategories(pageid) as 'categories',
	row_number() over(order by DateLastModified DESC) RowNumber
	from Sueetie_vw_MediaObjects where ContentTypeID between 6 and 10
	and DateLastModified > @pubdate
)

	select * from orderedEntries 

	--	select * from OrderedEntries
	--	where RowNumber between @PageIndex * @PageSize + 1 and @PageIndex * @PageSize + @PageSize 

END
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaObjects_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_MediaObjects_Get] 
(
	@contentViewTypeID int,
	@contentTypeID int,
	@userID int,
	@groupID int,
	@applicationID int,
	@numRecords int,
	@IsRestricted bit
)
AS
SET NOCOUNT, XACT_ABORT ON

	set rowcount  @numRecords
		
	if @contentViewTypeID = 1 
	begin	
SELECT  * from Sueetie_vw_MediaObjects where ContentTypeID = @contentTypeID 
	and GroupID = CASE WHEN @groupID > -1 then @groupID ELSE GroupID END
	and ApplicationID = CASE WHEN @applicationID > 0 then @applicationID ELSE ApplicationID END
	and SueetieUserID = CASE WHEN @UserID > 0 then @UserID ELSE SueetieUserID END
	and IsRestricted = CASE WHEN @IsRestricted = 1 then 0 ELSE IsRestricted END
	order by DateTimeCreated desc
end
	
	set RowCount 0
GO

Print '/****** Object:  Default [DF_Sueetie_beComments_SueetieUserID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_beComments] ADD  CONSTRAINT [DF_Sueetie_beComments_SueetieUserID]  DEFAULT ((-1)) FOR [UserID]
GO
Print '/****** Object:  Default [DF_Sueetie_beComments_PostCommentID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_beComments] ADD  CONSTRAINT [DF_Sueetie_beComments_PostCommentID]  DEFAULT (newid()) FOR [PostCommentID]
GO
Print '/****** Object:  Default [DF_Sueetie_beComments_ParentCommentID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_beComments] ADD  CONSTRAINT [DF_Sueetie_beComments_ParentCommentID]  DEFAULT ('00000000-0000-0000-0000-000000000000') FOR [ParentCommentID]
GO
Print '/****** Object:  Default [DF_Sueetie_bePosts_SueetieUserID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_bePosts] ADD  CONSTRAINT [DF_Sueetie_bePosts_SueetieUserID]  DEFAULT ((-1)) FOR [UserID]
GO
Print '/****** Object:  Default [DF_Sueetie_bePosts_PostID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_bePosts] ADD  CONSTRAINT [DF_Sueetie_bePosts_PostID]  DEFAULT (newid()) FOR [PostID]
GO
Print '/****** Object:  Default [DF_Sueetie_Blogs_CategoryID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Blogs] ADD  CONSTRAINT [DF_Sueetie_Blogs_CategoryID]  DEFAULT ((0)) FOR [CategoryID]
GO
Print '/****** Object:  Default [DF_Sueetie_Blogs_MostRecentContentID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Blogs] ADD  CONSTRAINT [DF_Sueetie_Blogs_MostRecentContentID]  DEFAULT ((-1)) FOR [MostRecentContentID]
GO
Print '/****** Object:  Default [DF_Sueetie_Blogs_PostCount]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Blogs] ADD  CONSTRAINT [DF_Sueetie_Blogs_PostCount]  DEFAULT ((0)) FOR [PostCount]
GO
Print '/****** Object:  Default [DF_Sueetie_Blogs_CommentCount]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Blogs] ADD  CONSTRAINT [DF_Sueetie_Blogs_CommentCount]  DEFAULT ((0)) FOR [CommentCount]
GO
Print '/****** Object:  Default [DF_Sueetie_Blogs_TrackbackCount]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Blogs] ADD  CONSTRAINT [DF_Sueetie_Blogs_TrackbackCount]  DEFAULT ((0)) FOR [TrackbackCount]
GO
Print '/****** Object:  Default [DF_Sueetie_Blogs_DisplayOnAggregate]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Blogs] ADD  CONSTRAINT [DF_Sueetie_Blogs_DisplayOnAggregate]  DEFAULT ((1)) FOR [IncludeInAggregateList]
GO
Print '/****** Object:  Default [DF_Sueetie_Blogs_IsActive]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Blogs] ADD  CONSTRAINT [DF_Sueetie_Blogs_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
Print '/****** Object:  Default [DF__Sueetie_B__Regis__3CB4DFB3]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Blogs] ADD  DEFAULT ((1)) FOR [RegisteredComments]
GO
Print '/****** Object:  Default [DF_Sueetie_CalendarEvents_CalendarID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_CalendarEvents] ADD  CONSTRAINT [DF_Sueetie_CalendarEvents_CalendarID]  DEFAULT ((1)) FOR [CalendarID]
GO
Print '/****** Object:  Default [DF_Sueetie_CalendarEvents_EndDateTime]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_CalendarEvents] ADD  CONSTRAINT [DF_Sueetie_CalendarEvents_EndDateTime]  DEFAULT (((6)/(9))/(1969)) FOR [EndDateTime]
GO
Print '/****** Object:  Default [DF_Sueetie_CalendarEvents_AllDat]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_CalendarEvents] ADD  CONSTRAINT [DF_Sueetie_CalendarEvents_AllDat]  DEFAULT ((0)) FOR [AllDayEvent]
GO
Print '/****** Object:  Default [DF_Sueetie_CalendarEvents_RepeatEndDate]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_CalendarEvents] ADD  CONSTRAINT [DF_Sueetie_CalendarEvents_RepeatEndDate]  DEFAULT (((6)/(9))/(1969)) FOR [RepeatEndDate]
GO
Print '/****** Object:  Default [DF_Sueetie_CalendarEvents_SourceContentID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_CalendarEvents] ADD  CONSTRAINT [DF_Sueetie_CalendarEvents_SourceContentID]  DEFAULT ((0)) FOR [SourceContentID]
GO
Print '/****** Object:  Default [DF_Sueetie_CalendarEvents_IsActive]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_CalendarEvents] ADD  CONSTRAINT [DF_Sueetie_CalendarEvents_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
Print '/****** Object:  Default [DF_Sueetie_Calendars_IsActive]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Calendars] ADD  CONSTRAINT [DF_Sueetie_Calendars_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
Print '/****** Object:  Default [DF_Table_1_IsGlobalCategory]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Categories] ADD  CONSTRAINT [DF_Table_1_IsGlobalCategory]  DEFAULT ((1)) FOR [IsContentCategory]
GO
Print '/****** Object:  Default [DF_Sueetie_Categories_IsGlobalCategory]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Categories] ADD  CONSTRAINT [DF_Sueetie_Categories_IsGlobalCategory]  DEFAULT ((1)) FOR [IsGlobalCategory]
GO
Print '/****** Object:  Default [DF_Sueetie_Categories_ApplicationTypeID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Categories] ADD  CONSTRAINT [DF_Sueetie_Categories_ApplicationTypeID]  DEFAULT ((0)) FOR [ApplicationTypeID]
GO
Print '/****** Object:  Default [DF_Sueetie_Categories_ApplicationID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Categories] ADD  CONSTRAINT [DF_Sueetie_Categories_ApplicationID]  DEFAULT ((0)) FOR [ApplicationID]
GO
Print '/****** Object:  Default [DF_Sueetie_Categories_IsActive]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Categories] ADD  CONSTRAINT [DF_Sueetie_Categories_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
Print '/****** Object:  Default [DF_Sueetie_Content_IsRestricted]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Content] ADD  CONSTRAINT [DF_Sueetie_Content_IsRestricted]  DEFAULT ((0)) FOR [IsRestricted]
GO
Print '/****** Object:  Default [DF__Sueetie_C__Appli__660BFB01]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_ContentPageGroups] ADD  CONSTRAINT [DF__Sueetie_C__Appli__660BFB01]  DEFAULT ((-1)) FOR [ApplicationID]
GO
Print '/****** Object:  Default [DF_Sueetie_ContentPageGroups_IsActive]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_ContentPageGroups] ADD  CONSTRAINT [DF_Sueetie_ContentPageGroups_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
Print '/****** Object:  Default [DF_Sueetie_ContentPages_LastUpdateUserID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_ContentPages] ADD  CONSTRAINT [DF_Sueetie_ContentPages_LastUpdateUserID]  DEFAULT ((-1)) FOR [LastUpdateUserID]
GO
Print '/****** Object:  Default [DF_Sueetie_ContentPages_IsActive]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_ContentPages] ADD  CONSTRAINT [DF_Sueetie_ContentPages_IsActive]  DEFAULT ((1)) FOR [IsPublished]
GO
Print '/****** Object:  Default [DF_Sueetie_ContentPages_DisplayOrder]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_ContentPages] ADD  CONSTRAINT [DF_Sueetie_ContentPages_DisplayOrder]  DEFAULT ((-1)) FOR [DisplayOrder]
GO
Print '/****** Object:  Default [DF_Sueetie_ContentParts_ContentPageID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_ContentParts] ADD  CONSTRAINT [DF_Sueetie_ContentParts_ContentPageID]  DEFAULT ((-1)) FOR [ContentPageID]
GO
Print '/****** Object:  Default [DF_Sueetie_ContentParts_LastUpdateUserID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_ContentParts] ADD  CONSTRAINT [DF_Sueetie_ContentParts_LastUpdateUserID]  DEFAULT ((-1)) FOR [LastUpdateUserID]
GO
Print '/****** Object:  Default [DF__Sueetie_C__IsAlb__37D02F05]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_ContentTypes] ADD  CONSTRAINT [DF__Sueetie_C__IsAlb__37D02F05]  DEFAULT ((0)) FOR [IsAlbum]
GO
Print '/****** Object:  Default [DF__Sueetie_C__UserL__3BA0BFE9]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_ContentTypes] ADD  CONSTRAINT [DF__Sueetie_C__UserL__3BA0BFE9]  DEFAULT ((0)) FOR [UserLogCategoryID]
GO
Print '/****** Object:  Default [DF__Sueetie_C__Album__3C94E422]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_ContentTypes] ADD  CONSTRAINT [DF__Sueetie_C__Album__3C94E422]  DEFAULT ((0)) FOR [AlbumMediaCategoryID]
GO
Print '/****** Object:  Default [DF_Sueetie_Groups_GroupTypeID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Groups] ADD  CONSTRAINT [DF_Sueetie_Groups_GroupTypeID]  DEFAULT ((0)) FOR [GroupTypeID]
GO
Print '/****** Object:  Default [DF_Sueetie_Groups_IsActive]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Groups] ADD  CONSTRAINT [DF_Sueetie_Groups_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
Print '/****** Object:  Default [DF_Sueetie_Groups_HasAvatar]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Groups] ADD  CONSTRAINT [DF_Sueetie_Groups_HasAvatar]  DEFAULT ((0)) FOR [HasAvatar]
GO
Print '/****** Object:  Default [DF_Sueetie_gs_MediaObject_InDownloadReport]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_gs_MediaObject] ADD  CONSTRAINT [DF_Sueetie_gs_MediaObject_InDownloadReport]  DEFAULT ((0)) FOR [InDownloadReport]
GO
Print '/****** Object:  Default [DF_Sueetie_Roles_IsGroupAdminRole]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Roles] ADD  CONSTRAINT [DF_Sueetie_Roles_IsGroupAdminRole]  DEFAULT ((0)) FOR [IsGroupAdminRole]
GO
Print '/****** Object:  Default [DF_Sueetie_Roles_IsGroupUserRole]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Roles] ADD  CONSTRAINT [DF_Sueetie_Roles_IsGroupUserRole]  DEFAULT ((0)) FOR [IsGroupUserRole]
GO
Print '/****** Object:  Default [DF__Sueetie_R__IsBlo__2C29722F]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Roles] ADD  DEFAULT ((0)) FOR [IsBlogOwnerRole]
GO
Print '/****** Object:  Default [DF_Table_1_BlogDocsIndexed]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_SearchIndexCounts] ADD  CONSTRAINT [DF_Table_1_BlogDocsIndexed]  DEFAULT ((0)) FOR [BlogPosts]
GO
Print '/****** Object:  Default [DF_Table_1_ForumDocsIndexed]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_SearchIndexCounts] ADD  CONSTRAINT [DF_Table_1_ForumDocsIndexed]  DEFAULT ((0)) FOR [ForumMessages]
GO
Print '/****** Object:  Default [DF_Table_1_WikiDocsIndexed]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_SearchIndexCounts] ADD  CONSTRAINT [DF_Table_1_WikiDocsIndexed]  DEFAULT ((0)) FOR [WikiPages]
GO
Print '/****** Object:  Default [DF_Sueetie_IndexCounts_MediaAlbums]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_SearchIndexCounts] ADD  CONSTRAINT [DF_Sueetie_IndexCounts_MediaAlbums]  DEFAULT ((0)) FOR [MediaAlbums]
GO
Print '/****** Object:  Default [DF_Sueetie_IndexCounts_MediaObjects]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_SearchIndexCounts] ADD  CONSTRAINT [DF_Sueetie_IndexCounts_MediaObjects]  DEFAULT ((0)) FOR [MediaObjects]
GO
Print '/****** Object:  Default [DF__Sueetie_S__Conte__12DEA178]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_SearchIndexCounts] ADD  DEFAULT ((0)) FOR [ContentPages]
GO
Print '/****** Object:  Default [DF_Sueetie_TagMaster_CreatedBy]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_TagMaster] ADD  CONSTRAINT [DF_Sueetie_TagMaster_CreatedBy]  DEFAULT ((-1)) FOR [CreatedBy]
GO
Print '/****** Object:  Default [DF_Sueetie_TagMaster_IsActive]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_TagMaster] ADD  CONSTRAINT [DF_Sueetie_TagMaster_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
Print '/****** Object:  Default [DF_Sueetie_Tags_TagMasterID]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Tags] ADD  CONSTRAINT [DF_Sueetie_Tags_TagMasterID]  DEFAULT ((-1)) FOR [TagMasterID]
GO
Print '/****** Object:  Default [DF_Sueetie_Tags_IsActive]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Tags] ADD  CONSTRAINT [DF_Sueetie_Tags_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
Print '/****** Object:  Default [DF_Sueetie_UserFriendsFavorites_Following]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_UserFollowCounts] ADD  CONSTRAINT [DF_Sueetie_UserFriendsFavorites_Following]  DEFAULT ((0)) FOR [Following]
GO
Print '/****** Object:  Default [DF_Sueetie_UserFriendsFavorites_Followers]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_UserFollowCounts] ADD  CONSTRAINT [DF_Sueetie_UserFriendsFavorites_Followers]  DEFAULT ((0)) FOR [Followers]
GO
Print '/****** Object:  Default [DF_Sueetie_UserFriendsFavorites_Friends]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_UserFollowCounts] ADD  CONSTRAINT [DF_Sueetie_UserFriendsFavorites_Friends]  DEFAULT ((0)) FOR [Friends]
GO
Print '/****** Object:  Default [DF_Sueetie_UserLogCategories_IsDisplayed]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_UserLogCategories] ADD  CONSTRAINT [DF_Sueetie_UserLogCategories_IsDisplayed]  DEFAULT ((1)) FOR [IsDisplayed]
GO
Print '/****** Object:  Default [DF_Sueetie_UserLogCategories_IsLocked]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_UserLogCategories] ADD  CONSTRAINT [DF_Sueetie_UserLogCategories_IsLocked]  DEFAULT ((0)) FOR [IsLocked]
GO
Print '/****** Object:  Default [DF__Sueetie_U__IsSyn__7874C3FF]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_UserLogCategories] ADD  DEFAULT ((1)) FOR [IsSyndicated]
GO
Print '/****** Object:  Default [DF__Sueetie_U__IsApp__5D36BDDE]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
Print '/****** Object:  Default [DF__Sueetie_U__InAna__3FB147EF]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_Users] ADD  DEFAULT ((1)) FOR [InAnalytics]
GO
Print '/****** Object:  Default [DF_Sueetie_WikiPages_Active]    Script Date: 10/15/2010 13:32:33 ******/'
ALTER TABLE [dbo].[Sueetie_WikiPages] ADD  CONSTRAINT [DF_Sueetie_WikiPages_Active]  DEFAULT ((1)) FOR [Active]
GO

/* ----------  Added from 2.x upgrade scripts ------------------------------------------------ */

Print '/****** Object:  Table [dbo].[Sueetie_DisplayTypes]    Script Date: 12/06/2010 20:59:20 ******/'

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sueetie_DisplayTypes](
	[DisplayTypeID] [int] NOT NULL,
	[DisplayTypeDescription] [nvarchar](50) NULL,
 CONSTRAINT [PK_Sueetie_DisplayTypes] PRIMARY KEY CLUSTERED 
(
	[DisplayTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


Print '/****** Object:  Table [dbo].[Sueetie_gs_Gallery]    Script Date: 12/07/2010 17:23:07 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sueetie_gs_Gallery](
	[GalleryID] [int] NOT NULL,
	[GalleryKey] [nvarchar](60) NULL,
	[GalleryDescription] [nvarchar](max) NULL,
	[DisplayTypeID] [int] NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[IsPublic] [bit] NOT NULL,
	[IsLogged] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_gs_Gallery] PRIMARY KEY CLUSTERED 
(
	[GalleryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Sueetie_gs_Gallery] ADD  CONSTRAINT [DF_Sueetie_gs_Gallery_DisplayTypeID]  DEFAULT ((0)) FOR [DisplayTypeID]
GO

ALTER TABLE [dbo].[Sueetie_gs_Gallery] ADD  CONSTRAINT [DF_Sueetie_gs_Gallery_ApplicationID]  DEFAULT ((0)) FOR [ApplicationID]
GO

ALTER TABLE [dbo].[Sueetie_gs_Gallery] ADD  CONSTRAINT [DF_Sueetie_gs_Gallery_IsPublic]  DEFAULT ((1)) FOR [IsPublic]
GO

ALTER TABLE [dbo].[Sueetie_gs_Gallery] ADD  CONSTRAINT [DF_Sueetie_gs_Gallery_IsLogged]  DEFAULT ((1)) FOR [IsLogged]
GO


Print '/****** Object:  Table [dbo].[Sueetie_WikiMessages]    Script Date: 12/07/2010 17:23:07 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sueetie_WikiMessages](
	[MessageID] [int] IDENTITY(1,1) NOT NULL,
	[PageID] [int] NOT NULL,
	[MessageQueryID] [nvarchar](50) NULL,
	[Subject] [nvarchar](500) NULL,
	[Message] [ntext] NULL,
	[UserID] [int] NOT NULL,
	[DateTimeCreated] [smalldatetime] NULL,
	[DateTimeModified] [smalldatetime] NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Sueetie_WikiMessages] PRIMARY KEY CLUSTERED 
(
	[MessageID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[Sueetie_WikiMessages] ADD  CONSTRAINT [DF_Sueetie_WikiMessages_UserID]  DEFAULT ((-1)) FOR [UserID]
GO

ALTER TABLE [dbo].[Sueetie_WikiMessages] ADD  CONSTRAINT [DF_Sueetie_WikiMessages_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO



Print '/****** Object:  View [dbo].[Sueetie_vw_WikiMessages]    Script Date: 12/23/2010 16:26:28 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Sueetie_vw_WikiMessages]
AS
SELECT     dbo.Sueetie_WikiMessages.MessageID, dbo.Sueetie_WikiMessages.PageID, dbo.Sueetie_WikiMessages.MessageQueryID, dbo.Sueetie_WikiMessages.Subject, 
                      dbo.Sueetie_WikiMessages.Message, dbo.Sueetie_WikiMessages.UserID, dbo.Sueetie_WikiMessages.DateTimeCreated, 
                      dbo.Sueetie_WikiMessages.DateTimeModified, dbo.Sueetie_WikiMessages.IsActive, dbo.Sueetie_Users.UserName, dbo.Sueetie_Users.Email, 
                      dbo.Sueetie_Users.DisplayName, dbo.Sueetie_WikiPages.PageFileName, dbo.Sueetie_WikiPages.PageTitle, dbo.Sueetie_WikiPages.Namespace, 
                      dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_ContentTypes.ContentTypeName, dbo.Sueetie_Content.SourceID, 
                      dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Applications.ApplicationTypeID, dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_Applications.GroupID, 
                      dbo.Sueetie_Content.Permalink, dbo.Sueetie_Content.IsRestricted, dbo.Sueetie_ContentTypes.UserLogCategoryID
FROM         dbo.Sueetie_Users INNER JOIN
                      dbo.Sueetie_WikiMessages ON dbo.Sueetie_Users.UserID = dbo.Sueetie_WikiMessages.UserID INNER JOIN
                      dbo.Sueetie_ContentTypes INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_ContentTypes.ContentTypeID = dbo.Sueetie_Content.ContentTypeID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID ON 
                      dbo.Sueetie_WikiMessages.MessageID = dbo.Sueetie_Content.SourceID INNER JOIN
                      dbo.Sueetie_WikiPages ON dbo.Sueetie_WikiMessages.PageID = dbo.Sueetie_WikiPages.PageID

GO


print '/****** Object:  StoredProcedure [dbo].[Sueetie_WikiMessage_Add]    Script Date: 12/23/2010 16:27:25 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_WikiMessage_Add]
	@PageID int,
	@MessageQueryID nvarchar(50),
	@Subject nvarchar(500),
	@Message ntext,
	@UserID int,
	@MessageID int OUTPUT


AS

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_WikiMessages] (
	[PageID],
	[MessageQueryID],
	[Subject],
	[Message],
	[UserID],
	[DateTimeCreated]
) VALUES (
	@PageID,
	@MessageQueryID,
	@Subject,
	@Message,
	@UserID,
	GETDATE()
)


SET @MessageID = SCOPE_IDENTITY() 

select @MessageID


GO


print '/****** Object:  StoredProcedure [dbo].[Sueetie_WikiMessages_Get]    Script Date: 12/26/2010 15:19:39 ******/'

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_WikiMessages_Get] 
(
	@contentTypeID int
)
AS
SET NOCOUNT, XACT_ABORT ON

SELECT * from Sueetie_vw_WikiMessages where ContentTypeID = @contentTypeID 
	order by DateTimeCreated desc

GO

print '/****** Object:  StoredProcedure [dbo].[Sueetie_WikiMessage_Update]    Script Date: 12/26/2010 15:20:56 ******/'

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_WikiMessage_Update]
	@MessageID int,
	@MessageQueryID nvarchar(50),
	@Subject nvarchar(500),
	@Message ntext,
	@IsActive bit
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_WikiMessages] SET
	[MessageQueryID] = @MessageQueryID,
	[Subject] = @Subject,
	[Message] = @Message,
	[DateTimeModified] = GETDATE(),
	[IsActive] = @IsActive
WHERE
	[MessageID] = @MessageID


GO
