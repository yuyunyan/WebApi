-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.02.09
-- Description:			Return list of actions for given @RuleID
-- =============================================
CREATE PROCEDURE [dbo].[uspStateEngineRuleActionsGet] 
	@RuleID INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT 
		MRA.RuleActionID,
		MRA.RuleID,
		MRA.ActionID,
		SA.ObjectTypeID,
		SA.ActionName,
		SDV.ValueID,
		SDV.ValueName,
		MRA.StaticValue
	FROM mapStateEngineRuleActions MRA
		INNER JOIN lkpStateEngineActions SA ON SA.ActionID = MRA.ActionID AND SA.IsDeleted = 0
		LEFT JOIN lkpStateEngineDynamicValues SDV ON MRA.ValueID = SDV.ValueID AND SDV.IsDeleted = 0
	WHERE MRA.IsDeleted = 0 AND MRA.RuleID = @RuleID
		
END
GO
