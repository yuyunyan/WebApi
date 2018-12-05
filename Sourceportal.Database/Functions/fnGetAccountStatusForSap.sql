CREATE FUNCTION [dbo].[fnGetAccountStatusForSap]
(
	@AccountID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
	DECLARE @StatusExternalId varchar(5)

	SELECT  TOP 1 @StatusExternalId = S.ExternalID 
	FROM mapAccountTypes mat
	INNER JOIN lkpAccountStatuses S on mat.AccountStatusID = S.AccountStatusID
	WHERE mat.AccountID = @AccountID AND mat.IsDeleted = 0
	ORDER BY S.AccountIsActive DESC

	RETURN @StatusExternalId

END