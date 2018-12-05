CREATE TABLE [dbo].[mapSalesOrderLineShipments] (
    [SOLineID]   INT          NOT NULL,
    [ShipmentID] INT          NOT NULL,
    [ExternalID] VARCHAR (50) NULL,
    [Qty]        INT          NOT NULL,
    [Created]    DATETIME     CONSTRAINT [DF__mapSalesO__Creat__72BBEAA9] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  INT          NOT NULL,
    [Modified]   DATETIME     NULL,
    [ModifiedBy] INT          NULL,
    [IsDeleted]  BIT          CONSTRAINT [DF__mapSalesO__IsDel__73B00EE2] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__mapSales__1934E6CE9ED0F9DC] PRIMARY KEY CLUSTERED ([ShipmentID] ASC, [SOLineID] ASC)
);


