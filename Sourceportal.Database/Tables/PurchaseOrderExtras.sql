CREATE TABLE [dbo].[PurchaseOrderExtras]
(
	[POExtraID] INT IDENTITY NOT NULL PRIMARY KEY, 
    [PurchaseOrderID] INT NOT NULL, 
    [POVersionID] INT NOT NULL, 
    [StatusID] INT NOT NULL, 
    [ItemExtraID] INT NOT NULL, 
    [LineNum] INT NOT NULL, 
    [RefLineNum] INT NULL, 
    [Qty] INT NOT NULL, 
    [Cost] MONEY NOT NULL, 
	[PrintOnPO] BIT NOT NULL DEFAULT 0,
    [Note] NVARCHAR(500) NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0
)
