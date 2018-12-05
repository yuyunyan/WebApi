CREATE TABLE [dbo].[Roles] (
    [RoleID]       INT           PRIMARY KEY IDENTITY NOT NULL,
    [RoleName]     NVARCHAR(128) NOT NULL,
    [ObjectTypeID] INT           NOT NULL,
    [Created]      DATETIME      DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy]    INT           NOT NULL,
    [Modified]     DATETIME      NULL,
    [ModifiedBy]   INT           NULL,
    [IsDeleted]    BIT           DEFAULT 0 NOT NULL
);

