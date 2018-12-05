CREATE TABLE [dbo].[tempSapRequests] (
    [ID]      INT            IDENTITY (1, 1) NOT NULL,
    [Created] DATETIME       CONSTRAINT [DF_tempSapRequests] DEFAULT (getutcdate()) NULL,
    [Request] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_tempSapRequests] PRIMARY KEY CLUSTERED ([ID] ASC)
);

