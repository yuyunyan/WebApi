/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.08
   Description:	Gets a list of payment terms
   Usage: EXEC uspPaymentTermsGet
   Revision History:
       2017.11.02  CT  Added ExternalID
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspPaymentTermsGet]
	@PaymentTermID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT PaymentTermID,
	TermName,
	TermDesc,
	NetDueDays,
	DiscountDays,
	DiscountPercent,
	ExternalID
	FROM [codes].[lkpPaymentTerms]
	WHERE PaymentTermID = ISNULL(NULLIF(@PaymentTermID,0),PaymentTermID)
END