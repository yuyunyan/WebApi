/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.04.25
   Description:	Retrieves user record from tblUsers

   Revision History:
		2017.05.09	AR	Depreciated Username
		2018.01.24  ML  Added ExternalId
   ============================================= */
CREATE PROCEDURE [dbo].[uspUserGet]
(
	@UserID INT
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT UserID,
		EmailAddress,
		FirstName,
		LastName,
		PhoneNumber,
		OrganizationID,
		isEnabled,
		LastLogin,
		Created,
		CreatedBy,
		Modified,
		ModifiedBy,
		ExternalId
	FROM Users WHERE UserID = @Userid
END