/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.11.15
   Description:	Creates an Item with IHS data
   Usage: EXEC uspItemCreateFromIHS 
   Revision History:
   Return Codes:
   ============================================= */
   
CREATE PROCEDURE [dbo].[uspItemCreateFromIHS]
	@MPN VARCHAR(32) = NULL,
	@Mfr VARCHAR(500) = NULL,
	@PartDescription VARCHAR(250) = NULL,
	@MfrPackageDescription VARCHAR(250) = NULL,
	@ReachCompliant VARCHAR(10) = NULL,
	@EuRohs VARCHAR(10) = NULL,
	@ChinaRohs VARCHAR(10) = NULL,
	@ManufacturerURL VARCHAR(500) = NULL,
	@Status VARCHAR(50) = NULL,
	@GenericNumber VARCHAR(32) = NULL,
	@ObjectID VARCHAR(50) = NULL,
	@Category VARCHAR(100) = NULL,
	@DatasheetURL VARCHAR(500) = NULL,
	@UserID INT = NULL
AS
BEGIN
	DECLARE 
	@MfrID INT = 0,
	@CommodityID INT = 0,
	@ItemStatusID INT = 0
	
	SELECT TOP 1 @MfrID = MfrID
	FROM Manufacturers
	WHERE MfrName = @Mfr AND IsDeleted = 0

	SELECT TOP 1 @CommodityID = CommodityID
	FROM lkpItemCommodities
	WHERE CommodityName = @Category AND IsDeleted = 0

	SELECT TOP 1 @ItemStatusID = ItemStatusID
	FROM lkpItemStatuses
	WHERE StatusName = @Status AND IsDeleted = 0

	INSERT INTO Items (MfrID, CommodityID, ItemStatusID, SourceDataID, PartNumber, PartNumberStrip, PartDescription, MfrDescription, EURoHS, CNRoHS, DatasheetURL, CreatedBy)
	VALUES (
				@MfrID,
				@CommodityID,
				@ItemStatusID,
				@ObjectID,
				@MPN,
				dbo.fnStripNonAlphaNumeric(@MPN),
				@PartDescription,
				@MfrPackageDescription,
				CASE WHEN @EuRohs = 'YES' THEN 1 ELSE 0 END,
				CASE WHEN @ChinaRohs = 'YES' THEN 1 ELSE 0 END,
				@DatasheetURL,
				ISNULL(@UserID, 0)
			)

	SELECT @@IDENTITY AS 'ItemId';  
END
