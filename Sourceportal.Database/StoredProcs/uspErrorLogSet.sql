/* =============================================
	Author:				Berry, Zhong
	Create date:		2017.09.29
	Description:		Insert Exception log data into ErrorLog
	Usage:				EXEC uspErrorLogSet @AppID = 2, @URL = 'http://dev.api.sourceportal.com/api/boms/getEMSs?searchId=598',
						@UserID =68, @TimeStamp = '9/29/2017 13:01:20 '
	Return Code:		
						-1 AppID can not be null
   =============================================*/
CREATE PROCEDURE [dbo].[uspErrorLogSet]
	@AppID INT = NULL,
	@URL VARCHAR(MAX) = NULL,
	@PostData VARCHAR(MAX) = NULL,
	@ExceptionType VARCHAR(100) = NULL,
	@ErrorMessage VARCHAR(MAX) = NULL,
	@InnerExceptionMessage VARCHAR(MAX) = NULL,
	@StackTrace VARCHAR(MAX) = NULL,
	@UserID INT = NULL,
	@TimeStamp DateTime = NULL
AS
BEGIN
	IF @AppID IS NULL
		RETURN -1

	IF @TimeStamp IS NULL
		SET @TimeStamp = GETUTCDATE()
	
	INSERT INTO ErrorLog (AppID, [URL], PostData, ExceptionType, ErrorMessage, InnerExceptionMessage, StackTrace, UserID, [TimeStamp])
	VALUES (@AppID, @URL, @PostData, @ExceptionType, @ErrorMessage, @InnerExceptionMessage, @StackTrace, @UserID, @TimeStamp)

	SELECT SCOPE_IDENTITY();
END
