/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.11.07
   Description: Updates isDeleted value of question record
   Usage:	
        
   Return Codes:
			-1	Record does not exist
   Revision History:
   ============================================= */
 
CREATE PROCEDURE [dbo].[uspQCCheckListQuestionDeleteSet]
    @QuestionID INT= NULL
AS
BEGIN
	UPDATE QCQuestions
	SET IsDeleted = 1
	WHERE QuestionID = @QuestionID

	IF @@rowcount = 0
		RETURN -1
END