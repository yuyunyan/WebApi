
CREATE TABLE [dbo].[Searches](
	[SearchID] [int] IDENTITY(1,1) NOT NULL,
	[SearchType] [char](1) NOT NULL,
	[DateStart] [date] NOT NULL,
	[DateEnd] [date] NOT NULL,
	[SearchSalesOrders] [bit] NOT NULL DEFAULT 1,
	[SearchInventory] [bit] NOT NULL DEFAULT 1,
	[SearchPurchaseOrders] [bit] NOT NULL DEFAULT 1,
	[SearchVendorRFQs] [bit] NOT NULL DEFAULT 1,
	[SearchQuotes] [bit] NOT NULL DEFAULT 1,
	[SearchCustomerRFQs] [bit] NOT NULL DEFAULT 1,
	[SearchOutsideOffers] [bit] NOT NULL DEFAULT 1,
	[SearchItemLists] [bit] NOT NULL DEFAULT 1,
	[IgnoreEPDS] [bit] NOT NULL DEFAULT 0,
	[Created] [datetime] NOT NULL DEFAULT getutcdate(),
	[CreatedBy] [int] NOT NULL,
	[Modified] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[IsDeleted] [bit] NOT NULL DEFAULT 0,
PRIMARY KEY CLUSTERED 
(
	[SearchID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
