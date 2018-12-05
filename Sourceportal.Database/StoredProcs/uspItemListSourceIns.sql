/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.11.09
   Description:	Creates source records from ItemListID
   Usage:		EXEC uspItemListSourceIns 1098
				
   Return Codes:
				-1 No rows inserted/invalid ItemListID
   Revision History:
   2018.01.03	AR	DateCode was not being pulled from target date code
   2017.01.18	AR	Added SPQ/MOQ/Cost column insert
   ============================================= */



CREATE PROCEDURE [dbo].[uspItemListSourceIns]
	@ItemListID INT,
	@SourceTypeID INT,
	@UserID INT = NULL

AS
BEGIN
InsertSource:
	INSERT INTO Sources (SourceTypeID, ItemID, CommodityID, AccountID, ContactID, CurrencyID, PartNumber, PartNumberStrip, Manufacturer, Qty, Cost, DateCode, PackagingID, MOQ, SPQ, LeadTimeDays, ValidForHours, RequestToBuy, RTBQty, CreatedBy)
	SELECT @SourceTypeID,
			LL.ItemID,
			LL.CommodityID,
			AccountID,
			ContactID,
			CurrencyID,
			LL.PartNumber,
			LL.PartNumberStrip,
			LL.Manufacturer,
			Qty,
			LL.TargetPrice,-- Cost		Note: LL.Cost?
			LL.TargetDateCode, --DateCode
			--REPLACE(CONVERT(VARCHAR(32),GETUTCDATE(),111),'/',''), --DateCode,
			NULL, --PackagingID,
			LL.MOQ,
			LL.SPQ,
			NULL, --LeadTimeDays,
			NULL, --ValidForHours,
			0, --RequestToBuy,
			NULL, --RTBQty,
			@UserID
		FROM ItemListLines LL
		LEFT OUTER JOIN Items I on I.PartNumberStrip = LL.PartNumberStrip
		INNER JOIN ItemLists L on L.ItemListID = LL.ItemListID
		WHERE LL.ItemListID = @ItemListID
		IF (@@rowcount = 0)
			RETURN -1	--Invalid ItemListID
END