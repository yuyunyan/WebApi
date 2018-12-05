/* =============================================
	 Author:			Aaron Rodecker
	 Create date:		2018.04.09
	 Description:		Returns default statusID for objectTypeID
	 Usage:				SELECT dbo.fnGetDefaultStatus(22)

	 Revision:
	 2018.04.09		AR	Initial Deployment
	============================================= */
CREATE FUNCTION [dbo].[fnGetDefaultStatus]
(
	@ObjectTypeID INT
)
RETURNS INT
AS
BEGIN
	DECLARE @StatusID INT
	SELECT @StatusID = StatusID
	FROM lkpStatuses
	where ObjectTypeID = @ObjectTypeID
	AND IsDefault = 1

	RETURN @StatusID

END