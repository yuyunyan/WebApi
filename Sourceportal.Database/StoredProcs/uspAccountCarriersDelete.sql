
/* =============================================
   Author:		Julia Thomas
   Create date: 2018.05.23
   Description:	Deletes Account Carriers  from mapAccountCarriers
   Usage: EXEC [uspAccountCarriersDelete] @AccountID = 4,@CarrierID=5

   Revision History:
   Return Codes:
   ============================================= */


 CREATE PROCEDURE [dbo].[uspAccountCarriersDelete]
(
	@AccountID INT = NULL,
	@CarrierID INT = NULL,
	@UserID INT = NULL
)
AS
BEGIN

	UPDATE [SourcePortal2_DEV].[dbo].mapAccountCarriers
	SET IsDeleted = 1,
	Modified = GETUTCDATE(),
	ModifiedBy = @UserID 
	WHERE CarrierID = @CarrierID AND AccountID=@AccountID
	SELECT CASE WHEN @@ROWCOUNT > 0 THEN 'true' ELSE 'false' END
END