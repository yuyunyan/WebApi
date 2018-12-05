CREATE TABLE [dbo].[QuoteExtras]
(
	[QuoteExtraID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [QuoteID] INT NOT NULL, 
    [QuoteVersionID] INT NOT NULL, 
    [StatusID] INT NOT NULL, 
    [ItemExtraID] INT NOT NULL, 
    [LineNum] INT NOT NULL, 
	[RefLineNum] INT NULL, 
    [Qty] INT NOT NULL, 
    [Price] MONEY NOT NULL, 
    [Cost] MONEY NOT NULL, 
    [PrintOnQuote] BIT NOT NULL DEFAULT 0, 
    [Note] NVARCHAR(256) NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL, 
    [IsDeleted] BIT NOT NULL DEFAULT 0
)
