CREATE TABLE [dbo].[WarehouseBins] (
    [WarehouseBinID] INT            IDENTITY (1, 1) NOT NULL,
    [WarehouseID]    INT            NOT NULL,
    [BinName]        NVARCHAR (100) NOT NULL,
    [IsSelectable]	 BIT			DEFAULT 0 NOT NULL,
	[ExternalID]     VARCHAR (50)   NULL,
    [ExternalUUID]   VARCHAR (50)   NULL,
    [Created]        DATETIME       CONSTRAINT [DF__Warehouse__Creat__16F94B1F] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]      INT            NOT NULL,
    [Modified]       DATETIME       NULL,
    [ModifiedBy]     INT            NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF__Warehouse__IsDel__17ED6F58] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__Warehous__87B5AA44F92F5B0E] PRIMARY KEY CLUSTERED ([WarehouseBinID] ASC)
);

