USE empdb
GO

IF EXISTS (
		SELECT *
		FROM sys.triggers
		WHERE name = N'OnEmployeeDelete'
			AND parent_class_desc = N'empdb'
		)
	DROP TRIGGER OnEmployeeDelete ON DATABASE
GO

CREATE TRIGGER OnEmployeeDelete ON dbo.Employees
INSTEAD OF DELETE
AS
DECLARE @DelId INT
	,@ChildrenCount INT
	,@NewBossId INT;

IF IS_MEMBER('db_owner') = 0
BEGIN
	SELECT @DelId = (
			SELECT Id
			FROM deleted
			);

	IF @DelId IS NULL
	BEGIN
		RAISERROR (
				16
				,- 1
				,- 1
				,'You cannot delete the main boss'
				);

		RETURN;
	END

	SELECT @ChildrenCount = COUNT(*)
	FROM dbo.Employees
	WHERE [BossId] = @DelId;

	IF @ChildrenCount = 0
		DELETE
		FROM dbo.Eployees
		WHERE Id = @DelId;
	ELSE
	BEGIN
		SELECT @NewBossId = (
				SELECT BossId
				FROM dbo.Employees
				WHERE @DelId = Id
				);

		UPDATE dbo.Employees
		SET [BossId] = @NewBossId
		WHERE [BossId] = @NewBossId;

		DELETE
		FROM dbo.Eployees
		WHERE [Id] = @DelId;
	END
END
GO


