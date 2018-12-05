/* =============================================
   Author:			Nathan Ayers
   Create date:		2017.10.27
   Description:		Retrieves the list of fields for a given Object the user has access to
   Usage:			EXEC uspFieldPermissionsGet @UserID = 64, @ObjectTypeID = 19, @ObjectID = 100009
   			
   Revision History:
	
   ============================================= */


CREATE PROCEDURE [dbo].[uspFieldPermissionsGet]
	@UserID INT = NULL,
	@ObjectTypeID INT = NULL,
	@ObjectID INT = NULL,
	@Interface VARCHAR(250) = NULL
AS
BEGIN
	--Declare a table variable to store the roles for a given object the user has
	DECLARE @UserRoles TABLE (ObjectID INT, RoleID INT)

	--Find the roles the user has assigned to the given object
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

	--Return the list of fields the user has access to based on their roles
	SELECT f.FieldID, f.FieldName, MAX(CAST(rfp.CanEdit AS INT)) 'CanEdit'
	FROM lkpFields f
	INNER JOIN mapRoleFieldPermissions rfp ON f.FieldID = rfp.FieldID AND rfp.IsDeleted = 0
	INNER JOIN Roles r ON rfp.RoleID = r.RoleID AND r.ObjectTypeID = @ObjectTypeID AND r.IsDeleted = 0
	INNER JOIN @UserRoles ur ON r.RoleID = ur.RoleID
	GROUP BY f.FieldID, f.FieldName
END