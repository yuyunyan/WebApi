/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.03.01
   Description:	finds matches based on external id
   Usage:	EXEC [uspStatusByExternalGet] @ExternalId = '1003'

   Return Codes:

   Revision History:

   ============================================= */
CREATE PROCEDURE [dbo].[uspStatusByExternalGet]
(
	@ExternalId NVARCHAR(50)
	
)
AS
BEGIN 
	SET NOCOUNT ON;

	  SELECT AccountStatusID
	  FROM lkpAccountStatuses
	  WHERE ExternalID = @ExternalID
END