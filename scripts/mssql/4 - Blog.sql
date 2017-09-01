create table [Blog]
(
	BlogId  integer primary key identity(1,1),
	Title  varchar(50) not null,
	[Body]  varchar(2000) not null,
	DatePublished datetime not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
    DateInactive datetime null
);

create table Category
(
	CategoryId  integer primary key identity(1,1),
	Name  varchar(50) not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
    DateInactive datetime null,
	
	constraint UQ_Category_Name unique (Name)
);

insert into Category(Name, CreatedBy, DateCreated, ModifiedBy, DateModified) values('Default', 'system', getdate(), 'system', getdate() );

create table BlogCategory
(
	BlogId int not null,
	CategoryId int not null,

	primary key(BlogId, CategoryId),	

	constraint UQ_BlogCategory_BlogId_CategoryId unique (BlogId, CategoryId),
	constraint FK_BlogCategory_Blog foreign key (BlogId)
		references [Blog] (BlogId) on delete cascade,
	constraint FK_BlogCategory_Category foreign key (CategoryId)
		references [Category] (CategoryId) on delete cascade
);

create table BlogTag
(
	BlogId int not null,
	TagId int not null,

	primary key(BlogId, TagId),	

	constraint UQ_BlogTag_BlogId_TagId unique (BlogId, TagId),
	constraint FK_BlogTag_Blog foreign key (BlogId)
		references [Blog] (BlogId) on delete cascade,
	constraint FK_BlogTag_Tag foreign key (TagId)
		references [Tag] (TagId) on delete cascade
);

create table BlogPhoto
(
	BlogId int not null,
	PhotoId int not null,

	primary key(BlogId, PhotoId),	

	constraint UQ_BlogPhoto_BlogId_PhotoId unique (BlogId, PhotoId),
	constraint FK_BlogPhoto_Blog foreign key (BlogId)
		references [Blog] (BlogId) on delete cascade,
	constraint FK_BlogPhoto_Photo foreign key (PhotoId)
		references [Photo] (PhotoId) on delete cascade
)