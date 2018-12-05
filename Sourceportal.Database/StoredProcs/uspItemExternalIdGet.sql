/* =============================================
   Author:		Corey Tyrrell
   Create date: 2017.11.07
   Description:	Retrieves the ExternalID of the Item
   Usage:		EXEC uspItemExternalIdGet @ItemID = 60
   Revision History:
   ============================================= */
CREATE PROCEDURE [dbo].[uspItemExternalIdGet]
(
	@ItemID INT
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		ExternalID
	FROM Items 
	WHERE ItemID = @ItemID
END