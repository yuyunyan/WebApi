/* =============================================
   Author:		Julia Thomas
   Create date: 2018.06.02
   Description:	Retrieves all carrierMethods or single one 
   Usage:		EXEC [uspCarrierMethodsGet] @MethodID=2
   Revision History:

   
   ============================================= */
CREATE PROCEDURE [dbo].[uspCarrierMethodsGet]
(
	@MethodID INT = NULL
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
	AND ISNULL(@MethodID,MethodID)= MethodID
END