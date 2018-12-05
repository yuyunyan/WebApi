CREATE PROCEDURE [dbo].[uspLocationsByAccountGet]
	@AccountID INT = NULL,
	@ExcludeBilling BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@AccountID, 0) = 0
	RETURN - 1

	SELECT LocationID, [Name] + '  (' + City + ')' 'LocationName'
	FROM Locations
	WHERE AccountID = @AccountID
	  AND IsDeleted = 0
	  AND (
			(@ExcludeBilling = 0)
			OR
			(@ExcludeBilling = 1 AND LocationTypeID <> 1)
		  )
END




