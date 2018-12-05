/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.10.29
   Description:	Returns Inspection Types as Checklist Types
   Usage:		EXEC uspQCChecklistTypeGet
   Revision History:
		2018.10.29	NA	Converted from the lkpQCChecklistTypes table to lkpQCInspectionTypes

   ============================================= */

CREATE OR ALTER PROCEDURE [dbo].[uspQCChecklistTypeGet]
AS
BEGIN
	SELECT 	InspectionTypeID AS 'ChecklistTypeId',
			TypeName AS 'ChecklistTypeName',
			ExternalID AS 'ExternalID'
	FROM lkpQCInspectionTypes it
	WHERE  it.IsDeleted = 0
END