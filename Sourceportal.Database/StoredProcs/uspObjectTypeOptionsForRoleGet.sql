-- =============================================
-- Author:				Berry, Zhong
-- Create date:			11.13.2017
-- Description:			Return list of type options in role grid
-- =============================================
CREATE PROCEDURE [dbo].[uspObjectTypeOptionsForRoleGet]
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		o.ObjectTypeID,
		o.ObjectName
	FROM lkpObjectTypes o
	INNER JOIN (SELECT DISTINCT ObjectTypeID FROM lkpObjectTypeSecurity) p ON o.ObjectTypeID = p.ObjectTypeID
	WHERE o.IsDeleted = 0
	UNION
	SELECT
		8,
		'Navigation'
END
GO
