/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.28
   Description:	Creates or updates a SourceJoin record
   Usage:		EXEC 
				
   Return Codes:
				-1 @ObjectTypeID, @ObjectID and @Source ID are all required
				-2 Error on new record insert
				-3 Error on record update
				-4 UserID is required
   Revision History:
				2017.07.18  NA  Added check for UserID
				2017.08.25  NA  Removed Note field
				2018.04.20	BZ	Added Qty
   ============================================= */

CREATE PROCEDURE [dbo].[uspSourceJoinSet]
	@ObjectTypeID INT = NULL,
	@ObjectID INT = NULL,
	@SourceID INT = NULL,
	@IsMatch BIT = NULL,	
	@IsDeleted BIT = NULL,
	@Qty INT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (ISNULL(@ObjectTypeID, 0) = 0 OR ISNULL(@ObjectID, 0) = 0 OR ISNULL(@SourceID,0) = 0)
		RETURN -1

	IF @UserID IS NULL
		RETURN -4

	IF EXISTS (SELECT 1 FROM mapSourcesJoin WHERE ObjectTypeID = @ObjectTypeID AND ObjectID = @ObjectID AND SourceID = @SourceID)
		GOTO UpdateJoin
	ELSE
		GOTO InsertJoin

InsertJoin:
	INSERT INTO mapSourcesJoin (ObjectTypeID, ObjectID, SourceID, IsMatch, CreatedBy)
	VALUES (@ObjectTypeID, @ObjectID, @SourceID, @IsMatch, @UserID)

	IF (@@ROWCOUNT=0)
		RETURN -2
	GOTO ReturnSelect

UpdateJoin:	
	UPDATE mapSourcesJoin
	SET
		IsMatch = @IsMatch,
		Qty = ISNULL(@Qty, Qty), 
		IsDeleted = ISNULL(@IsDeleted, 0),  --Updating a previously deleted join without specifying it is still deleted will un-delete it.
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE ObjectTypeID = @ObjectTypeID AND ObjectID = @ObjectID AND SourceID = @SourceID
	
	IF (@@ROWCOUNT=0)
		RETURN -3
	GOTO ReturnSelect

ReturnSelect:
	SELECT	@ObjectTypeID 'ObjectTypeID',
			@ObjectID 'ObjectID',
			@SourceID 'SourceID' 	
END

