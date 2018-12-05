/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.03.01
   Description:	finds matches based on external id
   Usage:	EXEC [uspAccountByExternalGet] @ExternalId = '1003'

   Return Codes:

   Revision History:

   ============================================= */
CREATE PROCEDURE [dbo].[uspAccountByExternalGet]
(
	@ExternalId NVARCHAR(50)
	
)
AS
BEGIN 
	SET NOCOUNT ON;

	  SELECT AccountID
	  FROM Accounts
	  WHERE ExternalID = @ExternalID
END