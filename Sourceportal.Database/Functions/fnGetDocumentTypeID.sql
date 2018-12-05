/* =============================================
	Author:			Aaron Rodecker
	Create date:	2017.11.28
	Description:	Returns the DocumentTypeID for a given FileMime Type
   =============================================*/
CREATE FUNCTION [dbo].[fnGetDocumentTypeID]
(
	@FileMimeType VARCHAR(256)
)
RETURNS INT
AS
BEGIN
	RETURN ( CASE
			WHEN @FileMimeType LIKE 'image/%' THEN 1		--Image
			WHEN @FileMimeType LIKE '%spreadsheet%' OR @FileMimeType LIKE '%excel%' THEN 2	--Spreadsheet
			WHEN @FileMimeType IN ('application/octet-stream', 'text/plain') THEN 4 --Text
			ELSE 8 END	--Other
	)
END