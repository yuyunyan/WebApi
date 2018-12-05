/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.10.19
   Description:	Retrieves a list of quotes and roles for a given user
   Usage:		EXEC uspQuoteSecurityGet @UserID = 64
   			
   Revision History:
	
   ============================================= */

CREATE PROCEDURE [dbo].[uspQuoteSecurityGet]
	@UserID INT = NULL,
	@QuoteID INT = NULL
AS
BEGIN	
	DECLARE @ObjectTypeID INT = 19

	--Quote Ownership (Individual)
	SELECT q.QuoteID, ua.RoleID
	FROM vwQuotes q
	  INNER JOIN mapOwnership o ON q.QuoteID = o.ObjectID AND o.ObjectTypeID = @ObjectTypeID AND o.IsGroup = 0
	  INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND q.QuoteID = ISNULL(@QuoteID, q.QuoteID)
	UNION
	--Account Ownership (Individual)
	SELECT q.QuoteID, ua.RoleID
	FROM vwQuotes q
	  INNER JOIN mapOwnership o ON q.AccountID = o.ObjectID AND o.ObjectTypeID = 1 AND o.IsGroup = 0
	  INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = 1
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND q.QuoteID = ISNULL(@QuoteID, q.QuoteID)
	UNION
	--Contact Ownership (Individual)
	SELECT q.QuoteID, ua.RoleID
	FROM vwQuotes q
	  INNER JOIN mapOwnership o ON q.ContactID = o.ObjectID AND o.ObjectTypeID = 2 AND o.IsGroup = 0
	  INNER JOIN vwUserAccess ua ON o.OwnerID = ua.FilterObjectID AND ua.FilterTypeID = 1 AND ua.FilterObjectTypeID = 2
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND q.QuoteID = ISNULL(@QuoteID, q.QuoteID)
	UNION
	--Account Access
	SELECT q.QuoteID, ua.RoleID
	FROM vwQuotes q
	  INNER JOIN vwUserAccess ua ON q.AccountID = CASE WHEN ua.FilterObjectID = 0 THEN q.AccountID ELSE ua.FilterObjectID END AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 1
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND q.QuoteID = ISNULL(@QuoteID, q.QuoteID)
	UNION
	--Hierarchy Node Access
	SELECT q.QuoteID, ua.RoleID
	FROM vwQuotes q  
	  INNER JOIN Accounts a ON q.AccountID = a.AccountID
	  INNER JOIN AccountHierarchies h ON a.AccountHierarchyID = h.AccountHierarchyID
	  INNER JOIN vwUserAccess ua ON a.AccountHierarchyID = CASE WHEN ua.FilterObjectID = 0 THEN a.AccountHierarchyID ELSE ua.FilterObjectID END
		AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 4
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND q.QuoteID = ISNULL(@QuoteID, q.QuoteID)
	UNION
	--Hierarchy Parent Node Access
	SELECT q.QuoteID, ua.RoleID
	FROM vwQuotes q  
	  INNER JOIN Accounts a ON q.AccountID = a.AccountID  
	  INNER JOIN AccountHierarchies h ON a.AccountHierarchyID = h.AccountHierarchyID
	  INNER JOIN vwUserAccess ua ON h.ParentID = CASE WHEN ua.FilterObjectID = 0 THEN h.ParentID ELSE ua.FilterObjectID END
		AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 4
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND q.QuoteID = ISNULL(@QuoteID, q.QuoteID)
	UNION
	--Region Access
	SELECT q.QuoteID, ua.RoleID
	FROM vwQuotes q  
	  INNER JOIN Accounts a ON q.AccountID = a.AccountID
	  INNER JOIN AccountHierarchies h ON a.AccountHierarchyID = h.AccountHierarchyID
	  INNER JOIN vwUserAccess ua ON h.RegionID = CASE WHEN ua.FilterObjectID = 0 THEN h.RegionID ELSE ua.FilterObjectID END
		AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = 3
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND q.QuoteID = ISNULL(@QuoteID, q.QuoteID)
	UNION
	--All Quotes
	SELECT q.QuoteID, ua.RoleID
	FROM vwQuotes q
	  INNER JOIN vwUserAccess ua ON q.QuoteID = CASE WHEN ua.FilterObjectID = 0 THEN q.QuoteID ELSE ua.FilterObjectID END AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND q.QuoteID = ISNULL(@QuoteID, q.QuoteID)
END
