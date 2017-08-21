create table Tag
(
	TagId  integer primary key identity(1,1),
	Name  varchar(30) not null,

	constraint UQ_Tag_Name unique (Name)
)