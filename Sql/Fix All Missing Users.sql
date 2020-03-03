select
	u.UserId,
	yu.DisplayName,
	m.Email,
	u.UserName
into #missingUsers
from
	aspnet_Users u join
	aspnet_Membership m on m.UserId = u.UserId join
	yaf_User yu on yu.ProviderUserKey = u.UserId
where
	u.UserId not in (select MembershipID from Sueetie_Users)

declare @userId uniqueidentifier 
declare @id int

declare missingUsers cursor for select UserId from #missingUsers

open missingUsers
FETCH NEXT FROM missingUsers INTO @userId
while @@FETCH_STATUS = 0
begin
	insert into Sueetie_Users select m.UserId as [MembershipID], m.UserName, m.Email, m.DisplayName, null as [Bio], 1 as [IsActive], null as [IP], null as [LastActivity], 1 as [InAnalytics] from #missingUsers m where m.UserId = @userId
		
	set @id = SCOPE_IDENTITY()
	insert into Sueetie_UserAvatar values (@id, null, null)
	print @id
	
	select * from Sueetie_Users where MembershipID = @userId
	FETCH NEXT FROM missingUsers INTO @userId
end

close missingUsers
deallocate missingUsers
	
drop table #missingUsers