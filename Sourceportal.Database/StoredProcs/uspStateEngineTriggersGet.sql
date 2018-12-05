-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.02.07
-- Description:			Retrun triggers for an given objectTypeID
-- =============================================
CREATE PROCEDURE [dbo].[uspStateEngineTriggersGet]
	@ObjectTypeID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		ST.TriggerID,
		ST.ObjectTypeID,
		ST.TriggerName,
		ST.TriggerDescription
	FROM lkpStateEngineTriggers ST
	WHERE ST.ObjectTypeID = @ObjectTypeID AND ST.IsDeleted = 0
END
GO
