/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.09.05
   Description:	Update VendorRfq
   Usage:	EXEC uspVendorRfqSet @rfqId = 4, @accountId = 3, @contactId = 4, @statusId
  
   ============================================= */

CREATE PROCEDURE [dbo].[uspVendorRfqSet]
	@rfqId INT,
	@accountId INT,
	@contactId INT,
	@statusId INT,
	@currencyId CHAR(3),
	@UserID INT,
	@organizationId INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@UserID, 0) = 0
		RETURN -3

	IF (ISNULL(@accountId, 0) = 0) OR (ISNULL(@contactId, 0) = 0)
		RETURN -4

	IF ISNULL(@statusId, 0) = 0
		RETURN -5

	IF ISNULL(@rfqId, 0) = 0
		BEGIN
			GOTO InsertLine
		END
	ELSE
		GOTO UpdateLine

InsertLine:
	
	INSERT INTO VendorRFQs (AccountID, ContactID, StatusID, CreatedBy, CurrencyID, OrganizationID, Created, SentDate)
	VALUES (@accountId, @contactId, @statusId, @UserID, @currencyId, @organizationId, GETUTCDATE(), GETUTCDATE())
	
	SET @rfqId = SCOPE_IDENTITY()

	IF (@@ROWCOUNT=0)
		RETURN -1
	GOTO ReturnSelect
		
UpdateLine:	

	UPDATE VendorRFQs 
	
	SET 
	AccountID = @accountId,
	ContactID = @contactId,
	StatusID = @statusId,
	CurrencyID = @currencyId,
	Modified =  GETUTCDATE(),
	ModifiedBy = @UserID
	
	WHERE
	VendorRFQID = @rfqId

	IF (@@rowcount = 0)
		RETURN -2
	GOTO ReturnSelect

ReturnSelect:
	SELECT @rfqId 'rfqId'
END
