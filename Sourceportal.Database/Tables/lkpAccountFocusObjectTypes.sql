CREATE TABLE [dbo].[lkpAccountFocusObjectTypes](
	[FocusObjectTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ObjectTypeID] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Modified] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [PK__lkpAccou__24957530EF48DF78] PRIMARY KEY CLUSTERED 
(
	[FocusObjectTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[lkpAccountFocusObjectTypes] ADD  CONSTRAINT [DF__lkpAccoun__Creat__1C9228E4]  DEFAULT (getutcdate()) FOR [Created]
GO

ALTER TABLE [dbo].[lkpAccountFocusObjectTypes] ADD  CONSTRAINT [DF__lkpAccoun__IsDel__1D864D1D]  DEFAULT ((0)) FOR [IsDeleted]
GO