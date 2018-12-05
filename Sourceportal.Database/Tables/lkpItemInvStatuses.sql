CREATE TABLE [dbo].[lkpItemInvStatuses] (
    [InvStatusID] INT           IDENTITY (1, 1) NOT NULL,
    [StatusName]  NVARCHAR (50) NOT NULL,
    [ExternalID]  VARCHAR (50)  NULL,
    [Created]     DATETIME      CONSTRAINT [DF__lkpItemIn__Creat__469D7149] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]   INT           NOT NULL,
    [Modified]    DATETIME      NULL,
    [ModifiedBy]  INT           NULL,
    [IsDeleted]   BIT           CONSTRAINT [DF__lkpItemIn__IsDel__47919582] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__lkpItemI__D8B3FF3FA902220D] PRIMARY KEY CLUSTERED ([InvStatusID] ASC)
);


