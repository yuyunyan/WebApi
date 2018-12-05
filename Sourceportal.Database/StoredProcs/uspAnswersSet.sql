/* =============================================
   Author:		Julia Thomas
   Create date: 2018.06.14
   Description:	Inserts a record in QCAnswers tbl
   Usage: 
   Revision History:
		
   Return Codes:
		
   ============================================= */
CREATE PROCEDURE [dbo].[uspAnswersSet]
(     @AnswerID INT = NULL OUTPUT
	, @QuestionID INT = NULL
	, @QuestionVersionID INT NULL
	, @InspectionID INT = NULL
	, @CreatedBy INT = NULL
)
AS
BEGIN 
	INSERT INTO QCAnswers(InspectionID,QuestionID,QuestionVersionID,CreatedBy,AddedByUser)
	VALUES (
	         @InspectionID
	       , @QuestionID
		   , @QuestionVersionID  
	       , @CreatedBy
		   ,1
		 )
	SET @AnswerID = @@IDENTITY 
    SELECT @@ROWCOUNT 'RowCount',1 'AddedByUser' 

END
