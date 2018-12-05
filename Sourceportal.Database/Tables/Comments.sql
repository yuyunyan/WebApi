CREATE TABLE [dbo].[Comments]
(
	[CommentID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CommentTypeID] INT NOT NULL, 
    [ObjectID] INT NOT NULL, 
    [ReplyToID] INT NULL, 
    [Comment] NVARCHAR(MAX) NULL, 
    [ExternalID] VARCHAR(50) NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
