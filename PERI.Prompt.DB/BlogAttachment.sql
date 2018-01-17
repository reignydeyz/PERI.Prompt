create table BlogAttachment
(
	BlogId int not null,
	AttachmentId int not null,

	primary key(BlogId, AttachmentId),	

	constraint UQ_BlogAttachment_BlogId_AttachmentId unique (BlogId, AttachmentId),
	constraint FK_BlogAttachment_Blog foreign key (BlogId)
		references [Blog] (BlogId) on delete cascade,
	constraint FK_BlogAttachment_Attachment foreign key (AttachmentId)
		references [Attachment] (AttachmentId) on delete cascade
)