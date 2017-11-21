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