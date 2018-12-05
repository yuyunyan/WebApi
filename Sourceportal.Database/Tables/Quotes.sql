CREATE TABLE [dbo].[Quotes] (
    [QuoteID]          INT            IDENTITY (100000, 1) NOT NULL,
    [VersionID]        INT            CONSTRAINT [DF_Quotes_VersionID] DEFAULT ((1)) NOT NULL,
    [AccountID]        INT            NOT NULL,
    [ContactID]        INT            NOT NULL,
    [ProjectID]        INT            NULL,
    [ItemListID]       INT            NULL,
    [StatusID]         INT            NOT NULL,
    [IncotermID]       INT            NULL,
    [PaymentTermID]    INT            NULL,
    [CurrencyID]       CHAR (3)       NULL,
    [QuoteTypeID]      INT            NULL,
    [ShipLocationID]   INT            NULL,
    [ShippingMethodID] INT            NULL,
    [OrganizationID]   INT            NULL,
    [SentDate]         DATETIME       NULL,
    [ValidForHours]    INT            NULL,
    [Created]          DATETIME       CONSTRAINT [DF_Quotes_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT            NOT NULL,
    [Modified]         DATETIME       NULL,
    [ModifiedBy]       INT            NULL,
    [IsDeleted]        BIT            CONSTRAINT [DF_Quotes_IsDeleted] DEFAULT ((0)) NOT NULL,
    [IncotermLocation] NVARCHAR (100) NULL,
    CONSTRAINT [PK_Quotes] PRIMARY KEY CLUSTERED ([QuoteID] ASC, [VersionID] ASC)
);



GO


GO


GO


GO


