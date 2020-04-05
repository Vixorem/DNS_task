CREATE TABLE [Departments] (
	[Id] INT identity(1, 1) PRIMARY KEY
	,[Name] VARCHAR(50) NOT NULL UNIQUE
	);

CREATE TABLE [Positions] (
	[Id] INT identity(1, 1) PRIMARY KEY
	,[Name] VARCHAR(50) NOT NULL UNIQUE
	);

CREATE TABLE [Employees] (
	[Id] INT identity(1, 1) PRIMARY KEY
	,[Hid] HIERARCHYID NOT NULL
	,[Name] VARCHAR(50) NULL
	,[Secondname] VARCHAR(50)
	,[Surname] VARCHAR(50)
	,[BossId] INT FOREIGN KEY REFERENCES [Employees]([Id])
	,[PositionId] INT FOREIGN KEY REFERENCES [Positions]([Id])
	,[DepartmentId] INT FOREIGN KEY REFERENCES [Departments]([Id])
	,[RecruitDate] DATE NOT NULL
	);
