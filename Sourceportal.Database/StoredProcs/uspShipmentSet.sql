/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.05.31
   Description:	Inserts/updates record in Shipments table
   Usage: EXEC 
   Revision History:
		2018.08.08	NA	Added modified date, changed @UserID to INT, Removed IsNull check for @UserID on modify
   Return Codes:

   ============================================= */

ALTER PROCEDURE [dbo].[uspShipmentSet]
	@ShipmentID INT = NULL OUTPUT,
	@ExternalID VARCHAR(50),
	@ExternalUUID VARCHAR(50),
	@CarrierName VARCHAR(200) = NULL,
	@TrackingNumber VARCHAR(200) = NULL,
	@TrackingURL VARCHAR(1000) = NULL,
	@ShipDate DATE = NULL,
	@UserID INT,
	@IsDeleted BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

UpdateInsertHandle:
		IF (ISNULL(@ShipmentID, 0) = 0)
			GOTO InsertShipment
		ELSE
			GOTO UpdateShipment

InsertShipment:
	INSERT INTO Shipments (ExternalID, ExternalUUID, CarrierName, TrackingNumber, TrackingURL, ShipDate, CreatedBy, IsDeleted)
	VALUES(@ExternalID, @ExternalUUID, @CarrierName, @TrackingNumber, @TrackingURL, @ShipDate, @UserID, @IsDeleted)

	SET @ShipmentID = @@identity
	IF (@ShipmentID = 0)
		RETURN -4

	RETURN @ShipmentID

UpdateShipment:
	UPDATE Shipments
		SET ExternalID = @ExternalID,
			ExternalUUID = @ExternalUUID,
			CarrierName = ISNULL(@CarrierName, CarrierName),
			TrackingNumber = ISNULL(@TrackingNumber, TrackingNumber),
			TrackingURL = ISNULL(@TrackingURL, TrackingURL),
			ShipDate = ISNULL(@ShipDate, ShipDate),
			Modified = GETUTCDATE(),
			ModifiedBy = @UserID,
			IsDeleted = ISNULL(@IsDeleted, IsDeleted)
	WHERE ShipmentID = @ShipmentID

	IF (@@ROWCOUNT = 0)
		RETURN -4
	
	RETURN @ShipmentID

END