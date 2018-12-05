-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2018.02.09
-- Description:			Return actions options for given @ObjectTypeID
-- =============================================
CREATE PROCEDURE [dbo].[uspStateEngineActionListGet]
	@ObjectTypeID INT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		SA.ActionID,
		SA.ObjectTypeID,
		SA.ActionName,
		(SELECT
			SDV.ValueID,
			SDV.ValueName
		 FROM mapStateEngineDynamicValues MDV
			LEFT JOIN lkpStateEngineDynamicValues SDV ON MDV.ValueID = SDV.ValueID AND SDV.IsDeleted = 0
		WHERE MDV.IsDeleted = 0 AND MDV.ObjectID = SA.ActionID AND MDV.MapType = 'A'
		FOR JSON PATH) AS DynamicValues 
	FROM lkpStateEngineActions SA
	WHERE SA.ObjectTypeID = @ObjectTypeID AND SA.IsDeleted = 0
END
GO
