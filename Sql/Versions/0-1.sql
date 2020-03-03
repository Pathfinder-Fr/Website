-- Suppression index nécessaire avant upgrade YAF
drop index IX_yaf_ActiveAccess_UserID on yaf_activeaccess
go

alter procedure [dbo].[Sueetie_UnreadPMs_Get] 
	@UserName varchar(max) = ''
as
begin
	set nocount on;

	SELECT
		pm.PMessageID,
		pm.FromUserID,
		ufrom.Name as [FromUser],
		pm.Created,
		pm.Subject
	FROM
		[yaf_PMessage] pm join
		[yaf_User] ufrom on ufrom.UserID = pm.FromUserID join
		[yaf_UserPMessage] upmto on upmto.PMessageID = pm.PMessageID join
		[yaf_User] uto on uto.UserID = upmto.UserID
	where
		lower(uto.Name) = lower(@UserName) and
		upmto.IsRead = 0 and
		upmto.IsDeleted = 0 and
		upmto.IsArchived = 0
	order by
		pm.Created desc
end
go