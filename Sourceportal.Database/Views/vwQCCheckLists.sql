CREATE VIEW [dbo].[vwQCCheckLists] AS 
	SELECT ChKL.ChecklistID, ChKL.ChecklistName, ANS.InspectionID,ANS.AddedByUser
	FROM QCAnswers ANS 
	INNER JOIN QCQuestions QUES ON ANS.QuestionID = QUES.QuestionID
	INNER JOIN QCChecklists ChKL ON QUES.ChecklistID = ChKL.ChecklistID
	
	GROUP BY ChKL.ChecklistID, ChKL.ChecklistName, ANS.InspectionID,ANS.AddedByUser

	


GO