CREATE TABLE [dbo].[mapSOPOAllocation] (
    [SOLineID]   INT          NOT NULL,
    [POLineID]   INT          NOT NULL,
    [Qty]        INT          NOT NULL,
    [ExternalID] VARCHAR (50) NULL,
    [Created]    DATETIME     DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  INT          NOT NULL,
    [Modified]   DATETIME     NULL,
    [ModifiedBy] INT          NULL,
    [IsDeleted]  BIT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([POLineID] ASC, [SOLineID] ASC)
);


