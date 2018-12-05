/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.06.06
   Description:	Gets locations from Locations tbl using AccountID and/or LocationID to narrow
   Usage: EXEC uspLocationsGet @AccountID = 52, @LocationTypeID = 4
		  EXEC uspLocationsGet @LocationID = 1
		  UPDATE Locations SET StateID = 4291 WHERE LocatioNID = 2

   Revision History:
		2017.06.08	AR	Renamed CountryCode to CountryID
		2017.06.16	AR	Added StateCode
		2017.07.25	AR	Added @LocationTypeID Paramter
		2017.10.17	CT	Added LocationTypes ExternalID
		2017.10.19  CT  Added Location ExternalID
		2017.12.05  CT  Added fnGetLocationTypes
		2018.01.10	CT	Added bitwise operand on LocationTypeID
		2018.02.28	BZ	Updated WHERE clause, fix bitwise location 
   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspLocationsGet]
(
	@AccountID INT = NULL
	, @LocationID INT = NULL
	, @LocationTypeID INT = NULL
	, @IsDeleted BIT = 0
)
AS
BEGIN 
	SET NOCOUNT ON;
		SELECT DISTINCT LocationID,
		AccountID,
		L.ExternalID,
		L.LocationTypeID,
		dbo.fnGetLocationTypes(LocationID) LocationTypeName,
		L.[Name],
		--LT.IsStatic,
		dbo.fnGetLocationTypeExternalIds(LocationID) LocationTypeExternalID,
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
		C.CountryCode,
		C.CountryCode2,
		Note,
		Created,
		CreatedBy
		FROM Locations L
		INNER JOIN lkpLocationTypes LT on LT.LocationTypeID & L.LocationTypeID = LT.LocationTypeID
		LEFT OUTER JOIN States S on S.StateID = L.StateID
		INNER JOIN Countries C on C.CountryID = L.CountryID
		WHERE AccountID = ISNULL(@AccountID,L.AccountID)
		AND LocationID = ISNULL(@LocationID,LocationID)
		AND L.LocationTypeID & ISNULL(@LocationTypeID,L.LocationTypeID) = ISNULL(@LocationTypeID,L.LocationTypeID)
		AND L.IsDeleted = ISNULL(@IsDeleted, L.IsDeleted)
END