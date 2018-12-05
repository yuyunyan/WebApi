/* =============================================
   Author:			Berry Zhong
   Create date:		2017.10.23
   Description:		Retrieves a list of purchase-order and roles for a given user
   Usage:			EXEC uspPurchaseOrderSecurityGet @UserID = 64
   			
   Revision History:
	
   ============================================= */
CREATE PROCEDURE [dbo].[uspPurchaseOrderSecurityGet]
	@UserID INT = NULL,
	@PurchaseOrderID INT = NULL
AS
BEGIN
	DECLARE @ObjectTypeID INT =  22;
	--All Purchase-Orders
	SELECT po.PurchaseOrderID, ua.RoleID
	FROM PurchaseOrders po
		INNER JOIN vwUserAccess ua ON po.PurchaseOrderID = CASE WHEN ua.FilterObjectID = 0 THEN po.PurchaseOrderID ELSE ua.FilterObjectTypeID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND po.PurchaseOrderID = ISNULL(@PurchaseOrderID, po.PurchaseOrderID)
END
