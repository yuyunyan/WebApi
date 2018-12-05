CREATE TABLE [dbo].[QCInspections] (
    [InspectionID]       INT          IDENTITY (1, 1) NOT NULL,
    [StockID]        INT          NOT NULL,
    [InspectionStatusID] INT          NOT NULL,
	[InspectionTypeID]	 INT		  NOT NULL,
    [InspectionQty]      INT          NOT NULL,
    [QtyFailed]          INT          CONSTRAINT [DF__QCInspect__QtyFa__3BB5CE82] DEFAULT ((0)) NOT NULL,
    [CompletedDate]      DATETIME     NULL,
    [CompletedBy]        INT          NULL,
    [ResultID]           INT          NULL,
    [ExternalID]         VARCHAR (50) NULL,
    [Created]            DATETIME     CONSTRAINT [DF__QCInspect__Creat__3CA9F2BB] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]          INT          NOT NULL,
    [Modified]           DATETIME     NULL,
    [ModifiedBy]         INT          NULL,
    [IsDeleted]          BIT          CONSTRAINT [DF__QCInspect__IsDel__3D9E16F4] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__QCInspec__30B2DC282F32711D] PRIMARY KEY CLUSTERED ([InspectionID] ASC)
);


