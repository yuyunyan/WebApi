/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.09.11
				Returns rejectedBinID from warehouse table for a given warehouseBinID
   Usage:		Select dbo.fnGetWarehouseRejectedBinID(64)

   Return Codes:
   Revision History:
		2018.09.11	AR	Initial deployment
============================================= */
CREATE FUNCTION [dbo].[fnGetWarehouseRejectedBinID](
	@WarehouseBinID INT
	)
RETURNS INT
AS
BEGIN
    RETURN (SELECT TOP 1 RejectedBinID FROM Warehouses W
			INNER JOIN WarehouseBins B on B.WarehouseID = W.WarehouseID
			WHERE B.WarehouseBinID = @WarehouseBinID)

END
