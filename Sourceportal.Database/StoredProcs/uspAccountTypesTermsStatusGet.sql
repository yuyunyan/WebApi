
-- =============================================
-- Author:				Corey Tyrrell
-- Create date:			2018.01.17
-- Description:			Return list of account types
-- Usage				exec uspAccountTypesTermsStatusGet 29
-- Revision History: 
      -- 2017.07.13  Julia Thomas  EPDSID added
-- =============================================
CREATE PROCEDURE [dbo].[uspAccountTypesTermsStatusGet]
(
	@AccountID INT
)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
	   mat.AccountTypeID,
	   mat.AccountStatusID,
	   S.Name 'StatusName',	
	   mat.PaymentTermID   ,
	   P.TermName 'PaymentTermName',
	   mat.EPDSID
	FROM mapAccountTypes mat
	INNER JOIN lkpAccountStatuses S ON S.AccountStatusID = mat.AccountStatusID
	LEFT JOIN codes.lkpPaymentTerms P ON P.PaymentTermID = mat.PaymentTermID
	WHERE mat.AccountID = @AccountID AND mat.IsDeleted = 0
END