/* =============================================
   Author:		Nathan Ayers
   Create date: 2018.02.07
   Description:	Gets a list of Users who are Buyers
   Usage:		EXEC uspBuyersGet

   Revision History:
				2018.11.06	JC	Added boolean flag to sort by FirstName
   Return Codes:
   ============================================= */

CREATE PROCEDURE [dbo].[uspBuyersGet]
	@SortByFirstName INT = 0
AS
BEGIN
	SELECT u.UserID, u.FirstName, u.LastName
	FROM Users u
		INNER JOIN mapUserBuyers b ON u.UserID = b.UserID AND b.IsDeleted = 0
	ORDER BY
		CASE 
			WHEN @SortByFirstName = 1 THEN u.FirstName
			ELSE u.LastName
		END ASC
END
