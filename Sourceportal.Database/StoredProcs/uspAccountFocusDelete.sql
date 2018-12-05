
/* =============================================
   Author:		Remya Varriem
   Create date: 2018.01.04
   Description:	Deletes AccountFocus row from mapAccountFocuses
   Usage: EXEC uspAccountFocusDelete @FocusID = 1

   Revision History:
   Return Codes:
   ============================================= */


CREATE PROCEDURE [dbo].[uspAccountFocusDelete]
(
	@FocusID INT = NULL,
	@UserID INT = NULL
)
AS
BEGIN

	IF ISNULL(@FocusID, 0) = 0
		RETURN -9

	UPDATE [SourcePortal2_DEV].[dbo].mapAccountFocuses
	SET IsDeleted = 1,
	Modified = GETUTCDATE(),
	ModifiedBy = @UserID 
	WHERE FocusID = @FocusID

	IF @@ROWCOUNT = 0
		RETURN -10

	SELECT @@ROWCOUNT 'RowCount'
END
GO
