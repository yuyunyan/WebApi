CREATE TABLE [dbo].[Users] (
    [UserID]         INT           IDENTITY (1, 1) NOT NULL,
    [EmailAddress]   VARCHAR (128) NULL,
    [PasswordHash]   VARCHAR (256) NULL,
    [FirstName]      VARCHAR (32)  NULL,
    [LastName]       VARCHAR (32)  NULL,
    [OrganizationID] INT           NULL,
    [IsEnabled]      BIT           DEFAULT ((1)) NOT NULL,
    [LastLogin]      DATETIME      NULL,
    [Created]        DATETIME      DEFAULT (getutcdate()) NULL,
    [CreatedBy]      INT           NULL,
    [Modified]       DATETIME      NULL,
    [ModifiedBy]     INT           NULL,
    [ExternalID]     VARCHAR (50)  NULL,
    [PhoneNumber]    VARCHAR (50)  NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC)
);



