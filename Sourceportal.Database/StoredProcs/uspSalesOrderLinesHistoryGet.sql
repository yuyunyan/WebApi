/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.08.24
				Returns Sales Order Line history by ItemID
   Usage:		EXEC uspSalesOrderLinesHistoryGet @ItemID = 8853, @UserID = 64
   Return Codes:
   Revision History:
	2018.10.16	NA	Added Account and Contact parameters
============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspSalesOrderLinesHistoryGet]
	@ItemID INT = NULL,
	@AccountID INT = NULL,
	@ContactID INT = NULL,
	@StartDate DATE = NULL,
	@EndDate DATE = NULL,
	@UserID INT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 1
AS
BEGIN
	IF @StartDate IS NULL
		SET @StartDate = DATEADD(month, -6, GETUTCDATE())
	IF @EndDate IS NULL
		SET @EndDate = GETUTCDATE()
	
	DECLARE @AccountSec TABLE (AccountID INT, RoleID INT)
	INSERT @AccountSec EXECUTE uspAccountSecurityGet @UserID = @UserID;

	DECLARE @ContactSec TABLE (ContactID INT, RoleID INT)
	INSERT @ContactSec EXECUTE uspContactSecurityGet @UserID = @UserID;

	DECLARE @SalesOrderSec TABLE (SalesOrderID INT, RoleID INT)
	INSERT @SalesOrderSec EXECUTE uspSalesOrderSecurityGet @UserID = @UserID;

	WITH Main_Temp AS (
		SELECT  sol.SOLineID,
				so.SalesOrderID,
				so.VersionID,
				so.ExternalID 'SOExternalID',
				so.AccountID,
				a.AccountName,
				c.ContactID,
				ISNULL(c.FirstName,'') FirstName,
				ISNULL(c.LastName,'') LastName,
				so.OrganizationID,
				o.[Name] 'OrgName',
				so.StatusID,
				s.StatusName,
				so.OrderDate,
				sol.ItemID,
				i.PartNumber,
				m.MfrID,
				m.MfrName,
				sol.ShipDate,
				sol.Qty,
				sol.Price,
				sol.Cost,
				CASE WHEN sol.Qty * sol.Price <> 0 THEN ((sol.Qty * sol.Price) - (sol.Qty * sol.Cost)) / ((sol.Qty * sol.Price)) ELSE 0 END 'GPM',
				sol.DateCode,
				sol.PackagingID,
				p.PackagingName,
				sol.PackageConditionID,
				pc.ConditionName,
				dbo.fnGetObjectOwners(so.SalesOrderID, 16) 'Owners',
				CASE WHEN asec.AccountID IS NULL THEN 0 ELSE 1 END 'CanViewAccount',
				CASE WHEN csec.ContactID IS NULL THEN 0 ELSE 1 END 'CanViewContact',
				CASE WHEN sosec.SalesOrderID IS NULL THEN 0 ELSE 1 END 'CanViewSalesOrder'
		FROM vwSalesOrderLines sol
		INNER JOIN vwSalesOrders so ON sol.SalesOrderID = so.SalesOrderID AND sol.SOVersionID = so.VersionID AND OrderDate BETWEEN @StartDate AND @EndDate
		INNER JOIN Accounts a ON so.AccountID = a.AccountID
		INNER JOIN Organizations o ON so.OrganizationID = o.OrganizationID
		INNER JOIN Items i ON sol.ItemID = i.ItemID
		INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
		LEFT OUTER JOIN Contacts c ON so.ContactID = c.ContactID
		LEFT OUTER JOIN lkpStatuses s ON so.StatusID = s.StatusID
		LEFT OUTER JOIN (SELECT DISTINCT AccountID FROM @AccountSec) asec ON so.AccountID = asec.AccountID
		LEFT OUTER JOIN (SELECT DISTINCT ContactID FROM @ContactSec) csec ON so.ContactID = csec.ContactID
		LEFT OUTER JOIN (SELECT DISTINCT SalesOrderID FROM @SalesOrderSec) sosec ON so.SalesOrderID = sosec.SalesOrderID
		LEFT OUTER JOIN codes.lkpPackaging p ON sol.PackagingID = p.PackagingID
		LEFT OUTER JOIN codes.lkpPackageConditions pc ON sol.PackageConditionID = pc.PackageConditionID
		WHERE sol.ItemID = ISNULL(@ItemID, sol.ItemID)
		AND a.AccountID = ISNULL(@AccountID, a.AccountID)
		AND ISNULL(c.ContactID, 0) = COALESCE(@ContactID, c.ContactID, 0)
	), 
	Count_Temp AS(
		SELECT COUNT(*) AS TotalRowCount
		From Main_Temp)

	SELECT *
	FROM Main_Temp,Count_Temp
	ORDER BY
		--Ascending
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.OrderDate
				WHEN @SortBy = 'OrderDate' THEN Main_Temp.OrderDate
				WHEN @SortBy = 'ShipDate' THEN Main_Temp.ShipDate
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName
				WHEN @SortBy = 'SOExternalID' THEN Main_Temp.SOExternalID
				WHEN @SortBy = 'OrgName' THEN Main_Temp.OrgName
				WHEN @SortBy = 'FirstName' THEN Main_Temp.FirstName
				WHEN @SortBy = 'LastName' THEN Main_Temp.LastName
				WHEN @SortBy = 'Owners' THEN Main_Temp.Owners
				WHEN @SortBy = 'StatusName' THEN Main_Temp.StatusName
				WHEN @SortBy = 'ContactName' THEN Main_Temp.FirstName + Main_Temp.LastName
				WHEN @SortBy = 'DateCode' THEN Main_Temp.DateCode
				WHEN @SortBy = 'PackagingName' THEN Main_Temp.PackagingName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 				
				WHEN @SortBy IN ('Qty','Quantity') THEN Main_Temp.Qty
				WHEN @SortBy = 'Price' THEN Main_Temp.Price
				WHEN @SortBy = 'Cost' THEN Main_Temp.Cost
				WHEN @SortBy = 'GPM' THEN Main_Temp.GPM
			END
		END ASC,
		--Descending
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.OrderDate
				WHEN @SortBy = 'OrderDate' THEN Main_Temp.OrderDate
				WHEN @SortBy = 'ShipDate' THEN Main_Temp.ShipDate
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName
				WHEN @SortBy = 'SOExternalID' THEN Main_Temp.SOExternalID
				WHEN @SortBy = 'OrgName' THEN Main_Temp.OrgName
				WHEN @SortBy = 'FirstName' THEN Main_Temp.FirstName
				WHEN @SortBy = 'LastName' THEN Main_Temp.LastName
				WHEN @SortBy = 'Owners' THEN Main_Temp.Owners
				WHEN @SortBy = 'StatusName' THEN Main_Temp.StatusName
				WHEN @SortBy = 'ContactName' THEN Main_Temp.FirstName + Main_Temp.LastName
				WHEN @SortBy = 'DateCode' THEN Main_Temp.DateCode
				WHEN @SortBy = 'PackagingName' THEN Main_Temp.PackagingName
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 				
				WHEN @SortBy IN ('Qty','Quantity') THEN Main_Temp.Qty
				WHEN @SortBy = 'Price' THEN Main_Temp.Price
				WHEN @SortBy = 'Cost' THEN Main_Temp.Cost
				WHEN @SortBy = 'GPM' THEN Main_Temp.GPM
			END
		END DESC
		
		OFFSET @RowOffset ROWS
		FETCH NEXT NULLIF(@RowLimit,0) ROWS ONLY
END