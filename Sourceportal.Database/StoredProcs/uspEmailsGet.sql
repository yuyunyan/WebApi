/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.09.04
   Description:	Gets a list of all e-mail messages
   Usage:	EXEC uspEmailsGet

   Return Codes:
   Revision History:

   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspEmailsGet]	
	@SearchString NVARCHAR(100) = NULL,	
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0
AS
BEGIN

	WITH Main_CTE AS (
		SELECT	e.EmailID,
				e.EmailTypeID,
				t.TypeName, 
				e.FromAddress,
				e.ToAddresses,
				e.CCAddresses,
				e.BCCAddresses,
				e.Body,
				e.SentStatus,
				e.ErrorMessage,
				e.CreatedBy,
				ISNULL(u.FirstName, 'System') + ' ' + ISNULL(u.LastName, '') 'CreatedByName',
				e.Created,
				e. Modified 'Sent'
		FROM Emails e
		INNER JOIN lkpEmailTypes t ON e.EmailTypeID = t.EmailTypeID
		LEFT OUTER JOIN Users u ON e.CreatedBy = u.UserID
		WHERE e.IsDeleted = 0
		AND (@SearchString IS NULL OR ISNULL(u.FirstName, 'System') + ISNULL(u.LastName, '') + t.TypeName + e.ToAddresses + e.SentStatus LIKE '%' + @SearchString + '%' )
	),
	Count_CTE AS (
		SELECT COUNT(*) 'TotalRows'
		FROM Main_CTE
	)

	SELECT * FROM Main_CTE m, Count_CTE
	ORDER BY
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN m.EmailID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'TypeName' THEN m.TypeName
				WHEN @SortBy = 'SentStatus' THEN m.SentStatus
				WHEN @SortBy = 'CreatedByName' THEN m.CreatedByName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN	
			CASE 
				WHEN @SortBy = 'Created' THEN m.Created
				WHEN @SortBy = 'Sent' THEN m.[Sent]
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN m.EmailID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'TypeName' THEN m.TypeName
				WHEN @SortBy = 'SentStatus' THEN m.SentStatus
				WHEN @SortBy = 'CreatedByName' THEN m.CreatedByName
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN	
			CASE 
				WHEN @SortBy = 'Created' THEN m.Created
				WHEN @SortBy = 'Sent' THEN m.[Sent]
			END
		END DESC
		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY
END

