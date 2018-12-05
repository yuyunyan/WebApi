CREATE TABLE [dbo].[lkpStateEngineActions] (
    [ActionID]          INT             IDENTITY (1, 1) NOT NULL,
    [ObjectTypeID]      INT             NOT NULL,
    [ActionName]        NVARCHAR (250)  NOT NULL,
    [ActionDescription] NVARCHAR (1000) NULL,
    [Created]           DATETIME        CONSTRAINT [DF__lkpStateE__Creat__61074EC2] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]         INT             NOT NULL,
    [Modified]          DATETIME        NULL,
    [ModifiedBy]        INT             NULL,
    [IsDeleted]         BIT             CONSTRAINT [DF__lkpStateE__IsDel__61FB72FB] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__lkpState__FFE3F4B9E6D312EA] PRIMARY KEY CLUSTERED ([ActionID] ASC)
);


