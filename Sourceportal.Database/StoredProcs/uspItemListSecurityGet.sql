/* =============================================
   Author:			Berry Zhong
   Create date:		2017.10.20
   Description:		Retrieves a list of item-list and roles for a given user
   Usage:			EXEC uspItemListSecurityGet @UserID = 64
   			
   Revision History:
	
   ============================================= */
CREATE PROCEDURE [dbo].[uspItemListSecurityGet]
	@UserID INT = NULL,
	@ItemListID INT = NULL
AS
BEGIN
	DECLARE @ObjectTypeID INT =  25;
	--Own the Item List
	SELECT il.ItemListID, ua.RoleID
	FROM ItemLists il
		INNER JOIN mapOwnership o ON o.ObjectTypeID = @ObjectTypeID AND il.ItemListID = o.ObjectID AND o.IsGroup = 0
		INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND il.ItemListID = ISNULL(@ItemListID, il.ItemListID)
	UNION
	--Own the Account
	SELECT il.ItemListID, ua.RoleID
	FROM ItemLists il
		INNER JOIN mapOwnership o ON o.ObjectTypeID = 1 AND il.AccountID = o.ObjectID AND o.IsGroup = 0
		INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = 1
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND il.ItemListID = ISNULL(@ItemListID, il.ItemListID)
	UNION
	--Own the Contact
	SELECT il.ItemListID, ua.RoleID
	FROM ItemLists il
		INNER JOIN Contacts c ON c.ContactID = il.ContactID
		INNER JOIN mapOwnership o ON o.ObjectTypeID = 2 AND c.ContactID = o.ObjectID AND o.IsGroup = 0
		INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = 2
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND il.ItemListID = ISNULL(@ItemListID, il.ItemListID)
	-- Hierarchy node access
	UNION
	SELECT il.ItemListID, ua.RoleID
	FROM ItemLists il
		INNER JOIN Accounts a ON a.AccountID = il.AccountID
		INNER JOIN vwUserAccess ua ON ua.FilterObjectID = a.AccountHierarchyID AND ua.FilterObjectTypeID = 4 AND ua.FilterTypeID = 3
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND il.ItemListID = ISNULL(@ItemListID, il.ItemListID)
	UNION
	--Hierarchy Parent Node Access
	SELECT il.ItemListID, ua.RoleID
	FROM ItemLists il
	  INNER JOIN Accounts a ON il.AccountID = a.AccountID  
	  INNER JOIN AccountHierarchies ah ON a.AccountHierarchyID = ah.AccountHierarchyID
	  INNER JOIN vwUserAccess ua ON ah.ParentID = ua.FilterObjectID AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 4
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND il.ItemListID = ISNULL(@ItemListID, il.ItemListID)
	UNION
	--Region Access
	SELECT il.ItemListID, ua.RoleID
	FROM ItemLists il
		INNER JOIN Accounts a ON a.AccountID = il.AccountID
		INNER JOIN AccountHierarchies ah ON ah.AccountHierarchyID = a.AccountHierarchyID
		INNER JOIN vwUserAccess ua ON ua.FilterObjectID = a.AccountHierarchyID AND ua.FilterObjectTypeID = 5 AND ua.FilterTypeID = 3
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND il.ItemListID = ISNULL(@ItemListID, il.ItemListID)
	UNION
	-- User Account access
	SELECT il.ItemListID, ua.RoleID
	FROM ItemLists il
		INNER JOIN mapOwnership o ON o.ObjectTypeID = 1 AND o.IsGroup = 1 AND o.ObjectID = il.AccountID
		INNER JOIN vwUserAccess ua ON o.OwnerID = CASE WHEN ua.FilterObjectID = 0 THEN o.OwnerID ELSE ua.FilterObjectID END
			AND ua.FilterObjectTypeID = 32
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND il.ItemListID = ISNULL(@ItemListID, il.ItemListID)
	UNION
	-- All Item List
	SELECT il.ItemListID, ua.RoleID
	FROM ItemLists il
	  INNER JOIN vwUserAccess ua ON il.ItemListID = CASE WHEN ua.FilterObjectID = 0 THEN il.ItemListID ELSE ua.FilterObjectID END AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND il.ItemListID = ISNULL(@ItemListID, il.ItemListID)
END
