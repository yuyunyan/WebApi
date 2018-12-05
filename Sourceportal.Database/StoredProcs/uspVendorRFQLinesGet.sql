/* =============================================
    Author:		Manuka Leelaratne
   Create date: 2017.09.06
   Description:	Gets the line items that make up a RFQ
   Usage: EXEC uspVendorRFQLinesGet  @partnumberstrip = 'AD1940YSTZRL%' , @statusId = 47 , @RfqID=23
		EXEC uspVendorRFQLinesGet @RfqID = 2 , @statusId = 8

   Revision History:
	2017.09.08		Added @returnUnsentLines, Added columns from VendorRFQs, Added 'Age' calculation based on VendorRFQs.SentDate, Added OwnerName	
	2018.01.17		Added StatusId and removed returnUnsentLines	
	2018.04.03	NA  Added QuoteLineID
	2018.04.12	AR	refactored subquery for SourcesTotalQty with fnGetRfqLineResponseCount in SELECT
   Return Codes:

   ============================================= */

CREATE PROCEDURE [dbo].[uspVendorRFQLinesGet]
	@RfqLineID INT = NULL,
	@RfqID INT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@partnumberstrip NVARCHAR(32) = NULL,
	@statusId INT = NULL --Gets Waiting Status ID for RFQ
AS
BEGIN
WITH Main_CTE AS(
	SELECT 
			VRL.VRFQLineID
			, VRL.QuoteLineID
			, VRL.LineNum
			, VRL.PartNumber
			, VRL.Manufacturer
			, VRL.CommodityID
			, C.CommodityName
			, VRL.Qty
			, VRL.TargetCost
			, VRL.DateCode
			, VRL.PackagingID
			, P.PackagingName
			, VRL.Note
			, dbo.fnGetRfqLineResponseCount(VRL.VRFQLineID) SourcesTotalQty
			, VRL.StatusID
			, VRL.ItemID
			, VRL.PartNumberStrip
			, VRL.HasNoStock
			, A.AccountName
			, A.AccountID
			, RC.ContactID
			, RC.FirstName + ' ' + RC.LastName AS 'ContactName'
			, R.SentDate 
			, DATEDIFF(day, R.SentDate, GETDATE()) AS 'Age'
			, dbo.fnGetRfqOwners(VRL.VendorRFQID) AS 'OwnerName'
			, R.VendorRFQID
	FROM VendorRFQLines VRL
	  INNER JOIN VendorRFQs R ON VRL.VendorRFQID = R.VendorRFQID
	  INNER JOIN Accounts A ON R.AccountID = A.AccountID
	  INNER JOIN Contacts RC ON R.ContactID = RC.ContactID
	  LEFT OUTER JOIN lkpStatuses S ON VRL.StatusID = S.StatusID 
	  INNER JOIN lkpItemCommodities C ON VRL.CommodityID = C.CommodityID AND C.IsDeleted = 0
	  LEFT OUTER JOIN codes.lkpPackaging P ON VRL.PackagingID = P.PackagingID
	WHERE (VRL.VRFQLineID = @RfqLineID OR VRL.VendorRFQID = @RfqID OR VRL.PartNumberStrip like @partnumberstrip + '%') AND VRL.IsDeleted = 0 AND VRL.StatusID = ISNULL(@statusId,VRL.StatusID)) ,
	  
	  Count_CTE AS (
		SELECT COUNT(*) AS [RowCount]
		FROM Main_CTE
	  )

	  SELECT *
FROM Main_CTE, Count_CTE
	
	ORDER BY 
		
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_CTE.VRFQLineID
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'LineNum' THEN Main_CTE.LineNum 
				WHEN @SortBy = 'Qty' THEN Main_CTE.Qty 
				WHEN @SortBy = 'TargetCost' THEN Main_CTE.TargetCost 
				WHEN @SortBy = 'SourcesTotalQty' THEN Main_CTE.SourcesTotalQty 
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'PartNumber' THEN Main_CTE.PartNumber
				WHEN @SortBy = 'Manufacturer' THEN Main_CTE.Manufacturer
				WHEN @SortBy = 'CommodityName' THEN Main_CTE.CommodityName
				WHEN @SortBy = 'DateCode' THEN Main_CTE.DateCode
				WHEN @SortBy = 'PackagingName' THEN Main_CTE.DateCode
			END
		END ASC,
		
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN Main_CTE.VRFQLineID
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'LineNum' THEN Main_CTE.LineNum 
				WHEN @SortBy = 'Qty' THEN Main_CTE.Qty 
				WHEN @SortBy = 'TargetCost' THEN Main_CTE.TargetCost 
				WHEN @SortBy = 'SourcesTotalQty' THEN Main_CTE.SourcesTotalQty 
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'PartNumber' THEN Main_CTE.PartNumber
				WHEN @SortBy = 'Manufacturer' THEN Main_CTE.Manufacturer
				WHEN @SortBy = 'CommodityName' THEN Main_CTE.CommodityName
				WHEN @SortBy = 'DateCode' THEN Main_CTE.DateCode
				WHEN @SortBy = 'PackagingName' THEN Main_CTE.DateCode
			END
		END DESC

		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY


	IF (@@rowcount = 0)
		RETURN -1

END
