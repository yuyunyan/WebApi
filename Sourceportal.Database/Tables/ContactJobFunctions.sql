CREATE TABLE dbo.ContactJobFunctions
	(
	JobFunctionID int identity(1,1) NOT NULL,
	JobFunctionName varchar(256) NOT NULL,
	ExternalID varchar(50) NULL,
	CreatedBy int NOT NULL,
	Created datetime NOT NULL DEFAULT GETUTCDATE(),
	ModifiedBy int NULL,
	Modified datetime NULL,
	IsDeleted bit NOT NULL DEFAULT 0
	)  ON [PRIMARY]

GO
ALTER TABLE dbo.ContactJobFunctions ADD CONSTRAINT
	PK_ContactJobFunctions PRIMARY KEY CLUSTERED 
	(
	JobFunctionID
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ContactJobFunctions SET (LOCK_ESCALATION = TABLE)
