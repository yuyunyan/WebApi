/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.11.13
   Description:	Retrieves all child nodes from Account Hierarchies for given Parent ID
   Usage:		EXEC uspAccountHierarchyChildrenGet @AccountHierarchyID = 1 
   Revision History:

   ============================================= */
CREATE PROCEDURE [dbo].[uspAccountHierarchyChildrenGet]
(
	@AccountHierarchyID INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		AccountHierarchyID
		, HierarchyName
		, SAPHierarchyID
		, SAPGroupID
		, RegionID
	FROM AccountHierarchies
	WHERE ParentID = @AccountHierarchyID
END