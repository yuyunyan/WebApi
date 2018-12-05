/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.06.15
   Description:	Gets the line items summary for sales order
   Usage:		EXEC uspSalesOrderSummaryTotalGet @SalesOrderID = 101483, @VersionID = 1
	
   Revision History:
		2018.06.15	AR	Intitial Deployment
		2018.09.20	AR	refactored to sub query so that subTotal and extTotalCost can be added without duplicating math functions
   Return Codes:
	
   ============================================= */

CREATE PROCEDURE [dbo].[uspSalesOrderSummaryTotalGet]
	@SalesOrderID INT = NULL,
	@VersionID INT = NULL
AS
BEGIN
	SELECT dbo.fnFormatWithCommas(SubTotal) + ' ' + CurrencyID 'SubTotal',
		dbo.fnFormatWithCommas(ExtTotalCost) + ' ' + CurrencyID 'ExtTotalCost',
		GST,
		dbo.fnFormatWithCommas(OrderTotal) + ' ' + CurrencyID 'OrderTotal',											--Sales Order Confirmation "Order Total"
		dbo.fnFormatWithCommas(CAST(SubTotal + ExtTotalCost AS NUMERIC(12,2))) + ' ' + CurrencyID 'ExtOrderTotal'	--Internal Sales Order "Order Total"
	FROM (
		SELECT CAST(ROUND(SUM(sol.Qty * sol.Price), 2) AS NUMERIC(12,2)) AS SubTotal,
			'0' GST,
			CAST(ROUND(SUM(sol.Qty * sol.Price), 2) AS NUMERIC(12,2)) AS OrderTotal,--add GST sum when available
			SUM(sol.Cost) AS ExtTotalCost,
			MAX(A.CurrencyID) CurrencyID
		FROM SalesOrderLines sol
		INNER JOIN SalesOrders SO on SO.SalesOrderID = sol.SalesOrderID AND SO.VersionID = @VersionID
		INNER JOIN Accounts A on A.AccountID = (SELECT AccountID FROM SalesOrders WHERE SalesOrderID = @SalesOrderID AND VersionID = @VersionID)
		INNER JOIN Items i ON sol.ItemID = i.ItemID
		INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
		INNER JOIN lkpItemCommodities c ON i.CommodityID = c.CommodityID
		WHERE sol.SalesOrderID = @SalesOrderID AND sol.SOVersionID = @VersionID
		AND sol.IsDeleted = 0
	) tbl
END