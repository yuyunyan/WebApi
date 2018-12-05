-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2017.12.04
-- Description:			Return insert or delete line in mapContactFocus
-- =============================================
CREATE PROCEDURE [dbo].[uspContactFocusesSet]
	@ContactID INT = NULL,
	@FocusID INT = NULL,
	@IsDeleted BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	DELETE mapContactFocuses
	WHERE ContactID = @ContactID 
	AND FocusID = @FocusID
	
	INSERT INTO mapContactFocuses(ContactID, FocusID, IsDeleted)
	VALUES (@ContactID, @FocusID, @IsDeleted)

	SELECT @@ROWCOUNT 'RowCount'

END
GO
