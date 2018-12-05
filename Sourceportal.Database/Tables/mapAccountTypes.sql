CREATE TABLE [dbo].[mapAccountTypes] (
    [AccountID]       INT      NOT NULL,
    [AccountTypeID]   INT      NOT NULL,
    [AccountStatusID] INT      NOT NULL,
    [PaymentTermID]   INT      NULL,
	[EPDSID]		  VARCHAR(10) NULL,
    [IsDeleted]       BIT      DEFAULT ((0)) NOT NULL,
    [Created]         DATETIME DEFAULT (getutcdate()) NULL,
    [CreatedBy]       INT      NULL,
    [Modified]        DATETIME NULL,
    [ModifiedBy]      INT      NULL,
    [Rating]          INT      NULL,
    PRIMARY KEY CLUSTERED ([AccountTypeID] ASC, [AccountID] ASC),
    CONSTRAINT [FK_mapAccountTypes_Accounts] FOREIGN KEY ([AccountID]) REFERENCES [dbo].[Accounts] ([AccountID]),
    CONSTRAINT [FK_mapAccountTypes_AccountStatusID] FOREIGN KEY ([AccountStatusID]) REFERENCES [dbo].[lkpAccountStatuses] ([AccountStatusID]),
    CONSTRAINT [FK_mapAccountTypes_AccountTypes] FOREIGN KEY ([AccountTypeID]) REFERENCES [dbo].[lkpAccountTypes] ([AccountTypeID]),
    CONSTRAINT [FK_mapAccountTypes_PaymentTermID] FOREIGN KEY ([PaymentTermID]) REFERENCES [codes].[lkpPaymentTerms] ([PaymentTermID])
);


