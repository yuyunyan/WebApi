-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.01.19
-- Description:			Delete the account group for given id
-- =============================================
CREATE PROCEDURE [dbo].[uspUserAccountGroupDelete] 
	@AccountGroupID INT = NULL
AS
BEGIN

	DECLARE @RowCount INT = 0;
    Update UserAccountGroups SET IsDeleted = 1 WHERE AccountGroupID = @AccountGroupID

	SET @RowCount = @@ROWCOUNT

	SELECT @RowCount 'RowCount'
END
GO
