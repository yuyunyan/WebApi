/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.04.27
   Description:	Gets owner of record by objectid from mapOwnership
   Usage: EXEC uspOwnershipGet @ObjectID = 4, @ObjectTypeID = 2

   Revision History:
		CT	2017.10.13	Added ExternalID
   Return Codes:
   ============================================= */
CREATE PROCEDURE [dbo].[uspOwnershipGet]
(
	@ObjectID INT
	, @ObjectTypeID INT
)
AS
BEGIN 
	SET NOCOUNT ON;
		SELECT OwnerID
		, U.FirstName OwnerFirstName
		, U.LastName OwnerLastName
		, U.EmailAddress OwnerEmailAddress
		, U.ExternalID
		, ObjectTypeID
		, ObjectID
		, IsGroup
		, [Percent]
		, O.Created
		, O.CreatedBy
		, dbo.fnGetUserImage(O.OwnerID) OwnerImageURL
		FROM mapOwnership O
		INNER JOIN Users U on U.UserID = O.OwnerID
		WHERE ObjectID = @ObjectID
		AND ObjectTypeID = @ObjectTypeID
END