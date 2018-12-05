/* =============================================
	Author:		  Berry, Zhong
	Create date:  2017.08.08
	Description:  Return info about purchase order allocation to Sales Orders for a POLineID
	Return Codes: 
				  -1 Missing POLineID
	Revision Hsitory:
				2018.05.11	NA	Converted to use views and added status to return.		
	============================================= */
CREATE PROCEDURE [dbo].[uspPurchaseOrderAllocationGet]
	-- Add the parameters for the stored procedure here
	@POLineID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@POLineID, 0) = 0
		RETURN -1

	SELECT
		so.SalesOrderID
		, sol.SOLineID
		, sol.LineNum
		, a.AccountName 'Customer'
		, dbo.fnGetObjectOwners(so.QuoteID, 16) 'Owners'
		, sol.Qty 'OrderQty'
		, poa.Qty 'ResevedQty'
		, sos.StatusID 'SOStatusID'
		, sos.Statusname 'SOStatus'
		, sols.StatusID 'SOLineStatusID'
		, sols.StatusName 'SOLineStatus'
		, so.VersionID
		, so.OrderDate
	FROM mapSOPOAllocation poa
	INNER JOIN vwSalesOrderLines sol ON sol.SOLineID = poa.SOLineID
	INNER JOIN vwSalesOrders so ON so.SalesOrderID = sol.SalesOrderID
	INNER JOIN vwPurchaseOrderLines pol ON poa.POLineID = pol.POLineID
	INNER JOIN vwPurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID
	INNER JOIN lkpStatuses sos ON so.StatusID = sos.StatusID
	INNER JOIN lkpStatuses sols ON sol.StatusID = sols.StatusID
	INNER JOIN Accounts a ON a.AccountID = so.AccountID
	WHERE poa.POLineID = @POLineID 
	AND poa.IsDeleted = 0
END
	-- Add the parameters for the stored procedure here
	@POLineID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@POLineID, 0) = 0
		RETURN -1

	SELECT
		so.SalesOrderID
		, sol.SOLineID
		, sol.LineNum
		, a.AccountName 'Customer'
		, dbo.fnGetObjectOwners(so.QuoteID, 16) 'Owners'
		, sol.Qty 'OrderQty'
		, poa.Qty 'ResevedQty'
		, sos.StatusID 'SOStatusID'
		, sos.Statusname 'SOStatus'
		, sols.StatusID 'SOLineStatusID'
		, sols.StatusName 'SOLineStatus'
		, so.VersionID
		, so.OrderDate
	FROM mapSOPOAllocation poa
	INNER JOIN vwSalesOrderLines sol ON sol.SOLineID = poa.SOLineID
	INNER JOIN vwSalesOrders so ON so.SalesOrderID = sol.SalesOrderID
	INNER JOIN vwPurchaseOrderLines pol ON poa.POLineID = pol.POLineID
	INNER JOIN vwPurchaseOrders po ON pol.PurchaseOrderID = po.PurchaseOrderID
	INNER JOIN lkpStatuses sos ON so.StatusID = sos.StatusID
	INNER JOIN lkpStatuses sols ON sol.StatusID = sols.StatusID
	INNER JOIN Accounts a ON a.AccountID = so.AccountID
	WHERE poa.POLineID = @POLineID 
	AND poa.IsDeleted = 0
END
