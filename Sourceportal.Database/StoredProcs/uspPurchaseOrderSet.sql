/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.08.03
   Description:	Inserts or updates a Purchase Order
   Usage:	EXEC uspPurchaseOrderSet @AccountID = 4, @ContactID = 3, @StatusID = 4, @UserID = 0
			
   Return Codes:
			-1	Invalid PurchaseOrderID for new PO version
			-2	Error creating new version
			-3	Error creating new PO
			-4	Error updating PO
			-5	No default status for new PO configured.  Update the lkpStatuses table with a default status for POs
			-6	UserID is required
   Revision History:
   2017.08.11	AR	Added ISNULL/NULLIF to update statement to allow blank entries (currently necessary for walkign skeleton that does not include all fields
   2018.01.31   ML  Added ExternalId and remove checks and added ISNULL functions for AccountID, ContactID, StatusID
   2018.02.06	RV	Added PO Notes field
   2018.03.26	AR	Removed PromiseDate
   2018.04.05	AR	Added NULLIF -1 to update statement to allow null values
   2018.07.11   CT  Added ExternalID to insert
   2018.07.13	NA	Replaced ToLocationID with ToWarehouseID
   
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspPurchaseOrderSet]
	@PurchaseOrderID INT = NULL OUTPUT,
	@VersionID INT = NULL OUTPUT,
	@AccountID INT = NULL,
	@ContactID INT = NULL,	
	@StatusID INT = NULL,
	@FromLocationID INT = NULL,
	@ToWarehouseID INT = NULL,
	@IncotermID INT = NULL,
	@PaymentTermID INT = NULL,
	@CurrencyID CHAR(3) = NULL,	
	@ShippingMethodID INT = NULL,
	@OrganizationID INT = NULL,
	@OrderDate DATETIME = NULL,	
	@IsDeleted BIT = NULL,
	@UserID INT = NULL,
	@ExternalID VARCHAR(50) = NULL,
	@PONotes NVARCHAR(100) = NULL 
AS
BEGIN
	SET NOCOUNT ON;
	--Store the PurchaseOrder ObjectTypeID
	DECLARE @ObjectTypeID INT = 22
	DECLARE @LineObjectTypeID INT = 23

	--Get a default status ID for new purchase orders or versions
	
	IF ISNULL(@UserID, 0) = 0
		RETURN -6

	IF ISNULL(@PurchaseOrderID, 0) = 0
		GOTO InsertNewPurchaseOrder		
	ELSE
		BEGIN
			IF ISNULL(@VersionID, 0) = 0
				GOTO InsertNewVersion
			ELSE
				GOTO UpdatePurchaseOrder
		END

InsertNewVersion:
	
	DECLARE @NewVersionCount INT
	SET @VersionID = (SELECT COALESCE(MAX(VersionID), 0) + 1 FROM PurchaseOrders WHERE PurchaseOrderID = @PurchaseOrderID)
		
	IF ISNULL(@VersionID, 1) = 1
		RETURN -1

	--Create the new version of the Purchase Order
	SET IDENTITY_INSERT PurchaseOrders ON	
	INSERT INTO PurchaseOrders (PurchaseOrderID, VersionID, AccountID, ExternalID, ContactID, StatusID, FromLocationID, ToWarehouseID, IncotermID, PaymentTermID, CurrencyID, ShippingMethodID, OrganizationID, OrderDate, CreatedBy , PONotes)
	VALUES (@PurchaseOrderID, @VersionID, @AccountID, @ExternalID, @ContactID, @StatusID, @FromLocationID, @ToWarehouseID, @IncotermID, @PaymentTermID, @CurrencyID, @ShippingMethodID, @OrganizationID, @OrderDate, @UserID , @PONotes)
	SET @NewVersionCount = @@ROWCOUNT
	SET IDENTITY_INSERT PurchaseOrders OFF

	IF (@NewVersionCount=0)
		RETURN -2

	--Copy the PO Lines from the previous version to the new one	
	DECLARE @InsertKeys TABLE (New INT, Old INT)
	
	MERGE PurchaseOrderLines
	USING (SELECT * FROM PurchaseOrderLines WHERE PurchaseOrderID = @PurchaseOrderID AND POVersionID = @VersionID - 1 AND IsDeleted = 0) s
	ON 0=1
	WHEN NOT MATCHED THEN
		INSERT (PurchaseOrderID, POVersionID, StatusID, ItemID, LineNum, VendorLine, Qty, Cost, DateCode, PackagingID, DueDate, CreatedBy)
		VALUES (@PurchaseOrderID, @VersionID, s.StatusID, s.ItemID, s.LineNum, s.VendorLine, s.Qty, s.Cost, s.DateCode, s.PackagingID, s.DueDate, @UserID)
		OUTPUT INSERTED.POLineID, s.POLineID INTO @InsertKeys;
	
	--Copy the Source Joins that were linked to the old PO Lines to the new PO Lines
	INSERT INTO mapSourcesJoin (ObjectTypeID, ObjectID, SourceID, IsMatch, CreatedBy)
	SELECT @LineObjectTypeID, ik.New, sj.SourceID, sj.IsMatch, @UserID
	FROM mapSourcesJoin sj
	  INNER JOIN @InsertKeys ik ON sj.ObjectID = ik.Old AND sj.ObjectTypeID = @LineObjectTypeID
	  INNER JOIN Sources s ON sj.SourceID = s.SourceID
	WHERE sj.IsDeleted = 0 AND s.IsDeleted = 0

	--Copy the PO Extras from the previous version to the new one
	INSERT INTO PurchaseOrderExtras (PurchaseOrderID, POVersionID, StatusID, ItemExtraID, LineNum, RefLineNum, Qty, Cost, Note, PrintOnPO, CreatedBy)
	SELECT							@PurchaseOrderID, @VersionID,  StatusID, ItemExtraID, LineNum, RefLineNum, Qty, Cost, Note, PrintOnPO, @UserID
	FROM PurchaseOrderExtras
	WHERE PurchaseOrderID = @PurchaseOrderID AND POVersionID = @VersionID - 1 AND IsDeleted = 0
	
	GOTO ReturnSelect

InsertNewPurchaseOrder:
	
	SET @VersionID = 1

	INSERT INTO PurchaseOrders (VersionID, AccountID, ContactID, StatusID, FromLocationID, ToWarehouseID, IncotermID, PaymentTermID, CurrencyID, ShippingMethodID, OrganizationID, OrderDate, CreatedBy, PONotes)
	VALUES (@VersionID, @AccountID, @ContactID, @StatusID, @FromLocationID, @ToWarehouseID, @IncotermID, @PaymentTermID, @CurrencyID, @ShippingMethodID, @OrganizationID, @OrderDate, @UserID , @PONotes)
	
	SET @PurchaseOrderID = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -3
	
	--Set the creator to owner of the PO
	INSERT INTO mapOwnership (OwnerID, ObjectTypeID, ObjectID, IsGroup, [Percent], CreatedBy)
	VALUES (@UserID, @ObjectTypeID, @PurchaseOrderID, 0, 100, @UserID)
	
	GOTO ReturnSelect

UpdatePurchaseOrder:
	UPDATE PurchaseOrders
		SET AccountID = ISNULL(NULLIF(@AccountID,0), AccountID),
			ContactID = ISNULL(NULLIF(@ContactID,0), ContactID),
			StatusID = ISNULL(NULLIF(@StatusID,0), StatusID),
			FromLocationID = ISNULL(NULLIF(@FromLocationID,0), FromLocationID),
			ToWarehouseID = NULLIF(ISNULL(NULLIF(@ToWarehouseID,0), ToWarehouseID),-1),
			IncotermID =  NULLIF(ISNULL(NULLIF(@IncotermID,0), IncotermID),-1),
			PaymentTermID =  ISNULL(NULLIF(@PaymentTermID,0), PaymentTermID),
			CurrencyID = ISNULL(@CurrencyID, CurrencyID),		
			ShippingMethodID = NULLIF(ISNULL(NULLIF(@ShippingMethodID,0), ShippingMethodID),-1),
			OrganizationID =  NULLIF(ISNULL(NULLIF(@OrganizationID,0), OrganizationID),-1),
			OrderDate = ISNULL(NULLIF(@OrderDate,''), OrderDate),			
			IsDeleted = ISNULL(@IsDeleted, IsDeleted),
			Modified = GETUTCDATE(),
			ModifiedBy = @UserID,
			ExternalID = ISNULL(@ExternalID, ExternalID),
			PONotes = ISNULL(@PONotes, PONotes)
		WHERE PurchaseOrderID = @PurchaseOrderID AND VersionID = @VersionID

	IF (@@ROWCOUNT=0)
		RETURN -4
	GOTO ReturnSelect

ReturnSelect:
	SELECT @PurchaseOrderID 'PurchaseOrderID', @VersionID 'VersionID'
END
