
-- =============================================
-- Author:				Remya, Varriem
-- Create date:			2018.01.04
-- Description:			Return list of account focus object types
-- Usage				exec uspAccountFocusObjectTypesGet
-- =============================================
CREATE PROCEDURE [dbo].[uspAccountFocusObjectTypesGet]

AS
BEGIN
	SET NOCOUNT ON;
	
	select lop.ObjectName,
	   lop.ObjectTypeID,
	   afot.FocusObjectTypeID 	   
 from lkpObjectTypes lop 
inner join lkpAccountFocusObjectTypes afot 
on lop.ObjectTypeID = afot.ObjectTypeID
where (lop.IsDeleted = 0 and afot.IsDeleted = 0)
order by lop.ObjectName ASC
END
GO

