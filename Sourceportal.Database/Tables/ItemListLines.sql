CREATE TABLE [dbo].[ItemListLines] (
    [ItemListLineID]  INT            IDENTITY (1, 1) NOT NULL,
    [ItemListID]      INT            NOT NULL,
    [StatusID]        INT            NOT NULL,
    [ItemID]          INT            NULL,
    [CommodityID]     INT            NOT NULL,
    [ProjectID]       INT            NULL,
    [AssocAccountID]  INT            NULL,
    [CustomerPartNum] NVARCHAR (32)  NULL,
    [PartNumber]      NVARCHAR (32)  NOT NULL,
    [PartNumberStrip] NVARCHAR (32)  NOT NULL,
    [Manufacturer]    NVARCHAR (128) NULL,
    [Qty]             INT            NULL,
    [TargetPrice]     MONEY          NULL,
    [TargetDateCode]  NVARCHAR (25)  NULL,
    [DueDate]         DATE           NULL,
    [Created]         DATETIME       CONSTRAINT [DF__ItemListL__Creat__08362A7C] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       INT            NOT NULL,
    [Modified]        DATETIME       NULL,
    [ModifiedBy]      INT            NULL,
    [IsDeleted]       BIT            CONSTRAINT [DF__ItemListL__IsDel__092A4EB5] DEFAULT ((0)) NOT NULL,
    [MOQ]             INT            NULL,
    [SPQ]             INT            NULL,
    CONSTRAINT [PK__ItemList__566D7E3A914EEA47] PRIMARY KEY CLUSTERED ([ItemListLineID] ASC)
);


