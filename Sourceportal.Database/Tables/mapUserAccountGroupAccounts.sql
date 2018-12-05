CREATE TABLE [dbo].[mapUserAccountGroupAccounts] (
    [GroupLineID]    INT      IDENTITY (1, 1) NOT NULL,
    [AccountGroupID] INT      NOT NULL,
    [AccountID]      INT      NOT NULL,
    [ContactID]      INT      NULL,
    [Created]        DATETIME CONSTRAINT [DF_mapUserAccountGroupAccounts_Created] DEFAULT (getdate()) NOT NULL,
    [CreatedBy]      INT      CONSTRAINT [DF_mapUserAccountGroupAccounts_CreatedBy] DEFAULT ((0)) NOT NULL,
    [Modified]       DATETIME NULL,
    [ModifiedBy]     INT      NULL,
    [IsDeleted]      BIT      CONSTRAINT [DF_mapUserAccountGroupAccounts_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_mapUserAccountGroupAccounts] PRIMARY KEY CLUSTERED ([GroupLineID] ASC)
);


