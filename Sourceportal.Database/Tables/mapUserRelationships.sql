CREATE TABLE [dbo].[mapUserRelationships] (
    [UserRelationshipID] INT      IDENTITY (1, 1) NOT NULL,
    [User1ID]            INT      NULL,
    [User2ID]            INT      NULL,
    [RelationshipTypeID] INT      NULL,
    [Created]            DATETIME DEFAULT (getutcdate()) NULL,
    [CreatedBy]          INT      NULL,
    [Modified]           DATETIME NULL,
    [ModifiedBy]         INT      NULL,
    [IsDeleted]          BIT      DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserRelationshipID] ASC)
);

