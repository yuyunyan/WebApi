/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.02.07
   Description:	Gets a list of Users who are Sellers
   Usage:		EXEC uspSellersGet

   Revision History:
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspSellersGet]
AS
BEGIN
	SELECT u.UserID, u.FirstName, u.LastName
	FROM Users u
		INNER JOIN mapUserSellers s ON u.UserID = s.UserID AND s.IsDeleted = 0
	ORDER BY u.LastName
END
