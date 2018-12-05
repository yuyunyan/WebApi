/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.26
   Description:	Sets details for a given Item or creates a new item
   Usage: EXEC uspItemSet
   Revision History:
       2017.11.08  CT  Added HTS and MSL fields to Insert and Update.
   Return Codes:
			-1 Missing UserID
			-2 Missing PartNumber
			-3 Missing CommodityID
			-4 Missing StatusID
			-5 Error in Update
			-6 Error in Insert
   ============================================= */


CREATE PROCEDURE [dbo].[uspItemSet]
	@ItemID INT = NULL,
	@MfrID INT = NULL,
	@CommodityID INT = NULL,
	@ItemStatusID INT = NULL,
	@SourceDataID NVARCHAR(50) = NULL,
	@ExternalID NVARCHAR(32) = NULL,
	@PartNumber NVARCHAR(32) = NULL,	
	@PartDescription NVARCHAR(250) = NULL,
	@MfrDescription NVARCHAR(250) = NULL,
	@EURoHS Bit = NULL,
	@CNRoHS Bit = NULL,
	@ECCN NVARCHAR(20) = NULL,
	@WeightG FLOAT = NULL,
	@LengthCM FLOAT = NULL,
	@WidthCM FLOAT = NULL,
	@DepthCM FLOAT = NULL,
	@DatasheetURL NVARCHAR(500) = NULL,
	@IsDeleted BIT = NULL,
	@HTS VARCHAR(50) = NULL,
	@MSL VARCHAR(20) = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@UserID, 0) = 0
		RETURN -1
	IF ISNULL(@PartNumber, '') = ''
		RETURN -2
	IF ISNULL(@CommodityID, 0) = 0
		RETURN -3
	IF ISNULL(@ItemStatusID, 0) = 0
		RETURN -4		

	IF ISNULL(@ItemID, 0) = 0
		GOTO InsertItem
	ELSE
		GOTO UpdateItem

InsertItem:
	
	INSERT INTO Items (MfrID, CommodityID, ItemstatusID, SourceDataID, ExternalID, PartNumber, PartNumberStrip, PartDescription, MfrDescription, EURoHS, CNRoHS, ECCN, WeightG, LengthCM, WidthCM, DepthCM, DatasheetURL, HTS, MSL, CreatedBy)
	VALUES (		
		@MfrID,
		@CommodityID,
		@ItemStatusID,
		@SourceDataID,
		@ExternalID,
		@PartNumber,
		dbo.fnStripNonAlphaNumeric(@PartNumber), --PartNumberStrip
		@PartDescription,
		@MfrDescription,
		@EURoHS,
		@CNRoHS,
		@ECCN,
		@WeightG,
		@LengthCM,
		@WidthCM,
		@DepthCM,
		@DatasheetURL,
		@HTS,
		@MSL,
		@UserID
	)

	SET @ItemID = SCOPE_IDENTITY()

	IF ISNULL(@ItemID, 0) = 0
		RETURN -6

	GOTO ReturnSelect

UpdateItem:
	
	UPDATE Items
	SET
		MfrID = @MfrID,
		CommodityID = @CommodityID,
		ItemStatusID = @ItemStatusID,
		SourceDataID = @SourceDataID,
		ExternalID = @ExternalID,
		PartNumber = @PartNumber,
		PartNumberStrip = dbo.fnStripNonAlphaNumeric(@PartNumber),
		PartDescription = @PartDescription,
		MfrDescription = @MfrDescription,
		EURoHS = @EURoHS,
		CNRoHS = @CNRoHS,
		ECCN = @ECCN,
		WeightG = @WeightG,
		LengthCM = @LengthCM,
		WidthCM = @WidthCM,
		DepthCM = @DepthCM,
		DatasheetURL = @DatasheetURL,
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		HTS = @HTS,
		MSL = @MSL,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE ItemID = @ItemID

	IF (@@ROWCOUNT=0)
		RETURN -5

	GOTO ReturnSelect

ReturnSelect:
	SELECT @ItemID 'ItemID'
END
