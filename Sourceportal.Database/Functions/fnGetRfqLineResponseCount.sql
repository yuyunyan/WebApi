CREATE   FUNCTION [dbo].[fnGetRfqLineResponseCount]
(
	@RfqLineID INT
)
RETURNS INT
AS
BEGIN
    RETURN (
		SELECT SUM(S.Qty)
		FROM
		VendorRFQLines RFQL 
		INNER JOIN mapSourcesJoin SJ ON RFQL.VRFQLineID = SJ.ObjectID AND SJ.objecttypeid = 28 AND SJ.IsDeleted = 0
		INNER JOIN Sources S ON SJ.SourceID = S.SourceID AND S.IsDeleted = 0
		WHERE RFQL.VRFQLineID = @RfqLineID
		)
END