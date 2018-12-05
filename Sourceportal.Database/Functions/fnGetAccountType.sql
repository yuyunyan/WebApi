CREATE FUNCTION [dbo].[fnGetAccountType]
(
	@AccountTypeID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
    RETURN (SELECT [Name] + ', '
		FROM lkpAccountTypes
		WHERE AccountTypeID & @AccountTypeID !=0
		FOR XML PATH(''))
END