CREATE TABLE [dbo].[AccountHierarchies]
(
	[AccountHierarchyID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ParentID] INT NULL, 
	[RegionID] INT NOT NULL, 
    [HierarchyName] NVARCHAR(250) NOT NULL, 
    [SAPHierarchyID] VARCHAR(50) NULL, 
    [SAPGroupID] VARCHAR(50) NULL,
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
