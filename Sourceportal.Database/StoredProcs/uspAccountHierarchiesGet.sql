-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2017.12.05
-- Description:			Return list of account hierarchies

-- Revision History:
--	2018.02.09	CT	Added AccountHierarchyID parameter to filter results
-- =============================================
CREATE PROCEDURE [dbo].[uspAccountHierarchiesGet] 
(
	  @AccountHierarchyID INT = NULL
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
	WHERE IsDeleted = 0 AND AccountHierarchyID = ISNULL(@AccountHierarchyID, AccountHierarchyID)

END
