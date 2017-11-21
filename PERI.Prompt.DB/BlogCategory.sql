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