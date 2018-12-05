/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.24
   Description:	Updates an existing Sales Order
   Usage:	EXEC uspSalesOrderSet
			
   Return Codes:
			-1  SalesOrderID is required
			-7  UserID is required
			-8  Error updating sales order
			-9  Only the latest version of a SalesOrder can be modified, or SalesOrderID does not exist/is deleted
			-10  Error creating new version
			-11  Invalid version number when attempting to create new version
   Revision History:
   2017.07.28	AR	Added ISNULL to OrderDate update
   2017.08.08	BZ	Added Copy to SOPO and SOInv allocation
   2017.08.09	NA	Added soft delete check to SOPO and SOInv Allocation copy
   2018.01.12   ML  Added External ID. Also if the parameters are not passed in then just update with the current value
   2018.02.06   RV  Added Shipping Notes and QC Notes fields
   2018.02.06   CT  Added @IncotermLocation
   2018.03.14	BZ	Added DeliveryRuleID
   2018.06.04	NA	Removed ShippingMethodID, Added Carrier Methods
   2018.06.13	NA	Added creation of new sales order if SalesOrderID is not supplied.  USE ONLY WITH SAP! otherwise create sales orders with uspQuoteToSO
   2018.06.13   CT  Added ExternalID to Insert from SAP
   2018.06.26	NA	Changed to ItemStock schema
   2018.07.12   JT  Changed NULLIF TO CarrierID AND CarrierMethodID for Null value 
   2018.07.30	NA	Added ShipFromRegionID and various fixes
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspSalesOrderSet]
	@SalesOrderID INT = NULL OUTPUT,
	@VersionID INT = NULL OUTPUT,
	@AccountID INT = NULL,
	@ContactID INT = NULL,
	@ProjectID INT = NULL,	
	@StatusID INT = NULL,
	@IncotermID INT = NULL,
	@PaymentTermID INT = NULL,
	@CurrencyID CHAR(3) = NULL,
	@ShipLocationID INT = NULL,	
	@OrganizationID INT = NULL,
	@DeliveryRuleID INT = NULL,
	@UltDestinationID INT = NULL,
	@FreightPaymentID INT = NULL,
	@FreightAccount NVARCHAR(50) = NULL,
	@OrderDate DATE = NULL,
	@CustomerPO NVARCHAR(50) = NULL,
	@IsDeleted BIT = NULL,
	@UserID INT = NULL,
	@ExternalID VARCHAR(50) = NULL,
	@ShippingNotes VARCHAR(100) = NULL ,
	@QCNotes VARCHAR(100) = NULL,
	@IncotermLocation VARCHAR(100) = NULL,
	@CarrierID INT = NULL,
	@CarrierMethodID INT = NULL,
	@ShipFromRegionID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@SalesOrderID, 0) = 0
		GOTO InsertNewSalesOrder

	IF @UserID IS NULL
		RETURN -7

	IF ISNULL(@VersionID, 0) = 0
		GOTO InsertNewVersion
	ELSE	
		IF (SELECT VersionID FROM vwSalesOrders WHERE SalesOrderID = @SalesOrderID) <> @VersionID
			RETURN -9
		ELSE
			GOTO UpdateSalesOrder

InsertNewSalesOrder:
	SET @VersionID = 1
	INSERT INTO SalesOrders (VersionID, AccountID, ExternalID, ContactID, ProjectID, QuoteID, StatusID, IncotermID, PaymentTermID, CurrencyID, ShipLocationID, OrganizationID, UltDestinationID, FreightPaymentID, FreightAccount, OrderDate, CustomerPO, CreatedBy, ShippingNotes, QCNotes, IncotermLocation, CarrierID, CarrierMethodID, ShipFromRegionID)
	VALUES (@VersionID, @AccountID, @ExternalID, @ContactID, @ProjectID, NULL, @StatusID, @IncotermID, @PaymentTermID, @CurrencyID, @ShipLocationID, @OrganizationID, @UltDestinationID, @FreightPaymentID, @FreightAccount, @OrderDate, @CustomerPO, @UserID, @ShippingNotes, @QCNotes, @IncotermLocation, @CarrierID, @CarrierMethodID, @ShipFromRegionID)
	SET @SalesorderID = SCOPE_IDENTITY()
	
	GOTO ReturnSelect

InsertNewVersion:

	DECLARE @NewVersionCount INT = NULL
	DECLARE @QuoteID INT = NULL
	SELECT @VersionID = MAX(VersionID) + 1,
		   @QuoteID = MAX(QuoteID)
	FROM SalesOrders
	WHERE SalesOrderID = @SalesOrderID
	
	IF ISNULL(@VersionID, 1) = 1
		RETURN -11
	--Create the new version of the Sales Order
	SET IDENTITY_INSERT SalesOrders ON	
	INSERT INTO SalesOrders (SalesOrderID, VersionID, AccountID, ContactID, ProjectID, QuoteID, StatusID, IncotermID, PaymentTermID, CurrencyID, ShipLocationID, OrganizationID, UltDestinationID, FreightPaymentID, FreightAccount, OrderDate, CustomerPO, CreatedBy , ShippingNotes, QCNotes, IncotermLocation, CarrierID, CarrierMethodID, ShipFromRegionID)
	VALUES (@SalesOrderID, @VersionID, @AccountID, @ContactID, @ProjectID, @QuoteID, @StatusID, @IncotermID, @PaymentTermID, @CurrencyID, @ShipLocationID, @OrganizationID, @UltDestinationID, @FreightPaymentID, @FreightAccount, @OrderDate, @CustomerPO, @UserID , @ShippingNotes , @QCNotes, @IncotermLocation, @CarrierID, @CarrierMethodID, @ShipFromRegionID)
	SET @NewVersionCount = @@ROWCOUNT	
	SET IDENTITY_INSERT SalesOrders OFF
	

	IF (@NewVersionCount=0)
		RETURN -10

	--Copy the Sales Order Lines from the previous version to the new one	
	DECLARE @InsertKeys TABLE (New INT, Old INT)
	
	MERGE SalesOrderLines
	USING (SELECT * FROM SalesOrderLines WHERE SalesOrderID = @SalesOrderID AND SOVersionID = @VersionID - 1 AND IsDeleted = 0) s
	ON 0=1
	WHEN NOT MATCHED THEN
		INSERT (SalesOrderID, SOVersionID, QuoteLineID, StatusID, ItemID, LineNum, CustomerLine, CustomerPartNum, PartNumberStrip, Qty, Price, Cost, DateCode, PackagingID, ShipDate, DueDate, CreatedBy)
		VALUES (@SalesOrderID, @VersionID, s.QuoteLineID, s.StatusID, s.ItemID, s.LineNum, s.CustomerLine, s.CustomerPartNum, s.PartNumberStrip, s.Qty, s.Price, s.Cost, s.DateCode, s.PackagingID, s.ShipDate, s.DueDate, @UserID)
		OUTPUT INSERTED.SOLineID, s.SOLineID INTO @InsertKeys;

	--Copy PO and other allocations to new sales order lines
	INSERT INTO mapSOPOAllocation (SOLineID, POLineID, Qty, ExternalID, CreatedBy)
	SELECT ins.New, pa.POLineId, pa.Qty, pa.ExternalID, @UserID
	FROM mapSOPOAllocation pa
	INNER JOIN @InsertKeys ins ON ins.Old = pa.SOLineID
	WHERE pa.IsDeleted = 0

	INSERT INTO mapSOInvFulfillment (SOLineID, StockID, Qty, ExternalID, CreatedBy)
	SELECT ins.New, sif.StockID, sif.Qty, sif.ExternalID, @UserID
	FROM mapSOInvFulfillment sif
	INNER JOIN @InsertKeys ins ON ins.Old = sif.SOLineID
	WHERE sif.IsDeleted = 0

	--Copy the Sales Order Extras from the previous version to the new one
	INSERT INTO SalesOrderExtras (SalesOrderID, SOVersionID, QuoteExtraID, StatusID, ItemExtraID, LineNum, RefLineNum, Qty, Price, Cost, PrintOnSO, Note, CreatedBy)
	SELECT						  @SalesOrderID, @VersionID, QuoteExtraID, StatusID, ItemExtraID, LineNum, RefLineNum, Qty, Price, Cost, PrintOnSO, Note, @UserID
	FROM SalesOrderExtras
	WHERE SalesOrderID = @SalesOrderID AND SOVersionID = @VersionID - 1 AND IsDeleted = 0


	GOTO ReturnSelect
UpdateSalesOrder:
	DECLARE @OldDeliveryID INT = (SELECT TOP 1 DeliveryRuleID FROM SalesOrders WHERE SalesOrderID = @SalesOrderID AND VersionID = @VersionID)

	UPDATE SalesOrders
	SET
		AccountID = ISNULL(NULLIF(@AccountID, 0), AccountID),
		ContactID = ISNULL(NULLIF( @ContactID, 0), ContactID),
		ProjectID = ISNULL(NULLIF( @ProjectID, 0), ProjectID),
		StatusID = ISNULL(NULLIF( @StatusID, 0), StatusID),
		IncotermID = ISNULL(NULLIF( @IncotermID, 0), IncotermID),
		PaymentTermID = ISNULL(NULLIF( @PaymentTermID, 0), PaymentTermID),
		CurrencyID =  ISNULL(@CurrencyID, CurrencyID),
		ShipLocationID = ISNULL(NULLIF( @ShipLocationID, 0), ShipLocationID),
		DeliveryRuleID = ISNULL(NULLIF(@DeliveryRuleID, 0), DeliveryRuleID),
		OrganizationID = ISNULL(NULLIF( @OrganizationID, 0), OrganizationID),
		UltDestinationID = ISNULL(NULLIF( @UltDestinationID, 0), UltDestinationID),
		FreightPaymentID =ISNULL(NULLIF( @FreightPaymentID, 0), FreightPaymentID),
		FreightAccount = ISNULL(@FreightAccount, FreightAccount),
		OrderDate = ISNULL(@OrderDate,OrderDate),
		CustomerPO = ISNULL(@CustomerPO, CustomerPO),
		IsDeleted = ISNULL(@IsDeleted, 0),
		ModifiedBy = NULLIF(@UserID, 0),
		Modified = GETUTCDATE(),
		ExternalID = ISNULL(NULLIF( @ExternalID, 0), ExternalID) ,
		ShippingNotes = ISNULL(@ShippingNotes, ShippingNotes),
		QCNotes = ISNULL( @QCNotes, QCNotes),
		IncotermLocation = ISNULL(@IncotermLocation, IncotermLocation),
		CarrierID = NULLIF(@CarrierID,0),
		CarrierMethodID = NULLIF(@CarrierMethodID,0),
		ShipFromRegionID = ISNULL(@ShipFromRegionID, ShipFromRegionID)
	WHERE SalesOrderID = @SalesOrderID AND VersionID = @VersionID

	IF @OldDeliveryID != @DeliveryRuleID
		GOTO UPDATE_QUOTELINES
	ELSE
		GOTO ReturnSelect

UPDATE_QUOTELINES:
	UPDATE SalesOrderLines
	SET DeliveryRuleID = @DeliveryRuleID
	WHERE SalesOrderID = @SalesOrderID AND SOVersionID = @VersionID

	GOTO ReturnSelect

ReturnSelect:
	SELECT @SalesOrderID 'SalesOrderID', @VersionID 'VersionID'
END
