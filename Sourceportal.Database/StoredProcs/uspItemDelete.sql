/* =============================================
   Author:		Hrag Sarkissian
   Create date: 2018.05.31
   Description:	deletes the item by updating the column isDeleted - 1
   Usage: EXEC uspItemDelete
   Revision History:
		2018.09.06	NA	Added ModifiedBy
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspItemDelete]
	@Id INT = NULL,
	@UserID INT = NULL
AS
BEGIN
	UPDATE Items
	SET
		isDeleted = 1,
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	WHERE ItemId = @Id
END