create table [Menu]
(
	MenuId integer primary key identity(1,1),
	Name varchar(50) not null,	

	constraint UQ_Menu_Name unique (Name)
);
