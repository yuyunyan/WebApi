/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.4.23
   Description:	Retrieves Country by External ID
   Usage:		EXEC [uspCountriesGet] @ExternalId = 'US'
   Revision History:

   Added CountryCode2 ML 2017-10-26
   ============================================= */
CREATE PROCEDURE [dbo].[uspCountriesGetByExternal]
(
	@ExternalId VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		CountryID
	FROM Countries
	WHERE CountryCode2 = @ExternalId
END