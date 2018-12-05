CREATE TABLE [dbo].[QCAnswers]
(
	[AnswerID] INT IDENTITY PRIMARY KEY,
	[InspectionID] INT NOT NULL , 
    [QuestionID] INT NOT NULL, 
    [QuestionVersionID] INT NOT NULL, 
    [Answer] VARCHAR(50) NULL, 
    [QtyFailed] INT NULL, 
    [Note] NVARCHAR(500) NULL, 
    [CompletedDate] DATETIME NULL, 
    [CompletedBy] INT NULL, 
    [Created] DATETIME DEFAULT GETUTCDATE() NOT NULL, 
	[CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
