CREATE TABLE [dbo].[lkpQCInspectionStatuses]
(
	[InspectionStatusID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StatusName] NVARCHAR(50) NOT NULL,
	[IsDefault] BIT NOT NULL DEFAULT 0,
	[ExternalID] NVARCHAR(50) NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
