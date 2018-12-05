/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.07.24
   Description:	Retrieves the header information for a given sales order & version
   Usage:		EXEC uspSalesOrderGet 100007, 1, 1
   Return Codes:
				-1 Missing SalesOrderID or VersionID
   Revision History:
			2018-01-08	ML Added ExternalID
			2018.02.06  RV Added Shipping Notes and QC Notes fields
			2018.02.06  CT Added IncotermLocation
			2018.03.14	BZ	Added DeliveryRuleID
			2018.06.04	JT Added CarrierName,MethodID,MethodName and CarrierMethodID
			2018.07.30	NA	Added ShipFromRegionID
			2018.10.29	NA	Added TransitDays for the Carrier Method
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspSalesOrderGet]
	@SalesOrderID INT = NULL,
	@VersionID INT = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF @SalesOrderID = NULL OR @VersionID = NULL
		RETURN -1

	DECLARE @Sec TABLE (SalesOrderID INT, RoleID INT)
	INSERT @Sec EXECUTE uspSalesOrderSecurityGet @UserID = @UserID;

	DECLARE @SOPrice MONEY
	DECLARE @SOCost MONEY
	DECLARE @SOProfit MONEY
	DECLARE @SOGPM FLOAT

	SELECT	@SOPrice = ROUND(SUM(Qty * Price), 2),
			@SOCost = ROUND(SUM(Qty * Cost), 2),
			@SOProfit = ROUND(SUM(Qty * Price - Qty * Cost), 2),
			@SOGPM = CASE WHEN SUM(Qty * Price) <> 0 THEN ROUND((SUM(Qty * Price) - SUM(Qty * Cost)) / SUM(Qty * Price), 4) ELSE 0 END
	FROM SalesOrderLines
	WHERE SalesOrderID = @SalesOrderID AND SOVersionID = @VersionID AND IsDeleted = 0

	SELECT	  so.SalesOrderID
			, so.VersionID
			, so.StatusID
			, s.StatusName
			, so.AccountID
			, a.AccountName
			, so.ContactID			
			, c.FirstName + ' ' + c.LastName 'ContactName'
			, c.OfficePhone
			, c.Email
			, so.ShipLocationID
			, so.ShippingMethodID
			, so.PaymentTermID
			, so.ProjectID
			, l.[Name] 'ShipLocationName'
			, so.IncotermID
			, inc.IncotermName
			, so.IncotermLocation
			, so.CurrencyID
			, so.DeliveryRuleID
			, so.UltDestinationID
			, co.CountryName 'UltDestinationName'
			, so.OrganizationID
			, o.[Name] 'OrganizationName'
			, so.FreightPaymentID
			, so.FreightAccount
			, so.OrderDate
			, so.CustomerPO
			, so.ProjectID
			, so.ShipFromRegionID
			, sfr.RegionName 'ShipFromRegion'
			, ISNULL(@SOPrice, 0) 'SOPrice'
			, ISNULL(@SOCost, 0) 'SOCost'
			, ISNULL(@SOProfit, 0) 'SOProfit'
			, ISNULL(@SOGPM, 0) 'SOGPM'
			, so.IsDeleted
			, so.ExternalID
			, so.ShippingNotes
			, so.QCNotes
			, so.CarrierID
			, cm.MethodID 'CarrierMethodID'
			, cm.MethodName
			, cm.TransitDays
			, A.AccountNum
			, @UserID UserID
	FROM SalesOrders so
	  LEFT OUTER JOIN lkpStatuses s ON so.StatusID = s.StatusID
	  LEFT OUTER JOIN Accounts a ON so.AccountID = a.AccountID
	  LEFT OUTER JOIN Contacts c ON so.ContactID = c.ContactID
	  LEFT OUTER JOIN Locations l ON so.ShipLocationID = l.LocationID
	  LEFT OUTER JOIN Countries co ON so.UltDestinationID = co.CountryID
	  LEFT OUTER JOIN Organizations o ON so.OrganizationID = o.OrganizationID
	  LEFT OUTER JOIN codes.lkpIncoterms inc ON so.IncotermID = inc.IncotermID
	  LEFT OUTER JOIN Carriers ca ON so.CarrierID = ca.CarrierID
	  LEFT OUTER JOIN CarrierMethods cm ON so.CarrierMethodID = cm.MethodID
	  LEFT OUTER JOIN lkpShipFromRegions sfr ON so.ShipFromRegionID = sfr.ShipFromRegionID
	  INNER JOIN (SELECT DISTINCT SalesOrderID FROM @Sec) sec ON so.SalesOrderID = sec.SalesOrderID 
	WHERE so.SalesOrderID = @SalesOrderID 
	  AND so.VersionID = @VersionID
END