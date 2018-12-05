/* =============================================
   Author:			Berry Zhong
   Create date:		2017.10.20
   Description:		Retrieves a list of sales order and roles for a given user
   Usage:			EXEC uspQuoteSecurityGet @UserID = 64
   			
   Revision History:
	
   ============================================= */
CREATE PROCEDURE [dbo].[uspSalesOrderSecurityGet]
	@UserID INT = NULL,
	@SalesOrderID INT = NULL
AS
BEGIN
	DECLARE @ObjectTypeID INT =  16;

	--Own the Sales Order
	SELECT so.SalesOrderID, ua.RoleID
	FROM SalesOrders so
		INNER JOIN mapOwnership o ON o.ObjectTypeID = @ObjectTypeID AND so.SalesOrderID = o.ObjectID AND o.IsGroup = 0
		INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND so.SalesOrderID = ISNULL(@SalesOrderID, so.SalesOrderID)
	UNION
	--Own the Account for the Sales Order
	SELECT so.SalesOrderID, ua.RoleID
	FROM SalesOrders so
		INNER JOIN mapOwnership o ON o.ObjectTypeID = 1 AND o.ObjectID = so.AccountID AND o.IsGroup = 0
		INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = 1
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND so.SalesOrderID = ISNULL(@SalesOrderID, so.SalesOrderID)
	UNION
	--Access the Account that sales order belongs to 
	SELECT so.SalesOrderID, ua.RoleID
	FROM SalesOrders so
		INNER JOIN vwUserAccess ua ON so.AccountID = CASE WHEN ua.FilterObjectID = 0 THEN so.AccountID ELSE ua.FilterObjectID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 1
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND so.SalesOrderID = ISNULL(@SalesOrderID, so.SalesOrderID)
	UNION
	--Contact Ownership
	SELECT so.SalesOrderID, ua.RoleID
	FROM SalesOrders so
	  INNER JOIN mapOwnership o ON so.ContactID = o.ObjectID AND o.ObjectTypeID = 2 AND o.IsGroup = 0
	  INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = 2
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND so.SalesOrderID = ISNULL(@SalesOrderID, so.SalesOrderID)
	UNION
	--Access the account hierarchy
	SELECT so.SalesOrderID, ua.RoleID
	FROM SalesOrders so
		INNER JOIN Accounts a ON a.AccountID = so.AccountID
		INNER JOIN vwUserAccess ua ON a.AccountHierarchyID = CASE WHEN ua.FilterObjectID = 0 THEN a.AccountHierarchyID ELSE ua.FilterObjectID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 4
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND so.SalesOrderID = ISNULL(@SalesOrderID, so.SalesOrderID)
	UNION
	--Hierarchy Parent Node Access
	SELECT so.SalesOrderID, ua.RoleID
	FROM SalesOrders so
	  INNER JOIN Accounts a ON so.AccountID = a.AccountID  
	  INNER JOIN AccountHierarchies ah ON a.AccountHierarchyID = ah.AccountHierarchyID
	  INNER JOIN vwUserAccess ua ON ah.ParentID = CASE WHEN ua.FilterObjectID = 0 THEN ah.ParentID ELSE ua.FilterObjectID END
		AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 4
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND so.SalesOrderID = ISNULL(@SalesOrderID, so.SalesOrderID)
	UNION
	--Region Access
	SELECT so.SalesOrderID, ua.RoleID
	FROM SalesOrders so
		INNER JOIN Accounts a ON a.AccountID = so.AccountID
		INNER JOIN AccountHierarchies ah ON ah.AccountHierarchyID = a.AccountHierarchyID
		INNER JOIN vwUserAccess ua ON ah.RegionID = CASE WHEN ua.FilterObjectID = 0 THEN ah.RegionID ELSE ua.FilterObjectID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 3
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND so.SalesOrderID = ISNULL(@SalesOrderID, so.SalesOrderID)
	UNION
	--All Sales Orders
	SELECT so.SalesOrderID, ua.RoleID
	FROM SalesOrders so
		INNER JOIN vwUserAccess ua ON so.SalesOrderID = CASE WHEN ua.FilterObjectID = 0 THEN so.SalesOrderID ELSE ua.FilterObjectID END 
			AND  ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 16
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND so.SalesOrderID = ISNULL(@SalesOrderID, so.SalesOrderID)

END
