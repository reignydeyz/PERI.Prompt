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