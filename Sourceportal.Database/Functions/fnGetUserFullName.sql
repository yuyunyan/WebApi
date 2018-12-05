CREATE FUNCTION [dbo].[fnGetUserFullName]
(
	@UserID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
    RETURN (SELECT FirstName + ' ' + LastName
		  FROM Users
		  WHERE UserID = @UserID )
END