CREATE TABLE [dbo].[Shipments]
(
	[ShipmentID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ExternalID] VARCHAR(50) NOT NULL, 
    [ExternalUUID] VARCHAR(50) NOT NULL, 
    [CarrierName] NVARCHAR(200) NULL, 
    [TrackingNumber] VARCHAR(200) NULL, 
    [TrackingURL] VARCHAR(1000) NULL, 
    [ShipDate] DATE NOT NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
