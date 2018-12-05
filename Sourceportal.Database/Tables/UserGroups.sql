CREATE TABLE [dbo].[UserGroups] (
    [UserGroupID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (228) NULL,
    [Created]     DATETIME      DEFAULT (getutcdate()) NULL,
    [CreatedBy]   INT           NULL,
    [Modified]    DATETIME      NULL,
    [ModifiedBy]  INT           NULL,
    [IsDeleted]   BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserGroupID] ASC)
);

