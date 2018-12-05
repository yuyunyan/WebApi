/* =============================================
   Author:				Berry, Zhong
   Create date:			2018.02.01
   Description:			Return conditions by objectTypes
   Revision History:
		2018.09.10	NA	Added lookup for Parent Object Type
						Removed support for passing in 0 or null ObjectTypeID
   =============================================*/
CREATE OR ALTER PROCEDURE [dbo].[uspStateEngineConditionsGet]
	@ObjectTypeID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		STC.ConditionID,
		STC.ObjectTypeID,
		STC.ConditionName,
		STC.ComparisonType,
		 (SELECT
			DV.ValueID,
			DV.ValueName
			FROM mapStateEngineDynamicValues MDV
			INNER JOIN lkpStateEngineDynamicValues DV ON MDV.ValueID = DV.ValueID AND DV.IsDeleted = 0
			WHERE MDV.IsDeleted = 0 AND MDV.ObjectID = STC.ConditionID AND MDV.MapType = 'C'
			FOR JSON PATH) AS DynamicValues 
	FROM lkpStateEngineConditions STC
	WHERE (STC.ObjectTypeID IN (SELECT ObjectTypeID FROM lkpObjectTypes WHERE ParentID = @ObjectTypeID) OR STC.ObjectTypeID = @ObjectTypeID)	
	AND STC.IsDeleted = 0
END
