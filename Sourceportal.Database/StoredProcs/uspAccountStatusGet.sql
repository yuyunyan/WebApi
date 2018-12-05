/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.02.08
   Description:	Gets Status of Accounts from lkpAccountStatus and mapAccountTypes tbl using AccountID to narrow
   Usage: EXEC uspAccountStatusGet @AccountID = 1
          
   Revision History:

   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspAccountStatusGet]
(
	@AccountID INT
)
AS
BEGIN
	SELECT  
		S.ExternalID 'ExternalId'
		, S.AccountIsActive
		, S.Name 'StatusName'
		, S.AccountStatusID 'AccountStatusId'
	FROM mapAccountTypes mat
	INNER JOIN lkpAccountStatuses S on mat.AccountStatusID = S.AccountStatusID
	WHERE mat.AccountID = @AccountID AND mat.IsDeleted = 0
	ORDER BY S.AccountIsActive DESC
END