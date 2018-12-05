-- =============================================
-- Author:			Berry, Zhong
-- Create date:		02.27.2018
-- Description:		Strip out non numbric 
-- =============================================
CREATE FUNCTION [dbo].[fnStripNonNumeric]
(
	@Temp VarChar(500)
)
RETURNS VARCHAR(500)
AS
BEGIN

	Declare @KeepValues as varchar(50)
    Set @KeepValues = '%[^0-9]%'
    While PatIndex(@KeepValues, @Temp) > 0
        Set @Temp = Stuff(@Temp, PatIndex(@KeepValues, @Temp), 1, '')

    Return @Temp

END