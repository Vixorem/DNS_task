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
DECLARE @DelHid HIERARCHYID
	,@BossHid HIERARCHYID
	,@BossId INT
	,@ChildrenCount INT

IF IS_MEMBER('db_owner') = 0
BEGIN
	SET @DelHid = (
			SELECT Hid
			FROM deleted
			);

	IF @DelHid = HIERARCHYID::GetRoot()
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
	WHERE Hid.GetAncestor(1) = @DelHid;

	IF @ChildrenCount = 0
		DELETE
		FROM dbo.Eployees
		WHERE Hid = @DelHid;
	ELSE
	BEGIN
		SET @BossHid = @DelHid.GetAncestor(1);

		@BossId = (
				SELECT Id
				FROM dbo.Employees
				WHERE Hid = @BossHid
				);

		UPDATE dbo.Employees
		SET [Hid] = Hid.GetReparentedValue(@DelHid, @BossHid)
			,[BossId] = @BossId
		WHERE Hid.IsDecendantOf(@DelHid) = 1;

		DELETE
		FROM dbo.Eployees
		WHERE Hid = @DelHid;
	END
END
GO


