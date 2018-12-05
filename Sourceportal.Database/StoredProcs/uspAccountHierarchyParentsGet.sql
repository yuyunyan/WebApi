/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.11.13
   Description:	Retrieves all parent nodes from Account Hierarchies
   Usage:		EXEC uspAccountHierarchyParentsGet
   Revision History:

   ============================================= */
CREATE PROCEDURE [dbo].[uspAccountHierarchyParentsGet]
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
	FROM AccountHierarchies
	WHERE ParentID IS NULL
END