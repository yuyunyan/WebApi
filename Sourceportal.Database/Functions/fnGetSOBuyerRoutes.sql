CREATE FUNCTION [dbo].[fnGetSOBuyerRoutes]
(
	@SOLineID INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN	
	RETURN (	
	SELECT	r.SOLineID,
		u.FirstName + ' ' + u.LastName 'BuyerName'
	FROM mapBuyerSORoutes r	
	INNER JOIN Users u ON r.UserID = u.UserID
	WHERE r.SOLineID = @SOLineID	
	FOR JSON PATH)
END
