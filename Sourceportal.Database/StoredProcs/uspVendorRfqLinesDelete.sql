/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.09.12
   Description:	Deletes one or more Rfq Lines
   Usage:	EXEC uspRfqLinesDelete @RFQLinesJSON = '[{"SOLineID":16}, {"SOLineID":88}]', @UserID = 0		
   Return Codes:
			-11 Missing JSON list of Rfq Lines to be deleted
			-3 Missing UserID
   Revision History:
			
   ============================================= */
   
CREATE PROCEDURE [dbo].[uspVendorRfqLinesDelete]
	@RFQLinesJSON VARCHAR(MAX) = NULL,
	@UserID INT = NULL,
	@ResultCount INT = NULL OUTPUT 
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@RFQLinesJSON, '') = ''
		RETURN -11

	IF ISNULL(@UserID, 0) = 0
		RETURN -3

	UPDATE rfqLine
	SET IsDeleted = 1,
		Modified = GETUTCDATE(),
		ModifiedBy = @UserID
	FROM VendorRFQLines rfqLine
	  INNER JOIN OPENJSON(@RFQLinesJSON) WITH (RfqLineID INT) AS j ON rfqLine.VRFQLineID = j.RFQLineID 

	SET @ResultCount = @@ROWCOUNT
END
