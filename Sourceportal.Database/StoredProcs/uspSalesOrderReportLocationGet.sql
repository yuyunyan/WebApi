/* =============================================
   Author:		Aaron Rodecker
   Create date: 2018.06.14
   Description:	Gets locations from Locations tbl using SalesOrderID
   Usage:		EXEC uspSalesOrderReportLocationGet @SalesOrderID = 100007, @VersionID = 2, @LocationTypeID = 2
				SELECT *FROM lkpLocationTypes
				Select * FROM SalesOrders
   Revision History:
		2018.06.14	AR	Initial Deployment
   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspSalesOrderReportLocationGet]
(
	@SalesOrderID INT = NULL
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

	IF (@LocationTypeID = 2)
	BEGIN
			SELECT
			@HouseStreet = L.HouseNumber + ' ' + L.Street,
			@Name = L.[Name],
			@Address1 = Address1,
			@Address2 = L.Address2,
			@Address4 = L.Address4,
			@City = ISNULL(City + ', ',''),
			@State = CASE WHEN ISNULL(S.StateCode,'') = '' THEN S.StateName + ' 'ELSE S.StateCode + ' ' END,
			@Zip = ISNULL(L.PostalCode,''),
			@Country = C.CountryName
		FROM Salesorders P
		INNER JOIN Locations L on l.LocationID = P.ShipLocationID
		INNER JOIN Accounts A on A.AccountID = L.AccountID
		LEFT OUTER JOIN States S on S.StateID = L.StateID
		INNER JOIN Countries C on C.CountryID = L.CountryID
		WHERE P.SalesOrderID = @SalesOrderID
		AND P.VersionID = @VersionID
	END
	ELSE BEGIN
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
		FROM SalesOrders P
		INNER JOIN Accounts A on A.AccountID = P.AccountID
		INNER JOIN Locations L on l.AccountID = P.AccountID
		LEFT OUTER JOIN States S on S.StateID = L.StateID
		INNER JOIN Countries C on C.CountryID = L.CountryID
		WHERE P.SalesOrderID = @SalesOrderID
		AND P.VersionID = @VersionID
		--AND ISNULL(@LocationTypeID,L.LocationTypeID) &  L.LocationTypeID =  L.LocationTypeID
		AND L.LocationTypeID & @LocationTypeID = @LocationTypeID
	END
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
END