-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.02.05
-- Description:			Return rule groups for given @RuleID
-- =============================================
CREATE PROCEDURE [dbo].[uspStateEngineRuleGroupsGet]
	@RuleID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SRG.RuleGroupID,
		SRG.RuleID,
		SRG.ParentGroupID,
		SRG.IsALL,
		SR.ObjectTypeID,
		(SELECT
			MRC.RuleConditionID,
			MRC.RuleGroupID,
			MRC.ConditionID,
			MRC.Comparison,
			MRC.ValueID,
			MRC.StaticValue,
			SDV.ValueName,
			SC.ConditionName,
			SC.ComparisonType,
			SC.ObjectTypeID
		 FROM mapStateEngineRuleConditions MRC
			INNER JOIN lkpStateEngineConditions SC ON SC.ConditionID = MRC.ConditionID AND SC.IsDeleted = 0
			LEFT JOIN lkpStateEngineDynamicValues SDV ON MRC.ValueID = SDV.ValueID AND SDV.IsDeleted = 0
			WHERE MRC.IsDeleted = 0 AND MRC.RuleGroupID = SRG.RuleGroupID
			FOR JSON PATH) AS RuleConditions 
	FROM StateEngineRuleGroups SRG
		LEFT JOIN StateEngineRules SR ON SR.RuleID = SRG.RuleID AND SR.IsDeleted = 0
	WHERE SRG.RuleID = @RuleID AND SRG.IsDeleted = 0
		
END
GO
