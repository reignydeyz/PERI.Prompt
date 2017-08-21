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

insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Config', 'Max user','0', 'number', 1, 'true');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Config', 'Allow signup','0', 'number', 1, 'true');

insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp server', null,'text',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp port',null,'number',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp user', null,'email',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp password', null,'password',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp use ssl', null,'text',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp display name', null,'text',  2, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Smtp', 'Smtp display email', null,'email',  2, 'false');

insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Recaptcha','Recaptcha public key', null,'text',  3, 'false');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Recaptcha','Recaptcha private key', null,'text',  3, 'false');

insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Site','Name', null,'text',  0, 'true');
insert into [Setting]([Group], [Key], [Value], [Type], Priority, Required) values('Site','Tagline', null,'text',  0, 'true');