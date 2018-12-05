/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.11.06
   Description:	Inserts mail sent record in to logEmailsSent or updates existing one
   Usage:		EXEC uspLogMailSentSet @InspectionID = 114, @StockID = 551
   Revision History:
	
   Return Codes:
		-1	Update failed, invalid LogID
   ============================================= */

CREATE PROCEDURE [dbo].[uspLogMailSentSet]
(
	@LogID INT = NULL,
	@UserID INT = NULL,
	@EmailFrom VARCHAR(256) = NULL,
	@FromName VARCHAR(256) = NULL,
	@EmailTo VARCHAR(256) = NULL,
	@CC VARCHAR(1024) = NULL,
	@BCC VARCHAR(1024) = NULL,
	@MailSubject VARCHAR(128) = NULL,
	@MailBody VARCHAR(MAX) = NULL,
	@AttachmentFilePath VARCHAR(MAX) = NULL,
	@Success BIT  = NULL,
	@ErrorMessage VARCHAR(MAX) = NULL,
	@StatusCode INT = NULL
)
AS
BEGIN
	IF (ISNULL(@LogID,0) != 0)
		GOTO UpdateLog

InsertLog:
	INSERT INTO dbo.logEmailsSent(EmailFrom, FromName, EmailTo, CC, BCC, MailSubject, MailBody, AttachmentFilePath, CreatedBy)
	VALUES (@EmailFrom, @FromName, @EmailTo, @CC, @BCC, @MailSubject, @MailBody, @AttachmentFilePath, @UserID)
	SET @LogID = @@IDENTITY
	RETURN @LogID

UpdateLog:
	UPDATE dbo.logEmailsSent
	SET EmailFrom = ISNULL(@EmailFrom, EmailFrom)
	 , FromName = ISNULL(@FromName, FromName)
	 , EmailTo = ISNULL(@EmailTo, EmailTo)
	 , CC = ISNULL(@CC, CC)
	 , BCC = ISNULL(@BCC, BCC)
	 , MailSubject = ISNULL(@MailSubject, MailSubject)
	 , MailBody = ISNULL(@MailBody, MailBody)
	 , AttachmentFilePath = ISNULL(@AttachmentFilePath, AttachmentFilePath)
	 , Success = ISNULL(@Success, Success)
	 , ErrorMessage = ISNULL(@ErrorMessage, ErrorMessage)
	 , StatusCode = ISNULL(@StatusCode, StatusCode)

	 IF (@@ROWCOUNT > 0)
		RETURN @LogID
	ELSE
		RETURN -1
END