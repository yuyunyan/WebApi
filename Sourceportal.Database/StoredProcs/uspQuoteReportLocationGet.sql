/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.01.31
   Description:	Gets locations from Locations tbl using QuoteID
   Usage: EXEC uspQuoteReportLocationGet @QuoteID = 102183, @VersionID = 1, @LocationTypeID = 1
   SELECT *FROM lkpLocationTypes
   Revision History:
		2018.01.31	AR	Initial Deployment
   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspQuoteReportLocationGet]
(
	@QuoteID INT = NULL
	, @VersionID INT = NULL
	, @LocationTypeID INT = NULL
)
AS
BEGIN 
	SET NOCOUNT ON;
	DECLARE @temp TABLE (ColData VARCHAR(255), OrderBy INT IDENTITY(1,1))
	DECLARE @Name VARCHAR(255), @HouseStreet VARCHAR(255)
	,  @Address1 VARCHAR(255), @Address2 VARCHAR(255)
	, @Address4 VARCHAR(255) , @City VARCHAR(255), @State VARCHAR(255)
	, @Zip VARCHAR(255), @Country VARCHAR(255)

	SELECT
		@HouseStreet = L.HouseNumber + ' ' + L.Street,
		@Name = A.AccountName,
		@Address1 = Address1,
		@Address2 = L.Address2,
		@Address4 = L.Address4,
		@City = ISNULL(City + ', ',''),
		@State = CASE WHEN ISNULL(S.StateCode,'') = '' THEN S.StateName + ' 'ELSE S.StateCode + ' ' END,
		@Zip = ISNULL(L.PostalCode,''),
		@Country = C.CountryName
	FROM Quotes Q
	INNER JOIN Accounts A on A.AccountID = Q.AccountID
	INNER JOIN Locations L on l.AccountID = Q.AccountID
	LEFT OUTER JOIN States S on S.StateID = L.StateID
	INNER JOIN Countries C on C.CountryID = L.CountryID
	WHERE Q.QuoteID = @QuoteID
	AND Q.VersionID = @VersionID
	--AND ISNULL(@LocationTypeID,L.LocationTypeID) &  L.LocationTypeID =  L.LocationTypeID
	AND L.LocationTypeID & @LocationTypeID = @LocationTypeID
	INSERT INTO @temp (ColData)
	VALUES 	(@Name),
	(@HouseStreet),
	(@Address1),
	(@Address2),
	(@Address4),
	(@City + @State + @Zip),
	(@Country)

	SELECT ColData
	FROM @temp
	WHERE ISNULL(ColData,'') != ''
	ORDER BY OrderBy ASC
	--SELECT ISNULL(@Name,@Address1)
	--UNION SELECT ISNULL(@Address1,ISNULL(@Address2,ISNULL(@Address4,ISNULL(@CityStateZip,@Country))))
	--UNION SELECT ISNULL(@Address2,ISNULL(@Address4,ISNULL(@CityStateZip,@Country)))
	--UNION SELECT ISNULL(@Address4,ISNULL(@CityStateZip,@Country))
	--UNION SELECT ISNULL(@CityStateZip,@Country)
	--UNION SELECT @Country
END