CREATE TABLE [dbo].[lkpXlsDataMap]
(
	[XlsDataMapID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [XlsType] VARCHAR(50) NOT NULL, 
    [ColumnName] VARCHAR(50) NOT NULL, 
    [FieldLabel] VARCHAR(50) NOT NULL, 
    [DataType] VARCHAR(50) NOT NULL, 
    [IsRequired] BIT NOT NULL DEFAULT 0, 
    [ItemListTypeID] INT NULL 
)
