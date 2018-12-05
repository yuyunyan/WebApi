CREATE TABLE [dbo].[lkpLocationTypes]
(
	[LocationTypeID] INT IDENTITY(1,1) NOT NULL PRIMARY KEY, 
    [IsStatic] BIT NULL, 
    [Name] VARCHAR(64) NULL,
	[ExternalID] VARCHAR(50) NULL
)
