/* =============================================
   Author:			Berry Zhong
   Create date:		2017.10.20
   Description:		Retrieves a list of account and roles for a given user
   Usage:			EXEC uspAccountSecurityGet @UserID = 64
   			
   Revision History:
	
   ============================================= */

CREATE PROCEDURE [dbo].[uspAccountSecurityGet]
	@UserID INT = NULL,
	@AccountID INT = NULL
AS
BEGIN	
	DECLARE @ObjectTypeID INT = 1

	--Account Ownership
	SELECT a.AccountID, ua.RoleID
	FROM Accounts a
	  INNER JOIN mapOwnership o ON a.AccountID = o.ObjectID AND o.ObjectTypeID = @ObjectTypeID AND o.IsGroup = 0
	  INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND a.AccountID = ISNULL(@AccountID, a.AccountID)
	UNION
	--Contact Ownership 
	SELECT a.AccountID, ua.RoleID
	FROM Accounts a
		INNER JOIN Contacts c ON c.AccountID = a.AccountID 
		INNER JOIN mapOwnership o ON o.ObjectTypeID = 2 AND o.IsGroup = 0 AND c.ContactID = o.ObjectID
		INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterObjectTypeID = 2 AND ua.FilterTypeID = 1
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND a.AccountID = ISNULL(@AccountID, a.AccountID)
	UNION
	--Account User Access
	SELECT a.AccountID, ua.RoleID
	FROM Accounts a
		INNER JOIN mapOwnership o ON o.ObjectTypeID = @ObjectTypeID AND o.IsGroup = 1 AND a.AccountID = o.ObjectID
		INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterObjectTypeID = 1 AND ua.FilterTypeID = 1
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND a.AccountID = ISNULL(@AccountID, a.AccountID)
	UNION
	--Region Account belongs to
	SELECT a.AccountID, ua.RoleID
	FROM Accounts a
		INNER JOIN AccountHierarchies ah ON a.AccountHierarchyID = ah.AccountHierarchyID
		INNER JOIN vwUserAccess ua ON ah.RegionID = CASE WHEN ua.FilterObjectID = 0 THEN ah.RegionID ELSE ua.FilterObjectID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 3
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND a.AccountID = ISNULL(@AccountID, a.AccountID)
	UNION
	--Hierarchy Node Access
	SELECT a.AccountID, ua.RoleID
	FROM Accounts a
		INNER JOIN vwUserAccess ua ON a.AccountHierarchyID = CASE WHEN ua.FilterObjectID = 0 THEN a.AccountHierarchyID ELSE ua.FilterObjectID END
		AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 4
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND a.AccountID = ISNULL(@AccountID, a.AccountID)
	UNION
	--All Accounts
	SELECT a.AccountID, ua.RoleID
	FROM Accounts a
	  INNER JOIN vwUserAccess ua ON a.AccountID = CASE WHEN ua.FilterObjectID = 0 THEN a.AccountID ELSE ua.FilterObjectID END AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND a.AccountID = ISNULL(@AccountID, a.AccountID)
END
