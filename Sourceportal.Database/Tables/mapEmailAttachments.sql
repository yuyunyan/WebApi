CREATE TABLE [dbo].[mapEmailAttachments]
(
	[EmailID] INT NOT NULL , 
    [DocumentID] INT NOT NULL, 
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT DEFAULT 0 NOT NULL,
    PRIMARY KEY ([EmailID], [DocumentID])
)
