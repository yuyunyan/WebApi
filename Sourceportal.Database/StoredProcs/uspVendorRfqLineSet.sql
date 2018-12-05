/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.09.08
   Description:	Inserts or updates a line item on a Vendor RFQ
   Usage:	EXEC [uspRfqLineSet] @VRfqLineId = 12345 [...]		
   			exec uspVendorRfqLineSet @VRfqLineId=3,@VRfqId=0,@CommodityId=2,@DateCode=N'121215',@Manufacturer=N'Analog Devices1',@Note=N'aasdasd',@PackagingId=1,@PartNumber=N'AD811ANZ',@Qty=208,@TargetCost=25.5,@UserID=1	
   Return Codes:
			-3 Missing UserID
			-6 Missing RFQID 
			-7 Error inserting new rfq line
			-8 ItemID is required
			-9	Missing both ItemID and PartNumber, at least one must be provided	
			-10 Error updating record		
   Revision History:
			2018.04.03	NA	Added @QuoteLineID
			2018.04.24	AR	Added @ItemID, moved to previous itemID logic to IF INSULL(@ItemID) statement
   ============================================= */

CREATE PROCEDURE [dbo].[uspVendorRfqLineSet]
	@VRfqLineId INT = NULL,
	@VRfqId INT = NULL,
	@QuoteLineId INT = NULL,
	@CommodityId INT = NULL,
	@DateCode NVARCHAR(25) = NULL,
	@Manufacturer NVARCHAR(128) = NULL,
	@Note NVARCHAR(500) = NULL,	
	@PackagingId INT = NULL,
	@PartNumber NVARCHAR(32) = NULL,	
	@ItemID INT = NULL,	
	@Qty INT = NULL,	
	@TargetCost MONEY = NULL,
	@IsDeleted BIT = 0,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	IF (ISNULL(@ItemID, 0) = 0 AND ISNULL(@PartNumber, '') = '')
		RETURN -0

	IF ISNULL(@UserID, 0) = 0
		RETURN -3

	--Get known ItemID information	
	DECLARE @PartNumberStrip NVARCHAR(32) = NULL
	IF (ISNULL(@ItemID, 0) = 0)
	BEGIN
		SELECT TOP 1
			@PartNumberStrip = i.PartNumberStrip
			, @ItemId = i.ItemID
			FROM Items I
			WHERE I.PartNumber = @PartNumber
	END

	IF ISNULL(@PartNumberStrip, '') = ''
	BEGIN
		Select @PartNumberStrip = dbo.[fnStripNonAlphaNumeric](@PartNumber)
	END
	
		
	IF ISNULL(@VRfqLineId, 0) = 0
		BEGIN
			IF ISNULL(@VRfqId, 0) = 0
				RETURN -6
			
			GOTO InsertLine
		END
	ELSE
		GOTO UpdateLine

 InsertLine:
	--Get the next LineNum
	DECLARE @LineNum INT = NULL
	SET @LineNum = 
		(SELECT ISNULL(MAX(LineNum), 0) + 1 
		FROM VendorRFQLines 
		WHERE VendorRFQID = @VRfqId)
	
	--Get the default status
	DECLARE @StatusID INT = NULL
	SET @StatusID = (SELECT TOP 1 ISNULL(StatusID, 0)
					FROM lkpStatuses 
					WHERE IsDeleted = 0 
					  AND IsDefault = 1 
					  AND ObjectTypeID = 28)  --ID of the Vendor RFQ Line object type

	--Create the record

	INSERT INTO VendorRFQLines (VendorRFQID, QuoteLineID, CommodityId, DateCode, Manufacturer, Note, PackagingId, 
	PartNumber, Qty, TargetCost, CreatedBy, PartNumberStrip, LineNum,StatusID, ItemID)
	VALUES (
	@VRfqId,
	@QuoteLineId,
	@CommodityId,
	@DateCode,
	@Manufacturer,
	@Note,	
	@PackagingId,
	@PartNumber,	
	@Qty,	
	@TargetCost,
	@UserID,
	@PartNumberStrip, 
	@LineNum,
	@StatusID,
	@ItemId
	)
			
	SET @VRfqLineId = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -7
	GOTO ReturnSelect

UpdateLine:	

	--Update the record
	UPDATE VendorRFQLines
	SET	
	QuoteLineID = @QuoteLineId,
	CommodityId = @CommodityId,
	DateCode = @DateCode,
	Manufacturer = @Manufacturer,
	Note= @Note	,
	PackagingId = @PackagingId,
	PartNumber = @PartNumber,	
	Qty = @Qty,	
	TargetCost = @TargetCost,
	IsDeleted =  ISNULL(@IsDeleted, IsDeleted),
	PartNumberStrip= @PartNumberStrip,
	ModifiedBy = @UserID,
	ItemID = @ItemId,
	Modified = GETUTCDATE()

	WHERE VRfqLineId = @VRfqLineId

	IF (@@ROWCOUNT=0)
		RETURN -10
	GOTO ReturnSelect

ReturnSelect:
	SELECT @VRfqLineId 'VRfqLineId'
END
