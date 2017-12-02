create table EventPhoto
(
	EventId int not null,
	PhotoId int not null,

	primary key(EventId, PhotoId),	

	constraint UQ_EventPhoto_EventId_PhotoId unique (EventId, PhotoId),
	constraint FK_EventPhoto_Event foreign key (EventId)
		references [Event] (EventId) on delete cascade,
	constraint FK_EventPhoto_Photo foreign key (PhotoId)
		references [Photo] (PhotoId) on delete cascade
)