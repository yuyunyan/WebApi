﻿CREATE TABLE [dbo].[lkpRouteStatuses]
(
	[RouteStatusID] INT NOT NULL PRIMARY KEY IDENTITY, 
    [StatusName] NVARCHAR(50) NOT NULL,
	[IsDefault] BIT NOT NULL DEFAULT 0,
	[IsComplete] BIT NOT NULL DEFAULT 0,
	[Icon] VARCHAR(50) NOT NULL,
	[IconColor] VARCHAR(6) NOT NULL DEFAULT '999999',
	[Created] DATETIME NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] INT NOT NULL, 
    [Modified] DATETIME NULL, 
    [ModifiedBy] INT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0
)