CREATE TABLE [dbo].[lkpAccountHistory]
(
	[HistoryID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AccountID] INT NULL, 
    [ExternalAccountID] VARCHAR(10) NOT NULL, 
    [ExternalRecordID] VARCHAR(10) NOT NULL, 
    [DataSource] VARCHAR(10) NOT NULL, 
    [AccountName] VARCHAR(50) NOT NULL, 
    [Quantity] INT NOT NULL, 
    [DueDate] DATE NOT NULL, 
    [ReceivedDate] DATE NOT NULL, 
    [QuantityFailedQC] INT NOT NULL, 
    [PartNumber] VARCHAR(50) NULL, 
    [Manufacturer] VARCHAR(250) NULL, 
    [OrderNumber] INT NULL, 
    [OrderLine] INT NULL
)
