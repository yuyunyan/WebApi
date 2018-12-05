-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2017.12.07
-- Description:			Create parent company and hierarchies, return the accountHierarchyId = @RegionID
-- =============================================
CREATE PROCEDURE [dbo].[uspAccountHierarchySet] 
	@RegionID INT = NULL,
	@HierarchyName VARCHAR(250) = NULL,
	@UserID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @ParentID INT;

	INSERT INTO AccountHierarchies (ParentID, RegionID, HierarchyName,
	 CreatedBy, IsDeleted)
	VALUES (NULL, 0, @HierarchyName, @UserID, 0)

	SET @ParentID = SCOPE_IDENTITY();

	INSERT INTO AccountHierarchies (ParentID, RegionID, HierarchyName,
	 CreatedBy, IsDeleted)
	SELECT
		@ParentID,
		R.RegionID,
		R.RegionName,
		@UserID,
		0
	FROM lkpRegions R
	WHERE R.IsDeleted = 0

	SELECT
		AccountHierarchyID
	FROM AccountHierarchies
	WHERE ParentID = @ParentID AND RegionID = @RegionID


END
