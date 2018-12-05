/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.10.11
   Description:	Gets profile image for a user from the documents table
   Usage:		SELECT dbo.fnGetUserImage(2)

   Revision History:
		2017.10.11	AR	Initial deployment
   Return Codes:
   ============================================= */
CREATE FUNCTION [dbo].[fnGetUserImage]
(
	@UserID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
    RETURN ( SELECT TOP 1 FolderPath +  '/' +  D.FileNameStored FROM Documents D

		INNER JOIN lkpDocumentTypeS T on T.DocumentTypeID = D.documentTypeID
		INNER JOIN lkpObjectTypes OB on OB.ObjectTypeID = D.objectTypeID
		WHERE D.ObjectTypeID = 32	--User type
		AND D.ObjectID = @UserID
		ORDER BY D.Created DESC)
END