/* =============================================
   Author:			Berry, Zhong
   Create date:		2017.09.11
   Description:		Return XlsDataMaps for @AccountID and @XlsType
   Usage:			EXEC XlsAccountGet @AccountID = 5, @XlsType = 'ItemListLines'
   Return Code:
					-1 @AccountID is missing
					-2 @XlsType is missing
   =============================================*/
CREATE PROCEDURE [dbo].[uspXlsAccountGet] 
	@AccountID INT = NULL,
	@XlsType nvarchar(25) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@AccountID, 0) = 0
		RETURN -1

	IF ISNULL(@XlsType, '') = ''
		RETURN -2

	SELECT
		mxa.XlsDataMapID 'XlsDataMapID',
		mxa.ColumnIndex 'ColumnIndex'
	FROM mapXlsAccount mxa
		LEFT OUTER JOIN lkpXlsDataMap xdm ON xdm.XlsDataMapID = mxa.XlsDataMapID
	WHERE mxa.AccountID = @AccountID AND xdm.XlsType = @XlsType

END
