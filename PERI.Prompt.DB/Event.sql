create table [Event]
(
	EventId integer primary key identity(1,1),
	[Name] varchar(50) not null,
	[Description] varchar(max) not null,
	[Location] varchar(200) not null,
	[Time] datetime not null,
	CreatedBy varchar(50) not null,
	DateCreated datetime not null,
	ModifiedBy varchar(50) not null,
	DateModified datetime not null,
    DateInactive datetime null
);