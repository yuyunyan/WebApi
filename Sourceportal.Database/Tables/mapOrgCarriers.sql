CREATE TABLE [dbo].[mapOrgCarriers] (
    [OrganizationID] INT           NOT NULL,
    [CarrierID]      INT           NOT NULL,
    [AccountNumber]  NVARCHAR (50) NULL,
    [Created]        DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]      INT           NOT NULL,
    [Modified]       DATETIME      NULL,
    [ModifiedBy]     INT           NULL,
    [IsDeleted]      BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([CarrierID] ASC, [OrganizationID] ASC)
);

