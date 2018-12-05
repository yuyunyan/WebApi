-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.01.19
-- Description:			Return the account group name and lines
-- =============================================
CREATE PROCEDURE [dbo].[uspAccountGroupDetailGet]
	@AccountGroupID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		aga.*, 
		ag.GroupName,
		ag.UserID,
		a.AccountName, 
		--a.AccountStatusId,
		c.FirstName + ' ' + c.LastName 'ContactName',
		--(S.Name) AccountStatus,
		dbo.fnGetAccountTypeObjects(a.AccountID) 'AccountTypeIds' 
	FROM mapUserAccountGroupAccounts aga
		INNER JOIN UserAccountGroups ag ON aga.AccountGroupID = ag.AccountGroupID AND ag.AccountGroupID = @AccountGroupID
		INNER JOIN Accounts a ON a.AccountID = aga.AccountID AND a.IsDeleted = 0
		LEFT JOIN Contacts c ON c.ContactID = aga.ContactID
		--INNER JOIN lkpAccountStatuses S ON S.AccountStatusID = A.AccountStatusID
	WHERE aga.IsDeleted = 0
	ORDER BY aga.GroupLineID
END
GO
