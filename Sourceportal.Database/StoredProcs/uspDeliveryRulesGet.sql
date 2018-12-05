/* =============================================
 Author:				Berry, Zhong
 Create date:			2018.03.14
 Description:			Return list of options for delivery rule
 Revisions:
	2018.08.09	NA	Added ExternalID
 =============================================*/
CREATE OR ALTER PROCEDURE [dbo].[uspDeliveryRulesGet]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		DR.DeliveryRuleID,
		DR.DeliveryRuleName,
		DR.ExternalID
	FROM lkpDeliveryRules DR
	WHERE DR.IsDeleted = 0
END 
GO
