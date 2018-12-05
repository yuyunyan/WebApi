CREATE PROCEDURE [dbo].[uspAccountTypesGet]

AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		AccountTypeID 'Id',
		[Name] 'Name'
	FROM lkpAccountTypes
	ORDER BY [Name]
END