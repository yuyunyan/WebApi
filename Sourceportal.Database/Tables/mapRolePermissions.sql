CREATE TABLE [dbo].[mapRolePermissions] (
    [RoleID]       INT      NOT NULL,
    [PermissionID] INT      NOT NULL,
    [Created]      DATETIME DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy]    INT      NOT NULL,
    [Modified]     DATETIME NULL,
    [ModifiedBy]   INT      NULL,
    [IsDeleted]    BIT      DEFAULT 0 NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleID] ASC, [PermissionID] ASC)
);

