CREATE TABLE [dbo].[lkpStateEngineTriggers] (
    [TriggerID]          INT             IDENTITY (1, 1) NOT NULL,
    [ObjectTypeID]       INT             NOT NULL,
    [TriggerName]        NVARCHAR (250)  NOT NULL,
    [TriggerDescription] NVARCHAR (1000) NULL,
    [Created]            DATETIME        CONSTRAINT [DF__lkpStateE__Creat__54A177DD] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]          INT             NOT NULL,
    [Modified]           DATETIME        NULL,
    [ModifiedBy]         INT             NULL,
    [IsDeleted]          BIT             CONSTRAINT [DF__lkpStateE__IsDel__55959C16] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__lkpState__11321F020F18AC0F] PRIMARY KEY CLUSTERED ([TriggerID] ASC)
);


