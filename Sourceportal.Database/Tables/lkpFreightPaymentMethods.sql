CREATE TABLE [codes].[lkpFreightPaymentMethods] (
    [FreightPaymentMethodID] INT            IDENTITY (1, 1) NOT NULL,
    [MethodName]             NVARCHAR (250) NOT NULL,
    [UseAccountNum]          BIT            CONSTRAINT [DF_lkpFreightPaymentMethods_UseAccountNum] DEFAULT ((0)) NOT NULL,
    [ExternalID]             VARCHAR (50)   NULL,
    [Created]                DATETIME       CONSTRAINT [DF__lkpFreigh__Creat__69279377] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]              INT            NOT NULL,
    [Modified]               DATETIME       NULL,
    [ModifiedBy]             INT            NULL,
    [IsDeleted]              BIT            CONSTRAINT [DF__lkpFreigh__IsDel__6A1BB7B0] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__lkpFreig__F46C84E7A85B95CE] PRIMARY KEY CLUSTERED ([FreightPaymentMethodID] ASC)
);


