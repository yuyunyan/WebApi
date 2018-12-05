/* =============================================
   Author:		Julia Thomas
   Create date: 2017.10.24
   Description:	Retrieves all Contacts from Contacts tbl by given AccountId
   Usage:		EXEC uspContactsByAccountIdGet @AccountId=5
				SELECT Top 100* FROM Contacts
				SELECT * FROM Locations
   Revision History:
    2017.10.25	JT	Improved the way the total count is calculate with Temp Table 
	2017.11.27	BZ	Removed IsActive from WHERE clause
	2017.11.28	BZ	Implementd Security
	2018.05.07	CT	Added ExternalID to select
		
   ============================================= */
CREATE PROCEDURE [dbo].[uspContactsByAccountIdGet]
(
	@AccountID INT = NULL,
	@UserID INT = NULL
	--@IsActive BIT = 1
)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Sec TABLE (ContactID INT, RoleID INT)
	INSERT @Sec EXECUTE uspContactSecurityGet @UserID = @UserID;

	WITH Main_TEM  AS (
	   SELECT C.ContactID,
		C.AccountID,
		C.ExternalID,
		A.AccountName,
		C.FirstName,
		C.LastName,
		C.OfficePhone,
		C.Email,
		C.IsActive
	FROM Contacts C
		INNER JOIN (SELECT DISTINCT ContactID FROM @Sec) sec ON C.ContactID = sec.ContactID AND C.IsDeleted = 0
		LEFT OUTER JOIN Accounts A ON A.AccountID = C.AccountID
	WHERE A.AccountID = ISNULL(NULLIF(@AccountID,0), A.AccountID)
		--AND IsActive = @IsActive
		),
  COUNT_TEM AS (
    SELECT COUNT(*) AS TotalROWCount
	FROM Main_TEM
  )
  SELECT
        Main_TEM.ContactID,
		Main_TEM.ExternalID,
        Main_TEM.AccountID,
		Main_TEM.AccountName,
		Main_TEM.FirstName,
		Main_TEM.LastName,
		Main_TEM.OfficePhone,
		Main_TEM.Email,
		Main_TEM.IsActive,
		COUNT_TEM.TotalROWCount
	FROM Main_TEM,COUNT_TEM
END
