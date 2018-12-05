/* =============================================
   Author:		Julia Thomas 
   Create date: 2017.08.28
   Description:	Inserts or updates Quote information from the QCQuestions table
   Usage:	
         DECLARE @result int;
	     EXECUTE @result=  uspQCCheckListQuestionSet @QuestionID=31,@ChecklistID=1,@AnswerTypeID = 2, @SortOrder = 9,
	     @QuestionText='Is the QuoteId?',@CanComment=1,@RequiresPicture=1,@PrintOnInspectReport=1,
	     @PrintOnRejectReport=1,
	     @RequiresSignature=1,@UserID=0
	     Print @result
   Return Codes:
			-4 Invalid QuestionID for new version insert
			-5 New version insert failed
			-6 New question insert failed
			-1 UserID is required
   Revision History:
   ============================================= */
 
CREATE PROCEDURE [dbo].[uspQCCheckListQuestionSet]
    @ChecklistID INT= NULL,
	@QuestionID INT = NULL OUTPUT,
	@VersionID INT = NULL OUTPUT,
	@AnswerTypeID INT = NULL,
	@SortOrder INT = NULL,
	@QuestionText NVARCHAR(500) = NULL,
	@QuestionSubText NVARCHAR(500)=NULL,
	@QuestionHelpText NVARCHAR(500) = NULL,
	@CanComment BIT = NULL,
	@RequiresPicture BIT = NULL,
	@PrintOnInspectReport BIT = NULL,
	@PrintOnRejectReport BIT = NULL,
	@RequiresSignature BIT = NULL,
	@UserID INT= NULL,
	@IsDeleted BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	IF ISNULL(@UserID, 0) = 0
		RETURN -1
    
	IF ISNULL(@QuestionID, 0) = 0
		GOTO InsertNewQuestion		
	ELSE
		BEGIN
			IF ISNULL(@VersionID, 0) = 0
				GOTO InsertNewVersion
		END

InsertNewVersion:
	
	DECLARE @NewVersionCount INT
	SET @VersionID = (SELECT COALESCE(MAX(VersionID), 0) + 1 FROM QCQuestions WHERE QuestionID = @QuestionID)
		
	IF ISNULL(@VersionID, 0) = 0
		RETURN -4

	--Create the new version of the Question
	SET IDENTITY_INSERT QCQuestions ON	
	INSERT INTO QCQuestions (ChecklistID,QuestionID,VersionID, AnswerTypeID, SortOrder, QuestionText,QuestionSubText, QuestionHelpText, CanComment, RequiresPicture, PrintOnInspectReport, PrintOnRejectReport, RequiresSignature, CreatedBy)
	VALUES (@ChecklistID,@QuestionID, @VersionID, @AnswerTypeID,@SortOrder, @QuestionText,@QuestionSubText, @QuestionHelpText, @CanComment,@RequiresPicture,@PrintOnInspectReport,@PrintOnRejectReport, @RequiresSignature,@UserID)
	SET @NewVersionCount = @@ROWCOUNT
	SET IDENTITY_INSERT QCQuestions OFF

	IF (@NewVersionCount=0)
		RETURN -5
	GOTO ReturnSelect

	
InsertNewQuestion:
	
	SET @VersionID = 1

	INSERT INTO QCQuestions (ChecklistID, VersionID, AnswerTypeID, SortOrder, QuestionText,QuestionSubText, QuestionHelpText, CanComment, RequiresPicture, PrintOnInspectReport, PrintOnRejectReport, RequiresSignature, CreatedBy)
	VALUES (@ChecklistID, @VersionID, @AnswerTypeID,@SortOrder, @QuestionText,@QuestionSubText, @QuestionHelpText, @CanComment,@RequiresPicture,@PrintOnInspectReport,@PrintOnRejectReport, @RequiresSignature, @UserID)
	
	SET @QuestionID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -6
	GOTO ReturnSelect 

ReturnSelect:
	SELECT @QuestionID 'QuestionID', @VersionID 'VersionID'
END