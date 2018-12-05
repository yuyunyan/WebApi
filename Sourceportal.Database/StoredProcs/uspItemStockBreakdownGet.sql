/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.06.27
   Description:	Gets ItemStockBreakdown list details
   Usage: EXEC uspItemStockBreakdownGet @StockID = 110
   Revision History:
		2018.11.01	NA	Added MfrLotNum
		
   Return Codes:
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspItemStockBreakdownGet]
(
	@StockID INT
)
AS
BEGIN
	SELECT
		BreakdownID,
		isb.StockID,
		isb.IsDiscrepant,
		isb.PackQty,
		isb.NumPacks,
		isb.DateCode,
		isb.PackagingID,
		isb.PackageConditionID,
		isb.COO,
		isb.Expiry,
		isb.MfrLotNum,
		isb.IsDeleted
	FROM ItemStockBreakdown isb
	WHERE isb.StockID = @StockID
	AND isb.isDeleted = 0
END