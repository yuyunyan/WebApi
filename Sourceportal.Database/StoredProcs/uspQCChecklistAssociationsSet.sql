
/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.08.24
   Description:	Deletes Checklist Associations from mapQCChecklistsJoin tbl using ChecklistID to discern
   Usage: EXEC uspQCChecklistAssociationsSet @ChecklistID = 1, @ObjectID = 1, @ObjectTypeID = 1

   Revision History:
   2017.11.30	AR	Added EXISTS check to avoid duplicate record error and GOTO ReturnSelect, changed -7 return code to -1
   Return Codes:
				-1	New checklist association insert failed
   ============================================= */


CREATE PROCEDURE [dbo].[uspQCChecklistAssociationsSet]
(
	@ChecklistID INT = NULL,
	@ObjectID INT = NULL,
	@ObjectTypeID INT = NULL,
	@CreatedBy INT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS (SELECT ChecklistID FROM dbo.mapQCChecklistJoins
				WHERE ChecklistID = @CheckListID
				AND ObjectID = @ObjectID
				AND ObjectTypeID = @ObjectTypeID
				)
		GOTO ReturnSelect
	ELSE BEGIN
			 INSERT INTO dbo.mapQCChecklistJoins (ChecklistID, ObjectTypeID, ObjectID, CreatedBy)
			 VALUES(@ChecklistID, @ObjectTypeID, @ObjectID, @CreatedBy)
	  
			 IF (@@rowcount = 0)

				RETURN -1

		 
			 GOTO ReturnSelect
		 END
ReturnSelect:
	SELECT @ChecklistID 'ChecklistID'
END