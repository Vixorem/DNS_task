SET
  ANSI_NULLS ON
GO
SET
  QUOTED_IDENTIFIER ON
GO
  CREATE PROCEDURE AddDepartment @Name varchar(50) AS BEGIN
SET
  NOCOUNT ON;
INSERT INTO dbo.Departments ([Name])
VALUES
  (@Name);
END
GO
  CREATE PROCEDURE AddPosition @Name varchar(50) AS BEGIN
SET
  NOCOUNT ON;
INSERT dbo.Positions
VALUES
  (@Name);
END
GO
  CREATE PROCEDURE AddEmployee @Name varchar(50),
  @Secondname varchar(50),
  @Surname varchar(50),
  @BossId int,
  @PosId int,
  @DepId int,
  @Rdate date AS BEGIN
SET
  NOCOUNT ON;
DECLARE @BossExist int,
  @BossHid hierarchyid,
  @LastHid hierarchyid;
SELECT
  @BossExist = count(*)
FROM dbo.Employees
WHERE
  Hid = hierarchyid :: GetRoot();
IF @BossExist != 0
  AND @BossId is NULL RETURN;
IF @BossId is NULL
INSERT dbo.Employees (
    [Hid],
    [Name],
    [Secondname],
    [Surname],
    [BossId],
    [PositionId],
    [DepartmentId],
    [RecruitDate]
  )
VALUES
  (
    hierarchyid :: GetRoot(),
    @Name,
    @Secondname,
    @Surname,
    @BossId,
    @PosId,
    @DepId,
    @Rdate
  );
  ELSE BEGIN
SELECT
  @BossHid = (
    SELECT
      Hid
    FROM dbo.Employees
    WHERE
      Id = @BossId
  );
SELECT
  @LastHid = MAX(Hid)
FROM dbo.Employees
WHERE
  Hid.GetAncestor(1) = @BossHid;
INSERT dbo.Employees (
    [Hid],
    [Name],
    [Secondname],
    [Surname],
    [BossId],
    [PositionId],
    [DepartmentId],
    [RecruitDate]
  )
VALUES
  (
    @BossHid.GetDescendant(@LastHid, NULL),
    @Name,
    @Secondname,
    @Surname,
    @BossId,
    @PosId,
    @DepId,
    @Rdate
  );
END;
END
GO
  CREATE PROCEDURE UpdateEmployee @Name varchar(50),
  @Secondname varchar(50),
  @Surname varchar(50),
  @BossId int,
  @PosId int,
  @DepId int,
  @Rdate date AS BEGIN
SET
  NOCOUNT ON;
DECLARE @BossExist int;
SELECT
  @BossExist = count(*)
FROM dbo.Employees
WHERE
  Hid = hierarchyid :: GetRoot();
IF @BossExist != 0
  AND @BossId is NULL RETURN;
UPDATE dbo.Employees
SET
  [Name] = @Name,
  [Secondname] = @Secondname,
  [Surname] = @Surname,
  [BossId] = @BossId,
  [PositionId] = @PosId,
  [DepartmentId] = @DepId,
  [RecruitDate] = @Rdate;
END
GO
  CREATE PROCEDURE DeleteEmployee @Id int AS BEGIN
SET
  NOCOUNT ON;
DELETE FROM dbo.Employees
WHERE
  [Id] = @Id;
END
GO
  CREATE PROCEDURE FetchAllEmployees AS BEGIN
SET
  NOCOUNT ON;
SELECT
  *
FROM (
    (
      SELECT
        [Emp].[Id],
        [Emp].[Name],
        [Emp].[Secondname],
        [Emp].[Surname],
        [Emp].[BossId],
        [Boss].[Surname] AS BossSurname,
        [Emp].[PositionId],
        [Pos].[Name] AS PosName,
        [Emp].[DepartmentId],
        [Dep].[Name] AS DepName,
        [Emp].[RecruitDate]
      FROM dbo.Employees as Emp
      JOIN dbo.Employees as Boss ON Emp.Hid.GetAncestor(1) = Boss.Hid
      JOIN dbo.Departments as Dep ON Emp.DepartmentId = Dep.Id
      JOIN dbo.Positions as Pos ON Emp.PositionId = Pos.Id
    )
    UNION
      (
        SELECT
          [Emp].[Id],
          [Emp].[Name],
          [Emp].[Secondname],
          [Emp].[Surname],
          [Emp].[BossId],
          '' AS BossSurname,
          [Emp].[PositionId],
          [Pos].[Name] AS PosName,
          [Emp].[DepartmentId],
          [Dep].[Name] AS DepName,
          [Emp].[RecruitDate]
        FROM dbo.Employees as Emp
        JOIN dbo.Employees as Boss ON (
            Emp.BossId is NULL
            and Boss.Hid = hierarchyid :: GetRoot()
          )
        JOIN dbo.Departments as Dep ON Emp.DepartmentId = Dep.Id
        JOIN dbo.Positions as Pos ON Emp.PositionId = Pos.Id
      )
  ) AS T
END
GO
  CREATE PROCEDURE FetchEmployeesRange @FetchNum int,
  @PageNum int AS BEGIN
SET
  NOCOUNT ON;
SELECT
  *
FROM (
    (
      SELECT
        [Emp].[Id],
        [Emp].[Name],
        [Emp].[Secondname],
        [Emp].[Surname],
        [Emp].[BossId],
        [Boss].[Surname] AS BossSurname,
        [Emp].[PositionId],
        [Pos].[Name] AS PosName,
        [Emp].[DepartmentId],
        [Dep].[Name] AS DepName,
        [Emp].[RecruitDate]
      FROM dbo.Employees as Emp
      JOIN dbo.Employees as Boss ON Emp.Hid.GetAncestor(1) = Boss.Hid
      JOIN dbo.Departments as Dep ON Emp.DepartmentId = Dep.Id
      JOIN dbo.Positions as Pos ON Emp.PositionId = Pos.Id
    )
    UNION
      (
        SELECT
          [Emp].[Id],
          [Emp].[Name],
          [Emp].[Secondname],
          [Emp].[Surname],
          [Emp].[BossId],
          '' AS BossSurname,
          [Emp].[PositionId],
          [Pos].[Name] AS PosName,
          [Emp].[DepartmentId],
          [Dep].[Name] AS DepName,
          [Emp].[RecruitDate]
        FROM dbo.Employees as Emp
        JOIN dbo.Employees as Boss ON (
            Emp.BossId is NULL
            and Boss.Hid = hierarchyid :: GetRoot()
          )
        JOIN dbo.Departments as Dep ON Emp.DepartmentId = Dep.Id
        JOIN dbo.Positions as Pos ON Emp.PositionId = Pos.Id
      )
  ) AS T
ORDER BY
  [T].[Id] OFFSET ((@PageNum - 1) * @FetchNum) ROWS FETCH NEXT @FetchNum ROWS ONLY;
END
GO
  CREATE PROCEDURE FetchAllPositions AS BEGIN
SET
  NOCOUNT ON;
SELECT
  [Id],
  [Name]
FROM dbo.Positions
ORDER BY
  [Id];
END
GO
  CREATE PROCEDURE FetchAllDepartments AS BEGIN
SET
  NOCOUNT ON;
SELECT
  [Id],
  [Name]
FROM dbo.Departments
ORDER BY
  [Id];
END
GO
  CREATE PROCEDURE EmployeesCount AS BEGIN
SET
  NOCOUNT ON;
SELECT
  count(*)
FROM dbo.Employees;
END
GO
  CREATE PROCEDURE PositionsCount AS BEGIN
SET
  NOCOUNT ON;
SELECT
  count(*)
FROM dbo.Positions;
END
GO
  CREATE PROCEDURE DepartmentsCount AS BEGIN
SET
  NOCOUNT ON;
SELECT
  count(*)
FROM dbo.Departments;
END
GO
  CREATE PROCEDURE FetchEmployeeById @Id int AS BEGIN
SET
  NOCOUNT ON;
SELECT
  [Emp].[Id],
  [Emp].[Name],
  [Emp].[Secondname],
  [Emp].[Surname],
  [Emp].[BossId],
  [Boss].[Surname],
  [Emp].[PositionId],
  [Pos].[Name],
  [Emp].[DepartmentId],
  [Dep].[Name],
  [Emp].[RecruitDate]
FROM dbo.Employees as Emp
JOIN dbo.Employees as Boss ON (
    Emp.Hid.GetAncestor(1) = Boss.Hid
    or Emp.Hid = hierarchyid :: GetRoot()
  )
  AND Emp.Hid != Boss.Hid
JOIN dbo.Departments as Dep ON Emp.DepartmentId = Dep.Id
JOIN dbo.Positions as Pos ON Emp.PositionId = Pos.Id
WHERE
  [Emp].[Id] = @Id;
END
GO
  CREATE PROCEDURE FetchDepartmentById @Id int AS BEGIN
SET
  NOCOUNT ON;
SELECT
  [Id],
  [Name]
FROM dbo.Departments
WHERE
  [Id] = @Id;
END
GO
  CREATE PROCEDURE FetchPositionById @Id int AS BEGIN
SET
  NOCOUNT ON;
SELECT
  [Id],
  [Name]
FROM dbo.Positions
WHERE
  [Id] = @Id;
END
GO
  CREATE PROCEDURE FetchBossesForId AS BEGIN
SET
  NOCOUNT ON;
END
GO