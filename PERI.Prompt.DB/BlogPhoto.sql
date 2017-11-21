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