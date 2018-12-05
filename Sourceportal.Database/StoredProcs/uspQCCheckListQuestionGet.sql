/* =============================================
   Author:		Julia Thomas 
   Create date: 2017.08.25
   Description:	Gets list of questions belonging to a QC checklist 
   Usage: EXEC uspQCCheckListQuestionGet @ChecklistID=1

   Revision History:
	   
   Return Codes:
   ============================================= */
 

CREATE PROCEDURE [dbo].[uspQCCheckListQuestionGet]
	@ChecklistID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;	
	SELECT
		q.ChecklistID,
		q.QuestionID,
		q.VersionID,
		q.AnswerTypeID,
		t.TypeName AS 'AnswerTypeName',
		q.QuestionText,
		q.QuestionSubText,
		q.QuestionHelpText,
		q.SortOrder,
		q.ShowQtyFailed,
		q.CanComment,
		q.RequiresPicture,
		q.RequiresSignature,
		q.PrintOnInspectReport,
		q.PrintOnRejectReport,
		q.IsDeleted,
		COUNT(*) OVER() AS 'TotalRows'
	FROM vwQCQuestions q  INNER JOIN lkpQCAnswerTypes t on q.AnswerTypeID = t.AnswerTypeID
	WHERE q.ChecklistID = ISNULL(@ChecklistID, ChecklistID)  AND q.IsDeleted = 0
END