/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.22
   Description:	Gets records from Documents table
   Usage: EXEC [uspDocumentGet] @ObjectTypeID = 105, @ObjectID = 1
   Revision History:

   Return Codes:
			-1	No records found
   ============================================= */

CREATE PROCEDURE [dbo].[uspDocumentGet_OLD]
	@DocumentID INT = NULL,
	@ObjectTypeID INT = NULL,
	@ObjectID INT = NULL

AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		DocumentID,
		ObjectTypeID,
		ObjectID,
		DocName,
		FileNameOriginal,
		FileNameStored,
		FileMimeType
	FROM Documents
	WHERE ObjectTypeID = ISNULL(@ObjectTypeID, ObjectTypeID)
	AND DocumentID = ISNULL(@DocumentID, DocumentID)
	AND ObjectID = ISNULL(@ObjectID, ObjectID)
	AND IsDeleted = 0

	IF (@@rowcount = 0)
	BEGIN
		RETURN -1
	END


END