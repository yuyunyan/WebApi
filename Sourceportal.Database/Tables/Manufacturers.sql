CREATE TABLE [dbo].[Manufacturers] (
    [MfrID]      INT            IDENTITY (1, 1) NOT NULL,
    [MfrName]    NVARCHAR (250) NOT NULL,
    [Code]       NVARCHAR (10)  NULL,
    [MfrURL]     NVARCHAR (500) NULL,
    [Created]    DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  INT            NOT NULL,
    [Modified]   DATETIME       NULL,
    [ModifiedBy] INT            NULL,
    [IsDeleted]  BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([MfrID] ASC)
);


