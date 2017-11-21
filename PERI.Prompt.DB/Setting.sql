create table [Setting]
(
	SettingId integer primary key identity(1,1),
	[Group] varchar(50) not null,
	[Key] varchar(50) not null,
	Value varchar(50),
	[Type] varchar(50) not null,
	Priority int not null,
	Required bit not null
);