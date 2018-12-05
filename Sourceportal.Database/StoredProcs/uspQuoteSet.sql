/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.20
   Description:	Inserts or updates Quote information from the Quotes table
   Usage:	EXEC uspQuoteSet @AccountID = 4, @ContactID = 3, @StatusID = 4, @UserID = 0
			
   Return Codes:
			-2 New version insert failed
			-3 New quote insert failed
			-4 Update failed, check QuoteID and VersionID
			-5 Insert of new quote or new version failed, no provided or default Status
			-6 UserID is required
			-7 Invalid QuoteID for new version insert
   Revision History:
			2017.06.29  NA  Renamed @IncotermID, @PaymentTermID, @CurrencyID.  Updated CurrencyID datatype to CHAR(3)
			2017.07.20  NA  Added logic to copy Lines, Extras and Source joins when creating a new version of an existing quote
			2017.09.13	BZ	Remove 'Note' from InsertNewVersion
			2018.02.06  CT  Added @IncotermLocation
   ============================================= */

CREATE PROCEDURE [dbo].[uspQuoteSet]
	@QuoteID INT = NULL OUTPUT,
	@VersionID INT = NULL OUTPUT,
	@AccountID INT = NULL,
	@ContactID INT = NULL,
	@ProjectID INT = NULL,
	@ItemListID INT = NULL,
	@StatusID INT = NULL,
	@IncotermID INT = NULL,
	@PaymentTermID INT = NULL,
	@CurrencyID CHAR(3) = NULL,
	@ShipLocationID INT = NULL,
	@ShippingMethodID INT = NULL,
	@OrganizationID INT = NULL,
	@QuoteTypeID INT = NULL,
	@SentDate DATETIME = NULL,
	@ValidForHours INT = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL,
	@IncotermLocation NVARCHAR(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	--Store the Quote ObjectTypeID
	DECLARE @ObjectTypeID INT = 19
	DECLARE @LineObjectTypeID INT = 20

	--Get a default status ID for new quotes or versions
	IF ISNULL(@StatusID, 0) = 0
		SET @StatusID = (SELECT TOP 1 StatusID FROM lkpStatuses WHERE ObjectTypeID = @ObjectTypeID AND IsDefault = 1 AND IsDeleted = 0)
	IF ISNULL(@StatusID, 0) = 0
		RETURN -5
	
	IF @UserID IS NULL
		RETURN -6

	IF ISNULL(@QuoteID, 0) = 0
		GOTO InsertNewQuote		
	ELSE
		BEGIN
			IF ISNULL(@VersionID, 0) = 0
				GOTO InsertNewVersion
			ELSE
				GOTO UpdateQuote
		END

InsertNewVersion:
	
	DECLARE @NewVersionCount INT
	SET @VersionID = (SELECT COALESCE(MAX(VersionID), 0) + 1 FROM Quotes WHERE QuoteID = @QuoteID)
		
	IF ISNULL(@VersionID, 1) = 1
		RETURN -7

	--Create the new version of the Quote
	SET IDENTITY_INSERT Quotes ON	
	INSERT INTO Quotes (QuoteID, VersionID, AccountID, ContactID, ProjectID, ItemListID, StatusID, IncotermID, PaymentTermID, CurrencyID, ShipLocationID, ShippingMethodID, OrganizationID,QuoteTypeID, SentDate, ValidForHours, CreatedBy, IncotermLocation)
	VALUES (@QuoteID, @VersionID, @AccountID, @ContactID, @ProjectID, @ItemListID, @StatusID, @IncotermID, @PaymentTermID, @CurrencyID, @ShipLocationID, @ShippingMethodID, @OrganizationID, @QuoteTypeID, @SentDate, @ValidForHours, @UserID, @IncotermLocation)
	SET @NewVersionCount = @@ROWCOUNT
	SET IDENTITY_INSERT Quotes OFF

	IF (@NewVersionCount=0)
		RETURN -2

	--Copy the Quote Lines from the previous version to the new one	
	DECLARE @InsertKeys TABLE (New INT, Old INT, AltFor INT)
	
	MERGE QuoteLines
	USING (SELECT * FROM QuoteLines WHERE QuoteID = @QuoteID AND QuoteVersionID = @VersionID - 1 AND IsDeleted = 0) s
	ON 0=1
	WHEN NOT MATCHED THEN
		INSERT (QuoteID, QuoteVersionID, ItemListLineID, StatusID, ItemID, CommodityID, AltFor, LineNum, CustomerLine, CustomerPartNum, PartNumber, PartNumberStrip, Manufacturer, Qty, TargetPrice, Price, Cost, TargetDateCode, DateCode, PackagingID, ShipDate, DueDate, IsRoutedToBuyers, CreatedBy)
		VALUES (@QuoteID, @VersionID, s.ItemListLineID, s.StatusID, s.ItemID, s.CommodityID, s.AltFor, s.LineNum, s.CustomerLine, s.CustomerPartNum, s.PartNumber, s.PartNumberStrip, s.Manufacturer, s.Qty, s.TargetPrice, s.Price, s.Cost, s.TargetDateCode, s.DateCode, s.PackagingID, s.ShipDate, s.DueDate, s.IsRoutedToBuyers, @UserID)
		OUTPUT INSERTED.QuoteLineID, s.QuoteLineID, s.AltFor INTO @InsertKeys;
	
	--Update the newly inserted Quote Lines with the new AltFor values
	UPDATE ql
	SET ql.AltFor = AltKey.New
	FROM QuoteLines ql
	  INNER JOIN @InsertKeys ik ON ql.QuoteLineID = ik.New
	  INNER JOIN @InsertKeys AltKey ON ik.AltFor = AltKey.Old
	WHERE ql.QuoteID = @QuoteID AND ql.QuoteVersionID = @VersionID
	
	--Copy the Source Joins that were linked to the old Quote Lines to the new Quote Lines
	INSERT INTO mapSourcesJoin (ObjectTypeID, ObjectID, SourceID, IsMatch, CreatedBy)
	SELECT @LineObjectTypeID, ik.New, sj.SourceID, sj.IsMatch, @UserID
	FROM mapSourcesJoin sj
	  INNER JOIN @InsertKeys ik ON sj.ObjectID = ik.Old AND sj.ObjectTypeID = @LineObjectTypeID
	  INNER JOIN Sources s ON sj.SourceID = s.SourceID
	WHERE sj.IsDeleted = 0 AND s.IsDeleted = 0

	--Copy the Quote Extras from the previous version to the new one
	INSERT INTO QuoteExtras (QuoteID, QuoteVersionID, StatusID, ItemExtraID, LineNum, RefLineNum, Qty, Price, Cost, PrintOnQuote, Note, CreatedBy)
	SELECT					@QuoteID, @VersionID,	  StatusID, ItemExtraID, LineNum, RefLineNum, Qty, Price, Cost, PrintOnQuote, Note, @UserID
	FROM QuoteExtras
	WHERE QuoteID = @QuoteID AND QuoteVersionID = @VersionID - 1 AND IsDeleted = 0
	
	GOTO ReturnSelect

InsertNewQuote:
	
	SET @VersionID = 1

	INSERT INTO Quotes (VersionID, AccountID, ContactID, ProjectID, ItemListID, StatusID, IncotermID, PaymentTermID, CurrencyID, ShipLocationID, ShippingMethodID, OrganizationID, QuoteTypeID, SentDate, ValidForHours, CreatedBy, IncotermLocation)
	VALUES (@VersionID, @AccountID, @ContactID, @ProjectID, @ItemListID, @StatusID, @IncotermID, @PaymentTermID, @CurrencyID, @ShipLocationID, @ShippingMethodID, @OrganizationID, @QuoteTypeID, @SentDate, @ValidForHours, @UserID, @IncotermLocation)
	
	SET @QuoteID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -3
	
	--Copy ownership from the selected Contact
	INSERT INTO mapOwnership (OwnerID, ObjectTypeID, ObjectID, IsGroup, [Percent], CreatedBy)
	SELECT OwnerID, @ObjectTypeID, @QuoteID, IsGroup, [Percent], @UserID
	FROM mapOwnership
	WHERE ObjectID = @ContactID AND ObjectTypeID = 2 AND IsDeleted = 0

	GOTO ReturnSelect

UpdateQuote:
	UPDATE Quotes
		SET AccountID = @AccountID,
			ContactID = @ContactID,
			ProjectID = @ProjectID,
			ItemListID = @ItemListID,
			StatusID = @StatusID,
			IncotermID = @IncotermID,
			PaymentTermID = @PaymentTermID,
			CurrencyID = @CurrencyID,
			ShipLocationID = @ShipLocationID,
			ShippingMethodID = @ShippingMethodID,
			OrganizationID = @OrganizationID,
			QuoteTypeID = @QuoteTypeID,
			SentDate = @SentDate,
			ValidForHours = @ValidForHours,
			IncotermLocation = @IncotermLocation,
			IsDeleted = ISNULL(@IsDeleted, IsDeleted),
			Modified = GETUTCDATE(),
			ModifiedBy = @UserID
		WHERE QuoteID = @QuoteID AND VersionID = @VersionID

	IF (@@ROWCOUNT=0)
		RETURN -4
	GOTO ReturnSelect

ReturnSelect:
	SELECT @QuoteID 'QuoteID', @VersionID 'VersionID'
END