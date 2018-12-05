CREATE TABLE [dbo].[mapUserGroupMembers] (
    [UserID]      INT      IDENTITY (1, 1) NOT NULL,
    [UserGroupID] INT      NOT NULL,
    [Created]     DATETIME DEFAULT (getutcdate()) NULL,
    [CreatedBy]   INT      NULL,
    [Modified]    DATETIME NULL,
    [ModifiedBy]  INT      NULL,
    [IsDeleted]   BIT      DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC, [UserGroupID] ASC)
);

