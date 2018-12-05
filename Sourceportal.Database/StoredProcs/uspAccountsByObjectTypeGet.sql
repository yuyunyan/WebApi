CREATE PROCEDURE [dbo].[uspAccountsByObjectTypeGet]
	@ObjectTypeID INT = NULL,
	@IncludeInactive BIT = 0,
	@IncludeBlacklisted BIT = 0,
	@SelectedAccountID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT A.AccountID
		, A.AccountName
		, mat.Rating SupplierRating
	FROM Accounts A
	INNER JOIN mapAccountTypes mat on mat.AccountID = A.AccountID
	WHERE A.AccountID = @SelectedAccountID
	UNION
	SELECT	a.AccountID
		, a.AccountName
		, mat.Rating SupplierRating
	FROM Accounts a
	INNER JOIN mapAccountTypes mat on mat.AccountID = a.AccountID
	LEFT OUTER JOIN mapAccountObjectTypes ab on mat.AccountTypeID &  ab.AccountTypeID = mat.AccountTypeID
    INNER JOIN lkpAccountStatuses accS ON accS.AccountStatusID = mat.AccountStatusID
	WHERE  ISNULL(@ObjectTypeID,ab.ObjectTypeID)  = ab.ObjectTypeID
	AND a.IsDeleted = 0
	AND mat.IsDeleted = 0
	AND accS.AccountIsActive = 1
	AND accS.AccountIsBlacklisted = 0

END