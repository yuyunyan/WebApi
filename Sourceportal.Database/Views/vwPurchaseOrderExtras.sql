/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.08.01
   Description:	Limits the view of PurchaseOrderExtras to only those on the latest version of a Purchase Order
   Revision History:
	
   ============================================= */


CREATE VIEW [dbo].[vwPurchaseOrderExtras] AS
	
	SELECT poe.* 
	FROM PurchaseOrderExtras poe
	INNER JOIN vwPurchaseOrders vpo ON poe.PurchaseOrderID = vpo.PurchaseOrderID AND poe.POVersionID = vpo.VersionID
	WHERE poe.IsDeleted = 0