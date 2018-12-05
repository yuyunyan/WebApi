CREATE PROCEDURE [dbo].[uspItemCommoditiesGet]
	@ItemGroupID INT = NULL
AS
BEGIN
	SELECT 
		c.CommodityID,
		c.CommodityName,
		c.Code 'CommodityCode',
		c.ItemGroupID,
		c.ExternalID,
		g.GroupName
	FROM lkpItemCommodities c
	  INNER JOIN lkpItemGroups g ON c.ItemGroupID = g.ItemGroupID
	WHERE c.IsDeleted = 0
	  AND c.ItemGroupID = ISNULL(@ItemGroupID, c.ItemGroupID)
	ORDER BY c.CommodityName
END
