CREATE TABLE [dbo].[UserGridSettings] (
    [UserID]    INT           NOT NULL,
    [GridName]  VARCHAR (128) NOT NULL,
    [ColumnDef] VARCHAR (MAX) NULL,
    [SortDef]   VARCHAR (MAX) NULL,
    [FilterDef] VARCHAR (MAX) NULL,
    [Created]   DATETIME      CONSTRAINT [DF_UserGridSettings_Created] DEFAULT (getutcdate()) NULL,
    [Modified]  DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([GridName] ASC, [UserID] ASC)
);


