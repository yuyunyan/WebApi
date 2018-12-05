/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.25
   Description:	Inserts conclusion comment data for an inspectionId
   Usage:		EXEC uspQCConclusionSet 7, 'intro', 'results', 'conclusion'
   Revision History:
      Julia Thomas: Check if objectID EXISTS, Added update Comments tbl if exsit 
	  Julia Thomas: Removed QtyFailed, Introduction and TestResults
	  Julia Thomas: update Inspecttion tbl CompletedDate and CompleteBy 
     Return Codes:
			-3 Inspection conclusion inserted failed
			-4 Inspection conclusion updated failed
   ============================================= */
ALTER PROCEDURE [dbo].[uspQCConclusionSet]
(
	@InspectionID INT= NULL,
	--@QtyFailed INT = 0,
	--@Introduction VARCHAR(MAX),
	--@TestResults VARCHAR(MAX),
	@Conclusion VARCHAR(MAX),
	@CreatedBy INT = NULL
)
AS
BEGIN
SET NOCOUNT ON;
  IF NOT EXISTS (SELECT ObjectID FROM Comments WHERE ObjectID=@InspectionID AND CommentTypeID=3)
    INSERT INTO Comments (CommentTypeID, ObjectID, Comment, CreatedBy)
	VALUES
	--(1, @InspectionID, @Introduction, @CreatedBy),	--Introduction Type
	--(2, @InspectionID, @TestResults, @CreatedBy),	--Test Results Type
	(3, @InspectionID, @Conclusion, @CreatedBy)		--Conclusion Type
  ELSE 
    UPDATE Comments
	SET Comment= @Conclusion
	WHERE ObjectID = @InspectionID AND CommentTypeID=3
  
	IF (@@rowcount = 0)
		SELECT -3

	--UPDATE QCInspections
	--SET CompletedDate= GETUTCDATE(), CompletedBy= @CreatedBy
	--WHERE InspectionID = @InspectionID

	--IF (@@rowcount = 0)
	--	SELECT -4
	SELECT 0
END