CREATE TABLE [dbo].[Countries] (
    [CountryID]    INT          IDENTITY (1, 1) NOT NULL,
    [CountryName]  VARCHAR (64) NULL,
    [CountryCode]  VARCHAR (6)  NULL,
    [CountryCode2] VARCHAR (2)  NULL,
    PRIMARY KEY CLUSTERED ([CountryID] ASC)
);


