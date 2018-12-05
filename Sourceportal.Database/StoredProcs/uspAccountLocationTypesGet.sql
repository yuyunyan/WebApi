CREATE PROCEDURE [dbo].[uspAccountLocationTypesGet]

AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		LocationTypeID,
		[Name],
		ExternalID
	FROM lkpLocationTypes	
END
GO