/* =============================================
   Author:				Aaron Rodecker
   Create date:			2018.06.27
   Description:			Gets list of warehouses
   Revision History
		2018.09.06	NA	Added IsDeleted check, removed unused @ItemID parameter
		2018.10.09	CT	Added OrganizationID, ShipFromRegionID
		2018.11.07	JC	Added @OrganizationId parameter to filter by Organization field.
   =============================================*/
CREATE PROCEDURE [dbo].[uspGetWarehouses]
	@OrganizationID INT = null
AS
BEGIN
	SET NOCOUNT ON
	SELECT WarehouseID
		, WarehouseName
		, LocationID
		, ExternalID
		, OrganizationID
		, ShipFromRegionID
	FROM Warehouses
	WHERE IsDeleted = 0
	AND ISNULL(@OrganizationID, OrganizationId) = OrganizationId
END
