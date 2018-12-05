CREATE TABLE [dbo].[mapStateEngineRuleActions] (
    [RuleActionID] INT          IDENTITY (1, 1) NOT NULL,
    [RuleID]       INT          NOT NULL,
    [ActionID]     INT          NOT NULL,
    [ValueID]      INT          NULL,
    [StaticValue]  VARCHAR (50) NULL,
    [Created]      DATETIME     CONSTRAINT [DF__mapStateE__Creat__64D7DFA6] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]    INT          NOT NULL,
    [Modified]     DATETIME     NULL,
    [ModifiedBy]   INT          NULL,
    [IsDeleted]    BIT          CONSTRAINT [DF__mapStateE__IsDel__65CC03DF] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__mapState__D4EB481EE9100A51] PRIMARY KEY CLUSTERED ([RuleActionID] ASC)
);


