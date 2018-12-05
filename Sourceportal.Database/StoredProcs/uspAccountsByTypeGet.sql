CREATE PROCEDURE [dbo].[uspAccountsByTypeGet]
	@AccountID INT = NULL,
	@AccountTypeID INT = NULL,
	@IncludeInactive BIT = 0,
	@IncludeBlacklisted BIT = 0,
	@SelectedAccountID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT AccountID
	, AccountName
	FROM Accounts
	WHERE AccountID = @SelectedAccountID
	
	UNION
	SELECT	a.AccountID
	, a.AccountName
	FROM Accounts a
	INNER JOIN mapAccountTypes mat ON mat.AccountID = a.AccountID
	WHERE @AccountTypeID & mat.AccountTypeID = mat.AccountTypeID
	AND a.AccountID = ISNULL(@AccountID, a.AccountID)
	AND a.IsDeleted = 0
END