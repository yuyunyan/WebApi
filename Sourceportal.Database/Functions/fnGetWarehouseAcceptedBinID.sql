/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.09.11
				Returns acceptedBinID from warehouse table for a given warehouseBinID
   Usage:		Select dbo.fnGetWarehouseAcceptedBinID(64)

   Return Codes:
   Revision History:
		2018.09.11	AR	Initial deployment
============================================= */
CREATE FUNCTION [dbo].[fnGetWarehouseAcceptedBinID](
	@WarehouseBinID INT
	)
RETURNS INT
AS
BEGIN
    RETURN (SELECT TOP 1 AcceptedBinID FROM Warehouses W
			INNER JOIN WarehouseBins B on B.WarehouseID = W.WarehouseID
			WHERE B.WarehouseBinID = @WarehouseBinID)

END
