create table SectionProperty
(
	SectionPropertyId integer primary key identity(1,1),
	SectionId int not null,
	Name varchar(50) not null,

	constraint UQ_Section_SectionId_Name unique (SectionId, Name),
	constraint FK_SectionProperty_Section foreign key (SectionId)
		references Section (SectionId) on delete cascade
);