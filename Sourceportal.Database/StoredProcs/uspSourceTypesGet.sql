/* =============================================
   Author:		Nathan Ayers
   Create date: 2017.08.01
   Description:	Returns IDs and Names for Source Types
   Usage:	EXEC uspSourceTypesGet			
   Return Codes:
   Revision History:			
   ============================================= */

CREATE PROCEDURE [dbo].[uspSourceTypesGet]

AS
BEGIN
	SET NOCOUNT ON;
	SELECT TypeName, SourceTypeID
	FROM lkpSourceTypes
	WHERE IsDeleted = 0
	ORDER BY TypeRank
END
