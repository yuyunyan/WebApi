CREATE TABLE [dbo].[lkpEmailTypes]
(
	[EmailTypeID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TypeName] VARCHAR(100) NOT NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
