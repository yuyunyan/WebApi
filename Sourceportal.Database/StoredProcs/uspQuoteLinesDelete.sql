/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.14
   Description:	Deletes one or more Quote Lines
   Usage:	EXEC uspQuoteLinesDelete @QuoteLinesJSON = '[{"QuoteLineID":16}, {"QuoteLineID":88}]', @UserID = 0		
   Return Codes:
			-6 Missing UserID
			-8 Missing JSON list of Quote Lines to be deleted
   Revision History:
			
   ============================================= */

   
CREATE PROCEDURE [dbo].[uspQuoteLinesDelete]
	@QuoteLinesJSON VARCHAR(MAX) = NULL,
	@UserID INT = NULL,
	@ResultCount INT = NULL OUTPUT 
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@QuoteLinesJSON, '') = ''
		RETURN -8

	IF ISNULL(@UserID, 0) = 0
		RETURN -6

	UPDATE ql
	SET IsDeleted = 1,
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	FROM QuoteLines ql
	  INNER JOIN OPENJSON(@QuoteLinesJSON) WITH (QuoteLineID INT) AS j ON (ql.QuoteLineID = j.QuoteLineID or ql.AltFor = j.QuoteLineID)

	SET @ResultCount = @@ROWCOUNT
END