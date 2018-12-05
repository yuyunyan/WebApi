/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.30
   Description:	Inserts or updates a Quote Extra on a Quote
   Usage:			
   Return Codes:
			-9  @QuoteID and @QuoteVersionID are both required to create a new record
			-13  The QuoteVersionID provided is not the latest QuoteVersionID for the given QuoteID
			-16	Error on Insert Quote Extra
			-17  Quote Extras that are not on the latest version of the quote cannot be updated
			-18	Error on Update Quote Extra
			-6  @UserID is required
			-19  @ItemExtraID is required
   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspQuoteExtraSet]
	@QuoteExtraID INT = NULL,
	@QuoteID INT = NULL,
	@QuoteVersionID INT = NULL,	
	@ItemExtraID INT = NULL,
	@RefLineNum INT = NULL,
	@StatusID INT = NULL,
	@Qty INT = NULL,	
	@Price MONEY = NULL,
	@Cost MONEY = NULL,
	@PrintOnQuote BIT = 0,
	@Note NVARCHAR(250) = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF @UserID IS NULL
		RETURN -6
	IF ISNULL(@ItemExtraID, 0) = 0
		RETURN -19
		
	IF ISNULL(@QuoteExtraID, 0) = 0
		BEGIN
			IF ISNULL(@QuoteID, 0) = 0 OR ISNULL(@QuoteVersionID, 0) = 0
				RETURN -9
			--Check to make sure the version number given is the latest for the quote
			DECLARE @LatestVersion INT = (SELECT ISNULL(MAX(VersionID), -5) FROM Quotes WHERE QuoteID = @QuoteID)
			IF @QuoteVersionID <> @LatestVersion
				RETURN -13
			
			GOTO InsertLine
		END
	ELSE
		GOTO UpdateLine
	
InsertLine:
	--Get the next LineNum
	DECLARE @LineNum INT = NULL
	
	SET @LineNum = 
		(SELECT ISNULL(MAX(LineNum), 0) + 1 
		FROM QuoteExtras
		WHERE QuoteID = @QuoteID AND QuoteVersionID = @QuoteVersionID)
	
	--Get the default status
	SET @StatusID = (SELECT TOP 1 ISNULL(StatusID, 0)
					FROM lkpStatuses 
					WHERE IsDeleted = 0 
					  AND IsDefault = 1 
					  AND ObjectTypeID = 21)  --ID of the Quote Extra object type

	--Create the record
	INSERT INTO QuoteExtras (QuoteID, QuoteVersionID, StatusID, ItemExtraID, LineNum, RefLineNum, Qty, Price, Cost, PrintOnQuote, Note, CreatedBy)
	VALUES (@QuoteID,
			@QuoteVersionID,
			@StatusID,
			@ItemExtraID,
			@LineNum,
			@RefLineNum,
			@Qty,
			@Price,
			@Cost,
			@PrintOnQuote,
			@Note,
			@UserID) --CreatedBy
			
	SET @QuoteExtraID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -16
	GOTO ReturnSelect

UpdateLine:	
	--Get the QuoteID and VersionID of the line to be updated
	SELECT  @QuoteVersionID = QuoteVersionID, 
			@QuoteID = QuoteID 
	FROM QuoteExtras
	WHERE QuoteExtraID = @QuoteExtraID
	
	--If the line is not on the most recent version, return an error
	DECLARE @LatestVersionUpdate INT = (SELECT ISNULL(MAX(VersionID), -5) FROM Quotes WHERE QuoteID = @QuoteID)
	IF @QuoteVersionID <> @LatestVersionUpdate
		RETURN -17

	--Update the record
	UPDATE QuoteExtras
	SET	
		StatusID = @StatusID,
		ItemExtraID = @ItemExtraID,
		RefLineNum = @RefLineNum,
		Qty = @Qty,
		Price = @Price,
		Cost = @Cost,
		PrintOnQuote = @PrintOnQuote,
		Note = @Note,		
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE QuoteExtraID = @QuoteExtraID

	IF (@@ROWCOUNT=0)
		RETURN -18
	GOTO ReturnSelect

ReturnSelect:
	SELECT @QuoteExtraID 'QuoteExtraID'
END
