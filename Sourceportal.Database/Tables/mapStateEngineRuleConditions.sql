CREATE TABLE [dbo].[mapStateEngineRuleConditions] (
    [RuleConditionID] INT           IDENTITY (1, 1) NOT NULL,
    [RuleGroupID]     INT           NOT NULL,
    [ConditionID]     INT           NOT NULL,
    [Comparison]      VARCHAR (10)  NOT NULL,
    [ValueID]         INT           NULL,
    [StaticValue]     NVARCHAR (50) NULL,
    [Created]         DATETIME      CONSTRAINT [DF__mapStateE__Creat__5D36BDDE] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]       INT           NOT NULL,
    [Modified]        DATETIME      NULL,
    [ModifiedBy]      INT           NULL,
    [IsDeleted]       BIT           CONSTRAINT [DF__mapStateE__IsDel__5E2AE217] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__mapState__8F2A203D16B01988] PRIMARY KEY CLUSTERED ([RuleConditionID] ASC)
);


