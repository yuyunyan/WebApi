CREATE TABLE [dbo].[VendorRFQLines] (
    [VRFQLineID]      INT            IDENTITY (1, 1) NOT NULL,
    [VendorRFQID]     INT            NOT NULL,
    [StatusID]        INT            NOT NULL,
    [ItemID]          INT            NULL,
    [QuoteLineID]     INT            NULL,
    [CommodityID]     INT            NOT NULL,
    [LineNum]         INT            NOT NULL,
    [PartNumber]      NVARCHAR (32)  NOT NULL,
    [PartNumberStrip] NVARCHAR (32)  NOT NULL,
    [Manufacturer]    NVARCHAR (128) NOT NULL,
    [Qty]             INT            NOT NULL,
    [TargetCost]      MONEY          NULL,
    [DateCode]        NVARCHAR (25)  NULL,
    [PackagingID]     INT            NULL,
    [Note]            NVARCHAR (500) NULL,
    [HasNoStock]      BIT            CONSTRAINT [DF__VendorRFQ__HasNo__0FD74C44] DEFAULT ((0)) NOT NULL,
    [Created]         DATETIME       CONSTRAINT [DF__VendorRFQ__Creat__10CB707D] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       INT            NOT NULL,
    [Modified]        DATETIME       NULL,
    [ModifiedBy]      INT            NULL,
    [IsDeleted]       BIT            CONSTRAINT [DF__VendorRFQ__IsDel__11BF94B6] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__VendorRF__3F3253D7C88BC5C2] PRIMARY KEY CLUSTERED ([VRFQLineID] ASC)
);


