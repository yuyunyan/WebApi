/*  =============================================
     Author:		Berry, Zhong
     Create date:   2017.08.29
     Description:	Return the AnswerTypeId and typeName dictionary 
    =============================================*/
CREATE PROCEDURE [dbo].[uspQCAnswerTypesGet]

AS
BEGIN
	SET NOCOUNT ON;

	SELECT AnswerTypeID, TypeName
	FROM lkpQCAnswerTypes
	WHERE IsDeleted = 0
	ORDER BY AnswerTypeID
END
