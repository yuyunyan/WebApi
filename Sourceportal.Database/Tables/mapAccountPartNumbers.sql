CREATE TABLE [dbo].[mapAccountPartNumbers] (
    [AccountPartNumID] INT           IDENTITY (1, 1) NOT NULL,
    [AccountID]        INT           NOT NULL,
    [ItemID]           INT           NOT NULL,
    [ProjectID]        INT           NULL,
    [InternalPartNum]  NVARCHAR (32) NOT NULL,
    [Created]          DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT           NOT NULL,
    [Modified]         DATETIME      NULL,
    [ModifiedBy]       INT           NULL,
    [IsDeleted]        BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([AccountPartNumID] ASC)
);


