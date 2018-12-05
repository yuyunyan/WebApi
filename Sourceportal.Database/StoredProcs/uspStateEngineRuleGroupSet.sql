-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.02.07
-- Description:			Insert or edit new record of StateEngineRuleGroup and children rule condition
-- =============================================
CREATE PROCEDURE [dbo].[uspStateEngineRuleGroupSet]
	@RuleGroupID INT = NULL,
	@RuleID INT = NULL,
	@ParentGroupID INT = NULL,
	@IsAll BIT = 0,
	@UserID INT = 0,
	@RuleConditionsJSON VARCHAR(MAX) = ''
AS
BEGIN
	SET NOCOUNT ON;
    
	IF ISNULL(@RuleGroupID, 0) = 0
		GOTO INSERT_RULE_GROUP
	ELSE
		GOTO UPDATE_RULE_GROUP

INSERT_RULE_GROUP:
	INSERT INTO StateEngineRuleGroups (RuleID, ParentGroupID, IsAll, CreatedBy, IsDeleted)
	VALUES (@RuleID, @ParentGroupID, @IsAll, @UserID, 0)
	
	SET @RuleGroupID = SCOPE_IDENTITY()
	GOTO INSERT_UPDATE_RULE_CONDITIONS

UPDATE_RULE_GROUP:
	UPDATE StateEngineRuleGroups
	SET RuleID = @RuleID,
		ParentGroupID = @ParentGroupID,
		IsAll = @IsAll,
		IsDeleted = 0
	WHERE RuleGroupID = @RuleGroupID

	UPDATE StateEngineRuleGroups
	SET IsDeleted = 1
	WHERE ParentGroupID = @RuleGroupID

	GOTO INSERT_UPDATE_RULE_CONDITIONS
		

INSERT_UPDATE_RULE_CONDITIONS:
	UPDATE mapStateEngineRuleConditions SET IsDeleted = 1 WHERE RuleGroupID = @RuleGroupID

	INSERT INTO mapStateEngineRuleConditions (RuleGroupID, ConditionID, Comparison, ValueID, StaticValue, CreatedBy, IsDeleted)
	SELECT
		@RuleGroupID,
		RC.ConditionID,
		RC.Comparison,
		RC.ValueID,
		RC.StaticValue,
		@UserID,
		0
	FROM OPENJSON(@RuleConditionsJSON)
		WITH(
			Comparison VARCHAR(256),
			ConditionID INT,
			RuleConditionID INT,
			ValueID INT,
			StaticValue VARCHAR(256)
		)RC
	WHERE ISNULL(RC.RuleConditionID, 0) = 0 

	UPDATE mapStateEngineRuleConditions
	SET Comparison = RC.Comparison,
		ConditionID = RC.ConditionID,
		RuleGroupID = @RuleGroupID,
		ValueID = RC.ValueID,
		StaticValue = RC.StaticValue,
		IsDeleted = 0
	FROM OPENJSON(@RuleConditionsJSON)
		WITH(
			Comparison VARCHAR(256),
			ConditionID INT,
			RuleConditionID INT,
			ValueID INT,
			StaticValue VARCHAR(256)
		)RC
	WHERE RC.RuleConditionID = mapStateEngineRuleConditions.RuleConditionID

	GOTO RETURN_SELECT

RETURN_SELECT:
	SELECT @RuleGroupID 'RuleGroupID'
END
