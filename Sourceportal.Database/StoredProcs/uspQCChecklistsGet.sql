/* =============================================
   Author:		Julia Thomas
   Create date: 2017.08.17
   Description:	Gets the QCChecklists
   Usage: EXEC uspQCChecklistsGet

   Revision History:
		2018.10.29	NA	Converted to QCInspectionTypes
   Return Codes:

   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspQCChecklistsGet]
    @ChecklistID INT = NULL
AS
BEGIN
	SELECT 
			qcc.ChecklistID,
			qcc.ParentChecklistID,
			qcc.ChecklistName,
			qcc.ChecklistDescription,
			qcc.ChecklistTypeID,
			qcc.SortOrder,
			qcc.EffectiveStartDate,
			qcc.IsDeleted,
			it.TypeName AS 'ChecklistTypeName'
			, COUNT(*) OVER() AS 'TotalRows'
	FROM QCChecklists qcc
	  INNER JOIN lkpQCInspectionTypes it ON qcc.ChecklistTypeID = it.InspectionTypeID
	WHERE qcc.IsDeleted  = 0 AND qcc.ChecklistID = ISNULL(@ChecklistID,ChecklistID)
END