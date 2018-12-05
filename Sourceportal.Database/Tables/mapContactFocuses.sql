CREATE TABLE [dbo].[mapContactFocuses] (
    [ContactID]  INT      NOT NULL,
    [FocusID]    INT      NOT NULL,
    [CreatedBy]  INT      CONSTRAINT [DF_mapContactFocuses_CreatedBy] DEFAULT ((0)) NOT NULL,
    [Created]    DATETIME CONSTRAINT [DF_mapContactFocuses_Created] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy] INT      NULL,
    [Modified]   DATETIME NULL,
    [IsDeleted]  BIT      CONSTRAINT [DF_mapContactFocuses_IsDeleted] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_mapContactFocuses] PRIMARY KEY CLUSTERED ([ContactID] ASC, [FocusID] ASC)
);


