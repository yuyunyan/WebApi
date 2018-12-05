/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.02.06
   Description:	updates purchase order with new SAP data
   Usage:	EXEC uspAccountSapDataSet @AccountID = 1, @ExternalId = '345', @UserID = 1		
   Return Codes:

   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspPurchaseOrderSapDataSet]
	@PurchaseOrderID INT,
	@ExternalId VARCHAR(50),
	@UserID INT
AS
BEGIN
	SET NOCOUNT ON;
	--Update the record
	UPDATE PurchaseOrders
	SET		
		ExternalId = @ExternalId,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE PurchaseOrderID = @PurchaseOrderID 
END