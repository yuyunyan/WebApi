CREATE TABLE [dbo].[lkpAccountFocusTypes] (
    [FocusTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [FocusName]     NVARCHAR (25) NULL,
    [TypeRank]      INT           NULL,
    [IsBlacklisted] BIT           CONSTRAINT [DF_lkpAccountFocusTypes_IsBlacklisted] DEFAULT ((0)) NOT NULL,
    [Created]       DATETIME      CONSTRAINT [DF_lkpAccountFocusTypes_Created] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     INT           NOT NULL,
    [Modified]      DATETIME      NULL,
    [ModifiedBy]    INT           NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF_lkpAccountFocusTypes_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__lkpAccount] PRIMARY KEY CLUSTERED ([FocusTypeID] ASC)
);


GO