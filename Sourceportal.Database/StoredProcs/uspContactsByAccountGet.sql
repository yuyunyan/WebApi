CREATE PROCEDURE [dbo].[uspContactsByAccountGet]
	@AccountID INT = NULL,
	@IncludeInactive BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
	
	IF ISNULL(@AccountID, 0) = 0
		RETURN - 1

	SELECT ContactID
			, FirstName + ' ' + LastName 'ContactName'
	FROM Contacts
	WHERE ISNULL(IsActive, 1) IN(1, @IncludeInactive - 1)
	  AND AccountID = @AccountID
	  --AND IsDeleted = 0
END
