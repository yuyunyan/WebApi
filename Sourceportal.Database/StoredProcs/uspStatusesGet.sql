CREATE PROCEDURE [dbo].[uspStatusesGet]
(
	@ObjectTypeID INT
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		StatusID
		, StatusName
		, IsDefault
	FROM lkpStatuses
	WHERE ObjectTypeID = @ObjectTypeID
	AND IsDeleted = 0
END
