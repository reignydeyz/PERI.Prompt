create table Photo
(
	PhotoId integer primary key identity(1,1),
	[Url] varchar(2000) not null
);

create table Gallery
(
	GalleryId integer primary key identity(1,1),
	[Name] varchar(50) not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
    DateInactive datetime null
);

insert into Gallery (Name, CreatedBy, DateCreated, ModifiedBy, DateModified) values('System', 'System', getdate(), 'System', getdate());

create table GalleryPhoto
(
	GalleryId int not null,
	PhotoId int not null,
	Title  varchar(50) not null,
	[Description]  varchar(2000) not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
    DateInactive datetime null,

	primary key(GalleryId, PhotoId),	

	constraint UQ_GalleryPhoto_GalleryId_PhotoId unique (GalleryId, PhotoId),
	constraint FK_GalleryPhoto_Gallery foreign key (GalleryId)
		references [Gallery] (GalleryId) on delete cascade,
	constraint FK_GalleryPhoto_Photo foreign key (PhotoId)
		references [Photo] (PhotoId) on delete cascade
)