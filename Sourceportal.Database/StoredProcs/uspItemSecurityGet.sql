/* =============================================
   Author:			Berry Zhong
   Create date:		2017.10.23
   Description:		Retrieves a list of purchase-order and roles for a given user
   Usage:			EXEC uspPurchaseOrderSecurityGet @UserID = 64
   			
   Revision History:
	
   ============================================= */
CREATE PROCEDURE [dbo].[uspItemSecurityGet]
	@UserID INT = NULL,
	@ItemID INT = NULL
AS
BEGIN
	DECLARE @ObjectTypeID INT =  103;
	--All Purchase-Orders
	SELECT i.ItemID, ua.RoleID
	FROM Items i
		INNER JOIN vwUserAccess ua ON i.ItemID = CASE WHEN ua.FilterObjectID = 0 THEN i.ItemID ELSE ua.FilterObjectTypeID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND i.ItemID = ISNULL(@ItemID, i.ItemID)
END
