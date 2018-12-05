
CREATE TABLE [dbo].[BOMCachedResults]
(
	[CacheID] [bigint] IDENTITY(1,1) NOT NULL,
	[DatabaseID] [int] NOT NULL,
	[RecordID] [int] NOT NULL,
	[LineID] [int] NOT NULL,
	[TypeID] [nchar](1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[PartNumberStrip] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Manufacturer] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[AccountName] [nvarchar](250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Price] [float] NULL,
	[RecDate] [date] NULL,
	[Version] [int] NOT NULL,

INDEX [IX_Index_AccountName] NONCLUSTERED 
(
	[Version] ASC,
	[AccountName] ASC
),
INDEX [IX_Index_Manufacturer] NONCLUSTERED 
(
	[Version] ASC,
	[Manufacturer] ASC
),
INDEX [IX_Index_PartNumberStrip] NONCLUSTERED 
(
	[Version] ASC,
	[PartNumberStrip] ASC
),
INDEX [IX_Index_Version] NONCLUSTERED 
(
	[Version] ASC
),
 CONSTRAINT [PK_CacheData]  PRIMARY KEY NONCLUSTERED 
(
	[CacheID] ASC
)
)WITH ( MEMORY_OPTIMIZED = ON , DURABILITY = SCHEMA_AND_DATA )
