/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.06.02
   Description:	Retrieves all Contacts for a specific account

   Revision History:
   ============================================= */
CREATE PROCEDURE [dbo].[uspAccountContactsGet]
(
	@AccountID INT
	, @ContactID INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT ContactID,
		C.AccountID,
		A.AccountName,
		FirstName,
		LastName,
		C.OfficePhone 'Phone',
		C.Email,
		Details,
		IsActive
	FROM Contacts C
	INNER JOIN Accounts A on A.AccountID = @AccountID
	WHERE IsActive = 1
	AND C.ContactID = ISNULL(@ContactID,ContactID)
END