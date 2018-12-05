CREATE TABLE [dbo].[QCChecklists]
(
	[ChecklistID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ParentChecklistID] INT NULL, 
    [ChecklistTypeID] INT NOT NULL, 
    [ChecklistName] NVARCHAR(50) NOT NULL, 
    [ChecklistDescription] NVARCHAR(500) NULL, 
    [SortOrder] INT NOT NULL, 
    [EffectiveStartDate] DATE NOT NULL DEFAULT GETUTCDATE(),
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
