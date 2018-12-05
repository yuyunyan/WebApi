-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.02.18
-- Description:			Insert new rule detail or update rule detail for given @RuleID
-- =============================================
CREATE PROCEDURE [dbo].[uspStateEngineRuleDetailSet] 
	@RuleID INT = NULL,
	@TriggerID INT = NULL,
	@RuleOrder INT = NULL,
	@ObjectTypeID INT = NULL,
	@RuleName VARCHAR(256) = NULL,
	@IsDeleted BIT = 0,
	@UserID INT = 0
AS
BEGIN
	SET NOCOUNT ON;

	IF ISNULL(@RuleID, 0) = 0
		GOTO INSERT_NEW_RULE
	ELSE
		GOTO UPDATE_RULE

INSERT_NEW_RULE:
	INSERT INTO StateEngineRules (TriggerID, ObjectTypeID, RuleName, ExecOrder, CreatedBy, IsDeleted)
	VALUES (@TriggerID, @ObjectTypeID, @RuleName, @RuleOrder, @UserID, 0)
	
	SET @RuleID = SCOPE_IDENTITY()

	GOTO RETURN_SELECT

UPDATE_RULE:
	UPDATE StateEngineRules
	SET TriggerID = ISNULL(@TriggerID, TriggerID),
		ObjectTypeID = ISNULL(@ObjectTypeID, ObjectTypeID),
		RuleName = ISNULL(@RuleName, RuleName),
		ExecOrder = ISNULL(@RuleOrder, ExecOrder),
		IsDeleted = @IsDeleted
	WHERE RuleID = @RuleID

	GOTO RETURN_SELECT

RETURN_SELECT:
	SELECT @RuleID 'RuleID'
END
GO
