CREATE TABLE [dbo].[ItemExtras] (
    [ItemExtraID]      INT            IDENTITY (1, 1) NOT NULL,
    [ExternalID]       VARCHAR (50)   NULL,
    [ExtraName]        NVARCHAR (50)  NOT NULL,
    [ExtraDescription] NVARCHAR (250) NULL,
    [Created]          DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT            NOT NULL,
    [Modified]         DATETIME       NULL,
    [ModifiedBy]       INT            NULL,
    [IsDeleted]        BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([ItemExtraID] ASC)
);


