CREATE TABLE [codes].[lkpPaymentTerms] (
    [PaymentTermID]   INT            IDENTITY (1, 1) NOT NULL,
    [TermName]        NVARCHAR (150) NOT NULL,
    [TermDesc]        NVARCHAR (500) NULL,
    [NetDueDays]      INT            NULL,
    [DiscountDays]    INT            NULL,
    [DiscountPercent] FLOAT (53)     NULL,
    [ExternalID]      VARCHAR (50)   NULL,
    [Created]         DATETIME       CONSTRAINT [DF__lkpPaymen__Creat__4C564A9F] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       INT            NOT NULL,
    [Modified]        DATETIME       NULL,
    [ModifiedBy]      INT            NULL,
    [IsDeleted]       BIT            CONSTRAINT [DF__lkpPaymen__IsDel__4D4A6ED8] DEFAULT ((0)) NOT NULL,
    [AccountTypeID]   INT            DEFAULT ((5)) NOT NULL,
    CONSTRAINT [PK__lkpPayme__80A6A8C71F60FD6F] PRIMARY KEY CLUSTERED ([PaymentTermID] ASC),
    CONSTRAINT [lkpPaymentTerms_AccountTypeID_Bitwise] CHECK ([dbo].[fnIsValidBitWiseInt]([AccountTypeID])=(1))
);

