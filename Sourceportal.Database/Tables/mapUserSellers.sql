CREATE TABLE [dbo].[mapUserSellers]
(
	[UserID] INT NOT NULL PRIMARY KEY, 
    [SOLimit] MONEY NULL, 
    [CommPercent] DECIMAL(2) NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0
)
