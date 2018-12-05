CREATE TABLE [dbo].[mapStateEngineDynamicValues] (
    [ValueMapID] INT      IDENTITY (1, 1) NOT NULL,
    [ValueID]    INT      NOT NULL,
    [MapType]    CHAR (1) NOT NULL,
    [ObjectID]   INT      NOT NULL,
    [Created]    DATETIME CONSTRAINT [DF__mapStateE__Creat__6C79016E] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]  INT      NOT NULL,
    [Modified]   DATETIME NULL,
    [ModifiedBy] INT      NULL,
    [IsDeleted]  BIT      CONSTRAINT [DF__mapStateE__IsDel__6D6D25A7] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__mapState__1FB5AC8D2FE0A982] PRIMARY KEY CLUSTERED ([ValueMapID] ASC)
);


