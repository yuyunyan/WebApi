-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.02.13
-- Description:			Update, Insert or Delete records on mapStateEngineRuleActions
-- =============================================
CREATE PROCEDURE [dbo].[mapStateEngineRuleActionsSet]
	@RuleID INT = NULL,
	@UserID INT = NULL,
	@RuleActionsJSON VARCHAR(MAX) = ''
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    MERGE mapStateEngineRuleActions MRA
	USING
	(
		(SELECT
			RuleActionID,
			StaticValue,
			ActionID,
			ValueID
		 FROM OPENJSON(@RuleActionsJSON)
			WITH(
				StaticValue VARCHAR(256),
				RuleActionID INT,
				RuleID INT,
				ActionID INT,
				ValueID INT
			))
	)RA
	ON MRA.RuleActionID = RA.RuleActionID
	WHEN MATCHED 
	THEN UPDATE SET MRA.ActionID = RA.ActionID,
		MRA.ValueID = CASE WHEN RA.ValueID = 0 THEN NULL ELSE RA.ValueID END,
		MRA.StaticValue = RA.StaticValue,
		MRA.ModifiedBy = @UserID,
		MRA.Modified = getdate()
	WHEN NOT MATCHED BY TARGET
	THEN INSERT (RuleID, ActionID, ValueID, StaticValue, CreatedBy, IsDeleted)
		 VALUES(
			@RuleID,
			RA.ActionID,
			CASE WHEN RA.ValueID = 0 THEN NULL ELSE RA.ValueID END,
			RA.StaticValue,
			@UserID,
			0
		 )
	WHEN NOT MATCHED BY SOURCE AND MRA.RuleID = @RuleID
	THEN UPDATE SET MRA.IsDeleted = 1;

	SELECT @@ROWCOUNT 'RowCount'
END
GO
