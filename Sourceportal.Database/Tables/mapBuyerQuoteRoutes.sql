CREATE TABLE [dbo].[mapBuyerQuoteRoutes] (
    [UserID]        INT      NOT NULL,
    [QuoteLineID]   INT      NOT NULL,
    [RouteStatusID] INT      NOT NULL,
    [Created]       DATETIME DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     INT      NOT NULL,
    [Modified]      DATETIME NULL,
    [ModifiedBy]    INT      NULL,
    [IsDeleted]     BIT      DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC, [QuoteLineID] ASC)
);


