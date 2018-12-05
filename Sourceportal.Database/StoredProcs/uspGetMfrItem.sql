/* =============================================
   Author:				Berry, Zhong
   Create date:			2017.10.18
   Description:			Return manufacturer of ItemId
   =============================================*/
CREATE PROCEDURE [dbo].[uspGetMfrItem]
	@ItemID INT = NULL
AS
BEGIN
	IF ISNULL(@ItemID, 0) = 0
		RETURN -10

	SELECT 
		m.MfrName
	FROM Items i
		INNER JOIN Manufacturers m ON i.MfrID = m.MfrID
	WHERE i.ItemID = @ItemID
END
GO
