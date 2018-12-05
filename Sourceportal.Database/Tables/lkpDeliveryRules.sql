CREATE TABLE [dbo].[lkpDeliveryRules] (
    [DeliveryRuleID]   INT            IDENTITY (1, 1) NOT NULL,
    [DeliveryRuleName] NVARCHAR (255) NOT NULL,
	[ExternalID]	   VARCHAR(50)	  NULL,
    [Created]          DATETIME       CONSTRAINT [DF_lkpDeliveryRules_Created] DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy]        INT            CONSTRAINT [DF_lkpDeliveryRules_CreatedBy] DEFAULT ((0)) NOT NULL,
    [Modified]         DATETIME       NULL,
    [ModifiedBy]       INT            NULL,
    [IsDeleted]        BIT            CONSTRAINT [DF_lkpDeliveryRules_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_lkpDeliveryRules] PRIMARY KEY CLUSTERED ([DeliveryRuleID] ASC)
);


