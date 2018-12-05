CREATE FUNCTION [dbo].[fnGetRfqOwners]
(
	@RfqID INT
)
RETURNS VARCHAR(512)
AS
BEGIN
    RETURN (SELECT FirstName + ' ' + LastName + ', '
		  FROM Users U
		  INNER JOIN mapOwnership O on O.OwnerID = U.UserID
		  WHERE O.ObjectTypeID = dbo.fnRfqObjectTypeID()
		  AND O.ObjectID = @RfqID
		  FOR XML PATH('') )
END