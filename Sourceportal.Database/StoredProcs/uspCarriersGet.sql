
-- =============================================
-- Author:				Julia Thomas
-- Create date:			2018.05.25
-- Description:			Return list of carriers
-- Usage				exec uspCarriersGet
-- =============================================
CREATE PROCEDURE [dbo].[uspCarriersGet]
	
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
	 c.CarrierID,
	 c.CarrierName
	 FROM Carriers  c
	 WHERE IsDeleted=0
END