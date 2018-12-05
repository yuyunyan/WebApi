/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.08.21
   Description:	soft Delete document
   Usage:		EXEC uspDocumentDelete
   Revision History:

     Return Codes:
			-6 Missing DocumentID
   ============================================= */
CREATE PROCEDURE [dbo].[uspDocumentDelete]
(
	@DocumentID INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@DocumentID, 0) = 0
		RETURN -6

	UPDATE Documents
	SET IsDeleted = 1
	WHERE DocumentID = @DocumentID
END