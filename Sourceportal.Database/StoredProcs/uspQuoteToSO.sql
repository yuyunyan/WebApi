/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.18
   Description:	Copies a Quote (Header, Lines, Extras and Ownership) into a new Sales Order
   Usage:	EXEC uspQuoteToSO	@QuoteID = 100002, 
								@CustomerPO = 'Test', 
								@UserID = 0,
								@LinesToCopyJSON = '[{"QuoteLineID":667},{"QuoteLineID":668},{"QuoteLineID":670},{"QuoteLineID":671},{"QuoteLineID":1}]',
								@ExtrasToCopyJSON = '[{"QuoteExtraID":6},{"QuoteExtraID":5},{"QuoteExtraID":4},{"QuoteExtraID":9}]'			
   Return Codes:
			-20 QuoteID is required
			-21 Cannot find valid verison of QuoteID provided
			-22 No default Status is configured for Sales Orders
			-23 No default Status is configured for Sales Order Lines
			-24 No default Status is configured for Sales Order Extras

			Procedure assumes that all Lines being chosen to copy have ItemIDs, as the ItemID is required in the SalesOrderLines table
   Revision History:
		2018.02.06  CT  Added IncotermLocation
		2018.03.26	NA	Added QCNotes and Shipping Instructions, added IsMatch=1 check to source join copy
		2018.04.16	NA	Added mapBuyerSORoutes insert statement
   ============================================= */


CREATE PROCEDURE [dbo].[uspQuoteToSO]
	@QuoteID INT = NULL,
	@CustomerPO VARCHAR(50) = NULL,
	@LinesToCopyJSON VARCHAR(MAX) = NULL,
	@ExtrasToCopyJSON VARCHAR(MAX) = NULL,
	@UserID INT = NULL
AS
BEGIN
	
	SET NOCOUNT ON;
	--Store the needed ObjectTypeIDs
	DECLARE @SOObjectTypeID INT = 16
	DECLARE @SOLineObjectTypeID INT = 17
	DECLARE @SOExtraObjectTypeID INT = 18
	DECLARE @QuoteObjectTypeID INT = 19
	DECLARE @QuoteLineObjectTypeID INT = 20

	DECLARE @SalesOrderID INT = NULL
	DECLARE @LinesCopied INT = NULL
	DECLARE @ExtrasCopied INT = NULL

	IF ISNULL(@QuoteID, 0) = 0
		RETURN -20

	--Get the latest version of the QuoteID provided
	DECLARE @QuoteVersionID INT = (SELECT MAX(VersionID) FROM Quotes WHERE QuoteID = @QuoteID AND IsDeleted = 0)
	IF ISNULL(@QuoteVersionID, 0) = 0
		RETURN -21

	--Get the default status IDs for Sales Orders, SO Lines and Extras
	DECLARE @OrderStatusID INT = (SELECT TOP 1 StatusID FROM lkpStatuses WHERE ObjectTypeID = @SOObjectTypeID AND IsDefault = 1 AND IsDeleted = 0)
	IF ISNULL(@OrderStatusID, 0) = 0
		RETURN -22

	DECLARE @LineStatusID INT = (SELECT TOP 1 StatusID FROM lkpStatuses WHERE ObjectTypeID = @SOLineObjectTypeID AND IsDefault = 1 AND IsDeleted = 0)
	IF ISNULL(@LineStatusID, 0) = 0
		RETURN -23

	DECLARE @ExtraStatusID INT = (SELECT TOP 1 StatusID FROM lkpStatuses WHERE ObjectTypeID = @SOExtraObjectTypeID AND IsDefault = 1 AND IsDeleted = 0)
	IF ISNULL(@LineStatusID, 0) = 0
		RETURN -24

	--Insert the new Sales Order, copying attributes from the Quote
	INSERT INTO SalesOrders (VersionID, AccountID, ContactID, ProjectID, QuoteID, StatusID, IncotermID, PaymentTermID, CurrencyID, ShipLocationID, ShippingMethodID, OrganizationID, CustomerPO, IncotermLocation, OrderDate, CreatedBy, QCNotes, ShippingNotes)
	SELECT 	1,
			q.AccountID,
			q.ContactID,
			q.ProjectID,
			q.QuoteID,
			@OrderStatusID,
			q.IncotermID,
			q.PaymentTermID,
			q.CurrencyID,
			q.ShipLocationID,
			q.ShippingMethodID,
			q.OrganizationID,
			@CustomerPO,
			q.IncotermLocation,
			GETUTCDATE(),
			@UserID,
			a.QCNotes,
			a.ShippingInstructions
	FROM Quotes q
		INNER JOIN Accounts a ON q.AccountID = a.AccountID
	WHERE q.QuoteID = @QuoteID AND q.VersionID = @QuoteVersionID

	SET @SalesOrderID = SCOPE_IDENTITY()	

	--Copy ownership from the selected Quote
	INSERT INTO mapOwnership (OwnerID, ObjectTypeID, ObjectID, IsGroup, [Percent], CreatedBy)
	SELECT OwnerID, @SOObjectTypeID, @SalesOrderID, IsGroup, [Percent], @UserID
	FROM mapOwnership
	WHERE ObjectID = @QuoteID AND ObjectTypeID = @QuoteObjectTypeID AND IsDeleted = 0

	--Copy the desired Quote Lines to SO Lines
	INSERT INTO SalesOrderLines (SalesOrderID, SOVersionID, QuoteLineID, StatusID, ItemID, LineNum, CustomerLine, CustomerPartNum, PartNumberStrip, Qty, Price, Cost, DateCode, PackagingID, ShipDate, DueDate, CreatedBy)
	SELECT	@SalesOrderID,
			1,
			ql.QuoteLineID,
			@LineStatusID,
			ql.ItemID,
			CASE WHEN ISNULL(ql.AltFor, 0) = 0 THEN ql.LineNum ELSE (SELECT ql2.LineNum FROM QuoteLines ql2 WHERE ql2.QuoteLineID = ql.AltFor) END, --LineNum			
			ql.CustomerLine,
			ql.CustomerPartNum,
			ql.PartNumberStrip,
			ql.Qty,
			ql.Price,
			ql.Cost,
			ql.DateCode,
			ql.PackagingID,
			ql.ShipDate,
			ql.dueDate,
			@UserID
	FROM QuoteLines ql
	  INNER JOIN OPENJSON(@LinesToCopyJSON) WITH (QuoteLineID INT) AS j ON ql.QuoteLineID = j.QuoteLineID
	WHERE ql.QuoteID = @QuoteID AND ql.QuoteVersionID = @QuoteVersionID

	SET @LinesCopied = @@ROWCOUNT
	
	--Get information on matched sources
	DECLARE @MatchedSources TABLE (SOLineID INT, SourceID INT, Qty INT, IsMatch BIT, SourceCreator INT)
		
	INSERT INTO @MatchedSources (SOLineID, SourceID, Qty, IsMatch, SourceCreator)
	SELECT	sol.SOLineID,
			sj.SourceID,
			sj.Qty,
			sj.IsMatch,
			s.CreatedBy
	FROM SalesOrderLines sol
	INNER JOIN mapSourcesJoin sj ON sol.QuoteLineID = sj.ObjectID 
							    AND sj.ObjectTypeID = @QuoteLineObjectTypeID 
								AND sj.IsMatch = 1
								AND sj.IsDeleted = 0
	INNER JOIN Sources s ON sj.SourceID = s.SourceID AND s.IsDeleted = 0
	WHERE sol.SalesOrderID = @SalesOrderID AND sol.SOVersionID = 1
	
	--Copy linked Sources for the desired Quote Lines to the new Sales Lines
	INSERT INTO mapSourcesJoin (ObjectTypeID, ObjectID, SourceID, Qty, IsMatch, CreatedBy)
	SELECT @SOLineObjectTypeID, SOLineID, SourceID, Qty, IsMatch, @UserID
	FROM @MatchedSources
	
	--Create routes to buyers based on the sources chosen
	INSERT INTO mapBuyerSORoutes (SOLineID, UserID, CreatedBy)
	SELECT DISTINCT SOLineID, SourceCreator, @UserID
	FROM @MatchedSources

	--Copy the desired Quote Extras to SO Extras
	INSERT INTO SalesOrderExtras (SalesOrderID, SOVersionID, QuoteExtraID, StatusID, ItemExtraID, LineNum, RefLineNum, Qty, Price, Cost, PrintOnSO, Note, CreatedBy)
	SELECT
		@SalesOrderID,
		1,
		qe.QuoteExtraID,
		@ExtraStatusID,
		qe.ItemExtraID,
		qe.LineNum,
		qe.RefLineNum,
		qe.Qty,
		qe.Price,
		qe.Cost,
		qe.PrintOnQuote,
		qe.Note,
		@UserID
	FROM QuoteExtras qe
		INNER JOIN OPENJSON(@ExtrasToCopyJSON) WITH (QuoteExtraID INT) AS j ON qe.QuoteExtraID = j.QuoteExtraID
	WHERE qe.QuoteID = @QuoteID AND qe.QuoteVersionID = @QuoteVersionID

	SET @ExtrasCopied = @@ROWCOUNT

	SELECT @SalesOrderID 'SalesOrderID', 1 'VersionID', @LinesCopied 'LinesCopiedCount', @ExtrasCopied 'ExtrasCopiedCount'	
END