-- =============================================
-- Author:				Remya, Varriem
-- Create date:			2017.12.01
-- Description:			Return list of account focus types
-- Usage				exec uspAccountFocusTypesGet
-- =============================================
CREATE PROCEDURE [dbo].[uspAccountFocusTypesGet]

AS
BEGIN
	SET NOCOUNT ON;
	
	select 
	   aft.FocusTypeID ,
	   aft.FocusName,	
	   aft.TypeRank   
 from lkpAccountFocusTypes aft
where aft.IsBlacklisted = 0
order by aft.FocusName ASC
END
GO

