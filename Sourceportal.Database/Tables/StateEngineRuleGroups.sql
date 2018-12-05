CREATE TABLE [dbo].[StateEngineRuleGroups] (
    [RuleGroupID]   INT      IDENTITY (1, 1) NOT NULL,
    [RuleID]        INT      NOT NULL,
    [ParentGroupID] INT      NULL,
    [IsAll]         BIT      CONSTRAINT [DF__StateEngi__IsAll__587208C1] DEFAULT ((0)) NOT NULL,
    [Created]       DATETIME CONSTRAINT [DF__StateEngi__Creat__59662CFA] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     INT      NOT NULL,
    [Modified]      DATETIME NULL,
    [ModifiedBy]    INT      NULL,
    [IsDeleted]     BIT      CONSTRAINT [DF__StateEngi__IsDel__5A5A5133] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__StateEng__7491321571272D49] PRIMARY KEY CLUSTERED ([RuleGroupID] ASC)
);


