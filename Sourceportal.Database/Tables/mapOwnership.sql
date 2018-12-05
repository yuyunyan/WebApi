CREATE TABLE [dbo].[mapOwnership] (
    [OwnerID]      INT        NOT NULL,
    [ObjectTypeID] INT        NOT NULL,
    [ObjectID]     INT        NOT NULL,
    [IsGroup]      BIT        DEFAULT ((0)) NOT NULL,
    [Percent]      FLOAT (53) DEFAULT ((100)) NULL,
    [Created]      DATETIME   DEFAULT (getutcdate()) NULL,
    [CreatedBy]    INT        NULL,
    [Modified]     DATETIME   NULL,
    [ModifiedBy]   INT        NULL,
    [IsDeleted]    BIT        DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_mapOwnership] PRIMARY KEY CLUSTERED ([OwnerID] ASC, [ObjectID] ASC, [ObjectTypeID] ASC, [IsGroup] ASC)
);

