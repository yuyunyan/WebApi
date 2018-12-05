/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.4.23
   Description:	Retrieves State ID by External ID
   Usage:		EXEC uspStatesGetByExternal @ExternalId  = 'CA'
   Revision History:
   ============================================= */
CREATE PROCEDURE [dbo].[uspStatesGetByExternal]
(
	@ExternalId VARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		StateID
	FROM States S
	WHERE S.StateCode = @ExternalId
END