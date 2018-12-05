CREATE TABLE [dbo].[lkpItemListTypes] (
    [ItemListTypeID] INT          IDENTITY (1, 1) NOT NULL,
    [TypeName]       VARCHAR (64) NULL,
    [CanMatch]       BIT          NULL,
    PRIMARY KEY CLUSTERED ([ItemListTypeID] ASC)
);

