CREATE PROCEDURE [dbo].[uspNavigationsGet]
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		NavID,
		ParentNavID,
		Interface,
		NavName,
		Icon,
		SortOrder
	FROM lkpNavigation
	WHERE IsNavMenu = 1 AND IsDeleted = 0
	ORDER BY SortOrder
END
GO
