create table [Section]
(
	SectionId integer primary key identity(1,1),
	TemplateId int not null,
	Name varchar(50) not null,	

	constraint UQ_Section_TemplateId_Name unique (TemplateId, Name),
	constraint FK_Template_TemplateId foreign key (TemplateId)
		references [Template] (TemplateId) on delete cascade
);

create table SectionProperty
(
	SectionPropertyId integer primary key identity(1,1),
	SectionId int not null,
	Name varchar(50) not null,

	constraint UQ_Section_SectionId_Name unique (SectionId, Name),
	constraint FK_SectionProperty_Section foreign key (SectionId)
		references Section (SectionId) on delete cascade
);

create table SectionItem
(
	SectionItemId integer primary key identity(1,1),
	SectionId int not null,
	Title  varchar(50) not null,
	[Body]  varchar(2000),
	[Order] int not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
    	DateInactive datetime null,

	constraint FK_SectionItem_Section foreign key (SectionId)
		references [Section] (SectionId) on delete cascade
);

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

create table SectionItemProperty
(
	SectionPropertyId int,
	SectionItemId int,
	Value varchar(50),

	primary key(SectionPropertyId, SectionItemId),

	constraint UQ_SectionItemProperty_SectionPropertyId_SectionItemId unique (SectionPropertyId, SectionItemId),
	constraint FK_SectionItemProperty_SectionProperty foreign key (SectionPropertyId)
		references [SectionProperty] (SectionPropertyId) on delete cascade,
	constraint FK_SectionItemProperty_SectionItem foreign key (SectionItemId)
		references [SectionItem] (SectionItemId) /*on delete cascade*/
)