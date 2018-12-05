/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.03.28
   Description:	Gets a list of sales order lines where the given buyer has accepted sources matched
				BuyerID is required
				AccountID is optional, will filter the results to only sources with the provided AccountID
   Usage: EXEC uspRequestToPurchaseGet @BuyerID = 3
   Revision History:
		2018.05.31	BZ	Added CommentCount and SOVersionID
		2018.05.31	NA	Implemented mapBuyerSORoutes
		2018.06.04	NA	Fixed issue with routed SO lines without sources being omitted from results
		2018.10.09	NA	Fixed sorting by OrderID
   Return Codes:
   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspRequestToPurchaseGet]
	@BuyerID INT = NULL,
	@AccountID INT = NULL,
	@UnderallocatedOnly BIT = 0,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0
AS
BEGIN
	DECLARE @ObjectTypeID INT = 17;  --Sales Order ObjectTypeID
	DECLARE @CommentTypeID INT = 10;  --SOLine CommentTypeID

	WITH Main_Temp AS (
	SELECT	sol.SOLineID,
			sol.SalesOrderID,
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
			dbo.fnGetObjectOwners(sol.SalesOrderID, 16) 'Owner',
			s.SourceCount,
			sol.SOVersionID 'VersionID',
			so.ExternalID,
			dbo.fnGetCommentsCount(sol.SOLineID, @CommentTypeID) 'Comments'
	FROM vwSalesOrderLines sol
	  LEFT OUTER JOIN (	SELECT sj.ObjectID, COUNT(*) 'SourceCount'
					FROM Sources s
					INNER JOIN mapSourcesJoin sj 
						ON s.SourceID = sj.SourceID 
						AND sj.ObjectTypeID = @ObjectTypeID 
						AND sj.IsMatch = 1 
						AND sj.IsDeleted = 0
					WHERE AccountID = ISNULL(@AccountID, AccountID)
					AND s.SourceTypeID <> 6
					GROUP BY sj.ObjectID) s ON sol.SOLineID = s.ObjectID
	  INNER JOIN mapBuyerSORoutes r ON sol.SOLineID = r.SOLineID AND r.UserID = @BuyerID AND r.IsDeleted = 0
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
	  AND (@AccountID IS NULL OR s.SourceCount > 0)
	),
	Count_Temp AS (
		SELECT COUNT(*) AS TotalRowCount
		FROM Main_Temp)

	SELECT * FROM Main_Temp, Count_Temp
	ORDER BY
		--Ascending
		CASE WHEN @DescSort = 0 THEN
			CASE
				WHEN ISNULL(@SortBy, 'SalesOrderID') = 'SalesOrderID' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
				WHEN @SortBy = 'DisplayID' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
				WHEN @SortBy = 'AllocatedQty' THEN AllocatedQty
				WHEN @SortBy = 'Qty' THEN Qty
			END 
		END ASC,
		CASE WHEN @DescSort = 0 THEN	
			CASE
				WHEN @SortBy = 'AccountName' THEN AccountName
				WHEN @SortBy = 'PartNumber' THEN PartNumber
				WHEN @SortBy = 'MfrName' THEN MfrName
				WHEN @SortBy = 'CommodityName' THEN CommodityName
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
				WHEN ISNULL(@SortBy, 'SalesOrderID') = 'SalesOrderID' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
				WHEN @SortBy = 'DisplayID' THEN CAST(ISNULL(ExternalID, SalesOrderID) AS INT)
				WHEN @SortBy = 'AllocatedQty' THEN AllocatedQty
				WHEN @SortBy = 'Qty' THEN Qty
			END 
		END DESC,
		CASE WHEN @DescSort = 1 THEN	
			CASE				
				WHEN @SortBy = 'AccountName' THEN AccountName
				WHEN @SortBy = 'PartNumber' THEN PartNumber
				WHEN @SortBy = 'MfrName' THEN MfrName
				WHEN @SortBy = 'CommodityName' THEN CommodityName
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