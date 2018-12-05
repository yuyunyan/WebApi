CREATE TABLE [dbo].[lkpItemGroups] (
    [ItemGroupID] INT           IDENTITY (1, 1) NOT NULL,
    [ExternalID]  VARCHAR (50)  NULL,
    [Groupname]   NVARCHAR (50) NOT NULL,
    [Code]        NVARCHAR (10) NULL,
    [Created]     DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]   INT           NOT NULL,
    [Modified]    DATETIME      NULL,
    [ModifiedBy]  INT           NULL,
    [IsDeleted]   BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ItemGroupID] ASC)
);


