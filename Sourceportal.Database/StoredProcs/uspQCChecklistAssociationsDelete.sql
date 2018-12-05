
/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.08.24
   Description:	Deletes Checklist Associations from mapQCChecklistsJoin tbl using ChecklistID to discern
   Usage: EXEC uspQCChecklistAssociationsDelete @ObjectID = 1 and @ObjectTypeID = 1

   Revision History:
   Return Codes:
   ============================================= */


CREATE PROCEDURE [dbo].[uspQCChecklistAssociationsDelete]
(
	@ChecklistID INT = NULL,
	@ObjectID INT = NULL,
	@ObjectTypeID INT = NULL
)
AS
BEGIN
	IF ISNULL(@ObjectTypeID, 0) = 0 OR ISNULL(@ObjectID, 0) = 0
		RETURN -8

	IF ISNULL(@ChecklistID, 0) = 0
		RETURN -9

	UPDATE [SourcePortal2_DEV].[dbo].mapQCChecklistJoins 
	SET IsDeleted = 1
	WHERE ObjectID = @ObjectID AND ObjectTypeID = @ObjectTypeID AND ChecklistID = @ChecklistID

	IF @@ROWCOUNT = 0
		RETURN -10
END