/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.08.10
   Description:	Returns all shipments for a given SOLineID
   Usage:	EXEC [uspSalesOrderLineShipmentsGet] @SOLineID = 30

   Return Codes:
	
   Revision History:

   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspSalesOrderLineShipmentsGet]
	@SOLineID INT = NULL
AS
BEGIN
	SELECT 
			s.ShipmentID,
			s.CarrierName,
			s.ShipDate,
			s.TrackingNumber,
			s.TrackingURL,
			ls.Qty,
			s.ExternalID 'ShipmentExternalID',
			ls.ExternalID 'LineExternalID'
	FROM mapSalesOrderLineShipments ls
	INNER JOIN Shipments s ON ls.ShipmentID = s.ShipmentID AND s.IsDeleted = 0
	WHERE SOLineID = @SOLineID 
	AND ls.IsDeleted = 0
	ORDER BY ShipDate DESC
END