create table SectionItem
(
	SectionItemId integer primary key identity(1,1),
	SectionId int not null,
	Title  varchar(50) not null,
	[Body]  varchar(2000),
	[Order] int not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
    	DateInactive datetime null,

	constraint FK_SectionItem_Section foreign key (SectionId)
		references [Section] (SectionId) on delete cascade
);