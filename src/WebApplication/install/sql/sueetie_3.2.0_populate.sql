SET NOCOUNT ON
GO

print 'CREATE Anonymous Sueetie User -1'

/* -- CREATE Anonymous Sueetie User -1 ------------------------------------------------------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[Sueetie_Users] ON

INSERT INTO [dbo].[Sueetie_Users] (UserID, MembershipID, UserName, Email, DisplayName) VALUES 
	(-1, 'DC050D74-D068-4149-91C2-B2E3F4ABEEA0', 'anonymous','anonymous@yourdomain.com', 'Guest' );

SET IDENTITY_INSERT [dbo].[Sueetie_Users] OFF


print 'POPULATE Sueetie_Users and Sueetie_UserAvatar with existing member data'

/* -- POPULATE Sueetie_Users and Sueetie_UserAvatar with existing member data ------------------------------------------------------------------------------------------------------------------------------- */


insert into Sueetie_Users (MembershipID, UserName, Email, DisplayName) 
	select userid, username, 'na@email.com', UserName from aspnet_Users where aspnet_Users.loweredusername <> 'anonymous'

update Sueetie_Users set Email = loweredEmail from aspnet_Membership m 
	inner join Sueetie_Users u on m.UserId = u.MembershipID
	where u.Email = 'na@email.com' and LoweredEmail is not null

update Sueetie_Users set DisplayName = 'Admin Dude' where username = 'admin'
update Sueetie_Users set DisplayName = 'Testguy Jones' where username = 'testguy'
	
insert into Sueetie_UserAvatar (UserID) select userid from Sueetie_Users
insert into Sueetie_UserFollowCounts (UserID) select userid from Sueetie_Users

print 'INSERT INTO [dbo].[Sueetie_Applications]'

/* -- INSERT INTO [dbo].[Sueetie_Applications]  ------------------------------------------------------------------------------- */


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
	SELECT 7, 7, 'cms', 'CMS', 0, 1, 1 
	GO		


print 'INSERT INTO [dbo].[Sueetie_ApplicationTypes]'

/* -- INSERT INTO [dbo].[Sueetie_ApplicationTypes]  ------------------------------------------------------------------------------- */


INSERT INTO [dbo].[Sueetie_ApplicationTypes] (
	[ApplicationTypeID],
	[ApplicationName]
)
	SELECT 0, 'Unknown' UNION
	SELECT 1, 'Blog' UNION
	SELECT 2, 'Forum' UNION
	SELECT 3, 'Wiki' UNION
	SELECT 4, 'MediaGallery' UNION
	SELECT 5, 'Marketplace' UNION
	SELECT 7, 'CMS'

GO


print 'INSERT INTO [dbo].[Sueetie_ContentTypes]'

/* -- INSERT INTO [dbo].[Sueetie_ContentTypes]  ------------------------------------------------------------------------------- */


INSERT INTO [dbo].[Sueetie_ContentTypes] (
	[ContentTypeID],
	[ContentTypeName],
	[Description],
	[IsAlbum],
	[UserLogCategoryID],
	[AlbumMediaCategoryID]
)
	SELECT 0, 'Unknown', 'NA', 0, 0, 0 UNION
	SELECT 1, 'BlogPost', 'Blog Posts', 0, 200, 0 UNION
	SELECT 2, 'BlogComment', 'Blog Comments', 0, 201, 0 UNION
	SELECT 3, 'ForumTopic', 'Forum Topics', 0, 300, 0 UNION
	SELECT 4, 'ForumMessage', 'Forum Messages', 0, 301, 0 UNION
	SELECT 5, 'WikiPage', 'Wiki Pages', 0, 400, 0 UNION
	SELECT 6, 'MediaImage', 'Photos', 0, 551, 0 UNION
	SELECT 7, 'MediaAudioFile', 'Audio Files', 0, 552, 0 UNION
	SELECT 8, 'MediaVideo', 'Videos', 0, 554, 0 UNION
	SELECT 9, 'MediaDocument', 'Documents', 0, 553, 0 UNION
	SELECT 10, 'MediaOther', 'Media Objects', 0, 556, 0 UNION
	SELECT 11, 'MultipurposeMediaAlbum', 'Media Album', 1, 500, 556 UNION
	SELECT 12, 'ImageMediaAlbum', 'Photo Album', 1, 501, 551 UNION
	SELECT 13, 'AudioMediaAlbum', 'Audio Media Album', 1, 502, 552 UNION
	SELECT 14, 'DocumentMediaAlbum', 'Document Folder', 1, 503, 553 UNION
	SELECT 15, 'VideoMediaAlbum', 'Video Album', 1, 504, 554 UNION
	SELECT 16, 'OtherAlbum', 'Other or Unknown Album Type', 1, 506, 556 UNION
	SELECT 17, 'UserMediaAlbum', 'User Owned Media Album', 1, 505, 556 UNION
	SELECT 18, 'MarketplaceProduct', 'Marketplace Products', 0, 600, 0 UNION
	SELECT 20, 'CMSPage', 'CMS Page', 0, 800, 0 UNION
	SELECT 21, 'CMSPagePart', 'CMS Page Content Part', 0, 801, 0 UNION
	SELECT 22, 'CalendarEvent', 'Calendar Event', 0, 900, 0 UNION
	SELECT 23, 'WikiMessage', 'Wiki Discussion Message', 0, 402, 0 UNION
	SELECT 24, 'ForumAnswer', 'Forum Answer', 0, 302, 0

GO

print 'INSERT INTO [dbo].[Sueetie_Settings]'

/* -- INSERT INTO [dbo].[Sueetie_Settings]   ------------------------------------------------------------------------------- */

INSERT INTO [dbo].[Sueetie_Settings] (
	[SettingName],
	[SettingValue]
)
	SELECT 'ContactEmail', 'admin@yourdomain.com' UNION
	SELECT 'CreateWikiUserAccount', '1' UNION
	SELECT 'DefaultLanguage', 'en-US' UNION
	SELECT 'DefaultTimeZone', '-300' UNION
	SELECT 'EnableSSL', '0' UNION
	SELECT 'ErrorEmails', 'admin@yourdomain.com' UNION
	SELECT 'FromEmail', 'admin@yourdomain.com' UNION
	SELECT 'FromName', 'Sueetie Support Services' UNION
	SELECT 'GroupsFolderName', 'groups' UNION
	SELECT 'HtmlHeader', NULL UNION
	SELECT 'MobileTheme', 'chiclet' UNION
	SELECT 'RegistrationType', '0' UNION
	SELECT 'SettingID', '1' UNION
	SELECT 'SiteName', 'Sueetie Online Community' UNION
	SELECT 'SitePageTitleLead', NULL UNION
	SELECT 'SmtpPassword', NULL UNION
	SELECT 'SmtpServer', 'localhost' UNION
	SELECT 'SmtpServerPort', NULL UNION
	SELECT 'SmtpUserName', NULL UNION
	SELECT 'Theme', 'lollipop' UNION
	SELECT 'TrackingScript', NULL UNION
	SELECT 'RecordAnalytics','True' UNION
	SELECT 'HandleWwwSubdomain','remove' UNION
	SELECT 'IpGeoLookupUrl','http://www.ip2location.com/'
GO

print 'INSERT INTO [dbo].[be_Users]'

/* -- INSERT INTO [dbo].[be_Users]   ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[be_Users] ON

INSERT INTO [dbo].[be_Users] (
	[UserID],
	[UserName],
	[Password],
	[LastLoginTime],
	[EmailAddress]
)
	SELECT 1, 'Admin', '', '11-29-2008 16:15:10.623', 'admin@yourdomain.com' UNION
	SELECT 2, 'Testguy', '', '11-29-2008 18:53:04.763', 'testguy@yourdomain.com' UNION
    SELECT 3, 'Anonymous', '', '11-29-2008 18:53:04.763', 'anonymous@yourdomain.com' 
     

SET IDENTITY_INSERT [dbo].[be_Users] OFF
GO

print 'INSERT INTO [dbo].[be_Roles]'

/* -- INSERT INTO [dbo].[be_Roles]   ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[be_Roles] ON

INSERT INTO [dbo].[be_Roles] (
	[RoleID],
	[Role]
)
	SELECT 1, 'BlogAdministrator' UNION
	SELECT 2, 'BlogEditor'

SET IDENTITY_INSERT [dbo].[be_Roles] OFF
GO

print 'INSERT INTO [dbo].[be_UserRoles]'

/* -- INSERT INTO [dbo].[be_UserRoles]  ------------------------------------------------------------------------------- */


SET IDENTITY_INSERT [dbo].[be_UserRoles] ON

INSERT INTO [dbo].[be_UserRoles] (
	[UserRoleID],
	[UserID],
	[RoleID]
)
	SELECT 1, 1, 1 UNION
	SELECT 2, 1, 2 UNION
	SELECT 3, 2, 2

SET IDENTITY_INSERT [dbo].[be_UserRoles] OFF
GO

print 'INSERT INTO [dbo].[Sueetie_ContentParts]'

/* -- INSERT INTO [dbo].[Sueetie_ContentParts] ------------------------------------------------------------------------------- */


SET IDENTITY_INSERT [dbo].[Sueetie_ContentParts] ON

INSERT INTO [dbo].[Sueetie_ContentParts] (
	[ContentPartID],
	[ContentName],
	[ContentPageID],
	[LastUpdateDateTime],
	[LastUpdateUserID],
	[ContentText]
)

	SELECT 1, 'HomeContent', -1, '10-20-2010 09:41:00.000', 2, '<div class="IceCreamText"><img src="/images/shared/sueetie/icecream.png" alt="" align="right" />
<h2>Congratulations!</h2>
<p>You have successfully installed Sueetie Version 3.2!  This version introduces Sueetie Marketplace, updates to YetAnotherFoum.NET and Gallery Server Pro, and other fun features.  As we mentioned on the Installation Page, please remember to review the <a href="http://sueetie.com/wiki/GummyBearSetup.ashx#Post-Installation_Checklist_10" target="_new">Post-Installation Checklist</a> on the Sueetie Wiki to ensure your experience with Sueetie is super-splendid and error-free.</p>
<h2>Where to start</h2>
<p>You may want to begin by logging in as "admin" (password "password") and becoming familiar with the Site Control Panel. You may also want to explore the Administrative areas of the great apps that comprise Sueetie: <a href="http://www.dotnetblogengine.net/" target="_blank">BlogEngine.NET,</a> <a href="http://www.yetanotherforum.net/" target="_blank">YetAnotherForum.NET,</a> <a href="http://www.galleryserverpro.com/" target="_blank">Gallery Server Pro</a> and <a href="http://www.screwturn.eu/" target="_blank">ScrewTurn Wiki.</a></p>
<p>Next, some content would be nice, so start entering blog posts, comments, forum discussions, media, cms pages, calendar events, global tags...go crazy. The Gummy Bear installation process is simple enough that you''ll be able to fill up with fun and then quickly lock-and-load for the real deal.  See <a href="http://sueetie.com/wiki/GummyBearReloaded.ashx" target="_blank">Gummy Bear Reloaded</a> in the Sueetie Wiki for info on restoring Gummy Bear to its pristine state before you go live.</p>
<h2>Your To Do List<br /></h2>
<p>Probably at the top of your Gummy Bear To Do List is Theming.  <a href="http://sueetie.com/wiki/ThemingGummyBear.ashx#Customizing_the_Lollipop_Theme_10" target="_blank">Customizing Lollipop</a> in the Sueetie Wiki gives you tips on changing the logo and other primary elements to quickly alter your site''s look. If you want to create a brand new theme, see <a href="http://sueetie.com/wiki/ThemingGummyBear.ashx">Theming Gummy Bear</a> where you can download the Lollipop Theme Pack to use as a basis for an all-new theme.</p>
<p>And also on theming, remember that your Sueetie Community is fully mobile ready!  That means you''ll want to visit it on your iPhone, Droid, WP7 phone or some other mobile device and most likely customize your mobile theme (named Chiclet) as well.  Complete instructions on <a href="http://sueetie.com/wiki/ThemingGummyBear.ashx">Theming Gummy Bear.</a></p>
<p>Near the top of your to-do list is probably customizing the site menu.  Sueetie uses a SiteMenu Control that is easily customizable.  See the Sueetie Wiki for more about <a href="http://sueetie.com/wiki/FeatureSiteMenu.ashx">the Global SiteMenu Control.</a></p>
<h2>Don''t Be a Stranger<br /></h2>
<p>I saved the most important info for last.  Please visit the <a href="http://sueetie.com/forum" target="_blank">Sueetie Discussion Forums</a> to share your experience, offer suggestions, report bugs (bugs? Yeah, right! :) or ask any questions you might have about Sueetie. </p>
<p>Thank you very much for using Sueetie for your Online Community.  I hope your experience is filled with sueetness.</p>
</div>
<p><img src="http://sueetie.com/images/shared/sueetie/db.png" alt="" /></p>
<p><a href="http://dbvt.com" target="_blank">Dave Burke</a> <br />Sueetie Head Ice Cream Dipper</p>' UNION
	SELECT 2, 'DemoContent', -1, '10-10-2010 09:01:00.000', -1, '<p><span style="font-weight: bold;">This is a demo of Sueetie Content Parts.</span> If you are sign-in as "admin" or any account in the ContentAdministrator or SueetieAdministrator roles, click me twice, really hard. Yes!!!<br><br>Sueetie Content Parts are fully SEO indexable, which is pretty great. We''ll be using the NicEdit JQuery control, so you will need to load the appropriate .JS support files. Use this page as an example for the required .JS scripts.</p>' UNION
	SELECT 3, 'WelcomeContent', -1, '10-13-2010 13:51:00.000', 2, '<div class="WelcomeMessageText">' + CHAR(13) + CHAR(10) + '<p>Thank you for becoming a member of our community!</p></div>' UNION
	SELECT 4, 'cmsSideTop', -1, '10-16-2010 16:53:00.000', 2, '<p>Sidebar global top demo content part. Appears on all pages.</p>' UNION
	SELECT 5, 'cms.default.sidebottom', 2, '10-16-2010 16:53:00.000', 2, '<p>Sidebar bottom demo content specific to this content page.</p>' UNION
	SELECT 6, 'cms.default.body', 2, '10-16-2010 16:47:00.000', 2, '<h3>Demo CMS Page</h3>' + CHAR(13) + CHAR(10) + '<p>Demo page for your templatizational enjoyment.</p>' + CHAR(13) + CHAR(10) + '<p>&nbsp;</p>'

	
SET IDENTITY_INSERT [dbo].[Sueetie_ContentParts] OFF
GO

print 'INSERT INTO [dbo].[Sueetie_SiteLogCategories]'

/* -- INSERT INTO [dbo].[Sueetie_SiteLogCategories] ------------------------------------------------------------------------------- */


INSERT INTO [dbo].[Sueetie_SiteLogCategories] (
	[SiteLogCategoryID],
	[SiteLogCategoryCode],
	[SiteLogCategoryDescription]
)
	SELECT 100, 'Generic Message', 'Generic System logging entry' UNION
	SELECT 101, 'Debug Info', 'System generated debugging information' UNION
	SELECT 102, 'Email Exception', 'System generated email exception' UNION
	SELECT 103, 'General Exception', 'Logging of a site runtime exception' UNION
	SELECT 104, 'Tasks Exception', 'Exception related to background task operations' UNION
	SELECT 105, 'App Start/Stop', 'Information on Site Application Startup and Shutdown' UNION
	SELECT 106, 'General App Event', 'Logged Site Application Event' UNION
	SELECT 107, 'SearchException', 'Exception related to Lucene search or indexing processes' UNION
	SELECT 109, 'Tasks Message','General Message produced by Background Task' UNION
	SELECT 201, 'Marketplace Message', 'General Message produced by Sueetie Marketplace' UNION
	SELECT 202, 'Marketplace Exception', 'Exception produced by Sueetie Marketplace' UNION
	SELECT 301, 'Addon Pack Message', 'General Message produced by Sueetie Addon Pack' UNION
	SELECT 302, 'Addon Pack Exception', 'Exception produced by Sueetie Addon Pack' UNION
	SELECT 401, 'Analytics Message', 'General Message produced by Sueetie Analytics' UNION
	SELECT 402, 'Analytics Exception', 'Exception produced by Sueetie Analytics' 

GO

print 'INSERT INTO [dbo].[Sueetie_UserLogCategories]'

/* -- INSERT INTO [dbo].[Sueetie_UserLogCategories] ------------------------------------------------------------------------------- */

INSERT INTO [dbo].[Sueetie_UserLogCategories] (
	[UserLogCategoryID],
	[UserLogCategoryCode],
	[UserLogCategoryDescription],
	[IsDisplayed],
	[IsLocked],
	[IsSyndicated]
)
	SELECT 100, 'Registered', 'New User Registered. Account awaiting email verification or admin approval', 0, 1, 0 UNION
	SELECT 101, 'Joined Community', 'Account created by automated registration or email verification', 1, 1, 1 UNION
	SELECT 102, 'Account Approved', 'New User Account Approved by Community Manager', 0, 1, 0 UNION
	SELECT 103, 'LoggedIn', 'User Logged in to site through site login page', 0, 1, 0 UNION
	SELECT 110, 'Following', 'User is following another user', 1, 1, 1 UNION
	SELECT 111, 'UnFollowing', 'User stopped following another user', 0, 1, 0 UNION
	SELECT 112, 'Friends', 'Users are now friends. Mutual following status', 1, 1, 1 UNION
	SELECT 200, 'Blog Post', 'User posted new blog post', 1, 1, 1 UNION
	SELECT 201, 'Blog Comment', 'User posted new blog comment', 1, 1, 1 UNION
	SELECT 300, 'Forum Topic', 'User created a new forum topic', 1, 1, 1 UNION
	SELECT 301, 'Forum Message', 'User posted new forum message', 1, 1, 1 UNION
	SELECT 302, 'Forum Answer', 'User marked a forum post as an Answer', 1, 1, 1 UNION
	SELECT 400, 'New Wiki Page', 'User created a new wiki page', 1, 1, 1 UNION
	SELECT 401, 'Wiki Page Updated', 'User updated an existing wiki page', 1, 1, 1 UNION
	SELECT 402, 'New Wiki Message', 'New Wiki Page Discussion Message', 1, 1, 1 UNION
	SELECT 500, 'Multipurpose Album', 'New Multipurpose Media Album', 1, 1, 1 UNION
	SELECT 501, 'Image Album', 'New Image Media Album', 1, 1, 1 UNION
	SELECT 502, 'Audio Album', 'New Audio Media Album', 1, 1, 1 UNION
	SELECT 503, 'Document Album', 'New Document Album', 1, 1, 1 UNION
	SELECT 504, 'Video Album', 'New Video Album', 1, 1, 1 UNION
	SELECT 505, 'User Media Album', 'New User Owned Media Album', 1, 1, 1 UNION
	SELECT 551, 'Image Uploaded', 'New Image Uploaded in Media Gallery', 1, 1, 1 UNION
	SELECT 552, 'Audio Uploaded', 'New Audio Uploaded in Media Gallery', 1, 1, 1 UNION
	SELECT 553, 'Document Uploaded', 'New Document Uploaded in Media Gallery', 1, 1, 1 UNION
	SELECT 554, 'Video Uploaded', 'New Video Uploaded in Media Gallery', 1, 1, 1 UNION
	SELECT 556, 'Other Media Uploaded', 'New Media of some other type uploaded in Media Gallery', 1, 1, 1 UNION
	SELECT 600, 'Marketplace Product', 'New Marketplace Product', 1, 1, 1 UNION
	SELECT 800, 'CMS Page Created', 'User created a new CMS Content Page', 0, 1, 1 UNION
	SELECT 801, 'CMS Page Updated', 'User updated a CMS Content Page', 1, 1, 1 UNION
	SELECT 900, 'Calendar Event', 'New Calendar Event', 1, 1, 1

GO

print 'INSERT INTO [dbo].[Sueetie_Roles]'

/* -- INSERT INTO [dbo].[Sueetie_Roles]  ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[Sueetie_Roles] ON

INSERT INTO [dbo].[Sueetie_Roles] (
	[SueetieRoleID],
	[RoleID],
	[RoleName],
	[IsGroupAdminRole],
	[IsGroupUserRole],
	[IsLocked],
	[IsBlogOwnerRole]
)
	SELECT 1, '8b56f594-7d2b-4d57-a013-25a16e9647c4', 'SueetieAdministrator', 0, 0, 1, 0 UNION
    SELECT 2, '46ca7176-d30f-47b6-a793-74a69b30438c', 'MembershipAdministrator', 0, 0, 0, 0 UNION 
	SELECT 3, 'f273dfeb-6b2d-4e84-8649-366f6c8728ea', 'BlogAdministrator', 0, 0, 1, 1 UNION
	SELECT 4, '8198af3e-692a-4ef4-ae3f-b7e21e2a8276', 'BlogEditor', 0, 0, 1, 0 UNION
	SELECT 5, '41e0c7a4-9c47-48f3-93f0-9c2f46b70817', 'ContentAdministrator', 0, 0, 1, 0 UNION
	SELECT 6, 'ded86f3f-ea46-440b-8519-86bbf416387e', 'ForumAdministrator', 0, 0, 1, 0 UNION
	SELECT 7, '4affd2bf-c4c5-4a9e-912f-3e02978b9883', 'GroupAdministrator', 1, 0, 1, 0 UNION
	SELECT 8, 'e290245f-a7aa-4e05-b5d6-39b51e481a72', 'Guests', 0, 0, 1, 0 UNION
	SELECT 9, 'fe5e9535-5405-4e8f-aa1a-4c98288537c0', 'MarketplaceAdministrator', 0, 0, 1, 0 UNION
	SELECT 10, '646ceae4-3a4e-410a-b797-8ece85bd1bb2', 'MediaAdministrator', 0, 0, 1, 0 UNION
	SELECT 11, '4cfa8159-b1b4-442b-abb1-f141f978f0e9', 'Registered', 0, 1, 1, 0 UNION
	SELECT 12, 'd51fa0a3-c7bb-445a-9c50-946b699cff3b', 'SiteBlogEditor', 0, 0, 1, 0 UNION
	SELECT 13, '5bfcce78-c43e-4de4-b556-6930b3dfa6de', 'WikiAdministrator', 0, 0, 1, 0


SET IDENTITY_INSERT [dbo].[Sueetie_Roles] OFF
GO

print 'INSERT INTO [dbo].[Sueetie_Categories]'

/* -- INSERT INTO [dbo].[Sueetie_Categories] ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[Sueetie_Categories] ON

INSERT INTO [dbo].[Sueetie_Categories] (
	[CategoryID],
	[Category],
	[IsContentCategory],
	[IsGlobalCategory],
	[ApplicationTypeID],
	[ApplicationID],
	[IsActive]
)
	SELECT 0, 'na', 0, 1, 0, 0, 1

SET IDENTITY_INSERT [dbo].[Sueetie_Categories] OFF
GO


print 'INSERT INTO [dbo].[Sueetie_Groups]'

/* -- INSERT INTO [dbo].[Sueetie_Groups] ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[Sueetie_Groups] ON

INSERT INTO [dbo].[Sueetie_Groups] (
	[GroupID],
	[GroupKey],
	[GroupName],
	[GroupAdminRole],
	[GroupUserRole],
	[GroupDescription],
	[GroupTypeID],
	[IsActive],
	[HasAvatar]
)
	SELECT 0, NULL, 'na', 'na', NULL, NULL, 1, 1, 0 UNION
	SELECT 1, 'demo', 'Public Demo Group', 'GroupAdministrator', NULL, 'Group for demonstration purposes', 0, 1, 0

SET IDENTITY_INSERT [dbo].[Sueetie_Groups] OFF
GO

print 'INSERT INTO [dbo].[Sueetie_SiteLogTypes] '

/* -- INSERT INTO [dbo].[Sueetie_SiteLogTypes]  ------------------------------------------------------------------------------- */

INSERT INTO [dbo].[Sueetie_SiteLogTypes] (
	[SiteLogTypeID],
	[SiteLogTypeCode]
)
	SELECT -1, 'NA' UNION
	SELECT 1, 'General' UNION
	SELECT 2, 'Warning' UNION
	SELECT 3, 'Exception' UNION
	SELECT 4, 'Debug' UNION
	SELECT 5, 'Custom'

GO

print 'INSERT INTO [dbo].[Sueetie_TaskQueue]'

/* -- INSERT INTO [dbo].[Sueetie_TaskQueue] ------------------------------------------------------------------------------- */


SET IDENTITY_INSERT [dbo].[Sueetie_TaskQueue] ON

INSERT INTO [dbo].[Sueetie_TaskQueue] (
	[TaskQueueID],
	[TaskTypeID],
	[TaskStartDateTime],
	[TaskEndDateTime],
	[SuccessBit],
	[Active]
)
	SELECT 1, 1, '06-09-1969 00:00:00.000', '06-09-1969 00:00:00.000', 1, 0
    
SET IDENTITY_INSERT [dbo].[Sueetie_TaskQueue] OFF
GO

print 'INSERT INTO [dbo].[Sueetie_Content]'

/* -- INSERT INTO [dbo].[Sueetie_Content] ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[Sueetie_Content] ON

INSERT INTO [dbo].[Sueetie_Content] (
	[ContentID],
	[SourceID],
	[ContentTypeID],
	[ApplicationID],
	[UserID],
	[Permalink],
	[DateTimeCreated],
	[IsRestricted]
)
	SELECT 0, 1, 0, 0, -1, 'na', GETDATE(), 0 UNION
	SELECT 2, 2, 20, 7, 2, '/cms/Demo-CMS-Page.aspx', '10-16-2010 17:30:49.177', 0 UNION
	SELECT 3, 2, 11, 4, 2, '/media/default.aspx?aid=2', '10-17-2010 18:21:30.677', 0 UNION
	SELECT 4, 1, 6, 4, 2, '/media/gs/handler/getmediaobject.ashx?5qFyNWhdCaNFND0A%2BeD3hufzOnKsoGMAwiTZkuJGvlmfrH4n1ny5DCtduvvF1hMmyQLeAsGt2D8xu47kXPig0Bx56KgbhI7PW%2BMRQH5CGkNDOVZqPJEgHyosuaSfT0ae5l7xqr%2FTwSz38YrOBqO9pg56SM%2BwH%2FoCCbX46%2Fsuv9o%3D', '10-17-2010 18:21:49.730', 0 UNION 
    SELECT 5, 1, 5, 3, 2, '/wiki/MainPage.ashx', '10-19-2010 12:24:31.527', 0

SET IDENTITY_INSERT [dbo].[Sueetie_Content] OFF
GO

print 'INSERT INTO [dbo].[Sueetie_Calendars]'

/* -- INSERT INTO [dbo].[Sueetie_Calendars] ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[Sueetie_Calendars] ON

INSERT INTO [dbo].[Sueetie_Calendars] (
	[CalendarID],
	[CalendarTitle],
	[CalendarDescription],
	[CalendarUrl],
	[IsActive]
)
	SELECT 1, 'Sueetie Event Calendar', 'Default Sueetie Community Calendar', '/calendar/default.aspx', 1 UNION
	SELECT 2, 'Demo Editable Calendar', 'Calendar for demonstration purpose, editable by registered users', '/calendar/editdemo.aspx', 1


SET IDENTITY_INSERT [dbo].[Sueetie_Calendars] OFF
GO


print 'INSERT INTO [dbo].[Sueetie_ContentPages]'

/* -- INSERT INTO [dbo].[Sueetie_ContentPages]  ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[Sueetie_ContentPages] ON

INSERT INTO [dbo].[Sueetie_ContentPages] (
	[ContentPageID],
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
)
	SELECT -1, -1, 'nopage', 'na', 'na', 'Page Placemarker for Non-Page Content Parts', NULL, '07-27-2010 13:24:00.000', -1, 1, -1 UNION
	SELECT 2, 1, 'default', 'Demo-CMS-Page', 'Demo CMS Page', 'Demo CMS Page for use as a template for new pages', NULL, '10-16-2010 17:32:00.000', 2, 1, -1

SET IDENTITY_INSERT [dbo].[Sueetie_ContentPages] OFF
GO

print 'INSERT INTO [dbo].[Sueetie_ContentPageGroups]'

/* -- INSERT INTO [dbo].[Sueetie_ContentPageGroups]  ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[Sueetie_ContentPageGroups] ON

INSERT INTO [dbo].[Sueetie_ContentPageGroups] (
	[ContentPageGroupID],
	[ApplicationID],
	[ContentPageGroupTitle],
	[EditorRoles],
	[IsActive]
)
	SELECT -1, 0, 'na', NULL, 1 UNION
	SELECT 1, 7, 'Default Content Page Group', 'ContentAdministrator,SueetieAdministrator', 1

SET IDENTITY_INSERT [dbo].[Sueetie_ContentPageGroups] OFF
GO


print 'INSERT INTO [dbo].[Sueetie_WikiPages]'

/* -- INSERT INTO [dbo].[Sueetie_WikiPages] ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[Sueetie_WikiPages] ON

INSERT INTO [dbo].[Sueetie_WikiPages] (
	[PageID],
	[PageFileName],
	[PageTitle],
	[Keywords],
	[Abstract],
	[Namespace],
	[DateTimeCreated],
	[DateTimeModified],
	[UserID],
	[ApplicationID],
	[Active],
	[PageContent]
)
	SELECT 1, 'MainPage', 'Main Page', '', NULL, NULL, '10-19-2010 12:25:00.000', '10-19-2010 12:25:00.000', 2, 3, 1, 'Welcome to ScrewTurn Wiki! This is the main page of your new ScrewTurn Wiki, created for you by the system.You should edit this page, using the Edit button in the top-right corner of the screen. You can also create a new page, using the Create a new Page link in the sidebar on the left.If you need help, try to visit our website or our forum.Warning: remember to setup the admin account by editing the Web.config file placed in the root directory of the Wiki. It is extremely dangerous to keep the default password.'

SET IDENTITY_INSERT [dbo].[Sueetie_WikiPages] OFF
GO

print 'INSERT INTO Sueetie_DisplayTypes'

/* -- INSERT INTO Sueetie_DisplayTypes  -------------------------------------------------------------------------------- */

INSERT INTO Sueetie_DisplayTypes select 0, 'Unknown'
INSERT INTO Sueetie_DisplayTypes select 1, 'Thumbnails'
INSERT INTO Sueetie_DisplayTypes select 2, 'Folders'
GO
