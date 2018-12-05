CREATE OR ALTER FUNCTION [dbo].[fnGetInvFulfillments]
(
	@StockID INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN	
	RETURN (	
	SELECT	ful.SOLineID AS [SOLineID], 
			sol.SalesOrderID AS [SalesOrderID], 
			sol.SOVersionID AS [SOVersionID], 
			sol.LineNum AS [LineNum], 
			so.AccountID AS [AccountID], 
			a.AccountName AS [AccountName], 
			sol.Qty AS [OrderQty], 
			ful.Qty AS [ResvQty],
			so.ExternalID AS [ExternalID]
	FROM mapSOInvFulfillment ful
	  INNER JOIN vwSalesOrderLines sol ON ful.SOLineID = sol.SOLineID
	  INNER JOIN vwSalesOrders so ON sol.SalesOrderID = so.SalesOrderID
	  INNER JOIN Accounts a ON so.AccountID = a.AccountID
	WHERE ful.StockID = @StockID AND ful.IsDeleted = 0
	FOR JSON PATH)
END
