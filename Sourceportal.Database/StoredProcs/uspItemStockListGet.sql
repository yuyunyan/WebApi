/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.07.02
   Description:	Gets ItemStock list from inspectionID
   Usage:		EXEC uspItemStockListGet @InspectionID = 47
   Revision History: 2018.11.02	HR	Added MfrLotNum
	
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspItemStockListGet]
(
	@InspectionID INT = NULL
)
AS
BEGIN
	DECLARE @InspectionWarehouseID INT = (SELECT WarehouseID FROM QCInspections Q
											INNER JOIN  vwStockQty S on S.StockID = Q.StockID
											WHERE InspectionID = @InspectionID )
	SELECT I.StockID 'ItemStockID'
		, POLineID
		, I.ItemID
		, I.MfrLotNum
		, SUM(ii.qty) 'Qty'
		, POLineID
		, ItemID
		, IsRejected
		, InvStatusID
		, ReceivedDate
		, DateCode
		, PackagingID
		, PackageConditionID
		, COO
		, Expiry
		, I.ExternalID
		, I.IsDeleted
		, I.StockDescription
		, MIN(ii.WarehouseBinID) 'WarehouseBinID'
		, whb.WarehouseID 'WarehouseID'
		, @InspectionWarehouseID 'InspectionWarehouseID'
		, AB.WarehouseBinID 'AcceptedBinID'
		, AB.BinName 'AcceptedBinName'
		, RB.WarehouseBinID 'RejectedBinID'
		, RB.BinName 'RejectedBinName'
	FROM mapQCInspectionStock S
	INNER JOIN ItemStock I on I.StockID = S.StockID
	LEFT OUTER JOIN ItemInventory ii ON I.StockID = ii.StockID AND ii.IsDeleted = 0
	LEFT OUTER JOIN WarehouseBins whb ON whb.WarehouseBinID = ii.WarehouseBinID
	INNER JOIN WarehouseBins AB on AB.WarehouseBinID = dbo.fnGetWarehouseAcceptedBinID(ii.WarehouseBinID)
	INNER JOIN WarehouseBins RB on RB.WarehouseBinID = dbo.fnGetWarehouseRejectedBinID(ii.WarehouseBinID)
	WHERE InspectionID = @InspectionID
	AND S.isDeleted = 0
	GROUP BY
		I.StockID,
		I.POLineID,
		I.ItemID,
		I.MfrLotNum,
		I.InvStatusID,
		I.ReceivedDate,
		I.DateCode,
		I.Expiry,
		I.PackagingID,
		I.PackageConditionID,
		I.COO,
		I.ExternalID,
		I.IsDeleted,
		I.IsRejected,
		I.StockDescription,
		whb.WarehouseID,
		AB.WarehouseBinID,
		AB.BinName,
		RB.WarehouseBinID,
		RB.BinName
	ORDER BY I.StockID
END

