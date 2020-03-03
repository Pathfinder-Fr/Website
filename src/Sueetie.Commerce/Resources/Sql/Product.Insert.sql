INSERT INTO [SuCommerce_Products] (
	[UserID],
	[CategoryID],
	[Title],
	[SubTitle],
	[ProductDescription],
	[DownloadURL],
	[Price],
	[DateCreated],
	[PurchaseTypeID],
	[PreviewImageID],
	[DocumentationURL],
	[ImageGalleryURL],
	[StatusTypeID],
	[ProductTypeID]
) VALUES (
	@UserID,
	@CategoryID,
	@Title,
	@SubTitle,
	@ProductDescription,
	@DownloadURL,
	@Price,
	GETDATE(),
	@PurchaseTypeID,
	@PreviewImageID,
	@DocumentationURL,
	@ImageGalleryURL,
	@StatusTypeID,
	@ProductTypeID
)

select SCOPE_IDENTITY() as [Id]