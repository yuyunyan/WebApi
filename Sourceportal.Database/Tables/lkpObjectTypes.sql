CREATE TABLE [dbo].[lkpObjectTypes] (
    [ObjectTypeID] INT           IDENTITY (1, 1) NOT NULL,
	[ParentID]	   INT		     NULL,
    [TableID]      NCHAR (10)    NULL,
    [ObjectName]   VARCHAR (64)  NULL,
    [Description]  VARCHAR (255) NULL,
    [Created]      DATETIME      DEFAULT (getutcdate()) NULL,
    [CreatedBy]    INT           NULL,
    [Modified]     DATETIME      NULL,
    [ModifiedBy]   INT           NULL,
    [IsDeleted]    BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ObjectTypeID] ASC)
);

