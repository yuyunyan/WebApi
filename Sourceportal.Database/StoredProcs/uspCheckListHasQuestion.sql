/* =============================================
   Author:		Julia Thomas 
   Create date: 2018.06.19
   Description:	Retrieves all child checkLists from QCChecklists tbl which has Question
   Usage: EXEC uspCheckListHasQuestion
   Return Codes:
				
   Revision History:
				
   ============================================= */

CREATE PROCEDURE [dbo].[uspCheckListHasQuestion]
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Distinct 
	  qcc.CheckListID,
	  qcc.ChecklistName
	FROM QCChecklists qcc
	INNER JOIN QCQuestions ques ON qcc.ChecklistID= ques.ChecklistID
	WHERE qcc.ParentChecklistID IS NOT NULL and ques.IsDeleted =0
END



