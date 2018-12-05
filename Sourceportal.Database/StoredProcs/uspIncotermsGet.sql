/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.11.02
   Description:	Gets a list of currencies
   Usage: EXEC [uspIncotermsGet]
   Revision History:
       
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspIncotermsGet]
	@IncotermID int = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
	IncotermID,
	IncotermName,
	ExternalID
	FROM codes.lkpIncoterms
	WHERE IncotermID = ISNULL(NULLIF(@IncotermID,''),IncotermID)
END