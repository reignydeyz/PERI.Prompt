create table [User]
(
	UserId integer primary key IDENTITY(1,1),
	FirstName varchar(30) not null,
	LastName varchar(30) not null,
	Email varchar(50) not null unique,
	RoleId int not null,
	PasswordHash nvarchar(128) not null,
	PasswordSalt nvarchar(128) not null,
	PasswordExpiry datetime null,
	LastSessionId varchar(50) null,
    LastLoginDate datetime null,
    LastPasswordChanged datetime null,
    FailedPasswordCount int not null,
    LastFailedPasswordAttempt datetime null,
    ConfirmationCode varchar(50) null,
    ConfirmationExpiry datetime null,
    DateConfirmed datetime null,
    DateCreated datetime not null,
    DateInactive datetime null,

	constraint UQ_User_FirstName_LastName unique (FirstName, LastName),
	constraint UQ_User_Email unique (Email),
	constraint FK_User_Role foreign key (RoleId)
		references [Role] (RoleId) on delete cascade
);