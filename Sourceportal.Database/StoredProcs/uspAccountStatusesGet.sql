CREATE PROCEDURE [dbo].[uspAccountStatusesAllGet]

AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		AccountStatusID,
		[Name] 'StatusName',
		ExternalID
	FROM lkpAccountStatuses
	WHERE IsDeleted = 0
	ORDER BY [Name]
END 