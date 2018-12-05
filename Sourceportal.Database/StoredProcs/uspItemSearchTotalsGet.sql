/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.08.28
   Description:	Gets a list of items and their total availability
   Usage:		EXEC uspItemSearchTotalsGet @SearchString = 'HMA', @SearchType = 'StartsWith'

   Revision History:
	
   Return Codes:
   ============================================= */
CREATE OR ALTER PROCEDURE [dbo].[uspItemSearchTotalsGet]
	@SearchString VARCHAR(200) = NULL,
	@SearchType VARCHAR(50) = 'StartsWith'
AS
BEGIN
	SET @SearchString = dbo.fnStripNonAlphaNumericWithPlus(@SearchString)

	SELECT
		i.ItemID,
		i.PartNumber,
		i.PartNumberStrip,
		i.MfrID,
		m.MfrName,
		i.CommodityID,
		c.CommodityName,
		ISNULL(po.Qty,0) 'OnOrder',
		ISNULL(inv.Qty,0) 'OnHand',
		ISNULL(inv.Available,0) + ISNULL(po.Available,0) 'Available',
		ISNULL(po.Qty,0) + ISNULL(inv.Qty,0) - ISNULL(inv.Available,0) - ISNULL(po.Available,0) 'Reserved'
	FROM Items i
	INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
	INNER JOIN lkpItemCommodities c ON i.CommodityID = c.CommodityID
	LEFT OUTER JOIN (SELECT ItemID, SUM(Qty) 'Qty', SUM(Available) 'Available' FROM vwAvailableInvPO WHERE [Type] = 'Purchase Order' GROUP BY ItemID) po ON i.ItemID = po.ItemID
	LEFT OUTER JOIN (SELECT ItemID, SUM(Qty) 'Qty', SUM(Available) 'Available' FROM vwAvailableInvPO WHERE [Type] = 'Inventory' GROUP BY ItemID) inv ON i.ItemID = inv.ItemID
	WHERE	(@SearchType = 'StartsWith' AND i.PartNumberStrip LIKE @SearchString + '%')
		OR	(@SearchType = 'Contains' AND i.PartNumberStrip LIKE '%' + @SearchString + '%')
		OR	(@SearchType = 'EndsWith' AND i.PartNumberStrip LIKE '%' + @SearchString)
		AND i.IsDeleted = 0
END
