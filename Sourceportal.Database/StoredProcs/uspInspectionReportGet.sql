/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.05.18
   Description:	Retrieves questions/Answers/images for a checklist from inspectionID
   Usage:		EXEC [uspInspectionReportGet] @InspectionID = 8

   Return Codes:

   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspInspectionReportGet]
	@InspectionID INT = NULL,
	@FirstPageRowCount INT = 9,
	@AlternatePageRowCount INT = 19
AS	
BEGIN
	DECLARE @ExternalID VARCHAR(100) = (SELECT ExternalID FROM QCInspections WHERE InspectionID = @InspectionID)
	--Declare tmp table so we can manipulate its data
	DECLARE @tmp TABLE (ChecklistID INT DEFAULT NULL, CreatorID INT DEFAULT NULL, ChecklistName VARCHAR(500) DEFAULT NULL, QuestionID INT DEFAULT NULL, InspectionID INT DEFAULT NULL, AnswerID INT DEFAULT NULL, SortOrder INT DEFAULT NULL, QuestionText VARCHAR(500) DEFAULT NULL, Response VARCHAR(500) DEFAULT NULL, QtyFailed INT DEFAULT NULL, Comment VARCHAR(500) DEFAULT NULL, ImageCount INT DEFAULT NULL, InspectedBy VARCHAR(500) DEFAULT NULL, 
	QuestionSubText VARCHAR(500) DEFAULT NULL, Answer VARCHAR(500) DEFAULT NULL, AnswerTypeID INT DEFAULT NULL, ShowQtyFailed BIT DEFAULT NULL, CanComment BIT DEFAULT NULL, CompletedDate DATETIME DEFAULT NULL, RequiresPicture BIT DEFAULT NULL, RowNumber INT DEFAULT NULL)
	INSERT INTO @tmp
	SELECT CHK.ChecklistID
		, CHK.CreatedBy
		, NULL	--ChecklistName
		, QUES.QuestionID
		, ANS.InspectionID
		, ANS.AnswerID	
		, QUES.SortOrder
		, QUES.QuestionText
		, ANST.TypeName
		, ANS.QtyFailed
		, CASE WHEN QUES.AnswerTypeID IN (6,7) THEN ISNULL(ANS.Note,'') ELSE '' END Comment --6/7 = Comment Only/Text
		, doc.docCount AS ImageCount 
		, dbo.fnGetUserFirstlastName(ANS.CompletedBy) InspectedBy
		, QUES.QuestionSubText
		, ANS.Answer
		, QUES.AnswerTypeID
		, QUES.ShowQtyFailed
		, QUES.CanComment
		, ANS.CompletedDate
		, QUES.RequiresPicture
		, ROW_NUMBER() OVER (order by QUES.QuestionID ASC) RowNumber
	FROM QCAnswers ANS
	INNER JOIN QCQuestions QUES  ON ANS.QuestionID = QUES.QuestionID AND ANS.QuestionVersionID = QUES.VersionID 
	INNER JOIN lkpQCAnswerTypes ANST ON ANST.AnswerTypeID = QUES.AnswerTypeID
	INNER JOIN QCChecklists CHK ON CHK.ChecklistID = QUES.ChecklistID
	LEFT OUTER JOIN ( SELECT ObjectId, COUNT(*) AS docCount
						FROM documents
						WHERE objectTypeId = 105 AND isDeleted = 0
						GROUP BY ObjectId ) doc ON doc.ObjectId = ANS.AnswerId 
	WHERE ANS.InspectionID = @InspectionID
	AND QUES.PrintOnInspectReport = 1
	
	DECLARE @UniqueChecklistList TABLE (ChecklistID INT)
	DECLARE @CurrentChecklistID INT = NULL DECLARE @ChecklistCount INT = 0
	DECLARE @index INT = 1 DECLARE @CurrentQuestionsCount INT = 0 DECLARE @PageRowCount INT = 0
	INSERT INTO @UniqueChecklistList
		SELECT DISTINCT ChecklistID FROM @tmp
		GROUP By ChecklistID
	DELETE @UniqueChecklistList WHERE ChecklistID = (SELECT TOP 1 ChecklistID FROM @UniqueChecklistList) --Remove first from checklist (we dont need spacing from this list)
	SET @ChecklistCount = (SELECT COUNT(*) FROM @UniqueChecklistList)

	----Loop spacing rows per predicted count per page
	--WHILE (@index <= @ChecklistCount)
	--BEGIN
	--	SET @CurrentChecklistID = (SELECT MIN(ChecklistID) FROM @UniqueChecklistList)
	--	SET @CurrentQuestionsCount =  (SELECT COUNT(DISTINCT QuestionID) FROM @tmp where checklistID = @CurrentChecklistID GROUP BY ChecklistID)

	--	--First page, insert 9 empty spaces
	--	IF (@index = 1)
	--		SET @PageRowCount = @FirstPageRowCount	--First page, insert 9 empty spaces
	--	ELSE
	--		SET @PageRowCount = @AlternatePageRowCount	--Second page onward, insert 15 empty spaces

	--	--if extends past first page, treat as second+ page and figure out question count for the last page
	--	IF (@CurrentQuestionsCount > @PageRowCount)
	--	BEGIN
	--		SET @PageRowCount = @AlternatePageRowCount
	--		SET @CurrentQuestionsCount = (@CurrentQuestionsCount - (@PageRowCount * (@CurrentQuestionsCount/@PageRowCount)))
	--	END
	--	WHILE (@CurrentQuestionsCount < @PageRowCount)
	--	BEGIN
	--		INSERT INTO @tmp (ChecklistID, QuestionID)
	--		VALUES (@CurrentChecklistID + .1, 0)
	--		SET @CurrentQuestionsCount = @CurrentQuestionsCount + 1
	--	END
	
	--	--Remove checklist, we are done adding blank rows
	--	DELETE @UniqueChecklistList WHERE ChecklistID = @CurrentChecklistID
	--	SET @index = @index+1
	--END
	
	SELECT ISNULL(t.ChecklistID, 0) ChecklistID
	, CASE WHEN t.CreatorID IS NULL THEN '1' WHEN CHKCreator.CreatorID = 0 THEN 'SAP (No Checklist)' ELSE ISNULL(U.FirstName + ' ' + U.LastName, '-1') END  ChecklistCreator
	, CASE WHEN t.CreatorID IS NULL THEN '2'  ELSE ISNULL(CHK.ChecklistName,'-1') END CheckListName	--First iteration of checklist
	, ISNULL(t.CreatorID,9999999) CreatorID
	, CASE WHEN t.QuestionID = 0 THEN 9999999 ELSE ISNULL(t.QuestionID,9999999) END QuestionID
	, ISNULL(t.AnswerID,9999999) AnswerID
	, ISNULL(t.SortOrder,9999999) SortOrder
	, ISNULL(t.QuestionText,' ') QuestionText
	, ISNULL(t.Response,' ') Response
	, ISNULL(t.QtyFailed,0) QtyFailed
	, ISNULL(t.Comment, ' ') Comment
	, ISNULL(t.ImageCount,0) ImageCount
	, ISNULL(t.InspectedBy,' ') InspectedBy
	, ISNULL(t.QuestionSubText,' ') QuestionSubText
	, ISNULL(t.Answer,' ') Answer
	, ISNULL(t.AnswerTypeID,0) AnswerTypeID
	, ISNULL(t.ShowQtyFailed,0) ShowQtyFailed
	, ISNULL(t.CanComment, 0) CanComment
	, ISNULL(t.CompletedDate, '') CompletedDate
	, ISNULL(t.RequiresPicture, 0) RequiresPicture
	, CONVERT(CHAR(15), CHK.EffectiveStartDate, 106) EffectiveStartDate
	, 'FM-QC-Checklist Rev' + ' ' + @ExternalID ExternalID
	, ROW_NUMBER() OVER (PARTITION BY t.ChecklistID order by t.ChecklistID, QuestionID) RowNumber
	--, ISNULL(t.RowNumber, 0) RowNumber
	, CASE WHEN QuestionID = 0 THEN 'true' ELSE 'false' END IsSpacerRow
	 FROM @tmp t
	LEFT OUTER JOIN (SELECT DISTINCT ChecklistID, MIN(RowNumber) RowNumber
				FROM @tmp
				GROUP BY ChecklistID ) CHKHeader on CHKHeader.RowNumber = t.RowNumber
	LEFT OUTER JOIN QCChecklists CHK on CHK.ChecklistID = CHKHeader.ChecklistID

	LEFT OUTER JOIN (SELECT DISTINCT MAX(CreatorID) CreatorID, Max(RowNumber) RowNumber
				FROM @tmp
				GROUP BY ChecklistID ) CHKCreator on CHKCreator.RowNumber = t.RowNumber
	LEFT OUTER JOIN Users U on CHKCreator.CreatorID = U.UserID

	ORDER BY t.ChecklistID, QuestionID

	IF (@@RowCount = 0)
		RETURN -1
END