/*	=============================================
	Author:			Berry, Zhong
	Create date:	2018.08.17
	Description:	Insert new comment into Comments table
	Usage:
	Return Codes:
				-6 @UserID is required
				-7 @ObjectID is required
				-3 @ObjectTypeID is required
				-4 @CommentTypeID is required if multiple CommentTypes match with given @ObjectTypeID
				-5 @ObjectTypeID is invalid

	Revision History:
		2018.06.27  changed @Comment NVARCHAR(250) to NVARCHAR(MAX)
		2018.03.09	AR	Implemented CommentTypeID from parent if reply
	============================================*/
CREATE PROCEDURE [dbo].[uspCommentSet]
	@CommentID INT = NULL,
	@ObjectID INT = NULL,
	@ObjectTypeID INT = NULL,
	@CommentTypeID INT = NULL,
	@ReplyToID INT = NULL,
	@Comment NVARCHAR(MAX) = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@UserID, 0) = 0
		RETURN -6
    
	IF ISNULL(@ObjectID, 0) = 0
		RETURN -7

	IF ISNULL(@ObjectTypeID, 0) = 0
		RETURN -3

	--Get Parent CommentTypeID for replies
	IF (ISNULL(@ReplyToID, 0) != 0)
		SET @CommentTypeID = ( SELECT CommentTypeID FROM Comments WHERE CommentID = @ReplyToID )

	DECLARE @CountCommentTypeForObjectType INT = (SELECT COUNT(CommentTypeID) FROM lkpCommentTypes WHERE ObjectTypeID = @ObjectTypeID)
	IF @CountCommentTypeForObjectType > 1
		GOTO InsertWithCommentTypeId
	ELSE 
		IF @CountCommentTypeForObjectType = 1
			GOTO InsertWithObjectTypeId
		ELSE
			RETURN -5

InsertWithCommentTypeID:
	IF ISNULL(@CommentTypeID, 0) = 0
		RETURN -4

	INSERT INTO Comments (CommentTypeID, ObjectID, ReplyToID, Comment, CreatedBy, IsDeleted)
	VALUES (@CommentTypeID, @ObjectID, @ReplyToID, @Comment, @UserID, @IsDeleted)

	SET @CommentID = SCOPE_IDENTITY()

	GOTO ReturnSelect

InsertWithObjectTypeID:
	SET @CommentTypeID = (SELECT CommentTypeID FROM lkpCommentTypes WHERE ObjectTypeID = @ObjectTypeID)

	INSERT INTO Comments (CommentTypeID, ObjectID, ReplyToID, Comment, CreatedBy, IsDeleted)
	VALUES (@CommentTypeID, @ObjectID, @ReplyToID, @Comment, @UserID, @IsDeleted)

	SET @CommentID = SCOPE_IDENTITY()

	GOTO ReturnSelect

ReturnSelect:
	SELECT 
		c.CommentID,
		c.CommentTypeID,
		c.CreatedBy,
		c.Created,
		c.Comment,
		c.ReplyToID,
		ct.TypeName,
		CONCAT(u.FirstName, ' ', u.LastName) As AuthorName,
		CONCAT(r.FirstName, ' ', r.LastName) As ReplyToName
	FROM Comments c
	LEFT OUTER JOIN lkpCommentTypes ct ON c.CommentTypeID = ct.CommentTypeID
	LEFT OUTER JOIN Users u ON c.CreatedBY = u.UserID
	LEFT OUTER JOIN Users r ON c.ReplyToID = r.UserID
	WHERE c.CommentID = @CommentID
END

