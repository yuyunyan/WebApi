CREATE TABLE [dbo].[VendorRFQs] (
    [VendorRFQID]    INT      IDENTITY (1, 1) NOT NULL,
    [AccountID]      INT      NOT NULL,
    [ContactID]      INT      NOT NULL,
    [StatusID]       INT      NOT NULL,
    [OrganizationID] INT      NULL,
    [CurrencyID]     CHAR (3) NULL,
    [SentDate]       DATETIME NULL,
    [Created]        DATETIME CONSTRAINT [DF__VendorRFQ__Creat__62CF9BA3] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]      INT      NOT NULL,
    [Modified]       DATETIME NULL,
    [ModifiedBy]     INT      NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF__VendorRFQ__IsDel__63C3BFDC] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__VendorRF__2825D1FEF90263E0] PRIMARY KEY CLUSTERED ([VendorRFQID] ASC)
);


