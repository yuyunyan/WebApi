/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.02.06
   Description:	updates with new SAP data
   Usage:	EXEC [uspAccountHierarchySapDataSet] @AccountHierarchyId = 1, @ExternalId = '345', @GroupId = 'SPTest', @UserID = 1		
   Return Codes:

   Revision History:
		2018.4.20	CT	Added Insert Hierarchy for when SAP is creating new 
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspAccountHierarchySapDataSet]
	@AccountHierarchyId INT = NULL,
	@ParentId INT = NULL,
	@RegionId INT = NULL,
	@ExternalId VARCHAR(50),
	@GroupId VARCHAR(50),
	@HierarchyName VARCHAR(50) = NULL,
	@UserID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	IF (ISNULL(@AccountHierarchyId, 0) = 0)
		GOTO InsertHierarchy
	ELSE
		GOTO UpdateHierarchy

InsertHierarchy:
	INSERT INTO AccountHierarchies(ParentID, RegionID, HierarchyName, SAPHierarchyID, SAPGroupID, CreatedBy)
	VALUES(
	@ParentId,
	@RegionId,
	@HierarchyName,
	@ExternalId,
	@GroupId,
	@UserID
	)

	SET @AccountHierarchyId = @@IDENTITY

	GOTO RETURN_SELECT 

UpdateHierarchy:
	UPDATE AccountHierarchies
	SET		
		RegionID = ISNULL(@RegionId, RegionID),
		HierarchyName = ISNULL(@HierarchyName, HierarchyName),
		SAPHierarchyID = @ExternalId,
		SAPGroupID = @GroupId,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE AccountHierarchyID = @AccountHierarchyId 

	GOTO RETURN_SELECT 


RETURN_SELECT:
	Select @AccountHierarchyId
END