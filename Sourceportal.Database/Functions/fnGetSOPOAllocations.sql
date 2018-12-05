CREATE OR ALTER FUNCTION [dbo].[fnGetSOPOAllocations]
(
	@POLineID INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN	
	RETURN (	
	SELECT	sopo.SOLineID AS [SOLineID], 
			sol.SalesOrderID AS [SalesOrderID], 
			sol.SOVersionID AS [SOVersionID], 
			sol.LineNum AS [LineNum], 
			so.AccountID AS [AccountID], 
			a.AccountName AS [AccountName], 
			sol.Qty AS [OrderQty], 
			sopo.Qty AS [ResvQty],
			so.ExternalID AS [ExternalID]
	FROM mapSOPOAllocation sopo
	  INNER JOIN SalesOrderLines sol ON sopo.SOLineID = sol.SOLineID
	  INNER JOIN SalesOrders so ON sol.SalesOrderID = so.SalesOrderID AND sol.SOVersionID = so.VersionID
	  INNER JOIN Accounts a ON so.AccountID = a.AccountID
	WHERE POLineID = @POLineID AND sopo.IsDeleted = 0
	FOR JSON PATH)
END
