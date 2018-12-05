/* =============================================
   Author:		Manuka Leelaratne
   Create date: 2018.05.01
   Description:	Update Inspection (Only updates as creation is entirely different from updating)
   Usage:		EXEC uspQCConclusionSet 7, 'intro', 'results', 'conclusion'
   Revision History:
	2018.08.01	NA	Fixed InspectionStatusID bug and added Modified Date
*/

CREATE OR ALTER PROCEDURE [dbo].[uspQCInspectionUpdate]

	@UserID INT = 0,
	@ExternalID VARCHAR(50),
	@InspectionQty INT = 0,
	@InspectionStatusID INT = NULL

AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE QCInspections
	SET 
	InspectionQty = @InspectionQty,
	ModifiedBy = @UserID,
	Modified = GETUTCDATE(),
	InspectionStatusID = ISNULL(@InspectionStatusID, InspectionStatusID)	
	WHERE ExternalID = @ExternalID

END