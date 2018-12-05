CREATE TABLE [dbo].[lkpCompanyTypes]
(
	[CompanyTypeID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [Name] VARCHAR(64) NULL, 
    [ExternalID] VARCHAR(50) NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
