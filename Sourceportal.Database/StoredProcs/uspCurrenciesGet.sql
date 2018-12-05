/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.09
   Description:	Gets a list of currencies
   Usage: EXEC [uspCurrenciesGet]
   Revision History:
       11.02.17  CT  Added ExternalID	
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspCurrenciesGet]
	@CurrencyID VARCHAR(6) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
	CurrencyID,
	CurrencyName,
	ExternalID
	FROM lkpCurrencies
	WHERE CurrencyID = ISNULL(NULLIF(@CurrencyID,''),CurrencyID)
END