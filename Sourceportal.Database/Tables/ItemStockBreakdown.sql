CREATE TABLE [dbo].[ItemStockBreakdown]
(
	[BreakdownID] INT NOT NULL IDENTITY PRIMARY KEY, 
    [StockID] INT NOT NULL, 
    [IsDiscrepant] BIT DEFAULT 0 NOT NULL,
	[PackQty] INT NULL, 
    [NumPacks] INT NULL, 
    [PackagingID] INT NULL, 
    [PackageConditionID] INT NULL, 
    [DateCode] VARCHAR(50) NULL, 
    [COO] INT NULL, 
    [Expiry] DATE NULL,
	[MfrLotNum] VARCHAR (50) NULL,
	[Created] DATETIME DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy] INT NOT NULL,
    [Modified] DATETIME NULL,
    [ModifiedBy] INT NULL,
    [IsDeleted] BIT DEFAULT 0 NOT NULL,
)
