
-- =============================================
-- Author:				Remya, Varriem
-- Create date:			2018.01.02
-- Description:			Return list of account focus types
-- Usage				exec uspAccountFocusesGet @AccountID = 1
-- =============================================
CREATE PROCEDURE [dbo].[uspAccountFocusesGet]
	@AccountID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	select
	   maf.ObjectID
	   ,maf.FocusTypeID
	   ,maf.FocusObjectTypeID 	   
	   ,maf.AccountID
	   , maf.FocusID
	   , afot.ObjectTypeID
	   , aft.FocusName
	   , ot.ObjectName
	   , ic.CommodityID
	   , ic.CommodityName as ObjectValue
	   , m.MfrID
	   , m.MfrName as ObjectValue
 from  mapAccountFocuses maf
 inner join lkpAccountFocusObjectTypes afot on afot.FocusObjectTypeID = maf.FocusObjectTypeID 
 inner join lkpObjectTypes ot on ot.ObjectTypeID = afot.ObjectTypeID  
 inner join lkpAccountFocusTypes aft on aft.FocusTypeID = maf.FocusTypeID
 left outer join lkpItemCommodities ic on ic.CommodityId = maf.ObjectID and ot.ObjectName LIKE '%Commodity%'
 left outer join Manufacturers m on m.MfrID = maf.ObjectID and ot.ObjectName LIKE '%Manufacturer%'
where maf.AccountID = @AccountID and maf.IsDeleted = 0 
AND (CASE ot.ObjectName WHEN 'Commodity' THEN ic.CommodityID ELSE m.MfrID END = maf.ObjectID )

END
GO