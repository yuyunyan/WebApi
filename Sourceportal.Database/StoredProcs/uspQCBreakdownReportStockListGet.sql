/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.07.16
   Description:	Gets ItemStock list data from inspectionID
   Usage:		EXEC uspQCBreakdownReportStockListGet @InspectionID = 65, @IsDiscrepant = 1, @StockID=339
				EXEC uspItemStockListGet @InspectionID = 65
   Revision History:
	
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspQCBreakdownReportStockListGet]
(
	@InspectionID INT = NULL,
	@IsDiscrepant BIT = NULL,
	@StockID INT = NULL
)
AS
BEGIN
	DECLARE @InspectionWarehouseID INT = (SELECT WarehouseID FROM QCInspections Q
											INNER JOIN  vwStockQty S on S.StockID = Q.StockID
											WHERE InspectionID = @InspectionID )
	SELECT I.StockID 'ItemStockID'
		, B.BreakdownID
		, I.POLineID
		, I.ItemID
		, b.PackQty 'QtyPerPackage'
		, b.NumPacks 'NumPackages'
		, SUM(b.PackQty * b.NumPacks) 'TotalQty'
		, I.ItemID
		, IT.PartNumber
		, B.MfrLotNum 'LotNumber'
		, CASE WHEN IT.EUROHS = 1 THEN 'Yes'
			WHEN IT.EUROHS = 0 THEN 'No' ELSE'N/A' END 'Rohs'
		, IT.MSL
		, M.MfrName
		, I.IsRejected
		, I.InvStatusID
		, I.ReceivedDate
		, B.DateCode
		, B.PackagingID
		, PT.PackagingName
		, B.PackageConditionID
		, PC.ConditionName 
		, I.COO
		, C.CountryName
		, I.Expiry
		, IT.PartNumber
		, I.ExternalID
		, I.IsDeleted
		, I.StockDescription
		, MIN(ii.WarehouseBinID) 'WarehouseBinID'
		, whb.WarehouseID 'WarehouseID'
		, @InspectionWarehouseID 'InspectionWarehouseID'
	FROM mapQCInspectionStock S
	INNER JOIN ItemStock I on I.StockID = S.StockID
	INNER JOIN ItemStockBreakdown B on S.StockID = B.StockID
	INNER JOIN Items IT on IT.ItemID = I.ItemID
	INNER JOIN Manufacturers M on M.MfrID = IT.MfrID
	LEFT OUTER JOIN vwStockQty SQ on SQ.StockID = I.StockID
	INNER JOIN [codes].lkpPackaging PT on PT.PackagingID = B.PackagingID
	INNER JOIN [codes].lkpPackageConditions PC on PC.PackageConditionID = B.PackageConditionID
	LEFT OUTER JOIN ItemInventory ii ON I.StockID = ii.StockID AND ii.IsDeleted = 0
	LEFT OUTER JOIN WarehouseBins whb ON whb.WarehouseBinID = ii.WarehouseBinID
	INNER JOIN Countries C ON C.CountryID = I.COO
	WHERE InspectionID = @InspectionID
	AND S.StockID = ISNULL(@StockID, S.StockID)
	AND B.isDeleted = 0
	AND B.IsDiscrepant = ISNULL(@IsDiscrepant, 0)
	GROUP BY
		I.StockID,
		B.BreakdownID,
		I.POLineID,
		I.ItemID,
		b.NumPacks,
		IT.PartNumber,
		SQ.ExternalID,
		M.MfrName,
		IT.EUROHS,
		IT.MSL,
		I.InvStatusID,
		I.ReceivedDate,
		B.DateCode,
		b.PackQty,
		b.PackageConditionID,
		pc.ConditionName,
		I.Expiry,
		B.PackagingID,
		PT.PackagingName,
		I.PackageConditionID,
		I.COO,
		I.ExternalID,
		I.IsDeleted,
		I.IsRejected,
		I.StockDescription,
		whb.WarehouseID,
		B.MfrLotNum,
		CountryName
	ORDER BY I.StockID
END