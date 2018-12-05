CREATE FUNCTION [dbo].[fnGetAccountTypes]
(
	@AccountID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
    DECLARE @Names VARCHAR(8000) 

	SELECT @Names =  COALESCE(@Names + ', ', '') + T.Name 
	FROM Accounts A
	INNER JOIN mapAccountTypes mat on mat.AccountID = A.AccountID
	INNER JOIN lkpAccountTypes T on T.AccountTypeID & mat.AccountTypeID = T.AccountTypeID
	WHERE A.AccountID = @AccountID
	GROUP BY A.AccountID, T.Name 

	RETURN @Names

END