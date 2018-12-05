﻿CREATE TABLE [dbo].[Items] (
    [ItemID]          INT            IDENTITY (1, 1) NOT NULL,
    [MfrID]           INT            NOT NULL,
    [CommodityID]     INT            NOT NULL,
    [ItemStatusID]    INT            NOT NULL,
    [SourceDataID]    NVARCHAR (50)  NULL,
    [ExternalID]      VARCHAR (50)   NULL,
    [PartNumber]      NVARCHAR (32)  NOT NULL,
    [PartNumberStrip] NVARCHAR (32)  NOT NULL,
    [PartDescription] NVARCHAR (250) NULL,
    [MfrDescription]  NVARCHAR (250) NULL,
    [EURoHS]          BIT            NULL,
    [CNRoHS]          BIT            NULL,
    [ECCN]            NVARCHAR (20)  NULL,
    [WeightG]         FLOAT (53)     NULL,
    [LengthCM]        FLOAT (53)     NULL,
    [WidthCM]         FLOAT (53)     NULL,
    [DepthCM]         FLOAT (53)     NULL,
    [DatasheetURL]    NVARCHAR (500) NULL,
    [Created]         DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       INT            NOT NULL,
    [Modified]        DATETIME       NULL,
    [ModifiedBy]      INT            NULL,
    [IsDeleted]       BIT            DEFAULT ((0)) NOT NULL,
    [HTS]             VARCHAR (50)   NULL,
    [MSL]             VARCHAR (20)   NULL,
    CONSTRAINT [PK__Items__727E83EB77EBC109] PRIMARY KEY CLUSTERED ([ItemID] ASC)
);

