/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.08.01
   Description:	Limits the view of Purchase Orders to only their most recent version
   Revision History:
	
   ============================================= */

CREATE VIEW [dbo].[vwPurchaseOrders] AS 

	WITH po AS
		(
			SELECT *,
				ROW_NUMBER() OVER (PARTITION BY PurchaseOrderID ORDER BY VersionID DESC) AS vn
			FROM PurchaseOrders
			WHERE IsDeleted = 0			  
		)
	SELECT * FROM po WHERE vn = 1
