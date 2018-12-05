CREATE TABLE [dbo].[Carriers] (
    [CarrierID]   INT           IDENTITY (1, 1) NOT NULL,
    [CarrierName] NVARCHAR (50) NOT NULL,
    [ExternalID]  VARCHAR (50)  NULL,
    [Created]     DATETIME      DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]   INT           NOT NULL,
    [Modified]    DATETIME      NULL,
    [ModifiedBy]  INT           NULL,
    [IsDeleted]   BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([CarrierID] ASC)
);

