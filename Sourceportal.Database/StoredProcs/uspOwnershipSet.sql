/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.04.27
   Description:	Inserts owner record into mapOwnership
   Usage: EXEC uspOwnershipSet @ObjectID=5,@ObjectTypeID=2,@OwnerList=N'[{"userId":1,"percentage":25.0},{"userId":2,"percentage":75.0}]',@CreatedBy=2
		SELECT * FROM mapOwnership

   Revision History:
		2017.06.13	AR	depreciated @OwnerID/@Percent with JSON param @OwnerList
		2017.12.12	BZ	Fixed duplicate owner

   Return Codes:
		-1	Insert Failed
   ============================================= */
CREATE PROCEDURE [dbo].[uspOwnershipSet]
(
	@ObjectID INT
	, @ObjectTypeID INT
	, @OwnerList VARCHAR(MAX) = NULL
	--, @OwnerID INT
	--, @IsGroup BIT = 0
	--, @Percent FLOAT = NULL
	, @CreatedBy INT = NULL
)
AS
BEGIN 
	--Declare Temp Table data
DECLARE @JsonTable TABLE (UserID INT, ObjectTypeID INT, ObjectID INT, IsGroup BIT, [Percentage] FLOAT, CreatedBy INT)
INSERT INTO @JsonTable
	SELECT result.UserId, @ObjectTypeID, @ObjectID, 0, result.[Percentage], @CreatedBy
	FROM OPENJSON(@OwnerList, 'lax $') 
			WITH (userId INT, [percentage] FLOAT) AS result

	--Delete preexisitng ownership data
	DELETE mapOwnership
	WHERE ObjectID = @ObjectID
	AND @ObjectTypeID = @ObjectTypeID
	--AND OwnerID IN (
	--			SELECT UserID
	--			FROM @JsonTable
	--)
	--insert new data
	INSERT INTO mapOwnership (OwnerID, ObjectTypeID, ObjectID, IsGroup, [Percent], CreatedBy)
	SELECT UserID
	, ObjectTypeID
	, ObjectID
	, IsGroup
	, [Percentage]
	, CreatedBy
	FROM @JsonTable

	--Output Select
	IF (@@rowcount>0)
	BEGIN
		SELECT OwnerID
		, U.FirstName OwnerFirstName
		, U.LastName OwnerLastName
		, O.[Percent]
		FROM mapOwnership O
		INNER JOIN Users U on U.UserID = O.OwnerID
		WHERE ObjectID = @ObjectID
		AND @ObjectTypeID = @ObjectTypeID
	END

	ELSE
		RETURN -1
END