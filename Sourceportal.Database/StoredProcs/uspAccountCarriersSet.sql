/* =============================================
   Author:		Julia Thomas
   Create date: 2018.05.23
   Description:	Inserts or updates a carrier info on mapAccountCarriers table
   Usage:	EXEC uspAccountCarriersSet @AccountID=325,@CarrierID = 2,@AccountNumber=10,@isDefault=false,@UserID=3
   Return Codes:
			-11 Error updating record
			-6 Missing UserID
   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspAccountCarriersSet]
	@CarrierID INT = NULL,
	@AccountID INT = NULL,
	@AccountNumber VARCHAR(128)= NULL,
	@isDefault BIT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @UserID IS NULL
		RETURN -6
	--Delete an existing record before inserting a new one
	DELETE mapAccountCarriers WHERE AccountID = @AccountID AND CarrierID = @CarrierID
	
	--Create the record
	INSERT INTO mapAccountCarriers(AccountID,CarrierID, AccountNumber, isDefault, Created, CreatedBy)
	VALUES (@AccountID, 
			@CarrierID,			
			@AccountNumber, 
			@isDefault,
			GETUTCDATE(),		
			@UserID) 			
	SET @CarrierID = SCOPE_IDENTITY()
	SELECT @@ROWCOUNT 'RowCount'
END