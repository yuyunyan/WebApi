/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.05.22
   Description:	Copies a Source (Header, Lines, Extras and Ownership) into a new Purchase Order
   Usage:	
   Return Codes:
			-20 AccountID is required
			-22 No default Status is configured for Purchase Orders
			-23 No default Status is configured for Purchase Order Lines
			-24	No default payment term ID
			-25	No currency ID for account

			Procedure assumes that all Lines being chosen to copy have ItemIDs, as the ItemID is required in the SalesOrderLines table
   Revision History:
		2018.05.22  AR  Initial Deployment
		2018.06.07	BZ	Added dummy organizationId and shipToMethodId
		2018.06.11	BZ	Added PaymentTermID as input param
		2018.07.13	NA	Changed ToLocationID to ToWarehouseID

   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspSourceToPO]
	@AccountID INT = NULL,
	@ContactID INT = NULL,
	@ShipFromLocationID INT = NULL,
	@ShipToWarehouseID INT = NULL,
	@LinesToCopyJSON VARCHAR(MAX) = NULL,
	@UserID INT = NULL,
	@PaymentTermID INT = NULL,
	@IncotermID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	--Store the needed ObjectTypeIDs
	DECLARE @POObjectTypeID INT = 16
	DECLARE @POLineObjectTypeID INT = 23

	DECLARE @SourceObjectTypeID INT = 106

	DECLARE @NewPurchaseOrderID INT = NULL
	DECLARE @LinesCopied INT = NULL

	IF ISNULL(@AccountID, 0) = 0
		RETURN -20

	--Get the default status IDs for Sales Orders, SO Lines and Extras
	DECLARE @OrderStatusID INT = (SELECT TOP 1 StatusID FROM lkpStatuses WHERE ObjectTypeID = @POObjectTypeID AND IsDefault = 1 AND IsDeleted = 0)
	IF ISNULL(@OrderStatusID, 0) = 0
		RETURN -22

	DECLARE @LineStatusID INT = (SELECT TOP 1 StatusID FROM lkpStatuses WHERE ObjectTypeID = @POLineObjectTypeID AND IsDefault = 1 AND IsDeleted = 0)
	IF ISNULL(@LineStatusID, 0) = 0
		RETURN -23

	--Get the default payment term ID for account
	--DECLARE @PaymentTermID INT = (SELECT TOP 1 PaymentTermID FROM mapAccountTypes where AccountID = @AccountID)
	IF ISNULL(@PaymentTermID, 0) = 0
		RETURN -24

	IF ISNULL(@IncotermID, 0) = 0
	RETURN -24

	--Get the default currency term ID for account
	DECLARE @CurrencyID CHAR(3) = (SELECT TOP 1 CurrencyID FROM Accounts where AccountID = @AccountID)
	IF @CurrencyID IS NULL
		RETURN -25

	DECLARE @OrganizationID CHAR(3) = (SELECT TOP 1 OrganizationID FROM Accounts where AccountID = @AccountID)
	IF @OrganizationID IS NULL
		RETURN -25

	--Insert the new Purchase Order, copying attributes from the Source
	INSERT INTO PurchaseOrders (VersionID, AccountID, ContactID, StatusID, PaymentTermID, CurrencyID, OrderDate, CreatedBy, FromLocationID,
	 ToWarehouseID, OrganizationID, IncotermID, ShippingMethodID)
	VALUES ( 1,
			@AccountID,
			@ContactID,
			@OrderStatusID,
			@PaymentTermID,
			@CurrencyID,
			GETUTCDATE(),
			@UserID,
			@ShipFromLocationID,
			@ShipToWarehouseID,
			@OrganizationID,
			@IncotermID,
			1)

	SET @NewPurchaseOrderID = SCOPE_IDENTITY()	

	--Create the PO lines from the Sources
	INSERT INTO PurchaseOrderLines (PurchaseOrderID, POVersionID, StatusID, ItemID, LineNum, LineRev, Qty, 
	Cost, DateCode, PackagingID, CreatedBy, IsDeleted)
	SELECT	@NewPurchaseOrderID,
			1,
			@LineStatusID,
			j.itemId,
			0,
			0,
			j.quantity,
			s.Cost,
			s.DateCode,
			s.PackagingID,
			@UserID,
			0
	FROM Sources s
	  INNER JOIN OPENJSON(@LinesToCopyJSON) WITH (sourceId INT, quantity INT, itemId INT) AS j ON s.SourceID = j.sourceId

	SET @LinesCopied = @@ROWCOUNT
	
	SELECT @NewPurchaseOrderID 'PurchaseOrderID', 1 'VersionID', @LinesCopied 'LinesCopiedCount'
END
