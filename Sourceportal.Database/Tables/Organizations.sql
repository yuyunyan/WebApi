CREATE TABLE [dbo].[Organizations] (
    [OrganizationID] INT          IDENTITY (1, 1) NOT NULL,
    [ParentOrgID]    INT          NULL,
    [Name]           VARCHAR (50) NOT NULL,
    [ExternalID]     VARCHAR (50) NULL,
	[BankName]		 VARCHAR (128) NULL,
	[BranchName]	 VARCHAR (128) NULL,
	[USDAccount]	 VARCHAR (128) NULL,
	[EURAccount]	 VARCHAR (128) NULL,
	[SwiftAccount]	 VARCHAR (128) NULL,
	[RoutingNumber]	 VARCHAR (128) NULL,
    [Created]        DATETIME     CONSTRAINT [DF__Organizat__Creat__4A4E069C] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]      INT          NOT NULL,
    [Modified]       DATETIME     NULL,
    [ModifiedBy]     INT          NULL,
    [IsDeleted]      BIT          CONSTRAINT [DF__Organizat__IsDel__4B422AD5] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__Organiza__CADB0B72934C4CA4] PRIMARY KEY CLUSTERED ([OrganizationID] ASC)
);



