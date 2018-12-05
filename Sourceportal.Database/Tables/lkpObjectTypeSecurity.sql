CREATE TABLE [dbo].[lkpObjectTypeSecurity]
(
	[TypeSecurityID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TypeDescription] NVARCHAR(250) NOT NULL, 
    [ObjectTypeID] INT NOT NULL, 
    [FilterObjectTypeID] INT NOT NULL, 
    [FilterTypeID] INT NOT NULL, 
    [IsForAllObjects] BIT NOT NULL DEFAULT 0,
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
