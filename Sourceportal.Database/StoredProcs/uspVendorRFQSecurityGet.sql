/* =============================================
   Author:			Berry Zhong
   Create date:		2017.10.20
   Description:		Retrieves a list of vendor-rfqs and roles for a given user
   Usage:			EXEC uspVendorRFQSecurityGet @UserID = 64
   			
   Revision History:
	
   ============================================= */
CREATE PROCEDURE [dbo].[uspVendorRFQSecurityGet]
	@UserID INT = NULL,
	@VendorRFQID INT = NULL
AS
BEGIN
	DECLARE @ObjectTypeID INT =  27;
	--All the Rfq
	SELECT rfq.VendorRFQID, ua.RoleID
	FROM VendorRFQs rfq
		INNER JOIN vwUserAccess ua ON rfq.VendorRFQID = CASE WHEN ua.FilterObjectID = 0 THEN rfq.VendorRFQID ELSE ua.FilterObjectID END
			AND ua.FilterTypeID = 3 AND ua.FilterObjectTypeID = @ObjectTypeID
	WHERE ua.UserID = @UserID AND ua.ObjectTypeID = @ObjectTypeID AND rfq.VendorRFQID = ISNULL(@VendorRFQID, rfq.VendorRFQID)
END
