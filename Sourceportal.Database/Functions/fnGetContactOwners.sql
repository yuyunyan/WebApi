CREATE FUNCTION [dbo].[fnGetContactOwners]
(
	@ContactID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
    RETURN (SELECT FirstName + ' ' + LastName + ', '
		  FROM Users U
		  INNER JOIN mapOwnership O on O.OwnerID = U.UserID
		  WHERE O.ObjectTypeID = dbo.fnContactObjectTypeID()
		  AND O.ObjectID = @ContactID
		  FOR XML PATH('') )
END