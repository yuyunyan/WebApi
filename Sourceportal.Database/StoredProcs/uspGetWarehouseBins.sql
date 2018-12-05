/* =============================================
   Author:			Aaron Rodecker
   Create date:		2018.06.27
   Description:		Gets list of warehouse bins
   Revision History:
		2018.09.06	NA	Added IsDeleted check
   =============================================*/
CREATE PROCEDURE [dbo].[uspGetWarehouseBins]
AS
BEGIN
	SET NOCOUNT ON
	SELECT WarehouseBinID
		, WarehouseID
		, BinName
		, ExternalID
	FROM WarehouseBins
	WHERE IsDeleted = 0
END
