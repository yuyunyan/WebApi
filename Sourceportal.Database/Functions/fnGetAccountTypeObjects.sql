-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.01.22
-- Description:			Return JSON of AccountTypeIds and AccountTypeNames for given @AccountID
-- =============================================
CREATE FUNCTION [dbo].[fnGetAccountTypeObjects]
(
	@AccountID INT
)
RETURNS VARCHAR(MAX)
AS
BEGIN
    RETURN (
	--SELECT mat.AccountID, SUBSTRING(
	--	(SELECT distinct CAST(mat.AccountTypeID AS VARCHAR(55)) + ', '
	--	FROM mapAccountTypes mat
	--	FOR XML PATH ('')) ,0, 200) AS AccountTypeIds 
	--, SUBSTRING(
	--	(SELECT distinct act.Name + ', '
	--	FROM mapAccountTypes mat
	--	INNER JOIN lkpAccountTypes act ON mat.AccountTypeID = act.AccountTypeID
	--	FOR XML PATH ('')) ,0, 200) AS AccountTypeNames     
	--FROM   mapAccountTypes mat               
	--WHERE  ( mat.AccountID = @AccountID AND mat.IsDeleted = 0 ) 
	--GROUP BY mat.AccountID
	SELECT 
		' ,' + CAST (mat.AccountTypeID AS VARCHAR(55))
	FROM   mapAccountTypes mat               
	WHERE  ( mat.AccountID = @AccountID AND mat.IsDeleted = 0 )
	FOR XML PATH (''))
END