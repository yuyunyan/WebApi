-- =============================================
-- Author:		Berry
-- Create date: 2017.08.02
-- Description:	Return a list of purchase orders
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[uspPurchaseOrdersListGet]
	@PurchaseOrderID INT = NULL,
	@SearchString NVARCHAR(100) = '',
	@IncludeComplete BIT = 0,
	@IncludeCanceled BIT = 0,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@UserID INT = NULL
AS
BEGIN
	DECLARE @Sec TABLE (PurchaseOrderID INT, RoleID INT)
	INSERT @Sec EXECUTE uspPurchaseOrderSecurityGet @UserID = @UserID;
		
	WITH Main_CTE AS (
		SELECT 
			po.PurchaseOrderID,
			po.VersionID,
			po.AccountID,
			a.AccountName,
			po.ContactID,
			c.FirstName 'ContactFirstName',
			c.LastName 'ContactLastName',
			po.StatusID,
			s.StatusName,
			po.OrganizationID,
			org.[Name] 'OrganizationName',
			po.OrderDate,
			po.externalID,
			dbo.fnGetObjectOwners(po.PurchaseOrderID, 22) Owners --22 = Purchase Order (lkpObjectTypes)		
		FROM vwPurchaseOrders po 
		  LEFT OUTER JOIN Accounts AS a ON po.AccountID = a.AccountID
		  LEFT OUTER JOIN Contacts AS c ON po.ContactID = c.ContactID
		  LEFT OUTER JOIN lkpStatuses AS s ON po.StatusID = s.StatusID
		  LEFT OUTER JOIN Organizations AS org ON po.OrganizationID = org.OrganizationID
		  INNER JOIN (SELECT DISTINCT PurchaseOrderID FROM @Sec) sec ON po.PurchaseOrderID = sec.PurchaseOrderID
		WHERE ISNULL(s.IsComplete, 0) IN(0, @IncludeComplete)
		  AND ISNULL(s.IsCanceled, 0) IN(0, @IncludeCanceled)
		  AND (dbo.fnReturnOrderDisplayID(po.PurchaseOrderID, po.ExternalID) + ISNULL(a.AccountName, '') + ISNULL(c.FirstName, '') + ISNULL(c.LastName, '') + ISNULL(c.FirstName, '') + ' ' + ISNULL(c.LastName, '') + ISNULL(dbo.fnGetObjectOwners(po.PurchaseOrderID, 22), '') LIKE '%' + ISNULL(@SearchString,'') + '%')
	),
	Count_CTE AS (
		SELECT COUNT(*) 'TotalRows'
		FROM Main_CTE
	)

	SELECT * FROM Main_CTE m, Count_CTE
	ORDER BY
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN CAST(ISNULL(ExternalID, PurchaseOrderID) AS INT)
				WHEN @SortBy = 'purchaseOrderId' THEN CAST(ISNULL(ExternalID, PurchaseOrderID) AS INT)
				WHEN @SortBy = 'DisplayID' THEN CAST(ISNULL(ExternalID, PurchaseOrderID) AS INT)
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'AccountName' THEN m.AccountName
				WHEN @SortBy = 'ContactFirstname' THEN m.ContactFirstName
				WHEN @SortBy = 'ContactLastName' THEN m.ContactLastName
				WHEN @SortBy = 'StatusName' THEN m.StatusName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN	
			CASE 
				WHEN @SortBy = 'OrderDate' THEN m.OrderDate
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN CAST(ISNULL(ExternalID, PurchaseOrderID) AS INT)
				WHEN @SortBy = 'purchaseOrderId' THEN CAST(ISNULL(ExternalID, PurchaseOrderID) AS INT)
				WHEN @SortBy = 'DisplayID' THEN CAST(ISNULL(ExternalID, PurchaseOrderID) AS INT)
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN m.AccountName
				WHEN @SortBy = 'ContactFirstname' THEN m.ContactFirstName
				WHEN @SortBy = 'ContactLastName' THEN m.ContactLastName
				WHEN @SortBy = 'StatusName' THEN m.StatusName
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN	
			CASE 
				WHEN @SortBy = 'OrderDate' THEN m.OrderDate
			END
		END DESC
		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY
END
