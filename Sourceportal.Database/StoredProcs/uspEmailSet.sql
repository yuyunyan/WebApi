/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.09.04
   Description:	Creates or updates an Email message
   Usage:	EXEC uspEmailSet @EmailTypeID = 1, @FromAddress = 'Notifications@Quotely.com', @ToAddresses = 'Nathan.Ayers@Sourceability.com', @Body = 'This is a test', @UserID = 0

   Return Codes:
		-1	FromAddress is required
		-2	ToAddress is required
		-3	Body is required
   Revision History:

   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspEmailSet]
	@EmailID INT = NULL,
	@EmailTypeID INT = NULL,
	@FromAddress VARCHAR(100) = NULL,
	@ToAddresses VARCHAR(MAX) = NULL,
	@CCAddresses VARCHAR(MAX) = NULL,
	@BCCAddresses VARCHAR(MAX) = NULL,
	@Body VARCHAR(MAX) = NULL,
	@SentStatus VARCHAR(50) = NULL,
	@ErrorMessage VARCHAR(MAX) = NULL,
	@UserID INT = NULL,
	@IsDeleted BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@FromAddress, '') = ''
		RETURN -1
	IF ISNULL(@ToAddresses, '') = ''
		RETURN -2
	IF ISNULL(@Body, '') = ''
		RETURN -3

	IF ISNULL(@EmailID, 0) = 0
		GOTO InsertEmail
	ELSE
		GOTO UpdateEmail


InsertEmail:
	INSERT INTO Emails (EmailTypeID, FromAddress, ToAddresses, CCAddresses, BCCAddresses, Body, SentStatus, CreatedBy)
	VALUES (@EmailTypeID, @FromAddress, @ToAddresses, @CCAddresses, @BCCAddresses, @Body, ISNULL(@SentStatus, 'Pending'), @UserID)

	SET @EmailID = SCOPE_IDENTITY()

	GOTO ReturnSelect

UpdateEmail:
	UPDATE Emails SET
		EmailTypeID = ISNULL(@EmailtypeID, EmailTypeID),
		FromAddress = ISNULL(@FromAddress, FromAddress),
		ToAddresses = ISNULL(@ToAddresses, ToAddresses),
		CCAddresses = ISNULL(@CCAddresses, CCAddresses),
		BCCAddresses = ISNULL(@BCCAddresses, BCCAddresses),
		Body = ISNULL(@Body, Body),
		SentStatus = ISNULL(@SentStatus, SentStatus),
		ErrorMessage = ISNULL(@ErrorMessage, ErrorMessage),
		IsDeleted = ISNULL(@IsDeleted, IsDeleted),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	FROM Emails
	WHERE EmailID = @EmailID

	GOTO ReturnSelect

ReturnSelect:
	SELECT @EmailID 'EmailID'
END


