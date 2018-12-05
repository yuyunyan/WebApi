/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.10.30
   Description:	Returns a list of permissions for a given UserID, ObjectTypeID and ObjectID
   Usage:	EXEC uspUserObjectSecurityGet @UserID = 64, @ObjectTypeID = 19, @ObjectID = 100013
			EXEC uspUserObjectSecurityGet @UserID = 64, @ObjectTypeID = 19, @ObjectID = 0       
			
   Return Codes:

   Revision History:
			2017.11.07	NA	Added ability to pass in 0 for ObjectID to get permissions for creating that ObjectType
   ============================================= */


CREATE PROCEDURE [dbo].[uspUserObjectSecurityGet]
	@UserID INT = NULL, 
	@ObjectTypeID INT = NULL, 
	@ObjectID INT = NULL, 
	@Interface VARCHAR(200) = NULL
AS
BEGIN	
	DECLARE @UserRoles TABLE (ObjectID INT, RoleID INT)

	IF @ObjectID = 0
	BEGIN
		INSERT INTO @UserRoles
		SELECT 0, ua.RoleID
		FROM vwUserAccess ua			
		WHERE ua.UserID = @UserID 
			AND ((ua.FilterObjectID = @UserID AND ua.FilterTypeID = 1) OR (ua.FilterTypeID = 3)) 
			AND FilterObjectTypeID = @ObjectTypeID 
			AND ua.ObjectTypeID = @ObjectTypeID
	END
	ELSE
	BEGIN
		IF @ObjectTypeID = 1
			INSERT @UserRoles EXECUTE uspAccountSecurityGet @UserID = @UserID, @AccountID = @ObjectID
		IF @ObjectTypeID = 2
			INSERT @UserRoles EXECUTE uspContactSecurityGet @UserID = @UserID, @ContactID = @ObjectID
		IF @ObjectTypeID = 16
			INSERT @UserRoles EXECUTE uspSalesOrderSecurityGet @UserID = @UserID, @SalesOrderID = @ObjectID
		IF @ObjectTypeID = 19
			INSERT @UserRoles EXECUTE uspQuoteSecurityGet @UserID = @UserID, @QuoteID = @ObjectID
		IF @ObjectTypeID = 22
			INSERT @UserRoles EXECUTE uspPurchaseOrderSecurityGet @UserID = @UserID, @PurchaseOrderID = @ObjectID
		IF @ObjectTypeID = 25
			INSERT @UserRoles EXECUTE uspItemListSecurityGet @UserID = @UserID, @ItemListID = @ObjectID
		IF @ObjectTypeID = 27
			INSERT @UserRoles EXECUTE uspVendorRFQSecurityGet @UserID = @UserID, @VendorRFQID = @ObjectID
		IF @ObjectTypeID = 103
			INSERT @UserRoles EXECUTE uspItemSecurityGet @UserID = @UserID, @ItemID = @ObjectID
		IF @ObjectTypeID = 104
			INSERT @UserRoles EXECUTE uspQCInspectionSecurityGet @UserID = @UserID, @InspectionID = @ObjectID
	END --IF/ELSE

	SELECT f.FieldID, f.FieldName 'Name', MAX(CAST(rfp.CanEdit AS INT)) 'CanEdit'
	FROM lkpFields f
		INNER JOIN mapRoleFieldPermissions rfp ON f.FieldID = rfp.FieldID AND rfp.IsDeleted = 0
		INNER JOIN Roles r ON rfp.RoleID = r.RoleID AND r.ObjectTypeID = @ObjectTypeID AND r.IsDeleted = 0
		INNER JOIN @UserRoles ur ON r.RoleID = ur.RoleID
	GROUP BY f.FieldID, f.FieldName
	
	UNION
	
	SELECT NULL, p.PermName 'Name', NULL
	FROM lkpPermissions p
		INNER JOIN mapRolePermissions rp ON  p.PermissionID = rp.PermissionID AND rp.IsDeleted = 0
		INNER JOIN @UserRoles ur ON rp.RoleID = ur.RoleID
	WHERE p.IsDeleted = 0 AND p.IsObjectSpecific = 1
END --Procedure
