/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.06.29
   Description:	Inserts record into mapQCInspectionStock
   Usage:		exec mapQCInspectionStock 10, 154
   Revision History:

   Return Codes:


   ============================================= */
CREATE PROCEDURE [dbo].[uspMapInspectionStockSet]
(
	@StockID INT = NULL,
	@InspectionID INT = NULL,
	@CreatedBy INT = NULL
)
AS
BEGIN
	INSERT INTO dbo.mapQCInspectionStock (InspectionID, StockID, CreatedBy)
	VALUES (@InspectionID, @StockID, @CreatedBy)

	SELECT @@ROWCOUNT
END
