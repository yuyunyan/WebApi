/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.08.22
   Description:	Retrieves questions for a checklist
   Usage:		EXEC uspQCQuestionsGet 6

   Return Codes:
				
   Revision History:
	2017.08.28	ML	Compare the version ID when inner joinng to Question to avoid duplicate rows
	2018.03.19	AFR	Added RequiresPicture/ShowQtyFailed column
   ============================================= */

CREATE PROCEDURE [dbo].[uspQCQuestionsGet]
	@ChecklistID INT = NULL,
	@InspectionID INT = NULL
AS
BEGIN

	
	SELECT QUES.QuestionID
		, ANS.AnswerID
		, QUES.SortOrder
		, QUES.QuestionText
		, QUES.QuestionSubText
		, ANS.Answer
		, QUES.AnswerTypeID
		, ANS.QtyFailed
		, QUES.ShowQtyFailed
		, QUES.CanComment
		, ANS.Note
		, ANS.CompletedDate
		, QUES.RequiresPicture
		, doc.docCount AS ImageCount 		
	FROM QCAnswers ANS
	INNER JOIN QCQuestions QUES  ON ANS.QuestionID = QUES.QuestionID AND ANS.QuestionVersionID = QUES.VersionID 
	LEFT OUTER JOIN ( SELECT ObjectId, COUNT(*) AS docCount
						FROM documents
						WHERE objectTypeId = 105 AND isDeleted = 0
						GROUP BY ObjectId ) doc ON doc.ObjectId = ANS.AnswerId 
	WHERE QUES.ChecklistID = @ChecklistID 
	AND ANS.InspectionID = @InspectionID 

	IF (@@rowcount = 0)
		RETURN -1
END

