CREATE TABLE [dbo].[CarrierMethods] (
    [MethodID]   INT           IDENTITY (1, 1) NOT NULL,
    [CarrierID]  INT           NOT NULL,
    [MethodName] NVARCHAR (50) NOT NULL,
    [TransitDays] INT		   NULL,
	[ExternalID] VARCHAR (50)  NULL,	
    [Created]    DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  INT           NOT NULL,
    [Modified]   DATETIME      NULL,
    [ModifiedBy] INT           NULL,
    [IsDeleted]  BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([MethodID] ASC)
);

