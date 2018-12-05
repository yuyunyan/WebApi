/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.08.01
   Description:	Limits the view of PurchaseOrderLines to only those on the latest version of a Purchase Order
   Revision History:
		2018.08.27	NA	Added IsCanceled and IsComplete
   ============================================= */

CREATE OR ALTER VIEW [dbo].[vwPurchaseOrderLines] AS
	
	SELECT pol.*, s.IsCanceled, s.IsComplete
	FROM PurchaseOrderLines pol
	INNER JOIN vwPurchaseOrders vpo ON pol.PurchaseOrderID = vpo.PurchaseOrderID AND pol.POVersionID = vpo.VersionID
	LEFT OUTER JOIN lkpStatuses s ON pol.StatusID = s.StatusID
	WHERE pol.IsDeleted = 0