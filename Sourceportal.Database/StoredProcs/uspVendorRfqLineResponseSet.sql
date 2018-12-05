/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.09.14
   Description:	Inserts or updates a line response on a Vendor RFQ
   Usage:	EXEC uspVendorRfqLineResponseSet @SourceId = 12345 [...]		
   			
   Return Codes:
			-3 Missing UserID
			-12 Missing RFQID 
			-13 Error inserting new response			
			-14	Missing Part number
   Revision History:
			2018.04.03	NA	Added SourceJoin for the quote line the VRFQLine was created from, removed hardcode for USD
			2018.04.19	AR	Changed ValidForHours from INT to DECIMAl
			2018.04.27	AR	Added @ItemID and @IsNoStock support
			2018.05.01	AR	Added NULL value set for @IsNoStock
   ============================================= */

CREATE PROCEDURE [dbo].[uspVendorRfqLineResponseSet]
	@SourceId INT = NULL,
	@VRfqLineId INT = NULL,
	@Cost MONEY = NULL,
	@DateCode NVARCHAR(25) = NULL,
	@Manufacturer NVARCHAR(128) = NULL,
	@PackagingId INT = NULL,
	@PartNumber NVARCHAR(32) = NULL,	
	@ItemID NVARCHAR(32) = NULL,	
	@Qty INT = NULL,	
	@Moq INT = NULL,	
	@Spq INT = NULL,	
	@IsDeleted BIT = NULL,
	@UserID INT = NULL,
	@LeadTimeDays INT = NULL,
	@IsNoStock BIT = NULL,
	@ValidForHours DECIMAL(18,2) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @UserID IS NULL
		RETURN -3
	
	IF ISNULL(@SourceId, 0) = 0
		BEGIN
			IF ISNULL(@VRfqLineId, 0) = 0
				RETURN -12
		END

	--Get required data from Vendor RFQ lines and Vendor RFQ
 	DECLARE @CommodityId INT = NULL
	DECLARE @AccountId INT = NULL
	DECLARE @ContactId INT = NULL
	DECLARE @CurrencyID CHAR(3) = NULL
	DECLARE @QuoteLineID INT = NULL

	DECLARE @OriginalSourceId INT = NULL
	SET @OriginalSourceId = @SourceId

	SELECT	@CommodityId = CommodityID, 
			@AccountId = VRFQ.AccountID, 
			@ContactId = VRFQ.ContactID,
			@CurrencyID = ISNULL(VRFQ.CurrencyID, 'USD'),
			@QuoteLineID = VRFQL.QuoteLineID
	FROM VendorRFQLines VRFQL 
		INNER JOIN VendorRFQs VRFQ ON VRFQ.VendorRFQID = VRFQL.VendorRFQID
	WHERE VRFQLineID = @VRfqLineId 
	
	--Set null values for IsNoStock
	IF (@IsNoStock = 1)
	BEGIN
		SET @PackagingID = NULL
		SET @MOQ = NULL
		SET @SPQ = NULL
		SET @SPQ = NULL
		SET @LeadTimeDays = NULL
		SET @validforHours = NULL
	END

	DECLARE @ReturnStatus INT = 0
	EXEC @ReturnStatus = [dbo].[uspSourceSet] 
		@SourceID = @SourceID OUTPUT,
		@ItemID = @ItemID,
		@SourceTypeID = 7,
		@CommodityID = @CommodityId,
		@AccountID = @AccountID,
		@ContactID = @ContactId,
		@CurrencyID = @CurrencyID,
		@PartNumber = @PartNumber,
		@Manufacturer = @Manufacturer,
		@Qty = @Qty,
		@Cost = @Cost,
		@DateCode = @DateCode,
		@PackagingID = @PackagingId,
		@MOQ = @Moq,
		@SPQ = @Spq,
		@LeadTimeDays = @LeadTimeDays,
		@ValidForHours = @ValidForHours,
		@IsNoStock = @IsNoStock,
		@UserID = @UserID
			
	IF @ReturnStatus< 0
		RETURN @ReturnStatus

	IF @OriginalSourceId = 0
	BEGIN
		--Map the source and the VRFQLine
		INSERT INTO MapSourcesJoin(ObjectTypeId, ObjectId, SourceId, CreatedBy)
		VALUES (28, @VRfqLineId,@SourceID, @UserID)
		--Map the source to the Quote line it was originally created for (if any)
		IF @QuoteLineID IS NOT NULL
			INSERT INTO mapSourcesJoin (ObjectTypeID, ObjectID, SourceID, CreatedBy) VALUES (20, @QuoteLineID, @SourceID, @UserID)
	END

	SELECT @SourceId 'SourceId'
END