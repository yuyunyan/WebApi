/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.06.25
   Description:	Gets ItemStock and its qty
   Usage: EXEC uspItemStockGet @StockID = 110
   Revision History:
		2018.11.01	NA	Added MfrLotNum
   Return Codes:
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspItemStockGet]
(
	@StockID INT
)
AS
BEGIN
	SELECT
		ist.StockID 'ItemStockID',
		ist.POLineID,
		ist.ItemID,
		SUM(ii.qty) 'Qty',
		ist.IsRejected,
		ist.InvStatusID,
		ist.ReceivedDate,
		ist.DateCode,
		ist.Expiry,
		ist.PackagingID,
		ist.PackageConditionID,
		ist.COO,
		ist.MfrLotNum,
		ist.ExternalID,
		ist.IsDeleted,
		MIN(ii.WarehouseBinID) 'WarehouseBinID',
		whb.WarehouseID 'WarehouseID',
		StockDescription
	FROM ItemStock ist
	LEFT OUTER JOIN ItemInventory ii ON ist.StockID = ii.StockID AND ii.IsDeleted = 0
	LEFT OUTER JOIN WarehouseBins whb ON whb.WarehouseBinID = ii.WarehouseBinID
	WHERE ist.StockID = @StockID
	GROUP BY
		ist.StockID,
		ist.POLineID,
		ist.ItemID,
		ist.InvStatusID,
		ist.ReceivedDate,
		ist.DateCode,
		ist.Expiry,
		ist.PackagingID,
		ist.PackageConditionID,
		ist.COO,
		ist.MfrLotNum,
		ist.ExternalID,
		ist.IsDeleted,
		ist.IsRejected,
		whb.WarehouseID,
		StockDescription
END