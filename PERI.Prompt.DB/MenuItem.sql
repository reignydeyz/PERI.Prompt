create table [MenuItem]
(
	MenuItemId integer primary key identity(1,1),
	MenuId integer not null,
	Label varchar(50) not null,
	Url varchar(50) not null,
	[Order] int not null,

	constraint UQ_MenuItem_MenuId_Label unique (MenuId, Label),
	constraint FK_MenuItem_Menu foreign key (MenuId)
		references Menu (MenuId) on delete cascade
);