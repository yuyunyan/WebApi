/* =============================================
Revision History:
2018.09.12	Julia Thoma Change XML to JSON AND ExternalID added 
 ============================================= */

ALTER   FUNCTION [dbo].[fnGetCustomersForInventory]
(
	@StockID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
    RETURN (SELECT AC.AccountName AS AccountName,
	      SO.SalesOrderID AS SalesOrderID,
		  SO.ExternalID AS ExternalID
		  FROM ItemStock IST 
		  INNER JOIN mapSOInvFulfillment SOIFF ON IST.StockID = SOIFF.StockID AND SOIFF.IsDeleted = 0
		  INNER JOIN vwSalesOrderLines SOL ON SOL.SOLineID = SOIFF.SOLineID
		  INNER JOIN vwSalesOrders SO ON SO.SalesOrderID = SOL.SalesOrderID
		  INNER JOIN Accounts AC ON AC.AccountID = SO.AccountID
		  WHERE IST.StockID = @StockID
		  FOR JSON PATH )
END



