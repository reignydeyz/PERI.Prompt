create table Template
(
	TemplateId integer primary key identity(1,1),
	Name varchar(50),
	DateInactive datetime null,

	constraint UQ_Template_Name unique (Name)
);

create table TemplateSetting
(
	SettingId integer primary key identity(1,1),
	TemplateId  int not null,
	[Key] varchar(50) not null,
	Value varchar(50) not null,

	constraint UQ_TemplateSetting_TemplateId_Key unique (TemplateId, [Key]),
	constraint FK_TemplateSetting_Template foreign key (TemplateId)
		references Template (TemplateId) on delete cascade
)