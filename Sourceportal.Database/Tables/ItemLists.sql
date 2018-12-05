CREATE TABLE [dbo].[ItemLists] (
    [ItemListID]     INT            IDENTITY (1, 1) NOT NULL,
    [AccountID]      INT            NOT NULL,
    [ContactID]      INT            NULL,
    [ProjectID]      INT            NULL,
    [StatusID]       INT            NOT NULL,
    [CurrencyID]     VARCHAR (8)    NOT NULL,
    [OrganizationID] INT            NOT NULL,
    [SalesUserID]    INT            NOT NULL,
    [ListName]       NVARCHAR (250) NULL,
    [ItemListTypeID] INT            NULL,
    [QuoteTypeID]    INT            NULL,
    [Created]        DATETIME       CONSTRAINT [DF__ItemLists__Creat__04659998] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]      INT            NOT NULL,
    [Modified]       DATETIME       NULL,
    [ModifiedBy]     INT            NULL,
    [IsDeleted]      BIT            CONSTRAINT [DF__ItemLists__IsDel__0559BDD1] DEFAULT ((0)) NOT NULL,
    [IsPublished]    BIT            CONSTRAINT [DF_ItemLists_IsPublished] DEFAULT ((0)) NULL,
    CONSTRAINT [PK__ItemList__74A68EC908E80A81] PRIMARY KEY CLUSTERED ([ItemListID] ASC)
);


