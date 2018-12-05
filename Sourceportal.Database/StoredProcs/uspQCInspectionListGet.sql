/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.08.25
   Description:	Retrieves inspection list with details
   Usage:		EXEC uspQCInspectionListGet @UserID=82,@SearchString='arrow'
   			
   Revision History:
	2017.08.28	ML	Added server side pagination and Sorting
	2017.08.29	ML	Improved the way the total count is calculate with Common Table Expressions
	2017.10.24	BZ	Added Security
	2017.11.03	AR	Replaced Inner joins with outer joins
	2018.06.22	NA	Converted to ItemStock schema
	2018.07.03	NA	Added filtering and Warehouse
	2018.09.11	NA	Added ExternalID for Stock and Inspection
	2018.09.12  JT	Added AccountID
	2018.09.14  JT	Added ExternalID and it's filters and sort by for PO
	2018.10.09	NA	Fixed Sorting by Stock ExternalID, PO ExternalID and QC ExternalID
	2018.10.16	NA	Added InspectionType
   ============================================= */

CREATE PROCEDURE [dbo].[uspQCInspectionListGet]
	@SearchString NVARCHAR(32) = '',
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@UserID INT = NULL,
	@FilterBy VARCHAR(25) = NULL,
	@FilterText VARCHAR(250) = NULL
AS
BEGIN
	DECLARE @Sec TABLE (InspectionID INT, RoleID INT)
	INSERT @Sec EXECUTE uspQCInspectionSecurityGet @UserID = @UserID;

	WITH Main_CTE AS(
		SELECT 
			INS.InspectionID AS InspectionID,
			INS.ExternalID AS ExternalID,
			SQ.ItemID AS ItemID,
			ACC.AccountName AS Supplier,
			POL.PurchaseOrderID AS PONumber,
			POL.POVersionID AS POVersionID,
			dbo.fnGetCustomersForInventory(SQ.StockID) AS Customers, 
			INSST.StatusName AS StatusName,
			INS.InspectionTypeID AS InspectionTypeID,
			INSTYPE.TypeName AS InspectionTypeName,
			SQ.ReceivedDate AS ReceivedDate,
			SQ.StockID AS InventoryID,
			SQ.ExternalID AS StockExternalID,
			dbo.fnGetEarliestShipDateOfSOLine(SQ.StockID) AS ShipDate,
			sq.WarehouseID,
			w.WarehouseName,
			acc.AccountID,
			PO.ExternalID 'POExternalID'
		FROM QCInspections INS 	
		LEFT OUTER JOIN vwStockQty SQ ON INS.StockID = SQ.StockID
		LEFT OUTER JOIN vwPurchaseOrderLines POL ON SQ.POLineID = POL.PoLineId
		LEFT OUTER JOIN vwPurchaseOrders PO ON POL.PurchaseOrderId = PO.PurchaseOrderId
		INNER JOIN Warehouses w ON sq.WarehouseID = w.WarehouseID
		INNER JOIN Accounts ACC ON ACC.AccountID = PO.AccountID
		INNER JOIN lkpQCInspectionStatuses INSST ON INSST.InspectionStatusID = INS.InspectionStatusID
		INNER JOIN lkpQCInspectionTypes INSTYPE ON INS.InspectionTypeID = INSTYPE.InspectionTypeID
		INNER JOIN (SELECT DISTINCT InspectionID FROM @Sec) sec ON INS.InspectionID = sec.InspectionID
		WHERE (@FilterBy IS NULL OR (
			(@FilterBy = 'WarehouseName' AND w.WarehouseName LIKE '%' + @FilterText + '%')
			OR
			(@FilterBy = 'Supplier' AND acc.AccountName LIKE '%' + @FilterText + '%')
			OR
			(@FilterBy = 'PONumber' AND PO.PurchaseOrderID LIKE '%' + @FilterText + '%')
			OR
			(@FilterBy = 'StatusName' AND insst.StatusName LIKE '%' + @FilterText + '%')
			OR
			(@FilterBy = 'StockID' AND sq.StockID LIKE '%' + @FilterText + '%')
			OR
			(@FilterBy = 'Customer' AND dbo.fnGetCustomersForInventory(SQ.StockID) LIKE '%' + @FilterText + '%')
			OR
			(@FilterBy = 'StockExternalID' AND SQ.ExternalID LIKE '%' + @FilterText + '%')
			OR
			(@FilterBy = 'ExternalID' AND INS.ExternalID LIKE '%' + @FilterText + '%')
			OR
			(@FilterBy = 'POExternalID' AND PO.ExternalID LIKE '%'+ @FilterText +'%')
			OR
			(@FilterBy = 'InspectionType' AND INSTYPE.TypeName LIKE '%'+ @FilterText +'%')
		))	AND
		(ISNULL(ACC.AccountName,'')+CAST(ISNULL(POL.PurchaseOrderID,'') AS VARCHAR(32))+ (ISNULL(SQ.ExternalID,'')) +CAST(ISNULL(dbo.fnGetEarliestShipDateOfSOLine(SQ.StockID),'') AS VARCHAR(32)) + CAST(ISNULL(dbo.fnGetSOLine(SQ.StockID),'')AS VARCHAR(32)) + ISNULL(PO.ExternalID,'') + ISNULL(INSST.StatusName,'') + ISNULL(w.WarehouseName,'') + ISNULL(INSTYPE.TypeName, '') LIKE '%' + ISNULL(@SearchString,'')+ '%' )
		
	),

	Count_CTE AS (
		SELECT COUNT(*) AS [RowCount]
		FROM Main_CTE
	)

SELECT *
FROM Main_CTE, Count_CTE
	
	ORDER BY 
		
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_CTE.InspectionID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'ItemId' THEN Main_CTE.ItemID 
				WHEN @SortBy = 'PoNumber' THEN Main_CTE.PONumber
				WHEN @SortBy in ('StockExternalID','InventoryId') THEN CAST(Main_CTE.StockExternalID AS INT)
				WHEN @SortBy = 'POExternalID' THEN CAST(Main_CTE.POExternalID AS INT)
				WHEN @SortBy = 'ExternalID' THEN CAST(Main_CTE.ExternalID AS INT)
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'Supplier' THEN Main_CTE.Supplier
				WHEN @SortBy = 'Status' THEN Main_CTE.StatusName
				WHEN @SortBy = 'Customers' THEN Main_CTE.Customers
				WHEN @SortBy = 'Warehouse' THEN Main_CTE.WarehouseName
				WHEN @SortBy = 'InspectionType' THEN Main_CTE.InspectionTypeName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'ShipDate' THEN Main_CTE.ShipDate 
				WHEN @SortBy = 'ReceivedDate' THEN Main_CTE.ReceivedDate
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_CTE.InspectionID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'ItemId' THEN Main_CTE.ItemID 
				WHEN @SortBy = 'PoNumber' THEN Main_CTE.PONumber
				WHEN @SortBy in ('StockExternalID','InventoryId') THEN CAST(Main_CTE.StockExternalID AS INT)
				WHEN @SortBy = 'POExternalID' THEN CAST(Main_CTE.POExternalID AS INT)
				WHEN @SortBy = 'ExternalID' THEN CAST(Main_CTE.ExternalID AS INT)
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'Supplier' THEN Main_CTE.Supplier
				WHEN @SortBy = 'Status' THEN Main_CTE.StatusName
				WHEN @SortBy = 'Customers' THEN Main_CTE.Customers
				WHEN @SortBy = 'Warehouse' THEN Main_CTE.WarehouseName
				WHEN @SortBy = 'InspectionType' THEN Main_CTE.InspectionTypeName
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'ReceivedDate' THEN Main_CTE.ReceivedDate
				WHEN @SortBy = 'ShipDate' THEN Main_CTE.ShipDate
			END
		END DESC		
		
		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY


	IF (@@rowcount = 0)
		RETURN -1
	END