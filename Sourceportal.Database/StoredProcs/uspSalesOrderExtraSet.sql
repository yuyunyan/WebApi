/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.25
   Description:	Inserts or updates a Sales Order Extra on a Sales Order
   Usage:		
   Return Codes:
			-1  @SalesOrderID and @SOVersionID are both required to create a new record
			-2	Error on Insert
			-3  @ItemExtraID is required
			-4	Error on Update
			-5  The SOVersionID provided is not the latest SOVersionID for the given SalesOrderID
			-7  @UserID is required
			-12  SalesOrder Extras that are not on the latest version of the SalesOrder cannot be updated
			
   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspSalesOrderExtraSet]
	@SOExtraID INT = NULL,
	@SalesOrderID INT = NULL,
	@SOVersionID INT = NULL,	
	@QuoteExtraID INT = NULL,
	@ItemExtraID INT = NULL,
	@RefLineNum INT = NULL,
	@StatusID INT = NULL,
	@Qty INT = NULL,	
	@Price MONEY = NULL,
	@Cost MONEY = NULL,
	@PrintOnSO BIT = 0,
	@Note NVARCHAR(250) = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @UserID IS NULL
		RETURN -7
	IF ISNULL(@ItemExtraID, 0) = 0
		RETURN -3
		
	IF ISNULL(@SOExtraID, 0) = 0
		BEGIN
			IF ISNULL(@SalesOrderID, 0) = 0 OR ISNULL(@SOVersionID, 0) = 0
				RETURN -1
			--Check to make sure the version number given is the latest for the SalesOrder
			DECLARE @LatestVersion INT = (SELECT ISNULL(VersionID, -5) FROM vwSalesOrders WHERE SalesOrderID = @SalesOrderID)
			IF @SOVersionID <> @LatestVersion
				RETURN -5
			
			GOTO InsertLine
		END
	ELSE
		GOTO UpdateLine
	
InsertLine:
	--Get the next LineNum
	DECLARE @LineNum INT = NULL
	
	SET @LineNum = 
		(SELECT ISNULL(MAX(LineNum), 0) + 1 
		FROM SalesOrderExtras
		WHERE SalesOrderID = @SalesOrderID AND SOVersionID = @SOVersionID)
	
	--Get the default status
	SET @StatusID = (SELECT TOP 1 ISNULL(StatusID, 0)
					FROM lkpStatuses 
					WHERE IsDeleted = 0 
					  AND IsDefault = 1 
					  AND ObjectTypeID = 18)  --ID of the SalesOrder Extra object type

	--Create the record
	INSERT INTO SalesOrderExtras (SalesOrderID, SOVersionID, QuoteExtraID, StatusID, ItemExtraID, LineNum, RefLineNum, Qty, Price, Cost, PrintOnSO, Note, CreatedBy)
	VALUES (@SalesOrderID,
			@SOVersionID,
			@QuoteExtraID,
			@StatusID,
			@ItemExtraID,
			@LineNum,
			@RefLineNum,
			@Qty,
			@Price,
			@Cost,
			@PrintOnSO,
			@Note,
			@UserID) --CreatedBy
			
	SET @SOExtraID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -2
	GOTO ReturnSelect

UpdateLine:	
	--Get the SalesOrderID and VersionID of the line to be updated
	SELECT  @SOVersionID = SOVersionID, 
			@SalesOrderID = SalesOrderID 
	FROM SalesOrderExtras
	WHERE SOExtraID = @SOExtraID
	
	--If the line is not on the most recent version, return an error
	DECLARE @LatestVersionUpdate INT = (SELECT ISNULL(VersionID, -5) FROM vwSalesOrders WHERE SalesOrderID = @SalesOrderID)
	IF @SOVersionID <> @LatestVersionUpdate
		RETURN -12

	--Update the record
	UPDATE SalesOrderExtras
	SET	
		StatusID = @StatusID,
		ItemExtraID = @ItemExtraID,
		RefLineNum = @RefLineNum,
		Qty = @Qty,
		Price = @Price,
		Cost = @Cost,
		PrintOnSO = @PrintOnSO,
		Note = @Note,		
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE SOExtraID = @SOExtraID

	IF (@@ROWCOUNT=0)
		RETURN -4
	GOTO ReturnSelect

ReturnSelect:
	SELECT @SOExtraID 'SOExtraID'
END
