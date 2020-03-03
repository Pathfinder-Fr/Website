SET NOCOUNT ON
GO

print 'INSERT INTO [dbo].[gs_Role]'

/* -- INSERT INTO [dbo].[gs_Role] ------------------------------------------------------------------------------- */

INSERT INTO [dbo].[gs_Role] select RoleName, 1, 0,0,0,0,0,0,0,0,0,0,0 from aspnet_Roles
GO

print 'UPDATE [dbo].[gs_Role]'

/* -- UPDATE [dbo].[gs_Role] ------------------------------------------------------------------------------- */

UPDATE [dbo].[gs_Role]
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

GO    

print 'INSERT INTO [dbo].[gs_gallery]'

/* -- INSERT INTO [dbo].[gs_gallery]   ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[gs_gallery] ON

INSERT INTO [dbo].[gs_gallery] (GalleryID, Description, DateAdded) values (1, 'Media Gallery', GETDATE())
GO

SET IDENTITY_INSERT [dbo].[gs_gallery] OFF

print 'INSERT INTO gs_album'

/* -- INSERT INTO gs_album ------------------------------------------------------------------------------- */

SET IDENTITY_INSERT [dbo].[gs_Album] ON

INSERT INTO [dbo].[gs_Album] (
	[AlbumId],
	[FKGalleryId],
	[AlbumParentId],
	[Title],
	[DirectoryName],
	[Summary],
	[ThumbnailMediaObjectId],
	[Seq],
	[DateStart],
	[DateEnd],
	[DateAdded],
	[CreatedBy],
	[LastModifiedBy],
	[DateLastModified],
	[OwnedBy],
	[OwnerRoleName],
	[IsPrivate]
)
	SELECT 1, 1, 0, 'All albums', '', 'Welcome to Gallery Server Pro!', 0, 0, NULL, NULL, getdate(), 'System', 'System', getdate(), '', '', 0 UNION
	SELECT 2, 1, 1, 'Samples', 'Samples', '', 1, 1, NULL, NULL, getdate(), 'admin', 'admin', getdate(), '', '', 0

SET IDENTITY_INSERT [dbo].[gs_Album] OFF
GO

print 'INSERT INTO [dbo].[gs_MediaObject]'

/* -- INSERT INTO [dbo].[gs_MediaObject]  ------------------------------------------------------------------------------- */


SET IDENTITY_INSERT [dbo].[gs_MediaObject] ON

INSERT INTO [dbo].[gs_MediaObject] (
	[MediaObjectId],
	[FKAlbumId],
	[Title],
	[HashKey],
	[ThumbnailFilename],
	[ThumbnailWidth],
	[ThumbnailHeight],
	[ThumbnailSizeKB],
	[OptimizedFilename],
	[OptimizedWidth],
	[OptimizedHeight],
	[OptimizedSizeKB],
	[OriginalFilename],
	[OriginalWidth],
	[OriginalHeight],
	[OriginalSizeKB],
	[ExternalHtmlSource],
	[ExternalType],
	[Seq],
	[CreatedBy],
	[DateAdded],
	[LastModifiedBy],
	[DateLastModified],
	[IsPrivate]
)
	SELECT 1, 2, 'lollipop.jpg', '5C-FF-58-1B-07-A9-26-E0-86-C0-D3-90-78-3A-7A-25', 'zThumb_lollipop.jpeg', 109, 115, 1, 'lollipop.jpg', 567, 593, 24, 'lollipop.jpg', 567, 593, 24, '', 'NotSet', 1, 'admin', getdate(), 'admin', getdate(), 0

SET IDENTITY_INSERT [dbo].[gs_MediaObject] OFF
GO


print 'INSERT INTO [dbo].[Sueetie_gs_MediaObject]'

/* -- 'INSERT INTO [dbo].[Sueetie_gs_MediaObject]  ------------------------------------------------------------------------------- */

INSERT INTO [dbo].[Sueetie_gs_MediaObject] (
	[MediaObjectID],
	[MediaObjectDescription],
	[InDownloadReport]
)
	SELECT 1, NULL, 0

GO

print 'INSERT INTO [dbo].[sueetie_gs_bibliography]'

/* -- 'INSERT INTO [dbo].[sueetie_gs_bibliography]  ------------------------------------------------------------------------------- */

INSERT INTO [dbo].[sueetie_gs_bibliography] (mediaobjectid) select mediaobjectid from gs_MediaObject
GO

print 'INSERT INTO Sueetie_gs_Album'

/* -- INSERT INTO Sueetie_gs_Album  ------------------------------------------------------------------------------- */

insert into Sueetie_gs_Album select albumid, 11, null, null from gs_Album where FKGalleryId = 1

GO

print 'INSERT INTO [dbo].[Sueetie_gs_Gallery]'

/* -- INSERT INTO [dbo].[Sueetie_gs_Gallery]  -------------------------------------------------------------------------------- */

INSERT INTO [dbo].[Sueetie_gs_Gallery] (galleryid) select galleryid from gs_gallery where galleryid > 0
GO

print 'UPDATE [dbo].[Sueetie_gs_gallery]'

/* -- UPDATE [dbo].[Sueetie_gs_gallery]  -------------------------------------------------------------------------------- */

UPDATE [dbo].[Sueetie_gs_gallery] set applicationID = (select applicationid from Sueetie_Applications where ApplicationKey = 'media' and GroupID = 0)
GO

UPDATE [dbo].[Sueetie_gs_gallery] set gallerykey = 'default' where galleryid = 1
GO

print 'Update Admin Email from YAF_USER'

/* -- Update Admin Email from YAF_USER  ------------------------------------------------------------------------------- */

declare @adminEmail nvarchar(255)
select @adminEmail = email from yaf_user where name = 'admin'
update Sueetie_Users set Email = @adminEmail where UserName = 'admin'

declare @adminKey nvarchar(60)
select @adminKey = ProviderUserKey from yaf_user where name = 'admin'
update aspnet_Membership set Email = @adminEmail, LoweredEmail = LOWER(@adminEmail) where
	UserId = @adminKey
	
GO

print 'INSERT INTO yaf_registry'

/* -- INSERT INTO yaf_registry  ------------------------------------------------------------------------------- */

INSERT INTO yaf_registry select 'theme','lollipop.xml',1
GO
INSERT INTO yaf_registry select 'enabledisplayname','1', NULL
GO

print 'INSERT INTO sueetie_blogs'

/* -- INSERT INTO sueetie_blogs  ------------------------------------------------------------------------------- */

INSERT INTO sueetie_blogs select 1, 0, null, null, 'Site Blog','<p>Site News and Information</p>',0,0,0,0,1,1,GETDATE(),0
GO

print 'INSERT INTO gs_Role_Album'

/* -- INSERT INTO gs_Role_Album  ------------------------------------------------------------------------------- */

declare @RegPresent int
select @RegPresent = COUNT(*) from gs_Role_Album where FKRoleName = 'Registered' and FKAlbumId = 1
if @RegPresent = 0
begin
	insert into gs_Role_Album select 'Registered',1
end
GO


print 'INSERT INTO [dbo].[yaf_User]'

/* -- INSERT INTO [dbo].[yaf_User]  ------------------------------------------------------------------------------- */

Exec Sueetie_yaf_user_aspnet 1, 'testguy','Testguy Jones','testguy@yourdomain.com', 'A1C190AF-7420-4D00-AF9F-9746F160C9D2', 1, -300
GO


print 'IMPORT NEW USERS into [gs_UserGalleryProfile]'

/* -- IMPORT NEW USERS into [gs_UserGalleryProfile]  -------------------------------------------------------------------------------- */

declare @username nvarchar(50)

declare userCursor cursor for select username from sueetie_users where 
(username not in (select username from gs_usergalleryprofile)
and username <> 'anonymous')

open userCursor

fetch next from userCursor into @username
while @@fetch_status = 0
begin

	insert into gs_UserGalleryProfile select @UserName,1,'ShowMediaObjectMetadata','False'
	insert into gs_UserGalleryProfile select @UserName,1,'EnableUserAlbum','True'
	insert into gs_UserGalleryProfile select @UserName,1,'UserAlbumId','0'
	fetch next from userCursor into @username
	
end

close userCursor
deallocate userCursor
GO

print 'update gs_gallerysetting'

/* -- update gs_gallerysetting  -------------------------------------------------------------------------------- */

update gs_gallerysetting set SettingValue = 'False' where SettingName = 'ShowHeader'
GO
