

/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.19
   Description:	Limits the view of SalesOrders to only their most recent version
   Revision History:
	
   ============================================= */

CREATE VIEW [dbo].[vwSalesOrders] AS 

	WITH s AS
		(
			SELECT *,
				ROW_NUMBER() OVER (PARTITION BY SalesOrderID ORDER BY VersionID DESC) AS vn
			FROM SalesOrders
			WHERE IsDeleted = 0			  
		)
	SELECT * FROM s WHERE vn = 1
