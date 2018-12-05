CREATE TABLE [dbo].[lkpQCInspectionTypes]
(
	[InspectionTypeID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [TypeName] NVARCHAR(200) NOT NULL, 
    [ExternalID] VARCHAR(50) NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
