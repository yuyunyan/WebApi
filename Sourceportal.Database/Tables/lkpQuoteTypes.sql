CREATE TABLE [dbo].[lkpQuoteTypes] (
    [QuoteTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [TypeName]    NVARCHAR (50) NOT NULL,
    [IsDeleted]   BIT           CONSTRAINT [DF_lkpQuoteTypes_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_lkpQuoteTypes] PRIMARY KEY CLUSTERED ([QuoteTypeID] ASC)
);


