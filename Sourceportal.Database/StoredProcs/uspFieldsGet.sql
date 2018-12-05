/* =============================================
   Author:		Aaron Rodecker
   Create date: 2017.05.11
   Description:	Retrieves all field records from lkpFields

   Revision History:
   ============================================= */
CREATE PROCEDURE [dbo].[uspFieldsGet]
(
	@ObjectTypeID INT = 0,
	@IsDeleted BIT = 0
)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT
		ObjectTypeID,
		FieldName,
		FieldID,
		FieldType
	FROM lkpFields
	WHERE ObjectTypeID = @ObjectTypeID
	AND IsDeleted = @IsDeleted
END