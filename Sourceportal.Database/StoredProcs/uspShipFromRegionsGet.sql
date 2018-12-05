/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.10.11
   Description:	Returns a list of ShipFromRegions
   Usage:	EXEC [uspShipFromRegionsGet]
   Return Codes:
   Revision History:
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspShipFromRegionsGet]

AS
BEGIN
	SELECT 
		ShipfromRegionID,
		RegionName,
		OrganizationID,
		CountryID
	FROM lkpShipFromRegions
	WHERE IsDeleted = 0
END