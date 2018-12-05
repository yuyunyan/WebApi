/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.30
   Description:	Returns a list of packaging types
   Usage: EXEC [uspPackagingGet]
   Revision History:
       2018.01.24	ML	Added PackagingId param to get 
   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspPackagingGet]

@PackagingID int = 0

AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT PackagingID, PackagingName, ExternalID
	FROM codes.lkpPackaging
	WHERE IsDeleted = 0 AND PackagingID = ISNULL(NULLIF(@PackagingID, 0), PackagingID)
	ORDER BY PackagingName
END
