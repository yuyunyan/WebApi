/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.31
   Description:	Retrieves Sales Order BOMB results
   Usage:		EXEC uspBOMQuoteList_Get
   Revision History:
   ============================================= */
CREATE PROCEDURE [dbo].[uspBOMSalesOrderListGet]
(
	@ItemListID INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT LL.ItemListID,
	LL.ItemListLineID,
	SL.StatusID,
	SL.ItemID,
	LL.CommodityID,
	SL.CustomerPartNum,
	SL.PartNumberStrip,
	LL.Manufacturer,
	SL.Qty,
	LL.TargetPrice,
	SL.DueDate,
	SL.Created,
	SL.CreatedBy
	FROM ItemListLines LL
	LEFT OUTER JOIN SalesOrderLines SL on SL.PartNumberStrip LIKE LL.PartNumberStrip + '%'
	WHERE LL.ItemListLineID = @ItemListID
END