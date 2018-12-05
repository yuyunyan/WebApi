/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.06.07
   Description:	Retrieves all records from States using CountryID
   Usage:		EXEC uspStatesGet @CountryID  = 240
   Revision History:
   ============================================= */
CREATE PROCEDURE [dbo].[uspStatesGet]
(
	@CountryID INT
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		StateID
		, S.StateName
		, S.StateCode
	FROM States S
	WHERE CountryID = @CountryID
END