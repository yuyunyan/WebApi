/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.21
   Description:	Inserts or updates a line item on a Quote
   Usage:	EXEC uspQuoteLineSet @QuoteLineID = 12345 [...]
			EXEC uspQuoteLineSet @QuoteID = 10, @QuoteVersionID = 2 [...]			
   Return Codes:
			-9 Missing QuoteID or QuoteVersionID for new record
			-10 Error inserting new quote line
			-11 Missing both ItemID and PartNumber, at least one must be provided
			-12 Error updating quote line record
			-13 QuoteVersionID is not the latest version for the given QuoteID
			-14 Line items on old versions of a quote cannot be updated
			-6 Missing UserID
			-15 StatusID is required
   Revision History:
			2017.08.04	NA	Added error for missing StatusID and removed automatic default of StatusID
   ============================================= */

CREATE PROCEDURE [dbo].[uspQuoteLineSet]
	@QuoteLineID INT = NULL,
	@QuoteID INT = NULL,
	@QuoteVersionID INT = NULL,
	@ItemListLineID INT = NULL,
	@StatusID INT = NULL,
	@ItemID INT = NULL,
	@CommodityID INT = NULL,
	@AltFor INT = NULL,
	@CustomerLine INT = NULL,
	@CustomerPartNum NVARCHAR(32) = NULL,
	@PartNumber NVARCHAR(32) = NULL,
	@Manufacturer NVARCHAR(128) = NULL,
	@Qty INT = NULL,
	@TargetPrice MONEY = NULL,
	@Price MONEY = NULL,
	@Cost MONEY = NULL,
	@TargetDateCode NVARCHAR(25) = NULL,
	@LeadTimeDays INT = NULL,
	@DateCode NVARCHAR(25) = NULL,
	@PackagingID INT = NULL,
	@ShipDate DATE = NULL,
	@DueDate DATE = NULL,
	@IsRoutedToBuyers BIT = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL

AS
BEGIN
	SET NOCOUNT ON;
	
	IF (ISNULL(@ItemID, 0) = 0 AND ISNULL(@PartNumber, '') = '')
		RETURN -11

	IF @UserID IS NULL
		RETURN -6

	IF ISNULL(@StatusID, 0) = 0
		RETURN -15

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
	
	
	IF ISNULL(@QuoteLineID, 0) = 0
		BEGIN
			IF ISNULL(@QuoteID, 0) = 0 OR ISNULL(@QuoteVersionID, 0) = 0
				RETURN -9
			--Check to make sure the version number given is the latest for the quote
			DECLARE @LatestVersion INT = (SELECT ISNULL(MAX(VersionID), -5) FROM Quotes WHERE QuoteID = @QuoteID)
			IF @QuoteVersionID <> @LatestVersion
				RETURN -13
			
			GOTO InsertLine
		END
	ELSE
		GOTO UpdateLine
	
InsertLine:
	--Get the next LineNum
	DECLARE @LineNum INT = NULL
	IF ISNULL(@AltFor, 0) <> 0
		BEGIN
			SET @LineNum = 
				(SELECT LineNum
				FROM QuoteLines
				WHERE QuoteLineID = @AltFor)
		END
	ELSE
		BEGIN
			SET @LineNum = 
				(SELECT ISNULL(MAX(LineNum), 0) + 1 
				FROM QuoteLines 
				WHERE QuoteID = @QuoteID AND QuoteVersionID = @QuoteVersionID)
		END

	--Create the record
	INSERT INTO QuoteLines (QuoteID, QuoteVersionID, ItemListLineID, StatusID, ItemID, CommodityID, AltFor, LineNum, CustomerLine, CustomerPartNum, PartNumber, PartNumberStrip, Manufacturer, Qty, TargetPrice, Price, Cost, TargetDateCode, DateCode, PackagingID, ShipDate, DueDate, IsRoutedToBuyers, CreatedBy, LeadTimeDays)
	VALUES (@QuoteID, 
			@QuoteVersionID, 
			@ItemListLineID, 
			@StatusID, 
			CASE WHEN @ItemID = 0 THEN NULL ELSE @ItemID END, --ItemID
			@CommodityID,
			CASE WHEN @AltFor = 0 THEN NULL ELSE @AltFor END, --AltFor 
			CASE WHEN ISNULL(@AltFor, 0) <> 0 THEN 0 ELSE @LineNum END, --LineNum.  Lines that are alternates do not get assigned a line number.
			CASE WHEN ISNULL(@CustomerLine,0) = 0 THEN @LineNum ELSE @CustomerLine END, --CustomerLine
			@CustomerPartNum, 
			@PartNumber, 
			@PartNumberStrip, 
			@Manufacturer, 
			@Qty, 
			@TargetPrice, 
			@Price, 
			@Cost, 
			@TargetDateCode,
			@DateCode, 
			@PackagingID, 
			@ShipDate, 
			@DueDate, 
			ISNULL(@IsRoutedToBuyers, 0), --IsRoutedToBuyers 
			@UserID, --CreatedBy
			@LeadTimeDays)
			
	SET @QuoteLineID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -10
	GOTO ReturnSelect

UpdateLine:	
	--Get the QuoteID and VersionID of the line to be updated
	SELECT  @QuoteVersionID = QuoteVersionID, 
			@QuoteID = QuoteID 
	FROM QuoteLines 
	WHERE QuoteLineID = @QuoteLineID
	
	--If the line is not on the most recent version, return an error
	DECLARE @LatestVersionUpdate INT = (SELECT ISNULL(MAX(VersionID), -5) FROM Quotes WHERE QuoteID = @QuoteID)
	IF @QuoteVersionID <> @LatestVersionUpdate
		RETURN -14

	--Update the record
	UPDATE QuoteLines
	SET	ItemListLineID = ISNULL(@ItemListLineID,ItemListLineID),
		StatusID =ISNULL(@StatusID,StatusID),
		ItemID = ISNULL(@ItemID,ItemID), --CASE WHEN @ItemID = 0 THEN NULL ELSE ISNULL(@ItemID,ItemID) END, --ItemID
		CommodityID = ISNULL(@CommodityID,CommodityID),
		--AltFor = CASE WHEN @AltFor = 0 THEN NULL ELSE @AltFor END, --AltFor  Cannot move alterantes to other lines or remove/add alternate status
		CustomerLine = ISNULL(@CustomerLine,CustomerLine),
		CustomerPartNum = ISNULL(@CustomerPartNum,CustomerPartNum),
		PartNumber = ISNULL(@PartNumber,PartNumber),
		PartNumberStrip = ISNULL(@PartNumberStrip,PartNumberStrip),
		Manufacturer = ISNULL(@Manufacturer,Manufacturer),
		LeadTimeDays = @LeadTimeDays, 
		Qty = ISNULL(@Qty,Qty),
		TargetPrice = ISNULL(@TargetPrice,TargetPrice),
		Price = ISNULL(@Price,Price),
		Cost = ISNULL(@Cost, Cost),
		TargetDateCode = ISNULL(@TargetDateCode, TargetDateCode),
		DateCode = ISNULL(@DateCode,DateCode),
		PackagingID = ISNULL(@PackagingID, PackagingID),
		ShipDate = ISNULL(@ShipDate, ShipDate),
		DueDate = ISNULL(@DueDate, DueDate),
		IsRoutedToBuyers = @IsRoutedToBuyers,
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE QuoteLineID = @QuoteLineID

	IF (@@ROWCOUNT=0)
		RETURN -12
	GOTO ReturnSelect

ReturnSelect:
	SELECT @QuoteLineID 'QuoteLineID'
END
