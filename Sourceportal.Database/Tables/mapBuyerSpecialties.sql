CREATE TABLE [dbo].[mapBuyerSpecialties]
(
	[UserID] INT NOT NULL , 
    [ObjectTypeID] INT NOT NULL, 
    [ObjectID] INT NOT NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0
    PRIMARY KEY ([UserID], [ObjectID], [ObjectTypeID])
)
