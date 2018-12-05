/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.03.01
   Description:	finds matches based on external id
   Usage:	EXEC [uspLocationByExternalGet] @ExternalId = '1003'

   Return Codes:

   Revision History:

   ============================================= */
CREATE PROCEDURE [dbo].[uspLocationByExternalGet]
(
	@ExternalId NVARCHAR(50)
	
)
AS
BEGIN 
	SET NOCOUNT ON;

	  SELECT LocationID
	  FROM Locations
	  WHERE ExternalID = @ExternalID
END