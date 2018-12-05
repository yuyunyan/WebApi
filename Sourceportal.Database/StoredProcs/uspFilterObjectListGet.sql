/* =============================================
   Author:			Berry Zhong
   Create date:		2017.10.26
   Description:		Retrieves a list of filter objects
   ============================================= */
CREATE PROCEDURE [dbo].[uspFilterObjectListGet]

AS
BEGIN
SET NOCOUNT ON;
	SELECT 
		AccountID 'ObjectID',
		AccountName 'ObjectName',
		1 'ObjectTypeID'
	FROM Accounts
	WHERE IsDeleted = 0
	UNION
	SELECT 
		RegionID 'ObjectID',
		RegionName 'ObjectName',
		3 'ObjectTypeID'
	FROM lkpRegions
	WHERE IsDeleted = 0
	UNION
	SELECT 
		AccountHierarchyID 'ObjectID',
		HierarchyName 'ObjectName',
		4 'ObjectTypeID'
	FROM AccountHierarchies
	WHERE IsDeleted = 0
	UNION
	SELECT 
		UserID 'ObjectID',
		FirstName + ' ' + LastName 'ObjectName',
		32 'ObjectTypeID'
	FROM Users

END
GO
