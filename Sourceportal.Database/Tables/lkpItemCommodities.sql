CREATE TABLE [dbo].[lkpItemCommodities] (
    [CommodityID]   INT           IDENTITY (1, 1) NOT NULL,
    [ItemGroupID]   INT           NOT NULL,
    [ExternalID]    VARCHAR (50)  NULL,
    [CommodityName] NVARCHAR (50) NOT NULL,
    [Code]          NVARCHAR (10) NULL,
    [Created]       DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     INT           NOT NULL,
    [Modified]      DATETIME      NULL,
    [ModifiedBy]    INT           NULL,
    [IsDeleted]     BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([CommodityID] ASC)
);


