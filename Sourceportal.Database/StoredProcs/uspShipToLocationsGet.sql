/* =============================================
	 Author:			Berry, Zhong
	 Create date:		11.07.2017
	 Description:		Return list of ship to address
	 Usage:				EXEC uspShipToLocationsGet @IsDropShip = 1
	============================================= */
CREATE PROCEDURE [dbo].[uspShipToLocationsGet] 
	@IsDropShip BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

    SELECT Distinct
		L.LocationID,
		L.AccountID,
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
		Note
	FROM Locations L
		INNER JOIN Accounts ac ON ac.AccountID = L.AccountID 
			AND ac.IsSourceability = (CASE WHEN @IsDropShip = 0 THEN ac.IsSourceability ELSE 1 END)
		INNER JOIN lkpLocationTypes LT on  L.LocationTypeID & LT.LocationTypeID = LT.LocationTypeID
		LEFT OUTER JOIN States S on S.StateID = L.StateID
		INNER JOIN Countries C on C.CountryID = L.CountryID
	WHERE L.AccountID > 0 AND L.IsDeleted = 0
END