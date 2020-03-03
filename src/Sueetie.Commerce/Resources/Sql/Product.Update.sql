update [SuCommerce_Products] set
	[CategoryID] = @CategoryID,
	[Title] = @Title,
	[SubTitle] = @SubTitle,
	[ProductDescription] = @ProductDescription,
	[DownloadURL] = @DownloadURL,
	[Price] = @Price,
	[PurchaseTypeID] = @PurchaseTypeID,
	[ProductTypeID] = @ProductTypeID,
	[StatusTypeID] = @StatusTypeID
WHERE
	[ProductID] = @ProductID

-- Update existing carts with new price
declare @cartnum int
select @cartnum = COUNT(*) from SuCommerce_CartLinks where ProductID = @ProductID
if @cartnum = 1
begin
	UPDATE SuCommerce_CartLinks set Price = @Price where ProductID = @ProductID
end