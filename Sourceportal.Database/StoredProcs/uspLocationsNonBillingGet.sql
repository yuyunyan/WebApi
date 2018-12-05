/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.08.10
   Description:	Gets locations from Locations tbl using AccountID that arent billing
   Usage: EXEC uspLocationsNonBillingGet @AccountID = 4

   Revision History:


   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspLocationsNonBillingGet]
(
	@AccountID INT = NULL
	, @LocationID INT = NULL
	, @IsDeleted BIT = 0
)
AS
BEGIN 
	SET NOCOUNT ON;
		SELECT LocationID,
		AccountID,
		L.LocationTypeID,
		L.[Name],
		LT.IsStatic,
		Address1,
		Address2,
		Address4,
		HouseNumber,
		Street,
		City,
		L.StateID,
		S.StateName,
		S.StateCode,
		PostalCode,
		District,
		L.CountryID,
		C.CountryName,
		Note,
		Created,
		CreatedBy
		FROM Locations L
		INNER JOIN lkpLocationTypes LT on LT.LocationTypeID = L.LocationTypeID
		LEFT OUTER JOIN States S on S.StateID = L.StateID
		INNER JOIN Countries C on C.CountryID = L.CountryID
		WHERE AccountID = ISNULL(@AccountID,L.AccountID)
		AND LocationID = ISNULL(@LocationID,LocationID)
		AND L.LocationTypeID <> 1
		AND L.IsDeleted = ISNULL(@IsDeleted, L.IsDeleted)
END