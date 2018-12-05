/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.06.07
   Description:	Retrieves all records from Countries
   Usage:		EXEC uspCountriesGet
   Revision History:

   Added CountryCode2 ML 2017-10-26
   ============================================= */
CREATE PROCEDURE [dbo].[uspCountriesGet]
(
	@CountryID INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		CountryID
		, CountryCode
		, CountryName
		, CountryCode2
	FROM Countries
	WHERE CountryID = ISNULL(@CountryID,CountryID)
END