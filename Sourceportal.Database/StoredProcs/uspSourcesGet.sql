/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.27
   Description:	Retrieves Sources for a given ItemID or Part Number
   Usage:		EXEC uspSourcesGet @ItemID = 56
				EXEC uspSourcesGet @PartNumber = ''
   Return Codes:
				-2 Either ItemID or PartNumber must be provided
   Revision History:
				2017.07.13  NA  Fixed sorting and added TotalRows
				2017.07.20	NA	Added boolean flags to denote how/if the source is matched to the referenced object
				2017.07.21	NA	Added IsDeleted check to mapSourcesJoin join
				2017.08.22	BZ	Added Comments
				2018.02.15  RV  Added Age In days field , CreatedBy and Created feilds for the tooltip
				2018.02.15  RV  Added Show All param to filter to show only thos match teh objectID or (all) when just partNumber is matched in the mapSourcesJoin table
				2018.03.08	NA	Added Package Condition
				2018.03.13	AR	Added @CommentQuoteLineID to add quote parts
				2018.04.18	BZ	Added IsConfirmed
				2018.04.19	BZ	Added Rating
				2018.06.01	NA	Added AccountIsActive check and Account.IsDeleted check to ensure only sources for active accounts are displayed.  Changed several joins to INNER joins.
				2018.06.03	BZ	Added BuyerID
				2018.09.28	AR	Added Sorting/removed Sorting TypeRank = TypeName, added @ShowInventory parameter/logicr
				2018.10.16	NA	Added Account and Contact parameters
   ============================================= */

ALTER PROCEDURE [dbo].[uspSourcesGet]
	@ItemID INT = NULL,
	@PartNumber VARCHAR(32) = '',
	@AccountID INT = NULL,
	@ContactID INT = NULL,
	@MatchedObjectTypeID INT = NULL,
	@MatchedObjectID INT = NULL,
	@RowOffset INT = 0,
	@RowLimit INT = 500,
	@SortBy NVARCHAR(25) = '',
	@DescSort BIT = 1,
	@CommentTypeID INT = 0,
	@SourceID INT = NULL,
	@ShowAll INT = 1,
	@ShowInventory BIT = 1,
	@CommentQuoteLineID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @PartNumberStrip VARCHAR(32) = '';

	IF ISNULL(@ItemID, 0) = 0 AND ISNULL(@PartNumber, '') <> ''
		SET @PartNumberStrip = dbo.fnStripNonAlphaNumeric(@PartNumber) + '%'
	ELSE
		IF ISNULL(@ItemID, 0) = 0 AND ISNULL(@SourceID, 0) = 0 AND ISNULL(@AccountID, 0) = 0 AND ISNULL(@ContactID, 0) = 0
			RETURN -2
		ELSE
			IF @ItemID IS NOT NULL
				SELECT @PartNumberStrip = PartNumberStrip + '%' FROM items WHERE @ItemID = ItemID
			ELSE 
				SET @PartNumberStrip = '%'
			
	SELECT
		s.SourceID,
		s.SourceTypeID,
		t.TypeName,
		t.TypeRank,
		t.IsConfirmed,
		s.ItemID,
		s.PartNumber,
		s.AccountID,
		a.AccountName,
		s.ContactID,
		con.FirstName + ' ' + con.LastName 'ContactName',
		s.Manufacturer,
		s.CommodityID,
		c.CommodityName,
		s.Qty,
		s.Cost,
		s.DateCode,
		s.PackagingID,
		p.PackagingName,
		s.PackageConditionID,
		pc.ConditionName,
		s.MOQ,
		s.SPQ,
		s.LeadTimeDays,
		s.ValidForHours,				
		FLOOR(CAST(GETUTCDATE() - s.Created AS FLOAT)) 'AgeInDays',
		dbo.fnGetSourceActivity(s.SourceID) 'Activity',
		CASE WHEN sj.SourceID IS NULL THEN 0 ELSE 1 END 'IsJoined',
		sj.IsMatch 'IsMatch',
		sj.Qty 'RTPQty',
		COUNT(*) OVER() AS 'TotalRows',
		dbo.fnGetCommentsCount(s.SourceID, @CommentTypeID) + dbo.fnGetCommentsCount(sj.CommentUID, 25) 'Comments',
		u.FirstName + ' ' + u.LastName 'CreatedBy',
		s.CreatedBy 'BuyerID',
		s.Created,
		mat.Rating
	FROM Sources s
	  INNER JOIN lkpSourceTypes t ON s.SourceTypeID = t.SourceTypeID
	  INNER JOIN lkpItemCommodities c ON s.CommodityID = c.CommodityID
	  INNER JOIN Accounts a ON s.AccountID = a.AccountID AND a.IsDeleted = 0	  
	  INNER JOIN mapAccountTypes mat ON mat.AccountID = a.AccountID AND mat.IsDeleted = 0 AND mat.AccountTypeID & 1 = 1
	  INNER JOIN lkpAccountStatuses ast ON mat.AccountStatusID = ast.AccountStatusID AND ast.AccountIsActive = 1
	  LEFT OUTER JOIN Contacts con ON s.ContactID = con.ContactID 
	  INNER JOIN Users u on s.CreatedBy = u.UserID
	  LEFT OUTER JOIN codes.lkpPackaging p ON s.PackagingID = p.PackagingID
	  LEFT OUTER JOIN codes.lkpPackageConditions pc ON s.PackageConditionID = pc.PackageConditionID
	  LEFT OUTER JOIN mapSourcesJoin sj ON s.SourceID = sj.SourceID AND sj.ObjectTypeID = @MatchedObjectTypeID AND sj.ObjectID = @MatchedObjectID AND sj.IsDeleted = 0
	WHERE s.IsDeleted = 0
	 AND( 
            (@ShowAll = 1 AND (s.PartNumberStrip LIKE @PartNumberStrip)) 
                OR
            (@ShowAll = 0 AND (sj.ObjectID = @MatchedObjectID))
                OR
            (s.SourceTypeID = 6 AND s.Qty > 0 AND (s.PartNumberStrip LIKE @PartNumberStrip)) --sourceType = 'Inventory'
          )
	AND s.SourceTypeID != (CASE WHEN @ShowInventory != 1 THEN 6 ELSE -1 END)	--Exclude inventory records when @ShowInventory != 1
	AND ISNULL(@SourceID, s.SourceID) = s.SourceID
	AND a.AccountID = ISNULL(@AccountID, a.AccountID)	
	AND ISNULL(con.ContactID, 0) = COALESCE(@ContactID, con.ContactID, 0)
	ORDER BY
		CASE WHEN @DescSort = 0 THEN 
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN t.TypeRank
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN 
			CASE 
				WHEN @SortBy = 'Cost' THEN s.Cost
				WHEN @SortBy = 'MOQ' THEN s.MOQ
				WHEN @SortBy = 'SPQ' THEN s.SPQ
				WHEN @SortBy = 'LeadTimeDays' THEN s.LeadTimeDays
				WHEN @SortBy = 'Rating' THEN mat.Rating
				WHEN @SortBy = 'TypeRank' THEN t.TypeRank
				WHEN @SortBy IN ('Quantity','Qty') THEN s.Qty
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN 
			CASE 
				WHEN @SortBy = 'PackagingName' THEN p.PackagingName
				WHEN @SortBy = 'ConditionName' THEN pc.ConditionName
				WHEN @SortBy = 'AccountName' THEN a.AccountName
				WHEN @SortBy = 'PartNumber' THEN s.PartNumberStrip
				WHEN @SortBy = 'Manufacturer' THEN s.Manufacturer
				WHEN @SortBy = 'DateCode' THEN s.DateCode
				WHEN @SortBy = 'TypeName' THEN t.TypeName
				WHEN @SortBy = 'CreatedBy' THEN u.FirstName + ' ' + u.LastName
			END
		END ASC,
		CASE WHEN @DescSort = 0 THEN 
			CASE 
				WHEN @SortBy = 'AgeInDays' THEN s.Created
				WHEN @SortBy = 'Created' THEN s.Created
			END
		END ASC,
		CASE WHEN @DescSort = 1 THEN 
			CASE 
				WHEN ISNULL(@SortBy, '') = '' THEN t.TypeRank
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN 
			CASE 
				WHEN @SortBy = 'Cost' THEN s.Cost
				WHEN @SortBy = 'MOQ' THEN s.MOQ
				WHEN @SortBy = 'SPQ' THEN s.SPQ
				WHEN @SortBy = 'LeadTimeDays' THEN s.LeadTimeDays
				WHEN @SortBy = 'Rating' THEN mat.Rating
				WHEN @SortBy = 'TypeRank' THEN t.TypeRank
				WHEN @SortBy IN ('Quantity','Qty') THEN s.Qty
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN 
			CASE 
				WHEN @SortBy = 'PackagingName' THEN p.PackagingName
				WHEN @SortBy = 'ConditionName' THEN pc.ConditionName
				WHEN @SortBy = 'AccountName' THEN a.AccountName
				WHEN @SortBy = 'PartNumber' THEN s.PartNumberStrip
				WHEN @SortBy = 'Manufacturer' THEN s.Manufacturer
				WHEN @SortBy = 'DateCode' THEN s.DateCode
				WHEN @SortBy = 'TypeName' THEN t.TypeName
				WHEN @SortBy = 'CreatedBy' THEN u.FirstName + ' ' + u.LastName
			END
		END DESC,
		CASE WHEN @DescSort = 1 THEN 
			CASE 
				WHEN @SortBy IN( 'AgeInDays', 'Created') THEN s.Created
			END
		END DESC
		OFFSET @RowOffset ROWS
		FETCH NEXT @RowLimit ROWS ONLY
END