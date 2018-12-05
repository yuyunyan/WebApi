CREATE TABLE [dbo].[mapAccountFocuses](
	[FocusID] [int] IDENTITY(1,1) NOT NULL,
	[AccountID] [int] NOT NULL,
	[FocusTypeID] [int] NOT NULL,
	[ObjectID] [int] NOT NULL,
	[FocusObjectTypeID] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[Modified] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[FocusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[mapAccountFocuses] ADD  DEFAULT (getutcdate()) FOR [Created]
GO

ALTER TABLE [dbo].[mapAccountFocuses] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO