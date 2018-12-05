/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.02.13
   Description:	Formats number in currency format
   Usage:		SELECT dbo.fnFormatWithCommas('12300.56')

   Revision History:
		2018.02.13	AR	Intitial Deployment
   Return Codes:

   ============================================= */
CREATE FUNCTION [dbo].[fnFormatWithCommas] 
(
    @value varchar(50)
)
RETURNS varchar(50)
AS
BEGIN
    -- Declare the return variable here
    DECLARE @WholeNumber varchar(50) = NULL, @Decimal varchar(10) = '', @CharIndex int = charindex('.', @value)

    IF (@CharIndex > 0)
        SELECT @WholeNumber = SUBSTRING(@value, 1, @CharIndex-1), @Decimal = SUBSTRING(@value, @CharIndex, LEN(@value))
    ELSE
        SET @WholeNumber = @value

    IF(LEN(@WholeNumber) > 3)
        SET @WholeNumber = dbo.fnFormatWithCommas(SUBSTRING(@WholeNumber, 1, LEN(@WholeNumber)-3)) + ',' + RIGHT(@WholeNumber, 3)



    -- Return the result of the function
    RETURN @WholeNumber + @Decimal

END