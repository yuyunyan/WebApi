/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.26
   Description:	Gets details for one or more items
   Usage: EXEC uspManufacturersGet @MfrID = 3
   Revision History:
		2017.07.10	NA	Implemented text search, pagination and remote sorting
   Return Codes:
   ============================================= */


CREATE PROCEDURE [dbo].[uspManufacturersGet]
	@MfrID INT = NULL,
	@SearchText VARCHAR(256) = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		MfrID,
		MfrName,
		Code,
		MfrURL,
		COUNT(*) OVER() AS 'TotalRows'
	FROM Manufacturers
	WHERE IsDeleted = 0
	  AND MfrID = ISNULL(@MfrID, MfrID)
	  AND (ISNULL(MfrName, '') + ISNULL(Code, '')) LIKE '%' + ISNULL(@SearchText,'') + '%'
	ORDER BY 
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN MfrID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'MfrName' THEN MfrName
				WHEN @SortBy = 'Code' THEN Code
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN MfrID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'MfrName' THEN MfrName
				WHEN @SortBy = 'Code' THEN Code
			END
		END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END