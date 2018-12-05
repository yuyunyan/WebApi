CREATE TABLE [codes].[lkpIncoterms] (
    [IncotermID]   INT            IDENTITY (1, 1) NOT NULL,
    [IncotermName] NVARCHAR (250) NOT NULL,
    [ExternalID]   VARCHAR (50)   NULL,
    [Created]      DATETIME       DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]    INT            NOT NULL,
    [Modified]     DATETIME       NULL,
    [ModifiedBy]   INT            NULL,
    [IsDeleted]    BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([IncotermID] ASC)
);


