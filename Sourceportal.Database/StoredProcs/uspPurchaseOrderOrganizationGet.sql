/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.06.04
   Description:	Gets the line items cost SUM
   Usage:		EXEC uspPurchaseOrderOrganizationGet @PurchaseOrderID = 100009, @VersionID = 2

   Revision History:
		2018.06.04	AR	Intitial Deployment
   Return Codes:

   ============================================= */

CREATE   PROCEDURE [dbo].[uspPurchaseOrderOrganizationGet]
	
	@PurchaseOrderID INT = NULL,
	@VersionID INT = NULL
AS
BEGIN
SELECT O.[Name]
	FROM PurchaseOrders po
	INNER JOIN Accounts A on A.AccountID = po.AccountID
	INNER JOIN Organizations O on O.OrganizationID = A.OrganizationID
	WHERE po.PurchaseOrderID = @PurchaseOrderID AND po.VersionID = @VersionID
	AND po.IsDeleted = 0
END