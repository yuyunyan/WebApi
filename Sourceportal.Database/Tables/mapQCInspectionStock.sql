CREATE TABLE [dbo].[mapQCInspectionStock]
(
	[InspectionID] INT NOT NULL, 
    [StockID] INT NOT NULL, 
	[Created] DATETIME DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy] INT NOT NULL,
    [Modified] DATETIME NULL,
    [ModifiedBy] INT NULL,
    [IsDeleted] BIT DEFAULT 0 NOT NULL,
    PRIMARY KEY ([StockID], [InspectionID])
)
