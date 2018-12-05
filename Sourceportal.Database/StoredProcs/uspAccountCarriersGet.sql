
/* =============================================
   Author:				Julia Thomas
   Create date:			2018.05.22
   Description:			Gets the list of Carries
   Usage:				EXEC [dbo].[uspAccountCarriersGet]	
   =============================================*/
 CREATE PROCEDURE [dbo].[uspAccountCarriersGet]
@AccountID INT = NULL
	
AS
BEGIN
	SET NOCOUNT ON;
		
	SELECT
		a.AccountID,
		a.AccountName,
		c.CarrierName,
		c.CarrierID,
		ac.IsDefault,
		ac.AccountNumber
	FROM mapAccountCarriers ac
	LEFT OUTER JOIN Accounts a ON ac.AccountID = a.AccountID
	LEFT OUTER JOIN Carriers c on ac.CarrierID = c.CarrierID
	-- If have no accountID Passed then return all
	WHERE ac.IsDeleted=0 AND ac.AccountID = ISNULL(@AccountID,ac.AccountID)
	
END