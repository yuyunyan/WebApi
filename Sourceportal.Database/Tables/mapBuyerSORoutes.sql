CREATE TABLE [dbo].[mapBuyerSORoutes]
(
	[UserID] INT NOT NULL, 
    [SOLineID] INT NOT NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0, 
    PRIMARY KEY ([SOLineID], [UserID])
)