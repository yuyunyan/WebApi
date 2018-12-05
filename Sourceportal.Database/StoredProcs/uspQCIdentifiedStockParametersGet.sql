/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.10.19
   Description:	Gets identified stock report parameters from ItemStock
   Usage:		EXEC uspQCIdentifiedStockParametersGet @InspectionID = 65
				SELECT * FROM ItemStockBreakdown WHERE STOCKID = 321
				UPDATE ItemStockBreakdown SET IsDiscrepant = 0 WHERE breakdownID = 119
   Revision History:
	
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspQCIdentifiedStockParametersGet]
(
	@InspectionID INT = NULL
)
AS
BEGIN
	
	SELECT I.StockID,
		BRD.isDiscrepant,
		CASE WHEN BRD.isDiscrepant = 0 THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END IsConforming,
		CASE WHEN BRD.isDiscrepant = 0 THEN 'Identified Stock Ticket' ELSE 'Discrepant Identified Stock Ticket' END ReportTitle
	FROM mapQCInspectionStock S
	INNER JOIN ItemStock I on I.StockID = S.StockID
	INNER JOIN ItemStockBreakdown BRD on BRD.StockID = I.StockID
	LEFT OUTER JOIN ItemInventory ii ON I.StockID = ii.StockID AND ii.IsDeleted = 0
	LEFT OUTER JOIN WarehouseBins whb ON whb.WarehouseBinID = ii.WarehouseBinID
	INNER JOIN WarehouseBins AB on AB.WarehouseBinID = dbo.fnGetWarehouseAcceptedBinID(ii.WarehouseBinID)	--Dont need this when launched, only to restrict bad data (no warehouse ID) from coming in
	INNER JOIN WarehouseBins RB on RB.WarehouseBinID = dbo.fnGetWarehouseRejectedBinID(ii.WarehouseBinID)	--Dont need this when launched, only to restrict bad data (no warehouse ID) from coming in

	WHERE InspectionID = @InspectionID
	AND S.isDeleted = 0

	GROUP BY I.StockID, isDiscrepant
	ORDER BY I.StockID

END

