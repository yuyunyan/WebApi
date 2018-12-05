CREATE TABLE [dbo].[mapXlsAccount]
(
	[XlsDataMapID] INT NOT NULL , 
    [AccountID] INT NOT NULL, 
    [ColumnIndex] INT NOT NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
    PRIMARY KEY ([AccountID], [XlsDataMapID])
)
