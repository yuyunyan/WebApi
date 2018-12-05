CREATE TABLE [dbo].[Projects]
(
	[ProjectID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(128) NULL,
    [Created] DATETIME NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0
)
