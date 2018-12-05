CREATE TABLE [dbo].[lkpCommentTypes] (
    [CommentTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [ObjectTypeID]  INT           NOT NULL,
    [TypeName]      NVARCHAR (50) NOT NULL,
    [ExternalID]    VARCHAR (50)  NULL,
    [Created]       DATETIME      CONSTRAINT [DF__lkpCommen__Creat__1995C0A8] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]     INT           NOT NULL,
    [Modified]      DATETIME      NULL,
    [ModifiedBy]    INT           NULL,
    [IsDeleted]     BIT           CONSTRAINT [DF__lkpCommen__IsDel__1A89E4E1] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__lkpComme__D7118B0AF9268799] PRIMARY KEY CLUSTERED ([CommentTypeID] ASC)
);


