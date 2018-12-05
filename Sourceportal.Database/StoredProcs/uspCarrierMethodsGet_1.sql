/* =============================================
   Author:		Julia Thomas
   Create date: 2018.06.02
   Description:	Retrieves all carrierMethods by all carries or carrierMethods by slected carrierID 
   Usage:		EXEC [uspCarrierMethodsGet] @CarrierID=2
   Revision History:

   
   ============================================= */
CREATE PROCEDURE [dbo].[uspCarrierMethodsGet]
(
	@CarrierID INT = NULL
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		MethodID,
		MethodName,
		CarrierID
	FROM CarrierMethods
	WHERE IsDeleted = 0 
	AND ISNULL(@CarrierID,CarrierID)= CarrierID
END

