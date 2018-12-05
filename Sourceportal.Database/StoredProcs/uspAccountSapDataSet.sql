/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.02.06
   Description:	updates a line item on a Sales Order with new SAP data
   Usage:	EXEC uspAccountSapDataSet @AccountID = 1, @ExternalId = '345', @UserID = 1		
   Return Codes:

   Revision History:
			2018.02.23  CT  Added AccountNum being set to same as external id
			2018.02.26  CT	Added currency and paymentterm updates
   ============================================= */

CREATE PROCEDURE [dbo].[uspAccountSapDataSet]
	@AccountID INT,
	@ExternalId VARCHAR(50),
	@CustomerCurrencyID VARCHAR(3) = NULL,
	@CustomerPaymentTermID INT = NULL,
	@CustomerCreditLimit INT = NULL,
	@SupplierCurrencyID VARCHAR(3) = NULL,
	@SupplierPaymentTermID INT = NULL,
	@Status INT = NULL,
	@UserID INT
AS
BEGIN
	SET NOCOUNT ON;
	--Update the record
	UPDATE Accounts
	SET		
		ExternalId = @ExternalId,
		AccountNum = @ExternalId,
		CurrencyID = ISNULL(@CustomerCurrencyID, ISNULL(@SupplierCurrencyID, CurrencyID)),
		CreditLimit = ISNULL(@CustomerCreditLimit, CreditLimit),
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE AccountID = @AccountID 

	UPDATE mapAccountTypes
	SET
		PaymentTermID = ISNULL(@CustomerPaymentTermID, PaymentTermID),
		AccountStatusID = ISNULL(@Status, AccountStatusID)
	WHERE AccountID = @AccountID AND AccountTypeID = 4

	UPDATE mapAccountTypes
	SET
		PaymentTermID = ISNULL(@SupplierPaymentTermID, PaymentTermID),
		AccountStatusID = ISNULL(@Status, AccountStatusID)
	WHERE AccountID = @AccountID AND AccountTypeID = 1
END
