-- =============================================
-- Author:				Berry, Zhong
-- Create date:			11.07.2017
-- Description:			Return list of shipping methods
-- =============================================
CREATE PROCEDURE [dbo].[uspShippingMethodsGet] 
	
AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT
		ShippingMethodID,
		MethodName
	FROM
		codes.lkpShippingMethods
	WHERE IsDeleted = 0
END
GO
