create table [Section]
(
	SectionId integer primary key identity(1,1),
	TemplateId int not null,
	Name varchar(50) not null,	

	constraint UQ_Section_TemplateId_Name unique (TemplateId, Name),
	constraint FK_Template_TemplateId foreign key (TemplateId)
		references [Template] (TemplateId) on delete cascade
);
