CREATE TABLE [dbo].[ErrorLog]
(
	[ErrorID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AppID] INT NOT NULL, 
    [URL] VARCHAR(MAX) NULL, 
    [PostData] VARCHAR(MAX) NULL, 
    [ExceptionType] VARCHAR(100) NULL, 
    [ErrorMessage] VARCHAR(MAX) NULL, 
    [InnerExceptionMessage] VARCHAR(MAX) NULL, 
    [StackTrace] VARCHAR(MAX) NULL, 
    [UserID] INT NULL, 
    [TimeStamp] DATETIME NOT NULL DEFAULT GETUTCDATE()
)
