select
	p.ProductID,
	p.UserID,
	p.CategoryID,
	p.Title,
	p.SubTitle,
	p.ProductDescription,
	p.DownloadURL,
	p.Price, 
	p.ExpirationDate,
	p.DateCreated,
	p.DateApproved,
	p.NumViews,
	p.NumDownloads,
	p.PurchaseTypeID,
	p.PreviewImageID,
	p.DocumentationURL,
	p.ImageGalleryURL,
	p.StatusTypeID,
	u.UserName,
	u.Email,
	u.DisplayName,
	c.ContentID,
	c.ContentTypeID,
	c.ApplicationID,
	st.StatusTypeCode,
	ca.CategoryName,
	ca.Path as [PurchaseTypeCode],
	p.ProductTypeID,
	prt.ProductTypeCode,
	prt.ProductTypeDescription
from
	SuCommerce_Products p join
	Sueetie_Users u on p.UserID = u.UserID join
	Sueetie_Content c on p.ProductID = c.SourceID and c.ContentTypeID = @contentTypeId join
	SuCommerce_StatusTypes st ON p.StatusTypeID = st.StatusTypeID join
	SuCommerce_Categories ca ON p.CategoryID = ca.CategoryID join
	SuCommerce_PurchaseTypes put ON p.PurchaseTypeID = put.PurchaseTypeID join
	SuCommerce_ProductTypes prt ON p.ProductTypeID = prt.ProductTypeID
where
	p.ProductId = @productId