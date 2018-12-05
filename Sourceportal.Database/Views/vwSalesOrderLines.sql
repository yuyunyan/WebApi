/* =============================================
   Author:		??
   Create date: ??
   Description:	Limits the view of SalesOrderLines to only those on the latest version of a Sales Order
   Revision History:
		2018.08.27	NA	Added IsCanceled and IsComplete
   ============================================= */

CREATE OR ALTER VIEW [dbo].[vwSalesOrderLines] AS
	
	SELECT sol.*, s.IsCanceled, s.IsComplete
	FROM SalesOrderLines sol
	INNER JOIN vwSalesOrders vso ON sol.SalesOrderID = vso.SalesOrderID AND sol.SOVersionID = vso.VersionID
	LEFT OUTER JOIN lkpStatuses s ON sol.StatusID = s.StatusID
	WHERE sol.IsDeleted = 0