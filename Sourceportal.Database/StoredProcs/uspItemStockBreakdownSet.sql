/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.06.28
   Description:	Inserts or updates a record in the ItemStockBreakdown table
   Usage: 
   Revision History:
		2018.11.01	NA	Added MfrLotNum
   Return Codes:
		-1	Error creating new stock record

   ============================================= */
CREATE OR ALTER PROCEDURE [dbo].[uspItemStockBreakdownSet]
(
	@BreakdownID INT = NULL,
	@StockID INT,
	@IsDiscrepant BIT = NULL,	
	@DateCode VARCHAR(50) = NULL,
	@PackQty INT = NULL,
	@NumPacks INT = NULL,
	@PackagingID INT = NULL,
	@PackageConditionID INT = NULL,	
	@COO INT = NULL,
	@Expiry DATE = NULL,
	@MfrLotNum VARCHAR(50) NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL	
)
AS
BEGIN
	SET NOCOUNT ON;

	IF (ISNULL(@StockID,0) = 0)
		RETURN -1
	ELSE IF (ISNULL(@BreakdownID, 0) = 0)
		GOTO InsertStockBreakdown
	ELSE 
		GOTO UpdateStockBreakdown

InsertStockBreakdown:
	INSERT INTO ItemStockBreakdown(StockID, IsDiscrepant, PackQty, NumPacks, DateCode, PackagingID, PackageConditionID, COO, Expiry, MfrLotNum, CreatedBy)
	VALUES (	@StockID,
				ISNULL(@IsDiscrepant, 0),
				@PackQty,
				@NumPacks,
				@DateCode,
				@PackagingID,
				@PackageConditionID,
				@COO,
				@Expiry,
				@MfrLotNum,
				@UserID	
		)
	SET @BreakdownID = SCOPE_IDENTITY()

	IF (ISNULL(@BreakdownID,0) = 0)
		RETURN -2

	GOTO SelectOutput

DeleteStock:
	--Delete the stock and its inventory and source

UpdateStockBreakdown:	
	UPDATE ItemStockBreakdown
	SET StockID = ISNULL(@StockID, StockID),
		IsDiscrepant = ISNULL(@IsDiscrepant, IsDiscrepant),
		DateCode = ISNULL(@DateCode, DateCode),
		PackagingID = ISNULL(@PackagingID, PackagingID),	
		PackageConditionID = ISNULL(@PackageConditionID, PackageConditionID),	
		COO = ISNULL(@COO, COO),
		Expiry = ISNULL(@Expiry, Expiry),
		MfrLotNum = ISNULL(@MfrLotNum, MfrLotNum),
		PackQty = ISNULL(@PackQty, PackQty),		
		NumPacks = ISNULL(@NumPacks, NumPacks),	
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,		
		Modified = GETUTCDATE()  
	WHERE BreakdownID = @BreakdownID
	  
	GOTO SelectOutput
	
SelectOutput:
	SELECT @BreakdownID 'BreakdownID'	
END