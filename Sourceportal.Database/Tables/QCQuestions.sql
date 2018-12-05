CREATE TABLE [dbo].[QCQuestions] (
    [QuestionID]           INT            IDENTITY (1, 1) NOT NULL,
    [VersionID]            INT            NOT NULL,
    [ChecklistID]          INT            NOT NULL,
    [AnswerTypeID]         INT            NOT NULL,
    [QuestionText]         NVARCHAR (500) NOT NULL,
    [QuestionSubText]      NVARCHAR (500) NULL,
    [QuestionHelpText]     NVARCHAR (500) NULL,
    [SortOrder]            INT            NOT NULL,
    [ShowQtyFailed]        BIT            CONSTRAINT [DF_QCQuestions_ShowQtyFailed] DEFAULT ((1)) NOT NULL,
    [CanComment]           BIT            CONSTRAINT [DF__QCQuestio__CanCo__6F9F86DC] DEFAULT ((1)) NOT NULL,
    [RequiresPicture]      BIT            CONSTRAINT [DF__QCQuestio__Requi__7093AB15] DEFAULT ((0)) NOT NULL,
    [RequiresSignature]    BIT            CONSTRAINT [DF__QCQuestio__Requi__7187CF4E] DEFAULT ((0)) NOT NULL,
    [PrintOnInspectReport] BIT            CONSTRAINT [DF__QCQuestio__Print__727BF387] DEFAULT ((0)) NOT NULL,
    [PrintOnRejectReport]  BIT            CONSTRAINT [DF__QCQuestio__Print__737017C0] DEFAULT ((0)) NOT NULL,
    [Created]              DATETIME       CONSTRAINT [DF__QCQuestio__Creat__74643BF9] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]            INT            NOT NULL,
    [Modified]             DATETIME       NULL,
    [ModifiedBy]           INT            NULL,
    [IsDeleted]            BIT            CONSTRAINT [DF__QCQuestio__IsDel__75586032] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__QCQuesti__C61A46D75638B145] PRIMARY KEY CLUSTERED ([VersionID] ASC, [QuestionID] ASC)
);


