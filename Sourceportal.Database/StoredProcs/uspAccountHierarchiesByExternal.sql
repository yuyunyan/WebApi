-- =============================================
-- Author:				Corey Tyrrell
-- Create date:			2018.4.20
-- Description:			Returns a hierarchy

-- Revision History:

-- =============================================
CREATE PROCEDURE [dbo].[uspAccountHierarchiesGetByExternal] 
(
	  @SAPHierarchyID VARCHAR(50)
)	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		AccountHierarchyID,
		ParentID, 
		RegionID,
		HierarchyName,
		SAPHierarchyID,
		SAPGroupID
	FROM AccountHierarchies
	WHERE IsDeleted = 0 AND SAPHierarchyID = @SAPHierarchyID

END