/* =============================================
   Author:		Nader Dibai
   Create date: 2017.05.26
   Description:	Inserts/updates record into mapUserRoles

   Return Codes:
   -1	@UserID, @RoleID, @TypeSecurityID are required
   -2	@CreatorID is required
   -3	Update failed: role deleted/or doesnt exist

   Useage:
	EXEC uspUserRoleSet @UserRoleID = 52, @UserID = 1, @RoleID = 79, @TypeSecurityID = 19, @FilterObjectID = 64, @CreatorID = 1

   Revision History:
   2017.05.30	AR	Added update support for "created","createdby"
   2017.05.30	AR	Added rollback support
   2017.05.31	AR	Renamed @RoleIDs to @RoleData
   2017.06.13	AR	Removed UTCDATE insert for Created column (default already)
   2017.10.27	BZ	Rewrote for new design of UserRole Mechanisum 
   ============================================= */
CREATE PROCEDURE [dbo].[uspUserRoleSet]
(
	@UserRoleID INT = NULL,
	@UserID	INT	= NULL,
	@RoleID INT = NULL,
	@TypeSecurityID INT = NULL,
	@FilterObjectID INT = 0,
	@IsDeleted BIT = 0,
	@CreatorID INT = NULL
)
AS
BEGIN  
	SET NOCOUNT ON;

	IF ISNULL(@UserID, 0) = 0 OR ISNULL(@RoleID, 0) = 0 OR ISNULL(@TypeSecurityID, 0) = 0
		RETURN -1

	IF ISNULL(@CreatorID, 0) = 0
		RETURN -2

	IF ISNULL(@UserRoleID, 0) = 0
		GOTO InsertLine
	ELSE
		GOTO UpdateLine

InsertLine:
	INSERT INTO mapUserRoles (UserID, RoleID, FilterObjectTypeID, FilterTypeID, FilterObjectID, CreatedBy, IsDeleted)
	SELECT @UserID, r.RoleID, FilterObjectTypeID, FilterTypeID, @FilterObjectID, @CreatorID, @IsDeleted
	FROM lkpObjectTypeSecurity os
		INNER JOIN Roles r ON os.ObjectTypeID = r.ObjectTypeID
	WHERE os.TypeSecurityID = @TypeSecurityID AND r.RoleID = @RoleID

	SET  @UserRoleID = SCOPE_IDENTITY();
	GOTO ReturnSelect

UpdateLine:

	BEGIN TRANSACTION 
		UPDATE mapUserRoles
			SET RoleID = ur.RoleID,
				FilterObjectTypeID = ur.FilterObjectTypeID,
				FilterTypeID = ur.FilterTypeID,
				IsDeleted = @IsDeleted,
				FilterObjectID = @FilterObjectID,
				ModifiedBy = @CreatorID
		FROM (SELECT FilterTypeID, FilterObjectTypeID, RoleID
			FROM lkpObjectTypeSecurity os
				INNER JOIN Roles r ON os.ObjectTypeID = r.ObjectTypeID
			WHERE os.TypeSecurityID = @TypeSecurityID AND r.RoleID = @RoleID) ur
		WHERE UserRoleID = @UserRoleID

		IF @@error != 0
		BEGIN
			ROLLBACK TRANSACTION
			RETURN -3
		END
		ELSE
			COMMIT TRANSACTION
	GOTO ReturnSelect

ReturnSelect:
	SELECT @UserRoleID 'UserRoleID'
END
