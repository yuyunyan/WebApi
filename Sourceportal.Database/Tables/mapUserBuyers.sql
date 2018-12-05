CREATE TABLE [dbo].[mapUserBuyers]
(
	[UserID] INT NOT NULL PRIMARY KEY, 
    [POLimit] MONEY NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0
)
