create table SectionItemPhoto
(
	SectionItemId int not null,
	PhotoId int not null,

	primary key(SectionItemId, PhotoId),	

	constraint UQ_SectionItemPhoto_SectionId_PhotoId unique (SectionItemId, PhotoId),
	constraint FK_SectionItemPhoto_Section foreign key (SectionItemId)
		references [SectionItem] (SectionItemId) on delete cascade,
	constraint FK_SectionItemPhoto_Photo foreign key (PhotoId)
		references [Photo] (PhotoId) on delete cascade
);