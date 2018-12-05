/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.10.17
   Description:	Gets ItemStock list total qty from inspectionID
   Usage:		EXEC uspQCBreakdownReportTotalGet @InspectionID = 65
   
   Revision History:
	
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspQCBreakdownReportTotalGet]
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
	SELECT SUM(TotalQty) TotalQty
	FROM
	(	SELECT SUM(b.PackQty* Numpacks) TotalQty
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
			CountryName
		) T

END