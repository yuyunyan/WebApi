CREATE TABLE [dbo].[lkpSourceTypes]
(
	[SourceTypeID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TypeName] NVARCHAR(50) NOT NULL, 
    [TypeRank] INT NOT NULL, 
    [IsConfirmed] BIT NOT NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL 
)
