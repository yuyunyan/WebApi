CREATE TABLE [dbo].[lkpQCResults] (
    [ResultID]       INT           IDENTITY (1, 1) NOT NULL,
    [ResultName]     NVARCHAR (50) NOT NULL,
    [IsRejected]     BIT           CONSTRAINT [DF__lkpQCResu__IsRej__0B879873] DEFAULT ((0)) NOT NULL,
    [DecisionCode]   VARCHAR (50)  NULL,
    [AcceptanceCode] VARCHAR (50)  NULL,
    [Created]        DATETIME      CONSTRAINT [DF__lkpQCResu__Creat__0C7BBCAC] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]      INT           NOT NULL,
    [Modified]       DATETIME      NULL,
    [ModifiedBy]     INT           NULL,
    [IsDeleted]      BIT           CONSTRAINT [DF__lkpQCResu__IsDel__0D6FE0E5] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__lkpQCRes__97690228EEB1755F] PRIMARY KEY CLUSTERED ([ResultID] ASC)
);

