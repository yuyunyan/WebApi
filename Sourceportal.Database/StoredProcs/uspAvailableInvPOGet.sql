/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.08.08
   Description:	Gets a list of Inventory and PO lines and their AVAILABLE quantities given a SOLineID
   Usage:		EXEC uspAvailableInvPOGet @SOLineID = 3146
				EXEC uspAvailableInvPOGet @ItemID = 12418
   Revision History:
	2018.03.26	AR	Removed PromiseDate
	2018.05.25	NA	Added IsDeleted check to ItemInventory
	2018.06.22	NA	Converted to ItemStock schema
	2018.07.18	AR	Added support for externalID
	2018.08.10	NA	Added Warehouse
	2018.08.23	NA	Added InTransit flag
	2018.08.28	NA	Added @ItemID Parameter; Converted to use new view, removed SO info because it was already in the "Allocations" functions
	2018.10.30  HR  Added isInspection boolean
	2018.10.30  NA  added ExcludePO
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspAvailableInvPOGet]
	@SOLineID INT = NULL,
	@ItemID INT = NULL,
	@ExcludePO BIT = 0,
	@RowOffset INT = 0,
	@RowLimit INT = 1000,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
	IF @ItemID IS NULL
		SELECT @ItemID = ItemID FROM SalesOrderLines WHERE SOLineID = @SOLineID

	SELECT *,
		COUNT(*) OVER() AS 'TotalRows'
	FROM (
	SELECT	v.[Type], 
			v.ID,
			v.ItemID, 
			i.PartNumber,			
			i.MfrID,
			m.MfrName,
			i.CommodityID,
			c.CommodityName,
			v.AccountID,
			a.AccountName,
			v.StatusID,
			v.StatusName,
			v.Qty 'OrigQty',
			v.Available, 
			v.Cost,
			v.DateCode,
			v.PackagingID,
			pack.PackagingName,
			CASE
				WHEN [Type] = 'Inventory' THEN dbo.fnGetInvFulfillments(ID)
				ELSE dbo.fnGetSOPOAllocations(ID)
			END 'Allocations',
			v.PurchaseOrderID,
			v.POVersionID,
			v.POExternalID,
			v.LineNum,
			v.LineRev,
			v.Buyers,
			v.ShipDate,
			pc.ConditionName,
			v.WarehouseID,
			v.WarehouseName,
			v.WarehouseExternalID,
			v.isInspection,
			v.InTransit,
			CASE 
				WHEN [Type] = 'Inventory' THEN dbo.fnGetCommentsCount(ID, 23)
				ELSE dbo.fnGetCommentsCount(ID, 19)
			END 'Comments'
	FROM vwAvailableInvPO v
	INNER JOIN Accounts a ON v.AccountID = a.AccountID
	INNER JOIN Items i ON v.ItemID = i.ItemID
	INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
	INNER JOIN lkpItemCommodities c ON i.CommodityID = c.CommodityID
	LEFT OUTER JOIN codes.lkpPackaging pack ON v.PackagingID = pack.PackagingID
	LEFT OUTER JOIN codes.lkpPackageConditions pc ON v.PackageConditionID = pc.PackageConditionID	
	WHERE v.ItemID = @ItemID
	AND (@ExcludePO = 0 OR v.[Type] = 'Inventory')
	) z
	ORDER BY	
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'OrigQty' THEN [OrigQty]
				WHEN @SortBy = 'Available' THEN [Available]
				WHEN @SortBy = 'Cost' THEN [Cost]
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN [Type]
				WHEN @SortBy = 'AccountName' THEN [AccountName]
				WHEN @SortBy = 'PartNumber' THEN [PartNumber]
				WHEN @SortBy = 'PackagingName' THEN [PackagingName]
				WHEN @SortBy = 'StatusName' THEN [StatusName]
				WHEN @SortBy = 'WarehouseName' THEN [WarehouseName]
				WHEN @SortBy = 'Buyers' THEN [Buyers]
				WHEN @SortBy = 'DateCode' THEN [DateCode]
				WHEN @SortBy = 'ConditionName' THEN [ConditionName]
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'ShipDate' THEN [ShipDate]
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'OrigQty' THEN [OrigQty]
				WHEN @SortBy = 'Available' THEN [Available]
				WHEN @SortBy = 'Cost' THEN [Cost]
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN [Type]
				WHEN @SortBy = 'AccountName' THEN [AccountName]
				WHEN @SortBy = 'PartNumber' THEN [PartNumber]
				WHEN @SortBy = 'PackagingName' THEN [PackagingName]
				WHEN @SortBy = 'StatusName' THEN [StatusName]
				WHEN @SortBy = 'WarehouseName' THEN [WarehouseName]
				WHEN @SortBy = 'Buyers' THEN [Buyers]
				WHEN @SortBy = 'DateCode' THEN [DateCode]
				WHEN @SortBy = 'ConditionName' THEN [ConditionName]
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'ShipDate' THEN [ShipDate]
			END
		END DESC,
		[ID]	
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END
