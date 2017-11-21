create table [ChildMenuItem]
(
	ChildMenuItemId integer primary key identity(1,1),
	MenuItemId integer not null,
	Label varchar(50) not null,
	Url varchar(50) not null,
	[Order] int not null,

	constraint UQ_ChildMenuItem_MenuItemId_Label unique (MenuItemId, Label),
	constraint FK_ChildMenuItem_MenuItem foreign key (MenuItemId)
		references MenuItem (MenuItemId) on delete cascade
);