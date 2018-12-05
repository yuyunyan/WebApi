CREATE TABLE [dbo].[lkpCurrencies] (
    [CurrencyID]   CHAR (3)      NOT NULL,
    [CurrencyName] NVARCHAR (50) NOT NULL,
    [Created]      DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]    INT           NOT NULL,
    [Modified]     DATETIME      NULL,
    [ModifiedBy]   INT           NULL,
    [IsDeleted]    BIT           DEFAULT ((0)) NOT NULL,
    [ExternalID]   VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([CurrencyID] ASC)
);


