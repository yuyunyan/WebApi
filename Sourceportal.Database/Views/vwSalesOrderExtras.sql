/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.24
   Description:	Limits the view of SalesOrderExtras to only those on the latest version of a Sales Order
   Revision History:
	
   ============================================= */


CREATE VIEW [dbo].[vwSalesOrderExtras] AS
	
	SELECT soe.* 
	FROM SalesOrderExtras soe
	INNER JOIN vwSalesOrders vso ON soe.SalesOrderID = vso.SalesOrderID AND soe.SOVersionID = vso.VersionID
	WHERE soe.IsDeleted = 0