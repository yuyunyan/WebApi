/* =============================================
	 Author:			Aaron Rodecker
	 Create date:		2018.08.02
	 Description:		Returns display order ID for SalesOrders PurchaseOrders
	 Usage:				SELECT dbo.fnReturnOrderDisplayID(10009, 'ASD')

	 Revision:
	 2018.08.02		AR	Initial Deployment
	============================================= */
CREATE FUNCTION [dbo].[fnReturnOrderDisplayID]
(
	@OrderID VARCHAR(100) = NULL,
	@ExternalID VARCHAR(500) = NULL
)
RETURNS VARCHAR(500)
AS
BEGIN
	RETURN ISNULL(@ExternalID, 'Q' + @OrderID)

END