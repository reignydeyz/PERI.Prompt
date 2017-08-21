create table [Menu]
(
	MenuId integer primary key identity(1,1),
	Name varchar(50) not null,	

	constraint UQ_Menu_Name unique (Name)
);

insert into [Menu](Name) values('System');

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

insert into [MenuItem](MenuId, Label, Url, [Order]) values(1, 'Home', '~/', 0);

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