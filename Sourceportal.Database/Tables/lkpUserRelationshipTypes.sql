﻿CREATE TABLE [dbo].[lkpUserRelationshipTypes] (
    [UserRelationshipTypeID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]                   VARCHAR (128) NULL,
    [Created]                DATETIME      DEFAULT (getdate()) NULL,
    [CreatedBy]              INT           NULL,
    [Modified]               DATETIME      NULL,
    [ModifiedBy]             INT           NULL,
    [IsDeleted]              BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserRelationshipTypeID] ASC)
);

