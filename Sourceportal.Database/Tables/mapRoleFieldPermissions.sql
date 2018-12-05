CREATE TABLE [dbo].[mapRoleFieldPermissions]
(
	[RoleID] INT NOT NULL, 
    [FieldID] INT NOT NULL,
	[CanEdit] BIT NOT NULL DEFAULT 0,
    [Created] DATETIME DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy] INT NOT NULL,
    [Modified] DATETIME NULL,
    [ModifiedBy] INT NULL,
    [IsDeleted] BIT DEFAULT 0 NOT NULL, 
    PRIMARY KEY ([FieldID], [RoleID])
)
