create table [Page]
(
	PageId integer primary key identity(1,1),
	Title varchar(50) not null unique,
	Permalink varchar(50) not null unique,
	Content varchar(max) not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
    	DateInactive datetime null
);