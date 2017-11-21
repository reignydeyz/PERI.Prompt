create table Category
(
	CategoryId  integer primary key identity(1,1),
	BlogSortOrderId int not null,
	Name  varchar(50) not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
    DateInactive datetime null,
	
	constraint UQ_Category_Name unique (Name),
	constraint FK_Category_BlogSortOrder foreign key (BlogSortOrderId)
		references [BlogSortOrder] (BlogSortOrderId) on delete cascade
);