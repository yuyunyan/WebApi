/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.31
   Description:	Inserts Item List data into itemList/itemlistlines
   Usage:		EXEC uspItemListIns 
				SELECT * FROM ItemLists
   Revision History:

   2017.11.09	AR	Added support for ListTypeID, and source insert support
   2018.01.09	AR	Added support for @PublishToSources
   2018.01.18	AR	Added Excess List (ListTypeID = 2) Columns & ISNULL(TargetDateCode,DateCode)
   2018.01.31  RV  Added QuoteTypeID param and field to ItemLists insert
   ============================================= */
CREATE PROCEDURE [dbo].[uspItemListIns]
(
	@ListData VARCHAR(MAX) = NULL
	, @AccountID INT = NULL
	, @ContactID INT = 0
	, @ProjectID INT = NULL
	, @StatusID INT = 1
	, @CurrencyID VARCHAR(12) = 'USD'
	, @OrganizationID INT = 0
	, @ListName VARCHAR(64) = NULL
	, @ListTypeID INT = 1
	, @SourceTypeID INT = NULL
	, @SalesUserID INT = NULL
	, @UserID INT
	, @PublishToSources BIT = 0
	, @QuoteTypeID INT = NULL 
	, @ItemListID INT = NULL OUTPUT
)
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @ListID INT
	INSERT INTO ItemLists (AccountID, ContactID, ProjectID, StatusID, CurrencyID, OrganizationID, ListName, SalesUserID, ItemListTypeID, IsPublished, CreatedBy, QuoteTypeID)
	VALUES ( @AccountID
	, @ContactID
	, @ProjectID
	, @StatusID
	, @CurrencyID
	, @OrganizationID
	, @ListName
	, @SalesUserID
	, @ListTypeID
	, @PublishToSources
	, @UserID
	, @QuoteTypeID )

	SET @ItemListID = @@IDENTITY
	SET @ListID = @@IDENTITY

	INSERT INTO ItemListLines (ItemListID, StatusID, ItemID, CommodityID, AssocAccountID, CustomerPartNum, PartNumber, PartNumberStrip, Manufacturer, Qty, TargetPrice, TargetDateCode, MOQ, SPQ, CreatedBy )
	SELECT @ListID
	, ISNULL(I.ItemStatusID, 0)
	, I.ItemID
	, ISNULL(I.CommodityID, 0)
	, D.AssocAccountID
	, D.CustomerPartNumber
	, D.PartNumber
	, D.PartNumberStrip
	, D.Manufacturer
	, D.Qty
	, D.TargetPrice
	, ISNULL(D.DateCode, D.TargetDateCode)	--Uses DateCode (ListTypeID = 2) Before TargetDateCode (ListTypeID = 1)
	, D.MOQ
	, D.SPQ
	, @UserID
	FROM
	OPENJSON(@ListData)
		WITH (
		 CustomerPartNumber VARCHAR(32),
		 PartNumber VARCHAR(32),
		 PartNumberStrip VARCHAR(32),
		 Manufacturer VARCHAR(128),
		 Qty INT,
		 TargetPrice MONEY,
		 AssocAccountID INT,
		 TargetDateCode VARCHAR(25),
		 DateCode VARCHAR(25),
		 MOQ INT,
		 SPQ INT
		 ) D
	LEFT OUTER JOIN Items I on I.PartNumberStrip = D.PartNumberStrip

	--Handle Source insert
	IF (ISNULL(@SourceTypeID,0) != 0)
		EXEC uspItemListSourceIns @ItemListID = @ListID, @SourceTypeID = @SourceTypeID, @UserID = @UserID

	SELECT @ItemListID 'ItemListID'
END