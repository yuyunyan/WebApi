/* =============================================
   Author:		Corey Tyrrell
   Create date: 2018.02.06
   Description:	updates with new SAP data
   Usage:	EXEC [[uspAccountHierarchyChildSapDataSet]] @AccountHierarchyId = 1, @ExternalId = '345', @GroupId = 'SPTest', @UserID = 1		
   Return Codes:

   Revision History:
			
   ============================================= */

CREATE PROCEDURE [dbo].[uspAccountHierarchyChildSapDataSet]
	@ParentId INT,
	@ExternalId VARCHAR(50),
	@GroupId VARCHAR(50),
	@HierarchyName VARCHAR(50),
	@UserID INT
AS
BEGIN
	SET NOCOUNT ON;
	--Update the record
	UPDATE AccountHierarchies
	SET		
		SAPHierarchyID = @ExternalId,
		SAPGroupID = @GroupId,
		ModifiedBy = @UserID,
		Modified = GETUTCDATE()
	WHERE ParentID = @ParentId AND HierarchyName = @HierarchyName
END
