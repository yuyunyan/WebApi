/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.03.01
   Description:	finds matches based on id or all
   Usage:	EXEC [uspCompanyTypesGet] @CompanyTypeID = 5

   Return Codes:

   Revision History:

   ============================================= */
CREATE PROCEDURE [dbo].[uspCompanyTypesGet]
(
	@CompanyTypeID int = NULL
	
)
AS
BEGIN 
	SET NOCOUNT ON;

	  SELECT CompanyTypeID,
	  Name,
	  ExternalID
	  FROM lkpCompanyTypes
	  WHERE CompanyTypeID = ISNULL(@CompanyTypeID, CompanyTypeID)
END