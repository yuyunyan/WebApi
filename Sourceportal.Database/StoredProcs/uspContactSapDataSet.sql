/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.02.06
   Description:	updates with new SAP data
   Usage:	EXEC [uspContactSapDataSet] @ContactID = 1, @ExternalId = '345', @UserID = 1		
   Return Codes:

   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspContactSapDataSet]
	@ContactID INT,
	@ExternalId VARCHAR(50),
	@UserID INT
AS
BEGIN
	SET NOCOUNT ON;
	--Update the record
	UPDATE Contacts
	SET		
		ExternalId = @ExternalId,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE ContactID = @ContactID
END