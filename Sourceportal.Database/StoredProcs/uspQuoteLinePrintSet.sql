/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.02.07
   Description:	Inserts or updates a line item on a Quote
   Usage:	EXEC uspQuoteLinePrintSet @QuoteLineID = 12345, @IsPrinted = 1
   		
   Return Codes:
			-1	IsPrinted Value invalid
			-2	Missing/invalid QuoteLineID
			
   Revision History:
			2017.08.04	AR	Initial Deployment
   ============================================= */

CREATE PROCEDURE [dbo].[uspQuoteLinePrintSet]
	@QuoteLineID INT = NULL,
	@IsPrinted BIT = 1
AS
BEGIN

	IF (@IsPrinted NOT IN (0,1))
		RETURN -1
	SET NOCOUNT ON;
	UPDATE QuoteLines
	SET IsPrinted = @IsPrinted
	WHERE QuoteLineID = @QuoteLineID

	IF (@@rowcount = 0)
		RETURN -2

END