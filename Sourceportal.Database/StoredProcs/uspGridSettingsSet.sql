/*	=============================================
	Author:			Aaron Rodecker
	Create date:	2018.10.02
	Description:	Inserts or updates record in userGridSettings
	Usage:
	Return Codes:
				
	============================================*/
CREATE PROCEDURE [dbo].[uspGridSettingsSet]
	@UserID INT,
	@GridName VARCHAR(128) = NULL,
	@ColumnDef VARCHAR(MAX) = NULL,
	@SortDef VARCHAR(MAX) = NULL,
	@FilterDef VARCHAR(MAX) = NULL
	
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS (SELECT GridName FROM UserGridSettings WHERE GridName = @GridName AND UserID = @UserID)
		GOTO UpdateGrid
	ELSE
		GOTO InsertGrid

InsertGrid:
	INSERT INTO UserGridSettings (UserID, GridName, ColumnDef, SortDef, FilterDef)
	VALUES (@UserID, @GridName, @ColumnDef, @SortDef, @FilterDef)
	
	IF (@@rowcount = 0)
		RETURN -1
	RETURN 0

UpdateGrid:
	BEGIN
		UPDATE UserGridSettings
		SET ColumnDef = @ColumnDef,
		SortDef = @SortDef,
		FilterDef = @FilterDef,
		Modified = GETUTCDATE()
		WHERE GridName = @GridName
		RETURN 0
	END
	RETURN 0
END

