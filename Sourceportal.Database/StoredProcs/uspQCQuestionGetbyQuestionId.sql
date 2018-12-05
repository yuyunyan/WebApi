/* =============================================
   Author:		Julia Thomas 
   Create date: 2017.08.25
   Description:	Gets question's detail by questionId
   Usage: EXEC uspQCQuestionGetbyQuestionId @QuestionID=38

   Revision History:
	   
   Return Codes:
   ============================================= */


CREATE PROCEDURE [dbo].[uspQCQuestionGetbyQuestionId]
	@QuestionID INT = NULL
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
		q.IsDeleted
	FROM vwQCQuestions q  INNER JOIN lkpQCAnswerTypes t on q.AnswerTypeID = t.AnswerTypeID
	WHERE q.QuestionID = ISNULL(@QuestionID, QuestionID)  AND q.IsDeleted = 0
END