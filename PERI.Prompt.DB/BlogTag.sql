create table BlogTag
(
	BlogId int not null,
	TagId int not null,

	primary key(BlogId, TagId),	

	constraint UQ_BlogTag_BlogId_TagId unique (BlogId, TagId),
	constraint FK_BlogTag_Blog foreign key (BlogId)
		references [Blog] (BlogId) on delete cascade,
	constraint FK_BlogTag_Tag foreign key (TagId)
		references [Tag] (TagId) on delete cascade
);