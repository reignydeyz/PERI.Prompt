create table [Blog]
(
	BlogId  integer primary key identity(1,1),
	Title  varchar(50) not null,
	[Body]  varchar(max) not null,
	DatePublished datetime not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
    DateInactive datetime null
);