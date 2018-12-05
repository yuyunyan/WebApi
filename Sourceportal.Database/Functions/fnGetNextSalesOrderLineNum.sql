/* =============================================
	Author:			Corey Tyrrell
	Create date:	2017.11.03
	Description:	Return the next Line Num of the Sales Order
   =============================================*/
CREATE FUNCTION [dbo].[fnGetNextSalesOrderLineNum]
(
	@SalesOrderID INT,
	@SOVersionID INT
)
RETURNS INT
AS
BEGIN
	RETURN (
		SELECT ISNULL(MAX(LineNum), 0) + 1 
		FROM SalesOrderLines 
		WHERE SalesOrderID = @SalesOrderID AND SOVersionID = @SOVersionID
	)
END