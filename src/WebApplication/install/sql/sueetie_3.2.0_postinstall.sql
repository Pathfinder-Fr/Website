Print '/****** Object:  UserDefinedFunction [dbo].[ForumLeadTopicMessageID]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ForumLeadTopicMessageID](@topicID int)
RETURNS VARCHAR(200)
AS
BEGIN
	DECLARE @Output int
	SELECT @Output = m.MessageID
	FROM	dbo.[yaf_Message] m
		JOIN dbo.yaf_topic t
			ON t.TopicID = @topicID
			and m.TopicID = @topicID and m.Position = 0
	
	RETURN @Output
END
GO

Print '/****** Object:  UserDefinedFunction [dbo].[ConcatForumTopicUsers]    Script Date: 03/08/2011 11:12:12 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[ConcatForumTopicUsers](@topicID int, @boardID int)
RETURNS VARCHAR(1000)
AS
BEGIN
	DECLARE @Output VARCHAR(1000)
	DECLARE @TopicUsers TABLE (userid nvarchar(10))
	
	insert into @TopicUsers SELECT distinct(CONVERT(nvarchar(10), s.UserID))
	FROM	dbo.[yaf_Message] m
		JOIN dbo.yaf_topic t
			ON t.TopicID = @topicID 
			JOIN dbo.yaf_User u ON
				m.UserID = u.UserID
				JOIN dbo.Sueetie_users s 
					ON s.Username = u.Name
					where m.TopicID = @topicID and u.BoardID = @boardID
					
	SELECT @Output =  COALESCE(@Output+'|', '') + userid
	FROM	@TopicUsers 
	ORDER BY userID
	
	RETURN @Output
END
GO

Print '/****** Object:  UserDefinedFunction [dbo].[ConcatForumTopicTags]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConcatForumTopicTags](@topicID int)
RETURNS VARCHAR(2000)
AS
BEGIN
	DECLARE @Output VARCHAR(2000)
	SELECT @Output = COALESCE(@Output+'|', '') + CONVERT(varchar(200), t.tag)
	FROM	dbo.[sueetie_ForumTopicTags] t
		JOIN dbo.yaf_topic y
			ON y.TopicID = @topicID
			and t.TopicID = @topicID
	ORDER BY t.tag

	RETURN @Output
END
GO


Print '/****** Object:  StoredProcedure [dbo].[yaf_sueetie_post_list]   Script Date: 11/24/2010 11:57:17 ******/'

/****** Object:  StoredProcedure [dbo].[yaf_sueetie_post_list]    Script Date: 04/05/2011 14:21:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[yaf_sueetie_post_list](
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


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_yaf_user_aspnet]    Script Date: 11/25/2010 10:39:15 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[yaf_sueetie_user_aspnet]
(
@BoardID int,
@UserName nvarchar(255),
@DisplayName nvarchar(255),
@Email nvarchar(255),
@ProviderUserKey nvarchar(64),
@IsApproved bit,
@TimeZone int
) 
as
BEGIN

	SET NOCOUNT ON

	DECLARE @UserID int, @RankID int, @approvedFlag int

	SET @approvedFlag = 0;
	IF (@IsApproved = 1) SET @approvedFlag = 2;	
	
	IF EXISTS(SELECT 1 FROM [dbo].[yaf_User] where BoardID=@BoardID and ([ProviderUserKey]=@ProviderUserKey OR [Name] = @UserName))
	BEGIN
		SELECT TOP 1 @UserID = UserID FROM [dbo].[yaf_User] WHERE [BoardID]=@BoardID and ([ProviderUserKey]=@ProviderUserKey OR [Name] = @UserName)
		
		IF (@DisplayName IS NULL) 
		BEGIN
			SELECT TOP 1 @DisplayName = DisplayName FROM [dbo].[yaf_User] WHERE UserId = @UserID
		END

		UPDATE [dbo].[yaf_User] SET 
			[Name] = @UserName,
			DisplayName = @DisplayName,
			Email = @Email,
			[ProviderUserKey] = @ProviderUserKey,
			Flags = Flags | @approvedFlag
		WHERE
			UserID = @UserID
	END ELSE
	BEGIN
		SELECT @RankID = RankID from [dbo].[yaf_Rank] where (Flags & 1)<>0 and BoardID=@BoardID
		
		IF (@DisplayName IS NULL) 
		BEGIN
			SET @DisplayName = @UserName
		END		

		INSERT INTO [dbo].[yaf_User](BoardID,RankID,[Name],DisplayName,Password,Email,Joined,LastVisit,NumPosts,TimeZone,Flags,ProviderUserKey) 
		VALUES(@BoardID,@RankID,@UserName,@DisplayName,'-',@Email,GETUTCDATE() ,GETUTCDATE() ,0,@TimeZone,@approvedFlag,@ProviderUserKey)
	
		SET @UserID = SCOPE_IDENTITY()	
	END
	
	SELECT UserID=@UserID
END
GO

Print '/****** Object:  View [dbo].[Sueetie_vw_ForumTopics]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_ForumTopics]
AS
SELECT     dbo.yaf_Topic.TopicID, dbo.yaf_Topic.ForumID, dbo.yaf_Topic.UserID, dbo.yaf_Topic.Topic, dbo.Sueetie_Users.UserID AS SueetieUserID, 
                      dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.SourceID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Content.ApplicationID, 
                      dbo.Sueetie_Content.Permalink, dbo.Sueetie_Content.IsRestricted, dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Users.DisplayName, 
                      dbo.Sueetie_Applications.ApplicationKey, dbo.Sueetie_Groups.GroupName, dbo.yaf_Forum.Name AS Forum, dbo.yaf_Topic.Posted AS DateTimeCreated, 
                      dbo.yaf_Topic.IsDeleted, dbo.ConcatForumTopicTags(dbo.yaf_Topic.TopicID) AS Tags, dbo.ConcatForumTopicUsers(dbo.yaf_Topic.TopicID, dbo.yaf_User.BoardID) 
                      AS SueetieUserIDs, dbo.ForumLeadTopicMessageID(dbo.yaf_Topic.TopicID) AS LeadTopicMessageID
FROM         dbo.Sueetie_Content INNER JOIN
                      dbo.Sueetie_Users INNER JOIN
                      dbo.yaf_Topic INNER JOIN
                      dbo.yaf_User ON dbo.yaf_Topic.UserID = dbo.yaf_User.UserID ON dbo.Sueetie_Users.MembershipID = dbo.yaf_User.ProviderUserKey ON 
                      dbo.Sueetie_Content.SourceID = dbo.yaf_Topic.TopicID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.yaf_Forum ON dbo.yaf_Topic.ForumID = dbo.yaf_Forum.ForumID
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ForumTopics_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_ForumTopics_Get] 
(
	@contentTypeID int,
	@userID int,
	@groupID int,
	@numRecords int,
	@IsRestricted bit
)
AS
SET NOCOUNT, XACT_ABORT ON


	set rowcount  @numRecords
	
SELECT * from Sueetie_vw_ForumTopics where IsDeleted = 0 and ContentTypeID = @contentTypeID 
	and GroupID = @groupID
	and SueetieUserID = CASE WHEN @UserID > 0 then @UserID ELSE SueetieUserID END
	and IsRestricted = CASE WHEN @IsRestricted = 1 then 0 ELSE IsRestricted END
	order by DateTimeCreated desc
	
	set RowCount 0
GO

Print '/****** Object:  Table [dbo].[Sueetie_ForumMessageTags]    Script Date: 10/15/2010 13:32:33 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sueetie_ForumMessageTags](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[MessageID] [int] NOT NULL,
	[Tag] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Sueetie_ForumMessageTags] PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
Print '/****** Object:  UserDefinedFunction [dbo].[ConcatForumMessageTags]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConcatForumMessageTags](@MessageID int)
RETURNS VARCHAR(2000)
AS
BEGIN
	DECLARE @Output VARCHAR(2000)
	SELECT @Output = COALESCE(@Output+'|', '') + CONVERT(varchar(200), t.tag)
	FROM	dbo.[sueetie_ForumMessageTags] t
		JOIN dbo.yaf_message y
			ON y.MessageID = @MessageID
			and t.MessageID = @MessageID
	ORDER BY t.tag

	RETURN @Output
END
GO


Print '/****** Object:  View [dbo].[Sueetie_vw_ForumMessages]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_ForumMessages]
AS
SELECT     dbo.yaf_Message.MessageID, dbo.yaf_Message.TopicID, dbo.yaf_Message.UserID, dbo.yaf_Message.Message, dbo.Sueetie_Users.UserID AS SueetieUserID, 
                      dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Content.IsRestricted, 
                      dbo.Sueetie_Content.Permalink, dbo.Sueetie_Content.DateTimeCreated, dbo.Sueetie_Content.SourceID, dbo.yaf_Topic.Topic, 
                      ISNULL(dbo.Sueetie_Users.DisplayName, dbo.Sueetie_Users.UserName) AS DisplayName, dbo.Sueetie_Users.Email, 
                      dbo.Sueetie_Applications.Description AS ApplicationDescription, dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Groups.GroupName, 
                      Sueetie_Users_1.UserID AS TopicSueetieUserID, Sueetie_Users_1.DisplayName AS TopicDisplayName, dbo.Sueetie_Users.UserName, 
                      dbo.Sueetie_Applications.ApplicationTypeID, dbo.Sueetie_Applications.ApplicationKey, dbo.yaf_Forum.Name AS Forum, dbo.yaf_Forum.ForumID, 
                      dbo.Sueetie_Groups.GroupKey, dbo.yaf_Message.Edited, dbo.ConcatForumMessageTags(dbo.yaf_Message.MessageID) AS Tags
FROM         dbo.Sueetie_Content INNER JOIN
                      dbo.yaf_Message ON dbo.Sueetie_Content.SourceID = dbo.yaf_Message.MessageID INNER JOIN
                      dbo.yaf_User INNER JOIN
                      dbo.Sueetie_Users ON dbo.yaf_User.ProviderUserKey = dbo.Sueetie_Users.MembershipID ON dbo.yaf_Message.UserID = dbo.yaf_User.UserID INNER JOIN
                      dbo.yaf_Topic ON dbo.yaf_Message.TopicID = dbo.yaf_Topic.TopicID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.yaf_User AS yaf_User_1 ON dbo.yaf_Topic.UserID = yaf_User_1.UserID INNER JOIN
                      dbo.Sueetie_Users AS Sueetie_Users_1 ON yaf_User_1.ProviderUserKey = Sueetie_Users_1.MembershipID INNER JOIN
                      dbo.yaf_Forum ON dbo.yaf_Topic.ForumID = dbo.yaf_Forum.ForumID
GO


Print '/****** Object:  View [dbo].[Sueetie_vw_FaveForumTopics]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_FaveForumTopics]
AS
SELECT     dbo.Sueetie_Favorites.FavoriteID, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Favorites.UserID, dbo.Sueetie_Users.UserID AS AuthorUserID, 
                      dbo.Sueetie_Users.DisplayName, dbo.yaf_Topic.Topic AS Title, 'In Forum "' + dbo.yaf_Forum.Name + '"' AS 'Description', dbo.Sueetie_Content.Permalink, 
                      dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Groups.GroupName, 
                      dbo.Sueetie_Content.IsRestricted, dbo.Sueetie_Content.DateTimeCreated, dbo.Sueetie_Applications.Description AS ApplicationName, 
                      dbo.Sueetie_ContentTypes.Description AS ContentType
FROM         dbo.Sueetie_Favorites INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_Favorites.ContentID = dbo.Sueetie_Content.ContentID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_Content.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_ContentTypes ON dbo.Sueetie_Content.ContentTypeID = dbo.Sueetie_ContentTypes.ContentTypeID INNER JOIN
                      dbo.yaf_Topic ON dbo.Sueetie_Content.SourceID = dbo.yaf_Topic.TopicID INNER JOIN
                      dbo.yaf_Forum ON dbo.yaf_Topic.ForumID = dbo.yaf_Forum.ForumID
GO

Print '/****** Object:  View [dbo].[Sueetie_vw_FaveForumMessages]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_FaveForumMessages]
AS
SELECT     dbo.Sueetie_Favorites.FavoriteID, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Favorites.UserID, dbo.Sueetie_Users.UserID AS AuthorUserID, 
                      dbo.Sueetie_Users.DisplayName, dbo.yaf_Topic.Topic AS Title, CONVERT(nvarchar(MAX), dbo.yaf_Message.Message) AS Description, 
                      dbo.Sueetie_Content.Permalink, dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Content.ContentTypeID, dbo.Sueetie_Applications.GroupID, 
                      dbo.Sueetie_Groups.GroupName, dbo.Sueetie_Content.IsRestricted, dbo.Sueetie_Content.DateTimeCreated, dbo.Sueetie_Applications.Description AS ApplicationName,
                       dbo.Sueetie_ContentTypes.Description AS ContentType
FROM         dbo.yaf_Message INNER JOIN
                      dbo.Sueetie_Favorites INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_Favorites.ContentID = dbo.Sueetie_Content.ContentID INNER JOIN
                      dbo.Sueetie_Applications ON dbo.Sueetie_Content.ApplicationID = dbo.Sueetie_Applications.ApplicationID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_Content.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_ContentTypes ON dbo.Sueetie_Content.ContentTypeID = dbo.Sueetie_ContentTypes.ContentTypeID ON 
                      dbo.yaf_Message.MessageID = dbo.Sueetie_Content.SourceID INNER JOIN
                      dbo.yaf_Topic ON dbo.yaf_Message.TopicID = dbo.yaf_Topic.TopicID
GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ForumTopicTags_CreateUpdate]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_ForumTopicTags_CreateUpdate]
	@ContentID int,
	@ItemID int,
	@ContentTypeID int,
	@UserID int,
	@Tags nvarchar(500)
AS


SET NOCOUNT ON

declare @LeadTopicMessageID int
select @LeadTopicMessageID = LeadTopicMessageID from Sueetie_vw_ForumTopics 
	where TopicID = @ItemID and ContentTypeID = @ContentTypeID 
	
delete from Sueetie_ForumTopicTags where TopicID = @ItemID
delete from Sueetie_ForumMessageTags where MessageID = @LeadTopicMessageID
delete from Sueetie_Tags where ContentID = @ContentID

declare @tagMasterID int
declare @tagID int
declare @tag nvarchar(50)

declare tagCursor cursor for select item from  fnSplit(@tags,'|')

open tagCursor

fetch next from tagCursor into @tag
while @@fetch_status = 0
begin

	insert into Sueetie_ForumTopicTags (TopicID, Tag) values (@ItemID, @tag)
	insert into Sueetie_ForumMessageTags (MessageID, Tag) values (@LeadTopicMessageID, @tag)
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

update yaf_Message set Edited = GETDATE() where MessageID = @LeadTopicMessageID
GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ForumTopic_Add]    Script Date: 01/11/2011 11:02:17 ******/'

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--region [dbo].[Sueetie_beComment]


CREATE PROCEDURE  [dbo].[Sueetie_ForumTopic_Add]
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


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_ForumMessage_Add]    Script Date: 01/11/2011 11:07:46 ******/'

/****** Object:  StoredProcedure [dbo].[Sueetie_ForumMessage_Add]    Script Date: 05/18/2011 15:37:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--region [dbo].[Sueetie_beComment]


CREATE PROCEDURE  [dbo].[Sueetie_ForumMessage_Add]
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


Print '/****** Object:  View [dbo].[Sueetie_vw_UserYafAvatar]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_UserYafAvatar]
AS
SELECT     dbo.Sueetie_UserAvatar.UserID, dbo.Sueetie_UserAvatar.AvatarImage, dbo.Sueetie_UserAvatar.AvatarImageType
FROM         dbo.Sueetie_Users INNER JOIN
                      dbo.yaf_User ON dbo.Sueetie_Users.MembershipID = dbo.yaf_User.ProviderUserKey INNER JOIN
                      dbo.Sueetie_UserAvatar ON dbo.Sueetie_Users.UserID = dbo.Sueetie_UserAvatar.UserID
GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Users_Get]    Script Date: 02/02/2011 16:21:11 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_Users_Get] 
(
	@UserTypeID int
)
AS
BEGIN

SET NOCOUNT, XACT_ABORT ON

--InactiveUsers = -4,
--UnapprovedUsers = -3,
--AllUsers = -2,
--Anonymous = -1,
--RegisteredUser = 0

SELECT     dbo.Sueetie_Users.UserID, dbo.Sueetie_Users.MembershipID, dbo.Sueetie_Users.UserName as 'username', dbo.Sueetie_Users.Email, 
                      dbo.Sueetie_UserAvatar.AvatarImage, dbo.Sueetie_Users.DisplayName, 0 as 'AvatarRoot',
                      dbo.aspnet_Membership.CreateDate as 'DateJoined', -1 as 'ForumUserID', dbo.Sueetie_Users.IsActive,
                      dbo.Sueetie_Users.LastActivity,  dbo.aspnet_Membership.IsApproved, CONVERT(bit,0) as 'IsFiltered' 
                      into #tempUser
FROM         dbo.Sueetie_Users INNER JOIN
                      dbo.Sueetie_UserAvatar ON dbo.Sueetie_Users.UserID = dbo.Sueetie_UserAvatar.UserID INNER JOIN
                      dbo.aspnet_Users ON dbo.Sueetie_Users.MembershipID = dbo.aspnet_Users.UserId 
                      inner join aspnet_Membership on Sueetie_Users.MembershipID = aspnet_Membership.UserId

   
declare @username nvarchar(50)
select @username = username from #tempUser
               
update #tempUser set AvatarRoot = UserID
	where AvatarImage is not null


update #tempUser set ForumUserID = y.UserID from yaf_User y
	inner join #tempUser t on
	y.ProviderUserKey = t.MembershipID 
	where y.BoardID = 1

update #tempUser set IsFiltered = 1 where userid in (select userid from SuAnalytics_FilteredUsers)


declare @filterQuery nvarchar(200)
set @filterQuery = CASE
	WHEN @UserTypeID = -4 THEN 'where IsActive = 0 and IsApproved = 0'
	WHEN @UserTypeID = -3 THEN 'where IsActive = 1 and IsApproved = 0'
	WHEN @UserTypeID = -2 THEN ''
	WHEN @UserTypeID = -1 THEN 'where userid = -1'
	WHEN @UserTypeID = 0 THEN 'where IsActive = 1 and IsApproved = 1'
	END

set @filterQuery = 'select userid, membershipid, username, email, displayname, avatarroot, datejoined, lastactivity, forumuserid, isfiltered from #tempUser ' + @filterQuery
EXEC (@filterQuery)

drop table #tempUser 
                      
END

GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_Update]    Script Date: 10/15/2010 13:32:30 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[Sueetie_User_Update]
(
	@UserName nvarchar(50),
	@UserID int,
	@Email nvarchar(255),
	@MembershipID uniqueidentifier,
	@DisplayName nvarchar(255),
	@IsActive bit,
	@TimeZone int
)
AS
BEGIN

	update sueetie_users set isActive = @IsActive, email = @Email, displayname = @DisplayName where UserID = @UserID
	UPDATE aspnet_membership set email = @email, loweredemail = lower(@email) where UserId = @MembershipID

	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[yaf_User]'))
	begin
		update yaf_User set email =  @Email, TimeZone = @TimeZone where ProviderUserKey = @MembershipID
	end


	IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[be_Users]'))
	begin
		update be_Users set emailAddress =  @Email where UserName = @UserName
	end

END
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_GetForumID]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_User_GetForumID] 
(
	@boardID int,
	@username nvarchar(50),
	@UserForumID int OUTPUT
)
AS
BEGIN


declare @existingUserForumID int
select @existingUserForumID= userid from yaf_user where boardID = @boardID and name = @username 


if @existingUserForumID is not null

begin
select userid from yaf_user where boardID = @boardID and name = @username 
	SET @UserForumID = SCOPE_IDENTITY() 
end
else begin
	SET @UserForumID = -1
end

select @UserForumID

end
GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_getByUsername]    Script Date: 02/02/2011 16:19:58 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_User_getByUsername] 
(
	@username nvarchar(50)
)
AS
BEGIN

SET NOCOUNT, XACT_ABORT ON

SELECT     dbo.Sueetie_Users.UserID, dbo.Sueetie_Users.MembershipID, dbo.Sueetie_Users.UserName as 'username', dbo.Sueetie_Users.Email, 
                      dbo.Sueetie_UserAvatar.AvatarImage, dbo.Sueetie_UserAvatar.AvatarImageType, dbo.Sueetie_Users.DisplayName, 0 as 'AvatarRoot',
                      dbo.aspnet_Membership.CreateDate as 'DateJoined', dbo.Sueetie_Users.Bio, -1 as 'ForumUserID', dbo.Sueetie_Users.IsActive,
                      dbo.Sueetie_Users.LastActivity,  dbo.Sueetie_Users.IP, 0 as 'TimeZone', CONVERT(bit,0) as 'IsFiltered' 
                      into #tempUser
FROM         dbo.Sueetie_Users INNER JOIN
                      dbo.Sueetie_UserAvatar ON dbo.Sueetie_Users.UserID = dbo.Sueetie_UserAvatar.UserID INNER JOIN
                      dbo.aspnet_Users ON dbo.Sueetie_Users.MembershipID = dbo.aspnet_Users.UserId 
                      inner join aspnet_Membership on Sueetie_Users.MembershipID = aspnet_Membership.UserId
           where Sueetie_Users.UserName = @username
   
               
update #tempUser set AvatarRoot = UserID where AvatarImage is not null


update #tempUser set ForumUserID = y.UserID, TimeZone = y.TimeZone from yaf_User y
	where name = @username and BoardID = 1


declare @filteredUserID int
select @filteredUserID = filteredUserID from SuAnalytics_FilteredUsers s, #tempUser t where s.userID = t.userid
if @filteredUserID > 0
begin
	update #tempUser set IsFiltered = 1
end

select * from #tempUser
drop table #tempUser
                      
END                      

GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_getByID]    Script Date: 02/02/2011 16:18:58 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Sueetie_User_getByID] 
(
	@userid int
)
AS
BEGIN

SET NOCOUNT, XACT_ABORT ON

SELECT     dbo.Sueetie_Users.UserID, dbo.Sueetie_Users.MembershipID, dbo.Sueetie_Users.UserName as 'username', dbo.Sueetie_Users.Email, 
                      dbo.Sueetie_UserAvatar.AvatarImage, dbo.Sueetie_UserAvatar.AvatarImageType, dbo.Sueetie_Users.DisplayName, 0 as 'AvatarRoot',
                      dbo.aspnet_Membership.CreateDate as 'DateJoined', dbo.Sueetie_Users.Bio, -1 as 'ForumUserID', dbo.Sueetie_Users.IsActive,
                      dbo.Sueetie_Users.LastActivity,  dbo.Sueetie_Users.IP, 0 as 'TimeZone', CONVERT(bit,0) as 'IsFiltered' 
                      into #tempUser
FROM         dbo.Sueetie_Users INNER JOIN
                      dbo.Sueetie_UserAvatar ON dbo.Sueetie_Users.UserID = dbo.Sueetie_UserAvatar.UserID INNER JOIN
                      dbo.aspnet_Users ON dbo.Sueetie_Users.MembershipID = dbo.aspnet_Users.UserId 
                      inner join aspnet_Membership on Sueetie_Users.MembershipID = aspnet_Membership.UserId
           where Sueetie_Users.UserId = @userid
   
declare @username nvarchar(50)
select @username = username from #tempUser
               
update #tempUser set AvatarRoot = UserID where AvatarImage is not null

update #tempUser set ForumUserID = y.UserID, TimeZone = y.TimeZone from yaf_User y
	where name = @username and BoardID = 1


declare @filteredUserID int
select @filteredUserID = filteredUserID from SuAnalytics_FilteredUsers where userID = @userid
if @filteredUserID > 0
begin
	update #tempUser set IsFiltered = 1
end

select * from #tempUser
drop table #tempUser
                      
END                      


GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_getByForumID]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_User_getByForumID] 
(
	@userforumid int
)
AS
BEGIN

SET NOCOUNT, XACT_ABORT ON

DECLARE @userID int
DECLARE @username nvarchar(65)
select @username = name from yaf_User where UserID = @userforumid
select @userID = userid from Sueetie_Users where UserName = @username


SELECT     dbo.Sueetie_Users.UserID, dbo.Sueetie_Users.MembershipID, dbo.Sueetie_Users.UserName as 'username', dbo.Sueetie_Users.Email, 
                      dbo.Sueetie_UserAvatar.AvatarImage, dbo.Sueetie_UserAvatar.AvatarImageType, dbo.Sueetie_Users.DisplayName, 0 as 'AvatarRoot',
                      dbo.aspnet_Membership.CreateDate as 'DateJoined', dbo.Sueetie_Users.Bio, -1 as 'ForumUserID'
                      into #tempUser
FROM         dbo.Sueetie_Users INNER JOIN
                      dbo.Sueetie_UserAvatar ON dbo.Sueetie_Users.UserID = dbo.Sueetie_UserAvatar.UserID INNER JOIN
                      dbo.aspnet_Users ON dbo.Sueetie_Users.MembershipID = dbo.aspnet_Users.UserId 
                      inner join aspnet_Membership on Sueetie_Users.MembershipID = aspnet_Membership.UserId
           where Sueetie_Users.UserId = @userid

               
update #tempUser set AvatarRoot = UserID
	where AvatarImage is not null

update #tempUser set ForumUserID = y.UserID from yaf_User y
	where name = @username and BoardID = 1
	

select * from #tempUser
                      
END
GO



Print '/****** Object:  Table [dbo].[SuAnalytics_FilteredUsers]    Script Date: 02/11/2011 19:18:52 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SuAnalytics_FilteredUsers](
	[FilteredUserID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
 CONSTRAINT [PK_SuAnalytics_FilteredUsers] PRIMARY KEY CLUSTERED 
(
	[FilteredUserID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_User_getByEmail]    Script Date: 02/02/2011 16:16:07 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_User_getByEmail] 
(
	@email nvarchar(150)
)
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

SELECT     dbo.Sueetie_Users.UserID, dbo.Sueetie_Users.MembershipID, dbo.Sueetie_Users.UserName as 'username', dbo.Sueetie_Users.Email, 
                      dbo.Sueetie_UserAvatar.AvatarImage, dbo.Sueetie_UserAvatar.AvatarImageType, dbo.Sueetie_Users.DisplayName, 0 as 'AvatarRoot',
                      dbo.aspnet_Membership.CreateDate as 'DateJoined', dbo.Sueetie_Users.Bio, -1 as 'ForumUserID', dbo.Sueetie_Users.IsActive,
                      dbo.Sueetie_Users.LastActivity,  dbo.Sueetie_Users.IP, 0 as 'TimeZone', CONVERT(bit,0) as 'IsFiltered'  
                      into #tempUser
FROM         dbo.Sueetie_Users INNER JOIN
                      dbo.Sueetie_UserAvatar ON dbo.Sueetie_Users.UserID = dbo.Sueetie_UserAvatar.UserID INNER JOIN
                      dbo.aspnet_Users ON dbo.Sueetie_Users.MembershipID = dbo.aspnet_Users.UserId 
                      inner join aspnet_Membership on Sueetie_Users.MembershipID = aspnet_Membership.UserId
           where Sueetie_Users.Email = @email
                
declare @username nvarchar(50)
select @username = username from #tempUser
               
update #tempUser set AvatarRoot = UserID where AvatarImage is not null

update #tempUser set ForumUserID = y.UserID, TimeZone = y.TimeZone from yaf_User y
	where name = @username and BoardID = 1

declare @filteredUserID int
select @filteredUserID = filteredUserID from SuAnalytics_FilteredUsers s, #tempUser t where s.userID = t.userid
if @filteredUserID > 0
begin
	update #tempUser set IsFiltered = 1
end

	
select * from #tempUser
drop table #tempUser
                      
END

                      
SET ANSI_NULLS ON

GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_UnreadPMs_Get]    Script Date: 11/11/2010 13:51:33 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_UnreadPMs_Get] 
	@UserName varchar(max) = ''
AS
BEGIN

	SET NOCOUNT ON;

	SELECT PMessageID, FromUserID, FromUser, Created, Subject
		FROM [dbo].[yaf_PMessageView]
		WHERE (LOWER(ToUser) = LOWER(@UserName) AND IsRead = 0 AND IsDeleted = 0 AND IsArchived = 0)
		ORDER BY Created DESC
END

GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Role_Add]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_Role_Add]
	@RoleID uniqueidentifier,
	@RoleName nvarchar(256),
	@IsGroupAdminRole bit,
	@IsGroupUserRole bit,
	@IsBlogOwnerRole bit	
 
AS
BEGIN

SET NOCOUNT ON

INSERT INTO [dbo].[Sueetie_Roles] (
	[RoleID],
	[RoleName],
	[IsGroupAdminRole],
	[IsGroupUserRole],
	[IsBlogOwnerRole],	
	[IsLocked]
) VALUES (
	@RoleID,
	@RoleName,
	@IsGroupAdminRole,
	@IsGroupUserRole,
	@IsBlogOwnerRole,
	0
)

INSERT INTO [dbo].[gs_Role] select @RoleName, galleryID, 0,0,0,0,0,0,0,0,0,0,0 from gs_Gallery
END

GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaAlbum_Update]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_MediaAlbum_Update]
	@AlbumID int,
	@AlbumTitle nvarchar(200),
	@AlbumDescription nvarchar(50)
AS

SET NOCOUNT ON

UPDATE [dbo].[Sueetie_gs_Album] SET
	[AlbumDescription] = @AlbumDescription
WHERE
	[AlbumID] = @AlbumID

UPDATE [dbo].[gs_Album] SET
	[Title] = @AlbumTitle
WHERE
	[AlbumID] = @AlbumID

GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaUpdates_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_MediaUpdates_Get]
(
	@AlbumId int
)
AS
SET NOCOUNT ON

if (@AlbumId > 0) begin

SELECT     m.MediaObjectId, m.FKAlbumId as 'AlbumID', u.UserID AS SueetieUserID
FROM         dbo.gs_Album a INNER JOIN
                      dbo.gs_MediaObject m ON a.AlbumId = m.FKAlbumId INNER JOIN
                      dbo.Sueetie_Users u ON m.CreatedBy = u.UserName
WHERE FKAlbumID = @AlbumId and 
                    m.MediaObjectId not in (select sourceid from Sueetie_Content where ContentTypeID in (6, 7, 8, 9, 10))
end
else begin
SELECT     m.MediaObjectId, m.FKAlbumId as 'AlbumID', u.UserID AS SueetieUserID
FROM         dbo.gs_Album a INNER JOIN
                      dbo.gs_MediaObject m ON a.AlbumId = m.FKAlbumId INNER JOIN
                      dbo.Sueetie_Users u ON m.CreatedBy = u.UserName
WHERE   m.MediaObjectId not in (select sourceid from Sueetie_Content where ContentTypeID in (6, 7, 8, 9, 10))
end

RETURN
GO


Print '/****** Object:  UserDefinedFunction [dbo].[ConcatMediaAlbumTags]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConcatMediaAlbumTags](@albumID int)
RETURNS VARCHAR(2000)
AS
BEGIN
	DECLARE @Output VARCHAR(2000)
	SELECT @Output = COALESCE(@Output+'|', '') + CONVERT(varchar(200), t.tag)
	FROM	dbo.[sueetie_gs_AlbumTags] t
		JOIN dbo.gs_album a
			ON t.AlbumID = @albumID
			and a.AlbumID = @albumID
	ORDER BY t.tag

	RETURN @Output
END
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaAlbumTags_CreateUpdate]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_MediaAlbumTags_CreateUpdate]
	@ContentID int,
	@ItemID int,
	@ContentTypeID int,
	@UserID int,
	@Tags nvarchar(500)
AS


SET NOCOUNT ON

delete from Sueetie_gs_AlbumTags where AlbumID = @ItemID
delete from Sueetie_Tags where ContentID = @ContentID

declare @tagMasterID int
declare @tagID int
declare @tag nvarchar(50)

declare tagCursor cursor for select item from  fnSplit(@tags,'|')

open tagCursor

fetch next from tagCursor into @tag
while @@fetch_status = 0
begin

	insert into Sueetie_gs_AlbumTags (AlbumID, Tag) values (@ItemID, @tag)
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

update gs_Album set DateLastModified = GETDATE(), LastModifiedBy = @UserID where AlbumId = @ItemID
GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaObjectTags_CreateUpdate]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_MediaObjectTags_CreateUpdate]
	@ContentID int,
	@ItemID int,
	@ContentTypeID int,
	@UserID int,
	@Tags nvarchar(500)
AS


SET NOCOUNT ON

delete from Sueetie_gs_MediaObjectTags where MediaObjectID = @ItemID
delete from Sueetie_Tags where ContentID = @ContentID

declare @tagMasterID int
declare @tagID int
declare @tag nvarchar(50)

declare tagCursor cursor for select item from  fnSplit(@tags,'|')

open tagCursor

fetch next from tagCursor into @tag
while @@fetch_status = 0
begin

	insert into Sueetie_gs_MediaObjectTags (MediaObjectID, Tag) values (@ItemID, @tag)
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

update gs_MediaObject set DateLastModified = GETDATE(), LastModifiedBy = @UserID where MediaObjectId = @ItemID
GO


Print '/****** Object:  UserDefinedFunction [dbo].[ConcatMediaObjectTags]    Script Date: 10/15/2010 13:32:35 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ConcatMediaObjectTags](@mediaobjectid int)
RETURNS VARCHAR(2000)
AS
BEGIN
	DECLARE @Output VARCHAR(2000)
	SELECT @Output = COALESCE(@Output+'|', '') + CONVERT(varchar(200), t.tag)
	FROM	dbo.[sueetie_gs_MediaObjectTags] t
		JOIN dbo.gs_mediaobject g
			ON t.MediaObjectID = @mediaobjectid
			and g.MediaObjectID = @mediaobjectid
	ORDER BY t.tag

	RETURN @Output
END
GO


Print '/****** Object:  View [dbo].[Sueetie_vw_MediaAlbums]    Script Date: 01/11/2011 15:22:00 ******/'

/****** Object:  View [dbo].[Sueetie_vw_MediaAlbums]    Script Date: 05/24/2011 20:28:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Sueetie_vw_MediaAlbums]
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




Print '/****** Object:  View [dbo].[Sueetie_vw_Downloads]    Script Date: 10/15/2010 13:32:37 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Sueetie_vw_Downloads]
AS
SELECT     dbo.Sueetie_Downloads.DownloadID, dbo.Sueetie_Downloads.ContentID, dbo.Sueetie_Content.ContentTypeID, 
                      dbo.Sueetie_ContentTypes.Description AS ContentTypeDescription, dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Applications.ApplicationKey, 
                      dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Groups.GroupKey, dbo.Sueetie_Groups.GroupName, dbo.Sueetie_gs_MediaObject.MediaObjectID, 
                      dbo.gs_MediaObject.Title AS MediaObjectTitle, dbo.gs_Album.AlbumId AS AlbumID, dbo.gs_Album.Title AS AlbumTitle, dbo.gs_Gallery.GalleryId AS GalleryID, 
                      dbo.gs_Gallery.Description AS GalleryName, dbo.Sueetie_Users.UserID AS DownloadUserID, dbo.Sueetie_Users.UserName AS DownloadUserName, 
                      dbo.Sueetie_Users.DisplayName AS DownloadDisplayName, dbo.Sueetie_Users.Email AS DownloadEmail, dbo.Sueetie_Downloads.DownloadDateTime, 
                      dbo.Sueetie_gs_MediaObject.InDownloadReport, 0 AS TotalDownloads, '6/9/1969' AS DateTimeLastDownload
FROM         dbo.Sueetie_Applications INNER JOIN
                      dbo.gs_MediaObject INNER JOIN
                      dbo.Sueetie_gs_MediaObject ON dbo.gs_MediaObject.MediaObjectId = dbo.Sueetie_gs_MediaObject.MediaObjectID INNER JOIN
                      dbo.gs_Album ON dbo.gs_MediaObject.FKAlbumId = dbo.gs_Album.AlbumId INNER JOIN
                      dbo.gs_Gallery ON dbo.gs_Album.FKGalleryId = dbo.gs_Gallery.GalleryId INNER JOIN
                      dbo.Sueetie_Users INNER JOIN
                      dbo.Sueetie_Downloads INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_Downloads.ContentID = dbo.Sueetie_Content.ContentID ON dbo.Sueetie_Users.UserID = dbo.Sueetie_Downloads.UserID ON 
                      dbo.Sueetie_gs_MediaObject.MediaObjectID = dbo.Sueetie_Content.SourceID ON 
                      dbo.Sueetie_Applications.ApplicationID = dbo.Sueetie_Content.ApplicationID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_ContentTypes ON dbo.Sueetie_Content.ContentTypeID = dbo.Sueetie_ContentTypes.ContentTypeID
GO



Print '/****** Object:  View [dbo].[Sueetie_vw_MediaObjects]    Script Date: 12/03/2010 16:24:53 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[Sueetie_vw_MediaObjects]
AS
SELECT     dbo.gs_MediaObject.MediaObjectId, dbo.gs_Album.AlbumId, dbo.Sueetie_Content.ContentID, dbo.Sueetie_Content.SourceID, dbo.Sueetie_Content.ContentTypeID, 
                      dbo.Sueetie_ContentTypes.Description AS ContentTypeDescription, dbo.Sueetie_Content.UserID AS SueetieUserID, dbo.Sueetie_Content.Permalink, 
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
                       0 AS ThumbnailWidth, CONVERT(bit, 0) AS IsImage, dbo.ConcatMediaObjectTags(dbo.gs_MediaObject.MediaObjectId) AS Tags
FROM         dbo.Sueetie_Applications INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_Applications.ApplicationID = dbo.Sueetie_Content.ApplicationID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_Users ON dbo.Sueetie_Content.UserID = dbo.Sueetie_Users.UserID INNER JOIN
                      dbo.gs_MediaObject ON dbo.Sueetie_Content.SourceID = dbo.gs_MediaObject.MediaObjectId INNER JOIN
                      dbo.gs_Album ON dbo.gs_MediaObject.FKAlbumId = dbo.gs_Album.AlbumId INNER JOIN
                      dbo.gs_Gallery ON dbo.gs_Album.FKGalleryId = dbo.gs_Gallery.GalleryId INNER JOIN
                      dbo.Sueetie_gs_MediaObject ON dbo.gs_MediaObject.MediaObjectId = dbo.Sueetie_gs_MediaObject.MediaObjectID INNER JOIN
                      dbo.Sueetie_ContentTypes ON dbo.Sueetie_Content.ContentTypeID = dbo.Sueetie_ContentTypes.ContentTypeID INNER JOIN
                      dbo.Sueetie_gs_Bibliography ON dbo.gs_MediaObject.MediaObjectId = dbo.Sueetie_gs_Bibliography.MediaObjectID

GO



Print '/****** Object:  StoredProcedure [dbo].[Sueetie_Downloads_Get]    Script Date: 10/15/2010 13:32:29 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--[Sueetie_Downloads_Get] 1, -1, -1, -1, -1, 1000, -1

        
CREATE PROCEDURE [dbo].[Sueetie_Downloads_Get]
(
	@contentViewTypeID int,
	@userID int,
	@applicationID int,
	@sourceID int,
	@groupID int,
	@numRecords int,
	@sortBy int
)
AS
SET NOCOUNT, XACT_ABORT ON


	set rowcount  @numRecords

CREATE TABLE #z_sueetie_vw_downloads(
	[DownloadID] [int] NULL,
	[ContentID] [int] NULL,
	[ContentTypeID] [int] NULL,
	[ContentTypeDescription] [nvarchar](255) NULL,
	[ApplicationID] [int] NULL,
	[ApplicationKey] [nvarchar](25) NULL,
	[GroupID] [int] NULL,
	[GroupKey] [nvarchar](25) NULL,
	[GroupName] [nvarchar](255) NULL,
	[MediaObjectID] [int] NULL,
	[MediaObjectTitle] [nvarchar](1000) NULL,
	[AlbumID] [int] NULL,
	[AlbumTitle] [nvarchar](200) NULL,
	[GalleryID] [int] NULL,
	[GalleryName] [nvarchar](100) NULL,
	[DownloadUserID] [int] NULL,
	[DownloadUserName] [nvarchar](50) NULL,
	[DownloadDisplayName] [nvarchar](150) NULL,
	[DownloadEmail] [nvarchar](255) NULL,
	[DownloadDateTime] [smalldatetime] NULL,
	[InDownloadReport] [bit] NULL,
	[TotalDownloads] [int] NULL,
	[DateTimeLastDownload] [smalldatetime] NULL,
 )

if @contentViewTypeID = 10
begin
 
insert into #z_sueetie_vw_downloads 	
SELECT     0, 0, dbo.Sueetie_Content.ContentTypeID, 
                      dbo.Sueetie_ContentTypes.Description AS ContentTypeDescription, dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Applications.ApplicationKey, 
                      dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Groups.GroupKey, 'na', dbo.Sueetie_gs_MediaObject.MediaObjectID, 
                      dbo.gs_MediaObject.Title AS MediaObjectTitle, dbo.gs_Album.AlbumId, dbo.gs_Album.Title AS AlbumTitle, dbo.gs_Gallery.GalleryId, 
                      dbo.gs_Gallery.Description AS GalleryName, -1, 'anonymous', 'na', 'na', getdate(), 1, COUNT(*) as 'TotalDownloads', MAX(DownloadDateTime) as 'DateTimeLastDownload'
FROM         dbo.Sueetie_Applications INNER JOIN
                      dbo.gs_MediaObject INNER JOIN
                      dbo.Sueetie_gs_MediaObject ON dbo.gs_MediaObject.MediaObjectId = dbo.Sueetie_gs_MediaObject.MediaObjectID INNER JOIN
                      dbo.gs_Album ON dbo.gs_MediaObject.FKAlbumId = dbo.gs_Album.AlbumId INNER JOIN
                      dbo.gs_Gallery ON dbo.gs_Album.FKGalleryId = dbo.gs_Gallery.GalleryId INNER JOIN
                      dbo.Sueetie_Downloads INNER JOIN
                      dbo.Sueetie_Content ON dbo.Sueetie_Downloads.ContentID = dbo.Sueetie_Content.ContentID ON 
                      dbo.Sueetie_gs_MediaObject.MediaObjectID = dbo.Sueetie_Content.SourceID ON 
                      dbo.Sueetie_Applications.ApplicationID = dbo.Sueetie_Content.ApplicationID INNER JOIN
                      dbo.Sueetie_Groups ON dbo.Sueetie_Applications.GroupID = dbo.Sueetie_Groups.GroupID INNER JOIN
                      dbo.Sueetie_ContentTypes ON dbo.Sueetie_Content.ContentTypeID = dbo.Sueetie_ContentTypes.ContentTypeID
                      where dbo.Sueetie_gs_MediaObject.InDownloadReport = 1
                      	and dbo.Sueetie_Applications.ApplicationID = CASE WHEN @applicationID > -1 then @applicationID ELSE dbo.Sueetie_Applications.ApplicationID END
						and dbo.Sueetie_Applications.GroupID = CASE WHEN @groupID > -1 then @groupID ELSE dbo.Sueetie_Applications.GroupID END
                     group by  dbo.Sueetie_gs_MediaObject.MediaObjectID, dbo.Sueetie_Content.ContentTypeID, 
                      dbo.Sueetie_ContentTypes.Description, dbo.Sueetie_Content.ApplicationID, dbo.Sueetie_Applications.ApplicationKey, 
                      dbo.Sueetie_Applications.GroupID, dbo.Sueetie_Groups.GroupKey, 
                      dbo.gs_MediaObject.Title, dbo.gs_Album.AlbumId, dbo.gs_Album.Title, dbo.gs_Gallery.GalleryId, 
                      dbo.gs_Gallery.Description

end
else
begin
            
            
insert into #z_sueetie_vw_downloads 	
SELECT    * from sueetie_vw_downloads
                      where InDownloadReport = 1
						and DownloadUserID = CASE WHEN @userid > -1 then @userID ELSE DownloadUserID END
                      	and ApplicationID = CASE WHEN @applicationID > -1 then @applicationID ELSE ApplicationID END
						and GroupID = CASE WHEN @groupID > -1 then @groupID ELSE GroupID END
						and MediaObjectID = CASE WHEN @sourceID > -1 then @sourceID ELSE MediaObjectID END

end

        --NA = -1,
        --MostRecentDateTimeDesc = 1,
        --TotalCountDesc = 2,
        --ItemTitle = 3,
        --UserName = 4

select * from #z_sueetie_vw_downloads order by 
	CASE WHEN @sortby = -1 THEN TotalDownloads END desc 
	,CASE WHEN @sortby = 1 THEN DownloadDateTime END desc
	,CASE WHEN @sortby = 1 THEN TotalDownloads END desc

drop table #z_sueetie_vw_downloads

	set RowCount 0
GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_gs_RoleUpdate]    Script Date: 01/11/2011 13:47:47 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_gs_RoleUpdate] 
(
	@galleryID int
)
AS
SET NOCOUNT, XACT_ABORT ON

	UPDATE [gs_Role]
	SET AllowViewAlbumsAndObjects = 1,
		AllowViewOriginalImage = 1,
		AllowAddChildAlbum = 1,
		AllowAddMediaObject = 1,
		AllowEditAlbum = 1,
		AllowEditMediaObject = 1,
		AllowDeleteChildAlbum = 1,
		AllowDeleteMediaObject = 1,
		AllowSynchronize = 1,
		HideWatermark = 1, 
		AllowAdministerSite = 1
	WHERE RoleName in ('MediaAdministrator', 'SueetieAdministrator')
	
		UPDATE [gs_Role]
	SET AllowViewAlbumsAndObjects = 1,
		AllowViewOriginalImage = 1
	WHERE RoleName = 'Registered'
	
RETURN

select * from gs_Role
GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_yaf_user_AddAdmin]    Script Date: 10/15/2010 22:29:04 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_yaf_user_AddAdmin]
	@boardID int
AS
BEGIN

	SET NOCOUNT ON;

insert into yaf_Group (BoardID, Name, Flags) values (@boardID, 'ForumAdministrator', 1)


END
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_gs_SelectChildMediaObjects]    Script Date: 10/15/2010 22:29:04 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_gs_SelectChildMediaObjects]
(
	@AlbumId int,
	@RecentPhotoCount int
)
AS
SET NOCOUNT ON

if (@AlbumId = -1 ) begin
SELECT top (@RecentPhotoCount) 
	MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, 
	ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, 
	OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, 
	CreatedBy, DateAdded, LastModifiedBy, DateLastModified, IsPrivate
FROM [gs_MediaObject]
where IsPrivate = 0
order by DateAdded desc

end
else begin

SELECT 
	MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, 
	ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, 
	OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, 
	CreatedBy, DateAdded, LastModifiedBy, DateLastModified, IsPrivate
FROM [gs_MediaObject]
WHERE FKAlbumID = @AlbumId
end

RETURN
GO
Print '/****** Object:  StoredProcedure [dbo].[Sueetie_yaf_user_aspnet]    Script Date: 10/15/2010 22:29:04 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sueetie_yaf_user_aspnet]
(
@BoardID int,
@UserName nvarchar(255),
@DisplayName nvarchar(255),
@Email nvarchar(255),
@ProviderUserKey nvarchar(64),
@IsApproved bit,
@TimeZone int
) 
as
BEGIN
	SET NOCOUNT ON

	DECLARE @UserID int, @RankID int, @approvedFlag int

	SET @approvedFlag = 0;
	IF (@IsApproved = 1) SET @approvedFlag = 2;	
	
	IF EXISTS(SELECT 1 FROM [dbo].[yaf_User] where BoardID=@BoardID and ([ProviderUserKey]=@ProviderUserKey OR [Name] = @UserName))
	BEGIN
		SELECT TOP 1 @UserID = UserID FROM [dbo].[yaf_User] WHERE [BoardID]=@BoardID and ([ProviderUserKey]=@ProviderUserKey OR [Name] = @UserName)
		
		IF (@DisplayName IS NULL) 
		BEGIN
			SELECT TOP 1 @DisplayName = DisplayName FROM [dbo].[yaf_User] WHERE UserId = @UserID
		END

		UPDATE [dbo].[yaf_User] SET 
			[Name] = @UserName,
			DisplayName = @DisplayName,
			Email = @Email,
			[ProviderUserKey] = @ProviderUserKey,
			Flags = Flags | @approvedFlag
		WHERE
			UserID = @UserID
	END ELSE
	BEGIN
		SELECT @RankID = RankID from [dbo].[yaf_Rank] where (Flags & 1)<>0 and BoardID=@BoardID
		
		IF (@DisplayName IS NULL) 
		BEGIN
			SET @DisplayName = @UserName
		END		

		INSERT INTO [dbo].[yaf_User](BoardID,RankID,[Name],DisplayName,Password,Email,Joined,LastVisit,NumPosts,TimeZone,Flags,ProviderUserKey) 
		VALUES(@BoardID,@RankID,@UserName,@DisplayName,'-',@Email,GETUTCDATE() ,GETUTCDATE() ,0,0,@approvedFlag,@ProviderUserKey)
	
		SET @UserID = SCOPE_IDENTITY()	
	END
	
	SELECT UserID=@UserID
END
GO

/* -----------------  Added from Upgrade Scripts 1/11/11 ------------------------------ */


Print '/****** Object:  StoredProcedure [dbo].[gs_sueetie_SelectChildMediaObjects]    Script Date: 12/02/2010 11:23:49 ******/'
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[gs_sueetie_SelectChildMediaObjects]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[gs_sueetie_SelectChildMediaObjects]
GO


Print '/****** Object:  StoredProcedure [dbo].[gs_sueetie_SelectChildMediaObjects]    Script Date: 12/02/2010 11:23:49 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[gs_sueetie_SelectChildMediaObjects]
(
	@AlbumId int,
	@RecentPhotoCount int
)
AS
SET NOCOUNT ON

if (@AlbumId = -1 ) begin
SELECT top (@RecentPhotoCount) 
	MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, 
	ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, 
	OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, 
	CreatedBy, DateAdded, LastModifiedBy, DateLastModified, IsPrivate
FROM [gs_MediaObject]
where IsPrivate = 0
order by DateAdded desc

end
else begin

SELECT 
	MediaObjectID, FKAlbumID, Title, HashKey, ThumbnailFilename, ThumbnailWidth, ThumbnailHeight, 
	ThumbnailSizeKB, OptimizedFilename, OptimizedWidth, OptimizedHeight, OptimizedSizeKB, 
	OriginalFilename, OriginalWidth, OriginalHeight, OriginalSizeKB, ExternalHtmlSource, ExternalType, Seq, 
	CreatedBy, DateAdded, LastModifiedBy, DateLastModified, IsPrivate
FROM [gs_MediaObject]
WHERE FKAlbumID = @AlbumId
end

RETURN

GO


Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaGalleries_Get]    Script Date: 12/07/2010 17:24:53 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_MediaGalleries_Get] 
AS
SET NOCOUNT, XACT_ABORT ON


CREATE TABLE [dbo].[#z_Sueetie_Gallery](
	[GalleryId] [int] NOT NULL,
	[GalleryKey] [nvarchar](25) NULL,
	[GalleryTitle] [nvarchar](1000) NOT NULL,
	[GalleryDescription] [nvarchar](max) NULL,
	[DateAdded] [datetime] NOT NULL,
	[DisplayTypeID] [int] NOT NULL,
	[DisplayTypeDescription] [nvarchar](50) NULL,
	[ApplicationID] [int] NOT NULL,
	[ApplicationKey] [nvarchar](25) NOT NULL,
	[ApplicationDescription] [nvarchar](1000) NOT NULL,
	[IsPublic] [bit] NOT NULL,
	[IsLogged] [bit] NOT NULL,
	[MediaObjectPath] [nvarchar](255) NULL
) 


insert into #z_Sueetie_Gallery
SELECT     g.GalleryId, s.GalleryKey, g.Description AS GalleryTitle, s.GalleryDescription, g.DateAdded, 
                      s.DisplayTypeID, d.DisplayTypeDescription, s.ApplicationID, 
                      a.ApplicationKey, a.Description AS ApplicationDescription, s.IsPublic, 
                      s.IsLogged, replicate(' ',255) AS MediaObjectPath
FROM         dbo.gs_Gallery g INNER JOIN
                      dbo.Sueetie_gs_Gallery s ON g.GalleryId = s.GalleryID INNER JOIN
                      dbo.Sueetie_DisplayTypes d ON s.DisplayTypeID = d.DisplayTypeID INNER JOIN
                      dbo.Sueetie_Applications a ON s.ApplicationID = a.ApplicationID 
                      and g.GalleryId > 0

update #z_Sueetie_Gallery set MediaObjectPath = 
(select  SettingValue from gs_GallerySetting where FKGalleryID = #z_Sueetie_Gallery.GalleryId
	and SettingName = 'MediaObjectPath')
	
	select * from  #z_Sueetie_Gallery
	
	drop table #z_Sueetie_Gallery
	

GO

Print '/****** Object:  StoredProcedure [dbo].[Sueetie_MediaGallery_AdminUpdate]    Script Date: 12/07/2010 17:25:34 ******/'
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Sueetie_MediaGallery_AdminUpdate] 
(
	@galleryID int,
	@galleryKey nvarchar(60),
	@displayTypeID int,
	@isPublic bit,
	@isLogged bit
)
AS
SET NOCOUNT, XACT_ABORT ON

update Sueetie_gs_Gallery set 
	DisplayTypeID = @displayTypeID, 
	galleryKey = @galleryKey, 
	isPublic = @isPublic,
	isLogged = @isLogged
where GalleryID = @galleryID




GO


Print '/****** Object:  StoredProcedure [dbo].[yaf_sueetie_user_list]    Script Date: 12/29/2010 13:15:19 ******/'
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[yaf_sueetie_user_list](@BoardID int,@UserID int=null,@Approved bit=null,@GroupID int=null,@RankID int=null,@StyledNicks bit = null) as
begin	
	if @UserID is not null
		 select 
			a.*,
			a.NumPosts,
			CultureUser = a.Culture,			
			b.RankID,						
			RankName = b.Name,
			Style = case(@StyledNicks)
			when 1 then  ISNULL(( SELECT TOP 1 f.Style FROM [dbo].[yaf_UserGroup] e 
			join [dbo].[yaf_Group] f on f.GroupID=e.GroupID WHERE e.UserID=a.UserID AND LEN(f.Style) > 2 ORDER BY f.SortOrder), b.Style)  
			else ''	 end, 
			NumDays = datediff(d,a.Joined,GETUTCDATE() )+1,
			NumPostsForum = (select count(1) from [dbo].[yaf_Message] x where (x.Flags & 24)=16),
			HasAvatarImage = (select count(1) from [dbo].[yaf_User] x where x.UserID=a.UserID and AvatarImage is not null),
			IsAdmin	= convert(bit, IsNull(c.IsAdmin,0)),
			IsGuest	= IsNull(a.Flags & 4,0),
			IsHostAdmin	= IsNull(a.Flags & 1,0),
			IsForumModerator	= convert(bit,IsNull(c.IsForumModerator,0)),
			IsModerator		= convert(bit,IsNull(c.IsModerator,0))
		from 
			[dbo].[yaf_User] a
			join [dbo].[yaf_Rank] b on b.RankID=a.RankID			
			left join [dbo].[yaf_vaccess] c on c.UserID=a.UserID
		where 
			a.UserID = @UserID and
			a.BoardID = @BoardID and
			IsNull(c.ForumID,0) = 0 and
			(@Approved is null or (@Approved=0 and (a.Flags & 2)=0) or (@Approved=1 and (a.Flags & 2)=2))
		order by 
			a.Name 
	else if @GroupID is null and @RankID is null
		select 
			a.*,
			a.NumPosts,
			CultureUser = a.Culture,	
			Style = case(@StyledNicks)
			when 1 then  ISNULL(( SELECT TOP 1 f.Style FROM [dbo].[yaf_UserGroup] e 
			join [dbo].[yaf_Group] f on f.GroupID=e.GroupID WHERE e.UserID=a.UserID AND LEN(f.Style) > 2 ORDER BY f.SortOrder), b.Style)  
			else ''	 end, 	
			IsAdmin = convert(bit,(select count(1) from [dbo].[yaf_UserGroup] x join [dbo].[yaf_Group] y on y.GroupID=x.GroupID where x.UserID=a.UserID and (y.Flags & 1)<>0)),
			IsGuest	= IsNull(a.Flags & 4,0),
			IsHostAdmin	= IsNull(a.Flags & 1,0),
			b.RankID,
			RankName = b.Name
		from 
			[dbo].[yaf_User] a
			join [dbo].[yaf_Rank] b on b.RankID=a.RankID			
		where 
			a.BoardID = @BoardID and
			(@Approved is null or (@Approved=0 and (a.Flags & 2)=0) or (@Approved=1 and (a.Flags & 2)=2))
		order by 
			a.Name
	else
		select 
			a.*,
			a.NumPosts,
			CultureUser = a.Culture,
			IsAdmin = convert(bit,(select count(1) from [dbo].[yaf_UserGroup] x join [dbo].[yaf_Group] y on y.GroupID=x.GroupID where x.UserID=a.UserID and (y.Flags & 1)<>0)),
			IsGuest	= IsNull(a.Flags & 4,0),
			IsHostAdmin	= IsNull(a.Flags & 1,0),
			b.RankID,
			RankName = b.Name,
			Style = case(@StyledNicks)
			when 1 then  ISNULL(( SELECT TOP 1 f.Style FROM [dbo].[yaf_UserGroup] e 
			join [dbo].[yaf_Group] f on f.GroupID=e.GroupID WHERE e.UserID=a.UserID AND LEN(f.Style) > 2 ORDER BY f.SortOrder), b.Style)  
			else ''	 end 
		from 
			[dbo].[yaf_User] a
			join [dbo].[yaf_Rank] b on b.RankID=a.RankID			
		where 
			a.BoardID = @BoardID and
			(@Approved is null or (@Approved=0 and (a.Flags & 2)=0) or (@Approved=1 and (a.Flags & 2)=2)) and
			(@GroupID is null or exists(select 1 from [dbo].[yaf_UserGroup] x where x.UserID=a.UserID and x.GroupID=@GroupID)) and
			(@RankID is null or a.RankID=@RankID)
		order by 
			a.Name
end
GO
