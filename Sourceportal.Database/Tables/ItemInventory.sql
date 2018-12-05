CREATE TABLE [dbo].[ItemInventory]
(
	[InventoryID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StockID] INT NOT NULL, 
    [WarehouseBinID] INT NOT NULL, 
	[IsInspection] BIT NOT NULL DEFAULT 0,
    [Qty] INT NOT NULL,
	[Created] DATETIME DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy] INT NOT NULL,
    [Modified] DATETIME NULL,
    [ModifiedBy] INT NULL,
    [IsDeleted] BIT DEFAULT 0 NOT NULL,
)
