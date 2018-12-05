/* =============================================
   Author:			Berry Zhong
   Create date:		2017.10.20
   Description:		Retrieves a list of contact and roles for a given user
   Usage:			EXEC uspContactSecurityGet @UserID = 64
   			
   Revision History:
	
   ============================================= */

CREATE PROCEDURE [dbo].[uspContactSecurityGet]
	@UserID INT = NULL,
	@ContactID INT = NULL
AS
BEGIN	
	DECLARE @ObjectTypeID INT = 2;

	--Contact Ownership
	SELECT c.ContactID, ua.RoleID
	FROM Contacts c
		INNER JOIN mapOwnership o ON c.ContactID = o.ObjectID AND o.ObjectTypeID = @ObjectTypeID AND o.IsGroup = 0
		INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND c.ContactID = ISNULL(@ContactID, c.ContactID)
	UNION
	--Account Ownership
	SELECT c.ContactID, ua.RoleID
	FROM Contacts c
		INNER JOIN mapOwnership o ON c.AccountID = o.ObjectID AND o.ObjectTypeID = 1 AND o.IsGroup = 0
		INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = 1
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND c.ContactID = ISNULL(@ContactID, c.ContactID)
	UNION
	--Hierarchy Node access
	SELECT c.ContactID, ua.RoleID
	FROM Contacts c
		INNER JOIN Accounts a ON c.AccountID = a.AccountID
		INNER JOIN vwUserAccess ua ON a.AccountHierarchyID = CASE WHEN ua.FilterObjectID = 0 THEN a.AccountHierarchyID ELSE ua.FilterObjectID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 4
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND c.ContactID = ISNULL(@ContactID, c.ContactID)
	UNION
	--Hierarchy Parent Node access
	SELECT c.ContactID, ua.RoleID
	FROM Contacts c
		INNER JOIN Accounts a ON c.AccountID = a.AccountID
		INNER JOIN AccountHierarchies ah ON a.AccountHierarchyID = ah.AccountHierarchyID
		INNER JOIN vwUserAccess ua ON ah.ParentID = CASE WHEN ua.FilterObjectID = 0 THEN ah.ParentID ELSE ua.FilterObjectID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 4
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND c.ContactID = ISNULL(@ContactID, c.ContactID)
	UNION
	--Region access
	SELECT c.ContactID, ua.RoleID
	FROM Contacts c
		INNER JOIN Accounts a ON c.AccountID = a.AccountID
		INNER JOIN AccountHierarchies ah ON a.AccountHierarchyID = ah.AccountHierarchyID
		INNER JOIN vwUserAccess ua ON ah.RegionID = CASE WHEN ua.FilterObjectID = 0 THEN ah.RegionID ELSE ua.FilterObjectID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 3
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND c.ContactID = ISNULL(@ContactID, c.ContactID)
	UNION
	--Account Access
	SELECT c.ContactID, ua.RoleID
	FROM Contacts c
		INNER JOIN vwUserAccess ua ON c.AccountID = CASE WHEN ua.FilterObjectID = 0 THEN c.AccountID ELSE ua.FilterObjectID END
		AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 1
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND c.ContactID = ISNULL(@ContactID, c.ContactID)
	UNION
	--User own Access
	SELECT c.ContactID, ua.RoleID
	FROM Contacts c
		INNER JOIN mapOwnership o ON o.ObjectTypeID = 2 AND o.IsGroup = 1 AND o.ObjectID = c.ContactID
		INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterObjectTypeID = 2 AND ua.FilterTypeID = 1
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND c.ContactID = ISNULL(@ContactID, c.ContactID)
	UNION
	--All Contacts
	SELECT c.ContactID, ua.RoleID
	FROM Contacts c
	  INNER JOIN vwUserAccess ua ON c.ContactID = CASE WHEN ua.FilterObjectID = 0 THEN c.ContactID ELSE ua.FilterObjectID END AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND c.ContactID = ISNULL(@ContactID, c.ContactID)
END
