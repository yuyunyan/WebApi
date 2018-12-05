/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2017.08.22
   Description:	Retrieves the checklists for an inspection
   Usage:		EXEC uspQCInspectionCheckListsGet @InspectionID=8
   Return Codes:
				
   Revision History: 
   2018.6.12  Julia Thomas: ISNULL(@InspectionID,CHKL.InspectionID),SELECT DISTINCT Added
   2018.6.18  Julia Thomas: Select CHKL.AddedByUser Added


  
   ============================================= */

CREATE PROCEDURE [dbo].[uspQCInspectionCheckListsGet]
	@InspectionID INT = NULL
AS
BEGIN
	SELECT Distinct CHKL.ChecklistID, CHKL.CheckListName, CHKL.AddedByUser
		
	 FROM vwQCCheckLists CHKL
	
	WHERE CHKL.InspectionID = ISNULL(@InspectionID,CHKL.InspectionID) 

	IF (@@rowcount = 0)
		RETURN -1
END
