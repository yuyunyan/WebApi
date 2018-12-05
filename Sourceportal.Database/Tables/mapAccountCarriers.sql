CREATE TABLE [dbo].[mapAccountCarriers] (
    [AccountID]     INT          NOT NULL,
    [CarrierID]     INT          NOT NULL,
    [AccountNumber] VARCHAR (50) NULL,
    [IsDefault]     BIT          CONSTRAINT [DF_mapAccountCarriers_IsDefault] DEFAULT ((0)) NOT NULL,
    [Created]       DATETIME     CONSTRAINT [DF__mapAccoun__Creat__31AD415B] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     INT          NOT NULL,
    [Modified]      DATETIME     NULL,
    [ModifiedBy]    INT          NULL,
    [IsDeleted]     BIT          CONSTRAINT [DF__mapAccoun__IsDel__32A16594] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__mapAccou__B8CBDF21786D20AD] PRIMARY KEY CLUSTERED ([CarrierID] ASC, [AccountID] ASC)
);

