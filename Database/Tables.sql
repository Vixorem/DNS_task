create table [Departments] (
	[Id] int identity(1,1) primary key,
	[Name] varchar(50) not null unique
);

create table [Positions](
	[Id] int identity(1,1) primary key,
	[Name] varchar(50) not null unique
);

create table [Employees] (
	[Id] int identity(1,1) primary key,
	[Hid] hierarchyid not null,
	[Name] varchar(50) null,
	[Secondname] varchar(50),
	[Surname] varchar(50),
	[BossId] int foreign key references [Employees]([Id]),
	[PositionId] int foreign key references [Positions]([Id]),
	[DepartmentId] int foreign key references [Departments]([Id]),
	[RecruitDate] date not null
);