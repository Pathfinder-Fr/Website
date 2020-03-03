declare @username varchar(80) = 'Nelemiah'
declare @uid uniqueidentifier  = (select UserId from aspnet_users where Username = @username)
declare @email varchar(80) = (select Email from aspnet_Membership where UserId = @uid)

-- Partie ASPNET
select * from aspnet_Membership where UserId = @uid
select * from aspnet_Users where UserId = @uid
select * from aspnet_UsersInRoles where UserId = @uid

-- YAF
select * from yaf_User where ProviderUserKey = @uid
declare @displayname varchar(80) = (select DisplayName from yaf_User where ProviderUserKey = @uid)

-- Sueetie
select * from Sueetie_Users where MembershipID = @uid
declare @id int = (select UserId from sueetie_users where MembershipID = @uid)
select * from Sueetie_UserAvatar where UserID = @id

-- BlogEngine.NET
select * from be_Users where be_users.UserName = @username

if (@id is null)
begin
	insert into Sueetie_Users (MembershipID, UserName, Email, DisplayName, IsActive, InAnalytics) values (@uid, @username, @email, @displayname, 1, 1)
	set @id = SCOPE_IDENTITY()
	insert into Sueetie_UserAvatar values (@id, null, null)
	print @id
	
	select * from Sueetie_Users where MembershipID = @uid
end