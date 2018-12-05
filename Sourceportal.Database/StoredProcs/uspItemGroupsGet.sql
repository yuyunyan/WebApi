/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.06.26
   Description:	Gets Item Groups
   Usage: EXEC uspItemGroupsGet
   Revision History:
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspItemGroupsGet]
AS
BEGIN
	SELECT
		ItemGroupID,
		GroupName,
		Code
	FROM lkpItemGroups
	WHERE IsDeleted = 0	
	ORDER BY GroupName
END
