/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.08.21
   Description:	Retrieves document for a given objectype id and object id and bitwise DocumentTypeID
				(value stored should NOT be bitwise, only used for checking)
   Usage:		EXEC uspDocumentsGet 104,4
   Revision History:
   2017.11.20	AR	Added Created column to proc/table
   2017.11.27	AR	Added support for  bitwise documentType 
   2017.12.08	AR	Implemented sort column/order logic
   2018.05.02   ML  Added IsSystem Parameter so that it returns either system files or non system files
   2018.05.30	AR	Added FullPath column, Added @FilterByOddOrEven logic
     Return Codes:
			-5 Missing ObjectTypeId or ObjectID
   ============================================= */
CREATE PROCEDURE [dbo].[uspDocumentsGet]
(
	@ObjectTypeID INT = NULL,
	@ObjectID INT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 500,
	@DocumentTypeID INT= NULL,
	@SortBy VARCHAR(64) = NULL,
	@DescSort BIT = 0,
	@IsSystem BIT =0,
	@FilePathPrefix VARCHAR(500) = NULL,
	@FilterByOddOrEven VARCHAR(50) = NULL
)
AS
BEGIN
	SET NOCOUNT ON;

	IF (ISNULL(@ObjectID, 0) = 0) OR (ISNULL(@ObjectTypeID, 0) = 0)
		RETURN -5

	--Check if this is not an Inspection Type AND if images are filtered out. If so, add images to returned documentTypes. Images would be filtered out if not
	IF (@ObjectTypeID != 104 AND @DocumentTypeID & 1 != 1)
		SET @DocumentTypeID = @DocumentTypeID + 1
	SELECT * FROM
	(
		SELECT
			DocumentID,
			ObjectTypeID,
			ObjectID,
			DocName,
			FileNameOriginal,
			FileNameStored,
			FolderPath,
			ISNULL(NULLIF(@FilePathPrefix,'') + '/Documents/','') + FolderPath  + '/' + FileNameStored FullPath,
			FileMimeType,
			Created,
			ExternalID,
			T.DocumentType,
			ROW_NUMBER() OVER(Order By DocumentID) Rownumber,
			COUNT(*) OVER() AS 'TotalRows'
		FROM Documents D 
		INNER JOIN lkpDocumentTypes T on T.DocumentTypeID = D.DocumentTypeID
		WHERE ObjectTypeID = @ObjectTypeID 
		AND ObjectID = ISNULL(NULLIF(@ObjectID,0), d.ObjectID)
		AND D.DocumentTypeID & ISNULL(@DocumentTypeID, D.DocumentTypeID) = D.DocumentTypeID
		AND D.IsSystem = @IsSystem
		AND IsDeleted = 0
		ORDER BY
			CASE WHEN @DescSort = 0 THEN
				CASE 
					WHEN ISNULL(@SortBy, '') = '' THEN D.DocumentID
				END
			END ASC,
			CASE WHEN @DescSort = 0 THEN
				CASE 
					WHEN @SortBy = 'DocName' THEN D.DocName
					WHEN @SortBy = 'FileNameOriginal' THEN D.FileNameOriginal
					WHEN @SortBy = 'FileNameStored' THEN D.FileNameStored
					WHEN @SortBy = 'FileMimeType' THEN D.FileMimeType
				END
			END ASC,
			CASE WHEN @DescSort = 0 THEN
				CASE 
					WHEN @SortBy = 'Created' THEN D.Created
				END
			END ASC,
			CASE WHEN @DescSort = 1 THEN
				CASE 
					WHEN ISNULL(@SortBy, '') = '' THEN D.DocumentID
				END
			END DESC,
			CASE WHEN @DescSort = 1 THEN
				CASE 
					WHEN @SortBy = 'DocName' THEN D.DocName
					WHEN @SortBy = 'FileNameOriginal' THEN D.FileNameOriginal
					WHEN @SortBy = 'FileNameStored' THEN D.FileNameStored
					WHEN @SortBy = 'FileMimeType' THEN D.FileMimeType
				END
			END DESC,
			CASE WHEN @DescSort = 1 THEN
				CASE 
					WHEN @SortBy = 'Created' THEN D.Created
				END
			END DESC

		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY
	) tbl
	WHERE  RowNumber % 2 = CASE WHEN @FilterByOddOrEven = 'even' THEN 0 WHEN @FilterByOddOrEven = 'odd' THEN 1 ELSE RowNumber % 2 END
END