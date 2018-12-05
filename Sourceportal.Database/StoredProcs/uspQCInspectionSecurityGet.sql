/* =============================================
   Author:			Berry Zhong
   Create date:		2017.10.23
   Description:		Retrieves a list of qc-inspection and roles for a given user
   Usage:			EXEC uspQCInspectionSecurityGet @UserID = 64
   			
   Revision History:
	
   ============================================= */
CREATE PROCEDURE [dbo].[uspQCInspectionSecurityGet]
	@UserID INT = NULL,
	@InspectionID INT = NULL
AS
BEGIN
	DECLARE @ObjectTypeID INT =  104;
	--All Purchase-Orders
	SELECT ins.InspectionID, ua.RoleID
	FROM QCInspections ins
		INNER JOIN vwUserAccess ua ON ins.InspectionID = CASE WHEN ua.FilterObjectID = 0 THEN ins.InspectionID ELSE ua.FilterObjectTypeID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND ins.InspectionID = ISNULL(@InspectionID, ins.InspectionID)
END
