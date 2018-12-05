/* =============================================
	Author:			Berry, Zhong
	Create date:	2017.08.22
	Description:	Return the number of comments for given ObjectID
	Usage:			SELECT * FROM Comments
   =============================================*/
CREATE FUNCTION [dbo].[fnGetCommentsCount]
(
	@ObjectID INT,
	@CommentTypeID INT
)
RETURNS INT
AS
BEGIN
	RETURN (
		SELECT COUNT(c.CommentID) 
		FROM Comments c
		WHERE c.ObjectID = @ObjectID AND c.CommentTypeID = @CommentTypeID
	)
END
GO
