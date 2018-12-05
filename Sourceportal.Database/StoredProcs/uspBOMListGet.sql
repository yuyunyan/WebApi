/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.09.21
   Description:	Retrieves BOM List - (ItemLists)

   Revision History:
   ============================================= */

CREATE PROCEDURE [dbo].[uspBOMListGet]
(
	@SearchString NVARCHAR(100) = '',
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@ObjectTypeID INT = NULL,
	@UserID INT = NULL
)
AS
BEGIN

SET NOCOUNT ON;

	DECLARE @Sec TABLE (ItemListID INT, RoleID INT)
	INSERT @Sec EXECUTE uspItemListSecurityGet @UserID = @UserID;

	WITH Main_CTE AS( SELECT 
	I.ItemListID,
	ListName,
	I.AccountID,
	A.AccountName,
	I.ContactID,
	C.FirstName + ' ' + C.LastName ContactName,
	IL.ItemCount,
	D.FileNameOriginal,
	I.StatusID,
	S.StatusName,
	U.FirstName + ' ' + U.LastName UserName,
	I.Created,
	O.Name OrganizationName,
	ISNULL(cmt.Comments, 0) Comments
	FROM ItemLists I
	OUTER APPLY (SELECT Count(*) AS ItemCount
                    FROM   ItemListLines L
                    WHERE  L.ItemListID = I.ItemListID) IL
	INNER JOIN Accounts A ON I.AccountID = A.AccountID
	INNER JOIN lkpStatuses S ON I.StatusID = S.StatusID
	INNER JOIN Users U ON I.SalesUserID = U.UserID
	INNER JOIN (SELECT DISTINCT ItemListID FROM @Sec) sec ON I.ItemListID = sec.ItemListID
	LEFT JOIN Organizations O ON I.OrganizationID = O.OrganizationID
	LEFT JOIN Contacts C ON I.ContactID = C.ContactID
	LEFT JOIN Documents D on D.ObjectID = I.ItemListID AND D.ObjectTypeID = 109 
	LEFT OUTER JOIN (
		SELECT 
			mc.ObjectID, 
			Count(mc.ObjectID) AS Comments
		FROM Comments mc
			LEFT OUTER JOIN lkpCommentTypes lct ON lct.CommentTypeID = mc.CommentTypeID
		WHERE lct.ObjectTypeID = @ObjectTypeID GROUP BY mc.ObjectID) cmt ON cmt.ObjectID = I.ItemListID
	WHERE (CAST(I.ItemListID AS NVARCHAR(16)) + ISNULL(A.AccountName, '') + ISNULL(C.FirstName, '') + ISNULL(C.LastName, '') + ISNULL(D.FileNameOriginal, '')
		LIKE '%' + ISNULL(@SearchString,'') + '%')
	),
	
	Count_CTE AS (
	SELECT COUNT(*) AS [RowCount]
	FROM Main_CTE
	)

	SELECT *
FROM Main_CTE, Count_CTE
	
	ORDER BY 
		
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_CTE.ItemListID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_CTE.AccountName 
				WHEN @SortBy = 'ContactName' THEN Main_CTE.ContactName
				WHEN @SortBy = 'StatusName' THEN Main_CTE.StatusName 				
				WHEN @SortBy = 'ListName' THEN Main_CTE.ListName 
				WHEN @SortBy = 'FileNameOriginal' THEN Main_CTE.FileNameOriginal 
				WHEN @SortBy = 'UserName' THEN Main_CTE.UserName 
				WHEN @SortBy = 'OrganizationName' THEN Main_CTE.OrganizationName 
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'Created' THEN Main_CTE.Created 
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'ItemListID' THEN Main_CTE.ItemListID 
				WHEN @SortBy = 'ItemCount' THEN Main_CTE.ItemCount 
			END
		END ASC,
		
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_CTE.ItemListID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'AccountName' THEN Main_CTE.AccountName 
				WHEN @SortBy = 'ContactName' THEN Main_CTE.ContactName
				WHEN @SortBy = 'StatusName' THEN Main_CTE.StatusName 				
				WHEN @SortBy = 'ListName' THEN Main_CTE.ListName 
				WHEN @SortBy = 'FileNameOriginal' THEN Main_CTE.FileNameOriginal 
				WHEN @SortBy = 'UserName' THEN Main_CTE.UserName 
				WHEN @SortBy = 'OrganizationName' THEN Main_CTE.OrganizationName 
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'Created' THEN Main_CTE.Created 
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'ItemListID' THEN Main_CTE.ItemListID 
				WHEN @SortBy = 'ItemCount' THEN Main_CTE.ItemCount 
			END
		END DESC

		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY


	IF (@@rowcount = 0)
		RETURN -1

END