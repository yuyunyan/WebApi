CREATE TABLE [dbo].[mapContactProjects]
(
	[ContactID] INT NOT NULL, 
    [ProjectID] INT NOT NULL,
	PRIMARY KEY ([ContactID], [ProjectID]),
	CreatedBy int NOT NULL DEFAULT 0,
	Created datetime NOT NULL DEFAULT (getdate()),
	ModifiedBy int NULL,
	Modified datetime NULL,
	IsDeleted bit NOT NULL DEFAULT 0
)