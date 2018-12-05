/* =============================================
	Author:				Berry, Zhong
	Create date:		2017.10.26
	Description:		Return list of avaliable type in role
   =============================================*/
CREATE PROCEDURE [dbo].[uspSecurityTypeListGet] 
	
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		o.ObjectTypeID,
		o.ObjectName
	FROM lkpObjectTypes o
		INNER JOIN (SELECT DISTINCT ObjectTypeID FROM Roles WHERE IsDeleted = 0) r ON o.ObjectTypeID = r.ObjectTypeID
		INNER JOIN (SELECT DISTINCT ObjectTypeID FROM lkpObjectTypeSecurity WHERE IsDeleted = 0) s ON o.ObjectTypeID = s.ObjectTypeID
	WHERE o.IsDeleted = 0
END
GO
