CREATE TABLE [dbo].[Locations] (
    [LocationID]     INT           IDENTITY (1, 1) NOT NULL,
    [AccountID]      INT           NULL,
    [LocationTypeID] INT           NULL,
    [Name]           VARCHAR (128) NULL,
    [Address1]       VARCHAR (64)  NULL,
    [Address2]       VARCHAR (64)  NULL,
    [HouseNumber]    VARCHAR (32)  NULL,
    [Street]         VARCHAR (64)  NULL,
    [Address4]       VARCHAR (64)  NULL,
    [City]           VARCHAR (64)  NULL,
    [StateID]        INT           NULL,
    [PostalCode]     VARCHAR (32)  NULL,
    [District]       VARCHAR (64)  NULL,
    [CountryID]      INT           NULL,
    [ExternalID]     VARCHAR (50)  NULL,
    [Note]           VARCHAR (512) NULL,
    [Created]        DATETIME      CONSTRAINT [DF__Locations__Creat__56B3DD81] DEFAULT (getutcdate()) NULL,
    [CreatedBy]      INT           NULL,
    [Modified]       DATETIME      NULL,
    [ModifiedBy]     INT           NULL,
    [IsDeleted]      BIT           CONSTRAINT [DF__Locations__IsDel__6D9742D9] DEFAULT ((0)) NULL,
    CONSTRAINT [PK__Location__E7FEA477114C9B5B] PRIMARY KEY CLUSTERED ([LocationID] ASC)
);


