
-- =============================================
/*
 Author:			Aaron Rodecker
 Create date:		2017.09.08
 Description:		BOM Matching Procedure - Made from SPC-spBomProcessMatch
 Usage:				EXEC uspBOMProcessMatch @PartNumbers  = '{"PartNumber": "BAV99"}', @UserID = 1, @SearchType = 'X', @DateStart='2017-01-01 00:00:00',@DateEnd='2017-09-22 00:00:00', @MatchQuote = 1, @MatchSO = 1, @MatchPO = 1, @MatchInventory = 1, @MatchOffers = 1, @MatchRFQ = 1, @MatchBOM = 1, @MatchCustomerQuote = 1, @MatchItemLists = 0
					EXEC uspBOMProcessMatch @ItemListID = 78, @PartNumbers  = NULL, @UserID = 1, @SearchType = 'p', @DateStart='2017-01-01 00:00:00',@DateEnd='2017-09-22 00:00:00', @MatchQuote = 1, @MatchSO = 1, @MatchPO = 1, @MatchInventory = 1, @MatchOffers = 1, @MatchRFQ = 1, @MatchBOM = 1, @MatchCustomerQuote = 1, @MatchItemLists = 1
					exec uspBOMProcessMatch @ItemListID=0, @PartNumbers=N'[{"PartNumber":"bav99"}]', @Manufacturer=0, @AccountName=0,@SearchType=N'P',@DateStart='2017-03-22 00:00:00',@DateEnd='2017-09-22 00:00:00',@MatchQuote=0,@MatchSO=0,@MatchPO=1,@MatchInventory=0,@MatchOffers=0,@MatchRFQ=0,@MatchBOM=0,@MatchCustomerQuote=0,@UserID=0
					EXEC uspBOMProcessMatch @PartNumbers  = '{"PartNumber": "BAV99"}', @UserID = 1, @SearchType = 'p', @DateStart='2016-01-01 00:00:00',@DateEnd='2017-10-13 00:00:00', @MatchQuote = 1, @MatchSO = 1, @MatchPO = 1, @MatchInventory = 1, @MatchOffers = 1, @MatchRFQ = 1, @MatchBOM = 1, @MatchCustomerQuote = 1, @MatchItemLists = 0
					SELECT  top 10 * FROM Searches order by SearchID desc
					SELECT top 10 * FROM SearchResults --WHERE SearchID = 1092
					SELECT * FROM ItemInventory
					SELECT top 100* FROM BomCachedResults
					EXEX uspBOMProcessMatch @Manufacturer='3D Plus' 
		
 Revision History:	
	 2017.09.21		AR		Changed [AccountID]  from int to varchar
	 2017.09.26		AR		Added dbo.fnStripNonAlphaNumeric to part number in all grids
	 2017.10.25		AR		Added BOM Columns
	 2017.11.03		AR		Added Buyer to Vendor RFQ, inserting SearchDetailID into SearchResults tbl
	 2017.12.15		AR		Added B to "@SearchType IN ('P','L','X','B')" so that @listID would be used if bom search type, removed AND @SaerchType = 'L', added PartNumberStrip INSERT to SaerchValue for ItemList INSERT
	 2018.01.03		AR		Added create_dt to PurchaseOrder and SalesOrder grid (missing)
	 2018.01.10		AR		Added Buyer = rd.create_id for Vendor RFQ
 exec uspBOMProcessMatch @ItemListID=0,@PartNumbers=N'[{"PartNumber":"ad8"}]',@Manufacturer=0,@AccountName=0,@SearchType=N'P',@DateStart='2017-01-01 00:00:00',@DateEnd='2017-09-26 00:00:00',@MatchQuote=1,@MatchSO=1,@MatchPO=1,@MatchInventory=1,@MatchOffers=1,@MatchRFQ=1,@MatchBOM=1,@MatchCustomerQuote=1,@UserID=0
-*/


--SELECT * FROM ItemListLines WHERE ItemListID = 78

-- =============================================
CREATE PROCEDURE [dbo].[uspBOMProcessMatch]
	@ItemListID				int	= NULL, 
	@PartNumbers			VARCHAR(MAX) = NULL,
	@Manufacturer			VARCHAR(64) = NULL,
	@AccountName			VARCHAR(64) = NULL,
	@UserID					int	= NULL,
	@SearchType				char(1)	= 'L',
	@DateStart				date = '01/01/1974',
	@DateEnd				date = '01/01/1974',
	@MatchQuote				bit = 0, 
	@MatchSO				bit = 0, 
	@MatchPO				bit = 0, 
	@MatchInventory			bit = 0,
	@MatchOffers			bit = 0,
	@MatchRFQ				bit = 0,
	@MatchBOM				bit = 0,
	@MatchCustomerQuote		bit = 0,
	@MatchItemLists			bit = 0,
	@IgnoreEPDS				bit = 0,
	@SearchID				int = NULL OUTPUT

AS
BEGIN
	SET NOCOUNT ON;

	--IF (SELECT COUNT(*) FROM bom.MatchExecResults WHERE MatchHeaderID = @MatchHeaderID) > 0 
	--BEGIN
	--	UPDATE bom.MatchExecResults
	--	SET		StartTime = GETDATE(),
	--			EndTime = NULL,
	--			TimeToComplete = NULL
	--	WHERE	MatchHeaderID = @MatchHeaderID

	--	DELETE FROM bom.MatchDetail WHERE MatchHeaderID = @MatchHeaderID
	--END
	--ELSE
	--BEGIN
	--	INSERT INTO bom.MatchExecResults (MatchHeaderID, StartTime)
	--	VALUES	(@MatchHeaderID, GETDATE())
	--END
		--DECLARE @ListHeaderID	int		= 0

	--Testing Declaration
	--DECLARE @PartNumbers VARCHAR(MAX) = '{PartNumbers: "BAV99" }', @Manufacturer VARCHAR(500) = NULL, @AccountName VARCHAR(500) = NULL, @UserID INT = 1, @SearchType VARCHAR(10) = 'L', @DateStart DATETIME = '01/01/2017', @DateEnd DATETIME = '06/01/2017', @MatchQuote BIT = 1, @MatchSO BIT = 1, @MatchPO BIT = 1, @MatchInventory BIT = 1, @MatchOffers BIT = 1, @MatchRFQ BIT = 1, @MatchBOM BIT = 1, @MatchCustomerQuote BIT= 1, @MatchItemLists BIT = 1, @IgnoreEPDS BIT = 0
	DECLARE	@TmpVal int = (SELECT TOP (1) CurrentVersion FROM logBOMCacheVersion)
	    
	DECLARE	@TempTable TABLE
	(
		--[MatchHeaderID] [int],
		[MatchTypeID] [nvarchar](5) NULL,
		[DatabaseID] [int] NULL,
		[ListDetailID] [int] NULL,
		[RecordID] [int] NULL,
		[LineID] [int] NULL,
		[MfgPartNumber] [nvarchar](250) NULL,
		[Manufacturer] [nvarchar](250) NULL,
		[IntPartNumber] [nvarchar](250) NULL,
		[Quantity] [int] NULL,
		[QuantityDelta] [int] NULL,
		[Price] [float] NULL,
		[PriceDelta] [float] NULL,
		[Potential] [float] NULL,
		[DateCode] [nvarchar](250) NULL,
		[LeadTime] [nvarchar](250) NULL,
		[DueDate] [date] NULL,
		[UnitCost] [float] NULL,
		[LineGP] [float] NULL,
		[Quantity2] [int] NULL,
		[WarehouseID] INT NULL,
		[WarehouseCode] [VARCHAR](32) NULL,
		[AccountID] [VARCHAR](32) NULL,
		[AccountName] [nvarchar](500) NULL,
		[ContactID] INT NULL,
		[ContactName] [nvarchar](500) NULL,
		[RepName] [nvarchar](50) NULL,
		[Notes] [nvarchar](2000) NULL,
		[RecDate] [datetime] NULL,
		[ExactMatch] [bit] NULL,
		[CreateUserID] INT NULL,
		[UpdateUserID] INT NULL,
		[Cost] [float] NULL,
		[QuotedPrice] [float] NULL,
		[TargetPrice] [float] NULL,
		[ShippedQty] [int] NULL,
		[OrderStatus] [nvarchar](200) NULL,
		[SalesPerson] VARCHAR(64) NULL,
		[SalesPersonID] INT NULL,
		[Buyer] VARCHAR(64) NULL,
		[SONumber] [int] NULL,
		[SODate] [datetime] null,
		[ReceivedDate] [datetime] NULL,
		[ReceivedQty] [int] NULL,
		[Customer] [nvarchar](50) NULL,
		--[POStatus] [nvarchar](50) NULL,
		[ReservedQty] [int] NULL,
		[AvailableQty] [int] NULL,
		[PONumber] [int] NULL,
		--[tmpCost] [float] NULL,
		[ItemID] [INT]  NULL,
		[OrderDate] [DATETIME] NULL,
		[VersionID] [INT]  NULL,

		/*Bom Columns*/
		[BOMPartNumber] VARCHAR(32),
		[BOMIntPartNumber] VARCHAR(32),
		[BOMMfg] VARCHAR(32),
		[BOMQty] VARCHAR(32),
		[BOMPrice] VARCHAR(32),
		[SearchDetailID] INT
	)
	
	--@ItemListID ?
	--@MatchBOM

	INSERT INTO Searches (SearchType, DateStart, DateEnd, SearchSalesOrders, SearchInventory, SearchPurchaseOrders,SearchVendorRFQs,
							SearchQuotes, SearchCustomerRFQs, SearchOutsideOffers, SearchItemLists, IgnoreEPDS, CreatedBy)
	VALUES (@SearchType, @DateStart, @DateEnd, @MatchSO, @MatchInventory, @MatchPO, @MatchRFQ, @MatchQuote
			, @MatchCustomerQuote, @MatchOffers, @MatchItemLists, @IgnoreEPDS, @UserID )

	SET @SearchID = @@identity
	--SELECT	@ListHeaderID = bom.MatchHeader.ListHeaderID, @DateStart = bom.MatchHeader.DateStart, @DateEnd = bom.MatchHeader.DateEnd, 
	--		@Variance = bom.MatchHeader.Variance, @SearchType = bom.ListHeader.HeaderType
	--FROM    bom.MatchHeader WITH (NOLOCK) INNER JOIN
 --           bom.ListHeader WITH (NOLOCK) ON bom.MatchHeader.ListHeaderID = bom.ListHeader.ListHeaderID
	--WHERE   (bom.MatchHeader.MatchHeaderID = @MatchHeaderID)
	
	--SELECT	@MatchQuote = MatchQuote, @MatchSO = MatchSO, @MatchPO= MatchPO, @MatchInventory= MatchInventory, 
	--		@MatchOffers = MatchOffers, @MatchRFQ = MatchRFQ, @MatchBOM = MatchBOM, @MatchCustomerQuote = MatchCustomerQuote	
	--FROM    bom.MatchHeader WITH (NOLOCK)
	--WHERE	(MatchHeaderID = @MatchHeaderID)
		
	IF @SearchType IN ('P','L','X','B')
	BEGIN
	
		--Handle for item list ID passed in
		IF (@ItemListID > 0)
		BEGIN
			INSERT INTO SearchDetails (SearchID, ItemListLineID, SearchValue )
			SELECT @SearchID, ItemListLineID, PartNumberStrip
			FROM ItemListLines  WHERE ItemListID = @ItemListID
		END
		ELSE BEGIN
			INSERT INTO SearchDetails (SearchID, SearchValue ) --ItemListLineID for list uploaded?
			SELECT @SearchID, PartNumber
			FROM OPENJSON(@PartNumbers)
			WITH  (PartNumber VARCHAR(64) '$.PartNumber')
		END
		INSERT INTO @TempTable ( DatabaseID, RecordID, LineID, MatchTypeID, RecDate, Cost /*tmpCost */, MfgPartNumber, Manufacturer, AccountName) -- MatchHeaderID, ListDetailID,
		
		SELECT  R.DatabaseID, R.RecordID, R.LineID, R.TypeID, R.RecDate, R.Price, R.PartNumberStrip, Manufacturer, AccountName -- @MatchHeaderID, R.ListDetailID,
		--FROM	bom.ListDetail WITH (NOLOCK)
		FROM SearchDetails SD WITH (NOLOCK)
		INNER JOIN BOMCachedResults R ON LEFT(R.PartNumberStrip,LEN(SD.SearchValue)) = SD.SearchValue
		WHERE SD.SearchID = @SearchID
		--AND R.Version = @TmpVal	--This breaks the query. moving down below

		--WHERE	R.ListHeaderID = @ListHeaderID AND R.Version = @TmpVal
	END

	IF @SearchType = 'M'
	BEGIN
		INSERT INTO SearchDetails (SearchID, SearchValue ) --ItemListLineID for list uploaded?
		VALUES (@SearchID, @Manufacturer)

		INSERT INTO @TempTable (DatabaseID, RecordID, LineID, MatchTypeID, RecDate, Cost /*tmpCost */, MfgPartNumber, Manufacturer, AccountName ) -- MatchHeaderID, ListDetailID,
		SELECT  R.DatabaseID, R.RecordID, R.LineID, R.TypeID, R.RecDate, R.Price, R.PartNumberStrip, R.Manufacturer, AccountName  -- @MatchHeaderID, bom.ListDetail.ListDetailID,
		FROM	SearchDetails SD WITH (NOLOCK)
		INNER JOIN  BOMCachedResults R ON LEFT(R.Manufacturer,LEN(SD.SearchValue)) = SD.SearchValue-- LIKE '%' + SD.SearchValue + '%'
		WHERE SD.SearchID = @SearchID
		--AND R.Version = @TmpVal	--This breaks the query. moving down below

		--FROM	bom.ListDetail WITH (NOLOCK)
				--INNER JOIN BOMCachedResults R ON R.Manufacturer LIKE bom.ListDetail.Manufacturer + '%' AND R.Version = @TmpVal
		--WHERE	bom.ListDetail.ListHeaderID = @ListHeaderID 
	END

	IF @SearchType = 'A'
	BEGIN
		INSERT INTO SearchDetails (SearchID, SearchValue ) --ItemListLineID for list uploaded?
		VALUES (@SearchID, @AccountName)

		INSERT INTO @TempTable (DatabaseID, RecordID, LineID, MatchTypeID, RecDate, Cost /*tmpCost */, MfgPartNumber, Manufacturer, AccountName) --ListDetailID,
		SELECT R.DatabaseID, R.RecordID, R.LineID, R.TypeID, R.RecDate, R.Price, R.PartNumberStrip, Manufacturer, AccountName --bom.ListDetail.ListDetailID,
		FROM	SearchDetails SD WITH (NOLOCK)
		INNER JOIN BOMCachedResults R ON R.AccountName LIKE '%' + SD.SearchValue + '%'
		WHERE SD.SearchID = @SearchID
		--AND R.Version = @TmpVal	--This breaks the query. moving down below
		--WHERE	bom.ListDetail.ListHeaderID = @ListHeaderID 
	END
	DELETE @TempTable
	WHERE VersionID != @TmpVal

	IF @MatchQuote = 0
	BEGIN
		DELETE FROM @TempTable WHERE MatchTypeID = 'Q'
	END
	
	IF @MatchSO = 0
	BEGIN
		DELETE FROM @TempTable WHERE MatchTypeID = 'S'
	END
	
	IF @MatchPO = 0
	BEGIN
		DELETE FROM @TempTable WHERE MatchTypeID = 'P'
	END
	
	IF @MatchInventory = 0
	BEGIN
		DELETE FROM @TempTable WHERE MatchTypeID = 'I'
	END
	
	IF @MatchOffers = 0
	BEGIN
		DELETE FROM @TempTable WHERE MatchTypeID = 'O'
	END
	
	IF @MatchRFQ = 0
	BEGIN
		DELETE FROM @TempTable WHERE MatchTypeID = 'R'
	END
	
	IF @MatchBOM = 0
	BEGIN
		DELETE FROM @TempTable WHERE MatchTypeID = 'B'
	END
	
	IF @MatchCustomerQuote = 0
	BEGIN
		DELETE FROM @TempTable WHERE MatchTypeID = 'C'
	END
	
	DELETE FROM @TempTable
	WHERE (RecDate NOT BETWEEN @DateStart AND @DateEnd) AND (MatchTypeID <> 'I')

	--DELETE FROM @TempTable
	--WHERE ListDetailID = RecordID and MatchTypeID = 'B'

	--IF @Variance > 0 AND @Variance < 100
	--BEGIN
	--	SET @VarianceLow = (100 - @Variance) / 100
	--	SET @VarianceHigh = (@Variance + 100) / 100

	--	DELETE FROM @TempTable 
	--	FROM	@TempTable tt INNER JOIN
	--			bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
	--	WHERE ((COALESCE(tt.tmpCost, 0) <= (COALESCE(ld.TargetPrice, 999999999) * @VarianceLow)) AND 
	--	      (COALESCE(tt.tmpCost, 999999999) >= (COALESCE(ld.TargetPrice, 0) * @VarianceHigh)))
	--END
	
	IF (@IgnoreEPDS != 1)
	BEGIN
	-- ########## GET SALES ORDER RECORD DETAILS ##########
	
		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(od.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(od.manu_no,tt.Manufacturer),
				Quantity = od.ord_qty,
				SODate = od.create_dt,
				Price = od.unit_price,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(od.unit_price, 0), --ld.TargetPrice
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(od.unit_price, 0)), --(COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0)
				UnitCost = od.unit_cost,
				LineGP = (COALESCE(od.unit_price, 0) - COALESCE(od.unit_cost, 0)) * COALESCE(od.ord_qty, 0),
				--AccountID = oh.cust_no,
				AccountName = c.cust_name,
				DateCode = od.req_dc,
				ExactMatch = CASE WHEN od.item_no = sd.SearchValue  THEN 1 ELSE 0 END, --CASE WHEN od.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				DueDate = od.due_date,
				ShippedQty = Trx.trx_qty_SUM,
				OrderStatus =  oh.status,
				SalesPerson = oh.slsman_1,
				SONumber = oh.order_no,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds01.dbo.ord_detl od ON tt.RecordID = od.order_no AND tt.LineID = od.lin_no
				INNER JOIN  SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN epds01.dbo.ord_hedr oh ON od.order_no = oh.order_no
				INNER JOIN epds01.dbo.customer c ON c.cust_no = oh.cust_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
				OUTER APPLY (SELECT SUM(TR.trx_qty) trx_qty_SUM FROM epds01.dbo.inv_trx TR
								INNER JOIN epds01.dbo.lot_hedr H ON TR.lot_no = H.lot_no
								INNER JOIN epds01.dbo.lot_info I ON H.li_int_id = I.li_int_id
								WHERE Tr.order_no =  od.order_no
								AND od.lin_no = TR.lin_no) TRX
		WHERE	(tt.DatabaseID = 1) AND (tt.MatchTypeID = 'S')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(od.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(od.manu_no,tt.Manufacturer),
				Quantity = od.ord_qty,
				SODate = od.create_dt,
				Price = od.unit_price,
				PriceDelta =  COALESCE(tt.TargetPrice, 0) - COALESCE(od.unit_price, 0), -- COALESCE(ld.TargetPrice, 0) - COALESCE(od.unit_price, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(od.unit_price, 0)), -- (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(od.unit_price, 0)),
				UnitCost = od.unit_cost,
				LineGP = (COALESCE(od.unit_price, 0) - COALESCE(od.unit_cost, 0)) * COALESCE(od.ord_qty, 0),
				--AccountID = oh.cust_no,
				AccountName = c.cust_name,
				DateCode = od.req_dc,
				ExactMatch = CASE WHEN od.item_no = SD.SearchValue THEN 1 ELSE 0 END, --CASE WHEN od.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				DueDate = od.due_date,
				ShippedQty = Trx.trx_qty_SUM,
				OrderStatus =  oh.status,
				SalesPerson = oh.slsman_1,
				SONumber = oh.order_no,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds02.dbo.ord_detl od ON tt.RecordID = od.order_no AND tt.LineID = od.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN  SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds02.dbo.ord_hedr oh ON od.order_no = oh.order_no
				INNER JOIN epds02.dbo.customer c ON c.cust_no = oh.cust_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
				OUTER APPLY (SELECT SUM(TR.trx_qty) trx_qty_SUM FROM epds02.dbo.inv_trx TR
								INNER JOIN epds02.dbo.lot_hedr H ON TR.lot_no = H.lot_no
								INNER JOIN epds02.dbo.lot_info I ON H.li_int_id = I.li_int_id
								WHERE Tr.order_no =  od.order_no
								AND od.lin_no = TR.lin_no) TRX
		WHERE	(tt.DatabaseID = 2) AND (tt.MatchTypeID = 'S')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(od.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(od.manu_no,tt.Manufacturer),
				Quantity = od.ord_qty,
				SODate = od.create_dt,
				Price = od.unit_price,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(od.unit_price, 0), --COALESCE(ld.TargetPrice, 0) - COALESCE(od.unit_price, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(od.unit_price, 0)), --(COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(od.unit_price, 0)),
				UnitCost = od.unit_cost,
				LineGP = (COALESCE(od.unit_price, 0) - COALESCE(od.unit_cost, 0)) * COALESCE(od.ord_qty, 0),
				--AccountID = oh.cust_no,
				AccountName = c.cust_name,
				DateCode = od.req_dc,
				ExactMatch = CASE WHEN od.item_no = SD.SearchValue THEN 1 ELSE 0 END, -- CASE WHEN od.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				DueDate = od.due_date,
				ShippedQty = Trx.trx_qty_SUM,
				OrderStatus =  oh.status,
				SalesPerson = oh.slsman_1,
				SONumber = oh.order_no,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds03.dbo.ord_detl od ON tt.RecordID = od.order_no AND tt.LineID = od.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN  SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds03.dbo.ord_hedr oh ON od.order_no = oh.order_no
				INNER JOIN epds03.dbo.customer c ON c.cust_no = oh.cust_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
				OUTER APPLY (SELECT SUM(TR.trx_qty) trx_qty_SUM FROM epds03.dbo.inv_trx TR
								INNER JOIN epds03.dbo.lot_hedr H ON TR.lot_no = H.lot_no
								INNER JOIN epds03.dbo.lot_info I ON H.li_int_id = I.li_int_id
								WHERE Tr.order_no =  od.order_no
								AND od.lin_no = TR.lin_no) TRX
		WHERE	(tt.DatabaseID = 3) AND (tt.MatchTypeID = 'S')


		INSERT INTO @TempTable (ItemID, Manufacturer, SONumber, RecordID, SODate, AccountName, MfgPartNumber, Quantity, Price, DateCode, Cost, DueDate, ShippedQty, OrderStatus, LineGP, SalesPerson, MatchTypeID, DatabaseID, BOMPartNumber, BOMIntPartNumber, BOMMfg, BOMQty, BOMPrice, PriceDelta, Potential, SearchDetailID)
		SELECT
			F.ItemID,
			MfrName Manufacturer,
			R.SalesOrderID OrderNumber,
			R.SOLineID RecordID,
			SO.OrderDate SODate,
			A.AccountName Customer,
			R.PartNumberStrip PartNumber,
			R.Qty QtySold,
			R.Price SoldPrice,
			R.DateCode DateCode,
			R.Cost UnitCost,
			R.DueDate,
			null ShippedQty,
			S.StatusName OrderStatus,
			(R.Price - R.Cost) * R.Qty GP,
			dbo.fnGetObjectOwners(16, R.SOLineID) SalesPerson,
			'S',
			0,
			IL.PartNumber BOMPartNumber,
			IL.PartNumberStrip BOMIntPartNumber,
			Il.Manufacturer BOMMfg,
			IL.Qty BOMQty,
			IL.TargetPrice BOMPrice,
			COALESCE(IL.TargetPrice, 0) - COALESCE(R.Price, 0) PriceDelta,
			(COALESCE(IL.Qty, 0)) * (COALESCE(IL.TargetPrice, 0) - COALESCE(R.Price, 0)) Potential,
			SD.SearchDetailID
			--PriceDelta
			--Potential
			--BOMPartNumber
			--BOMIntPartNumber
			--BOMMfg
			--BOMQty
			--BOMPrice	
			--SalesPerson
		FROM SalesOrderLines R
		INNER JOIN vwItemInventoryWithFulfillment F on F.SOLineID = R.SOLineID
		INNER JOIN SalesOrders SO on SO.SalesOrderID = R.SalesOrderID
		INNER JOIN Accounts A on A.AccountID = SO.AccountID
		INNER JOIN Manufacturers M on M.MfrID = F.MfrID
		LEFT OUTER JOIN lkpStatuses S on S.StatusID = R.StatusID
		INNER JOIN SearchDetails SD on SD.SearchID = @SearchID
		LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE R.PartNumberStrip LIKE SD.SearchValue + '%'
		AND @MatchSO = 1

		-- ########## GET VENDOR RFQ RECORD DETAILS ##########
		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(rd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(rd.manu_no,tt.Manufacturer),
				Quantity = rd.resp_qty,
				Price = rd.quote_prc,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(rd.quote_prc, 0), --COALESCE(ld.TargetPrice, 0) - COALESCE(rd.quote_prc, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(rd.quote_prc, 0)), --(COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(rd.quote_prc, 0)),
				DateCode = rd.resp_dc,
				LeadTime = rd.lead_time,
				Notes = rd.resp_comm,
				--AccountID = rh.vend_no,
				AccountName = rh.vend_name,	
				RepName = rh.enter_by,
				ExactMatch = CASE WHEN rd.strip_item = SD.SearchValue THEN 1 ELSE 0 END, --CASE WHEN rd.strip_item = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				Buyer = rd.create_id,
				--Buyer = rh.enter_by,
				--Buyer = dbo.fnGetObjectOwners(27,rd.rfq_no),
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds01.dbo.rfq_detl rd ON tt.RecordID = rd.rfq_no AND tt.LineID = rd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN  SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds01.dbo.rfq_hedr rh ON rd.rfq_no = rh.rfq_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 1) AND (tt.MatchTypeID = 'R')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(rd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(rd.manu_no,tt.Manufacturer),
				Quantity = rd.resp_qty,
				Price = rd.quote_prc,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(rd.quote_prc, 0), --COALESCE(ld.TargetPrice, 0) - COALESCE(rd.quote_prc, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(rd.quote_prc, 0)), --(COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(rd.quote_prc, 0)),
				DateCode = rd.resp_dc,
				LeadTime = rd.lead_time,
				Notes = rd.resp_comm,
				--AccountID = rh.vend_no,
				AccountName = rh.vend_name,	
				RepName = rh.enter_by,
				ExactMatch = CASE WHEN rd.strip_item = SD.SearchValue THEN 1 ELSE 0 END, --CASE WHEN rd.strip_item = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				--Buyer = rh.enter_by,
				Buyer = rd.create_id,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds02.dbo.rfq_detl rd ON tt.RecordID = rd.rfq_no AND tt.LineID = rd.lin_no
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN epds02.dbo.rfq_hedr rh ON rd.rfq_no = rh.rfq_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 2) AND (tt.MatchTypeID = 'R')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(rd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(rd.manu_no,tt.Manufacturer),
				Quantity = rd.resp_qty,
				Price = rd.quote_prc,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(rd.quote_prc, 0), --COALESCE(ld.TargetPrice, 0) - COALESCE(rd.quote_prc, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(rd.quote_prc, 0)), --(COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(rd.quote_prc, 0)),
				DateCode = rd.resp_dc,
				LeadTime = rd.lead_time,
				Notes = rd.resp_comm,
				--AccountID = rh.vend_no,
				AccountName = rh.vend_name,	
				RepName = rh.enter_by,
				ExactMatch = CASE WHEN rd.strip_item = SD.SearchValue THEN 1 ELSE 0 END, --CASE WHEN rd.strip_item = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				--Buyer = rh.enter_by,
				Buyer = rd.create_id,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt 
				INNER JOIN epds03.dbo.rfq_detl rd ON tt.RecordID = rd.rfq_no AND tt.LineID = rd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds03.dbo.rfq_hedr rh ON rd.rfq_no = rh.rfq_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 3) AND (tt.MatchTypeID = 'R')

		INSERT INTO @TempTable (RecordID, AccountID, AccountName, MfgPartNumber, Manufacturer, Quantity, Cost, Buyer, DateCode, LeadTime, MatchTypeID, DatabaseID, BOMPartNumber, BOMIntPartNumber, BOMMfg, BOMQty, BOMPrice, PriceDelta, Potential, SearchDetailID)
		SELECT
				S.SourceID,
				S.AccountID,
				A.AccountName,
				S.PartNumber,
				S.Manufacturer,
				S.Qty,
				S.Cost,
				U.FirstName + ' ' + U.LastName Buyer,
				S.DateCode,
				S.LeadTimeDays,
				'R',
				0,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				PriceDelta = COALESCE (IL.TargetPrice, 0) - COALESCE (S.Cost, 0), --Should be using QuotePrice, this accurate?
				Potential = COALESCE (IL.Qty, 0) * (COALESCE (IL.TargetPrice, 0) - COALESCE (S.Cost, 0)),	--Should be using QuotePrice, this accurate?
				SD.SearchDetailID
		FROM Sources S
				INNER JOIN Accounts A ON S.AccountID = A.AccountID
				INNER JOIN Users U ON U.UserID = S.CreatedBy
				INNER JOIN SearchDetails SD on SD.SearchID = @SearchID
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE S.SourceTypeID = 7 AND S.PartNumberStrip LIKE SD.SearchValue + '%' AND @MatchRFQ = 1

		-- ########## GET CUSTOMER RFQ RECORD DETAILS ##########

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(qd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(qd.manu_no,tt.Manufacturer),
				IntPartNumber = qd.cust_itmno,
				Quantity = qd.quantity,
				Price = qd.target_prc,
				PriceDelta = COALESCE(qd.target_prc, 0) - COALESCE(qd.target_prc, 0), -- COALESCE(ld.TargetPrice, 0) - COALESCE(qd.target_prc, 0),
				Potential = (COALESCE(qd.Quantity, 0)) * (COALESCE(qd.target_prc, 0) -	COALESCE(qd.target_prc, 0)), --(COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)),
				DateCode = qd.date_code,			
				--AccountID = qh.cust_no,
				AccountName = qh.cust_name,
				ContactID = qh.contactid,
				ContactName = qh.contact,	
				SalesPerson = qh.ent_by,
				ExactMatch = CASE WHEN qd.strip_item_no = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN qd.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds01.dbo.quo_detl qd ON tt.RecordID = qd.quote_no AND tt.LineID = qd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN  SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds01.dbo.quote qh ON qd.quote_no = qh.quote_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 1) AND (tt.MatchTypeID = 'Q')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(qd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(qd.manu_no,tt.Manufacturer),
				IntPartNumber = qd.cust_itmno,
				Quantity = qd.quantity,
				Price = qd.target_prc,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(qd.target_prc, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(qd.target_prc, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)),
				DateCode = qd.date_code,			
				--AccountID = qh.cust_no,
				AccountName = qh.cust_name,
				ContactID = qh.contactid,
				ContactName = qh.contact,	
				SalesPerson = qh.ent_by,
				ExactMatch = CASE WHEN qd.strip_item_no = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN qd.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds02.dbo.quo_detl qd ON tt.RecordID = qd.quote_no AND tt.LineID = qd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN  SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds02.dbo.quote qh ON qd.quote_no = qh.quote_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 2) AND (tt.MatchTypeID = 'Q')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(qd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(qd.manu_no,tt.Manufacturer),
				IntPartNumber = qd.cust_itmno,
				Quantity = qd.quantity,
				Price = qd.target_prc,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(qd.target_prc, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(qd.target_prc, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)), --(COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)),
				DateCode = qd.date_code,			
				--AccountID = qh.cust_no,
				AccountName = qh.cust_name,
				ContactID = qh.contactid,
				ContactName = qh.contact,	
				SalesPerson = qh.ent_by,
				ExactMatch = CASE WHEN qd.strip_item_no = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN qd.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds03.dbo.quo_detl qd ON tt.RecordID = qd.quote_no AND tt.LineID = qd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID INNER JOIN
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds03.dbo.quote qh ON qd.quote_no = qh.quote_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 3) AND (tt.MatchTypeID = 'Q')

		INSERT INTO @TempTable (RecordID, AccountID,  AccountName, ContactID, ContactName, MfgPartNumber, Manufacturer, Quantity, TargetPrice, SalesPerson,
		IntPartNumber, DateCode, MatchTypeID, DatabaseID, BOMPartNumber, BOMIntPartNumber, BOMMfg, BOMQty, BOMPrice, PriceDelta, Potential, SearchDetailID)
		SELECT
			   Q.QuoteID,
			   Q.AccountID,
			   A.AccountName,
			   Q.ContactID,
			   C.FirstName + ' ' + C.LastName ContactName,
			   QL.PartNumber,
			   QL.Manufacturer,
			   QL.Qty,
			   QL.TargetPrice,
			   dbo.fnGetObjectOwners(Q.QuoteID, 19) SalesRep,
			   QL.CustomerPartNum,
			   QL.DateCode,
			   'Q',
			   0,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				PriceDelta = COALESCE (IL.TargetPrice, 0) - COALESCE (QL.Price, 0), 
				Potential = COALESCE (IL.Qty, 0) * (COALESCE (IL.TargetPrice, 0) - COALESCE (QL.Price, 0)),
				SD.SearchDetailID
		FROM vwQuotes Q
			   INNER JOIN vwQuoteLines QL ON Q.QuoteID = QL.QuoteID
			   INNER JOIN Accounts A ON Q.AccountID = A.AccountID
			   INNER JOIN Contacts C ON Q.ContactID = C.ContactID
			   INNER JOIN SearchDetails SD on SD.SearchID = @SearchID
			   LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE QL.PartNumberStrip LIKE SD.SearchValue + '%' AND @MatchQuote = 1

		-- ########## GET CUSTOMER QUOTE RECORD DETAILS ##########

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(qd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(qd.manu_no,tt.Manufacturer),
				IntPartNumber = qd.cust_itmno,
				Quantity = qd.quantity,
				Price = qd.target_prc,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(qd.target_prc, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(qd.target_prc, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)),
				DateCode = qd.date_code,			
				--AccountID = qh.cust_no,
				AccountName = qh.cust_name,
				ContactID = qh.contactid,
				ContactName = qh.contact,	
				SalesPerson = qh.ent_by,
				ExactMatch = CASE WHEN qd.strip_item_no = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN qd.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds01.dbo.quo_detl qd ON tt.RecordID = qd.quote_no AND tt.LineID = qd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds01.dbo.quote qh ON qd.quote_no = qh.quote_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 1) AND (tt.MatchTypeID = 'C')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(qd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(qd.manu_no,tt.Manufacturer),
				IntPartNumber = qd.cust_itmno,
				Quantity = qd.quantity,
				Price = qd.target_prc,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(qd.target_prc, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(qd.target_prc, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)),
				DateCode = qd.date_code,			
				--AccountID = qh.cust_no,
				AccountName = qh.cust_name,
				ContactID = qh.contactid,
				ContactName = qh.contact,	
				SalesPerson = qh.ent_by,
				ExactMatch = CASE WHEN qd.strip_item_no = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN qd.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds02.dbo.quo_detl qd ON tt.RecordID = qd.quote_no AND tt.LineID = qd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds02.dbo.quote qh ON qd.quote_no = qh.quote_no 
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 2) AND (tt.MatchTypeID = 'C')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(qd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(qd.manu_no,tt.Manufacturer),
				IntPartNumber = qd.cust_itmno,
				Quantity = qd.quantity,
				Price = qd.target_prc,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(qd.target_prc, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(qd.target_prc, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(qd.target_prc, 0)),
				DateCode = qd.date_code,			
				--AccountID = qh.cust_no,
				AccountName = qh.cust_name,
				ContactID = qh.contactid,
				ContactName = qh.contact,	
				SalesPerson = qh.ent_by,
				ExactMatch = CASE WHEN qd.strip_item_no = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN qd.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds03.dbo.quo_detl qd ON tt.RecordID = qd.quote_no AND tt.LineID = qd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds03.dbo.quote qh ON qd.quote_no = qh.quote_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 3) AND (tt.MatchTypeID = 'C')

		INSERT INTO @TempTable (RecordID, AccountID,  AccountName, ContactID, ContactName, MfgPartNumber, Manufacturer, Quantity, TargetPrice, SalesPerson,
		IntPartNumber, DateCode, MatchTypeID, DatabaseID, BOMPartNumber, BOMIntPartNumber, BOMMfg, BOMQty, BOMPrice, PriceDelta, Potential, SearchDetailID)
		SELECT
			   Q.QuoteID,
			   Q.AccountID,
			   A.AccountName,
			   Q.ContactID,
			   C.FirstName + ' ' + C.LastName ContactName,
			   QL.PartNumber,
			   QL.Manufacturer,
			   QL.Qty,
			   QL.TargetPrice,
			   dbo.fnGetObjectOwners(Q.QuoteID, 19) SalesRep,
			   QL.CustomerPartNum,
			   QL.DateCode,
			   'C',
			   0,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				PriceDelta = COALESCE(QL.Price, 0) - COALESCE(IL.TargetPrice, 0),
				Potential = (COALESCE(IL.Qty, 0)) * (COALESCE(IL.TargetPrice, 0) - COALESCE(QL.Price, 0)),
				SD.SearchDetailID
		FROM vwQuotes Q
			   INNER JOIN vwQuoteLines QL ON Q.QuoteID = QL.QuoteID
			   INNER JOIN Accounts A ON Q.AccountID = A.AccountID
			   INNER JOIN Contacts C ON Q.ContactID = C.ContactID
			   INNER JOIN SearchDetails SD on SD.SearchID = @SearchID
			   LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE QL.PartNumberStrip LIKE SD.SearchValue + '%' AND @MatchCustomerQuote = 1


		-- ########## GET PURCHASE ORDER RECORD DETAILS ##########

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(pd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(pd.manu_no,tt.Manufacturer),
				Quantity = pd.ord_qty,
				OrderDate = pd.create_dt,
				Price = pd.unit_cost,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(pd.unit_cost, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(pd.unit_cost, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(pd.unit_cost, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(pd.unit_cost, 0)),
				--AccountID = ph.vend_no,
				AccountName = v.ap_name,
				DateCode = pd.req_dc,
				ExactMatch = CASE WHEN pd.item_no = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN pd.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				Buyer = ph.buyer,	--RepName = ph.buyer
				ReceivedDate = trx.trx_date,
				ReceivedQty = pd.qty_recvd,
				OrderStatus = ph.status, --POStatus = ph.status
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds01.dbo.po_detl pd ON tt.RecordID = pd.po_no AND tt.LineID = pd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN  SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds01.dbo.po_hedr ph ON pd.po_no = ph.po_no
				INNER JOIN epds01.dbo.vendor v ON v.vend_no = ph.vend_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
				OUTER APPLY (SELECT DISTINCT MAX(TR.trx_date) trx_date FROM epds01.dbo.po_detl D
							 INNER JOIN epds01.dbo.inv_trx TR on TR.order_no = D.po_no
							 WHERE TR.lin_no = pd.lin_no AND TR.order_no = pd.po_no 
							 AND TR.trx_type = 'R' AND TR.trx_qty <> 0) TRX
		WHERE	(tt.DatabaseID = 1) AND (tt.MatchTypeID = 'P')
	
		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(pd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(pd.manu_no,tt.Manufacturer),
				Quantity = pd.ord_qty,
				OrderDate = pd.create_dt,
				Price = pd.unit_cost,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(pd.unit_cost, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(pd.unit_cost, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(pd.unit_cost, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(pd.unit_cost, 0)),
				--AccountID = ph.vend_no,
				AccountName = v.ap_name,
				DateCode = pd.req_dc,
				ExactMatch = CASE WHEN pd.item_no = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN pd.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				Buyer = ph.buyer,	--RepName = ph.buyer
				ReceivedDate = trx.trx_date,
				ReceivedQty = pd.qty_recvd,
				OrderStatus = ph.status, --POStatus = ph.status
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds02.dbo.po_detl pd ON tt.RecordID = pd.po_no AND tt.LineID = pd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds02.dbo.po_hedr ph ON pd.po_no = ph.po_no
				INNER JOIN epds02.dbo.vendor v ON v.vend_no = ph.vend_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
				OUTER APPLY (SELECT DISTINCT MAX(TR.trx_date) trx_date FROM epds02.dbo.po_detl D
							 INNER JOIN epds02.dbo.inv_trx TR on TR.order_no = D.po_no
							 WHERE TR.lin_no = pd.lin_no AND TR.order_no = pd.po_no 
							 AND TR.trx_type = 'R' AND TR.trx_qty <> 0) TRX
		WHERE	(tt.DatabaseID = 2) AND (tt.MatchTypeID = 'P')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(pd.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(pd.manu_no,tt.Manufacturer),
				Quantity = pd.ord_qty,
				OrderDate = pd.create_dt,
				Price = pd.unit_cost,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(pd.unit_cost, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(pd.unit_cost, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(pd.unit_cost, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(pd.unit_cost, 0)),
				--AccountID = ph.vend_no,
				AccountName = v.ap_name,
				DateCode = pd.req_dc,
				ExactMatch = CASE WHEN pd.item_no = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN pd.strip_item_no = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				Buyer = ph.buyer,	--RepName = ph.buyer
				ReceivedDate = trx.trx_date,
				ReceivedQty = pd.qty_recvd,
				OrderStatus = ph.status, --POStatus = ph.status
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds03.dbo.po_detl pd ON tt.RecordID = pd.po_no AND tt.LineID = pd.lin_no
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds03.dbo.po_hedr ph ON pd.po_no = ph.po_no
				INNER JOIN epds03.dbo.vendor v ON v.vend_no = ph.vend_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
				OUTER APPLY (SELECT DISTINCT MAX(TR.trx_date) trx_date FROM epds03.dbo.po_detl D
							 INNER JOIN epds03.dbo.inv_trx TR on TR.order_no = D.po_no
							 WHERE TR.lin_no = pd.lin_no AND TR.order_no = pd.po_no 
							 AND TR.trx_type = 'R' AND TR.trx_qty <> 0) TRX
		WHERE	(tt.DatabaseID = 3) AND (tt.MatchTypeID = 'P')

		INSERT INTO @TempTable (ItemID ,Manufacturer, PONumber, RecordID, DateCode, AccountName, MfgPartNumber, Quantity, Cost, ReceivedDate, ReceivedQty, OrderStatus, OrderDate, Buyer, MatchTypeID, DatabaseID, BOMPartNumber, BOMIntPartNumber, BOMMfg, BOMQty, BOMPrice, PriceDelta, Potential, SearchDetailID)
			SELECT I.ItemID,
					M.MfrName Manufacturer,
					R.PurchaseOrderID OrderNumber,
					R.POLineID RecordID,
					R.DateCode DateCode,
					A.AccountName Vendor,
					I.PartNumber PartNumber,
					R.Qty QtyOrdered,
					R.Cost POCost,
					II.ReceivedDate,
					II.ReceivedQty,
					S.StatusName,
					PO.OrderDate PODate,
					dbo.fnGetObjectOwners(22, R.POLineID) Buyer,
					'P',
					0,
					BOMPartNumber = IL.PartNumber,
					BOMIntPartNumber = IL.PartNumberStrip,
					BOMMfg = Il.Manufacturer,
					BOMQty = IL.Qty,
					BOMPrice = IL.TargetPrice,
					PriceDelta = COALESCE(IL.TargetPrice, 0) - COALESCE(R.Cost, 0),
					Potential = (COALESCE(IL.Qty, 0)) * (COALESCE(IL.TargetPrice, 0) - COALESCE(R.Cost, 0)),
					SearchDetailID = SD.SearchDetailID
			FROM PurchaseOrderLines R
			LEFT OUTER JOIN vwItemInventoryWithFulfillment F on F.POLineID = R.PurchaseOrderID
			LEFT OUTER JOIN PurchaseOrders PO on PO.PurchaseOrderID = R.PurchaseOrderID
			LEFT OUTER JOIN SalesOrderLines SL on SL.SOLineID = F.SOLineID
			LEFT OUTER JOIN Items I on I.ItemID = R.ItemID
			INNER JOIN Manufacturers M on M.MfrID = I.MfrID
			INNER JOIN Accounts A on A.AccountID = PO.AccountID
			OUTER APPLY (SELECT SUM(sq.Qty) ReceivedQty, MIN(sq.ReceivedDate) ReceivedDate FROM vwStockQty sq WHERE sq.POLineID = R.POLineID AND sq.IsDeleted = 0 ) II
			LEFT OUTER JOIN lkpStatuses S on S.StatusID = R.StatusID
			INNER JOIN SearchDetails SD on SD.SearchID = @SearchID
			LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
			WHERE SL.PartNumberStrip LIKE SD.SearchValue + '%'
			AND @MatchPO = 1

		-- ########## GET OUTSIDE OFFERS RECORD DETAILS ##########

		UPDATE	@TempTable
		SET		--MfgPartNumber =isnull(o.item_no,tt.MfgPartNumber),
				MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(o.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(o.manu_no,tt.Manufacturer),
				Quantity = o.quantity,
				Price = o.price,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(o.price, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(o.price, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(o.price, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(o.price, 0)),
				--AccountID = o.vend_no,
				AccountName = v.ap_name,
				DateCode = o.date_code,
				LeadTime = o.lead_time,
				ExactMatch = CASE WHEN o.strip_item = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN o.strip_item = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				Buyer = o.owner,
				Notes = o.notes,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds01.dbo.offers o ON tt.RecordID = o.off_int_id
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds01.dbo.vendor v ON v.vend_no = o.vend_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID	
		WHERE	(tt.DatabaseID = 1) AND (tt.MatchTypeID = 'O')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(o.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(o.manu_no,tt.Manufacturer),
				Quantity = o.quantity,
				Price = o.price,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(o.price, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(o.price, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(o.price, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(o.price, 0)),
				--AccountID = o.vend_no,
				AccountName = v.ap_name,
				DateCode = o.date_code,
				LeadTime = o.lead_time,
				ExactMatch = CASE WHEN o.strip_item = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN o.strip_item = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				Buyer = o.owner,
				Notes = o.notes,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds02.dbo.offers o ON tt.RecordID = o.off_int_id
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds02.dbo.vendor v ON v.vend_no = o.vend_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 2) AND (tt.MatchTypeID = 'O')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(o.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(o.manu_no,tt.Manufacturer),
				Quantity = o.quantity,
				Price = o.price,
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(o.price, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(o.price, 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(o.price, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(o.price, 0)),
				--AccountID = o.vend_no,
				AccountName = v.ap_name,
				DateCode = o.date_code,
				LeadTime = o.lead_time,
				ExactMatch = CASE WHEN o.strip_item = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN o.strip_item = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				Buyer = o.owner,
				Notes = o.notes,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds03.dbo.offers o ON tt.RecordID = o.off_int_id
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				INNER JOIN epds03.dbo.vendor v ON v.vend_no = o.vend_no	
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID		
		WHERE	(tt.DatabaseID = 3) AND (tt.MatchTypeID = 'O')

		INSERT INTO @TempTable (RecordID, AccountID, AccountName, MfgPartNumber, Manufacturer, Quantity, Cost, Buyer, DateCode, LeadTime, MatchTypeID, DatabaseID, BOMPartNumber, BOMIntPartNumber, BOMMfg, BOMQty, BOMPrice, PriceDelta, Potential, SearchDetailID)
		SELECT
				S.SourceID,
				S.AccountID,
				A.AccountName,
				S.PartNumber,
				S.Manufacturer,
				S.Qty,
				S.Cost,
				U.FirstName + ' ' + U.LastName Buyer,
				S.DateCode,
				S.LeadTimeDays,
				'O',
				0,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				PriceDelta = COALESCE(IL.TargetPrice, 0) - COALESCE(S.Cost, 0),		--Cost ok? Should be price
				Potential = (COALESCE(IL.Qty, 0)) * (COALESCE(IL.TargetPrice, 0) - COALESCE(S.Cost, 0)),		--Cost ok? Should be price
				SearchDetailID = SD.SearchDetailID
		FROM Sources S
				INNER JOIN Accounts A ON S.AccountID = A.AccountID
				INNER JOIN Users U ON U.UserID = S.CreatedBy
				INNER JOIN SearchDetails SD on SD.SearchID = @SearchID
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE S.SourceTypeID = 9 AND S.PartNumberStrip LIKE SD.SearchValue + '%' AND @MatchOffers = 1
 

	-- ########## GET INVENTORY RECORD DETAILS ##########

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(lh.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(lh.manu_no,tt.Manufacturer),
				Quantity = lh.bal_of_lot,
				Price = epds01.dbo.calc_lot_cost(lh.lot_no),
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(epds01.dbo.calc_lot_cost(lh.lot_no), 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(epds01.dbo.calc_lot_cost(lh.lot_no), 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(epds01.dbo.calc_lot_cost(lh.lot_no), 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(epds01.dbo.calc_lot_cost(lh.lot_no), 0)),
				DateCode = lh.date_code,
				Quantity2 = lh.bal_of_lot - lh.qty_commit,
				--WarehouseID = lh.whse_cd,
				WareHouseCode = lh.whse_cd,
				ExactMatch = CASE WHEN it.strip_item = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN it.strip_item = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				ReservedQty = lh.qty_commit,
				AvailableQty = lh.bal_of_lot - lh.qty_commit,
				Customer = oh.cust1,
				PONumber = r.order_no,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds01.dbo.lot_hedr lh ON tt.RecordID = lh.li_int_id
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				LEFT OUTER JOIN (SELECT DISTINCT I.lot_no, I.order_no FROM epds01.dbo.inv_trx I WHERE I.trx_type = 'R') r ON lh.lot_no = r.lot_no LEFT OUTER JOIN 
				epds01.dbo.ord_hedr oh on oh.order_no = r.order_no
				INNER JOIN epds01.dbo.item it ON lh.item_no = it.item_no AND lh.manu_no = it.manu_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 1) AND (tt.MatchTypeID = 'I')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(lh.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(lh.manu_no,tt.Manufacturer),
				Quantity = lh.bal_of_lot,
				Price = epds02.dbo.calc_lot_cost(lh.lot_no),
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(epds02.dbo.calc_lot_cost(lh.lot_no), 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(epds02.dbo.calc_lot_cost(lh.lot_no), 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(epds02.dbo.calc_lot_cost(lh.lot_no), 0)),			 --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(epds02.dbo.calc_lot_cost(lh.lot_no), 0)),			
				DateCode = lh.date_code,
				Quantity2 = lh.bal_of_lot - lh.qty_commit,
				--WarehouseID = lh.whse_cd,
				WarehouseCode = lh.whse_cd,
				ExactMatch = CASE WHEN it.strip_item = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN it.strip_item = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				ReservedQty = lh.qty_commit,
				AvailableQty = lh.bal_of_lot - lh.qty_commit,
				Customer = oh.cust1,
				PONumber = r.order_no,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds02.dbo.lot_hedr lh ON tt.RecordID = lh.li_int_id
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				LEFT OUTER JOIN (SELECT DISTINCT I.lot_no, I.order_no FROM epds02.dbo.inv_trx I WHERE I.trx_type = 'R') r ON lh.lot_no = r.lot_no
				LEFT OUTER JOIN epds02.dbo.ord_hedr oh on oh.order_no = r.order_no
				INNER JOIN epds02.dbo.item it ON lh.item_no = it.item_no AND lh.manu_no = it.manu_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID

		WHERE	(tt.DatabaseID = 2) AND (tt.MatchTypeID = 'I')

		UPDATE	@TempTable
		SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(lh.item_no,tt.MfgPartNumber)),
				Manufacturer = isnull(lh.manu_no,tt.Manufacturer),
				Quantity = lh.bal_of_lot,
				Price = epds03.dbo.calc_lot_cost(lh.lot_no),
				PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(epds03.dbo.calc_lot_cost(lh.lot_no), 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(epds03.dbo.calc_lot_cost(lh.lot_no), 0),
				Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(epds03.dbo.calc_lot_cost(lh.lot_no), 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(epds03.dbo.calc_lot_cost(lh.lot_no), 0)),			
				DateCode = lh.date_code,
				Quantity2 = lh.bal_of_lot - lh.qty_commit,
				--WarehouseID = lh.whse_cd,
				WareHouseCode = lh.whse_cd,
				ExactMatch = CASE WHEN it.strip_item = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN it.strip_item = ld.MfgPartNumberStrip THEN 1 ELSE 0 END,
				CreateUserID = @UserID,
				UpdateUserID = @UserID,
				ReservedQty = lh.qty_commit,
				AvailableQty = lh.bal_of_lot - lh.qty_commit,
				Customer = oh.cust1,
				PONumber = r.order_no,
				BOMPartNumber = IL.PartNumber,
				BOMIntPartNumber = IL.PartNumberStrip,
				BOMMfg = Il.Manufacturer,
				BOMQty = IL.Qty,
				BOMPrice = IL.TargetPrice,
				SearchDetailID = SD.SearchDetailID
		FROM	@TempTable tt
				INNER JOIN epds03.dbo.lot_hedr lh ON tt.RecordID = lh.li_int_id
				--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID
				INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
				LEFT OUTER JOIN (SELECT DISTINCT I.lot_no, I.order_no FROM epds03.dbo.inv_trx I WHERE I.trx_type = 'R') r ON lh.lot_no = r.lot_no
				LEFT OUTER JOIN epds03.dbo.ord_hedr oh on oh.order_no = r.order_no
				INNER JOIN epds03.dbo.item it ON lh.item_no = it.item_no AND lh.manu_no = it.manu_no
				LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE	(tt.DatabaseID = 3) AND (tt.MatchTypeID = 'I')
	
		INSERT INTO @TempTable (RecordID, ItemID, Manufacturer, WarehouseCode, WarehouseID, Quantity, Cost, ReservedQty, AvailableQty, DateCode, MatchTypeID, DatabaseID, BOMPartNumber, BOMIntPartNumber, BOMMfg, BOMQty, BOMPrice, PriceDelta, Potential, SearchDetailID)
		SELECT
			R.POLineID,
			R.ItemID,
			M.MfrName Manufacturer,
			W.WarehouseName,
			F.WarehouseID,
			R.Qty InventoryQty,
			PL.Cost,
			QtySum ReservedQty,
			R.Qty-QtySum AvailableQty,
			R.DateCode,
			'I',
			0,
			BOMPartNumber = IL.PartNumber,
			BOMIntPartNumber = IL.PartNumberStrip,
			BOMMfg = Il.Manufacturer,
			BOMQty = IL.Qty,
			BOMPrice = IL.TargetPrice,
			PriceDelta = COALESCE(TargetPrice, 0) - COALESCE(PL.Cost, 0),
			Potential = (COALESCE(IL.Qty, 0)) * (COALESCE(TargetPrice, 0) - COALESCE(PL.Cost, 0)),
			SearchDetailID = SD.SearchDetailID
		FROM vwStockQty R
		LEFT OUTER JOIN Warehouses W on W.WarehouseID = R.WarehouseID
		LEFT OUTER JOIN vwItemInventoryWithFulfillment F on F.ItemID = R.ItemID
		LEFT OUTER JOIN PurchaseOrderLines PL on PL.POLineID = F.POLineID
		INNER JOIN Manufacturers M on M.mfrID = F.MfrID
		OUTER APPLY ( SELECT SUM(SF.Qty) QtySum FROM mapSOInvFulfillment SF WHERE SF.StockID = R.StockID ) SF
		INNER JOIN SearchDetails SD on SD.SearchID = @SearchID
		LEFT OUTER JOIN ItemListLines IL on IL.ItemListLineID = SD.ItemListLineID
		WHERE F.PartNumberStrip LIKE SD.SearchValue + '%'
		AND R.IsDeleted = 0
		AND @MatchInventory = 1
	
		-- ########## GET BOM RECORD DETAILS ##########
		
		--UPDATE	@TempTable
		--SET		MfgPartNumber = dbo.fnStripNonAlphaNumeric(tt.MfgPartNumber),
		--		--MfgPartNumber = dbo.fnStripNonAlphaNumeric(isnull(dj.MfgPartNumber,tt.MfgPartNumber)),
		--		--Manufacturer = isnull(dj.Manufacturer,tt.Manufacturer),
		--		--IntPartNumber = dj.IntPartNumber,
		--		--Quantity = dj.Quantity,
		--		--Price = dj.TargetPrice,
		--		--PriceDelta = COALESCE(tt.TargetPrice, 0) - COALESCE(dj.TargetPrice, 0), --PriceDelta = COALESCE(ld.TargetPrice, 0) - COALESCE(dj.TargetPrice, 0),
		--		--Potential = (COALESCE(tt.Quantity, 0)) * (COALESCE(tt.TargetPrice, 0) -	COALESCE(dj.TargetPrice, 0)), --Potential = (COALESCE(ld.Quantity, 0)) * (COALESCE(ld.TargetPrice, 0) -	COALESCE(dj.TargetPrice, 0)),
		--		--AccountID = dj.AccountID,
		--		--AccountName = dj.ListAccountName,			
		--		ExactMatch = CASE WHEN dbo.fnStripNonAlphaNumeric(tt.MfgPartNumber) = dbo.fnStripNonAlphaNumeric(SD.SearchValue) THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN dj.MfgPartNumberStrip = ld.MfgPartNumberStrip THEN 1 ELSE 0 END, 
		--		--ExactMatch = CASE WHEN dj.MfgPartNumberStrip = SD.SearchValue THEN 1 ELSE 0 END, --ExactMatch = CASE WHEN dj.MfgPartNumberStrip = ld.MfgPartNumberStrip THEN 1 ELSE 0 END, 
		--		CreateUserID = @UserID,
		--		--SalesPerson = dj.ListAccountName,
		--		UpdateUserID = @UserID,
		--		SearchDetailID = SD.SearchDetailID
		--FROM	@TempTable tt
		--		--INNER JOIN [SourcePortal_DEV].bom.viewListDetailJoined dj ON tt.RecordID = dj.ListDetailID AND tt.DatabaseID = dj.DatabaseID
		--		--INNER JOIN bom.ListDetail ld ON tt.ListDetailID = ld.ListDetailID 
		--		INNER JOIN SearchDetails SD on TT.MfgPartNumber lIKE SD.SearchValue  + '%'
		--WHERE	(tt.MatchTypeID = 'B') 
		
		INSERT INTO @TempTable (RecordID, AccountID, AccountName, MfgPartNumber, Manufacturer, Quantity, TargetPrice, IntPartNumber, SalesPerson, MatchTypeID, DatabaseID, SearchDetailID) --CreatedBy
		SELECT
			   IL.ItemListID,
			   IL.AccountID,
			   A.AccountName,
			   ILL.PartNumber,
			   ILL.Manufacturer,
			   ILL.Qty,
			   ILL.TargetPrice,			  
			   ILL.CustomerPartNum,
			    U.FirstName + ' ' + U.LastName Uploader,
				'B',
				0,
				SD.SearchDetailID
		FROM ItemLists IL
			   INNER JOIN ItemListLines ILL ON IL.ItemListID = ILL.ItemListID
			   INNER JOIN Accounts A ON IL.AccountID = A.AccountID
			   INNER JOIN Users U ON IL.CreatedBy = U.UserID
			   INNER JOIN SearchDetails SD on SD.SearchID = @SearchID

		WHERE ILL.PartNumberStrip LIKE SD.SearchValue + '%' AND @MatchBOM = 1

		
	END
	
	--INSERT INTO bom.MatchDetail
	INSERT	INTO SearchResults
            (SearchID, ResultType, DataSource, RecordID, PartNumber, Manufacturer, CustomerPartNumber, --DatabaseID, MfgPartNumber, LineID,, Quantity, MatchHeaderID,  ListDetailID,
			Price, PriceDelta, Potential, DateCode, LeadTimeDays, DueDate, Qty, Warehouse, AccountID, SODate, --MatchHeaderID, QuantityDelta, Quantity2, LeadTime,  UnitCost, LineGP, WarehouseID
			AccountName, ContactID, ContactName, Note, CreatedBy, Cost, TargetPrice, ShippedQty, --CreateUserID, UpdateUserID, RepName, Notes, RecDate, ExactMatch, CreatorID, QuotedPrice,
            OrderStatus, ReservedQty, AvailableQty, PurchaseOrderID, SalesOrderID, ItemID, ReceivedDate, ReceivedQty, Buyer, OrderDate, OwnerName, --OwnerName == SalesPerson,  SONumber, ReceivedQty, Customer, POStatus,
			BOMPartNumber, BOMIntPartNumber, BOMMfg, BOMQty, BOMPrice, SearchDetailID)
	SELECT	@SearchID, MatchTypeID, DatabaseID, RecordID, ISNULL(MfgPartNumber,''), Manufacturer, IntPartNumber,  --LineID, Quantity, ListDetailID,IntPartNumber
			Price, PriceDelta, Potential, DateCode, LeadTime, DueDate, Quantity, WarehouseCode, AccountID, SODate,--Quantity2,, UnitCost, LineGP, 
			AccountName, ContactID, ContactName, Notes, @UserID, Cost, TargetPrice, ShippedQty, --CreateUserID, UpdateUserID, ExactMatch, QuotedPrice,
            OrderStatus, ReservedQty, AvailableQty, PONumber, SONumber, ItemID, ReceivedDate, ReceivedQty, Buyer, OrderDate, SalesPerson, --, SONumber, ReceivedQty, Customer, POStatus,
			BOMPartNumber, BOMIntPartNumber, BOMMfg, BOMQty, BOMPrice, SearchDetailID
	FROM	@TempTable

	SELECT @SearchID SearchID 
	--EXEC dbo.spBomUpdateMatchCounts @MatchHeaderID

	--UPDATE bom.MatchExecResults
	--SET EndTime = GETDATE(),
	--	TimeToComplete = Datediff(s, tt.StartTime, GETDATE())
	--FROM bom.MatchExecResults tt
	--WHERE tt.MatchHeaderID = @MatchHeaderID

END