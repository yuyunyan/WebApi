/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.03.28
   Description:	Gets a list of sales order lines that may need fulfillment
				BuyerID is optional, focuses the results to only those where the provided buyer created matched sources
				AccountID is optional, will filter the results to only sources with the provided AccountID
   Usage: EXEC uspOrderFulfillmentGet @BuyerID = 3
   Revision History:
		2018.04.09	NA	Converted to SO routing table and added Buyer's name
		2018.05.03	NA	Added support for @SearchString
		2018.05.08	BZ	Added FilterText, FilterBy
		2018.06.04	NA	Disabled BuyerID filter until it is fully implemented on the interface
		2018.10.09	NA	Fixed Sorting by OrderID
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspOrderFulfillmentGet]
	@BuyerID INT = NULL,
	@AccountID INT = NULL,
	@UnderallocatedOnly BIT = 0,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@CommentTypeID INT = NULL,
	@SearchString NVARCHAR(50) = '',
	@FilterBy NVARCHAR(25) = NULL,
	@FilterText NVARCHAR(25) = NULL
AS
BEGIN
	DECLARE @ObjectTypeID INT = 17;  --Sales Order ObjectTypeID

	WITH Main_Temp AS (
	SELECT	sol.SOLineID,
			sol.SalesOrderID,
			sol.SOVersionID 'VersionID',
			sol.LineNum,
			so.AccountID,
			cust.AccountName,
			sol.ItemID,
			i.PartNumber,
			i.MfrID,
			m.MfrName,
			i.CommodityID,
			c.CommodityName,
			sol.Qty,
			sola.POAllocated + sola.InvAllocated 'AllocatedQty',
			sol.Price,
			sol.PackagingID,
			p.PackagingName,
			sol.PackageConditionID,
			pc.ConditionName,
			sol.DateCode,
			sol.ShipDate,
			sol.StatusID,
			st.StatusName,
			dbo.fnGetObjectOwners(sol.SalesOrderID, 16) 'Owner',
			dbo.fnGetSoBuyerRoutes(sol.SOLineID) 'Buyers',
			s.SourceCount,
			so.ExternalID,
			dbo.fnGetCommentsCount(sol.SOLineID, @CommentTypeID) 'Comments'
	FROM vwSalesOrderLines sol	  	  
	  LEFT OUTER JOIN mapBuyerSORoutes r ON sol.SOLineID = r.SOLineID AND r.UserID = @BuyerID AND r.IsDeleted = 0
	  LEFT OUTER JOIN (	SELECT sj.ObjectID, COUNT(*) 'SourceCount'
						FROM Sources s
						INNER JOIN mapSourcesJoin sj 
							ON s.SourceID = sj.SourceID 
							AND sj.ObjectTypeID = @ObjectTypeID 
							AND sj.IsMatch = 1 
							AND sj.IsDeleted = 0
						WHERE AccountID = ISNULL(@AccountID, AccountID)
						GROUP BY sj.ObjectID) s ON sol.SOLineID = s.ObjectID
	  INNER JOIN vwSalesOrders so ON sol.SalesOrderID = so.SalesOrderID
	  INNER JOIN vwSalesOrderLineAllocations sola ON sol.SOLineID = sola.SOLineID
	  INNER JOIN lkpStatuses st ON sol.StatusID = st.StatusID  
	  INNER JOIN Accounts cust ON so.AccountID = cust.AccountID
	  INNER JOIN Items i ON sol.ItemID = i.ItemID
	  INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
	  INNER JOIN lkpItemCommodities c ON i.CommodityID = c.CommodityID
	  LEFT OUTER JOIN codes.lkpPackaging p ON sol.PackagingID = p.PackagingID
	  LEFT OUTER JOIN codes.lkpPackageConditions pc ON sol.PackageConditionID = pc.PackageConditionID 
	WHERE (st.IsCanceled = 0 AND st.IsComplete = 0)
	  AND (sola.POAllocated + sola.InvAllocated != sol.Qty OR @UnderallocatedOnly = 0)
	  --AND (@BuyerID IS NULL OR r.UserID = @BuyerID) --disabled buyerID filter.
	  AND (@AccountID IS NULL OR s.SourceCount IS NOT NULL)
	  AND (CAST(ISNULL(so.ExternalID, sol.SalesOrderID) AS VARCHAR) + ISNULL(cust.AccountName, '') + ISNULL(i.PartNumber, '') + ISNULL(p.PackagingName, '') + ISNULL(dbo.fnGetObjectOwners(sol.SalesOrderID, 16), '') LIKE '%' + ISNULL(@SearchString,'') + '%')
	  AND (@FilterBy IS NULL OR (
		(@FilterBy = 'orderNumber' AND ((sol.SalesOrderID LIKE '%' + ISNULL(@FilterText , '') + '%'))) OR
		(@FilterBy = 'partNumber' AND ((i.PartNumber LIKE '%' + ISNULL(@FilterText , '') + '%'))) OR
		(@FilterBy = 'customer' AND ((cust.AccountName LIKE '%' + ISNULL(@FilterText , '') + '%'))) OR
		(@FilterBy = 'mfr' AND ((m.MfrName LIKE '%' + ISNULL(@FilterText , '') + '%'))) OR
		(@FilterBy = 'commodity' AND ((c.CommodityName LIKE '%' + ISNULL(@FilterText , '') + '%'))) OR
		(@FilterBy = 'orderQty' AND ((sol.Qty LIKE ISNULL(@FilterText , '')))) OR
		(@FilterBy = 'allocatedQty' AND ((CAST((sola.POAllocated + sola.InvAllocated) AS NVARCHAR(25)) LIKE ISNULL(@FilterText , '')))) OR
		(@FilterBy = 'shipDate' AND ((FORMAT (sol.ShipDate, 'MM/dd/yyyy', 'en-US' ) LIKE ISNULL(@FilterText , '') + '%'))) OR
		(@FilterBy = 'salesPerson' AND ((dbo.fnGetObjectOwners(sol.SalesOrderID, 16) LIKE '%' + ISNULL(@FilterText , '') + '%')))
	  ))
	),
	Count_Temp AS (
		SELECT COUNT(*) AS TotalRowCount
		FROM Main_Temp)

	SELECT * FROM Main_Temp, Count_Temp
	ORDER BY
		--Ascending
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'AllocatedQty' THEN AllocatedQty
				WHEN @SortBy = 'Qty' THEN Qty
				WHEN @SortBy = 'orderQty' THEN Qty
				WHEN @SortBy = 'price' THEN Price
				WHEN @SortBy = 'lineNum' THEN LineNum
				WHEN ISNULL(@SortBy, 'SalesOrderID') = 'SalesOrderID' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)				
				WHEN @SortBy = 'orderNumber' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
				WHEN @SortBy = 'DisplayID' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
			END 
		END ASC,
		CASE WHEN @DescSort = 0 THEN	
			CASE
				WHEN @SortBy = 'customer' THEN AccountName
				WHEN @SortBy = 'packaging' THEN PackagingName
				WHEN @SortBy = 'AccountName' THEN AccountName
				WHEN @SortBy = 'PartNumber' THEN PartNumber
				WHEN @SortBy = 'MfrName' THEN MfrName
				WHEN @SortBy = 'CommodityName' THEN CommodityName
				WHEN @SortBy = 'dateCode' THEN DateCode
				WHEN @SortBy = 'buyers' THEN Buyers
				WHEN @SortBy = 'salesPerson' THEN [Owner]	
				WHEN @SortBy = 'Commodity' THEN CommodityName
				WHEN @SortBy = 'Owner' THEN [Owner]
			END 
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN @SortBy = 'ShipDate' THEN ShipDate
			END 
		END ASC,
		--Descending
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'AllocatedQty' THEN AllocatedQty
				WHEN @SortBy = 'Qty' THEN Qty
				WHEN @SortBy = 'orderQty' THEN Qty
				WHEN @SortBy = 'price' THEN Price
				WHEN @SortBy = 'lineNum' THEN LineNum
				WHEN ISNULL(@SortBy, 'SalesOrderID') = 'SalesOrderID' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)				
				WHEN @SortBy = 'orderNumber' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
				WHEN @SortBy = 'DisplayID' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
			END 
		END DESC,
		CASE WHEN @DescSort = 1 THEN	
			CASE
				WHEN @SortBy = 'customer' THEN AccountName
				WHEN @SortBy = 'packaging' THEN PackagingName
				WHEN @SortBy = 'AccountName' THEN AccountName				
				WHEN @SortBy = 'PartNumber' THEN PartNumber
				WHEN @SortBy = 'MfrName' THEN MfrName
				WHEN @SortBy = 'dateCode' THEN DateCode
				WHEN @SortBy = 'CommodityName' THEN CommodityName
				WHEN @SortBy = 'salesPerson' THEN [Owner]	
				WHEN @SortBy = 'Commodity' THEN CommodityName
				WHEN @SortBy = 'buyers' THEN Buyers
				WHEN @SortBy = 'Owner' THEN [Owner]				
			END 
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE
				WHEN @SortBy = 'ShipDate' THEN ShipDate
			END 
		END DESC

	OFFSET @RowOffset ROWS
	FETCH NEXT NULLIF(@RowLimit,0) ROWS ONLY
END