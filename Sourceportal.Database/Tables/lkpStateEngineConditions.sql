CREATE TABLE [dbo].[lkpStateEngineConditions]
(
	[ConditionID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ObjectTypeID] INT NOT NULL, 
    [ConditionName] NVARCHAR(250) NOT NULL, 
    [ConditionDescription] NVARCHAR(1000) NULL, 
    [SQLQuery] VARCHAR(MAX) NOT NULL, 
    [ComparisonType] CHAR NOT NULL,	    
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0, 
)
