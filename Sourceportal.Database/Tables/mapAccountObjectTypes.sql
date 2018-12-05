CREATE TABLE [dbo].[mapAccountObjectTypes] (
    [AccountTypeID] INT NOT NULL,
    [ObjectTypeID]  INT NOT NULL,
    CONSTRAINT [PK_mapAccountObjectTypes] PRIMARY KEY CLUSTERED ([AccountTypeID] ASC, [ObjectTypeID] ASC)
);


