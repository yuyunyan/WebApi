/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.04.27
   Description:	Inserts or updates user records
   Usage: EXEC uspUserSet @UserID = 1, @Username = 'AARONR, @ModifiedBy = 1

   Revision History:
		2017.05.04	AR	Added dup username/email check
		2017.05.09	AR	Depreciated Username
		2017.06.05	AR	Moved dup email check to Insert and update seperately
   Return Codes:
    0		Success
   -1		Insert Failed
   -2		Update Failed - Invalid UserID
   -3		EmailAddress already used
   -4		Invalid EmailAddress
   ============================================= */
CREATE PROCEDURE [dbo].[uspUserSet]
(
	@UserID INT = NULL OUTPUT,
	@FirstName VARCHAR(32) = NULL,
	@LastName VARCHAR(32) = NULL,
	@PhoneNumber VARCHAR(32) = NULL,
	@PasswordHash VARCHAR(256) = NULL,
	@EmailAddress VARCHAR(128) = NULL,
	@OrganizationID INT = NULL,
	@isEnabled BIT = NULL,
	@ModifiedBy INT
)
AS
BEGIN 
	SET NOCOUNT ON;
	IF (@EmailAddress NOT LIKE '%@%')
		RETURN -4
	
	IF (RIGHT(@EmailAddress,4) NOT IN ('.com','.net','.org','.biz'))
		RETURN -4

	IF ISNULL(@UserID,0) = 0
		GOTO InsertUser
	ELSE
		GOTO UpdateUser

InsertUser:
BEGIN
	IF EXISTS(
		SELECT UserID
			FROM Users
			WHERE EmailAddress = ISNULL(@EmailAddress,'-123'))
		RETURN -3

	INSERT INTO Users (
		FirstName,
		LastName,
		PhoneNumber,
		PasswordHash,
		EmailAddress,
		OrganizationID,
		isEnabled,
		Created,
		CreatedBy )
	VALUES (
		@FirstName,
		@LastName,
		@PhoneNumber,
		@PasswordHash,
		@EmailAddress,
		@OrganizationID,
		ISNULL(@isEnabled,1),
		GETUTCDATE(),
		@ModifiedBy )
		
		SET @UserID = @@IDENTITY
		IF ISNULL(@UserID,-1) = -1
			RETURN -1
		GOTO OutputSelect
END
UpdateUser:
BEGIN
	IF EXISTS(
		SELECT UserID
			FROM Users
			WHERE EmailAddress = ISNULL(@EmailAddress,'-123')
			AND UserID != @UserID)
		RETURN -3

	UPDATE Users SET
		Firstname = ISNULL(@FirstName, FirstName),
		LastName = ISNULL(@LastName, LastName),
		PhoneNumber = ISNULL(@PhoneNumber, PhoneNumber),
		PasswordHash = ISNULL(@PasswordHash, PasswordHash),
		EmailAddress = ISNULL(@EmailAddress, EmailAddress),
		OrganizationID = ISNULL(@OrganizationID, OrganizationID),
		isEnabled = ISNULL(@isEnabled,isEnabled),
		Modified = GETUTCDATE(),
		ModifiedBy = @ModifiedBy
		WHERE UserID = @UserID
		
		IF (@@ROWCOUNT = 0)
			RETURN -2
		ELSE
			GOTO OutputSelect
END

OutputSelect:
BEGIN
	SELECT
		UserID, 
		EmailAddress, 
		PasswordHash, 
		FirstName, 
		LastName, 
		PhoneNumber,
		OrganizationID, 
		isEnabled, 
		Created, 
		LastLogin, 
		CreatedBy, 
		Modified, 
		ModifiedBy
		FROM Users
		WHERE UserID = @UserID
END

END