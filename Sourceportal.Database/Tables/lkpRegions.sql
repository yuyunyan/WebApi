CREATE TABLE [dbo].[lkpRegions]
(
	[RegionID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RegionName] NVARCHAR(50) NOT NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
