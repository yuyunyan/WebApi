/* =============================================
   Author:		Julia Thomas
   Create date: 2018.06.15
   Description:	Delete a checkList: Delete all QuestionIds belong to a checklist Id and inspection Id;
   Usage: EXEC [uspAnswersDeleteForCheckList] 

   Revision History:
   Return Codes:
   ============================================= */


CREATE PROCEDURE [dbo].[uspAnswersDeleteForCheckList]
(
	  @QuestionID INT = NULL
	, @InspectionID INT = NULL
)
AS
BEGIN

	DELETE FROM [SourcePortal2_DEV].[dbo].QCAnswers
	WHERE InspectionID = @InspectionID AND QuestionID=@QuestionID
	SELECT  @@ROWCOUNT 'RowCount'
END
