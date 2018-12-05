/* =============================================
	Author:			Aaron Rodecker
	Create date:	2018.05.18
	Description:	Return the first + ' ' + last name of a userID
	Usage:			SELECT dbo.[fnGetUserFirstlastName](3)
   =============================================*/
CREATE FUNCTION [dbo].[fnGetUserFirstlastName]
(
	@UserID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
	RETURN (
		SELECT FirstName + ' ' + LastName From Users
		WHERE UserID = @UserID
	)
END
