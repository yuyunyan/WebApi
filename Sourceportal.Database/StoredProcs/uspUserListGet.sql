/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.05.03
   Description:	Retrieves user list from tblUsers with lazyload rownum (default to 0-25)
   Usage: EXEC uspUserListGet @StartRow = 10

   Revision History:
		2017.05.09	AR	Depreciated Username
		2017.06.06	AR	Changed EndRow default to max
		2018.08.01  Julia Thomas IsEnabled modifed and SearchString added
		2018.08.02  Julia Thomas OrganizationName added  
   ============================================= */
CREATE PROCEDURE [dbo].[uspUserListGet]
(
	@IsEnabled BIT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@SearchString NVARCHAR(100) = ''
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		U.UserID,
		U.EmailAddress,
		U.FirstName,
		U.LastName,
		U.OrganizationID,
		U.isEnabled,
		U.LastLogin,
		U.Created,
		U.CreatedBy,
		U.Modified,
		U.ModifiedBy,
		O.[Name] 'OrganizationName',
		COUNT(*) OVER() AS 'TotalRows'
	FROM Users U
	LEFT OUTER JOIN Organizations O ON U.OrganizationID = O.OrganizationID
	WHERE IsEnabled = ISNULL(@IsEnabled,isEnabled)
	AND (ISNULL(FirstName, '') + ISNULL(LastName, '') +  ISNULL(EmailAddress, '') LIKE '%' + ISNULL(@SearchString,'') + '%')
	ORDER BY 
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN UserID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'LastLogin' THEN LastLogin
				WHEN @SortBy = 'Created' THEN U.Created
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'FirstName' THEN FirstName
				WHEN @SortBy = 'LastName' THEN LastName
				WHEN @SortBy = 'EmailAddress' THEN EmailAddress
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN UserID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'LastLogin' THEN LastLogin
				WHEN @SortBy = 'Created' THEN U.Created
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'FirstName' THEN FirstName
				WHEN @SortBy = 'LastName' THEN LastName
				WHEN @SortBy = 'EmailAddress' THEN EmailAddress
			END
		END DESC
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
END
