/*	=============================================
	Author:			Aaron Rodecker
	Create date:	2018.10.03
	Description:	Gets record in userGridSettings from userID and gridName
	Usage:			EXEC [uspGridSettingsGet] @UserID = 76, @GridName = 'sourcing-quotes'
	Return Codes:
	
	Revision History:
	2017.10.12	AR	Added NULL SELECT on 0 rowcount (returning no symbols error)
	============================================*/
CREATE PROCEDURE [dbo].[uspGridSettingsGet]
	@UserID INT,
	@GridName VARCHAR(128) = NULL
	
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS (SELECT UserID FROM UserGridSettings WHERE UserID = @UserID AND GridName = @GridName)
	BEGIN
		SELECT GridName,
			ColumnDef,
			SortDef,
			FilterDef,
			Created,
			Modified
		FROM UserGridSettings
		WHERE UserID = @UserID
		AND GridName = @GridName
	END
	ELSE BEGIN
		SELECT NULL GridName,
		NULL ColumnDef,
		NULL SortDef,
		NULL FilterDef
	END
END