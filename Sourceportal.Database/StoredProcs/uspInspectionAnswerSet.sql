/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.08.23
   Description:	Update the answer fields of an answer
   Usage: EXEC uspInspectionAnswerSet
   Return Codes:
		-1	Answer ID required
		-2	Update Failed, check AnswerId
   ============================================= */
CREATE PROCEDURE [dbo].[uspInspectionAnswerSet]
(
	@AnswerID INT = NULL OUTPUT
	, @Answer VARCHAR(50) = NULL
	, @QtyFailed INT = NULL
	, @Note nVARCHAR(500)  = NULL
	, @CompletedDate DATETIME = NULL
	, @CompletedBy INT = NULL
)
AS
BEGIN 
	SET NOCOUNT ON;
	IF (ISNULL(@AnswerID, 0) = 0)
		RETURN -1;
	
	UPDATE QCAnswers
	SET Answer = @Answer
		, QtyFailed = @QtyFailed
		, Note = @Note
		, CompletedBy= @CompletedBy
		, CompletedDate = @CompletedDate
	WHERE AnswerID = @AnswerID

		IF (@@rowcount = 0)
			RETURN -2
END
