-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.01.19
-- Description:			Return list of AccountGroups for a user
-- =============================================
CREATE PROCEDURE [dbo].[uspUserAccountGroupListGet]
	@UserID INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT
		AccountGroupID,
		GroupName
	FROM UserAccountGroups ag
	WHERE ag.UserID = @UserID AND ag.IsDeleted = 0
	ORDER BY ag.Created ASC
END
GO