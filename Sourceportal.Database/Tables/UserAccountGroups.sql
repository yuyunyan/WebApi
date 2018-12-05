CREATE TABLE [dbo].[UserAccountGroups] (
    [AccountGroupID] INT           IDENTITY (1, 1) NOT NULL,
    [UserID]         INT           NOT NULL,
    [GroupName]      VARCHAR (255) NOT NULL,
    [Created]        DATETIME      CONSTRAINT [DF_UserAccountGroups_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT           CONSTRAINT [DF_UserAccountGroups_CreatedBy] DEFAULT ((0)) NOT NULL,
    [Modified]       DATETIME      NULL,
    [ModifiedBy]     INT           NULL,
    [IsDeleted]      BIT           CONSTRAINT [DF_UserAccountGroups_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UserAccountGroups] PRIMARY KEY CLUSTERED ([AccountGroupID] ASC)
);


