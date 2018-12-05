CREATE TABLE [codes].[lkpPackageConditions]
(
	[PackageConditionID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ConditionName] NVARCHAR(50) NOT NULL,
	[ExternalID] VARCHAR(50) NULL,
	[Created] DATETIME DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy] INT NOT NULL,
    [Modified] DATETIME NULL,
    [ModifiedBy] INT NULL,
    [IsDeleted] BIT DEFAULT 0 NOT NULL
)
