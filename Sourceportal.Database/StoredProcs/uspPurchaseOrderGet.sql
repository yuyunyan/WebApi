/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.02
   Description:	Retrieves the header information for a given sales order & version 
   Usage:		EXEC uspPurchaseOrderGet 100009, 2, 1

   Return Codes:
				-7 Invalid PurchaseOrderID or VersionID to get PO header
   Revision History:
   2017.08.03	AR	Added PurchaseOrderLines join, cost column
   2017.08.04	AR	Join on Subquery to return sum as column
   2017.08.10	AR	Added BillTo (locations tbl), and ShipTo data
   2017.08.11	AR	Renamed ShipTo alias' to ShipFrom
   2017.08.14	AR	Converted orderDate to VARCHAR (was being casted as DateTime mm/dd/yyyy HH:MM in API)
   2018.01.24   ML  Added ExternalID
   2018.02.06   RV  Added PO Notes field
   2018.07.13	NA	Changed return value ShipToLocationID to ToWarehouseID
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspPurchaseOrderGet]
	@PurchaseOrderID INT = NULL,
	@VersionID INT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @SOPrice MONEY
	DECLARE @SOCost MONEY
	DECLARE @SOProfit MONEY
	DECLARE @SOGPM FLOAT

	--SELECT	@SOPrice = ROUND(SUM(Qty * Price), 2),
	--		@SOCost = ROUND(SUM(Qty * Cost), 2),
	--		@SOProfit = ROUND(SUM(Qty * Price - Qty * Cost), 2),
	--		@SOGPM = CASE WHEN SUM(Qty * Price) <> 0 THEN ROUND((SUM(Qty * Price) - SUM(Qty * Cost)) / SUM(Qty * Price), 4) ELSE 0 END
	--FROM PurchaseOrders
	--WHERE PurchaseOrderID = @PurchaseOrderID AND POVersionID = @VersionID AND IsDeleted = 0
	DECLARE @Sec TABLE (PurchaseOrderID INT, RoleID INT)
	INSERT @Sec EXECUTE uspPurchaseOrderSecurityGet @UserID = @UserID;

	SELECT	  po.PurchaseOrderID
			, po.VersionID
			, po.StatusID
			, s.StatusName
			, po.AccountID
			, a.AccountName
			, po.ContactID			
			, c.FirstName + ' ' + c.LastName 'ContactName'
			, c.OfficePhone
			, c.Email
			--, po.ShipLocationID
			--, l.[Name] 'ShipLocationName'
			, po.IncotermID
			, inc.IncotermName
			, po.CurrencyID
			, po.ShippingMethodID
			, po.PaymentTermID
			, po.ContactID
			, po.ToWarehouseID
			, c.LocationID
			, bl.LocationID BillToLocationID
			, bl.Name BillToLocationName
			, bl.HouseNumber BillToHouseNumber
			, bl.City BillToCity
			, bl.Street BillToStreet
			, bl.StateID BillToStateID
			, bs.StateCode BillToStateCode
			, bl.PostalCode BillToPostalCode
			, sf.LocationID ShipFromLocationID
			, sf.Name ShipFromLocationName
			, sf.HouseNumber ShipFromHouseNumber
			, sf.City ShipFromCity
			, sf.Street ShipFromStreet
			, sf.StateID ShipFromStateID
			, ss.StateCode ShipFromStateCode
			, sf.PostalCode ShipFromPostalCode
			--, po.UltDestinationID
			--, co.CountryName 'UltDestinationName'
			, po.OrganizationID
			, o.[Name] 'OrganizationName'
			--, po.FreightPaymentID
			--, po.FreightAccount
			, CONVERT(VARCHAR(16), po.OrderDate) OrderDate
			, pol.Cost
			--, po.CustomerPO
			--, ISNULL(@SOPrice, 0) 'SOPrice'
			--, ISNULL(@SOCost, 0) 'SOCost'
			--, ISNULL(@SOProfit, 0) 'SOProfit'
			--, ISNULL(@SOGPM, 0) 'SOGPM'
			, po.IsDeleted
			, po.ExternalID 
			, po.PONotes
			, a.CreatedBy
	FROM PurchaseOrders po
	  LEFT OUTER JOIN (SELECT SUM(Qty * Cost) Cost, PurchaseOrderID
						  FROM PurchaseOrderLines
						  WHERE purchaseOrderID = @PurchaseOrderID
						  AND POVersionID = @VersionID
						  and IsDeleted = 0
						  GROUP BY PurchaseOrderID) pol ON pol.PurchaseOrderID = pol.PurchaseOrderID
	  LEFT OUTER JOIN lkpStatuses s ON po.StatusID = s.StatusID
	  LEFT OUTER JOIN Accounts a ON po.AccountID = a.AccountID
	  LEFT OUTER JOIN Contacts c ON po.ContactID = c.ContactID
	  LEFT OUTER JOIN Locations bl ON bl.AccountID = a.AccountID AND bl.LocationTypeID = 1
	  LEFT OUTER JOIN Locations sf ON sf.LocationID = po.FromLocationID-- AND sf.LocationTypeID = 2
	  LEFT OUTER JOIN States bs on bs.StateID = bl.StateID
	  LEFT OUTER JOIN States ss on ss.StateID = sf.StateID
	  --LEFT OUTER JOIN Countries co ON po.UltDestinationID = co.CountryID
	  LEFT OUTER JOIN Organizations o ON po.OrganizationID = o.OrganizationID
	  LEFT OUTER JOIN codes.lkpIncoterms inc ON po.IncotermID = inc.IncotermID
	  INNER JOIN (SELECT DISTINCT PurchaseOrderID FROM @Sec) sec ON po.PurchaseOrderID = sec.PurchaseOrderID
	WHERE po.PurchaseOrderID = @PurchaseOrderID 
	AND po.VersionID = @VersionID

	  IF (@@rowcount = 0)
		 RETURN -7
END