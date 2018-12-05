CREATE TABLE [dbo].[lkpItemStatuses]
(
	[ItemStatusID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StatusName] NVARCHAR(50) NOT NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
