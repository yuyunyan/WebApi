CREATE TABLE [dbo].[Warehouses] (
    [WarehouseID]    INT           IDENTITY (1, 1) NOT NULL,
    [OrganizationID] INT           NOT NULL,
    [LocationID]     INT           NULL,
    [WarehouseName]  NVARCHAR (50) NOT NULL,
	[AcceptedBinID]  INT           NULL,
	[RejectedBinID]  INT           NULL,
	[ShipFromRegionID]	INT		   NULL,
    [ExternalID]     NVARCHAR (32) NULL,
    [Created]        DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]      INT           NOT NULL,
    [Modified]       DATETIME      NULL,
    [ModifiedBy]     INT           NULL,
    [IsDeleted]      BIT           DEFAULT ((0)) NOT NULL,
    [ExternalUUID]   VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([WarehouseID] ASC)
);


