CREATE FUNCTION [dbo].[tfnFieldPermissions]
()
RETURNS @RolePermissions TABLE
(
	ID INT IDENTITY(1,1),
	bitID INT,
	Name VARCHAR(128)
)
AS
BEGIN
	Process:
	INSERT INTO @RolePermissions (bitID, Name)
		SELECT TOP 1
		  FieldID
		 , FieldName
		FROM lkpFields
		WHERE IsDeleted = 0
		AND FieldID NOT IN (SELECT bitID FROM @RolePermissions)
	DECLARE @count INT = @@ROWCOUNT

	INSERT INTO @RolePermissions  (bitID, Name)
		SELECT PermissionID
			, PermName
		FROM lkpPermissions

	IF (@count!=0)
		GOTO Process
	ELSE
		Update @RolePermissions SET bitID = POWER(2,ID)
	RETURN
END