/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.18
   Description:	Inserts/updates record in Documents table
   Usage: EXEC [uspDocumentSet] @ObjectTypeID = 104, @ObjectID = 4, @DocName = 'Image', @FileNameOriginal = 'Doc1.jpg', @FileNameStored = '41A81679-1DAA-483A-A29B-F6B0D9B6FF1D.jpg', @FileMimeType = 'image/jpeg'
		  SELECT NEW GUID()
   Revision History:
	   2017.11.28	AR	Added support for documentTypeID, and MimeType check
	   2018.03.27	AR	Added @FolderPath parameter
	   2018.05.10	CT	Added @ExternalId
	   2018.05.10	AR	Added case statement to UPDATE/DocumentTypeID

   Return Codes:
			-1	ObjectTypeID Missing
			-2	ObjectID Missing
			-3	FileNameStored Missing
			-4	Invalid DocumentID
   ============================================= */

CREATE PROCEDURE [dbo].[uspDocumentSet]
	@DocumentID INT = NULL OUTPUT,
	@ObjectTypeID INT = NULL,
	@ObjectID INT = NULL,
	@ExternalID VARCHAR(64) = NULL,
	@DocName VARCHAR(64) = NULL,
	@FileNameOriginal VARCHAR(128) = NULL,
	@FileNameStored VARCHAR(128) = NULL,
	@FileMimeType VARCHAR(256) = NULL,
	@FolderPath VARCHAR(255) = NULL,
	@IsDeleted BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
FileTypeCheck:
	DECLARE @DocumentTypeID INT = NULL
	SET @DocumentTypeID = dbo.fnGetDocumentTypeID(@FileMimeType)
UpdateInsertHandle:
		IF (ISNULL(@DocumentID, 0) = 0)
			GOTO InsertDocument
		ELSE
			GOTO UpdateDocument

InsertDocument:
	INSERT INTO Documents (ObjectTypeID, ObjectID, DocName, FileNameOriginal, FileNameStored, FileMimeType, FolderPath, DocumentTypeID)
	VALUES(@ObjectTypeID, @ObjectID, @DocName, @FileNameOriginal, @FileNameStored, @FileMimeType, @FolderPath, @DocumentTypeID)

	IF (@@rowcount = 0)
	BEGIN
		IF (ISNULL(@ObjectTypeID,0) = 0)
			RETURN -1
		IF (ISNULL(@ObjectID,0) = 0)
			RETURN -2
		IF (ISNULL(@FileNameStored,0) = 0)
			RETURN -3
	END

	RETURN 0

UpdateDocument:
	UPDATE Documents
		SET DocName = ISNULL(@Docname,DocName),
			ObjectID = ISNULL(@ObjectID, ObjectID),
			ExternalID = ISNULL(@ExternalID, ExternalID),
			FileNameOriginal = ISNULL(@FileNameOriginal, FileNameOriginal),
			FileNameStored = ISNULL(@FileNameStored, FileNameStored),
			FolderPath = ISNULL(@FolderPath, FolderPath),
			IsDeleted = ISNULL(@IsDeleted, IsDeleted),
			DocumentTypeID = CASE WHEN ISNULL(@FileMimeType,'') = '' THEN DocumentTypeID ELSE dbo.fnGetDocumentTypeID(ISNULL(@FileMimeType,FileMimeType)) END
		WHERE DocumentID = @DocumentID

	SET @DocumentID = @@identity
	IF (@DocumentID = 0)
		RETURN -4
	
	RETURN 0

END