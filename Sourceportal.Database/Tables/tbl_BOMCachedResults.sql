CREATE TABLE [dbo].[tbl_BOMCachedResults] (
    [CacheID]         BIGINT         IDENTITY (1, 1) NOT NULL,
    [DatabaseID]      INT            NOT NULL,
    [RecordID]        INT            NOT NULL,
    [LineID]          INT            NOT NULL,
    [TypeID]          NCHAR (1)      NOT NULL,
    [PartNumberStrip] NVARCHAR (250) NOT NULL,
    [Manufacturer]    NVARCHAR (250) NULL,
    [AccountName]     NVARCHAR (250) NULL,
    [Price]           FLOAT (53)     NULL,
    [RecDate]         DATE           NULL,
    [Version]         INT            NOT NULL,
    CONSTRAINT [PK_tbl_CacheData] PRIMARY KEY NONCLUSTERED ([CacheID] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Index_AccountName]
    ON [dbo].[tbl_BOMCachedResults]([Version] ASC, [AccountName] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Index_Manufacturer]
    ON [dbo].[tbl_BOMCachedResults]([Version] ASC, [Manufacturer] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Index_PartNumberStrip]
    ON [dbo].[tbl_BOMCachedResults]([Version] ASC, [PartNumberStrip] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Index_Version]
    ON [dbo].[tbl_BOMCachedResults]([Version] ASC);

