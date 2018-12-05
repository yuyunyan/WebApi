CREATE FUNCTION [dbo].[fnGetEarliestShipDateOfSOLine]
(
	@StockID INT
)
RETURNS Date
AS
BEGIN
    RETURN (SELECT MIN(SOL.ShipDate) 
		  FROM ItemStock II 
		  INNER JOIN mapSOInvFulfillment SOIFF ON II.StockID = SOIFF.StockID AND SOIFF.IsDeleted = 0
		  INNER JOIN vwSalesOrderLines SOL ON SOL.SOLineID = SOIFF.SOLineID
		  WHERE II.StockID = @StockID)
END