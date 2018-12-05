/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.03.01
   Description:	finds matches based on external id
   Usage:	EXEC [uspContactByExternalGet] @ExternalId = '1003'

   Return Codes:

   Revision History:

   ============================================= */
CREATE PROCEDURE [dbo].[uspContactByExternalGet]
(
	@ExternalId NVARCHAR(50)
	
)
AS
BEGIN 
	SET NOCOUNT ON;

	  SELECT ContactID
	  FROM Contacts
	  WHERE ExternalID = @ExternalID
END