CREATE TABLE [dbo].[StateEngineRules] (
    [RuleID]       INT            IDENTITY (1, 1) NOT NULL,
    [TriggerID]    INT            NOT NULL,
    [ObjectTypeID] INT            NOT NULL,
    [RuleName]     NVARCHAR (250) NOT NULL,
    [ExecOrder]    INT            NOT NULL,
    [RequiresApproval]	BIT		  NOT NULL DEFAULT 0,
	[Created]      DATETIME       CONSTRAINT [DF__StateEngi__Creat__50D0E6F9] DEFAULT (getutcdate()) NOT NULL,	
    [CreatedBy]    INT            NOT NULL,
    [Modified]     DATETIME       NULL,
    [ModifiedBy]   INT            NULL,
    [IsDeleted]    BIT            CONSTRAINT [DF__StateEngi__IsDel__51C50B32] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__StateEng__110458C26187BDBA] PRIMARY KEY CLUSTERED ([RuleID] ASC)
);


