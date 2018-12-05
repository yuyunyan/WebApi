/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.04.25
   Description:	Retrieves user record from tblUsers with passwordhash
   Revision:
		2017.04.27	AR	Changed login method to use email address instead of UN
		2017.05.09	AR	Depreciated Username
   Return Codes:
   -1		Invalid EmailAddress
   -2		Invalid Password
   -3		User disabled
   ============================================= */
CREATE PROCEDURE [dbo].[uspUserLogin]
(
	@EmailAddress VARCHAR(64),
	@PasswordHash VARCHAR(256)
)
AS
BEGIN
	SET NOCOUNT ON;
	UPDATE Users
		SET LastLogin = GETDATE()
		OUTPUT INSERTED.UserID, INSERTED.EmailAddress, INSERTED.PasswordHash, INSERTED.FirstName, INSERTED.LastName, INSERTED.OrganizationID, INSERTED.isEnabled, INSERTED.LastLogin, INSERTED.Created, INSERTED.CreatedBy, INSERTED.Modified, INSERTED.ModifiedBy
		WHERE EmailAddress = @EmailAddress
		--WHERE ((Username = @Username) OR (EmailAddress = @UserName))
		AND PasswordHash = @PasswordHash
		AND isEnabled = 1

	IF (@@rowcount=0)
	BEGIN
		IF NOT EXISTS (SELECT UserID FROM Users WHERE EmailAddress = @EmailAddress)
		RETURN -1	--Invalid EmailAddress

		IF NOT EXISTS (SELECT UserID FROM Users WHERE EmailAddress = @EmailAddress AND PasswordHash = @PasswordHash)
		RETURN -2	--Invalid password

		IF NOT EXISTS (SELECT UserID FROM Users WHERE EmailAddress = @EmailAddress AND isEnabled = 1)
		RETURN -3	--User Disabled

	END
END