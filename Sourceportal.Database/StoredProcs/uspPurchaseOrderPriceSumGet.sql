/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.06.04
   Description:	Gets the line items cost SUM
   Usage:		EXEC uspPurchaseOrderPriceSumGet @PurchaseOrderID = 100009, @VersionID = 2

   Revision History:
		2018.02.02	AR	Intitial Deployment
   Return Codes:

   ============================================= */

CREATE   PROCEDURE [dbo].[uspPurchaseOrderPriceSumGet]
	
	@PurchaseOrderID INT = NULL,
	@VersionID INT = NULL
AS
BEGIN
SELECT dbo.fnFormatWithCommas(CAST(ROUND(SUM(pol.Qty * pol.Cost), 2) AS NUMERIC(12,2)))
		+ ' ' + MAX(A.CurrencyID) AS PriceSum	
	FROM PurchaseOrderLines pol
	  INNER JOIN Accounts A on A.AccountID = (SELECT AccountID FROM PurchaseOrders WHERE PurchaseOrderID = @PurchaseOrderID AND VersionID = @VersionID)
	  INNER JOIN Items i ON pol.ItemID = i.ItemID
	  WHERE pol.PurchaseOrderID = @PurchaseOrderID AND pol.POVersionID = @VersionID
	  AND pol.IsDeleted = 0
END