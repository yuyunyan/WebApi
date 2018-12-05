/* =============================================
	Author:				Berry, Zhong
	Create date:		2017.10.26
	Description:		Return list of ObjectTypeSecurity
	Usage:				exec uspObjectTypeSecurityListGet 19
   =============================================*/
CREATE PROCEDURE [dbo].[uspObjectTypeSecurityListGet]
	@ObjectTypeID INT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		os.TypeSecurityID,
		os.TypeDescription,
		os.ObjectTypeID,
		os.FilterTypeID,
		os.FilterObjectTypeID
	FROM
		lkpObjectTypeSecurity os
	WHERE os.IsDeleted = 0 AND os.ObjectTypeID = ISNULL(@ObjectTypeID, os.ObjectTypeID)
END
GO
