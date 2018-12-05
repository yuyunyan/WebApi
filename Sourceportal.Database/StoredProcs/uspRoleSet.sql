/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.05.12
   Description:	Inserts/updates record into Roles and mapRolePermissions

   Return Codes:
   -1	Insert Failed, check input variables
   -2	Update failed: role deleted/or doesnt exist

   Revision History:
   2017.05.30	AR	Replaced permission values with int scalar functions
   2017.11.16	BZ	Update the stored procedure just for Insert/Update RoleDetail
   ============================================= */
CREATE PROCEDURE [dbo].[uspRoleSet]
(
	@RoleID				INT				= NULL OUTPUT,
	@RoleName			VARCHAR(128)	= NULL,
	@ObjectTypeID		INT				= NULL,
	--@CanEditFieldIDs	NVARCHAR(MAX)	= NULL,
	--@CanViewFieldIDs	NVARCHAR(MAX)	= NULL,
	--@PermissionIDs		NVARCHAR(MAX)	= NULL,
	--@NavIDs				NVARCHAR(MAX)	= NULL,
	@CreatorID			INT				= NULL,
	@IsDeleted			BIT				= 0
)
AS
BEGIN  
	SET NOCOUNT ON;
	
	IF (ISNULL(@RoleID, 0)=0)
		GOTO InsertRole
	ELSE
		GOTO UpdateRole

InsertRole:
BEGIN
	INSERT INTO Roles (RoleName, ObjectTypeID, CreatedBy, IsDeleted)
	VALUES (@RoleName, @ObjectTypeID, @CreatorID, @IsDeleted)
	SET @RoleID = @@IDENTITY

	IF (ISNULL(@RoleID,0)!=0 )
		GOTO ReturnSelect
	ELSE
		RETURN -1	--Insert Failed
END

UpdateRole:
BEGIN
	UPDATE Roles
	SET RoleName = @RoleName,
	ModifiedBy = @CreatorID,
	Modified = GETUTCDATE(),
	IsDeleted = @IsDeleted
	WHERE RoleID = @RoleID
	DECLARE @UpdateRowCount INT = @@ROWCOUNT

	IF (@UpdateRowCount=0)
		RETURN -2
	ELSE
		GOTO ReturnSelect;

END

ReturnSelect:
BEGIN
	SELECT @RoleID 'RoleID'
END
/*InsertPermissions:
BEGIN
	DELETE mapRolePermissions
	WHERE RoleID = @RoleID
	AND IsDeleted = 0
	
	IF (@ObjectTypeID = dbo.fnNavigationObjectTypeID() AND @NavIDs IS NOT NULL)
		BEGIN
			INSERT INTO dbo.mapRolePermissions (RoleID, PermissionID, ObjectID, CreatedBy)
			SELECT @RoleID, dbo.fnCanViewLinkPermissionTypeID(), result.value, @CreatorID
			FROM  OPENJSON(@NavIDs) AS result									
		END

		IF (@ObjectTypeID IN (dbo.fnContactObjectTypeID(), dbo.fnAccountObjectTypeID(),
		dbo.fnSalesOrderObjectTypeID(), dbo.fnUserGroupObjectTypeID(), dbo.fnUserObjectTypeID() ))
		BEGIN

			INSERT INTO dbo.mapRolePermissions (RoleID, PermissionID, ObjectID, CreatedBy)
			SELECT @RoleID, dbo.fnCanEditAccountFieldPermissionTypeID(), result.value, @CreatorID
			FROM OPENJSON(@CanEditFieldIDs) AS result

			INSERT INTO dbo.mapRolePermissions (RoleID, PermissionID, ObjectID, CreatedBy)
			SELECT @RoleID, dbo.fnCanViewAccountFieldPermissionTypeID(), result.value, @CreatorID
			FROM OPENJSON(@CanViewFieldIDs) AS result

			INSERT INTO dbo.mapRolePermissions (RoleID, PermissionID, ObjectID, CreatedBy)
			SELECT @RoleID, result.value, 0, @CreatorID
			FROM OPENJSON(@PermissionIDs) AS result

		END	
		SELECT @RoleID NewRoleID
		RETURN
END*/
END
