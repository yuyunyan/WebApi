﻿CREATE TABLE [dbo].[SalesOrders] (
    [SalesOrderID]     INT            IDENTITY (100000, 1) NOT NULL,
    [VersionID]        INT            NOT NULL,
    [AccountID]        INT            NOT NULL,
    [ContactID]        INT            NOT NULL,
    [ProjectID]        INT            NULL,
    [QuoteID]          INT            NULL,
    [StatusID]         INT            NOT NULL,
    [IncotermID]       INT            NULL,
    [PaymentTermID]    INT            NULL,
    [CurrencyID]       CHAR (3)       NULL,
    [ShipLocationID]   INT            NULL,
    [ShippingMethodID] INT            NULL,
    [OrganizationID]   INT            NULL,
    [UltDestinationID] INT            NULL,
    [FreightPaymentID] INT            NULL,
    [CarrierID]        INT            NULL,
    [CarrierMethodID]  INT            NULL,
    [DeliveryRuleID]   INT            NULL,
    [FreightAccount]   NVARCHAR (50)  NULL,
	[ShipFromRegionID] INT			  NULL,
    [OrderDate]        DATE           NOT NULL,
    [CustomerPO]       NVARCHAR (50)  NULL,
    [ShippingNotes]    NVARCHAR (MAX) NULL,
    [QCNotes]          NVARCHAR (MAX) NULL,
    [IncotermLocation] NVARCHAR (100) NULL,
    [ExternalID]       VARCHAR (50)   NULL,
    [Created]          DATETIME       CONSTRAINT [DF__SalesOrde__Creat__69678A99] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT            NOT NULL,
    [Modified]         DATETIME       NULL,
    [ModifiedBy]       INT            NULL,
    [IsDeleted]        BIT            CONSTRAINT [DF__SalesOrde__IsDel__6A5BAED2] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__SalesOrd__2DD2401302936DC7] PRIMARY KEY CLUSTERED ([VersionID] ASC, [SalesOrderID] ASC)
);


