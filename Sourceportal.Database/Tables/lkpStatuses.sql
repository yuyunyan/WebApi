CREATE TABLE [dbo].[lkpStatuses] (
    [StatusID]           INT           IDENTITY (1, 1) NOT NULL,
    [ObjectTypeID]       INT           NOT NULL,
    [StatusName]         NVARCHAR (50) NOT NULL,
    [IsDefault]          BIT           DEFAULT ((0)) NOT NULL,
    [IsAwaitingApproval] BIT           DEFAULT ((0)) NOT NULL,
    [IsApproved]         BIT           DEFAULT ((0)) NOT NULL,
    [IsComplete]         BIT           DEFAULT ((0)) NOT NULL,
    [IsCanceled]         BIT           DEFAULT ((0)) NOT NULL,
    [ExternalID]         VARCHAR (50)  NULL,
    [Created]            DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]          INT           NOT NULL,
    [Modified]           DATETIME      NULL,
    [ModifiedBy]         INT           NULL,
    [IsDeleted]          BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([StatusID] ASC)
);


