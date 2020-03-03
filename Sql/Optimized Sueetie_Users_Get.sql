USE [Coldwire Pathfinder-Sueetie]
GO
/****** Object:  StoredProcedure [dbo].[Sueetie_Users_Get]    Script Date: 11/09/2011 15:18:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[Sueetie_Users_Get] 
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

select
	u.UserID,
	u.MembershipID,
	u.UserName as [username],
	u.Email,
	a.AvatarImage,
	u.DisplayName,
	(case when a.AvatarImage is null then u.UserID else 0 end) as [AvatarRoot],
	m.CreateDate as [DateJoined],
	ISNULL(yu.UserID, -1) as [ForumUserID],
	u.IsActive,
	u.LastActivity,
	m.IsApproved,
	cast(case when f.UserID IS null then 0 else 1 end as bit) as [IsFiltered]
from
	Sueetie_Users u join
	Sueetie_UserAvatar a on u.UserID = a.UserID join
	aspnet_Users au on au.UserId = u.MembershipID join
	aspnet_Membership m on m.UserId = u.MembershipID left outer join
	yaf_User yu on yu.ProviderUserKey = u.MembershipID left outer join
	SuAnalytics_FilteredUsers f on f.UserID = u.UserID
where
	(u.IsActive = 0 or @UserTypeID != -4) and
	(u.IsActive = 1 or @UserTypeID not in (-3, 0)) and
	(m.IsApproved = 0 or @UserTypeID not in (-4, -3)) and
	(m.IsApproved = 1 or @UserTypeID != 0) and
	(u.UserID = -1 or @UserTypeID != -1)
                      
END

