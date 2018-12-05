CREATE TABLE [dbo].[lkpStateEngineDynamicValues] (
    [ValueID]          INT            IDENTITY (1, 1) NOT NULL,
    [ValueName]        NVARCHAR (50)  NOT NULL,
    [ValueDescription] NVARCHAR (250) NULL,
    [SQLQuery]         VARCHAR (MAX)  NOT NULL,
    [Created]          DATETIME       CONSTRAINT [DF__lkpStateE__Creat__68A8708A] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT            NOT NULL,
    [Modified]         DATETIME       NULL,
    [ModifiedBy]       INT            NULL,
    [IsDeleted]        BIT            CONSTRAINT [DF__lkpStateE__IsDel__699C94C3] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__lkpState__93364E6882FEBD6D] PRIMARY KEY CLUSTERED ([ValueID] ASC)
);


