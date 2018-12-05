-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2017.12.08
-- Description:			Return list of FreightPayment
--Revision History: Julia Thomas Added UseAccountNum
-- =============================================
CREATE PROCEDURE [dbo].[uspFreightPaymentMethodsGet]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SELECT
		FreightPaymentMethodID,
		MethodName,
		ExternalID,
		UseAccountNum
	FROM codes.lkpFreightPaymentMethods FPM
	WHERE FPM.IsDeleted = 0

END
