CREATE TABLE [dbo].[mapSourcesJoin] (
    [ObjectTypeID] INT      NOT NULL,
    [ObjectID]     INT      NOT NULL,
    [SourceID]     INT      NOT NULL,
    [IsMatch]      BIT      DEFAULT ((1)) NULL,
    [CommentUID]   INT      IDENTITY (1, 1) NOT NULL,
    [Created]      DATETIME DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]    INT      NOT NULL,
    [Modified]     DATETIME NULL,
    [ModifiedBy]   INT      NULL,
    [IsDeleted]    BIT      DEFAULT ((0)) NOT NULL,
    [Qty]          INT      NULL,
    PRIMARY KEY CLUSTERED ([ObjectTypeID] ASC, [ObjectID] ASC, [SourceID] ASC),
    UNIQUE NONCLUSTERED ([CommentUID] ASC)
);


