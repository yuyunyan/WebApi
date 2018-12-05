CREATE TABLE [dbo].[lkpNavigation] (
    [NavID]       INT           PRIMARY KEY IDENTITY NOT NULL,
    [ParentNavID] INT    NULL,
    [Interface]   VARCHAR (256) NULL,
    [NavName]     VARCHAR (128) NOT NULL,
    [Icon]        VARCHAR (128) NULL,
    [SortOrder]   INT           NOT NULL,
    [IsNavMenu]   BIT           NOT NULL DEFAULT 0,
    [Created]     DATETIME      DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy]   INT           NOT NULL,
    [Modified]    DATETIME      NULL,
    [ModifiedBy]  INT           NULL,
    [IsDeleted]   BIT           DEFAULT 0 NOT NULL
);

