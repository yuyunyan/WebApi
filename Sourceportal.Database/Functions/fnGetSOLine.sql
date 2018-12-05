CREATE FUNCTION [dbo].[fnGetSOLine]
(
	@StockID INT
)
RETURNS VARCHAR(32)
AS
BEGIN
    RETURN (SELECT ISNULL(CONVERT(VARCHAR(50), SO.ExternalID),'') + ' ' + ISNULL(CONVERT(VARCHAR(50), SO.SalesOrderID),'')
         FROM ItemStock II
         INNER JOIN mapSOInvFulfillment SOIFF ON II.StockID = SOIFF.StockID AND SOIFF.IsDeleted = 0
         INNER JOIN vwSalesOrderLines SOL ON SOL.SOLineID = SOIFF.SOLineID
         INNER JOIN SalesOrders SO ON SO.SalesOrderID = SOL.SalesOrderID
         WHERE II.StockID = @StockID)
END