/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.27
   Description:	Creates or updates a Source record
   Usage:		EXEC 
				
   Return Codes:
				-2 Either ItemID or PartNumber must be provided
				-3 AccountID is required
				-4 CommodityID is required
				-5 CurrencyID is required
				-6 Error inserting source record
				-7 Error updating source record
   Revision History:
		2017.09.15	ML	Made SourcceID and Output parameter
		2018.03.08	NA	Added Package Condition
		2018.03.01	BZ	Change insert ContactID to nullable			
		2018.04.19	AR	Changed ValidForHours from INT to DOUBLE
		2018.04.27	AR	Added @IsNoStock
   ============================================= */



CREATE PROCEDURE [dbo].[uspSourceSet]
	@SourceID INT = NULL OUTPUT,
	@SourceTypeID INT = NULL,
	@ItemID INT = NULL,
	@CommodityID INT = NULL,
	@AccountID INT = NULL,
	@ContactID INT = NULL,
	@CurrencyID CHAR(3) = NULL,
	@PartNumber NVARCHAR(32) = NULL,
	@Manufacturer NVARCHAR(128) = NULL,
	@Qty INT = NULL,
	@Cost MONEY = NULL,
	@DateCode NVARCHAR(25) = NULL,
	@PackagingID INT = NULL,
	@PackageConditionID INT = NULL,
	@MOQ INT = NULL,
	@SPQ INT = NULL,
	@LeadTimeDays INT = NULL,
	@ValidForHours DECIMAL(10,2) = NULL,
	@IsNoStock BIT = NULL,
	@RequestToBuy BIT = NULL,
	@RTBQty BIT = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL

AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@ItemID, 0) = 0 AND ISNULL(@PartNumber, '') = ''
		RETURN -2
	IF ISNULL(@AccountID, 0) = 0
		RETURN -3
	IF ISNULL(@CommodityID, 0) = 0 AND ISNULL(@ItemID, 0) = 0
		RETURN -4
	IF ISNULL(@CurrencyID, '') = ''
		RETURN -5
	
	
	--Check if it is a known ItemID and set the known information	
	DECLARE @PartNumberStrip NVARCHAR(32) = NULL
	IF ISNULL(@ItemID, 0) <> 0
		BEGIN
			SELECT
				@PartNumber = i.PartNumber,
				@PartNumberStrip = i.PartNumberStrip,
				@Manufacturer = m.MfrName,
				@CommodityID = i.CommodityID
			FROM Items i
			INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
			WHERE ItemID = @ItemID
		END
	ELSE --If it isn't a known item, set the stripped part number
		SET @PartNumberStrip = dbo.fnStripNonAlphaNumeric(@PartNumber)
	
	IF ISNULL(@SourceID, 0) = 0
		GOTO InsertSource
	ELSE 
		GOTO UpdateSource

InsertSource:
	INSERT INTO Sources (SourceTypeID, ItemID, CommodityID, AccountID, ContactID, CurrencyID, PartNumber, PartNumberStrip, Manufacturer, Qty, Cost, DateCode, PackagingID, PackageConditionID, MOQ, SPQ, LeadTimeDays, ValidForHours, IsNoStock, RequestToBuy, RTBQty, CreatedBy)
	VALUES (@SourceTypeID, 
			CASE WHEN @ItemID = 0 THEN NULL ELSE @ItemID END, --ItemID
			@CommodityID, 
			@AccountID, 
			NULLIF(@ContactID, 0), 
			@CurrencyID, 
			@PartNumber, 
			@PartNumberStrip,
			@Manufacturer,
			@Qty,
			@Cost,
			@DateCode,
			@PackagingID,
			@PackageConditionID,
			@MOQ,
			@SPQ,
			@LeadTimeDays,
			@ValidForHours,
			@IsNoStock,
			ISNULL(@RequestToBuy, 0), --RequestToBuy
			@RTBQty,
			@UserID)

	SET @SourceID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -6
	GOTO ReturnSelect
UpdateSource:
	
	UPDATE Sources
	SET
		SourceTypeID = @SourceTypeID,
		ItemID = CASE WHEN @ItemID = 0 THEN NULL ELSE @ItemID END, --ItemID
		CommodityID = @CommodityID,
		AccountID = @AccountID,
		ContactID = ISNULL(@ContactID, ContactID),
		CurrencyID = @CurrencyID,
		PartNumber = @PartNumber,
		PartNumberStrip = @PartNumberStrip,
		Manufacturer = @Manufacturer,
		Qty = @Qty,
		Cost = @Cost,
		DateCode = @DateCode,
		PackagingID = @PackagingID,
		PackageConditionID = @PackageConditionID,
		MOQ = @MOQ,
		SPQ = @SPQ,
		LeadTimeDays = @LeadTimeDays,
		ValidForHours = @ValidForHours,
		RequestToBuy = ISNULL(@RequestToBuy, RequestToBuy),
		RTBQty = @RTBQty,
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		IsNoStock = ISNULL(@IsNoStock, IsNoStock),
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	WHERE SourceID = @SourceID

	IF (@@ROWCOUNT=0)
		RETURN -7
	GOTO ReturnSelect
ReturnSelect:
	SELECT @SourceID 'SourceID'
END
