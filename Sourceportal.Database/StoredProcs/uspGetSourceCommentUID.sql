/* =============================================
   Author:		  Berry, Zhong
   Create date:   2017.08.25
   Description:	  Return CommentUID of relationship for given Object and Source
   Usage:         
                  EXEC uspGetSourceCommentUID @ObjectID = 1, @ObjectTypeID = 17, @SourceID = 452
   Return Codes:  
                  -11 ObjectID and ObjectTypeID must be provided
				  -12 SourceID must be provided
   =============================================*/
CREATE PROCEDURE [dbo].[uspGetSourceCommentUID]
	@ObjectID INT = NULL,
	@ObjectTypeID INT = NULL,
	@SourceID INT = NULL
AS
BEGIN
	IF ISNULL(@ObjectID, 0) =  0 OR ISNULL(@ObjectTypeID, 0) = 0
		RETURN -11

	IF ISNULL(@SourceID, 0) = 0
		RETURN -12

	SELECT
		sj.CommentUID
	FROM mapSourcesJoin sj
	WHERE sj.ObjectID = @ObjectID
		AND sj.ObjectTypeID = @ObjectTypeID
		AND sj.SourceID = @SourceID
END
