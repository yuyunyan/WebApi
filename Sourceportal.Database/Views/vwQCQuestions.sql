CREATE VIEW [dbo].[vwQCQuestions] AS 

	WITH q AS
		(
			SELECT *,
				ROW_NUMBER() OVER (PARTITION BY QuestionID ORDER BY VersionID DESC) AS vn
			FROM QCQuestions
			WHERE IsDeleted = 0			  
		)
	SELECT * FROM q WHERE vn = 1