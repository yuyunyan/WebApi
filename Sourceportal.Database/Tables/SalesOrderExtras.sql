CREATE TABLE [dbo].[SalesOrderExtras]
(
	[SOExtraID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [SalesOrderID] INT NOT NULL, 
    [SOVersionID] INT NOT NULL, 
    [QuoteExtraID] INT NULL, 
    [StatusID] INT NOT NULL, 
    [ItemExtraID] INT NOT NULL, 
    [LineNum] INT NOT NULL, 
    [RefLineNum] INT NULL, 
    [Qty] INT NOT NULL, 
    [Price] MONEY NOT NULL, 
    [Cost] MONEY NOT NULL, 
    [PrintOnSO] BIT NOT NULL DEFAULT 0, 
    [Note] NVARCHAR(500) NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0
)
