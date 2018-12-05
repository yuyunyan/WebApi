CREATE TABLE [dbo].[Emails]
(
	[EmailID] INT NOT NULL PRIMARY KEY IDENTITY,     
    [EmailTypeID] INT NOT NULL, 
    [FromAddress] VARCHAR(100) NOT NULL, 
    [ToAddresses] VARCHAR(MAX) NOT NULL, 
    [CCAddresses] VARCHAR(MAX) NULL, 
    [BCCAddresses] VARCHAR(MAX) NULL, 
    [Body] VARCHAR(MAX) NOT NULL, 
    [SentStatus] VARCHAR(50) NOT NULL DEFAULT 'Pending', 
    [ErrorMessage] VARCHAR(MAX) NULL,
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL
)
