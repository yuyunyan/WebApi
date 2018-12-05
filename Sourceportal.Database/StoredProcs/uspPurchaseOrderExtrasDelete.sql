/* =============================================
   Author:		Berry
   Create date: 2017.08.07
   Description:	Deletes one or many Purchase Order Extras
   Usage:	EXEC uspPurchaseOrderExtrasDelete @POExtrasJSON = '[{"POExtraID":16}, {"POExtraID":88}]', @UserID = 0		
   Return Codes:
			-1 Missing JSON list of Purchase Order Extras to be deleted
			-2 Missing UserID
   Revision History:
			
   ============================================= */
CREATE PROCEDURE [dbo].[uspPurchaseOrderExtrasDelete] 
	@POExtrasJSON VARCHAR(MAX) = NULL,
	@UserID INT = NULL,
	@ResultCount INT = NULL OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@POExtrasJSON, '') = ''
		RETURN -1

	IF ISNULL(@UserID, 0) = 0 
		RETURN -2

	UPDATE poe
	SET IsDeleted = 1,
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	FROM PurchaseOrderExtras poe
	  INNER JOIN OPENJSON(@POExtrasJSON) WITH (POExtraID INT) AS j ON poe.POExtraID = j.POExtraID

	SET @ResultCount = @@ROWCOUNT

	SELECT @ResultCount 'ResultCount'
END

