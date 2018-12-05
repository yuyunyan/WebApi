CREATE TABLE [dbo].[PurchaseOrders] (
    [PurchaseOrderID]  INT            IDENTITY (100000, 1) NOT NULL,
    [VersionID]        INT            NOT NULL,
    [AccountID]        INT            NOT NULL,
    [ContactID]        INT            NOT NULL,
    [StatusID]         INT            NOT NULL,
    [FromLocationID]   INT            NULL,
    [ToWarehouseID]     INT            NOT NULL,
    [IncotermID]       INT            NULL,
    [PaymentTermID]    INT            NOT NULL,
    [CurrencyID]       CHAR (3)       NOT NULL,
    [ShippingMethodID] INT            NULL,
    [OrganizationID]   INT            NOT NULL,
    [OrderDate]        DATE           CONSTRAINT [DF__PurchaseO__Order__64A2D57C] DEFAULT (getutcdate()) NULL,
    [Created]          DATETIME       CONSTRAINT [DF__PurchaseO__Creat__6596F9B5] DEFAULT (getutcdate()) NOT NULL,
    [CreatedBy]        INT            NOT NULL,
    [Modified]         DATETIME       NULL,
    [ModifiedBy]       INT            NULL,
    [IsDeleted]        BIT            CONSTRAINT [DF__PurchaseO__IsDel__668B1DEE] DEFAULT ((0)) NOT NULL,
    [ExternalID]       VARCHAR (50)   NULL,
    [PONotes]          NVARCHAR (MAX) NULL,
    CONSTRAINT [PK__Purchase__46F0FAEBB625E3A8] PRIMARY KEY CLUSTERED ([VersionID] ASC, [PurchaseOrderID] ASC)
);


