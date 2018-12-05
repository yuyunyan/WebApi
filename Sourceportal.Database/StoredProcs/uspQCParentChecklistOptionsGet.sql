/* =============================================
   Author:		Julia Thomas
   Create date: 2017.08.22
   Description:	Gets parent checkList options 
   Usage: EXEC uspQCParentChecklistOptionsGet

   Revision History:
			
   Return Codes:

   ============================================= */

CREATE PROCEDURE [dbo].[uspQCParentChecklistOptionsGet]
   
AS
BEGIN
	SELECT 
			qcc.ChecklistName,
			qcc.ChecklistID
	FROM QCChecklists qcc
	WHERE qcc.IsDeleted =0 AND ISNULL(ParentChecklistID,0) = 0
END