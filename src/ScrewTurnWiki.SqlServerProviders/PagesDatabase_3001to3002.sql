create table [PagesContentCache] (
	[Page] nvarchar(200) not null,
	[Namespace] nvarchar(100) not null,
	[FormattedContent] nvarchar(max) not null,
	[LinkedPages] nvarchar(max) not null,
	constraint PK_PagesContentCache primary key ([Page], [Namespace])
)