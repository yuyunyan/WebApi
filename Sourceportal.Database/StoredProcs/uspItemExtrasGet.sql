CREATE PROCEDURE [dbo].[uspItemExtrasGet]

AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT ItemExtraID, ExtraName, ExtraDescription
	FROM ItemExtras
	WHERE IsDeleted = 0
	ORDER BY ExtraName
END
