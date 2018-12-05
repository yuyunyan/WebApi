/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.06.06
   Description:	Inserts or updates record in locations tbl
   Usage: EXEC uspLocationSet @AccountID = 4, @LocationTypeID = 1, @Name = 'TestLocation', @HouseNumber = '60', @Street = 'Bunsen', @City = 'Irvine'
		  SELECT * FROM Locations
   Revision History:
		2017.06.08	AR	Renamed CountryCode to CountryID
		2017.06.09	AR	Added LocationID col to output select
		2017.07.21	AR	Added NULLIF to CountryID, stateID, AccountID, and LocationTypeID
		2017.10.30 ML Added ExternalID
		2018.01.10  CT  Added LocationTypeID swap logic for 'bill to' rules
		2018.04.02	CT  Update Row if External ID is matching
		2018.08.09	NA	Made @CreatedBy required
   Return Codes:
		-1  @CreatedBy is required
		-3	Insert Failed
		-4	Update Failed, check LocationID
   ============================================= */
CREATE PROCEDURE [dbo].[uspLocationSet]
(
	@LocationID INT = NULL OUTPUT
	, @AccountID INT = NULL
	, @LocationTypeID INT = NULL
	, @Name VARCHAR(128) = NULL
	, @Address1 VARCHAR(64) = NULL
	, @Address2 VARCHAR(64) = NULL
	, @HouseNumber VARCHAR(32) = NULL
	, @Street VARCHAR(64) = NULL
	, @Address4 VARCHAR(64) = NULL
	, @City VARCHAR(64) = NULL
	, @StateID INT = NULL
	, @PostalCode VARCHAR(32) = NULL
	, @District VARCHAR(64) = NULL
	, @CountryID INT = NULL
	, @Note VARCHAR(51) = NULL
	, @CreatedBy INT = NULL
	, @IsDeleted BIT = 0
	, @ExternalID VARCHAR(50) = NULL
)
AS
BEGIN 
	SET NOCOUNT ON;
	
	IF ISNULL(@CreatedBy, 0) = 0
		RETURN -1

	IF (@LocationTypeID & 1 = 1)
		GOTO SwapLocationTypes
	ELSE IF (ISNULL(@LocationID, 0) = 0)
		GOTO InsertLocation
	ELSE
		GOTO UpdateLocation

SwapLocationTypes:
	UPDATE Locations
	SET LocationTypeID = LocationTypeID - 1
	WHERE AccountID = @AccountID AND (LocationTypeID & 1 = 1)

	UPDATE Locations
	SET LocationTypeID = 2
	WHERE AccountID = @AccountID AND LocationTypeID = 0

	IF (ISNULL(@LocationID, 0) = 0)
		GOTO InsertLocation
	ELSE
		GOTO UpdateLocation

InsertLocation:
	INSERT INTO Locations (AccountID, LocationTypeID, [Name], Address1, Address2, HouseNumber, Street, Address4, City, StateID, PostalCode, District, CountryID, Note, CreatedBy, ExternalID )
	VALUES ( @AccountID
		, @LocationTypeID
		, @Name
		, @Address1
		, @Address2
		, @HouseNumber
		, @Street
		, @Address4
		, @City
		, @StateID
		, @PostalCode
		, @District
		, @CountryID
		, @Note
		, @CreatedBy
		, @ExternalID )
	SET @LocationID = @@IDENTITY

	IF (ISNULL(@LocationID,0) = 0)
		RETURN -3
	GOTO SelectOutput

UpdateLocation:
	UPDATE Locations
	SET AccountID = ISNULL(NULLIF(@AccountID,0), AccountID)
		, LocationTypeID = ISNULL(NULLIF(@LocationTypeID,0), LocationTypeID)
		, [Name] = ISNULL(@Name, [Name])
		, Address1 = ISNULL(@Address1, Address1)
		, Address2 = ISNULL(@Address2, Address2)
		, HouseNumber = ISNULL(@HouseNumber, HouseNumber)
		, Street = ISNULL(@Street, Street)
		, Address4 = ISNULL(@Address4, Address4)
		, City = ISNULL(@City, City)
		, StateID = ISNULL(NULLIF(@StateID,0), StateID)
		, PostalCode = ISNULL(@PostalCode, PostalCode)
		, District = ISNULL(@District, District)
		, CountryID = ISNULL(NULLIF(@CountryID,''), CountryID)
		, Note = ISNULL(@Note, Note)
		, ModifiedBy = @CreatedBy
		, Modified = GETUTCDATE()
		, IsDeleted = ISNULL(@IsDeleted, IsDeleted) 
		, ExternalID = ISNULL(@ExternalID, ExternalID)
	WHERE LocationID = @LocationID  OR ExternalID = @ExternalID --null = null ? equals false

		IF (@@rowcount = 0)
			RETURN -4
		GOTO SelectOutput

SelectOutput:
	SELECT AccountID
		, LocationID
		, LocationTypeID
		, [Name]
		, Address1
		, Address2
		, HouseNumber
		, Street
		, Address4
		, City
		, StateID
		, PostalCode
		, District
		, CountryID
		, Note
		, CreatedBy
		, ExternalID
	FROM Locations
	WHERE LocationID = @LocationID
END