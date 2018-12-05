/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.03.01
   Description:	finds matches based on external id
   Usage:	EXEC [uspPaymentTermByExternalGet] @ExternalId = '345'

   Return Codes:

   Revision History:

   ============================================= */
CREATE PROCEDURE [dbo].[uspPaymentTermByExternalGet]
(
	@ExternalId NVARCHAR(50)
	
)
AS
BEGIN 
	SET NOCOUNT ON;

	  SELECT PaymentTermID
	  FROM codes.lkpPaymentTerms
	  WHERE ExternalID = @ExternalID
END