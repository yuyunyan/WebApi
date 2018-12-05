-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.01.25
-- Description:			Return suppliers in Account Group that is active is not blacklisted
-- =============================================
CREATE PROCEDURE [dbo].[uspSuppliersAccountGroupMatch]
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
		c.FirstName,
		c.LastName,
		c.OfficePhone,
		c.Email,
		--(S.Name) AccountStatus,
		dbo.fnGetAccountTypeObjects(a.AccountID) 'AccountTypeIds' 
	FROM mapUserAccountGroupAccounts aga
		INNER JOIN UserAccountGroups ag ON aga.AccountGroupID = ag.AccountGroupID AND ag.AccountGroupID = @AccountGroupID
		INNER JOIN Accounts a ON a.AccountID = aga.AccountID AND a.IsDeleted = 0
		LEFT JOIN Contacts c ON c.ContactID = aga.ContactID
		INNER JOIN mapAccountTypes mat ON mat.AccountID = a.AccountID AND mat.AccountTypeID = 1 AND mat.IsDeleted = 0
		INNER JOIN lkpAccountStatuses las ON las.AccountStatusID = mat.AccountStatusID AND las.AccountIsActive = 1 AND ISNULL(las.AccountIsBlacklisted, 0) = 0
		--INNER JOIN lkpAccountStatuses S ON S.AccountStatusID = A.AccountStatusID
	WHERE aga.IsDeleted = 0
	ORDER BY aga.GroupLineID
END
GO
