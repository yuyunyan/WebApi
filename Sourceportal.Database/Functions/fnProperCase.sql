CREATE FUNCTION [dbo].[fnProperCase]
(
	@Text VARCHAR(8000)
)
RETURNS VARCHAR(8000)
AS
BEGIN
	DECLARE @Reset BIT;
	DECLARE @Ret VARCHAR(8000);
	DECLARE @i INT;
	DECLARE @c CHAR(1);

	SELECT @Reset = 1, @i=1, @Ret = '';
   
	WHILE (@i <= LEN(@Text))
   	SELECT @c= SUBSTRING(@Text,@i,1),
               @Ret = @Ret + CASE WHEN @Reset=1 THEN UPPER(@c) ELSE LOWER(@c) END,
               @Reset = CASE WHEN @c LIKE '[a-zA-Z]' THEN 0 ELSE 1 END,
               @i = @i +1
	RETURN @Ret
END