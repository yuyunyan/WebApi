CREATE TABLE [dbo].[lkpAccountStatuses] (
    [AccountStatusID]          INT           IDENTITY (1, 1) NOT NULL,
    [Name]                     VARCHAR (128) NULL,
    [AccountIsMgmtApproved]    BIT           NULL,
    [AccountIsFinanceApproved] BIT           NULL,
    [AccountIsActive]          BIT           NULL,
    [AccountIsBlacklisted]     BIT           NULL,
    [ExternalID]               VARCHAR (50)  NULL,
    [IsDefault]                BIT           CONSTRAINT [DF_lkpAccountStatuses_IsDefault] DEFAULT ((0)) NOT NULL,
    [Created]                  DATETIME      CONSTRAINT [DF__AccountSt__Creat__69C6B1F5] DEFAULT (getutcdate()) NULL,
    [CreatedBy]                INT           NULL,
    [Modified]                 DATETIME      NULL,
    [ModifiedBy]               INT           NULL,
    [IsDeleted]                BIT           CONSTRAINT [DF__AccountSt__IsDel__6ABAD62E] DEFAULT ((0)) NULL,
    CONSTRAINT [PK__AccountS__6E191785DD3F279F] PRIMARY KEY CLUSTERED ([AccountStatusID] ASC)
);


