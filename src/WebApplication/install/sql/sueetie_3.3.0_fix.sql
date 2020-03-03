ALTER PROCEDURE [dbo].[Sueetie_User_getByID]
(
	@userid int
)
AS
BEGIN
SET NOCOUNT, XACT_ABORT ON

SELECT
	dbo.Sueetie_Users.UserID,
	dbo.Sueetie_Users.MembershipID,
	dbo.Sueetie_Users.UserName as 'username',
	dbo.Sueetie_Users.Email, 
	dbo.Sueetie_UserAvatar.AvatarImage,
	dbo.Sueetie_UserAvatar.AvatarImageType,
	dbo.Sueetie_Users.DisplayName,
	0 as 'AvatarRoot',
    dbo.aspnet_Membership.CreateDate as 'DateJoined',
	dbo.Sueetie_Users.Bio,
	-1 as 'ForumUserID',
	dbo.Sueetie_Users.IsActive,
	dbo.Sueetie_Users.LastActivity,
	dbo.Sueetie_Users.IP,
	0 as 'TimeZone',
	CONVERT(bit,0) as 'IsFiltered' 
into
	#tempUser
FROM
	dbo.Sueetie_Users
	INNER JOIN dbo.Sueetie_UserAvatar ON dbo.Sueetie_Users.UserID = dbo.Sueetie_UserAvatar.UserID
	INNER JOIN dbo.aspnet_Users ON dbo.Sueetie_Users.MembershipID = dbo.aspnet_Users.UserId 
	inner join aspnet_Membership on Sueetie_Users.MembershipID = aspnet_Membership.UserId
where
	Sueetie_Users.UserId = @userid
   
declare @username nvarchar(50)
select @username = username from #tempUser
update #tempUser set AvatarRoot = UserID where AvatarImage is not null
update #tempUser set ForumUserID = y.UserID from yaf_User y	where name = @username and BoardID = 1

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

ALTER PROCEDURE [dbo].[Sueetie_User_getByUsername] 
(
	@username nvarchar(50)
)
AS
BEGIN

SET NOCOUNT, XACT_ABORT ON

SELECT
	dbo.Sueetie_Users.UserID,
	dbo.Sueetie_Users.MembershipID,
	dbo.Sueetie_Users.UserName as 'username',
	dbo.Sueetie_Users.Email,
	dbo.Sueetie_UserAvatar.AvatarImage,
	dbo.Sueetie_UserAvatar.AvatarImageType,
	dbo.Sueetie_Users.DisplayName, 0 as 'AvatarRoot',
	dbo.aspnet_Membership.CreateDate as 'DateJoined',
	dbo.Sueetie_Users.Bio,
	-1 as 'ForumUserID',
	dbo.Sueetie_Users.IsActive,
	dbo.Sueetie_Users.LastActivity,
	dbo.Sueetie_Users.IP, 0 as 'TimeZone',
	CONVERT(bit,0) as 'IsFiltered' 
into #tempUser
FROM
	dbo.Sueetie_Users
	INNER JOIN dbo.Sueetie_UserAvatar ON dbo.Sueetie_Users.UserID = dbo.Sueetie_UserAvatar.UserID
	INNER JOIN dbo.aspnet_Users ON dbo.Sueetie_Users.MembershipID = dbo.aspnet_Users.UserId
	inner join aspnet_Membership on Sueetie_Users.MembershipID = aspnet_Membership.UserId
where
	Sueetie_Users.UserName = @username
           
update #tempUser set AvatarRoot = UserID where AvatarImage is not null
update #tempUser set ForumUserID = y.UserID from yaf_User y	where name = @username and BoardID = 1
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

ALTER PROCEDURE [dbo].[Sueetie_User_getByEmail] 
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

update #tempUser set ForumUserID = y.UserID from yaf_User y	where name = @username and BoardID = 1

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

