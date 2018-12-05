-- =============================================
-- Author:				Berry, Zhong
-- Create date:			2017.12.04
-- Description:			Return list of focuses for a contact
--   Revision History:
--   2018.06.27	Hrag Added condition IsDeleted = 0 for both 
--						uspContactFocusesGet @AccountID=1
-- =============================================
CREATE PROCEDURE [dbo].[uspContactFocusesGet]
	@ContactID INT = NULL,
	@AccountID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	 AF.FocusID,
	 M.MfrName 'FocusName',
	 OT.ObjectName,
	 (CASE WHEN ISNULL(CF.ContactID, 0) != @ContactID THEN 'True' ELSE 'False' END) 'IsOption'
	FROM mapAccountFocuses AF
		INNER JOIN lkpAccountFocusTypes FT ON FT.FocusTypeID = AF.FocusTypeID
		inner join lkpAccountFocusObjectTypes FO ON AF.FocusObjectTypeID = FO.FOcusObjectTypeID 
		INNER JOIN lkpObjectTypes OT ON OT.ObjectTypeID = FO.ObjectTypeID
		INNER JOIN Manufacturers M ON M.MfrID = AF.ObjectID AND OT.ObjectTypeID = 102
		LEFT OUTER JOIN mapContactFocuses CF ON CF.FocusID = AF.FocusID AND CF.IsDeleted = 0
	WHERE AF.AccountID = @AccountID AND AF.IsDeleted=0

	UNION
	SELECT 
	 AF.FocusID,
	 IC.CommodityName 'FocusName',
	 OT.ObjectName,
	 (CASE WHEN ISNULL(CF.ContactID, 0) != @ContactID THEN 'True' ELSE 'False' END) 'IsOption'
	FROM mapAccountFocuses AF
		INNER JOIN lkpAccountFocusTypes FT ON FT.FocusTypeID = AF.FocusTypeID
		inner join lkpAccountFocusObjectTypes FO ON AF.FocusObjectTypeID = FO.FOcusObjectTypeID 
		INNER JOIN lkpObjectTypes OT ON OT.ObjectTypeID = FO.ObjectTypeID
		INNER JOIN lkpItemCommodities IC ON IC.CommodityID = AF.ObjectID AND OT.ObjectTypeID = 101
		LEFT OUTER JOIN mapContactFocuses CF ON CF.FocusID = AF.FocusID AND CF.IsDeleted = 0
	WHERE AF.AccountID = @AccountID AND AF.IsDeleted=0
END
GO
