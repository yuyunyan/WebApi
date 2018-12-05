
/* =============================================
   Author:		Julia Thomas
   Create date: 2018.06.28
   Description:	Retrieves all qcResult from lkpQCResults or per ResultID
   Usage:		EXEC uspQCResultsGet @ResultID=1
   Revision History:

   
   ============================================= */
CREATE PROCEDURE [dbo].[uspQCResultsGet]
(
	@ResultID INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		ResultID,
		ResultName
	FROM lkpQCResults
	WHERE IsDeleted = 0 
	AND ISNULL(@ResultID,ResultID)= ResultID
END