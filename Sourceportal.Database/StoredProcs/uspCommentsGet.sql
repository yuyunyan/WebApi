/* =============================================
	Author:			  Berry, Zhong
	Create date:	  2017.08.15
-	Description:	  Return list of comments for given ObjectID and ObjectTypeID
	Usage:			  exec uspCommentsGet @ObjectID = 648, @ObjectTypeID = 20
	Return codes:
					  -1 Missing ObjectID or ObjectTypeID
	Revision History: 
	                  2017.08.28 BZ Update ReplyTo, Adding SearchString
  =============================================*/
CREATE PROCEDURE [dbo].[uspCommentsGet]
	@ObjectID INT = NULL,
	@ObjectTypeID INT = NULL,
	@SearchString NVARCHAR(100) = ''
AS
BEGIN
	SET NOCOUNT ON;

	IF (ISNULL(@ObjectID, 0) = 0) OR (ISNULL(@ObjectTypeID, 0) = 0)
		RETURN -1

    -- Insert statements for procedure here
	SELECT
		c.CommentID,
		c.CommentTypeID,
		c.CreatedBy,
		c.Created,
		c.Comment,
		c.ReplyToID,
		ct.TypeName,
		CONCAT(u.FirstName, ' ', u.LastName) As AuthorName,
		CONCAT(ru.FirstName, ' ', ru.LastName) As ReplyToName
	FROM Comments c
	LEFT OUTER JOIN lkpCommentTypes ct ON c.CommentTypeID = ct.CommentTypeID
	LEFT OUTER JOIN Users u ON c.CreatedBY = u.UserID
	LEFT OUTER JOIN Comments rc ON c.ReplyToID = rc.CommentID
	LEFT OUTER JOIN Users ru ON rc.CreatedBY = ru.UserID
	WHERE c.ObjectID = @ObjectID 
		AND ct.ObjectTypeID = @ObjectTypeID
		AND ((ISNULL(c.Comment, '') + ISNULL(u.FirstName, '') + ISNULL(u.LastName, '') +
			  ISNULL(ru.FirstName, '') + ISNULL(ru.LastName, '') LIKE '%' + ISNULL(@SearchString, '') + '%')
			 OR (EXISTS  (SELECT 1 
						  FROM Comments subc 
				          WHERE subc.ReplyToID = c.CommentID 
						    AND (ISNULL(subc.Comment, '') LIKE '%' + ISNULL(@SearchString, '') + '%')
						  )
				 )
			)
	ORDER BY 
		c.Created
END
