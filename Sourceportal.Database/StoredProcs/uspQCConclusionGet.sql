/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.24
   Description:	Retrieves conclusion data for an inspectionId
   Usage:		EXEC uspQCConclusionGet 7
   Revision History:
   2017.11.06	AR	Added Case on QtyFailed to use saved value over predicted value if saved value exists
   2018.05.29	AR	Added "TOP 1"
   2018.06.22	NA	Rewrote procedure for new QC and ItemStock design
     Return Codes:
   ============================================= */
CREATE   PROCEDURE [dbo].[uspQCConclusionGet]
(
	@InspectionID INT
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		Q.InspectionID,
		SUM(sqA.Qty - isb.Qty) 'QtyPassed',
		SUM(isb.Qty) 'QtyDiscrepantAccepted',
		SUM(sqR.Qty) 'QtyRejected',
		InspectionQty,
		--CI.Comment Introduction,
		--CTR.Comment TestResults,
		CC.Comment 'Conclusion'
	FROM QCInspections Q
	INNER JOIN mapQCInspectionStock qcis ON q.InspectionID = qcis.InspectionID AND qcis.IsDeleted = 0
	LEFT OUTER JOIN vwStockQty sqA on qcis.StockID = sqA.StockID AND sqA.IsRejected = 0 AND sqA.IsDeleted = 0 --Accepted stock
	LEFT OUTER JOIN (	SELECT StockID, SUM(PackQty * NumPacks) 'Qty'  --Discrepant accepted stock
						FROM ItemStockBreakdown 
						WHERE IsDiscrepant = 1 
						  AND IsDeleted = 0
						GROUP BY StockID
					 ) isb ON sqA.StockID = isb.StockID
	LEFT OUTER JOIN vwStockQty sqR on qcis.StockID = sqR.StockID AND sqR.IsRejected = 1 AND sqR.IsDeleted = 0 --Rejected stock	
	--LEFT OUTER JOIN Comments CI on CI.ObjectID = @InspectionID AND CI.CommentTypeID = 1
	--LEFT OUTER JOIN Comments CTR on CTR.ObjectID = @InspectionID AND CTR.CommentTypeID = 2
	LEFT OUTER JOIN Comments CC on CC.ObjectID = @InspectionID AND CC.CommentTypeID = 3
	WHERE Q.InspectionID = @InspectionID
	GROUP BY q.InspectionID, q.InspectionQty, cc.Comment
END