CREATE TABLE [dbo].[States] (
    [StateID]   INT           IDENTITY (1, 1) NOT NULL,
    [StateName] VARCHAR (128) NULL,
    [StateCode] VARCHAR (8)   NULL,
    [CountryID] INT           NULL,
    CONSTRAINT [PK__States__C3BA3B5A148D4ECF] PRIMARY KEY CLUSTERED ([StateID] ASC)
);


