/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.05.30
   Description:	Gets question photos for inspection ID
   Usage:		EXEC [uspInspectionReportAnswerPhotosGet] @InspectionID = 8,  @FilePathPrefix = 'http://dev.api.sourceportal.com', @FilterByOddOrEven = 'odd'
   Return Codes:
			
   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspInspectionReportAnswerPhotosGet]
	@InspectionID INT = NULL,
	@FilePathPrefix VARCHAR(500) = NULL,
	@FilterByOddOrEven VARCHAR(50) = NULL
AS
BEGIN
	DECLARE @tmp TABLE (Rownumber INT, RowNumberByChecklist INT, AnswerID INT, QuestionID INT, ChecklistID INT, ChecklistName VARCHAR(500), FullPath VARCHAR(1024), isFirst INT DEFAULT NULL)
	DECLARE @tmp2 TABLE (ID INT IDENTITY(1,1), RowNumberByChecklist INT, QuestionID INT, ChecklistID INT, ChecklistName VARCHAR(500), FullPath VARCHAR(1024), isFirst INT DEFAULT NULL)
	INSERT INTO @tmp
	SELECT * FROM (
		SELECT ROW_NUMBER() OVER (ORDER BY A.AnswerID) RowNumber,
			ROW_NUMBER() OVER (PARTITION BY CHK.ChecklistID order by CHK.ChecklistID, Q.QuestionID) RowNumberByChecklist,
			A.AnswerID,
			Q.QuestionID,
			CHK.ChecklistID,
			CHK.ChecklistName,
			null FullPath,
			null isFirst
			FROM QCAnswers A
			INNER JOIN QCQuestions Q  ON A.QuestionID = Q.QuestionID AND A.QuestionVersionID = Q.VersionID 
			INNER JOIN QCChecklists CHK on CHK.ChecklistID = Q.ChecklistID	
			WHERE A.InspectionID = @InspectionID
			AND Q.PrintOnInspectReport = 1
		) D
	INSERT INTO @tmp2
	SELECT RowNumberByChecklist,
		QuestionID,
		T.ChecklistID,
		ISNULL(Q.ChecklistName,'-1') ChecklistName,
		ISNULL(ISNULL(NULLIF(@FilePathPrefix,'') + '/Documents/','') + FolderPath  + '/' + FileNameStored,'-1') FullPath,
		CASE WHEN tt.ChecklistID > 0 THEN 1 ELSE 0 END isFirst
	FROM @tmp T
	LEFT OUTER JOIN Documents D on D.objectID = t.AnswerID AND D.objectTypeID = 105
	LEFT OUTER JOIN (SELECT DISTINCT ChecklistID, MIN(Rownumber) Rownumber FROM @tmp GROUP BY ChecklistID) tt on tt.Rownumber = T.Rownumber
	LEFT OUTER JOIN QCChecklists Q on Q.ChecklistID = tt.ChecklistID

	WHERE  T.RowNumber % 2 = CASE WHEN @FilterByOddOrEven = 'even' THEN 0 WHEN @FilterByOddOrEven = 'odd' THEN 1 ELSE T.RowNumber % 2 END
	--AND ((Q.ChecklistName IS NOT NULL AND FolderPath IS NULL) OR (Q.ChecklistName IS NULL AND FolderPath IS NOT NULL)) 
	
	DELETE FROM @tmp2 WHERE ChecklistName = '-1' and Fullpath = '-1'
	UPDATE @tmp2 SET isFirst = 1 WHERE ID = (SELECT TOP 1 ID FROM @tmp2 WHERE FullPath != '-1' ORDER BY ID ASC)
	SELECT ID,
		RowNumberByChecklist,
		 QuestionID,
		 t.ChecklistID,
		 Q.ChecklistName,
		 Fullpath,
		 isFirst
	FROM @tmp2 t
	INNER JOIN QCChecklists Q on Q.ChecklistID = t.ChecklistId
	WHERE FullPath != '-1'
END