/* =============================================
   Modified:     Adrián Castán
   Create date: 2018.11.01
   Description:	Added a filter to return only active accounts
*/

CREATE PROCEDURE [dbo].[uspAccountSearchByNameNumGet]
(
	@IsDeleted BIT = 0
	,@SearchString NVARCHAR(32) = ''
	,@AccountTypeID INT = 0
	,@ObjectTypeID INT = NULL
	,@SelectedAccountID INT = NULL
	
)
AS
BEGIN 
	SET NOCOUNT ON;

	SELECT  A.AccountID
		, AccountName 
		, AccountNum
		, dbo.fnGetAccountTypeObjects(a.AccountID) AccountTypeIds
		, A.ExternalID
	FROM Accounts A
	WHERE AccountID = @SelectedAccountID

	UNION

	  SELECT DISTINCT TOP 10
	     A.AccountID
		, AccountName 
		, AccountNum
		, dbo.fnGetAccountTypeObjects(a.AccountID) AccountTypeIds
		, A.ExternalID
		FROM Accounts A
		LEFT OUTER JOIN mapAccountTypes mat ON mat.AccountID = a.AccountID
		LEFT OUTER JOIN mapAccountObjectTypes ab on ab.AccountTypeID & mat.AccountTypeID = mat.AccountTypeID
		LEFT OUTER JOIN lkpAccountStatuses acs on acs.AccountStatusID = mat.AccountStatusID-- AND acs.IsDeleted = 0
		WHERE A.IsDeleted = ISNULL(@IsDeleted, A.IsDeleted) AND mat.IsDeleted=0
		AND ISNULL(@ObjectTypeID,ab.ObjectTypeID)  = ab.ObjectTypeID
		AND (ISNULL(A.AccountName,'') + ISNULL(A.AccountNum,'') LIKE '%' + ISNULL(@SearchString,'')+ '%')
		AND ((@AccountTypeID = 0) OR (@AccountTypeID & mat.AccountTypeID = mat.AccountTypeID)) 
		AND acs.AccountIsActive = 1 AND ISNULL(acs.AccountIsBlacklisted, 0) = 0

END