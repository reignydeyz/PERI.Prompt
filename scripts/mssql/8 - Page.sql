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

create table PagePhoto
(
	PageId int not null,
	PhotoId int not null,

	primary key(PageId, PhotoId),	

	constraint UQ_PagePhoto_PageId_PhotoId unique (PageId, PhotoId),
	constraint FK_PagePhoto_Page foreign key (PageId)
		references [Page] (PageId) on delete cascade,
	constraint FK_PagePhoto_Photo foreign key (PhotoId)
		references [Photo] (PhotoId) on delete cascade
)