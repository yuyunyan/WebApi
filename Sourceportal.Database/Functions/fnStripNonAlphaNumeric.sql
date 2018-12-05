CREATE FUNCTION [dbo].[fnStripNonAlphaNumeric]
(
    @input VARCHAR(500)
)
RETURNS VARCHAR(500)
AS
BEGIN
    
    DECLARE @i INT
    DECLARE @result VARCHAR(500)
    SET @result = @input
    SET @i = PATINDEX('%[^a-zA-Z0-9]%', @result)
    WHILE @i > 0
    BEGIN
        SET @result = STUFF(@result, @i, 1, '')
        SET @i = PATINDEX('%[^a-zA-Z0-9]%', @result)
    END

    RETURN @result

END