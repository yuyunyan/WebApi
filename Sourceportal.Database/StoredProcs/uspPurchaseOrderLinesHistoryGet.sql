/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.08.29
				Returns Purchase Order Line history by ItemID
   Usage:		EXEC uspPurchaseOrderLinesHistoryGet @ItemID = 8853, @UserID = 64
   Return Codes:
   Revision History:
		2018.08.30	NA	Added Warehouse
		2018.10.16	NA	Added Account and Contact parameters
		2018.10.31  HR  Added LineRev
============================================= */
CREATE PROCEDURE [dbo].[uspPurchaseOrderLinesHistoryGet]
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
		SET @StartDate = DATEADD(month, -6, GETDATE())
	IF @EndDate IS NULL
		SET @EndDate = GETDATE()
	
	DECLARE @AccountSec TABLE (AccountID INT, RoleID INT)
	INSERT @AccountSec EXECUTE uspAccountSecurityGet @UserID = @UserID;

	DECLARE @ContactSec TABLE (ContactID INT, RoleID INT)
	INSERT @ContactSec EXECUTE uspContactSecurityGet @UserID = @UserID;

	DECLARE @PurchaseOrderSec TABLE (PurchaseOrderID INT, RoleID INT)
	INSERT @PurchaseOrderSec EXECUTE uspPurchaseOrderSecurityGet @UserID = @UserID;

	WITH Main_Temp AS (
		SELECT  pol.POLineID,
		        pol.LineNum,
				pol.LineRev,
				po.PurchaseOrderID,
				po.VersionID,
				po.ExternalID 'POExternalID',
				po.AccountID,
				a.AccountName,
				c.ContactID,
				c.FirstName,
				c.LastName,
				po.OrganizationID,
				o.[Name] 'OrgName',
				po.StatusID,
				s.StatusName,
				po.OrderDate,
				i.ItemID,
				i.PartNumber,
				i.MfrID,
				m.MfrName,
				pol.DueDate,
				pol.Qty,
				pol.Cost,
				pol.DateCode,
				pol.PackagingID,
				p.PackagingName,
				pol.PackageConditionID,
				pc.ConditionName,
				w.WarehouseID,
				w.WarehouseName,
				dbo.fnGetObjectOwners(po.PurchaseOrderID, 22) 'Owners',
				CASE WHEN asec.AccountID IS NULL THEN 0 ELSE 1 END 'CanViewAccount',
				CASE WHEN csec.ContactID IS NULL THEN 0 ELSE 1 END 'CanViewContact',
				CASE WHEN posec.PurchaseOrderID IS NULL THEN 0 ELSE 1 END 'CanViewPurchaseOrder'
		FROM vwPurchaseOrderLines pol
		INNER JOIN vwPurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID AND pol.POVersionID = po.VersionID AND OrderDate BETWEEN @StartDate AND @EndDate
		INNER JOIN Accounts a ON po.AccountID = a.AccountID
		INNER JOIN Organizations o ON po.OrganizationID = o.OrganizationID
		INNER JOIN Items i ON pol.ItemID = i.ItemID
		INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
		LEFT OUTER JOIN Contacts c ON po.ContactID = c.ContactID
		LEFT OUTER JOIN lkpStatuses s ON po.StatusID = s.StatusID
		LEFT OUTER JOIN (SELECT DISTINCT AccountID FROM @AccountSec) asec ON po.AccountID = asec.AccountID
		LEFT OUTER JOIN (SELECT DISTINCT ContactID FROM @ContactSec) csec ON po.ContactID = csec.ContactID
		LEFT OUTER JOIN (SELECT DISTINCT PurchaseOrderID FROM @PurchaseOrderSec) posec ON po.PurchaseOrderID = posec.PurchaseOrderID
		LEFT OUTER JOIN Warehouses w ON po.ToWarehouseID = w.WarehouseID
		LEFT OUTER JOIN codes.lkpPackaging p ON pol.PackagingID = p.PackagingID
		LEFT OUTER JOIN codes.lkpPackageConditions pc ON pol.PackageConditionID = pc.PackageConditionID
		WHERE pol.ItemID = ISNULL(@ItemID, pol.ItemID)
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
				WHEN @SortBy = 'DueDate' THEN Main_Temp.DueDate
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName
				WHEN @SortBy = 'DisplayID' THEN dbo.fnReturnOrderDisplayID(PurchaseOrderID,POExternalID)
				WHEN @SortBy = 'OrgName' THEN Main_Temp.OrgName
				WHEN @SortBy = 'FirstName' THEN Main_Temp.FirstName
				WHEN @SortBy = 'LastName' THEN Main_Temp.LastName
				WHEN @SortBy = 'Owners' THEN Main_Temp.Owners
				WHEN @SortBy = 'StatusName' THEN Main_Temp.StatusName
				WHEN @SortBy = 'WarehouseName' THEN Main_Temp.WarehouseName
				WHEN @SortBy = 'ConditionName' THEN Main_Temp.ConditionName
				WHEN @SortBy = 'PackagingName' THEN Main_Temp.PackagingName
				WHEN @SortBy = 'DateCode' THEN Main_Temp.DateCode
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 				
				WHEN @SortBy = 'Qty' THEN Main_Temp.Qty
				WHEN @SortBy = 'Cost' THEN Main_Temp.Cost
			END
		END ASC,
		--Descending
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_Temp.OrderDate
				WHEN @SortBy = 'OrderDate' THEN Main_Temp.OrderDate
				WHEN @SortBy = 'DueDate' THEN Main_Temp.DueDate
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_Temp.AccountName
				WHEN @SortBy = 'POExternalID' THEN Main_Temp.POExternalID
				WHEN @SortBy = 'OrgName' THEN Main_Temp.OrgName
				WHEN @SortBy = 'FirstName' THEN Main_Temp.FirstName
				WHEN @SortBy = 'LastName' THEN Main_Temp.LastName
				WHEN @SortBy = 'Owners' THEN Main_Temp.Owners
				WHEN @SortBy = 'StatusName' THEN Main_Temp.StatusName
				WHEN @SortBy = 'WarehouseName' THEN Main_Temp.WarehouseName
				WHEN @SortBy = 'PackagingName' THEN Main_Temp.PackagingName
				WHEN @SortBy = 'DateCode' THEN Main_Temp.DateCode
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 				
				WHEN @SortBy = 'Qty' THEN Main_Temp.Qty
				WHEN @SortBy = 'Cost' THEN Main_Temp.Cost
			END
		END DESC
		
		OFFSET @RowOffset ROWS
		FETCH NEXT NULLIF(@RowLimit,0) ROWS ONLY
END