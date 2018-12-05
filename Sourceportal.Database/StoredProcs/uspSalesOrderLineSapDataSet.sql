/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.02.06
   Description:	updates a line item on a Sales Order with new SAP data
   Usage:	EXEC uspSalesOrderLineSet @LineNum = 1, @SalesOrderID = 1, @SOVersionID = 3, @ProductSpec = '345', @UserID = 1		
   Return Codes:

   Revision History:
			2018.08.09	NA	Added DeliveryStatus and InvoiceStatus
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspSalesOrderLineSapDataSet]
	@LineNum INT,
	@SalesOrderID INT,
	@SOVersionID INT,
	@ProductSpec VARCHAR(50),
	@DeliveryStatus VARCHAR(100) = NULL,
	@InvoiceStatus VARCHAR(100) = NULL,
	@UserID INT
AS
BEGIN
	SET NOCOUNT ON;
	--Update the record
	UPDATE SalesOrderLines
	SET		
		ProductSpec = @ProductSpec,
		DeliveryStatus = ISNULL(@DeliveryStatus, DeliveryStatus),
		InvoiceStatus = ISNULL(@InvoiceStatus, InvoiceStatus),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE SalesOrderID = @SalesOrderID AND LineNum = @LineNum AND SOVersionID = @SOVersionID
END