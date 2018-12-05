CREATE TABLE [dbo].[mapQCChecklistJoins]
(
	[ChecklistID] INT NOT NULL , 
    [ObjectTypeID] INT NOT NULL, 
    [ObjectID] INT NOT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL,
    PRIMARY KEY ([ChecklistID], [ObjectID], [ObjectTypeID])
)
