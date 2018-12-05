CREATE TABLE [dbo].[mapStateEngineApprovalRoutes]
(
	[RuleID] INT NOT NULL, 
    [UserID] INT NOT NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL, 
    PRIMARY KEY ([UserID], [RuleID])
)
