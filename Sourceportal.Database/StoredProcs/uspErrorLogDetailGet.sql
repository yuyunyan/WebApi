/* =============================================
   Author:				Berry, Zhong
   Create date:			2017.10.10
   Description:			Get the error log detail
   Usage:				EXEC uspErrorLogDetailGet @ErrorID = 150
   =============================================*/
CREATE PROCEDURE [dbo].[uspErrorLogDetailGet]
	@ErrorID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@ErrorID, 0) = 0
		RETURN -1

     SELECT
		erl.ErrorID,
		[URL],
		erl.PostData,
		erl.ExceptionType,
		erl.ErrorMessage,
		erl.InnerExceptionMessage,
		erl.StackTrace,
		app.AppName 'Application',
		usr.FirstName,
		usr.LastName,
		[Timestamp]
	FROM ErrorLog erl
		LEFT OUTER JOIN Applications AS app ON app.AppID = erl.AppID
		LEFT OUTER JOIN Users AS usr ON usr.UserID = erl.UserID
	WHERE erl.ErrorID = @ErrorID
END