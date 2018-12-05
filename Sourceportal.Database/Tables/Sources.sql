﻿CREATE TABLE [dbo].[Sources] (
    [SourceID]           INT             IDENTITY (1, 1) NOT NULL,
    [SourceTypeID]       INT             NOT NULL,
    [ItemID]             INT             NULL,
    [CommodityID]        INT             NOT NULL,
    [AccountID]          INT             NOT NULL,
    [ContactID]          INT             NULL,
    [CurrencyID]         CHAR (3)        NOT NULL,
    [PartNumber]         NVARCHAR (32)   NOT NULL,
    [PartNumberStrip]    NVARCHAR (32)   NOT NULL,
    [Manufacturer]       NVARCHAR (128)  NOT NULL,
    [Qty]                INT             NOT NULL,
    [Cost]               MONEY           NOT NULL,
    [DateCode]           NVARCHAR (25)   NULL,
    [PackagingID]        INT             NULL,
    [PackageConditionID] INT             NULL,
    [MOQ]                INT             NULL,
    [SPQ]                INT             NULL,
    [LeadTimeDays]       INT             NULL,
    [ValidForHours]      DECIMAL (18, 2) NULL,
    [RequestToBuy]       BIT             CONSTRAINT [DF__Sources__Request__668030F6] DEFAULT ((0)) NOT NULL,
    [RTBQty]             INT             NULL,
    [IsNoStock]          BIT             CONSTRAINT [DF_Sources_IsNoStock] DEFAULT ((0)) NOT NULL,
    [Created]            DATETIME        CONSTRAINT [DF__Sources__Created__6774552F] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]          INT             NOT NULL,
    [Modified]           DATETIME        NULL,
    [ModifiedBy]         INT             NULL,
    [IsDeleted]          BIT             CONSTRAINT [DF__Sources__IsDelet__68687968] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__Sources__16E019F909823849] PRIMARY KEY CLUSTERED ([SourceID] ASC)
);


