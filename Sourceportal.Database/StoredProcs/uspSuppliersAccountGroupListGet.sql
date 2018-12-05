-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.01.25
-- Description:			Return list of AccountGroup that contain supplier with valid status 
-- =============================================
CREATE PROCEDURE [dbo].[uspSuppliersAccountGroupListGet]
	@UserID INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT 
		distinct ag.AccountGroupID,
		ag.GroupName
	FROM mapUserAccountGroupAccounts aga
		INNER JOIN UserAccountGroups ag ON aga.AccountGroupID = ag.AccountGroupID AND ag.UserID = @UserID AND ag.IsDeleted = 0
		INNER JOIN Accounts a ON a.AccountID = aga.AccountID AND a.IsDeleted = 0
		INNER JOIN mapAccountTypes mat ON mat.AccountID = a.AccountID AND mat.AccountTypeID = 1 AND mat.IsDeleted = 0
		INNER JOIN lkpAccountStatuses las ON las.AccountStatusID = mat.AccountStatusID AND las.AccountIsActive = 1 AND ISNULL(las.AccountIsBlacklisted, 0) = 0
		--INNER JOIN lkpAccountStatuses S ON S.AccountStatusID = A.AccountStatusID
	WHERE aga.IsDeleted = 0
	ORDER BY ag.AccountGroupID
END