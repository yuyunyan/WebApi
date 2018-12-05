/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.02.06
   Description:	updates with new SAP data
   Usage:	EXEC [uspLocationSapDataSet] @LocationID = 1, @ExternalId = '345', @UserID = 1		
   Return Codes:

   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspLocationSapDataSet]
	@LocationID INT,
	@ExternalId VARCHAR(50),
	@UserID INT
AS
BEGIN
	SET NOCOUNT ON;
	--Update the record
	UPDATE Locations
	SET		
		ExternalId = @ExternalId,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE LocationID = @LocationID
END