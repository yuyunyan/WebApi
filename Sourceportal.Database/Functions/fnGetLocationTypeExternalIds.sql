CREATE FUNCTION [dbo].[fnGetLocationTypeExternalIds]
(
	@LocationID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
    DECLARE @Names VARCHAR(8000) 

	SELECT @Names =  COALESCE(@Names + ', ', '') + T.ExternalID 
	FROM Locations L
	INNER JOIN lkpLocationTypes T on T.LocationTypeID & L.LocationTypeID = T.LocationTypeID
	WHERE L.LocationID = @LocationID
	GROUP BY L.LocationID, T.ExternalID 

	RETURN @Names

END