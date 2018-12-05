/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.22
   Description:	Gets the line items that make up a Quote
   Usage: EXEC uspQuoteLinesGet @QuoteLineID = 7
		  EXEC uspQuoteLinesGet @QuoteID = 100019, @QuoteVersionID = 2, @IsPrinted = 1

   Revision History:
		2017.07.10	NA	Implemented AltCount, pagination and remote sorting
		2017.07.17	ML	Added Manufacturer
		2017.08.22	BZ	Added Comments
		2017.11.08  CT  Added ItemExternalId
		2017.11.20	BZ	Added RouteToId
		2018.02.07	AR	Added IsPrinted col, where clause/parameter
		2018.02.12	AR	Added PartNoCombined col, MfgDateCodeCombined col
		2018.02.14	AR	Added column LineNumGrouped and logic to append alt parts with 'A,B,C'
		2018.02.15	AR	Modified A,B,C LineNumGroup logic to always append 'A' instead
		2018.03.20	BZ	Added RouteTo
		2018.04.06	NA	Modified to use new mapBuyerQuoteRoutes table name
		2018.04.09	NA	Added fnGetQuoteRouteIcons function, removed old commented out code and old comment count placeholder
		2018.04.10	NA	Added HasSourceMatch value
		2018.04.13	BZ  Added FilterText, FilterBy
		2018.04.18	BZ	Added PartNumberStrip
		2018.04.19	NA	Fixed HasMatch to return NULL if it has matches set to 0 
		2018.04.26	BZ	Added LeadTimeDays
		2018.05.09	AR	Added SourceMatchCount & SourceType columns
		2018.10.19  NA  Fixed IsMatch condition in subquery 
   Return Codes:

   ============================================= */
   
CREATE PROCEDURE [dbo].[uspQuoteLinesGet]
	@QuoteLineID INT = NULL,
	@QuoteID INT = NULL,
	@QuoteVersionID INT = NULL,
	@AltFor INT = 0,
	@RowOffset INT = 0,
	@RowLimit INT = 50,
	@SortBy NVARCHAR(25) = '', 
	@DescSort BIT = 0,
	@FilterBy NVARCHAR(25) = NULL,
	@FilterText NVARCHAR(25) = NULL,
	@CommentTypeID INT = 0,
	@IsPrinted BIT = NULL
AS
BEGIN
	DECLARE @QuoteLineObjectTypeID INT = 20
	
	SELECT 
			ql.QuoteLineID
			, ql.StatusID
			, s.StatusName
			, s.IsCanceled 'StatusIsCanceled'
			, ql.LineNum
			, ql.AltFor
			, ql.ItemID
			, i.ExternalID 'ItemExternalId'
			, ql.CustomerLine
			, ql.PartNumber
			, ql.PartNumberStrip
			, ql.Manufacturer
			, ql.CommodityID
			, c.CommodityName
			, ql.CustomerPartNum
			, ql.Qty
			, ql.Price
			, ql.Cost
			, CASE WHEN (ql.Qty * ql.Price) <> 0 THEN ((ql.Qty * ql.Price) - (ql.Qty * ql.Cost)) / (ql.Qty * ql.Price) ELSE 0 END 'GPM'
			, CONVERT(VARCHAR(15), ql.Price, 1) + ' ' + Q.CurrencyID 'PriceFormatted'
			, CONVERT(VARCHAR(15), CAST(ROUND(ql.Qty * ql.Price, 2) AS NUMERIC(12,2)), 1) + ' ' + Q.CurrencyID 'ExtPrice'
			, ql.DateCode
			, ql.PackagingID
			, p.PackagingName
			, ql.ShipDate
			, ql.LeadTimeDays
			, CONVERT(CHAR(15), ql.ShipDate, 106) 'ShipDateFormatted'
			, ql.RoutedToUserID 'RouteToID'
			, dbo.fnGetQuoteRouteIcons(ql.QuoteLineID) 'RoutedTo'			
			, ql.IsRoutedToBuyers
			, ISNULL(alts.AltCount, 0) 'AltCount'
			, CASE WHEN ql.AltFor > 0 THEN CONVERT(VARCHAR(20), qp.LineNum ) + 'A' ELSE CONVERT(VARCHAR(20), ql.LineNum) END 'LineNumGrouped'			
			, COUNT(*) OVER() AS 'TotalRows'
			, dbo.fnGetCommentsCount(ql.QuoteLineID, @CommentTypeID) 'Comments'
			, ql.PartNumber + ISNULL(NULLIF(' / ' + ql.Manufacturer,''),'') 'PartNoMfgCombined'
			, ql.Manufacturer + ISNULL(NULLIF(' / ' + ql.DateCode,''),'')  'MfgDateCodeCombined'
			, ql.IsPrinted
			, CASE WHEN ql.AltFor > 0 THEN qp.LineNum + qp.SortDifferentiator Else ql.LineNum END 'SortBy'
			, sj.Records SourceMatchCount
			, sj.TotalQty SourceMatchQty
			, CASE
				WHEN sj.Records > 1 THEN 'Multiple' 
				WHEN sj.Records = 1 THEN ST.TypeName
				ELSE NULL
			  END SourceType
			, CASE
				WHEN sj.HasMatch = 1 THEN 1
				WHEN sj.HasMatch = 0 THEN 0
				WHEN sj.HasMatch IS NULL AND sj.Records IS NOT NULL THEN 0				
				ELSE NULL
			  END 'HasSourceMatch'
	FROM QuoteLines ql
	  INNER JOIN Quotes Q on Q.QuoteID = ql.QuoteID AND Q.VersionID = ISNULL(@QuoteVersionID, ql.QuoteVersionID)
	  LEFT OUTER JOIN lkpStatuses s ON ql.StatusID = s.StatusID AND s.IsDeleted = 0
	  LEFT OUTER JOIN lkpItemCommodities c ON ql.CommodityID = c.CommodityID AND c.IsDeleted = 0
	  LEFT OUTER JOIN codes.lkpPackaging p ON ql.PackagingID = p.PackagingID
	  LEFT OUTER JOIN Items i ON ql.ItemID = i.ItemID
	  LEFT OUTER JOIN ( SELECT ObjectID,
						MAX(J.SourceID) TopSourceID,
						 MAX(CAST(IsMatch AS INT)) 'HasMatch',
						 COUNT(ObjectID) 'Records',
						 SUM(S.Qty) TotalQty
						--SourceID
						FROM mapSourcesJoin J
						INNER JOIN Sources S on S.SourceID = J.SourceID
						WHERE ObjectTypeID = @QuoteLineObjectTypeID
						AND S.IsDeleted = 0 
						AND J.IsDeleted = 0
						AND ISNULL(IsMatch, 1) = 1
						GROUP BY ObjectID
						) sj ON ql.QuoteLineID = sj.ObjectID
	LEFT OUTER JOIN Sources TS on TS.SourceID = sj.TopSourceID
	LEFT OUTER JOIN lkpSourceTypes ST on St.SourceTypeID = TS.SourceTypeID
	--LEFT OUTER JOIN Sources SO on SO.SourceID = sj.SourceID
	--LEFT OUTER JOIN lkpSourceTypes ST on ST.SourceTypeID = SO.SourceTypeID
	  --Get alt parts (children) for ParentIDs 
	  LEFT OUTER JOIN (	SELECT qll.AltFor 'ParentID',
								COUNT(qll.QuoteLineID) 'AltCount'
						FROM QuoteLines  qll
						WHERE (qll.QuoteLineID = @QuoteLineID
								OR (qll.QuoteID = @QuoteID AND qll.QuoteVersionID = @QuoteVersionID))
							AND qll.IsDeleted = 0
						GROUP BY qll.AltFor
					  ) alts ON ql.QuoteLineID = alts.ParentID

	--Get Parent AltFor's LineNum
	OUTER APPLY (SELECT qp.LineNum,
					ROW_NUMBER() OVER(ORDER BY qp.QuoteLineID DESC) * .01 'SortDifferentiator'
				FROM QuoteLines qp WHERE qp.QuoteLineID = ql.AltFor
				) QP

	WHERE (ql.QuoteLineID = @QuoteLineID
			OR (ql.QuoteID = @QuoteID AND ql.QuoteVersionID = @QuoteVersionID))
	  AND ql.IsDeleted = 0	  
	  AND ql.IsPrinted = ISNULL(@IsPrinted,ql.IsPrinted)
	  AND (@FilterBy IS NULL OR (
                     (@FilterBy = 'partNumber' AND ((ql.PartNumber LIKE '%' + ISNULL(@FilterText , '') + '%')
						OR (EXISTS (SELECT 1 
						  FROM QuoteLines subQl 
				          WHERE subQl.AltFor = ql.QuoteLineID
						    AND (ISNULL(subQl.PartNumber, '') LIKE '%' + ISNULL(@FilterText, '') + '%')
						  )))
					 )
                     OR 
					  (@FilterBy = 'manufacturer' AND ((ql.Manufacturer LIKE '%' + ISNULL(@FilterText , '') + '%')
						OR (EXISTS (SELECT 1 
						  FROM QuoteLines subQl 
				          WHERE subQl.AltFor = ql.QuoteLineID
						    AND (ISNULL(subQl.Manufacturer, '') LIKE '%' + ISNULL(@FilterText, '') + '%')
						  )))
					  )
					 OR
					  (@FilterBy = 'customerPN' AND ((ql.CustomerPartNum LIKE '%' + ISNULL(@FilterText , '') + '%')
						OR (EXISTS (SELECT 1 
						  FROM QuoteLines subQl 
				          WHERE subQl.AltFor = ql.QuoteLineID
						    AND (ISNULL(subQl.CustomerPartNum, '') LIKE '%' + ISNULL(@FilterText, '') + '%')
						  )))
					  )
					  OR
					  (@FilterBy = 'commodity' AND ((c.CommodityName LIKE '%' + ISNULL(@FilterText , '') + '%')
						OR (EXISTS (SELECT 1 
						  FROM QuoteLines subQl 
						  LEFT OUTER JOIN lkpItemCommodities subC ON subQl.CommodityID = subC.CommodityID AND subC.IsDeleted = 0
				          WHERE subQl.AltFor = ql.QuoteLineID
						    AND (ISNULL(subC.CommodityName, '') LIKE '%' + ISNULL(@FilterText, '') + '%')
						  )))
					  )
					  OR
					  (@FilterBy = 'status' AND ((s.StatusName LIKE '%' + ISNULL(@FilterText , '') + '%')
						OR (EXISTS (SELECT 1 
						  FROM QuoteLines subQl 
						  LEFT OUTER JOIN lkpStatuses subS ON subQl.StatusID = subS.StatusID AND subS.IsDeleted = 0
				          WHERE subQl.AltFor = ql.QuoteLineID
						    AND (ISNULL(subS.StatusName, '') LIKE '%' + ISNULL(@FilterText, '') + '%')
						  )))
					  )
					  OR
					  (@FilterBy = 'routedTo' AND ((dbo.fnGetQuoteRouteIcons(ql.QuoteLineID) LIKE '%' + ISNULL(@FilterText , '') + '%')
						OR (EXISTS (SELECT 1 
						  FROM QuoteLines subQl 
				          WHERE subQl.AltFor = ql.QuoteLineID
						    AND (ISNULL(dbo.fnGetQuoteRouteIcons(subQl.QuoteLineID), '') LIKE '%' + ISNULL(@FilterText, '') + '%')
						  )))
					   )
            ))
	ORDER BY 
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN ql.LineNum
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'ShipDate' THEN ql.ShipDate
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'Qty' THEN ql.Qty
				WHEN @SortBy = 'Price' THEN ql.Price
				WHEN @SortBy = 'Cost' THEN ql.Cost
				WHEN @SortBy = 'CustomerLine' THEN ql.CustomerLine
				WHEN @SortBy = 'GPM' THEN CASE WHEN (ql.Qty * ql.Price) <> 0 THEN ((ql.Qty * ql.Price) - (ql.Qty * ql.Cost)) / (ql.Qty * ql.Price) ELSE 0 END
				WHEN @SortBy = 'HasSourceMatch' THEN sj.HasMatch
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN
			CASE 
				WHEN @SortBy = 'PackagingName' THEN p.PackagingName
				WHEN @SortBy = 'CustomerPartNum' THEN ql.CustomerPartNum
				WHEN @SortBy = 'CommodityName' THEN c.CommodityName
				WHEN @SortBy = 'PartNumber' THEN ql.PartNumber
				WHEN @SortBy = 'StatusName' THEN s.StatusName
				WHEN @SortBy = 'LineNumGrouped' THEN CASE WHEN ql.AltFor > 0 THEN qp.LineNum + qp.SortDifferentiator Else ql.LineNum END				
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN ql.LineNum
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'ShipDate' THEN ql.ShipDate
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'Qty' THEN ql.Qty
				WHEN @SortBy = 'Price' THEN ql.Price
				WHEN @SortBy = 'Cost' THEN ql.Cost
				WHEN @SortBy = 'CustomerLine' THEN ql.CustomerLine
				WHEN @SortBy = 'GPM' THEN CASE WHEN (ql.Qty * ql.Price) <> 0 THEN ((ql.Qty * ql.Price) - (ql.Qty * ql.Cost)) / (ql.Qty * ql.Price) ELSE 0 END
				WHEN @SortBy = 'HasSourceMatch' THEN sj.HasMatch
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN
			CASE 
				WHEN @SortBy = 'PackagingName' THEN p.PackagingName
				WHEN @SortBy = 'CustomerPartNum' THEN ql.CustomerPartNum
				WHEN @SortBy = 'CommodityName' THEN c.CommodityName
				WHEN @SortBy = 'PartNumber' THEN ql.PartNumber
				WHEN @SortBy = 'StatusName' THEN s.StatusName
				WHEN @SortBy = 'LineNumGrouped' THEN CASE WHEN ql.AltFor > 0 THEN qp.LineNum + qp.SortDifferentiator Else ql.LineNum END				
			END
		END DESC, alts.AltCount
	OFFSET @RowOffset ROWS
	FETCH NEXT @RowLimit ROWS ONLY
	  
END