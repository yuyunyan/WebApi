CREATE FUNCTION [dbo].[fnGetObjectOwners]
(
	@ObjectID INT,
	@ObjectTypeID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
    RETURN (SELECT FirstName + ' ' + LastName + ', '
		  FROM Users U
		  INNER JOIN mapOwnership O on O.OwnerID = U.UserID
		  WHERE O.ObjectTypeID = @ObjectTypeID
		  AND O.ObjectID = @ObjectID
		  FOR XML PATH('') )
END