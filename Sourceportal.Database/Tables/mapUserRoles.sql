CREATE TABLE [dbo].[mapUserRoles] (
    [UserRoleID] INT NOT NULL PRIMARY KEY IDENTITY,
	[UserID] INT NOT NULL,
    [RoleID] INT NOT NULL,    
    [FilterObjectTypeID] INT NOT NULL,
	[FilterTypeID] INT NOT NULL,
    [FilterObjectID] INT NOT NULL,
    [Created] DATETIME DEFAULT (GETUTCDATE()) NOT NULL,
    [CreatedBy] INT NOT NULL,
    [Modified] DATETIME NULL,
    [ModifiedBy] INT NULL,
    [IsDeleted] BIT DEFAULT ((0)) NOT NULL
);

