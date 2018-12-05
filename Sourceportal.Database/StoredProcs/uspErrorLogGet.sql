/* =============================================
   Author:				Berry, Zhong
   Create date:			2017.10.09
   Description:			Gets the error logs list
   Usage:				EXEC uspErrorLogGet @AppID = 2
   =============================================*/
CREATE PROCEDURE [dbo].[uspErrorLogGet]
	@AppID INT = NULL,
	@SearchString NVARCHAR(100) = '',
	@RowLimit INT = 50,
	@RowOffset INT = 0,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@DateStart DATETIME = '1974-01-01 00:00:00',
	@DateEnd DATETIME = '1974-01-01 :00:00:00'
AS
BEGIN
	SET NOCOUNT ON;

    SELECT
		erl.ErrorID,
		[URL],
		erl.PostData,
		erl.ExceptionType,
		erl.ErrorMessage,
		app.AppName 'Application',
		usr.FirstName,
		usr.LastName,
		[Timestamp],
		COUNT(*) OVER() AS 'TotalRows'
	FROM ErrorLog erl
		LEFT OUTER JOIN Applications AS app ON app.AppID = erl.AppID
		LEFT OUTER JOIN Users AS usr ON usr.UserID = erl.UserID
	WHERE erl.AppID = ISNULL(@AppID, erl.AppID) AND
		([Timestamp] BETWEEN @DateStart AND @DateEnd) AND
		(CAST(ErrorID AS NVARCHAR(16)) + ISNULL(erl.ErrorMessage, '') + ISNULL([URL], '') + ISNULL(usr.FirstName, '') + ISNULL(usr.LastName, '') LIKE '%' + ISNULL(@SearchString,'') + '%')
	ORDER BY
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN ISNULL(@SortBy, '') = '' THEN erl.ErrorID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'URL' THEN [URL]
				WHEN @SortBy = 'ErrorMessage' THEN erl.ErrorMessage
				WHEN @SortBy = 'User' THEN usr.FirstName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'ExceptionType' THEN erl.ExceptionType
				WHEN @SortBy = 'Application' THEN erl.AppID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'TimeStamp' THEN [TimeStamp]
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN erl.ErrorID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'URL' THEN [URL]
				WHEN @SortBy = 'ErrorMessage' THEN erl.ErrorMessage
				WHEN @SortBy = 'User' THEN usr.FirstName
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'ExceptionType' THEN erl.ExceptionType
				WHEN @SortBy = 'Application' THEN erl.AppID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'TimeStamp' THEN [TimeStamp]
			END
		END DESC
		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY
END