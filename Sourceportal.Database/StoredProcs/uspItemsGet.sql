/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.26
   Description:	Gets details for one or more items
   Usage: EXEC uspItemsGet @ItemID = 1

   Revision History:
	   2017.06.29	AR	Added ItemStatusID column
	   2017.07.14	NA	Added TotalRows and cleaned up sort logic
	   2017.10.13	AR	Renamed sort columns to match angular endpoints, added part number to sort columns
	   2017.10.24	BZ	Adding security
	   2017.11.08	CT  Added HTS and MSL fields
	   2017.11.28	BZ	Added Status
	   2017.12.08   ML  Added SourceDataID
	   2018.01.05   ML  Added ExternalID
	   2018.02.19	CT  Added PartNumberStrip
	   2018.06.25	NA	Added ItemID NULL check to IsDeleted filter
   Return Codes:
   ============================================= */


CREATE PROCEDURE [dbo].[uspItemsGet]
	@ItemID INT = NULL,
	@SearchString NVARCHAR(32) = '',
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 0,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Search VARCHAR(32) = dbo.fnStripNonAlphaNumeric(ISNULL(@SearchString, '')) + '%'
	DECLARE @Sec TABLE (ItemID INT, RoleID INT)
	INSERT @Sec EXECUTE uspItemSecurityGet @UserID = @UserID;
	
	WITH Main_CTE AS (
		SELECT
			i.ItemID,
			i.MfrID,
			m.MfrName,
			i.CommodityID,
			c.CommodityName,
			c.ItemGroupID,
			g.GroupName,
			i.PartNumber,
			i.PartNumberStrip,
			i.DatasheetURL,
			i.PartDescription,
			i.MfrDescription,
			i.ECCN,
			i.EURoHS,
			i.CNRoHS,
			i.WeightG,
			i.LengthCM,
			i.WidthCM,
			i.DepthCM,
			i.ItemStatusID,
			iSts.StatusName 'Status',			
			i.HTS,
			i.MSL,
			i.SourceDataID,
			i.ExternalID,
			COUNT(*) OVER() AS 'TotalRows'
		FROM Items i
		  LEFT OUTER JOIN Manufacturers m ON i.MfrID = m.MfrID
		  LEFT OUTER JOIN lkpItemCommodities c ON i.CommodityID = c.CommodityID
		  LEFT OUTER JOIN lkpItemGroups g ON c.ItemGroupID = g.ItemGroupID
		  LEFT OUTER JOIN lkpItemStatuses iSts ON iSts.ItemStatusID = i.ItemStatusID AND iSts.IsDeleted = 0
		  INNER JOIN (SELECT DISTINCT ItemID FROM @Sec) sec ON i.ItemID = sec.ItemID
		WHERE i.ItemID = ISNULL(NULLIF(@ItemID,0), i.ItemID) 
		  AND (@ItemID IS NOT NULL OR i.IsDeleted = 0)
		  AND (@Search = '%' OR i.PartNumberStrip LIKE @Search)
	),
	Count_CTE AS (
		SELECT COUNT(*) 'TotalRows'
		FROM Main_CTE
	)

	SELECT * FROM Main_CTE m, Count_CTE
	ORDER BY
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'ManufacturerName' THEN m.MfrName 
				WHEN @SortBy = 'ManufacturerPartNumber' THEN m.PartNumber
				WHEN @SortBy = 'CommodityName' THEN m.CommodityName
				WHEN @SortBy = 'GroupName' THEN m.GroupName
				WHEN @SortBy = 'HTS' THEN m.HTS
				WHEN @SortBy = 'MSL' THEN m.MSL
				ELSE m.PartNumber
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN 
			CASE 
				WHEN @SortBy = 'ManufacturerName' THEN m.MfrName 
				WHEN @SortBy = 'ManufacturerPartNumber' THEN m.PartNumber
				WHEN @SortBy = 'CommodityName' THEN m.CommodityName
				WHEN @SortBy = 'GroupName' THEN m.GroupName
				WHEN @SortBy = 'HTS' THEN m.HTS
				WHEN @SortBy = 'MSL' THEN m.MSL
				ELSE m.PartNumber
			END
		END DESC
		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY
END

