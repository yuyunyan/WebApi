
CREATE TABLE [dbo].[BOMCacheDataPre](
	[DatabaseID] [int] NULL,
	[RecordID] [int] NULL,
	[LineID] [int] NULL,
	[TypeID] [char](1) NULL,
	[PartNumberStrip] [varchar](250) NULL,
	[Manufacturer] [varchar](250) NULL,
	[AccountName] [varchar](250) NULL,
	[Price] [float] NULL,
	[RecDate] [date] NULL
) ON [PRIMARY]

GO

