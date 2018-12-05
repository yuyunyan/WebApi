CREATE TABLE [dbo].[lkpPermissions] (
    [PermissionID] INT           PRIMARY KEY IDENTITY NOT NULL,
    [ObjectTypeID] INT  NOT NULL,
    [PermName]         VARCHAR (128) NOT NULL,
    [PermDescription]  VARCHAR (256) NULL,
	[IsObjectSpecific] BIT NOT NULL DEFAULT 0,
    [Created]      DATETIME      DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy]    INT           NOT NULL,
    [Modified]     DATETIME      NULL,
    [ModifiedBy]   INT           NULL,
    [IsDeleted]    BIT           DEFAULT 0 NOT NULL
);

