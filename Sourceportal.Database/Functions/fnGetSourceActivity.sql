CREATE FUNCTION [dbo].[fnGetSourceActivity]
(
	@SourceID INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
	RETURN (
	SELECT ObjectTypeID, COUNT(*) 'Count'
	FROM MapSourcesJoin
	WHERE IsMatch = 1 AND IsDeleted = 0 AND SourceID = @SourceID
	GROUP BY ObjectTypeID
	FOR JSON AUTO)
END
