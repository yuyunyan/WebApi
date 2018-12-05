-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.02.07
-- Description:			Return StateEngineRules for given @ObjectTypeID
-- =============================================
CREATE PROCEDURE [dbo].[uspStateEngineRulesGet]
	@ObjectTypeID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		R.RuleID,
		R.TriggerID,
		R.ObjectTypeID,
		R.RuleName,
		R.ExecOrder 'RuleOrder'
	FROM StateEngineRules R 
	WHERE R.IsDeleted = 0 AND R.ObjectTypeID = ISNULL(@ObjectTypeID, R.ObjectTypeID)
END
GO
