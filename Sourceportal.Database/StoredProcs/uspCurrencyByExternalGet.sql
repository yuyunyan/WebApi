/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.03.01
   Description:	finds matches based on external id
   Usage:	EXEC [uspCurrencyByExternalGet] @ExternalId = '1003'

   Return Codes:

   Revision History:

   ============================================= */
CREATE PROCEDURE [dbo].[uspCurrencyByExternalGet]
(
	@ExternalId NVARCHAR(50)
	
)
AS
BEGIN 
	SET NOCOUNT ON;

	  SELECT CurrencyID
	  FROM lkpCurrencies
	  WHERE ExternalID = @ExternalID
END