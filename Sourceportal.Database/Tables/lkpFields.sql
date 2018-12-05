CREATE TABLE [dbo].[lkpFields] (
    [FieldID]      INT          IDENTITY PRIMARY KEY NOT NULL,
    [NavID]		   INT          NULL,
    [FieldName]    VARCHAR (50) NOT NULL,
    [FieldType]    VARCHAR (50) NOT NULL,
    [ObjectTypeID] INT          NOT NULL,
    [DataLocation] VARCHAR (64) NULL,
    [Created]      DATETIME     DEFAULT GETUTCDATE() NOT NULL,
    [CreatedBy]    INT          NOT NULL,
    [Modified]     DATETIME     NULL,
    [ModifiedBy]   INT          NULL,
    [IsDeleted]    BIT          DEFAULT 0 NOT NULL
);

