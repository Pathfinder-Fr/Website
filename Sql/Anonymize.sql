-- Réinitialisation mot de passe & email ASPNET Membership
update m set
	m.Email = u.LoweredUserName + '@pathfinder-fr.org',
	m.LoweredEmail = u.LoweredUserName + '@pathfinder-fr.org',
	m.PasswordQuestion = null,
	m.PasswordAnswer = null,
	m.PasswordSalt = '',
	m.PasswordFormat = 0,
	m.Password = 'password'	
from
	aspnet_Membership m join
	aspnet_Users u on u.UserId = m.UserId

-- Suppression email Blog
update dbo.be_Users set EmailAddress = lower(UserName) + '@pathfinder-fr.org'

-- Suppression erreurs Galerie
delete from gs_AppError

-- Suppression emails commentaires blogs
update Sueetie_beComments set Email = 'null@pathfinder-fr.org', Ip = null

-- Suppression messages du blog non publiés
delete from Sueetie_bePosts where IsPublished = 0

-- Réinitialisation paramètres Sueetie
update Sueetie_Settings set SettingValue = 'errors@pathfinder-fr.org' where SettingName = 'ErrorEmails'
update Sueetie_Settings set SettingValue = 'localhost' where SettingName = 'SmtpServer'
update Sueetie_Settings set SettingValue = 'password' where SettingName = 'SmtpPassword'
update Sueetie_Settings set SettingValue = 'username' where SettingName = 'SmtpUserName'
update Sueetie_Settings set SettingValue = '' where SettingName = 'TrackingScript'

-- Suppression log Sueetie
delete from Sueetie_SiteLog

-- Nettoyage utilisateurs Sueetie
update Sueetie_Users set Email = lower(UserName) + '@pathfinder-fr.org', IP = null, LastActivity = GETDATE()

-- Nettoyage utilisateurs
update [User] set PasswordHash = '', Email = LOWER([Username]) + '@pathfinder-fr.org', DateTime = GETDATE()

-- Suppression activité YAF
delete from yaf_Active

-- Suppression IP Bannies YAF
delete from yaf_BannedIP

-- Suppression emails
delete from yaf_CheckEmail

-- Suppression evènements
delete from yaf_EventLog

-- Nettoyage utilisateurs
update yaf_User set Email = LOWER(Name) + '@pathfinder-fr.org', Password = 'password', IP = null

-- Suppression messages forums privés
select yaf_Forum.ForumID into #privateforums from yaf_Forum where yaf_Forum.Flags = 6

delete from yaf_Attachment where yaf_Attachment.MessageID in (select MessageID from yaf_Message where TopicID in (select TopicID from yaf_Topic where ForumId in (select ForumID from #privateforums)))
update yaf_Topic set LastMessageID = null where ForumID in (select ForumID from #privateforums)
update yaf_Forum set LastMessageID = null, LastTopicID = null where ForumID in (select ForumID from #privateforums)
delete from yaf_Message where TopicID in (select TopicID from yaf_Topic where ForumId in (select ForumID from #privateforums))
delete from yaf_Topic where ForumID in (select ForumID from #privateforums)

drop table #privateforums

-- Suppression message privés
delete from yaf_UserPMessage
delete from yaf_PMessage

-- Suppression abonnements forums
delete from yaf_WatchForum
delete from yaf_WatchTopic