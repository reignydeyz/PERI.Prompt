create table Template
(
	TemplateId integer primary key identity(1,1),
	Name varchar(50),
	DateInactive datetime null,

	constraint UQ_Template_Name unique (Name)
);