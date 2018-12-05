/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.14
   Description:	Deletes one or more Sales Order Lines
   Usage:	EXEC uspSalesOrderLinesDelete @SOLinesJSON = '[{"SOLineID":16}, {"SOLineID":88}]', @UserID = 0		
   Return Codes:
			-1 Missing JSON list of Sales Order Lines to be deleted
			-2 Missing UserID
   Revision History:
			
   ============================================= */

   
CREATE PROCEDURE [dbo].[uspSalesOrderLinesDelete]
	@SOLinesJSON VARCHAR(MAX) = NULL,
	@UserID INT = NULL,
	@ResultCount INT = NULL OUTPUT 
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@SOLinesJSON, '') = ''
		RETURN -13

	IF ISNULL(@UserID, 0) = 0
		RETURN -7

	UPDATE sol
	SET IsDeleted = 1,
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	FROM SalesOrderLines sol
	  INNER JOIN OPENJSON(@SOLinesJSON) WITH (SOLineID INT) AS j ON sol.SOLineID = j.SOLineID

	SET @ResultCount = @@ROWCOUNT
	SELECT @ResultCount 'ResultCount'
END
