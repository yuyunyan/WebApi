/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.01.03
   Description:	Inserts or updates a line item on a Purchase Order
   Usage:	EXEC uspAccountFocusSet @FocusID = 1 [...]	
   Return Codes:
			-11 Error updating record
			-6 Missing UserID
   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspAccountFocusSet]
	@FocusID INT = NULL,
	@AccountID INT = NULL,
	@FocusTypeID INT = NULL,
	@FocusObjectTypeID INT = NULL,	
	@ObjectID INT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @UserID IS NULL
		RETURN -6
	
	IF ISNULL(@FocusID, 0) = 0
		GOTO InsertLine
	ELSE
		GOTO UpdateLine
	
InsertLine:
	--Create the record
	INSERT INTO mapAccountFocuses(AccountID, FocusTypeID, ObjectID, FocusObjectTypeID, Created, CreatedBy)
	VALUES (@AccountID, 
			@FocusTypeID, 			
			@ObjectID, 
			@FocusObjectTypeID,
			GETUTCDATE(),		
			@UserID) 
			
	SET @FocusID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -9
	GOTO ReturnSelect

UpdateLine:	
	--Update the record
	UPDATE mapAccountFocuses
	SET	
		AccountID = @AccountID,
		FocusTypeID = @FocusTypeID,		
		ObjectID = @ObjectID,		
        FocusObjectTypeID = @FocusObjectTypeID,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE FocusID = @FocusID

	IF (@@ROWCOUNT=0)
		RETURN -11
	GOTO ReturnSelect

ReturnSelect:
	SELECT @@ROWCOUNT 'RowCount'
END
GO

