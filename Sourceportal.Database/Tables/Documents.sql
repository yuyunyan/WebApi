

CREATE TABLE [dbo].[Documents](
	[DocumentID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectTypeID] [int] NOT NULL,
	[ObjectID] [int] NOT NULL,
	[DocName] [varchar](64) NULL,
	[FileNameOriginal] [varchar](256) NULL,
	[FileNameStored] [varchar](256) NOT NULL,
	[FileMimeType] [varchar](256) NULL,
	[DocumentTypeID] [int] NULL,
	[FolderPath] [varchar](255) NULL,
	[ExternalID] [varchar](50) NULL,
	[IsDeleted] [bit] NULL,
	[Created] [datetime] NOT NULL,
	[IsSystem] [bit] NOT NULL,
 CONSTRAINT [PK__Document__1ABEEF6FFCC02D6A] PRIMARY KEY CLUSTERED 
(
	[DocumentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF__Documents__IsDel__390E6C01]  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Documents] ADD  CONSTRAINT [DF__Documents__Creat__224B023A]  DEFAULT (getutcdate()) FOR [Created]
GO

ALTER TABLE [dbo].[Documents] ADD  DEFAULT ((0)) FOR [IsSystem]
GO


