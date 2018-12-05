/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.05.11
   Description:	Retrieves all navigation records from lkpNavigation

   Revision History:
   2017.11.15		BZ		Rename column names
   ============================================= */
CREATE PROCEDURE [dbo].[uspNavigationGet]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
	[NavID],
	[NavName],
	[ParentNavID] 
	FROM lkpNavigation
	WHERE IsDeleted = 0 AND IsNavMenu = 1
END
