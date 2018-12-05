CREATE PROCEDURE [dbo].[uspItemListGet]	
	@RowOffset INT = 0,
	@RowLimit INT = 5000,
	@SearchString VARCHAR(50) = NULL	
AS
BEGIN
	SET NOCOUNT ON;
		
	SELECT
		i.ItemID,
		i.PartNumber,
		COUNT(*) OVER() AS 'TotalRows'
	FROM Items i	  
	WHERE i.IsDeleted = 0 AND (i.PartNumber LIKE '%' + ISNULL(@SearchString, '') + '%')
	ORDER BY i.PartNumber
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END

