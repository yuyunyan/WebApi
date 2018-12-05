/* =============================================
   Author:			Berry, Zhong
   Create date:		2017.08.08
   Description:		Route Quote Lines to buyer
   Usage:			EXEC uspQuoteLinesRouteTo @QuoteLinesJSON = '[{"QuoteLineID":16}, {"QuoteLineID":88}]', @BuyerID = 2, @IsSpecificBuyer=1, @UserID = 1
   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspQuoteLinesRouteTo]
	@QuoteLinesJSON VARCHAR(MAX) = NULL,
	@BuyerID INT = NULL,
	@IsSpecificBuyer BIT = 0,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @ResultCount INT;

	UPDATE ql
	SET RoutedToUserID = CASE WHEN @IsSpecificBuyer = 1 THEN @BuyerID ELSE NULL END,
		StatusID = (SELECT ConfigValue FROM lkpConfigVariables WHERE ConfigName = 'QuoteLineRoutedToBuyerStatus'),
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	FROM QuoteLines ql
	  INNER JOIN OPENJSON(@QuoteLinesJSON) WITH (QuoteLineID INT) AS j ON ql.QuoteLineID = j.QuoteLineID

	SET @ResultCount = @@ROWCOUNT

	SELECT @ResultCount 'ResultCount'
END
