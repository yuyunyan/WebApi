/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.25
   Description:	Deletes one or more Sales Order Extras
   Usage:	EXEC uspSalesOrderExtrasDelete @SOExtrasJSON = '[{"SOExtraID":16}, {"SOExtraID":88}]', @UserID = 0		
   Return Codes:
			-14 Missing JSON list of Sales Order Extras to be deleted
			-7 Missing UserID
   Revision History:
			
   ============================================= */

   
CREATE PROCEDURE [dbo].[uspSalesOrderExtrasDelete]
	@SOExtrasJSON VARCHAR(MAX) = NULL,
	@UserID INT = NULL,
	@ResultCount INT = NULL OUTPUT 
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@SOExtrasJSON, '') = ''
		RETURN -14

	IF ISNULL(@UserID, '') = ''
		RETURN -7

	UPDATE soe
	SET IsDeleted = 1,
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	FROM SalesOrderExtras soe
	  INNER JOIN OPENJSON(@SOExtrasJSON) WITH (SOExtraID INT) AS j ON soe.SOExtraID = j.SOExtraID

	SET @ResultCount = @@ROWCOUNT
	SELECT @ResultCount
END
