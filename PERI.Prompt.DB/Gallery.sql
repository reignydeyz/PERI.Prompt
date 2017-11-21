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