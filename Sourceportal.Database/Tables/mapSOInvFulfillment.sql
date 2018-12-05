CREATE TABLE [dbo].[mapSOInvFulfillment] (
    [SOLineID]    INT          NOT NULL,
    [StockID] INT          NOT NULL,
    [Qty]         INT          NOT NULL,
    [ExternalID]  VARCHAR (50) NULL,
    [Created]     DATETIME     DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]   INT          NOT NULL,
    [Modified]    DATETIME     NULL,
    [ModifiedBy]  INT          NULL,
    [IsDeleted]   BIT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([SOLineID] ASC, [StockID] ASC)
);


