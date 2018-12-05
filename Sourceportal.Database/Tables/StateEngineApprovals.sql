CREATE TABLE [dbo].[StateEngineApprovals]
(
	[ApprovalID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [RuleConditionID] INT NOT NULL, 
    [ApprovalObjectID] INT NOT NULL, 
    [ApprovedBy] INT NULL, 
    [ApprovalValue] INT NULL, 
    [ApprovedDate] DATETIME NULL, 
    [CreatedBy] INT NOT NULL, 
    [Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [IsDeleted] BIT NOT NULL DEFAULT 0
)
